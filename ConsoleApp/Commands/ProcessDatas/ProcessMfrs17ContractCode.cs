using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using Services;
using Services.Identity;
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
    public class ProcessMfrs17ContractCode : Command
    {
        public List<Column> Columns { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }

        public List<string> UpdateAction { get; set; } = new List<string> { "u", "update" };

        public List<string> DeleteAction { get; set; } = new List<string> { "d", "del", "delete" };

        public ProcessMfrs17ContractCode()
        {
            Title = "ProcessMfrs17ContractCode";
            Description = "To read MFRS17 Contract Code csv file and insert into database";
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

                Mfrs17ContractCodeBo cc = null;
                Mfrs17ContractCodeDetailBo ccd = null;
                try
                {
                    cc = SetParentData();
                    ccd = SetChildData();

                    if (!string.IsNullOrEmpty(cc.CedantCode))
                    {
                        CedantBo cedantBo = CedantService.FindByCode(cc.CedantCode);
                        if (cedantBo == null)
                        {
                            SetProcessCount("Cedant Code Not Found");
                            Errors.Add(string.Format("The Cedant Code doesn't exists: {0} at row {1}", cc.CedantCode, TextFile.RowIndex));
                            error = true;
                        }
                        else if (cedantBo.Status == CedantBo.StatusInactive)
                        {
                            SetProcessCount("Cedant Inactive");
                            Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.CedantStatusInactive, cc.CedantCode, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            cc.CedingCompanyId = cedantBo.Id;
                            cc.CedingCompanyBo = cedantBo;
                        }
                    }
                    else
                    {
                        SetProcessCount("Cedant Code Empty");
                        Errors.Add(string.Format("Please enter the Cedant Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (string.IsNullOrEmpty(cc.ModifiedContractCode))
                    {
                        SetProcessCount("Modified Contract Code Empty");
                        Errors.Add(string.Format("Please enter the Modified Contract Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    // Child
                    if (ccd.Id != 0 && string.IsNullOrEmpty(cc.ModifiedContractCode))
                    {
                        SetProcessCount("MFRS17 Contract Code Empty");
                        Errors.Add(string.Format("Please enter the MFRS17 Contract Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                }
                catch (Exception e)
                {
                    SetProcessCount("Error");
                    Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, TextFile.RowIndex));
                    error = true;
                }
                string parentAction = TextFile.GetColValue(Mfrs17ContractCodeBo.ColumnAction);
                string childAction = TextFile.GetColValue(Mfrs17ContractCodeBo.ColumnContractCodeAction);

                if (error)
                {
                    continue;
                }

                // Parent
                Mfrs17ContractCodeBo mfrs17ContractCodeBo;
                if (cc.Id != 0)
                {
                    mfrs17ContractCodeBo = Mfrs17ContractCodeService.Find(cc.Id);
                }
                else
                {
                    mfrs17ContractCodeBo = Mfrs17ContractCodeService.FindByCedingCompanyModifiedContractCodeId(cc.CedingCompanyId, cc.ModifiedContractCode);
                    cc.Id = mfrs17ContractCodeBo?.Id ?? 0;
                }

                if (UpdateAction.Contains(parentAction.ToLower().Trim()))
                {
                    // Handle Update
                    if (ccd.Id != 0 || !string.IsNullOrEmpty(ccd.ContractCode) || !string.IsNullOrEmpty(childAction))
                    {
                        SetProcessCount("Child Data Not Allowed");
                        Errors.Add(string.Format("Unable to process Row {0} due to child data exist in Parent's Update Action", TextFile.RowIndex));
                        continue;
                    }

                    if (mfrs17ContractCodeBo == null)
                    {
                        AddParentNotFoundError(mfrs17ContractCodeBo);
                        continue;
                    }

                    var duplicateResult = Mfrs17ContractCodeService.FindByCedingCompanyModifiedContractCodeId(mfrs17ContractCodeBo.CedingCompanyId, mfrs17ContractCodeBo.ModifiedContractCode, mfrs17ContractCodeBo.Id);
                    if (duplicateResult != null)
                    {
                        SetProcessCount("Existing Parent Record Found");
                        Errors.Add(string.Format("The Modified Contract Code: {0} exists with same Ceding Company at row {1}", cc.ModifiedContractCode, TextFile.RowIndex));
                        continue;
                    }

                    UpdateParentData(ref mfrs17ContractCodeBo, cc);

                    trail = new TrailObject();
                    result = Mfrs17ContractCodeService.Update(ref mfrs17ContractCodeBo, ref trail);
                    TrailParent(result, mfrs17ContractCodeBo, trail, "Update");

                    continue;
                }
                else if (DeleteAction.Contains(parentAction.ToLower().Trim()))
                {
                    // Handle Delete
                    if (ccd.Id != 0 || !string.IsNullOrEmpty(ccd.ContractCode) || !string.IsNullOrEmpty(childAction))
                    {
                        SetProcessCount("Child Data Not Allowed");
                        Errors.Add(string.Format("Unable to process Row {0} due to child data exist in Parent's Delete Action", TextFile.RowIndex));
                        continue;
                    }

                    if (mfrs17ContractCodeBo == null)
                    {
                        AddParentNotFoundError(mfrs17ContractCodeBo);
                        continue;
                    }

                    trail = new TrailObject();
                    result = Mfrs17ContractCodeService.Delete(mfrs17ContractCodeBo, ref trail);

                    if (!result.Valid)
                    {
                        foreach (var e in result.ToErrorArray())
                        {
                            Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                        }
                        continue;
                    }

                    TrailParent(result, mfrs17ContractCodeBo, trail, "Delete");

                    continue;
                }
                else
                {
                    // Handle Create
                    if (mfrs17ContractCodeBo == null)
                    {
                        var duplicateResult = Mfrs17ContractCodeService.FindByCedingCompanyModifiedContractCodeId(cc.CedingCompanyId, cc.ModifiedContractCode, cc.Id);
                        if (duplicateResult != null)
                        {
                            SetProcessCount("Existing Parent Record Found");
                            Errors.Add(string.Format("The Modified Contract Code: {0} exists with same Ceding Company at row {1}", cc.ModifiedContractCode, TextFile.RowIndex));
                            continue;
                        }

                        trail = new TrailObject();
                        result = Mfrs17ContractCodeService.Create(ref cc, ref trail);
                        if (result.Valid)
                        {
                            mfrs17ContractCodeBo = cc;
                            TrailParent(result, cc, trail, "Create");
                        }
                        else
                        {
                            Errors.Add(string.Format("Unable to process Row {0} due to error occured during create Modified Contract Code", TextFile.RowIndex));
                            foreach (var e in result.ToErrorArray())
                            {
                                Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                            }
                            continue;
                        }
                    }

                    switch (childAction.ToLower().Trim())
                    {
                        case "u":
                        case "update":
                            Mfrs17ContractCodeDetailBo ccdDb = Mfrs17ContractCodeDetailService.Find(ccd.Id);
                            if (ccdDb == null)
                            {
                                AddChildNotFoundError(ccd);
                                continue;
                            }

                            var duplicateResult = Mfrs17ContractCodeDetailService.FindByMfsr17ContractCodeIdContractCodeId(mfrs17ContractCodeBo.Id, ccd.ContractCode, ccd.Id);
                            if (duplicateResult != null)
                            {
                                SetProcessCount("Existing Child Record Found");
                                Errors.Add(string.Format("The MFRS17 Contract Code: {0} exists with same Modified Contract Code at row {1}", ccd.ContractCode, TextFile.RowIndex));
                                continue;
                            }

                            ccd.Mfrs17ContractCodeId = mfrs17ContractCodeBo.Id;
                            UpdateChildData(ref ccdDb, ccd);

                            trail = new TrailObject();
                            result = Mfrs17ContractCodeDetailService.Update(ref ccdDb, ref trail);
                            TrailChild(result, ccdDb, trail, "Update");

                            break;
                        case "d":
                        case "del":
                        case "delete":
                            ccdDb = Mfrs17ContractCodeDetailService.Find(ccd.Id);
                            if (ccdDb == null)
                            {
                                AddChildNotFoundError(ccd);
                                continue;
                            }

                            if (Mfrs17CellMappingService.CountByMfrs17ContractCodeDetailId(ccd.Id) > 0)
                            {
                                SetProcessCount("Child Record In Use");
                                Errors.Add(string.Format("The MFRS17 Contract Code In Use: {0} at row {1}", ccd.ContractCode, TextFile.RowIndex));
                                continue;
                            }

                            trail = new TrailObject();
                            result = Mfrs17ContractCodeDetailService.Delete(ccd, ref trail);
                            TrailChild(result, ccd, trail, "Delete");

                            break;
                        default:
                            if (cc.Id != 0 && Mfrs17ContractCodeDetailService.IsExists(ccd.Id))
                            {
                                SetProcessCount("Child Record Found");
                                Errors.Add(string.Format("The MFRS17 Contract Code ID exists: {0} at row {1}", ccd.Id, TextFile.RowIndex));
                                continue;
                            }

                            if (cc.Id == 0 && string.IsNullOrEmpty(cc.Mfrs17ContractCode))
                                continue;

                            var mfrs17ContractCodeDetailBo = Mfrs17ContractCodeDetailService.FindByMfsr17ContractCodeIdContractCodeId(mfrs17ContractCodeBo.Id, ccd.ContractCode);
                            if (mfrs17ContractCodeDetailBo != null)
                            {
                                SetProcessCount("Child Record Found");
                                Errors.Add(string.Format("The MFRS17 Contract Code: {0} exists with same Modified Contract Code at row {1}", ccd.ContractCode, TextFile.RowIndex));
                                continue;
                            }

                            trail = new TrailObject();
                            ccd.Mfrs17ContractCodeId = mfrs17ContractCodeBo.Id;
                            result = Mfrs17ContractCodeDetailService.Create(ref ccd, ref trail);
                            TrailChild(result, ccd, trail, "Create");
                            break;
                    }
                    continue;
                }
            }

            PrintProcessCount();

            TextFile.Close();

            foreach (string error in Errors)
            {
                PrintError(error);
            }
        }

        public Mfrs17ContractCodeBo SetParentData()
        {
            var cc = new Mfrs17ContractCodeBo
            {
                Id = 0,
                CedantCode = TextFile.GetColValue(Mfrs17ContractCodeBo.ColumnCedant),
                ModifiedContractCode = TextFile.GetColValue(Mfrs17ContractCodeBo.ColumnModifiedContractCode),
                CreatedById = AuthUserId != null ? AuthUserId.Value : User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : User.DefaultSuperUserId,
            };

            cc.CedantCode = cc.CedantCode?.Trim();
            cc.ModifiedContractCode = cc.ModifiedContractCode?.Trim();

            string idStr = TextFile.GetColValue(Mfrs17ContractCodeBo.ColumnId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                cc.Id = id;
            }

            return cc;
        }

        public Mfrs17ContractCodeDetailBo SetChildData()
        {
            var ccd = new Mfrs17ContractCodeDetailBo
            {
                Id = 0,
                ContractCode = TextFile.GetColValue(Mfrs17ContractCodeBo.ColumnContractCode),
                CreatedById = AuthUserId != null ? AuthUserId.Value : User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : User.DefaultSuperUserId,
            };

            ccd.ContractCode = ccd.ContractCode?.Trim();

            string idStr = TextFile.GetColValue(Mfrs17ContractCodeBo.ColumnContractCodeId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                ccd.Id = id;
            }

            return ccd;
        }

        public void UpdateParentData(ref Mfrs17ContractCodeBo ccDb, Mfrs17ContractCodeBo cc)
        {
            ccDb.CedingCompanyId = cc.CedingCompanyId;
            ccDb.ModifiedContractCode = cc.ModifiedContractCode;
            ccDb.UpdatedById = cc.UpdatedById;
        }

        public void UpdateChildData(ref Mfrs17ContractCodeDetailBo ccdDb, Mfrs17ContractCodeDetailBo ccd)
        {
            ccdDb.Mfrs17ContractCodeId = ccd.Mfrs17ContractCodeId;
            ccdDb.ContractCode = ccd.ContractCode;
            ccdDb.UpdatedById = ccd.UpdatedById;
        }

        public void TrailParent(Result result, Mfrs17ContractCodeBo cc, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    cc.Id,
                    string.Format("{0} MFRS17 Contract Code", action),
                    result,
                    trail,
                    cc.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(string.Format("{0} Parent", action));
            }
        }

        public void TrailChild(Result result, Mfrs17ContractCodeDetailBo ccd, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    ccd.Id,
                    string.Format("{0} MFRS17 Contract Code Detail", action),
                    result,
                    trail,
                    ccd.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(string.Format("{0} Child", action));
            }
        }

        public bool ValidateDateTimeFormat(int type, ref Mfrs17ContractCodeBo cc)
        {
            string header = Columns.Where(m => m.ColIndex == type).Select(m => m.Header).FirstOrDefault();
            string property = Columns.Where(m => m.ColIndex == type).Select(m => m.Property).FirstOrDefault();

            bool valid = true;
            string value = TextFile.GetColValue(type);
            if (!string.IsNullOrEmpty(value))
            {
                if (Util.TryParseDateTime(value, out DateTime? datetime, out string error))
                {
                    cc.SetPropertyValue(property, datetime.Value);
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

        public void AddParentNotFoundError(Mfrs17ContractCodeBo cc)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The Modified Contract Code ID doesn't exists: {0} at row {1}", cc.Id, TextFile.RowIndex));
        }

        public void AddChildNotFoundError(Mfrs17ContractCodeDetailBo ccd)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The MFRS17 Contract Code ID doesn't exists: {0} at row {1}", ccd.Id, TextFile.RowIndex));
        }

        public List<Column> GetColumns()
        {
            Columns = Mfrs17ContractCodeBo.GetColumns();
            return Columns;
        }
    }
}
