using BusinessObject.TreatyPricing;
using Services.TreatyPricing;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands
{
    public class GenerateGroupMasterLetter: Command
    {
        public List<Column> Cols { get; set; }

        public TreatyPricingGroupReferralBo GroupReferralBo { get; set; }

        public int GroupMasterLetterId { get; set; }

        public TextFile TextFile { get; set; }

        public string FilePath { get; set; }

        public const int TypeInsuredGroupName = 1;
        public const int TypeRiGroupSlip = 2;
        public const int TypeCoverageStartDate = 3;
        public const int TypeCoverageEndDate = 4;

        public GenerateGroupMasterLetter()
        {
            Title = "GenerateGroupMasterLetterSummary";
            Description = "To generate Ri Group Slip Summary list under Group Master Letter";
        }

        public override void Run()
        {
            Process();
        }

        public void Process()
        {
            GetColumns();

            var GroupMasterLetterBo = TreatyPricingGroupMasterLetterService.Find(GroupMasterLetterId);

            string filename = string.Format("RiGroupSlip_{0}", GroupMasterLetterBo.Code).AppendDateTimeFileName(".csv");
            string path = Util.GetTemporaryPath();

            FilePath = string.Format("{0}/{1}", path, filename);
            Util.MakeDir(FilePath);

            // Delete all previous files
            Util.DeleteFiles(path, @"RiGroupSlip*");

            // Header
            ExportWriteLine(string.Join(",", Cols.Select(p => p.Header)));

            int total = TreatyPricingGroupMasterLetterGroupReferralService.CountByGroupMasterLetterId(GroupMasterLetterId);
            int take = 50;
            for (int skip = 0; skip < (total + take); skip += take)
            {
                if (skip >= total)
                    break;

                foreach (var bo in TreatyPricingGroupMasterLetterGroupReferralService.GetByGroupMasterLetterId(GroupMasterLetterId, skip, take))
                {
                    GroupReferralBo = TreatyPricingGroupReferralService.Find(bo.TreatyPricingGroupReferralId);
                    ProcessData(GroupReferralBo);
                }
            }
        }

        public void ProcessData(TreatyPricingGroupReferralBo GroupReferralBo)
        {
            List<string> values = new List<string> { };
            foreach (var col in Cols)
            {
                if (string.IsNullOrEmpty(col.Property))
                {
                    values.Add("");
                    continue;
                }

                string value = "";

                object v = null;

                switch (col.ColIndex)
                {
                    case TypeInsuredGroupName:
                        v = GroupReferralBo.InsuredGroupNameBo?.Name;
                        break;
                    default:
                        v = GroupReferralBo.GetPropertyValue(col.Property);
                        break;
                }

                if (v != null)
                {
                    if (v is DateTime d)
                    {
                        value = d.ToString(Util.GetDateFormat());
                    }
                    else
                    {
                        value = v.ToString();
                    }
                }

                values.Add(string.Format("\"{0}\"", value));
            }
            string line = string.Join(",", values.ToArray());
            ExportWriteLine(line);
        }

        public void ExportWriteLine(object line)
        {
            using (var textFile = new TextFile(FilePath, true, true))
            {
                textFile.WriteLine(line);
            }
        }

        public List<Column> GetColumns()
        {
            Cols = new List<Column>
            {
                new Column
                {
                    Header = "Insured Group Name",
                    ColIndex = TypeInsuredGroupName,
                    Property = "InsuredGroupNameId",
                },
                new Column
                {
                    Header = "Ri Group Slip ID",
                    ColIndex = TypeRiGroupSlip,
                    Property = "RiGroupSlipCode",
                },
                new Column
                {
                    Header = "Coverage Start Date",
                    ColIndex = TypeCoverageStartDate,
                    Property = "CoverageStartDate",
                },
                new Column
                {
                    Header = "Coverage End Date",
                    ColIndex = TypeCoverageEndDate,
                    Property = "CoverageEndDate",
                },
            };

            return Cols;
        }
    }
}
