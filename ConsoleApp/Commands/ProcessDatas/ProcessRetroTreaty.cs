using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.Retrocession;
using DataAccess.Entities.Identity;
using Services;
using Services.Identity;
using Services.Retrocession;
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
    public class ProcessRetroTreaty : Command
    {
        public List<Column> Columns { get; set; }

        public string FilePath { get; set; }

        public HttpPostedFileBase PostedFile { get; set; }

        public int? AuthUserId { get; set; }

        public TextFile TextFile { get; set; }

        public List<string> Errors { get; set; }

        public List<string> UpdateAction { get; set; } = new List<string> { "u", "update" };

        public List<string> DeleteAction { get; set; } = new List<string> { "d", "del", "delete" };

        public char[] charsToTrim = { ',', '.', ' ' };

        public bool IsChild { get; set; } = false;

        public ProcessRetroTreaty()
        {
            Title = "ProcessRetroTreaty";
            Description = "To read Retro Treaty Table csv file and insert into database";
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

                RetroTreatyBo rt = null;
                RetroTreatyDetailBo rtd = null;
                try
                {
                    rt = SetParentData();
                    rtd = SetChildData();

                    if (!string.IsNullOrEmpty(rt.RetroPartyParty))
                    {
                        var retroPartyBo = RetroPartyService.FindByParty(rt.RetroPartyParty);
                        if (retroPartyBo != null)
                        {
                            rt.RetroPartyId = retroPartyBo.Id;
                            rt.RetroPartyBo = retroPartyBo;
                        }
                        else
                        {
                            SetProcessCount("Retro Party Not Found");
                            Errors.Add(string.Format("The Reinsurance Retro Party doesn't exists: {0} at row {1}", rt.RetroPartyParty, TextFile.RowIndex));
                            error = true;
                        }
                    }
                    else
                    {
                        SetProcessCount("Retro Party Empty");
                        Errors.Add(string.Format("Please enter the Retro Party at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    var statusStr = TextFile.GetColValue(RetroTreatyBo.ColumnStatus);
                    if (string.IsNullOrEmpty(statusStr))
                    {
                        SetProcessCount("Status Empty");
                        Errors.Add(string.Format("Please enter the Status at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                    else
                    {
                        var status = RetroTreatyBo.GetStatusKey(statusStr);
                        if (status == 0)
                        {
                            SetProcessCount("Status Invalid");
                            Errors.Add(string.Format("Please enter valid Status (Active/Inactive) at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            rt.Status = status;
                        }
                    }

                    if (string.IsNullOrEmpty(rt.Code))
                    {
                        SetProcessCount("Code Empty");
                        Errors.Add(string.Format("Please enter the Code at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    if (!string.IsNullOrEmpty(rt.TreatyType))
                    {
                        PickListDetailBo tt = PickListDetailService.FindByPickListIdCode(PickListBo.TreatyType, rt.TreatyType);
                        if (tt == null)
                        {
                            SetProcessCount("Treaty Type Not Found");
                            Errors.Add(string.Format("The Treaty Type doesn't exists: {0} at row {1}", rt.TreatyType, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            rt.TreatyTypePickListDetailId = tt.Id;
                            rt.TreatyTypePickListDetailBo = tt;
                        }
                    }
                    else
                    {
                        SetProcessCount("Traety Type Empty");
                        Errors.Add(string.Format("Please enter the Treaty Type at row {0}", TextFile.RowIndex));
                        error = true;
                    }

                    string lineOfBusinessStr = TextFile.GetColValue(RetroTreatyBo.ColumnLineOfBusiness);
                    if (!string.IsNullOrEmpty(lineOfBusinessStr))
                    {
                        lineOfBusinessStr = lineOfBusinessStr.TrimEnd(charsToTrim);
                        var lineOfBusiness = Util.ToArraySplitTrim(lineOfBusinessStr);

                        rt.IsLobAutomatic = lineOfBusiness.Contains("AUTO");
                        rt.IsLobFacultative = lineOfBusiness.Contains("FAC");
                        rt.IsLobAdvantageProgram = lineOfBusiness.Contains("AP");
                    }

                    var retroShareStr = TextFile.GetColValue(RetroTreatyBo.ColumnRetroShare);
                    if (string.IsNullOrEmpty(retroShareStr))
                    {
                        SetProcessCount("Retro Share Empty");
                        Errors.Add(string.Format("Please enter the Retro Share at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                    else
                    {
                        if (double.TryParse(retroShareStr, out double retroShare))
                        {
                            rt.RetroShare = retroShare;
                        }
                        else
                        {
                            SetProcessCount("Retro Share Invalid");
                            Errors.Add(string.Format("The Retro Share is invalid: {0} at row {1}", retroShareStr, TextFile.RowIndex));
                            error = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(rt.TreatyDiscountRule))
                    {
                        var treatyDiscountTableBo = TreatyDiscountTableService.FindByRule(rt.TreatyDiscountRule);
                        if (treatyDiscountTableBo == null)
                        {
                            SetProcessCount("Treaty Discount Table Not Found");
                            Errors.Add(string.Format("The Treaty Discount Table doesn't exists: {0} at row {1}", rt.TreatyType, TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            rt.TreatyDiscountTableId = treatyDiscountTableBo.Id;
                            rt.TreatyDiscountTableBo = treatyDiscountTableBo;
                        }
                    }

                    var effectiveStartDateStr = TextFile.GetColValue(RetroTreatyBo.ColumnEffectiveStartDate);
                    if (string.IsNullOrEmpty(effectiveStartDateStr))
                    {
                        SetProcessCount("Effective Start Date Empty");
                        Errors.Add(string.Format("Please enter the Effective Start Date at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                    else
                    {
                        if (Util.TryParseDateTime(effectiveStartDateStr, out DateTime? datetime, out string errorMsg))
                        {
                            rt.EffectiveStartDate = datetime.Value;
                        }
                        else
                        {
                            SetProcessCount("Effective Start Date Error");
                            Errors.Add(string.Format("The Effective Start Date format can not be read: {0} at row {1}", effectiveStartDateStr, TextFile.RowIndex));
                            error = true;
                        }
                    }

                    var effectiveEndDateStr = TextFile.GetColValue(RetroTreatyBo.ColumnEffectiveEndDate);
                    if (string.IsNullOrEmpty(effectiveEndDateStr))
                    {
                        SetProcessCount("Effective End Date Empty");
                        Errors.Add(string.Format("Please enter the Effective End Date at row {0}", TextFile.RowIndex));
                        error = true;
                    }
                    else
                    {
                        if (Util.TryParseDateTime(effectiveEndDateStr, out DateTime? datetime, out string errorMsg))
                        {
                            rt.EffectiveEndDate = datetime.Value;
                        }
                        else
                        {
                            SetProcessCount("Effective End Date Error");
                            Errors.Add(string.Format("The Effective End Date format can not be read: {0} at row {1}", effectiveEndDateStr, TextFile.RowIndex));
                            error = true;
                        }
                    }

                    // Child
                    var configEffectiveStartDateStr = TextFile.GetColValue(RetroTreatyBo.ColumnDetailConfigEffectiveStartDate);
                    var configEffectiveEndDateStr = TextFile.GetColValue(RetroTreatyBo.ColumnDetailConfigEffectiveEndDate);
                    var configRiskQuarterStartDateStr = TextFile.GetColValue(RetroTreatyBo.ColumnDetailConfigRiskQuarterStartDate);
                    var configRiskQuarterEndDateStr = TextFile.GetColValue(RetroTreatyBo.ColumnDetailConfigRiskQuarterEndDate);
                    var IstoAggregateStr = TextFile.GetColValue(RetroTreatyBo.ColumnDetailConfigIsToAggregate);

                    IsChild = rtd.Id != 0 ||
                        !string.IsNullOrEmpty(rtd.ConfigTreatyCode) ||
                        !string.IsNullOrEmpty(rtd.ConfigTreatyType) ||
                        !string.IsNullOrEmpty(rtd.ConfigFundsAccountingType) ||
                        !string.IsNullOrEmpty(configEffectiveStartDateStr) ||
                        !string.IsNullOrEmpty(configEffectiveEndDateStr) ||
                        !string.IsNullOrEmpty(configRiskQuarterStartDateStr) ||
                        !string.IsNullOrEmpty(configRiskQuarterEndDateStr) ||
                        !string.IsNullOrEmpty(IstoAggregateStr) ||
                        !string.IsNullOrEmpty(rtd.ConfigRemark) ||
                        !string.IsNullOrEmpty(rtd.PremiumSpreadRule) ||
                        !string.IsNullOrEmpty(rtd.TreatyDiscountRule) ||
                        !string.IsNullOrEmpty(rtd.MlreShareStr) ||
                        !string.IsNullOrEmpty(rtd.GrossRetroPremium) ||
                        !string.IsNullOrEmpty(rtd.TreatyDiscount) ||
                        !string.IsNullOrEmpty(rtd.NetRetroPremium) ||
                        !string.IsNullOrEmpty(rtd.Remark);

                    // Validate child if have child
                    if (IsChild)
                    {
                        if (!string.IsNullOrEmpty(rtd.ConfigTreatyCode))
                        {
                            var treatyCodeBo = TreatyCodeService.FindByCode(rtd.ConfigTreatyCode);

                            if (treatyCodeBo == null)
                            {
                                SetProcessCount("Config's Treaty Code Not Found");
                                Errors.Add(string.Format("The Config's Treaty Code doesn't exists: {0} at row {1}", rtd.ConfigTreatyCode, TextFile.RowIndex));
                                error = true;
                            }
                            else
                            {
                                rtd.ConfigTreatyCodeId = treatyCodeBo.Id;
                                rtd.ConfigTreatyCodeBo = treatyCodeBo;
                            }
                        }
                        else
                        {
                            SetProcessCount("Config's Treaty Code Empty");
                            Errors.Add(string.Format("Please enter the Config's Treaty Code at row {0}", TextFile.RowIndex));
                            error = true;
                        }

                        if (!string.IsNullOrEmpty(rtd.ConfigTreatyType))
                        {
                            PickListDetailBo tt = PickListDetailService.FindByPickListIdCode(PickListBo.TreatyType, rtd.ConfigTreatyType);
                            if (tt == null)
                            {
                                SetProcessCount("Treaty Type Not Found");
                                Errors.Add(string.Format("The config's Treaty Type doesn't exists: {0} at row {1}", rtd.ConfigTreatyType, TextFile.RowIndex));
                                error = true;
                            }
                            else
                            {
                                rtd.ConfigTreatyTypeId = tt.Id;
                                rtd.ConfigTreatyTypeBo = tt;
                            }
                        }
                        else
                        {
                            SetProcessCount("Config's Treaty Type Empty");
                            Errors.Add(string.Format("Please enter the Config's Treaty Type at row {0}", TextFile.RowIndex));
                            error = true;
                        }

                        if (!string.IsNullOrEmpty(rtd.ConfigFundsAccountingType))
                        {
                            PickListDetailBo fat = PickListDetailService.FindByPickListIdCode(PickListBo.FundsAccountingType, rtd.ConfigFundsAccountingType);
                            if (fat == null)
                            {
                                SetProcessCount("Config's Funds Accounting Type Not Found");
                                Errors.Add(string.Format("The Config's Funds Accounting Type doesn't exists: {0} at row {1}", rtd.ConfigFundsAccountingType, TextFile.RowIndex));
                                error = true;
                            }
                            else
                            {
                                rtd.ConfigFundsAccountingTypeId = fat.Id;
                                rtd.ConfigFundsAccountingTypeBo = fat;
                            }
                        }
                        else
                        {
                            SetProcessCount("Config's Funds Accounting Type Empty");
                            Errors.Add(string.Format("Please enter the Config's Funds Accounting Type at row {0}", TextFile.RowIndex));
                            error = true;
                        }

                        if (string.IsNullOrEmpty(configEffectiveStartDateStr))
                        {
                            SetProcessCount("Config's Effective Start Date Empty");
                            Errors.Add(string.Format("Please enter the Config's Effective Start Date at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            if (Util.TryParseDateTime(configEffectiveStartDateStr, out DateTime? datetime, out string errorMsg))
                            {
                                rtd.ConfigEffectiveStartDate = datetime.Value;
                            }
                            else
                            {
                                SetProcessCount("Config's Effective Start Date Error");
                                Errors.Add(string.Format("The Config's Effective Start Date format can not be read: {0} at row {1}", configEffectiveStartDateStr, TextFile.RowIndex));
                                error = true;
                            }
                        }

                        if (string.IsNullOrEmpty(configEffectiveEndDateStr))
                        {
                            SetProcessCount("Config's Effective End Date Empty");
                            Errors.Add(string.Format("Please enter the Config's Effective End Date at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            if (Util.TryParseDateTime(configEffectiveEndDateStr, out DateTime? datetime, out string errorMsg))
                            {
                                rtd.ConfigEffectiveEndDate = datetime.Value;
                            }
                            else
                            {
                                SetProcessCount("Config's Effective End Date Error");
                                Errors.Add(string.Format("The Config's Effective End Date format can not be read: {0} at row {1}", configEffectiveEndDateStr, TextFile.RowIndex));
                                error = true;
                            }
                        }

                        if (string.IsNullOrEmpty(configRiskQuarterStartDateStr))
                        {
                            SetProcessCount("Config's Risk Quarter Start Date Empty");
                            Errors.Add(string.Format("Please enter the Config's Risk Quarter Start Date at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            if (Util.TryParseDateTime(configRiskQuarterStartDateStr, out DateTime? datetime, out string errorMsg))
                            {
                                rtd.ConfigRiskQuarterStartDate = datetime.Value;
                            }
                            else
                            {
                                SetProcessCount("Config's Risk Quarter Start Date Error");
                                Errors.Add(string.Format("The Config's Risk Quarter Start Date format can not be read: {0} at row {1}", configRiskQuarterStartDateStr, TextFile.RowIndex));
                                error = true;
                            }
                        }

                        if (string.IsNullOrEmpty(configRiskQuarterEndDateStr))
                        {
                            SetProcessCount("Config's Risk Quarter End Date Empty");
                            Errors.Add(string.Format("Please enter the Config's Risk Quarter End Date at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            if (Util.TryParseDateTime(configRiskQuarterEndDateStr, out DateTime? datetime, out string errorMsg))
                            {
                                rtd.ConfigRiskQuarterEndDate = datetime.Value;
                            }
                            else
                            {
                                SetProcessCount("Config's Risk Quarter End Date Error");
                                Errors.Add(string.Format("The Config's Risk Quarter End Date format can not be read: {0} at row {1}", configRiskQuarterEndDateStr, TextFile.RowIndex));
                                error = true;
                            }
                        }

                        if (string.IsNullOrEmpty(IstoAggregateStr))
                        {
                            SetProcessCount("Config's To Aggregate Empty");
                            Errors.Add(string.Format("Please enter the Config's To Aggregate at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                        else
                        {
                            if (Util.StringToBool(IstoAggregateStr, out bool bl))
                            {
                                rtd.ConfigIsToAggregate = bl;
                            }
                            else
                            {
                                SetProcessCount("Config's To Aggregate Invalid");
                                Errors.Add(string.Format("Please enter valid Config's To Aggregate (Y/N) at row {0}", TextFile.RowIndex));
                                error = true;
                            }
                        }

                        if (!string.IsNullOrEmpty(rtd.PremiumSpreadRule))
                        {
                            var premiumSpreadTableBo = PremiumSpreadTableService.FindByRule(rtd.PremiumSpreadRule);
                            if (premiumSpreadTableBo == null)
                            {
                                SetProcessCount("Detail's Premium Spread Table Not Found");
                                Errors.Add(string.Format("The Detail's Premium Spread Table doesn't exists: {0} at row {1}", rtd.PremiumSpreadRule, TextFile.RowIndex));
                                error = true;
                            }
                            else
                            {
                                rtd.PremiumSpreadTableId = premiumSpreadTableBo.Id;
                                rtd.PremiumSpreadTableBo = premiumSpreadTableBo;
                            }
                        }

                        if (!string.IsNullOrEmpty(rtd.TreatyDiscountRule))
                        {
                            var treatyDiscountTableBo = TreatyDiscountTableService.FindByRule(rtd.TreatyDiscountRule);
                            if (treatyDiscountTableBo == null)
                            {
                                SetProcessCount("Detail's Treaty Discount Table Not Found");
                                Errors.Add(string.Format("The Detail's Treaty Discount Table doesn't exists: {0} at row {1}", rtd.TreatyDiscountRule, TextFile.RowIndex));
                                error = true;
                            }
                            else
                            {
                                rtd.TreatyDiscountTableId = treatyDiscountTableBo.Id;
                                rtd.TreatyDiscountTableBo = treatyDiscountTableBo;
                            }
                        }

                        //!string.IsNullOrEmpty(rtd.GrossRetroPremium) ||
                        //!string.IsNullOrEmpty(rtd.TreatyDiscount) ||
                        //!string.IsNullOrEmpty(rtd.NetRetroPremium) ||
                        //!string.IsNullOrEmpty(rtd.Remark);

                        if (!string.IsNullOrEmpty(rtd.MlreShareStr))
                        {
                            if (double.TryParse(rtd.MlreShareStr, out double mlreShare))
                            {
                                rtd.MlreShare = mlreShare;
                            }
                            else
                            {
                                SetProcessCount("Detail's MLRe Share Invalid");
                                Errors.Add(string.Format("The Detail's MLRe Share is invalid: {0} at row {1}", rtd.MlreShareStr, TextFile.RowIndex));
                                error = true;
                            }
                        }
                        else
                        {
                            SetProcessCount("Detail's MLRe Share Empty");
                            Errors.Add(string.Format("Please enter the Detail's MLRe Share at row {0}", TextFile.RowIndex));
                            error = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    SetProcessCount("Error");
                    Errors.Add(string.Format("Error Triggered: {0} at row {1}", e.Message, TextFile.RowIndex));
                    error = true;
                }
                string parentAction = TextFile.GetColValue(RetroTreatyBo.ColumnAction);
                string childAction = TextFile.GetColValue(RetroTreatyBo.ColumnDetailAction);

                if (error)
                {
                    continue;
                }

                // Parent
                RetroTreatyBo retroTreatyBo;
                if (rt.Id != 0)
                {
                    retroTreatyBo = RetroTreatyService.Find(rt.Id);
                }
                else
                {
                    retroTreatyBo = RetroTreatyService.FindByParams(rt);
                    rt.Id = retroTreatyBo?.Id ?? 0;
                }

                if (UpdateAction.Contains(parentAction.ToLower().Trim()))
                {
                    // Handle Update
                    if (IsChild || !string.IsNullOrEmpty(childAction))
                    {
                        SetProcessCount("Child Data Not Allowed");
                        Errors.Add(string.Format("Unable to process Row {0} due to child data exist in Parent's Update Action", TextFile.RowIndex));
                        continue;
                    }

                    if (retroTreatyBo == null)
                    {
                        AddParentNotFoundError(rt);
                        continue;
                    }

                    // Date Range
                    var errorMsg = ValidateParentRange(rt);
                    if (!string.IsNullOrEmpty(errorMsg))
                    {
                        SetProcessCount("Range Error");
                        Errors.Add(string.Format(errorMsg + " at row {0}", TextFile.RowIndex));
                        continue;
                    }

                    UpdateParentData(ref retroTreatyBo, rt);

                    trail = new TrailObject();
                    result = RetroTreatyService.Update(ref retroTreatyBo, ref trail);

                    if (!result.Valid)
                    {
                        foreach (var e in result.ToErrorArray())
                        {
                            Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                        }
                        error = true;
                    }
                    if (error)
                    {
                        continue;
                    }

                    TrailParent(result, retroTreatyBo, trail, "Update");

                    continue;
                }
                else if (DeleteAction.Contains(parentAction.ToLower().Trim()))
                {
                    // Handle Delete
                    if (IsChild || !string.IsNullOrEmpty(childAction))
                    {
                        SetProcessCount("Child Data Not Allowed");
                        Errors.Add(string.Format("Unable to process Row {0} due to child data exist in Parent's Delete Action", TextFile.RowIndex));
                        continue;
                    }

                    if (retroTreatyBo == null)
                    {
                        AddParentNotFoundError(rt);
                        continue;
                    }

                    trail = new TrailObject();
                    result = RetroTreatyService.Delete(retroTreatyBo, ref trail);

                    if (!result.Valid)
                    {
                        foreach (var e in result.ToErrorArray())
                        {
                            Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                        }
                        continue;
                    }

                    TrailParent(result, retroTreatyBo, trail, "Delete");

                    continue;
                }
                else
                {
                    // Handle Create
                    if (retroTreatyBo == null)
                    {
                        // Date Range
                        var errorMsg = ValidateParentRange(rt);
                        if (!string.IsNullOrEmpty(errorMsg))
                        {
                            SetProcessCount("Range Error");
                            Errors.Add(string.Format(errorMsg + " at row {0}", TextFile.RowIndex));
                            continue;
                        }

                        trail = new TrailObject();
                        result = RetroTreatyService.Create(ref rt, ref trail);
                        if (result.Valid)
                        {
                            retroTreatyBo = rt;
                            TrailParent(result, rt, trail, "Create");
                        }
                        else
                        {
                            Errors.Add(string.Format("Unable to process Row {0} due to error occured during create Retro Treaty", TextFile.RowIndex));
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
                            RetroTreatyDetailBo rtdDb = RetroTreatyDetailService.Find(rtd.Id);
                            if (rtdDb == null)
                            {
                                AddChildNotFoundError(rtd);
                                continue;
                            }

                            // Search Config
                            if (!searchTreatyConfig(ref rtd))
                            {
                                SetProcessCount("Per Life Retro Treaty Configuration Not Found");
                                Errors.Add(string.Format("Per Life Retro Treaty Configuration Not Found with the parameters at row {0}", TextFile.RowIndex));
                                continue;
                            }

                            // Duplicate
                            if (IsChildDuplicate(retroTreatyBo, rtd))
                            {
                                SetProcessCount("Existing Child Record Found");
                                Errors.Add(string.Format("The Retro Treaty Detail exists with same Per Life Retro Treaty Configuration at row {0}", TextFile.RowIndex));
                                continue;
                            }

                            rtd.RetroTreatyId = retroTreatyBo.Id;
                            UpdateChildData(ref rtdDb, rtd);

                            trail = new TrailObject();
                            result = RetroTreatyDetailService.Update(ref rtdDb, ref trail);

                            if (!result.Valid)
                            {
                                foreach (var e in result.ToErrorArray())
                                {
                                    Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                                }
                                continue;
                            }

                            TrailChild(result, rtdDb, trail, "Update");

                            break;
                        case "d":
                        case "del":
                        case "delete":
                            rtdDb = RetroTreatyDetailService.Find(rtd.Id);
                            if (rtdDb == null)
                            {
                                AddChildNotFoundError(rtd);
                                continue;
                            }

                            trail = new TrailObject();
                            result = RetroTreatyDetailService.Delete(rtd, ref trail);

                            if (!result.Valid)
                            {
                                foreach (var e in result.ToErrorArray())
                                {
                                    Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                                }
                                continue;
                            }

                            TrailChild(result, rtd, trail, "Delete");

                            break;
                        default:
                            if (rt.Id != 0 && RetroTreatyDetailService.IsExists(rtd.Id))
                            {
                                SetProcessCount("Child Record Found");
                                Errors.Add(string.Format("The Retro Treaty Detail ID exists: {0} at row {1}", rtd.Id, TextFile.RowIndex));
                                continue;
                            }

                            if (rt.Id == 0)
                                continue;

                            // Search Config
                            if (!searchTreatyConfig(ref rtd))
                            {
                                SetProcessCount("Per Life Retro Treaty Configuration Not Found");
                                Errors.Add(string.Format("Per Life Retro Treaty Configuration Not Found with the parameters at row {0}", TextFile.RowIndex));
                                continue;
                            }

                            // Duplicate
                            if (IsChildDuplicate(retroTreatyBo, rtd))
                            {
                                SetProcessCount("Existing Child Record Found");
                                Errors.Add(string.Format("The Retro Treaty Detail exists with same Per Life Retro Treaty Configuration at row {0}", TextFile.RowIndex));
                                continue;
                            }

                            trail = new TrailObject();
                            rtd.RetroTreatyId = retroTreatyBo.Id;
                            result = RetroTreatyDetailService.Create(ref rtd, ref trail);

                            if (!result.Valid)
                            {
                                foreach (var e in result.ToErrorArray())
                                {
                                    Errors.Add(string.Format("{0} at row {1}", e, TextFile.RowIndex));
                                }
                                continue;
                            }

                            TrailChild(result, rtd, trail, "Create");
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

        public RetroTreatyBo SetParentData()
        {
            var rt = new RetroTreatyBo
            {
                Id = 0,
                RetroPartyParty = TextFile.GetColValue(RetroTreatyBo.ColumnRetroPartyParty),
                //Status = TextFile.GetColValue(RetroTreatyBo.ColumnStatus),
                Code = TextFile.GetColValue(RetroTreatyBo.ColumnCode),
                TreatyType = TextFile.GetColValue(RetroTreatyBo.ColumnTreatyType),
                //LineOfBusiness = TextFile.GetColValue(RetroTreatyBo.ColumnLineOfBusiness),
                //RetroShare = TextFile.GetColValue(RetroTreatyBo.ColumnRetroShare),
                TreatyDiscountRule = TextFile.GetColValue(RetroTreatyBo.ColumnTreatyDiscountRule),
                //EffectiveStartDate = TextFile.GetColValue(RetroTreatyBo.ColumnEffectiveStartDate),
                //EffectiveEndDate = TextFile.GetColValue(RetroTreatyBo.ColumnEffectiveEndDate),
                CreatedById = AuthUserId != null ? AuthUserId.Value : User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : User.DefaultSuperUserId,
            };

            rt.RetroPartyParty = rt.RetroPartyParty?.Trim();
            rt.Code = rt.Code?.Trim();
            rt.TreatyType = rt.TreatyType?.Trim();
            rt.TreatyDiscountRule = rt.TreatyDiscountRule?.Trim();

            string idStr = TextFile.GetColValue(RetroTreatyBo.ColumnId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                rt.Id = id;
            }

            return rt;
        }

        public RetroTreatyDetailBo SetChildData()
        {
            var rtd = new RetroTreatyDetailBo
            {
                Id = 0,

                // Retro Config
                ConfigTreatyCode = TextFile.GetColValue(RetroTreatyBo.ColumnDetailConfigTreatyCode),
                ConfigTreatyType = TextFile.GetColValue(RetroTreatyBo.ColumnDetailConfigTreatyType),
                ConfigFundsAccountingType = TextFile.GetColValue(RetroTreatyBo.ColumnDetailConfigFundsAccountingType),
                //ConfigEffectiveStartDate = TextFile.GetColValue(RetroTreatyBo.ColumnDetailConfigEffectiveStartDate),
                //ConfigEffectiveEndDate = TextFile.GetColValue(RetroTreatyBo.ColumnDetailConfigEffectiveEndDate),
                //ConfigRiskQuarterStartDate = TextFile.GetColValue(RetroTreatyBo.ColumnDetailConfigRiskQuarterStartDate),
                //ConfigRiskQuarterEndDate = TextFile.GetColValue(RetroTreatyBo.ColumnDetailConfigRiskQuarterEndDate),
                //ConfigIsToAggregate = TextFile.GetColValue(RetroTreatyBo.ColumnDetailConfigIsToAggregate),
                ConfigRemark = TextFile.GetColValue(RetroTreatyBo.ColumnDetailConfigRemark), // Just for check child

                // Detail
                PremiumSpreadRule = TextFile.GetColValue(RetroTreatyBo.ColumnDetailPremiumSpreadRule),
                TreatyDiscountRule = TextFile.GetColValue(RetroTreatyBo.ColumnDetailTreatyDiscountRule),
                MlreShareStr = TextFile.GetColValue(RetroTreatyBo.ColumnDetailMlreShare),
                GrossRetroPremium = TextFile.GetColValue(RetroTreatyBo.ColumnDetailGrossRetroPremium),
                TreatyDiscount = TextFile.GetColValue(RetroTreatyBo.ColumnDetailTreatyDiscount),
                NetRetroPremium = TextFile.GetColValue(RetroTreatyBo.ColumnDetailNetRetroPremium),
                Remark = TextFile.GetColValue(RetroTreatyBo.ColumnDetailRemark),

                CreatedById = AuthUserId != null ? AuthUserId.Value : User.DefaultSuperUserId,
                UpdatedById = AuthUserId != null ? AuthUserId : User.DefaultSuperUserId,
            };

            rtd.ConfigTreatyCode = rtd.ConfigTreatyCode?.Trim();
            rtd.ConfigTreatyType = rtd.ConfigTreatyType?.Trim();
            rtd.ConfigFundsAccountingType = rtd.ConfigFundsAccountingType?.Trim();

            rtd.PremiumSpreadRule = rtd.PremiumSpreadRule?.Trim();
            rtd.TreatyDiscountRule = rtd.TreatyDiscountRule?.Trim();
            rtd.GrossRetroPremium = rtd.GrossRetroPremium?.Trim();
            rtd.TreatyDiscount = rtd.TreatyDiscount?.Trim();
            rtd.NetRetroPremium = rtd.NetRetroPremium?.Trim();
            rtd.Remark = rtd.Remark?.Trim();

            string idStr = TextFile.GetColValue(RetroTreatyBo.ColumnDetailId);
            if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
            {
                rtd.Id = id;
            }

            return rtd;
        }

        public void UpdateParentData(ref RetroTreatyBo rtDb, RetroTreatyBo rt)
        {
            rtDb.RetroPartyId = rt.RetroPartyId;
            rtDb.Status = rt.Status;
            rtDb.Code = rt.Code;
            rtDb.TreatyTypePickListDetailId = rt.TreatyTypePickListDetailId;
            rtDb.IsLobAutomatic = rt.IsLobAutomatic;
            rtDb.IsLobFacultative = rt.IsLobFacultative;
            rtDb.IsLobAdvantageProgram = rt.IsLobAdvantageProgram;
            rtDb.RetroShare = rt.RetroShare;
            rtDb.TreatyDiscountTableId = rt.TreatyDiscountTableId;
            rtDb.EffectiveStartDate = rt.EffectiveStartDate;
            rtDb.EffectiveEndDate = rt.EffectiveEndDate;
            rtDb.UpdatedById = rt.UpdatedById;
        }

        public void UpdateChildData(ref RetroTreatyDetailBo rtdDb, RetroTreatyDetailBo rtd)
        {
            rtdDb.RetroTreatyId = rtd.RetroTreatyId;
            rtdDb.PerLifeRetroConfigurationTreatyId = rtd.PerLifeRetroConfigurationTreatyId;
            rtdDb.PremiumSpreadTableId = rtd.PremiumSpreadTableId;
            rtdDb.TreatyDiscountTableId = rtd.TreatyDiscountTableId;
            rtdDb.MlreShare = rtd.MlreShare;
            rtdDb.GrossRetroPremium = rtd.GrossRetroPremium;
            rtdDb.TreatyDiscount = rtd.TreatyDiscount;
            rtdDb.NetRetroPremium = rtd.NetRetroPremium;
            rtdDb.Remark = rtd.Remark;
            rtdDb.UpdatedById = rtd.UpdatedById;
        }

        public void TrailParent(Result result, RetroTreatyBo rt, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    rt.Id,
                    string.Format("{0} Retro Treaty", action),
                    result,
                    trail,
                    rt.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(string.Format("{0} Parent", action));
            }
        }

        public void TrailChild(Result result, RetroTreatyDetailBo rtd, TrailObject trail, string action)
        {
            if (result.Valid)
            {
                UserTrailBo userTrailBo = new UserTrailBo(
                    rtd.Id,
                    string.Format("{0} Retro Treaty Detail", action),
                    result,
                    trail,
                    rtd.UpdatedById.Value
                );
                UserTrailService.Create(ref userTrailBo);

                SetProcessCount(string.Format("{0} Child", action));
            }
        }

        public bool ValidateDateTimeFormat(int type, ref RetroTreatyBo rt)
        {
            string header = Columns.Where(m => m.ColIndex == type).Select(m => m.Header).FirstOrDefault();
            string property = Columns.Where(m => m.ColIndex == type).Select(m => m.Property).FirstOrDefault();

            bool valid = true;
            string value = TextFile.GetColValue(type);
            if (!string.IsNullOrEmpty(value))
            {
                if (Util.TryParseDateTime(value, out DateTime? datetime, out string error))
                {
                    rt.SetPropertyValue(property, datetime.Value);
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

        public void AddParentNotFoundError(RetroTreatyBo rt)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The Retro Treaty ID doesn't exists: {0} at row {1}", rt.Id, TextFile.RowIndex));
        }

        public void AddChildNotFoundError(RetroTreatyDetailBo rtd)
        {
            SetProcessCount("Record Not Found");
            Errors.Add(string.Format("The Retro Treaty Detail ID doesn't exists: {0} at row {1}", rtd.Id, TextFile.RowIndex));
        }

        public List<Column> GetColumns()
        {
            Columns = PremiumSpreadTableBo.GetColumns();
            return Columns;
        }

        // Parent have range only - Ignore the config's date (query by param only)
        public string ValidateParentRange(RetroTreatyBo rt)
        {
            string error = null;

            if (rt.EffectiveStartDate.HasValue && !rt.EffectiveEndDate.HasValue)
            {
                SetProcessCount("Effective End Date Required");
                error = string.Format(MessageBag.Required, "Effective End Date");
            }
            else if (!rt.EffectiveStartDate.HasValue && rt.EffectiveEndDate.HasValue)
            {
                SetProcessCount("Effective Start Date Required");
                error = string.Format(MessageBag.Required, "Effective Start Date");
            }
            else if (rt.EffectiveStartDate.HasValue && rt.EffectiveEndDate.HasValue && rt.EffectiveEndDate < rt.EffectiveStartDate)
            {
                SetProcessCount("Effective End Date Greater or Equal to Effective Start Date");
                error = string.Format(MessageBag.GreaterOrEqualTo, "Effective End Date", "Effective Start Date");
            }

            return error;
        }

        public bool searchTreatyConfig(ref RetroTreatyDetailBo rtd)
        {
            PerLifeRetroConfigurationTreatyBo perLifeRetroConfigurationTreatyBo = new PerLifeRetroConfigurationTreatyBo
            {
                TreatyCodeId = rtd.ConfigTreatyCodeId,
                TreatyTypePickListDetailId = rtd.ConfigTreatyTypeId,
                FundsAccountingTypePickListDetailId = rtd.ConfigFundsAccountingTypeId,
                ReinsEffectiveStartDate = rtd.ConfigEffectiveStartDate,
                ReinsEffectiveEndDate = rtd.ConfigEffectiveEndDate,
                RiskQuarterStartDate = rtd.ConfigRiskQuarterStartDate,
                RiskQuarterEndDate = rtd.ConfigRiskQuarterEndDate,
                IsToAggregate = rtd.ConfigIsToAggregate,
            };

            var bo = PerLifeRetroConfigurationTreatyService.FindByParam(perLifeRetroConfigurationTreatyBo);

            if (bo != null)
            {
                rtd.PerLifeRetroConfigurationTreatyId = bo.Id;
                return true;
            }

            return false;
        }

        public bool IsChildDuplicate(RetroTreatyBo retroTreatyBo, RetroTreatyDetailBo rtd)
        {
            if (RetroTreatyDetailService.CountByRetroTreatyIdExcludeId(retroTreatyBo.Id, rtd.Id) > 0)
            {
                return true;
            }

            return false;
        }
    }
}
