using ConsoleApp.Commands;
using ConsoleApp.Commands.Mappings;
using ConsoleApp.Commands.ProcessDatas;
using ConsoleApp.Commands.RawFiles;
using ConsoleApp.Commands.RawFiles.ClaimData;
using ConsoleApp.Commands.RawFiles.ClaimRegister;
using ConsoleApp.Commands.RawFiles.ReferralClaim;
using ConsoleApp.Commands.RawFiles.Retrocession;
using ConsoleApp.Commands.RawFiles.RiData;
using ConsoleApp.Commands.RawFiles.Sanction;
using ConsoleApp.Commands.RawFiles.SoaData;
using ConsoleApp.Commands.RawFiles.TreatyPricing;
using ConsoleApp.Commands.UpdateDatas;
using Shared;
using Shared.Argument;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ConsoleHandler.Set();

                CommandInput ci = null;
                if (args.Length > 0)
                    ci = new CommandInput(args);

                bool loading = ReadCommands(ci);
                while (loading)
                {
                    PrintCommands();
                    Console.WriteLine("");
                    Console.Write("> ");
                    var read = Console.ReadLine();

                    switch (read.ToLower())
                    {
                        case ":q":
                        case "exit":
                            loading = false;
                            continue;
                        case "hide":
                        case "hidden":
                            PrintHiddenCommands();
                            continue;
                    }

                    Console.WriteLine("");
                    ci = new CommandInput(read);
                    var failed = ReadCommands(ci);
                    if (failed)
                    {
                        Console.WriteLine("* Unrecognized command. Please try again.");
                    }
                    Console.WriteLine("");
                }
                ConsoleHandler.WriteLog("Console Ends");
            }
            catch (Exception e)
            {
                ConsoleHandler.WriteLog(e.Message, "ConsoleException");
                if (e.StackTrace.Length > 0)
                {
                    ConsoleHandler.WriteLog(e.StackTrace, "ConsoleException");
                }
            }
        }

        static IList<Command> Commands()
        {
            return new List<Command>()
            {
                new FinaliseRiDataBatch(),
                new GenerateE1(),
                new GenerateE2(),
                new GenerateE3E4(),
                new GenerateExportData(),
                new GenerateMfrs17Reporting(),
                new GenerateMfrs17SummaryReport(),
                new GeneratePdf(),
                new Migration(),
                new PasswordExpiryReminder(),
                new ProcessClaimDataBatch(),
                new ProcessClaimRegisterBatch(),
                new ProcessCutOff(),
                new ProcessDirectRetro(),
                new ProcessFacMasterListing(),
                new ProcessFinanceProvisioning(),
                new ProcessFinancialTable(),
                new ProcessInvoiceRegisterBatch(),
                new ProcessMedicalTable(),
                new ProcessMfrs17Reporting(),
                new ProcessPerLifeAggregation(),
                new ProcessPerLifeClaimDataValidation(),
                new ProcessPerLifeClaims(),
                new ProcessPerLifeClaimsRetroRecovery(),
                new ProcessPerLifeDataValidation(),
                new ProcessPerLifeDetailDataValidation(),
                new ProcessPerLifeSoa(),
                new ProcessPerLifeSplitData(),
                new ProcessProductFeatureMapping(),
                new ProcessQuotationWorkflowQuoteSpec(),
                new ProcessQuotationWorkflowRateTable(),
                new ProcessQuotationWorkflowCampaignSpec(),
                new ProcessRateTableMapping(),
                new ProcessReferralClaimAssessmentBatch(),
                new ProcessReferralRiDataFile(),
                new ProcessRetroRegisterBatch(),
                new ProcessRiDataBatch(),
                new ProcessSanctionBatch(),
                new ProcessSanctionFile(),
                new ProcessSanctionNameBatch(),
                new ProcessSanctionVerification(),
                new ProcessSoaDataBatch(),
                new ProcessTreatyPricingGroupReferral(),
                new ProcessTreatyPricingRateTableGroup(),
                new ProcessTreatyPricingReport(),
                new ProcessUwQuestionnaire(),
                new ProcessWarehouseRiDataBatch(),
                new ProvisionClaimRegisterBatch(),
                new ProvisionDirectRetroClaimRegisterBatch(),
                new ReportingClaimDataBatch(),
                new ReprocessProvisionClaimRegisterBatch(),
                new ScheduleSanctionVerification(),
                new SendInactiveUserReport(),
                new SendTestEmail(),
                new SummarySoaDataBatch(),
                new SuspendInactiveUser(),
                new UpdateMfrs17Reporting(),

                // Hidden Menu
                new CreateDummyData(),
                new DeleteClaimDataBatch(),
                new DeleteRiDataBatch(),
                new DeleteSoaDataBatch(),
                new DeletePerLifeAggregation(),
                new DeleteExportData(),
                new EvaluateStandardOutput(),
                new ExecuteSqlFile(),
                new ProcessAccountCodeMapping(),
                new ProcessBenefit(),
                new ProcessCellMappingCombination(),
                new ProcessInvoiceRegister(),
                new ProcessRetroRegister(),
                new ProcessMfrs17CellMapping(),
                new ProcessPerLifeDataCorrection(),
                new ProcessPickList(),
                new ProcessRateTableMappingCombination(),
                new ProcessRawFile(),
                new ProcessRetroStatement(),
                new ProcessRiDataCorrection(),
                new ProcessTreatyBenefitMappingCombination(),
                new ProcessPerLifeRetroStatement(),
                new ReadExcel(),
                new ReadFileMimeType(),
                new ReadTextFile(),
                new UpdateCellMapping(),
                new UpdateClaimAuthorityLimitCedant(),
                new UpdateClaimRegister(),
                new UpdateFinanceProvisioning(),
                new UpdateFinanceProvisioningTransaction(),
                new UpdateItemCodeMapping(),
                new UpdateMfrs17ReportingDetail(),
                new UpdatePerLifeAggregation(),
                new UpdatePremiumSpreadTable(),
                new UpdatePremiumSpreadTableDetail(),
                new UpdateRateTableMapping(),
                new UpdateReferralClaim(),
                new UpdateRiData(),
                new UpdateRiDataBatch(),
                new UpdateRiDataBatchFailedNumber(),
                new UpdateRiDataConfig(),
                new UpdateRiDataFile(),
                new UpdateRiDataMapping(),
                new UpdateTreatyBenefitCodeMapping(),
                new UpdateTreatyDiscountTable(),
                new UpdateTreatyOldCode(),
                new UpdateTreatyPricingGroupReferral(),
                new UpdateTreatyPricingPickListDetail(),
                new UpdateSanctionVerification(),
                new UpdateStoredProcedure(),
                new UpdateUserTrail(),
                new WriteExcel(),
                new UpdateTreatyPricingTreatyWorkflow(),
                new ProcessRateDetail(),
            };
        }

        static bool ReadCommands(CommandInput ci)
        {
            if (ci == null)
                return true;

            IList<Command> commands = Commands();
            var command = commands.Where(c => c.Title == ci.Command).FirstOrDefault();
            if (command != null)
            {
                command.CommandInput = ci;
                command.Initial();

                if (ci.IsHelp())
                {
                    command.PrintHelp();
                }
                else
                {
                    if (command.ValidRequiredArguments() && command.Validate())
                    {
                        command.Run();
                    }
                }
                return false;
            }
            return true;
        }

        static void PrintCommands()
        {
            IList<Command> commands = Commands();
            commands = commands.Where(c => !c.Hide).OrderBy(c => c.Title).ToList();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(Util.GetConfig("AppName") + " Console App");
            Console.ResetColor();

            int padRightWidth = commands.Max(c => c.Title.Length) + 5;
            PrintCommand("Exit", "To exit program", padRightWidth);
            foreach (Command cmd in commands)
            {
                PrintCommand(cmd.Title, cmd.Description, padRightWidth);
            }
        }

        static void PrintHiddenCommands()
        {
            IList<Command> commands = Commands();
            commands = commands.Where(c => c.Hide).OrderBy(c => c.Title).ToList();

            Console.WriteLine("");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Console App - Hidden Commands");
            Console.ResetColor();

            int padRightWidth = commands.Max(c => c.Title.Length) + 5;
            foreach (Command cmd in commands)
            {
                PrintCommand(cmd.Title, cmd.Description, padRightWidth);
            }

            Console.WriteLine("");
            Console.WriteLine("");
        }

        static void PrintCommand(string name, string description, int padRightWidth)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(name.PadRight(padRightWidth, ' '));
            Console.ResetColor();
            Console.Write(description + "\n");
        }
    }
}
