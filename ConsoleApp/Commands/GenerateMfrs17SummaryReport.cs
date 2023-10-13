using BusinessObject;
using Services;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands
{
    public class GenerateMfrs17SummaryReport : Command
    {
        public List<Column> Cols { get; set; }

        public int Mfrs17ReportingId { get; set; }

        public string Quarter { get; set; }

        public bool IsDefault { get; set; }

        public TextFile TextFile { get; set; }

        public string FilePath { get; set; }

        public const int TypeCedant = 1;
        public const int TypeTreatyCode = 2;
        public const int TypePremiumFrequencyCode = 3;
        public const int TypeCedingPlanCode = 4;
        public const int TypeRiskQuarter = 5;
        public const int TypeLatestDataStartDate = 6;
        public const int TypeLatestDataEndDate = 7;
        public const int TypeRecord = 8;
        public const int TypeMfrs17TreatyCode = 9;
        public const int TypeStatus = 10;

        public GenerateMfrs17SummaryReport()
        {
            Title = "GenerateMfrs17SummaryReport";
            Description = "To generate MFRS 17 Summary Report";
        }

        public override void Run()
        {
            Process();
        }

        public void Process()
        {
            if (IsDefault)
            {
                GetColumns();
            }
            else
            {
                GetMfrs17TreatyCodeColumns();
            }
            string filename = IsDefault ? string.Format("MFRS17SummaryReport{0}_Cedant", Quarter).AppendDateTimeFileName(".csv") : string.Format("MFRS17SummaryReport{0}_MFRS17TreatyCode", Quarter).AppendDateTimeFileName(".csv");
            string path = Util.GetTemporaryPath();

            FilePath = string.Format("{0}/{1}", path, filename);
            Util.MakeDir(FilePath);

            // Delete all previous files
            Util.DeleteFiles(path, @"MFRS17SummaryReport*");

            // Header
            ExportWriteLine(string.Join(",", Cols.Select(p => p.Header)));

            int total = Mfrs17ReportingDetailService.CountDataByMfrs17ReportingId(Mfrs17ReportingId, IsDefault);
            int take = 50;
            for (int skip = 0; skip < (total + take); skip += take)
            {
                if (skip >= total)
                    break;

                foreach (var mfrs17ReportingDetailBo in Mfrs17ReportingDetailService.GetDataByMfrs17ReportingId(Mfrs17ReportingId, skip, take, IsDefault))
                {
                    ProcessData(mfrs17ReportingDetailBo);
                }
            }
        }

        public void ProcessData(Mfrs17ReportingDetailBo mfrs17ReportingDetailBo)
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
                    case TypeCedant:
                        v = mfrs17ReportingDetailBo.CedantBo?.Name;
                        break;
                    case TypePremiumFrequencyCode:
                        v = mfrs17ReportingDetailBo.PremiumFrequencyCodePickListDetailBo?.Code;
                        break;
                    case TypeStatus:
                        int status = mfrs17ReportingDetailBo.Status.Value;
                        v = Mfrs17ReportingDetailBo.GetStatusName(status);
                        break;
                    default:
                        v = mfrs17ReportingDetailBo.GetPropertyValue(col.Property);
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
                    Header = "Cedant",
                    ColIndex = TypeCedant,
                    Property = "Cedant",
                },
                new Column
                {
                    Header = "Treaty Code",
                    ColIndex = TypeTreatyCode,
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "Payment Mode",
                    ColIndex = TypePremiumFrequencyCode,
                    Property = "PremiumFrequencyCode",
                },
                new Column
                {
                    Header = "Ceding Plan Code",
                    ColIndex = TypeCedingPlanCode,
                    Property = "CedingPlanCode",
                },
                new Column
                {
                    Header = "Risk Quarter",
                    ColIndex = TypeRiskQuarter,
                    Property = "RiskQuarter",
                },
                new Column
                {
                    Header = "Data Start Date",
                    ColIndex = TypeLatestDataStartDate,
                    Property = "LatestDataStartDate",
                },
                new Column
                {
                    Header = "Data End Date",
                    ColIndex = TypeLatestDataEndDate,
                    Property = "LatestDataEndDate",
                },
                new Column
                {
                    Header = "Record",
                    ColIndex = TypeRecord,
                    Property = "Record",
                },
                new Column
                {
                    Header = "Status",
                    ColIndex = TypeStatus,
                    Property = "Status",
                },
            };

            return Cols;
        }

        public List<Column> GetMfrs17TreatyCodeColumns()
        {
            Cols = new List<Column>
            {
                new Column
                {
                    Header = "MFRS17 Treaty Code",
                    ColIndex = TypeMfrs17TreatyCode,
                    Property = "Mfrs17TreatyCode",
                },
                new Column
                {
                    Header = "Payment Mode",
                    ColIndex = TypePremiumFrequencyCode,
                    Property = "PremiumFrequencyCode",
                },
                new Column
                {
                    Header = "Risk Quarter",
                    ColIndex = TypeRiskQuarter,
                    Property = "RiskQuarter",
                },
                new Column
                {
                    Header = "Data Start Date",
                    ColIndex = TypeLatestDataStartDate,
                    Property = "LatestDataStartDate",
                },
                new Column
                {
                    Header = "Data End Date",
                    ColIndex = TypeLatestDataEndDate,
                    Property = "LatestDataEndDate",
                },
                new Column
                {
                    Header = "Record",
                    ColIndex = TypeRecord,
                    Property = "Record",
                },
            };

            return Cols;
        }
    }
}
