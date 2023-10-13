using DataAccess.EntityFramework;
using Services;
using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;

namespace ConsoleApp.Commands.Mappings
{
    public class ProcessRateTableMappingCombination : Command
    {
        public ProcessRateTableMappingCombination()
        {
            Title = "ProcessRateTableMappingCombination";
            Description = "To process combination for Rate Table Mapping";
            Arguments = new string[]
            {
            };
            Options = new string[] {
            };
            Hide = true;
        }

        public override void Initial()
        {
            base.Initial();
        }

        public override bool Validate()
        {
            return base.Validate();
        }

        public override void Run()
        {
            PrintStarting();
            var list = new Dictionary<string, List<string>> { };
            using (var db = new AppDbContext())
            {
                try
                {
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [RateTableDetails]");
                    PrintMessage("Truncated [RateTableDetails] table");

                    int total = db.RateTables.Count();
                    int take = CommitLimit;
                    for (int skip = 0; skip < (total + take); skip += take)
                    {
                        if (skip >= total)
                            break;

                        PrintCommitBuffer();

                        foreach (var data in db.RateTables.OrderBy(q => q.Id).Skip(skip).Take(take).ToList())
                        {
                            SetProcessCount();

                            var bo = RateTableService.FormBo(data);
                            foreach (var detailBo in RateTableService.CreateDetails(bo, bo.CreatedById))
                            {
                                var d = detailBo;
                                RateTableService.TrimMaxLength(ref d, ref list);

                                SetProcessCount("Combination");
                                db.RateTableDetails.Add(RateTableDetailService.FormEntity(detailBo));
                            }
                        }
                        db.SaveChanges();

                        if (!IsCommitBuffer())
                            PrintProcessCount();

                        total = db.RateTables.Count();
                    }
                }
                catch (Exception e)
                {
                    // catch error open file
                    var message = e.Message;
                    if (e is DbEntityValidationException dbEx)
                    {
                        message = Util.CatchDbEntityValidationException(dbEx).ToString();
                    }

                    PrintError(message);
                }
            }

            if (list.Count > 0)
            {
                var path = LogFilePath(string.Format("{0}.TrimMaxLength", Title));
                var logFileInfo = new FileInfo(path);

                Util.MakeDir(path);
                
                var logFile = new TextFile(path, true);
                logFile.WriteLine(DateTime.Now.ToString(Util.GetDateTimeFormat()));
                logFile.WriteLine("");

                foreach(var prop in list)
                {
                    logFile.WriteLine("".PadRight(50, '-'));
                    logFile.WriteLine(prop.Key);
                    logFile.WriteLine("");
                    foreach(var item in prop.Value)
                    {
                        var split = item.Split('|');
                        var oldvalue = split[0];
                        var newValue = split[1];
                        logFile.WriteLine(Util.FormatDetail(" * old", oldvalue, width: 6));
                        logFile.WriteLine(Util.FormatDetail(" > new", newValue, width: 6));
                        logFile.WriteLine("");
                    }
                    logFile.WriteLine("");
                }

                logFile.Dispose();

                PrintMessage(string.Format("Please verify {0} file for TrimMaxLength", logFileInfo.FullName));
            }

            PrintEnding();
        }
    }
}
