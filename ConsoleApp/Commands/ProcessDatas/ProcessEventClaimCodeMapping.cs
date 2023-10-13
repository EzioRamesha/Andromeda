using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using Services;
using Services.Identity;
using Services.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.ProcessFile;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


namespace ConsoleApp.Commands.ProcessDatas
{
    public class ProcessEventClaimCodeMapping : Command
    {
        public List<Column> Cols { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }

        public char[] charsToTrim = { ',', '.', ' ' };

        public const int TypeId = 1;
        public const int TypeCedantCode = 2;
        public const int TypeMLReEventCode = 3;
        public const int TypeCedingEventCode = 4;
        public const int TypeCedingClaimType = 5;
        public const int TypeAction = 6;

        public ProcessEventClaimCodeMapping()
        {
            Title = "ProcessEventClaimCodeMapping";
            Description = "To read Event Claim Code Mapping csv file and insert into database";
            Arguments = new string[]
            {
                "filePath",
            };
            Hide = true;
            Errors = new List<string> { };
            GetMappings();
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

                EventClaimCodeMappingBo cm = null;
                try
                {
                    cm = SetData();

                    if (!string.IsNullOrEmpty(cm.CedantCode))
                    {
                        CedantBo cedant = CedantService.FindByCode(cm.CedantCode);
                        if (cedant == null)
                        {
                            SetProcessCount("Cedant Code Not Found");
                            Errors.Add(string.Format("The Cedant Code doesn't exists: {0} at row {1}", cm.CedantCode, TextFile.RowIndex));
                            error = true;
                        }
                        else if (cedant.Status == CedantBo.StatusInactive)
                        {
                            SetProcessCount("Cedant Code Inactive");
                            Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.CedantStatusInactive, cm.CedantCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            cm.CedantId = cedant.Id;
                            cm.CedantBo = cedant;
                        }
                    }
                    else
                    {
                        SetProcessCount("Cedant Code Empty");
                        Errors.Add(string.Format("Please enter the Cedant Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(cm.MLReEventCode))
                    {
                        EventCodeBo ec = EventCodeService.FindByCode(cm.MLReEventCode);
                        if (ec == null)
                        {
                            SetProcessCount("MLRe Event Code Not Found");
                            Errors.Add(string.Format("The MLRe Event Code doesn't exists: {0} at row {1}", cm.MLReEventCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            cm.EventCodeId = ec.Id;
                            cm.EventCodeBo = ec;
                        }
                    }
                    else
                    {
                        SetProcessCount("MLRe Event Code Empty");
                        Errors.Add(string.Format("Please enter the MLRe Event Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (string.IsNullOrEmpty(cm.CedingEventCode) && string.IsNullOrEmpty(cm.CedingClaimType))
                    {
                        SetProcessCount("Ceding Event Code & Ceding Claim Type Empty");
                        Errors.Add(string.Format("Please enter the Ceding Event Code or Ceding Claim Type at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(cm.CedingEventCode))
                        {
                            cm.CedingEventCode = null;
                        }

                        if (string.IsNullOrEmpty(cm.CedingClaimType))
                        {
                            cm.CedingClaimType = null;
                        }
                    }
                }
                catch (Exception e)
                {
                    SetProcessCount("Error");
                    Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, TextFile.RowIndex));
                    error = true;
                }

                if (error)
                {
                    continue;
                }

                string action = TextFile.GetColValue(TypeAction);
                if (action == null)
                    action = "";

                switch (action.ToLower().Trim())
                {
                    case "u":
                    case "update":

                        EventClaimCodeMappingBo cmDb = EventClaimCodeMappingService.Find(cm.Id);
                        if (cmDb == null)
                        {
                            AddNotFoundError(cm);
                            continue;
                        }

                        var mappingResult = EventClaimCodeMappingService.ValidateMapping(cm);
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

                        UpdateData(ref cmDb, cm);

                        trail = new TrailObject();
                        result = EventClaimCodeMappingService.Update(ref cmDb, ref trail);

                        EventClaimCodeMappingService.ProcessMappingDetail(cmDb, cmDb.CreatedById); // DO NOT TRAIL
                        Trail(result, cmDb, trail, "Update");

                        break;

                    case "d":
                    case "del":
                    case "delete":

                        if (cm.Id != 0 && EventClaimCodeMappingService.IsExists(cm.Id))
                        {
                            trail = new TrailObject();
                            result = EventClaimCodeMappingService.Delete(cm, ref trail);
                            Trail(result, cm, trail, "Delete");
                        }
                        else
                        {
                            AddNotFoundError(cm);
                            continue;
                        }

                        break;

                    default:

                        if (cm.Id != 0 && EventClaimCodeMappingService.IsExists(cm.Id))
                        {
                            SetProcessCount("Record Found");
                            Errors.Add(string.Format("The Event Claim Code Mapping ID exists: {0} at row {1}", cm.Id, TextFile.RowIndex));
                            continue;
                        }

                        mappingResult = EventClaimCodeMappingService.ValidateMapping(cm);
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
                        result = EventClaimCodeMappingService.Create(ref cm, ref trail);

                        EventClaimCodeMappingService.ProcessMappingDetail(cm, cm.CreatedById); // DO NOT TRAIL
                        Trail(result, cm, trail, "Create");

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

        public void ExportToCsv(IEnumerable<EventClaimCodeMappingBo> query)
        {
            string filename = "EventClaimCodeMapping".AppendDateTimeFileName(".csv");
            string path = Util.GetTemporaryPath();

            FilePath = string.Format("{0}/{1}", path, filename);
            Util.MakeDir(FilePath);

            // Delete all previous files
            Util.DeleteFiles(path, @"EventClaimCodeMapping*");

            TextFile textFile = new TextFile(FilePath, true, true);

            // Header
            textFile.WriteLine(string.Join(",", Cols.Select(p => p.Header)));

            if (query != null)
            {
                int total = query.Count();
                int take = 50;
                for (int skip = 0; skip < total + take; skip += take)
                {
                    if (skip >= total)
                        break;

                    foreach (var eventClaimCodeMapping in query.OrderBy(q => q.Id).Skip(skip).Take(take).ToList())
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
                            object v = eventClaimCodeMapping.GetPropertyValue(col.Property);

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
                        textFile.WriteLine(line);
                    }

                    total = query.Count();
                }
            }
            textFile.Close();
        }

        public EventClaimCodeMappingBo SetData()
        {
            var rdc = new EventClaimCodeMappingBo
            {
                Id = 0,
                CedantCode = TextFile.GetColValue(TypeCedantCode),
                MLReEventCode = TextFile.GetColValue(TypeMLReEventCode),
                CedingEventCode = TextFile.GetColValue(TypeCedingEventCode),
                CedingClaimType = TextFile.GetColValue(TypeCedingClaimType),
                CreatedById = AuthUserId != null ? AuthUserId.Value : DataAccess.Entities.Identity.User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : DataAccess.Entities.Identity.User.DefaultSuperUserId,
            };

            rdc.CedantCode = rdc.CedantCode?.Trim();
            rdc.MLReEventCode = rdc.MLReEventCode?.Trim();
            rdc.CedingEventCode = rdc.CedingEventCode?.TrimEnd(charsToTrim);
            rdc.CedingClaimType = rdc.CedingClaimType?.TrimEnd(charsToTrim);

            string idStr = TextFile.GetColValue(TypeId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                rdc.Id = id;
            }

            return rdc;
        }

        public void UpdateData(ref EventClaimCodeMappingBo cmDb, EventClaimCodeMappingBo cm)
        {
            cmDb.CedantId = cm.CedantId;
            cmDb.EventCodeId = cm.EventCodeId;
            cmDb.CedingEventCode = cm.CedingEventCode;
            cmDb.CedingClaimType = cm.CedingClaimType;
            cmDb.UpdatedById = cm.UpdatedById;
        }

        public void Trail(Result result, EventClaimCodeMappingBo rdc, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    rdc.Id,
                    string.Format("{0} Event Claim Code Mapping", action),
                    result,
                    trail,
                    rdc.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(action);
            }
        }

        public void AddNotFoundError(EventClaimCodeMappingBo rdc)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The Event Claim Code Mapping ID doesn't exists: {0} at row {1}", rdc.Id, TextFile.RowIndex));
        }

        public string GetHeader(int col)
        {
            return Cols.Where(m => m.ColIndex == col).Select(m => m.Header).FirstOrDefault();
        }

        public string GetProperty(int col)
        {
            return Cols.Where(m => m.ColIndex == col).Select(m => m.Property).FirstOrDefault();
        }

        public List<Column> GetMappings()
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
                    Header = "Ceding Company",
                    ColIndex = TypeCedantCode,
                    Property = "CedantCode",
                },
                new Column
                {
                    Header = "MLRe Event Code",
                    ColIndex = TypeMLReEventCode,
                    Property = "MLReEventCode",
                },
                new Column
                {
                    Header = "Ceding Event Code",
                    ColIndex = TypeCedingEventCode,
                    Property = "CedingEventCode",
                },
                new Column
                {
                    Header = "Ceding Claim Type",
                    ColIndex = TypeCedingClaimType,
                    Property = "CedingClaimType",
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
