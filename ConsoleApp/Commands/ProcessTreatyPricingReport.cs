using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.Identity;
using Newtonsoft.Json;
using Services.Identity;
using Services.TreatyPricing;
using Shared;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp.Commands
{
    public class ProcessTreatyPricingReport : Command
    {
        public IList<TreatyPricingReportGenerationBo> ReportGenerationBos { get; set; }

        public int UserId { get; set; }

        public List<string> Errors { get; set; }

        public ProcessTreatyPricingReport()
        {
            Title = "ProcessTreatyPricingReport";
            Description = "To process treaty & pricing non-SSRS reports export";
            Errors = new List<string> { };
        }

        public override void Run()
        {
            PrintStarting();
            Process();
            PrintEnding();
        }

        public void Process()
        {
            if (LoadReportGenerationBo() != null)
            {
                foreach (var reportGenerationBo in ReportGenerationBos)
                {
                    if (reportGenerationBo.ReportName == "Underwriting Limit Comparison Report")
                    {
                        var process = new ProcessUwLimitComparisonReport()
                        {
                            ReportGenerationBo = reportGenerationBo,
                        };
                        process.Process();
                        Errors = process.Errors;
                    }

                    if (reportGenerationBo.ReportName == "Advantage Program Comparison Report")
                    {
                        var process = new ProcessAdvantageProgramComparisonReport()
                        {
                            ReportGenerationBo = reportGenerationBo,
                        };
                        process.Process();
                        Errors = process.Errors;
                    }
                }
            }

            PrintProcessCount();

            foreach (string error in Errors)
            {
                PrintError(error);
            }
        }

        public IList<TreatyPricingReportGenerationBo> LoadReportGenerationBo()
        {
            ReportGenerationBos = TreatyPricingReportGenerationService.GetByStatus(TreatyPricingReportGenerationBo.StatusPending);

            return ReportGenerationBos;
        }
    }
}
