using BusinessObject.Sanctions;
using DataAccess.Entities.Identity;
using Services.Sanctions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.Sanction
{
    public class ProcessSanctionName
    {
        public SanctionBo SanctionBo { get; set; }

        public Dictionary<int, string> SanctionNames { get; set; }

        public IList<SanctionFormatNameBo> ExistingFormatNames { get; set; }

        public IList<SanctionFormatNameBo> FormatNames { get; set; }

        public int SanctionNameId { get; set; }

        public int TypeIndex { get; set; }

        public ProcessSanctionName(SanctionBo sanctionBo)
        {
            SanctionBo = sanctionBo;
        }

        public void Process()
        {
            LoadData();

            foreach (var sanctionName in SanctionNames)
            {
                SanctionNameId = sanctionName.Key;
                TypeIndex = 0;

                Regex symbol = new Regex(@"[^a-zA-Z0-9 ]+");
                string name = symbol.Replace(sanctionName.Value, string.Empty);

                if (string.IsNullOrEmpty(name))
                    continue;

                AddData(SanctionFormatNameBo.TypeSymbolRemoval, name.Replace(" ", string.Empty));

                string[] splitNames = name.Split(' ');
                List<string> formattedName = SanctionExclusionService.FormatName(splitNames);
                foreach (string splitName in formattedName)
                {
                    AddData(SanctionFormatNameBo.TypeGroupKeyword, splitName);
                }

                ReplaceKeyword(formattedName, 0);
            }
            FormatNames = FormatNames.Distinct().ToList();

            RemoveData();
            SaveData();
        }

        public void ReplaceKeyword(List<string> nameList, int index)
        {
            if (nameList.Count() <= index)
                return;

            int nextIndex = index + 1;
            string partial = nameList[index];
            IList<string> keywords = SanctionKeywordDetailService.GetKeywords(partial);
            if (!keywords.IsNullOrEmpty())
            {
                foreach (string keyword in keywords)
                {
                    nameList[index] = keyword;
                    AddData(SanctionFormatNameBo.TypeKeywordReplacement, string.Join("", nameList));
                    AddType4Data(nameList);

                    ReplaceKeyword(nameList, nextIndex);
                }
            }
            else
            {

                ReplaceKeyword(nameList, nextIndex);
            }
        }

        public void AddData(int type, string name, int? typeIndex = null)
        {
            bool isExists = FormatNames.Where(q => q.SanctionNameId == SanctionNameId)
                .Where(q => q.Type == type)
                .Where(q => q.TypeIndex == typeIndex)
                .Where(q => q.Name == name)
                .Any();

            if (isExists)
                return;

            FormatNames.Add(
                new SanctionFormatNameBo()
                {
                    SanctionId = SanctionBo.Id,
                    SanctionNameId = SanctionNameId,
                    Type = type,
                    TypeIndex = typeIndex,
                    Name = name,
                    CreatedById = User.DefaultSuperUserId
                }
            );
        }

        public void AddType4Data(List<string> nameList)
        {
            nameList = nameList.Distinct().ToList();
            bool isExists = FormatNames.Where(q => q.SanctionNameId == SanctionNameId)
                .Where(q => q.Type == SanctionFormatNameBo.TypeGroupKeywordReplacement)
                .GroupBy(q => q.TypeIndex)
                .Where(g => g.Count() == nameList.Count())
                .Where(g => nameList.All(g.Select(q => q.Name).ToList().Contains))
                .Any();

            if (isExists)
                return;

            TypeIndex++;
            foreach (string name in nameList)
            {
                AddData(SanctionFormatNameBo.TypeGroupKeywordReplacement, name, TypeIndex);
            }
        }

        public void RemoveData()
        {
            foreach (var formatName in ExistingFormatNames)
            {
                if (FormatNames.Any(q => q.SanctionNameId == formatName.SanctionNameId && q.Type == formatName.Type && q.TypeIndex == formatName.TypeIndex && q.Name == formatName.Name))
                    continue;

                SanctionFormatNameService.DeleteBySanctionIdName(formatName.SanctionNameId, formatName.Type, formatName.TypeIndex, formatName.Name);
            }

            foreach (var formatNames in ExistingFormatNames.GroupBy(q => new { q.SanctionNameId, q.Type, q.TypeIndex, q.Name }).Where(g => g.Count() > 1).ToList())
            {
                SanctionFormatNameService.DeleteByIds(formatNames.Skip(1).Select(q => q.Id).ToList());
            }
        }

        public void SaveData()
        {
            foreach (var formatName in FormatNames)
            {
                if (string.IsNullOrEmpty(formatName.Name))
                    continue;

                if (ExistingFormatNames.Any(q => q.SanctionNameId == formatName.SanctionNameId && q.Type == formatName.Type && q.TypeIndex == formatName.TypeIndex && q.Name == formatName.Name))
                    continue;

                var bo = formatName;

                SanctionFormatNameService.Create(ref bo);
            }
        }

        public void LoadData()
        {
            SanctionNames = SanctionNameService.GetNameBySanctionId(SanctionBo.Id);
            ExistingFormatNames = SanctionFormatNameService.GetBySanctionId(SanctionBo.Id);
            FormatNames = new List<SanctionFormatNameBo>();
        }
    }
}
