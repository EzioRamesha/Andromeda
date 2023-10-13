using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using DataAccess.Entities.Identity;
using Services;
using Services.Identity;
using Services.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApp.Commands
{
    public class GenerateMfrs17Reporting : Command
    {
        public Mfrs17ReportingBo Mfrs17ReportingBo { get; set; }

        public List<Column> Cols { get; set; } = new List<Column> { };

        public string Delimiter = "|";

        public string Quarter { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public TextFile TextFile { get; set; }

        public ModuleBo ModuleBo { get; set; }

        public GenerateMfrs17Reporting()
        {
            Title = "GenerateMfrs17Reporting";
            Description = "To generate MFRS 17 Reporting";

            GetMappings();
        }

        public override bool Validate()
        {
            try
            {
                // nothing
            }
            catch (Exception e)
            {
                PrintMessage(e.Message);
                return false;
            }
            return base.Validate();
        }

        public override void Initial()
        {
            ModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.Mfrs17Reporting.ToString());
        }

        public override void Run()
        {
            try
            {
                if (Mfrs17ReportingService.CountByStatus(Mfrs17ReportingBo.StatusPendingGenerate) == 0)
                {
                    PrintMessage(MessageBag.NoMfrs17reportingPendingGenerate);
                    return;
                }

                PrintStarting();

                while (LoadMfrs17ReportingBo() != null)
                {
                    try
                    {
                        if (GetProcessCount("Process") > 0)
                            PrintProcessCount();
                        SetProcessCount("Process");

                        UpdateStatus(Mfrs17ReportingBo.StatusGenerating, "Generating MFRS17 Reporting");

                        Process();

                        UpdateStatus(GetStatus(), "Successfully Generating MFRS17 Reporting");
                    }
                    catch (Exception e)
                    {
                        UpdateStatus(Mfrs17ReportingBo.StatusFailedOnGenerate, "Failed to Generate MFRS17 Reporting");
                        if (e is RetryLimitExceededException dex)
                        {
                            PrintMessage(dex.ToString(), log: true);
                        }
                        else
                        {
                            PrintMessage(e.ToString());
                        }
                    }
                }

                if (GetProcessCount("Process") > 0)
                    PrintProcessCount();

                PrintEnding();
            }
            catch (Exception e)
            {
                PrintMessage("Error running Generate MFRS17 Reporting");
                PrintMessage(e.ToString());
            }
        }

        public void Process()
        {
            ProcessSingle();
            ProcessMultiple(Mfrs17ReportingBo.IsResume.GetValueOrDefault());
        }

        public void ProcessSingle()
        {
            if (Mfrs17ReportingBo.GenerateType != Mfrs17ReportingBo.GenerateTypeSingle)
            {
                return;
            }

            List<string> files = new List<string>();

            string path = Util.GetMfrs17ReportingPath(Quarter + "/Single");
            Util.MakeDir(path, false);
            // Delete all previous files
            Util.DeleteFiles(path, "*");

            FileName = string.Format("{0} MFRS17 Reporting Data-{1}.txt", Quarter, "tmp");

            FilePath = string.Format("{0}/{1}", path, FileName);
            Util.MakeDir(FilePath);

            files.Add(FileName);

            UpdateGeneratePercentage(0.0);

            // Header
            ExportWriteLine(GetHeader());

            int total = Mfrs17ReportingDetailService.CountByMfrs17ReportingId(Mfrs17ReportingBo.Id);

            double count = 0;
            double totalDetail = total;

            int fileCount = 2;
            int take = 50;
            for (int skip = 0; skip < (total + take); skip += take)
            {
                if (skip >= total)
                    break;

                foreach (var mfrs17ReportingDetailBo in Mfrs17ReportingDetailService.GetByMfrs17ReportingId(Mfrs17ReportingBo.Id, skip, take))
                {
                    DateTime? dt = mfrs17ReportingDetailBo.LatestDataEndDate;
                    if (count > 0)
                    {
                        double generatePercentage = ((count / totalDetail) * 100);
                        UpdateGeneratePercentage(generatePercentage);
                    }

                    int totalRiData = Mfrs17ReportingDetailRiDataService.CountByMfrs17ReportingDetailId(mfrs17ReportingDetailBo.Id);
                    int takeRiData = Util.GetConfigInteger("Mfrs17QueryMaxRow", 50);

                    int page = 0;
                    for (int skipRiData = 0; skipRiData < (totalRiData + takeRiData); skipRiData += takeRiData)
                    {
                        if (skipRiData >= totalRiData)
                            break;

                        var lineout = new List<string>();

                        List<int> ids = Mfrs17ReportingDetailRiDataService.GetIdsByMfrs17ReportingDetailIdPage(mfrs17ReportingDetailBo.Id, skipRiData, takeRiData, page);
                        foreach (var riDataWarehouseHistoryBo in RiDataWarehouseHistoryService.GetByIds(ids, Mfrs17ReportingBo.CutOffId))
                            lineout.Add(GetDataRow(riDataWarehouseHistoryBo, dt));

                        if (IsMaxFileSize())
                        {
                            FileName = string.Format("{0} MFRS17 Reporting Data_{1}-{2}.txt", Quarter, fileCount, "tmp");
                            FilePath = string.Format("{0}/{1}", path, FileName);
                            Util.MakeDir(FilePath);
                            ExportWriteLine(GetHeader());
                            files.Add(FileName);
                            fileCount++;
                        }

                        ExportWrite(string.Join(Environment.NewLine, lineout) + "\n");
                        lineout.Clear();

                        page++;
                    }
                    count++;
                }
            }

            RenameCompleteMfrs17ReportingFile(path, files.ToArray());
        }

        public void ProcessMultiple(bool resume = false)
        {
            if (Mfrs17ReportingBo.GenerateType != Mfrs17ReportingBo.GenerateTypeMultiple)
            {
                return;
            }

            string path = Util.GetMfrs17ReportingPath(Quarter + "/Multiple");
            Util.MakeDir(path, false);
            // Dont delete all files 
            //Util.DeleteFiles(path, "*");

            if (!resume)
                UpdateGeneratePercentage(0.0);

            List<string> mfrs17TreatyCodes = Mfrs17ReportingDetailService.GetDistinctMfrs17TreatyCodes(Mfrs17ReportingBo.Id, Mfrs17ReportingBo.GenerateModifiedOnly, resume);
            Dictionary<int, DateTime> latestEndDate = Mfrs17ReportingDetailService.GetLatestEndDateByMfrs17ReportingId(Mfrs17ReportingBo.Id);

            double count = 0;
            double totalMfrs17TreatyCode = mfrs17TreatyCodes.Count();
            if (resume)
            {
                var allMfrs17TreatyCodes = Mfrs17ReportingDetailService.GetDistinctMfrs17TreatyCodes(Mfrs17ReportingBo.Id, Mfrs17ReportingBo.GenerateModifiedOnly);
                count = (allMfrs17TreatyCodes.Count() - mfrs17TreatyCodes.Count());
                totalMfrs17TreatyCode = allMfrs17TreatyCodes.Count();
            }

            foreach (string mfrs17TreatyCode in mfrs17TreatyCodes)
            {
                List<string> files = new List<string>();

                FileName = string.Format("{0}-{1}.txt", mfrs17TreatyCode, "tmp");
                FilePath = string.Format("{0}/{1}", path, FileName);
                Util.MakeDir(FilePath);

                string extendedFileName = string.Format("{0}_?.txt", mfrs17TreatyCode);

                bool isDeleted = Mfrs17ReportingDetailService.IsExistDeletedByMfrs17TreatyCodes(Mfrs17ReportingBo.Id, mfrs17TreatyCode);
                bool isNonDeleted = Mfrs17ReportingDetailService.IsExistNonDeletedByMfrs17TreatyCodes(Mfrs17ReportingBo.Id, mfrs17TreatyCode);
                var fileWithoutExtension = Path.GetFileNameWithoutExtension(FilePath);
                var renamedExistingFile = fileWithoutExtension.Remove(fileWithoutExtension.Length - 4);

                if (isDeleted && !isNonDeleted)
                {
                    // Delete file by Treaty Code
                    Util.DeleteFiles(path, extendedFileName);
                    Util.DeleteFiles(path, FileName);
                    Util.DeleteFiles(path, $"{renamedExistingFile}.txt");

                    IList<Mfrs17ReportingDetailBo> bos = Mfrs17ReportingDetailService.GetModifieBydMfrs17TreatyCode(Mfrs17ReportingBo.Id, mfrs17TreatyCode, Mfrs17ReportingBo.GenerateModifiedOnly);
                    foreach (Mfrs17ReportingDetailBo bo in bos)
                    {
                        TrailObject trail = new TrailObject();
                        Mfrs17ReportingDetailBo mfrs17ReportingDetailBo = bo;
                        if (bo.IsModified)
                            mfrs17ReportingDetailBo.IsModified = false;
                        mfrs17ReportingDetailBo.UpdatedById = User.DefaultSuperUserId;
                        mfrs17ReportingDetailBo.GenerateStatus = Mfrs17ReportingDetailBo.GenerateStatusSuccess;
                        mfrs17ReportingDetailBo.UpdatedById = User.DefaultSuperUserId;

                        Result result = Mfrs17ReportingDetailService.Update(ref mfrs17ReportingDetailBo, ref trail);
                        UserTrailBo userTrailBo = new UserTrailBo(
                            Mfrs17ReportingBo.Id,
                            "Update Mfrs17 Reporting Detail",
                            result,
                            trail,
                            User.DefaultSuperUserId
                        );
                        UserTrailService.Create(ref userTrailBo);
                    }

                    count++;
                    continue;
                }

                // Delete file by Treaty Code
                Util.DeleteFiles(path, extendedFileName);
                Util.DeleteFiles(path, FileName);
                Util.DeleteFiles(path, $"{renamedExistingFile}.txt");

                files.Add(FileName);

                int fileCount = 2;
                if (count > 0)
                {
                    double generatePercentage = ((count / totalMfrs17TreatyCode) * 100);
                    UpdateGeneratePercentage(generatePercentage);
                }

                // Header
                ExportWriteLine(GetHeader());

                int total = Mfrs17ReportingDetailRiDataService.CountByMfrs17ReportingIdMfrs17TreatyCode(Mfrs17ReportingBo.Id, mfrs17TreatyCode);
                int take = Util.GetConfigInteger("Mfrs17QueryMaxRow", 50);

                int page = 0;
                for (int skip = 0; skip < (total + take); skip += take)
                {
                    if (skip >= total)
                        break;

                    var lineout = new List<string>();

                    IList<Mfrs17ReportingDetailRiDataBo> mfrs17ReportingDetailRiDataBos = Mfrs17ReportingDetailRiDataService.GetSimplifiedByMfrs17ReportingIdMfrs17TreatyCodePage(Mfrs17ReportingBo.Id, mfrs17TreatyCode, skip, take, page);
                    List<int> ids = mfrs17ReportingDetailRiDataBos.Select(q => q.RiDataWarehouseId).ToList();
                    Dictionary<int, int> dic = mfrs17ReportingDetailRiDataBos.ToDictionary(q => q.RiDataWarehouseId, q => q.Mfrs17ReportingDetailId);

                    foreach (var riDataWarehouseHistoryBo in RiDataWarehouseHistoryService.GetByIds(ids, Mfrs17ReportingBo.CutOffId))
                        lineout.Add(GetDataRow(riDataWarehouseHistoryBo, latestEndDate[dic[riDataWarehouseHistoryBo.Id]]));

                    if (IsMaxFileSize())
                    {
                        FileName = string.Format("{0}_{1}-{2}.txt", mfrs17TreatyCode, fileCount, "tmp");
                        FilePath = string.Format("{0}/{1}", path, FileName);
                        Util.MakeDir(FilePath);
                        ExportWriteLine(GetHeader());
                        files.Add(FileName);
                        fileCount++;
                    }

                    ExportWrite(string.Join(Environment.NewLine, lineout) + "\n");
                    lineout.Clear();

                    page++;
                }

                RenameCompleteMfrs17ReportingFile(path, files.ToArray());

                IList<Mfrs17ReportingDetailBo> mfrs17ReportingDetailBos = Mfrs17ReportingDetailService.GetModifieBydMfrs17TreatyCode(Mfrs17ReportingBo.Id, mfrs17TreatyCode, Mfrs17ReportingBo.GenerateModifiedOnly);
                foreach (Mfrs17ReportingDetailBo bo in mfrs17ReportingDetailBos)
                {
                    TrailObject trail = new TrailObject();
                    Mfrs17ReportingDetailBo mfrs17ReportingDetailBo = bo;
                    if (bo.IsModified)
                        mfrs17ReportingDetailBo.IsModified = false;
                    mfrs17ReportingDetailBo.UpdatedById = User.DefaultSuperUserId;
                    mfrs17ReportingDetailBo.GenerateStatus = Mfrs17ReportingDetailBo.GenerateStatusSuccess;
                    mfrs17ReportingDetailBo.UpdatedById = User.DefaultSuperUserId;

                    Result result = Mfrs17ReportingDetailService.Update(ref mfrs17ReportingDetailBo, ref trail);
                    UserTrailBo userTrailBo = new UserTrailBo(
                        Mfrs17ReportingBo.Id,
                        "Update Mfrs17 Reporting Detail",
                        result,
                        trail,
                        User.DefaultSuperUserId
                    );
                    UserTrailService.Create(ref userTrailBo);
                }

                count++;
            }
        }

        public Mfrs17ReportingBo LoadMfrs17ReportingBo()
        {
            Mfrs17ReportingBo = Mfrs17ReportingService.FindByStatus(Mfrs17ReportingBo.StatusPendingGenerate);
            if (Mfrs17ReportingBo != null)
            {
                Quarter = Mfrs17ReportingBo.Quarter.Replace(" ", string.Empty);
            }
            return Mfrs17ReportingBo;
        }

        public void ExportWriteLine(object line)
        {
            using (var textFile = new TextFile(FilePath, true, true, Encoding.Default))
            {
                textFile.WriteLine(line);
            }
        }

        public void ExportWrite(object line)
        {
            //using (StreamWriter outFile = new StreamWriter(FilePath, true, new System.Text.UTF8Encoding(false, true), 65536))
            using (StreamWriter outFile = new StreamWriter(FilePath, true))
            {
                if (line == null) outFile.Write("");
                else outFile.Write(line);
            }
        }

        public bool IsMaxFileSize()
        {
            FileInfo fi = new FileInfo(FilePath);
            if (fi != null && fi.Length > Util.GetConfigInteger("Mfrs17MaxFileSize", 1717986038))
            {
                return true;
            }
            return false;
        }

        public string GetHeader()
        {
            return string.Join(Delimiter, Cols.Select(m => m.Header).ToArray());
        }

        public void GetMappings()
        {
            var mr = new Mfrs17ReportingBo();
            int index = 1;
            Cols = new List<Column> { };
            foreach (int type in mr.Mfrs17ReportingColumns)
            {
                var header = StandardOutputBo.GetCodeByType(type);
                if (string.IsNullOrEmpty(header))
                {
                    header = Mfrs17ReportingBo.GetMfrs17ReportingColumnName(type);
                }

                var length = Mfrs17ReportingBo.GetMfrs17ReportingColumnRevisedLength(type);
                var mapping = new Column()
                {
                    Header = header,
                    Type = type,
                    ColIndex = index,
                    Length = length,
                };

                if (mapping.Header.Length > length)
                {
                    mapping.Header = mapping.Header.Substring(0, length);
                }
                else if (mapping.Header.Length < length)
                {
                    mapping.Header = mapping.Header.PadRight(length, ' ');
                }

                Cols.Add(mapping);
                index++;
            }
        }

        public string GetDataRow(RiDataWarehouseHistoryBo riDataWarehouseHistoryBo, DateTime? dt)
        {
            List<string> cols = new List<string> { };
            foreach (var col in Cols)
            {
                string riDataProp = StandardOutputBo.GetPropertyNameByType(col.Type);

                object value = null;
                if (!string.IsNullOrEmpty(riDataProp))
                {
                    if (StandardOutputBo.GetAmountTypes().Contains(col.Type))
                    {
                        value = Util.DoubleToString(riDataWarehouseHistoryBo.GetPropertyValue(StandardOutputBo.GetPropertyNameByType(col.Type)));
                    }
                    else
                    {
                        value = riDataWarehouseHistoryBo.GetPropertyValue(StandardOutputBo.GetPropertyNameByType(col.Type));
                    }
                }
                else
                {
                    if (col.Type == Mfrs17ReportingBo.TypeLatestDataEndDate)
                    {
                        value = dt.HasValue ? dt : null;
                    }
                    //value = Mfrs17ReportingDetailRiDataBo.Mfrs17ReportingDetailBo.GetPropertyValue(Mfrs17ReportingBo.GetPropertyNameByType(col.Type));
                }

                string v = "";
                if (value != null)
                {
                    if (value is DateTime d)
                        v = d.ToString(Util.GetDateFormat());
                    else
                        v = Regex.Replace(value.ToString(), @"(\r\n?|\r?\n)+", " ");     // It removes \r\n , \n and \r
                }

                if (v.Length > col.Length)
                {
                    v = v.Substring(0, col.Length);
                }

                switch (Mfrs17ReportingBo.GetMfrs17ReportingColumnAlign(col.Type))
                {
                    case "L":
                        cols.Add(v.PadLeft(col.Length, ' '));
                        break;
                    default:
                        cols.Add(v.PadRight(col.Length, ' '));
                        break;
                }
            }

            return string.Join(Delimiter, cols.ToArray());
        }

        public int GetStatus()
        {
            StatusHistoryBo bo = StatusHistoryService.FindByModuleIdObjectIdStatus(ModuleBo.Id, Mfrs17ReportingBo.Id, Mfrs17ReportingBo.StatusFinalised);
            return bo != null ? Mfrs17ReportingBo.StatusFinalised : Mfrs17ReportingBo.StatusSuccess;
        }

        public void UpdateStatus(int status, string des)
        {
            TrailObject trail = new TrailObject();
            StatusHistoryBo statusBo = new StatusHistoryBo
            {
                ModuleId = ModuleBo.Id,
                ObjectId = Mfrs17ReportingBo.Id,
                Status = status,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            StatusHistoryService.Create(ref statusBo, ref trail);

            var reporting = Mfrs17ReportingBo;
            Mfrs17ReportingBo.Status = status;

            if (status == Mfrs17ReportingBo.StatusSuccess || status == Mfrs17ReportingBo.StatusFinalised)
            {
                Mfrs17ReportingBo.GenerateType = null;
                Mfrs17ReportingBo.GenerateModifiedOnly = null;
                Mfrs17ReportingBo.GeneratePercentage = null;
                Mfrs17ReportingBo.IsResume = false;
            }

            if (status == Mfrs17ReportingBo.StatusFailedOnGenerate && Mfrs17ReportingBo.GenerateType == Mfrs17ReportingBo.GenerateTypeMultiple)
            {
                Mfrs17ReportingBo.IsResume = true;
            }

            Result result = Mfrs17ReportingService.Update(ref reporting, ref trail);
            UserTrailBo userTrailBo = new UserTrailBo(
                Mfrs17ReportingBo.Id,
                des,
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);
        }

        public void UpdateGeneratePercentage(double? percent)
        {
            var reporting = Mfrs17ReportingBo;

            if (percent.HasValue)
            {
                Mfrs17ReportingBo.GeneratePercentage = Util.RoundValue(percent.Value, 2);
            }
            else
            {
                Mfrs17ReportingBo.GeneratePercentage = null;
            }

            Mfrs17ReportingService.Update(ref reporting);
        }

        private static void RenameCompleteMfrs17ReportingFile(string path, string[] fileNames)
        {
            if (fileNames.Count() <= 0)
                return;

            for (int i = 0; i < fileNames.Length; i++)
            {
                string file = Directory.GetFiles(path, fileNames[i]).FirstOrDefault();
                if (file == null)
                    return;

                FileInfo fi = new FileInfo(file);
                var fileWithoutExtension = Path.GetFileNameWithoutExtension(fi.DirectoryName + "\\" + fileNames[i]);
                var fileName = fileWithoutExtension.Remove(fileWithoutExtension.Length - 4);
                fi.RenameTxtFile(fi.DirectoryName + "\\" + fileName);
            }
        }
    }
}
