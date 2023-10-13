using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities;
using Services;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace ConsoleApp.Commands.ProcessDatas
{
    public class ProcessClaimCodeMapping : Command
    {
        public List<Column> Cols { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }

        public const int TypeId = 1;
        public const int TypeMlreEventCode = 2;
        public const int TypeMlreBenefitCode = 3;
        public const int TypeClaimCode = 4;
        public const int TypeAction = 5;

        public ProcessClaimCodeMapping()
        {
            Title = "ProcessClaimCodeMapping";
            Description = "To read Claim Code Mapping csv file and insert into database";
            Arguments = new string[]
            {
                "filePath",
            };
            Hide = true;
            Errors = new List<string> { };
            GetColumns();
        }

        public override bool Validate()
        {
            string filepath = CommandInput.Arguments[0];
            if (!File.Exists(filepath))
            {
                PrintError(string.Format(MessageBag.FileNotExists, filepath));
                return false;
            }
            else
            {
                FilePath = filepath;
            }

            return base.Validate();
        }

        public override void Run()
        {
            PrintStarting();

            Process();

            PrintEnding();
        }

        public void Process()
        {
            if (PostedFile != null)
            {
                TextFile = new TextFile(PostedFile.InputStream);
            }
            else if (File.Exists(FilePath))
            {
                TextFile = new TextFile(FilePath);
            }
            else
            {
                throw new Exception("No file can be read");
            }

            TrailObject trail;
            Result result;
            while (TextFile.GetNextRow() != null)
            {
                if (TextFile.RowIndex == 1)
                    continue; // Skip header row

                if (PrintCommitBuffer())
                {
                }
                SetProcessCount();

                bool error = false;

                ClaimCodeMappingBo ccm = null;
                try
                {
                    ccm = SetData();

                    if (!string.IsNullOrEmpty(ccm.MlreEventCode))
                    {
                        string[] mlreEventCodes = ccm.MlreEventCode.ToArraySplitTrim();
                        foreach (string mlreEventCodeStr in mlreEventCodes)
                        {
                            EventCodeBo eventCodeBo = EventCodeService.FindByCode(mlreEventCodeStr);
                            if (eventCodeBo == null)
                            {
                                SetProcessCount("MLRe Event Code Not Found");
                                Errors.Add(string.Format("{0} : {1} at row {2}", "MLRe Event Code not exist", mlreEventCodeStr, TextFile.RowIndex));
                                error = true;
                            }
                        }
                    }
                    else
                    {
                        SetProcessCount("MLRe Event Code Empty");
                        Errors.Add(string.Format("Please enter the MLRe Event Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }


                    if (!string.IsNullOrEmpty(ccm.MlreBenefitCode))
                    {
                        string[] mlreBenefitCodes = ccm.MlreBenefitCode.ToArraySplitTrim();
                        foreach (string mlreBenefitCodeStr in mlreBenefitCodes)
                        {
                            var benefiBos = BenefitService.FindByCode(mlreBenefitCodeStr);
                            if (benefiBos != null)
                            {
                                if (benefiBos.Status == BenefitBo.StatusInactive)
                                {
                                    SetProcessCount("MLRe Benefit Code Inactive");
                                    Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.BenefitStatusInactiveWithCode, mlreBenefitCodeStr, TextFile.RowIndex));
                                    error = true;
                                }
                            }
                            else
                            {
                                SetProcessCount("MLRe Benefit Code Not Found");
                                Errors.Add(string.Format("{0} at row {1}", string.Format(MessageBag.BenefitCodeNotFound, mlreBenefitCodeStr), TextFile.RowIndex));
                                error = true;
                            }
                        }
                    }
                    else
                    {
                        SetProcessCount("MLRe Benefit Code Empty");
                        Errors.Add(string.Format("Please enter the MLRe Benefit Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(ccm.ClaimCode))
                    {
                        ClaimCodeBo claimCodeBo = ClaimCodeService.FindByCode(ccm.ClaimCode);
                        if (claimCodeBo == null)
                        {
                            SetProcessCount("Claim Code Not Found");
                            Errors.Add(string.Format("The Claim Code doesn't exists: {0} at row {1}", ccm.ClaimCode, TextFile.RowIndex));
                            error = true;
                        }
                        else if (claimCodeBo.Status == ClaimCodeBo.StatusInactive)
                        {
                            SetProcessCount("Claim Code Inactive");
                            Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.ClaimCodeStatusInactive, ccm.ClaimCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            ccm.ClaimCodeId = claimCodeBo.Id;
                            ccm.ClaimCodeBo = claimCodeBo;
                        }
                    }
                    else
                    {
                        SetProcessCount("Claim Code Empty");
                        Errors.Add(string.Format("Please enter the Claim Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                }
                catch (Exception e)
                {
                    SetProcessCount("Error");
                    Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, TextFile.RowIndex));
                    error = true;
                }
                string action = TextFile.GetColValue(TypeAction);

                if (error)
                {
                    continue;
                }

                switch (action.ToLower().Trim())
                {
                    case "u":
                    case "update":

                        ClaimCodeMappingBo ccmDb = ClaimCodeMappingService.Find(ccm.Id);
                        if (ccmDb == null)
                        {
                            AddNotFoundError(ccm);
                            continue;
                        }

                        var mappingResult = ClaimCodeMappingService.ValidateMapping(ccm);
                        if (!mappingResult.Valid)
                        {
                            foreach (var e in mappingResult.ToErrorArray())
                            {
                                Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                            }
                            error = true;
                        }
                        if (error)
                        {
                            continue;
                        }

                        UpdateData(ref ccmDb, ccm);

                        trail = new TrailObject();
                        result = ClaimCodeMappingService.Update(ref ccmDb, ref trail);

                        ClaimCodeMappingService.ProcessMappingDetail(ccm, ccm.CreatedById); // DO NOT TRAIL
                        Trail(result, ccmDb, trail, "Update");

                        break;

                    case "d":
                    case "del":
                    case "delete":

                        if (ccm.Id != 0 && ClaimCodeMappingService.IsExists(ccm.Id))
                        {
                            trail = new TrailObject();
                            result = ClaimCodeMappingService.Delete(ccm, ref trail);
                            Trail(result, ccm, trail, "Delete");
                        }
                        else
                        {
                            AddNotFoundError(ccm);
                            continue;
                        }

                        break;

                    default:

                        if (ccm.Id != 0 && ClaimCodeMappingService.IsExists(ccm.Id))
                        {
                            SetProcessCount("Record Found");
                            Errors.Add(string.Format("The Claim Code Mapping ID exists: {0} at row {1}", ccm.Id, TextFile.RowIndex));
                            continue;
                        }

                        mappingResult = ClaimCodeMappingService.ValidateMapping(ccm);
                        if (!mappingResult.Valid)
                        {
                            foreach (var e in mappingResult.ToErrorArray())
                            {
                                Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                            }
                            error = true;
                        }
                        if (error)
                        {
                            continue;
                        }

                        trail = new TrailObject();
                        result = ClaimCodeMappingService.Create(ref ccm, ref trail);

                        ClaimCodeMappingService.ProcessMappingDetail(ccm, ccm.CreatedById); // DO NOT TRAIL
                        Trail(result, ccm, trail, "Create");

                        break;
                }
            }

            PrintProcessCount();

            TextFile.Close();

            foreach (string error in Errors)
            {
                PrintError(error);
            }
        }

        public void ExportToCsv(IEnumerable<ClaimCodeMappingBo> query)
        {
            string filename = "ClaimCodeMapping".AppendDateTimeFileName(".csv");
            string path = Util.GetTemporaryPath();

            FilePath = string.Format("{0}/{1}", path, filename);
            Util.MakeDir(FilePath);

            // Delete all previous files
            Util.DeleteFiles(path, @"ClaimCodeMapping*");

            // Header
            ExportWriteLine(string.Join(",", Cols.Select(p => p.Header)));

            if (query != null)
            {
                int total = query.Count();
                int take = 50;
                for (int skip = 0; skip < total + take; skip += take)
                {
                    if (skip >= total)
                        break;

                    foreach (var claimCodeMapping in query.OrderBy(q => q.Id).Skip(skip).Take(take).ToList())
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
                                default:
                                    v = claimCodeMapping.GetPropertyValue(col.Property);
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

                    total = query.Count();
                }
            }
        }

        public void ExportWriteLine(object line)
        {
            using (var textFile = new TextFile(FilePath, true, true))
            {
                textFile.WriteLine(line);
            }
        }

        public ClaimCodeMappingBo SetData()
        {
            var ccm = new ClaimCodeMappingBo
            {
                Id = 0,
                MlreEventCode = TextFile.GetColValue(TypeMlreEventCode),
                MlreBenefitCode = TextFile.GetColValue(TypeMlreBenefitCode),
                ClaimCode = TextFile.GetColValue(TypeClaimCode),
                CreatedById = AuthUserId != null ? AuthUserId.Value : DataAccess.Entities.Identity.User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : DataAccess.Entities.Identity.User.DefaultSuperUserId,
            };

            string idStr = TextFile.GetColValue(TypeId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                ccm.Id = id;
            }

            return ccm;
        }

        public void UpdateData(ref ClaimCodeMappingBo ccmDb, ClaimCodeMappingBo ccm)
        {
            ccmDb.MlreEventCode = ccm.MlreEventCode;
            ccmDb.MlreBenefitCode = ccm.MlreBenefitCode;
            ccmDb.ClaimCodeId = ccm.ClaimCodeId;
            ccmDb.UpdatedById = ccm.UpdatedById;
        }

        public void Trail(Result result, ClaimCodeMappingBo rt, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    rt.Id,
                    string.Format("{0} Claim Code Mapping", action),
                    result,
                    trail,
                    rt.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(action);
            }
        }

        public bool ValidateDateTimeFormat(int type, ref ClaimCodeMappingBo ccm)
        {
            string header = Cols.Where(m => m.ColIndex == type).Select(m => m.Header).FirstOrDefault();
            string property = Cols.Where(m => m.ColIndex == type).Select(m => m.Property).FirstOrDefault();

            bool valid = true;
            string value = TextFile.GetColValue(type);
            if (!string.IsNullOrEmpty(value))
            {
                if (Util.TryParseDateTime(value, out DateTime? datetime, out string error))
                {
                    ccm.SetPropertyValue(property, datetime.Value);
                }
                else
                {
                    SetProcessCount(string.Format(header, "Error"));
                    Errors.Add(string.Format("The {0} format can not be read: {1} at row {2}", header, value, TextFile.RowIndex));
                    valid = false;
                }
            }
            return valid;
        }

        public void AddNotFoundError(ClaimCodeMappingBo ccm)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The Rate Table ID doesn't exists: {0} at row {1}", ccm.Id, TextFile.RowIndex));
        }

        public List<Column> GetColumns()
        {
            Cols = new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    ColIndex = TypeId,
                    Property = "Id",
                },
                new Column
                {
                    Header = "MLRe Event Code",
                    ColIndex = TypeMlreEventCode,
                    Property = "MlreEventCode",
                },
                new Column
                {
                    Header = "MLRe Benefit Code",
                    ColIndex = TypeMlreBenefitCode,
                    Property = "MlreBenefitCode",
                },
                new Column
                {
                    Header = "Claim Code",
                    ColIndex = TypeClaimCode,
                    Property = "ClaimCode",
                },
                new Column
                {
                    Header = "Action",
                    ColIndex = TypeAction,
                },
            };

            return Cols;
        }
    }
}
