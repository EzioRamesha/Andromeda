using BusinessObject;
using Services;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands
{
    public class GenerateTargetPlanningReport : Command
    {
        public List<string> ColumnHeaders { get; set; }

        public List<string> CompleteColumnHeaders { get; set; }

        public List<TargetPlanningReportOutput> Output { get; set; }

        public TextFile TextFile { get; set; }

        public string FilePath { get; set; }

        public GenerateTargetPlanningReport()
        {
            Title = "GenerateTargetPlanningReport";
            Description = "To generate Target Planning Report";
        }

        public override void Run()
        {
            string filename = "";

            Process(ref filename);
        }

        public void Process(ref string filename)
        {
            List<string> completeColumnHeaders = new List<string>();

            completeColumnHeaders.Add("Ceding Company");
            completeColumnHeaders.Add("Treaty ID");
            completeColumnHeaders.Add("Person In-Charge");
            completeColumnHeaders.Add("Treaty Code");
            completeColumnHeaders.AddRange(ColumnHeaders);

            CompleteColumnHeaders = completeColumnHeaders;

            filename = "TargetPlanningReport.csv";
            string path = Util.GetTemporaryPath();

            FilePath = string.Format("{0}/{1}", path, filename);
            Util.MakeDir(FilePath);

            // Delete all previous files
            Util.DeleteFiles(path, @"TargetPlanningReport*");

            // Header
            //ExportWriteLine(string.Join(",", Cols.Select(p => p.Header)));
            ExportWriteLine(string.Join(",", CompleteColumnHeaders.ToArray()));

            foreach (var output in Output)
            {
                List<string> fullOutputStr = new List<string>();

                fullOutputStr.Add(output.CedingCompany);
                fullOutputStr.Add(output.TreatyId);
                fullOutputStr.Add(output.PersonInCharge);
                fullOutputStr.Add(output.TreatyCode);
                fullOutputStr.AddRange(output.OutputData);

                ExportWriteLine(string.Join(",", fullOutputStr.ToArray()));
            }
        }

        public void ExportWriteLine(object line)
        {
            using (var textFile = new TextFile(FilePath, true, true))
            {
                textFile.WriteLine(line);
            }
        }
    }
}
