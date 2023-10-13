using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.Identity;
using BusinessObject.SoaDatas;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using DataAccess.Entities.SoaDatas;
using DataAccess.EntityFramework;
using Services;
using Services.SoaDatas;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class SoaDataBatchViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ceding Company")]
        public int CedantId { get; set; }

        [Display(Name = "Ceding Company")]
        public CedantBo CedantBo { get; set; }

        public virtual Cedant Cedant { get; set; }

        [Required]
        [Display(Name = "Treaty ID")]
        public int? TreatyId { get; set; }

        [Display(Name = "Treaty ID")]
        public TreatyBo TreatyBo { get; set; }

        public virtual Treaty Treaty { get; set; }

        [Required]
        public string Quarter { get; set; }

        public int Type { get; set; }

        [Required, Display(Name = "Currency Conversion Code")]
        public int? CurrecncyCodePickListDetailId { get; set; }
        public virtual PickListDetail CurrecncyCodePickListDetail { get; set; }
        public PickListDetailBo CurrecncyCodePickListDetailBo { get; set; }


        [Display(Name = "Currency Conversion Rate")]
        public double? CurrecncyRate { get; set; }
        [Required, Display(Name = "Currency Conversion Rate")]
        public string CurrecncyRateStr { get; set; }

        public int Status { get; set; }

        [Display(Name = "Data Update Status")]
        public int DataUpdateStatus { get; set; }


        [ValidateFileUpload]
        public HttpPostedFileBase[] Upload { get; set; }

        [Display(Name = "Person In-Charge")]
        public int PersonInChargeId { get; set; }

        public UserBo PersonInChargeBo { get; set; }

        public User PersonInCharge { get; set; }

        public DateTime? StatementReceivedAt { get; set; }

        [Required, Display(Name = "Statement Received Date")]
        public string StatementReceivedAtStr { get; set; }

        [Display(Name = "Upload Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? UploadDate { get; set; }

        [RequiredIfSubmitForDataUpdate, Display(Name = "Ri Data")]
        public int? RiDataBatchId { get; set; }

        [Display(Name = "Claim Data")]
        public int? ClaimDataBatchId { get; set; }

        public int ModuleId { get; set; }

        public string Mode { get; set; }

        public int TotalMappingFailedStatus { get; set; }

        public bool IsSave { get; set; } = false;

        public bool IsUpdateData { get; set; } = false;

        public string ClaimDataStatus { get; set; }

        [Display(Name = "Direct Retro Status")]
        public int DirectRetroStatus { get; set; }
         
        [Display(Name = "Invoice Status")]
        public int InvoiceStatus { get; set; }

        public bool IsRiDataAutoCreate { get; set; }
        public bool IsClaimDataAutoCreate { get; set; }

        public bool IsProfitCommissionData { get; set; }

        public SoaDataBatchViewModel() { Set(); }

        public SoaDataBatchViewModel(SoaDataBatchBo soaDataBatchBo)
        {
            Set(soaDataBatchBo);
        }

        public void Set(SoaDataBatchBo soaDataBatchBo = null)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.SoaData.ToString());
            if (soaDataBatchBo != null)
            {
                Id = soaDataBatchBo.Id;

                CedantId = soaDataBatchBo.CedantId;
                CedantBo = soaDataBatchBo.CedantBo;

                TreatyBo = soaDataBatchBo.TreatyBo;
                TreatyId = soaDataBatchBo.TreatyId;

                Quarter = soaDataBatchBo.Quarter;
                Status = soaDataBatchBo.Status;
                DataUpdateStatus = soaDataBatchBo.DataUpdateStatus;
                Type = soaDataBatchBo.Type;

                CurrecncyCodePickListDetailId = soaDataBatchBo.CurrencyCodePickListDetailId;
                CurrecncyCodePickListDetailBo = soaDataBatchBo.CurrencyCodePickListDetailBo;

                CurrecncyRateStr = Util.DoubleToString(soaDataBatchBo.CurrencyRate);

                PersonInChargeId = soaDataBatchBo.CreatedById;
                PersonInChargeBo = soaDataBatchBo.CreatedByBo;

                StatementReceivedAtStr = soaDataBatchBo.StatementReceivedAt.ToString(Util.GetDateFormat());
                ModuleId = moduleBo.Id;
                RiDataBatchId = soaDataBatchBo.RiDataBatchId;
                ClaimDataBatchId = soaDataBatchBo.ClaimDataBatchId;

                TotalMappingFailedStatus = soaDataBatchBo.TotalMappingFailedStatus;

                if (soaDataBatchBo.ClaimDataBatchBo != null)
                    ClaimDataStatus = ClaimDataBatchBo.GetStatusName(soaDataBatchBo.ClaimDataBatchBo.Status);

                DirectRetroStatus = soaDataBatchBo.DirectStatus;
                InvoiceStatus = soaDataBatchBo.InvoiceStatus;
                IsRiDataAutoCreate = soaDataBatchBo.IsRiDataAutoCreate;
                IsClaimDataAutoCreate = soaDataBatchBo.IsClaimDataAutoCreate;
                IsProfitCommissionData = soaDataBatchBo.IsProfitCommissionData;

                var files = SoaDataFileService.GetBySoaDataBatchIdMode(soaDataBatchBo.Id, SoaDataFileBo.ModeExclude);
                if (files != null)
                    Mode = string.Join(",", SoaDataFileService.GetBySoaDataBatchIdMode(soaDataBatchBo.Id, SoaDataFileBo.ModeExclude).Select(q => q.Id).ToArray());
            }
            else
            {
                Status = SoaDataBatchBo.StatusPending;
                DataUpdateStatus = SoaDataBatchBo.DataUpdateStatusPending;
                StatementReceivedAtStr = DateTime.Now.ToString(Util.GetDateFormat());
                ModuleId = moduleBo.Id;

                var pickListDetailBo = PickListDetail.FindByPickListIdCode(PickListBo.CurrencyCode, "MYR");
                CurrecncyCodePickListDetailId = pickListDetailBo?.Id;
                CurrecncyRateStr = "1";
            }
        }

        public void Get(ref SoaDataBatchBo soaDataBatchBo)
        {
            soaDataBatchBo.Id = Id;
            soaDataBatchBo.Status = Status;
            soaDataBatchBo.CedantId = CedantId;
            soaDataBatchBo.TreatyId = TreatyId;
            soaDataBatchBo.Quarter = Quarter;
            soaDataBatchBo.CurrencyCodePickListDetailId = CurrecncyCodePickListDetailId;
            soaDataBatchBo.CurrencyRate = Util.StringToDouble(CurrecncyRateStr);
            soaDataBatchBo.StatementReceivedAt = DateTime.Parse(StatementReceivedAtStr);
            soaDataBatchBo.Type = Type;
            soaDataBatchBo.RiDataBatchId = RiDataBatchId;
            soaDataBatchBo.ClaimDataBatchId = ClaimDataBatchId;
            soaDataBatchBo.TotalMappingFailedStatus = TotalMappingFailedStatus;
            soaDataBatchBo.IsRiDataAutoCreate = IsRiDataAutoCreate;
            soaDataBatchBo.IsClaimDataAutoCreate = IsClaimDataAutoCreate;
            soaDataBatchBo.IsProfitCommissionData = IsProfitCommissionData;
        }

        public void SetBos(SoaDataBatchBo soaDataBatchBo)
        {
            Set(soaDataBatchBo);
        }

        public static Expression<Func<SoaDataBatch, SoaDataBatchViewModel>> Expression()
        {
            return entity => new SoaDataBatchViewModel
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                Cedant = entity.Cedant,
                TreatyId = entity.TreatyId,
                Treaty = entity.Treaty,
                Quarter = entity.Quarter,
                Status = entity.Status,
                DataUpdateStatus = entity.DataUpdateStatus,
                UploadDate = entity.CreatedAt,
                PersonInChargeId = entity.CreatedById,
                PersonInCharge = entity.CreatedBy,
                TotalMappingFailedStatus = entity.TotalMappingFailedStatus,
                DirectRetroStatus = entity.DirectStatus,
                InvoiceStatus = entity.InvoiceStatus,                
            };
        }

        public void ProcessFileUpload(int authUserId, ref TrailObject trail)
        {
            foreach (var uploadItem in Upload)
            {
                if (uploadItem == null)
                    continue;

                RawFileBo rawFileBo = new RawFileBo
                {
                    Type = RawFileBo.TypeSoaData,
                    Status = RawFileBo.StatusPending,
                    FileName = uploadItem.FileName,
                    CreatedById = authUserId,
                    UpdatedById = authUserId,
                };

                rawFileBo.FormatHashFileName();
                string path = rawFileBo.GetLocalPath();
                Util.MakeDir(path);
                uploadItem.SaveAs(path);

                RawFileService.Create(ref rawFileBo, ref trail);

                SoaDataFileBo soaDataFileBo = new SoaDataFileBo
                {
                    SoaDataBatchId = Id,
                    RawFileId = rawFileBo.Id,
                    TreatyId = TreatyId,
                    Mode = SoaDataFileBo.ModeInclude,
                    Status = SoaDataFileBo.StatusPending,
                    CreatedById = authUserId,
                    UpdatedById = authUserId,
                };
                SoaDataFileService.Create(ref soaDataFileBo, ref trail);
            }
        }

        public void ProcessExistingFile(string modeIds, int authUserId, ref TrailObject trail)
        {
            IList<SoaDataFileBo> soaDataFileBos = SoaDataFileService.GetBySoaDataBatchId(Id);
            if (!string.IsNullOrEmpty(modeIds))
            {
                List<int> checkExcludeIds = modeIds.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToList();

                foreach (SoaDataFileBo bo in soaDataFileBos)
                {
                    SoaDataFileBo soaDataFileBo = bo;
                    if (checkExcludeIds.Contains(bo.Id)) soaDataFileBo.Mode = SoaDataFileBo.ModeExclude;
                    else soaDataFileBo.Mode = SoaDataFileBo.ModeInclude;
                    soaDataFileBo.UpdatedById = authUserId;
                    SoaDataFileService.Update(ref soaDataFileBo, ref trail);
                }
            }
            else
            {
                foreach (SoaDataFileBo bo in soaDataFileBos)
                {
                    if (bo.Mode == SoaDataFileBo.ModeInclude)
                        continue;

                    SoaDataFileBo soaDataFileBo = bo;
                    soaDataFileBo.Mode = SoaDataFileBo.ModeInclude;
                    soaDataFileBo.UpdatedById = authUserId;
                    SoaDataFileService.Update(ref soaDataFileBo, ref trail);
                }
            }
        }

        public void ProcessStatusHistory(FormCollection form, int authUserId, ref TrailObject trail, bool dataUpdate = false)
        {
            var status = Status;
            if (dataUpdate) status = DataUpdateStatus;
            StatusHistoryBo statusHistoryBo = new StatusHistoryBo
            {
                ModuleId = ModuleId,
                ObjectId = Id,
                Status = status,
                CreatedById = authUserId,
                UpdatedById = authUserId,
            };
            StatusHistoryService.Save(ref statusHistoryBo, ref trail);

            string statusRemark = form.Get(string.Format("StatusRemark"));
            if (!string.IsNullOrWhiteSpace(statusRemark))
            {
                RemarkBo remarkBo = new RemarkBo
                {
                    ModuleId = ModuleId,
                    ObjectId = Id,
                    Status = status,
                    Content = statusRemark,
                    CreatedById = authUserId,
                    UpdatedById = authUserId,
                };

                RemarkService.Save(ref remarkBo, ref trail);
            }
        }

        public List<RemarkBo> GetRemarks(FormCollection form)
        {
            int index = 0;
            List<RemarkBo> remarkBos = new List<RemarkBo> { };

            while (!string.IsNullOrWhiteSpace(form.Get(string.Format("r.content[{0}]", index))))
            {
                string createdAtStr = form.Get(string.Format("r.createdAtStr[{0}]", index));
                string status = form.Get(string.Format("r.status[{0}]", index));
                string content = form.Get(string.Format("r.content[{0}]", index));
                string id = form.Get(string.Format("remarkId[{0}]", index));

                DateTime dateTime = DateTime.Parse(createdAtStr, new CultureInfo("fr-FR", false));

                RemarkBo remarkBo = new RemarkBo
                {
                    ModuleId = ModuleId,
                    ObjectId = Id,
                    Status = int.Parse(status),
                    Content = content,
                    CreatedAt = dateTime,
                    UpdatedAt = dateTime,
                };

                if (!string.IsNullOrEmpty(id))
                    remarkBo.Id = int.Parse(id);

                remarkBos.Add(remarkBo);

                index++;
            }

            return remarkBos;
        }

        public void ProcessRemark(FormCollection form, int authUserId, ref TrailObject trail)
        {
            List<RemarkBo> remarkBos = GetRemarks(form);

            foreach (RemarkBo bo in remarkBos)
            {
                RemarkBo remarkBo = bo;
                remarkBo.CreatedById = authUserId;
                remarkBo.UpdatedById = authUserId;

                RemarkService.Save(ref remarkBo, ref trail);
            }
        }

        public void UpdatePostValidationDifferenceSummary(FormCollection form, int authUserId, ref TrailObject trail)
        {
            int index = 0;
            while (form.AllKeys.Contains(string.Format("differencesId[{0}]", index)))
            {
                string remark = form.Get(string.Format("remark[{0}]", index));
                string check = form.Get(string.Format("check[{0}]", index));
                string id = form.Get(string.Format("differencesId[{0}]", index));

                if (!string.IsNullOrEmpty(check) || !string.IsNullOrEmpty(remark))
                {
                    SoaDataPostValidationDifferenceBo bo = SoaDataPostValidationDifferenceService.Find(int.Parse(id));
                    bo.UpdatedById = authUserId;
                    if (!string.IsNullOrEmpty(remark)) bo.Remark = remark;
                    if (!string.IsNullOrEmpty(check)) bo.Check = check;

                    SoaDataPostValidationDifferenceService.Update(ref bo, ref trail);
                }
                index++;
            }
        }

        public void UpdateCompiledSummary(FormCollection form, int authUserId, ref TrailObject trail)
        {
            // Compiled Summary: Amount 1 no need to update if it is WM/OM
            List<int> type = new List<int> { SoaDataCompiledSummaryBo.InvoiceTypeWM, SoaDataCompiledSummaryBo.InvoiceTypeOM };

            List<string> typeSummaries = new List<string> { "wm", "oc", "sf" };
            foreach(var i in typeSummaries)
            {
                int index = 0;
                while (form.AllKeys.Contains(string.Format("{0}Id[{1}]", i, index)))
                {
                    string nbPremium = form.Get(string.Format("{0}NbPremium[{1}]", i, index));
                    string rnPremium = form.Get(string.Format("{0}RnPremium[{1}]", i, index));
                    string altPremium = form.Get(string.Format("{0}AltPremium[{1}]", i, index));
                    string nbDiscount = form.Get(string.Format("{0}NbDiscount[{1}]", i, index));
                    string rnDiscount = form.Get(string.Format("{0}RnDiscount[{1}]", i, index));
                    string altDiscount = form.Get(string.Format("{0}AltDiscount[{1}]", i, index));
                    string claim = form.Get(string.Format("{0}Claim[{1}]", i, index));
                    string surrValue = form.Get(string.Format("{0}SurrenderValue[{1}]", i, index));
                    string noClaimBonus = form.Get(string.Format("{0}NoClaimBonus[{1}]", i, index));
                    string dbComm = form.Get(string.Format("{0}DatabaseCommission[{1}]", i, index));
                    string brokerageFee = form.Get(string.Format("{0}BrokerageFee[{1}]", i, index));
                    string dth = form.Get(string.Format("{0}DTH[{1}]", i, index));
                    string tpd = form.Get(string.Format("{0}TPD[{1}]", i, index));
                    string ci = form.Get(string.Format("{0}CI[{1}]", i, index));
                    string pa = form.Get(string.Format("{0}PA[{1}]", i, index));
                    string hs = form.Get(string.Format("{0}HS[{1}]", i, index));
                    string soaQuarter = form.Get(string.Format("{0}SoaQuarter[{1}]", i, index));
                    string amount1 = form.Get(string.Format("{0}Amount1[{1}]", i, index));
                    string amount2 = form.Get(string.Format("{0}Amount2[{1}]", i, index));
                    string id = form.Get(string.Format("{0}Id[{1}]", i, index));
                    string accountForCode = form.Get(string.Format("{0}AccountFor[{1}]", i, index));
                    string sttReceivedDate = form.Get(string.Format("{0}StatementRecDate[{1}]", i, index));
                    string invoiceDate1 = form.Get(string.Format("{0}InvoiceDate1[{1}]", i, index));
                    string invoiceDate2 = form.Get(string.Format("{0}InvoiceDate2[{1}]", i, index));
                    string invoiceNo1 = form.Get(string.Format("{0}InvoiceNumber1[{1}]", i, index));
                    string invoiceNo2 = form.Get(string.Format("{0}InvoiceNumber2[{1}]", i, index));
                    string reason1 = form.Get(string.Format("{0}ReasonOfAdjustment1[{1}]", i, index));
                    string reason2 = form.Get(string.Format("{0}ReasonOfAdjustment2[{1}]", i, index));

                    string contractCode = form.Get(string.Format("{0}ContractCode[{1}]", i, index));
                    string annualCohort = form.Get(string.Format("{0}AnnualCohort[{1}]", i, index));
                    string riskPrem = form.Get(string.Format("{0}RiskPremium[{1}]", i, index));
                    string profitComm = form.Get(string.Format("{0}ProfitComm[{1}]", i, index));
                    string levy = form.Get(string.Format("{0}Levy[{1}]", i, index));
                    string modcoReserveIncome = form.Get(string.Format("{0}ModcoReserveIncome[{1}]", i, index));
                    string riDeposit = form.Get(string.Format("{0}RiDeposit[{1}]", i, index));
                    string administrationContribution = form.Get(string.Format("{0}AdministrationContribution[{1}]", i, index));
                    string shareOfRiCommissionFromCompulsoryCession = form.Get(string.Format("{0}ShareOfRiCommissionFromCompulsoryCession[{1}]", i, index));
                    string recaptureFee = form.Get(string.Format("{0}RecaptureFee[{1}]", i, index));
                    string creditCardCharges = form.Get(string.Format("{0}CreditCardCharges[{1}]", i, index));


                    SoaDataCompiledSummaryBo cs = SoaDataCompiledSummaryService.Find(int.Parse(id));
                    cs.NbPremium = Util.StringToDouble(nbPremium);
                    cs.RnPremium = Util.StringToDouble(rnPremium);
                    cs.AltPremium = Util.StringToDouble(altPremium);
                    cs.NbDiscount = Util.StringToDouble(nbDiscount);
                    cs.RnDiscount = Util.StringToDouble(rnDiscount);
                    cs.AltDiscount = Util.StringToDouble(altDiscount);
                    cs.Claim = Util.StringToDouble(claim);
                    cs.SurrenderValue = Util.StringToDouble(surrValue);
                    cs.NoClaimBonus = Util.StringToDouble(noClaimBonus);
                    cs.DatabaseCommission = Util.StringToDouble(dbComm);
                    cs.BrokerageFee = Util.StringToDouble(brokerageFee);
                    cs.DTH = Util.StringToDouble(dth);
                    cs.TPD = Util.StringToDouble(tpd);
                    cs.CI = Util.StringToDouble(ci);
                    cs.PA = Util.StringToDouble(pa);
                    cs.HS = Util.StringToDouble(hs);
                    if (!type.Contains(cs.InvoiceType)) cs.Amount1 = Util.StringToDouble(amount1);
                    cs.Amount2 = Util.StringToDouble(amount2);
                    cs.UpdatedById = authUserId;
                    cs.SoaQuarter = string.IsNullOrEmpty(soaQuarter) ? null : soaQuarter;
                    cs.AccountDescription = string.IsNullOrEmpty(accountForCode) ? null : accountForCode;
                    cs.InvoiceNumber1 = string.IsNullOrEmpty(invoiceNo1) ? null : invoiceNo1;
                    cs.InvoiceNumber2 = string.IsNullOrEmpty(invoiceNo2) ? null : invoiceNo2;
                    cs.ReasonOfAdjustment1 = string.IsNullOrEmpty(reason1) ? null : reason1;
                    cs.ReasonOfAdjustment2 = string.IsNullOrEmpty(reason2) ? null : reason2;
                    cs.StatementReceivedDate = string.IsNullOrEmpty(sttReceivedDate) ? (DateTime?)null : Util.GetParseDateTime(sttReceivedDate);
                    cs.InvoiceDate1 = string.IsNullOrEmpty(invoiceDate1) ? (DateTime?)null : Util.GetParseDateTime(invoiceDate1);
                    cs.InvoiceDate2 = string.IsNullOrEmpty(invoiceDate2) ? (DateTime?)null : Util.GetParseDateTime(invoiceDate2);

                    if (cs.ReportingType == SoaDataCompiledSummaryBo.ReportingTypeIFRS17)
                    {
                        cs.ContractCode = string.IsNullOrEmpty(contractCode) ? null : contractCode;
                        cs.AnnualCohort = string.IsNullOrEmpty(annualCohort) ? (int?)null : int.Parse(annualCohort);
                        cs.RiskPremium = Util.StringToDouble(riskPrem);
                        cs.ProfitComm = Util.StringToDouble(profitComm);
                        cs.Levy = Util.StringToDouble(levy);
                        cs.ModcoReserveIncome = Util.StringToDouble(modcoReserveIncome);
                        cs.RiDeposit = Util.StringToDouble(riDeposit);
                        cs.AdministrationContribution = Util.StringToDouble(administrationContribution);
                        cs.ShareOfRiCommissionFromCompulsoryCession = Util.StringToDouble(shareOfRiCommissionFromCompulsoryCession);
                        cs.RecaptureFee = Util.StringToDouble(recaptureFee);
                        cs.CreditCardCharges = Util.StringToDouble(creditCardCharges);
                    }

                    cs.GetNetTotalAmount();
                    SoaDataCompiledSummaryService.Update(ref cs, ref trail);
                    index++;
                }
            }
        }

        public void UpdateClaimOffsetStatus(int authUserId)
        {
            if (ClaimDataBatchId.HasValue)
            {                
                IList<ClaimRegisterBo> claimRegisterBos = ClaimRegisterService.GetByClaimDataBatchId(ClaimDataBatchId.Value);
                if (claimRegisterBos.Count > 0)
                {
                    using (var db = new AppDbContext())
                    {
                        db.Database.ExecuteSqlCommand("UPDATE [ClaimRegister] SET [OffsetStatus] = {0}, [UpdatedById] = {1}, [UpdatedAt] = {2} WHERE [ClaimDataBatchId] = {3}", 
                            ClaimRegisterBo.OffsetStatusPendingInvoicing, authUserId, DateTime.Now, ClaimDataBatchId);
                        db.SaveChanges();
                    }
                }
            }
        }
    }

    public class ValidateFileUpload : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            long maxContent = Math.Abs((long)(Convert.ToInt32(Util.GetConfig("RiDataMaxFileSize")) * Math.Pow(1024, 3)));
            //long maxContent = Convert.ToInt64("2147483648");

            var rawFileModel = validationContext.ObjectInstance as SoaDataBatchViewModel;
            if ((rawFileModel.IsRiDataAutoCreate || rawFileModel.IsClaimDataAutoCreate) && (rawFileModel.Upload == null || rawFileModel.Upload[0] == null))
                return null;

            if (rawFileModel.Status == SoaDataBatchBo.StatusSubmitForProcessing)
            {
                var fileHistories = SoaDataFileService.GetBySoaDataBatchId(rawFileModel.Id);
                string[] checkExclude = (string.IsNullOrEmpty(rawFileModel.Mode) ? new string[0] : rawFileModel.Mode.Split(','));

                if (fileHistories.Count() == checkExclude.Length && (rawFileModel.Upload == null || rawFileModel.Upload[0] == null))
                {
                    return new ValidationResult("Please upload at least one file or include excluded files");
                }
            }

            if (rawFileModel.Upload != null)
            {
                foreach (var item in rawFileModel.Upload)
                {
                    if (item != null)
                    {
                        if (ValidateFileMimeType(item) == false)
                        {
                            return new ValidationResult("Allowed file of type: .xls, .xlsx");
                        }
                        else if (item.ContentLength > maxContent)
                        {
                            return new ValidationResult("Maximum allowed size is : " + Util.GetConfig("RiDataMaxFileSize") + "GB");
                        }
                        else
                            return null;
                    }
                }
            }
            return null;
        }

        public static bool ValidateFileMimeType(HttpPostedFileBase file)
        {
            var excel = new string[]
            {
                "application/vnd.ms-excel",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            };

            var mime = file.ContentType;

            if (Path.GetExtension(file.FileName) != ".csv" && excel.Contains(mime))
                return true;
            
            return false;
        }
    }

    public class RequiredIfSubmitForDataUpdate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var Model = validationContext.ObjectInstance as SoaDataBatchViewModel;
            if (Model.DataUpdateStatus == SoaDataBatchBo.DataUpdateStatusSubmitForDataUpdate && Model.IsUpdateData)
            {
                if (Model.RiDataBatchId == null && Model.ClaimDataBatchId == null)
                {
                    return new ValidationResult(string.Format(MessageBag.Required, "Ri Data"));
                }
            }
            return null;
        }
    }
}