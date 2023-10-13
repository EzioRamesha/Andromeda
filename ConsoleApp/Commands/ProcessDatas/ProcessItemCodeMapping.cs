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
using System.IO;
using System.Linq;
using System.Web;

namespace ConsoleApp.Commands.ProcessDatas
{
    public class ProcessItemCodeMapping : Command
    {
        public List<Column> Columns { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }

        public ProcessItemCodeMapping()
        {
            Title = "ProcessItemCodeMapping";
            Description = "To read Item Code Mapping csv file and insert into database";
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

                ItemCodeMappingBo icm = null;
                try
                {
                    icm = SetData();

                    if (!string.IsNullOrEmpty(icm.InvoiceField))
                    {
                        PickListDetailBo rbc = PickListDetailService.FindByPickListIdCode(PickListBo.InvoiceField, icm.InvoiceField);
                        if (rbc == null)
                        {
                            SetProcessCount("Invoice Field Not Found");
                            Errors.Add(string.Format("The Invoice Field doesn't exists: {0} at row {1}", icm.InvoiceField, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            icm.InvoiceFieldPickListDetailId = rbc.Id;
                            icm.InvoiceFieldPickListDetailBo = rbc;
                        }
                    }
                    else
                    {
                        SetProcessCount("Invoice Field Empty");
                        Errors.Add(string.Format("Please enter the Invoice Field at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(icm.TreatyType))
                    {
                        string[] treatyTypes = icm.TreatyType.ToArraySplitTrim();
                        foreach (string treatyTypeStr in treatyTypes)
                        {
                            PickListDetailBo pickListDetailBo = PickListDetailService.FindByPickListIdCode(PickListBo.TreatyType, treatyTypeStr);
                            if (pickListDetailBo == null)
                            {
                                SetProcessCount("Treaty Type Not Found");
                                Errors.Add(string.Format("{0} : {1} at row {2}", "Treaty Type not exist", treatyTypeStr, TextFile.RowIndex));
                                error = true;
                            }
                        }
                    }
                    else
                    {
                        SetProcessCount("Treaty Type Empty");
                        Errors.Add(string.Format("Please enter the Treaty Type at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(icm.TreatyCode))
                    {
                        string[] treatyCodes = icm.TreatyCode.ToArraySplitTrim();
                        foreach (string treatyCodeStr in treatyCodes)
                        {
                            var treatyCodeBo = TreatyCodeService.FindByCode(treatyCodeStr);
                            if (treatyCodeBo != null)
                            {
                                if (treatyCodeBo.Status == TreatyCodeBo.StatusInactive)
                                {
                                    SetProcessCount("Treaty Code Inactive");
                                    Errors.Add(string.Format("{0}: {1} at row {2}", MessageBag.TreatyCodeStatusInactive, treatyCodeStr, TextFile.RowIndex));
                                    error = true;
                                }
                            }
                            else
                            {
                                SetProcessCount("Treaty Code Not Found");
                                Errors.Add(string.Format("{0} at row {1}", string.Format(MessageBag.TreatyCodeNotFound, treatyCodeStr), TextFile.RowIndex));
                                error = true;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(icm.BusinessOrigin))
                    {
                        PickListDetailBo bo = PickListDetailService.FindByPickListIdCode(PickListBo.BusinessOrigin, icm.BusinessOrigin);
                        if (bo == null)
                        {
                            icm.BusinessOrigin = null;
                            SetProcessCount("Business Origin Not Found");
                            Errors.Add(string.Format("The Business Origin doesn't exists: {0} at row {1}", icm.BusinessOrigin, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            icm.BusinessOriginPickListDetailId = bo.Id;
                            icm.BusinessOriginPickListDetailBo = bo;
                        }
                    }
                    else
                    {
                        SetProcessCount("Business Origin Empty");
                        Errors.Add(string.Format("Please enter the Business Origin at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    var ReportingTypeStr = TextFile.GetColValue(ItemCodeMappingBo.ColumnReportingType);
                    if (!string.IsNullOrEmpty(ReportingTypeStr))
                    {
                        var ReportingType = ItemCodeBo.GetReportingType(ReportingTypeStr);
                        if (ReportingType == 0)
                        {
                            SetProcessCount("Reporting Type Not Found");
                            Errors.Add(string.Format("The Reporting Type doesn't exists: {0} at row {1}", ReportingTypeStr, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            icm.ReportingType = ReportingType;
                        }
                    }
                    else
                    {
                        SetProcessCount("Reporting Type Empty");
                        Errors.Add(string.Format("Please enter the Reporting Type at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(icm.ItemCode))
                    {
                        if (!string.IsNullOrEmpty(icm.BusinessOrigin))
                        {
                            if (icm.ReportingType != 0)
                            {
                                ItemCodeBo ic = ItemCodeService.FindByCodeBusinessOriginCodeReportingType(icm.ItemCode, icm.BusinessOrigin, icm.ReportingType);
                                if (ic == null)
                                {
                                    SetProcessCount("Item Code Not Found");
                                    Errors.Add(string.Format("The Item code with Busness Origin: {0} and Reporting Type: {1} doesn't exists: {2} at row {3}", icm.BusinessOrigin, ReportingTypeStr, icm.ItemCode, TextFile.RowIndex));
                                    error = true;
                                }
                                else
                                {
                                    icm.ItemCodeId = ic.Id;
                                    icm.ItemCodeBo = ic;
                                }
                            }  
                        }
                    }
                    else
                    {
                        SetProcessCount("Item Code Empty");
                        Errors.Add(string.Format("Please enter the Item Code at row {0}", TextFile.RowIndex));
                        error = true;
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

                string action = TextFile.GetColValue(ItemCodeMappingBo.ColumnAction);
                if (action == null)
                    action = "";

                switch (action.ToLower().Trim())
                {
                    case "u":
                    case "update":

                        ItemCodeMappingBo icmDb = ItemCodeMappingService.Find(icm.Id);
                        if (icmDb == null)
                        {
                            AddNotFoundError(icm);
                            continue;
                        }

                        var mappingResult = ItemCodeMappingService.ValidateMapping(icm);
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

                        UpdateData(ref icmDb, icm);

                        trail = new TrailObject();
                        result = ItemCodeMappingService.Update(ref icmDb, ref trail);

                        ItemCodeMappingService.ProcessMappingDetail(icmDb, icmDb.CreatedById); // DO NOT TRAIL
                        Trail(result, icmDb, trail, "Update");

                        break;

                    case "d":
                    case "del":
                    case "delete":

                        if (icm.Id != 0 && ItemCodeMappingService.IsExists(icm.Id))
                        {
                            trail = new TrailObject();
                            result = ItemCodeMappingService.Delete(icm, ref trail);
                            Trail(result, icm, trail, "Delete");
                        }
                        else
                        {
                            AddNotFoundError(icm);
                            continue;
                        }

                        break;

                    default:

                        if (icm.Id != 0 && ItemCodeMappingService.IsExists(icm.Id))
                        {
                            SetProcessCount("Record Found");
                            Errors.Add(string.Format("The Item Code Mapping ID exists: {0} at row {1}", icm.Id, TextFile.RowIndex));
                            continue;
                        }

                        mappingResult = ItemCodeMappingService.ValidateMapping(icm);
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
                        result = ItemCodeMappingService.Create(ref icm, ref trail);

                        ItemCodeMappingService.ProcessMappingDetail(icm, icm.CreatedById); // DO NOT TRAIL
                        Trail(result, icm, trail, "Create");

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

        public ItemCodeMappingBo SetData()
        {
            var icm = new ItemCodeMappingBo
            {
                Id = 0,
                InvoiceField = TextFile.GetColValue(ItemCodeMappingBo.ColumnInvoiceField),
                TreatyType = TextFile.GetColValue(ItemCodeMappingBo.ColumnTreatyType),
                TreatyCode = TextFile.GetColValue(ItemCodeMappingBo.ColumnTreatyCode),
                BusinessOrigin = TextFile.GetColValue(ItemCodeMappingBo.ColumnBusinessOrigin),
                ItemCode = TextFile.GetColValue(ItemCodeMappingBo.ColumnItemCode),
                CreatedById = AuthUserId != null ? AuthUserId.Value : User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : User.DefaultSuperUserId,
            };

            string idStr = TextFile.GetColValue(ItemCodeMappingBo.ColumnId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                icm.Id = id;
            }

            return icm;
        }

        public void UpdateData(ref ItemCodeMappingBo icmDb, ItemCodeMappingBo icm)
        {
            icmDb.InvoiceFieldPickListDetailId = icm.InvoiceFieldPickListDetailId;
            icmDb.TreatyType = icm.TreatyType;
            icmDb.TreatyCode = icm.TreatyCode;
            icmDb.BusinessOriginPickListDetailId = icm.BusinessOriginPickListDetailId;
            icmDb.ItemCodeId = icm.ItemCodeId;
            icmDb.UpdatedById = icm.UpdatedById;
        }

        public void Trail(Result result, ItemCodeMappingBo rdc, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    rdc.Id,
                    string.Format("{0} Item Code Mapping", action),
                    result,
                    trail,
                    rdc.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(action);
            }
        }

        public void AddNotFoundError(ItemCodeMappingBo icm)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The Item Code Mapping ID doesn't exists: {0} at row {1}", icm.Id, TextFile.RowIndex));
        }

        public List<Column> GetColumns()
        {
            Columns = ItemCodeMappingBo.GetColumns();
            return Columns;
        }
    }
}
