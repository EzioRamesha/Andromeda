using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using DataAccess.Entities.TreatyPricing;
using Newtonsoft.Json;
using Services;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class TreatyPricingGroupReferralViewModel : ObjectVersion, IValidatableObject
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        public int GRCedantId { get; set; }
        public virtual Cedant Cedant { get; set; }

        [Required, DisplayName("Group Referral ID")]
        public string Code { get; set; }

        [Required, DisplayName("Group Referral Description")]
        public string Description { get; set; }

        [Required, DisplayName("Insured Group Name")]
        public int InsuredGroupId { get; set; }
        public virtual InsuredGroupName InsuredGroupName { get; set; }

        [DisplayName("Industry Name")]
        public int? IndustryNameId { get; set; }
        public virtual PickListDetail IndustryNamePickListDetail { get; set; }

        [DisplayName("Policy Number")]
        public string GRPolicyNumber { get; set; }

        [DisplayName("First Referral Date")]
        public DateTime? FirstReferralDate { get; set; }
        [DisplayName("First Referral Date")]
        public string FirstReferralDateStr { get; set; }

        [DisplayName("Coverage Start Date")]
        public DateTime? CoverageStartDate { get; set; }
        [DisplayName("Coverage Start Date")]
        public string CoverageStartDateStr { get; set; }

        [DisplayName("Coverage End Date")]
        public DateTime? CoverageEndDate { get; set; }
        [DisplayName("Coverage End Date")]
        public string CoverageEndDateStr { get; set; }

        [Required, DisplayName("Referred Type")]
        public int? ReferredTypeId { get; set; }
        public virtual PickListDetail ReferredTypePickListDetail { get; set; }

        public int Status { get; set; }

        public int? WorkflowStatus { get; set; }
        public string WorkflowStatusName { get; set; }

        public int? RiArrangementPickListDetailId { get; set; }

        public string QuotationnName { get; set; }

        // General
        public int PrimaryTreatyPricingProductId { get; set; }
        public int PrimaryTreatyPricingProductVersionId { get; set; }
        public int PrimaryTreatyPricingProductVersion { get; set; }
        public string PrimaryTreatyPricingProductSelect { get; set; }
        public string PrimaryTreatyPricingProductCode { get; set; }
        public string PrimaryTreatyPricingProductName { get; set; }
        public TreatyPricingProductBo PrimaryTreatyPricingProductBo { get; set; }
        public TreatyPricingProductVersionBo PrimaryTreatyPricingProductVersionBo { get; set; }

        public int? SecondaryTreatyPricingProductId { get; set; }
        public int? SecondaryTreatyPricingProductVersionId { get; set; }
        public int SecondaryTreatyPricingProductVersion { get; set; }
        public string SecondaryTreatyPricingProductSelect { get; set; }
        public string SecondaryTreatyPricingProductCode { get; set; }
        public string SecondaryTreatyPricingProductName { get; set; }
        public virtual TreatyPricingProductBo SecondaryTreatyPricingProductBo { get; set; }
        public virtual TreatyPricingProductVersionBo SecondaryTreatyPricingProductVersionBo { get; set; }

        [DisplayName("Current Quotation TAT")]
        public int? QuotationTAT { get; set; }

        [DisplayName("Current Internal TAT")]
        public int? InternalTAT { get; set; }

        [DisplayName("Current Quotation Validity Date")]
        public DateTime? QuotationValidityDate { get; set; }
        public string QuotationValidityDateStr { get; set; }

        [DisplayName("Quotation Validity Day")]
        public string QuotationValidityDay { get; set; }

        [DisplayName("First Quotation Sent Week")]
        public int? FirstQuotationSentWeek { get; set; }
        [DisplayName("First Quotation Sent Month")]
        public int? FirstQuotationSentMonth { get; set; }
        [DisplayName("First Quotation Sent Quarter")]
        public string FirstQuotationSentQuarter { get; set; }
        [DisplayName("First Quotation Sent Year")]
        public int? FirstQuotationSentYear { get; set; }

        [DisplayName("Won Version")]
        public string WonVersion { get; set; }

        [DisplayName("RI Group Slip")]
        public bool HasRiGroupSlip { get; set; }
        [DisplayName("RI Group Slip ID")]
        public string RiGroupSlipIdCode { get; set; }
        [DisplayName("RI Group Slip Status")]
        public int? RiGroupSlipStatus { get; set; }
        [DisplayName("Person In-Charge")]
        public int? RiGroupSlipPersonInChargeId { get; set; }
        public virtual User RiGroupSlipPersonInCharge { get; set; }
        [DisplayName("RI Group Slip Confirmation Date")]
        public DateTime? RiGroupSlipConfirmationDate { get; set; }
        public string RiGroupSlipConfirmationDateStr { get; set; }

        [DisplayName("RI Group Slip Version")]
        public int? RiGroupSlipVersionId { get; set; }

        [DisplayName("RI Group Slip Template")]
        public int? RiGroupSlipTemplateId { get; set; }

        [DisplayName("SharePoint Link")]
        public string RiGroupSlipSharePointLink { get; set; }
        [DisplayName("SharePoint Folder Path")]
        public string RiGroupSlipSharePointFolderPath { get; set; }

        public int? GroupMasterLetterId { get; set; }
        [DisplayName("Group Master Letter ID")]
        public string GroupMasterLetterCode { get; set; }
        public virtual TreatyPricingGroupMasterLetter GroupMasterLetter { get; set; }

        // Version

        [DisplayName("Version")]
        public int Version { get; set; }

        [Required, DisplayName("Group Referral Person In-Charge")]
        public int? GroupReferralPersonInChargeId { get; set; }
        public virtual User GroupReferralPersonInCharge { get; set; }

        [Required, DisplayName("Cedant's Person In-Charge")]
        public string CedantPersonInCharge { get; set; }

        [Required, DisplayName("Type of Request")]
        public int? RequestTypePickListDetailId { get; set; }
        public virtual PickListDetail RequestTypePickListDetail { get; set; }

        [Required, DisplayName("Type of Premium")]
        public int? PremiumTypePickListDetailId { get; set; }

        [DisplayName("Expected Annual Gross / Risk Premium")]
        public double? GrossRiskPremium { get; set; }
        [DisplayName("Expected Annual Gross / Risk Premium")]
        public string GrossRiskPremiumStr { get; set; }

        [DisplayName("Expected Annual Reinsurance Premium")]
        public double? ReinsurancePremium { get; set; }
        [DisplayName("Expected Annual Reinsurance Premium")]
        public string ReinsurancePremiumStr { get; set; }

        [DisplayName("Expected Annual Gross / Risk Premium (GTL)")]
        public double? GrossRiskPremiumGTL { get; set; }
        [DisplayName("Expected Annual Gross / Risk Premium (GTL)")]
        public string GrossRiskPremiumGTLStr { get; set; }

        [DisplayName("Expected Annual Reinsurance Premium (GTL)")]
        public double? ReinsurancePremiumGTL { get; set; }
        public string ReinsurancePremiumGTLStr { get; set; }

        [DisplayName("Expected Annual Gross / Risk Premium (GHS)")]
        public double? GrossRiskPremiumGHS { get; set; }
        [DisplayName("Expected Annual Gross / Risk Premium (GHS)")]
        public string GrossRiskPremiumGHSStr { get; set; }

        [DisplayName("Expected Annual Reinsurance Premium (GHS)")]
        public double? ReinsurancePremiumGHS { get; set; }
        [DisplayName("Expected Annual Reinsurance Premium (GHS)")]
        public string ReinsurancePremiumGHSStr { get; set; }

        [DisplayName("Expected Average Sum Assured")]
        public double? AverageSumAssured { get; set; }
        [Required, DisplayName("Expected Average Sum Assured")]
        public string AverageSumAssuredStr { get; set; }

        [DisplayName("Expected Group Size")]
        public double? GroupSize { get; set; }
        [Required, DisplayName("Expected Group Size")]
        public string GroupSizeStr { get; set; }

        [DisplayName("Compulsory / Voluntary")]
        public int IsCompulsoryOrVoluntary { get; set; }

        [Required, DisplayName("Underwriting Method")]
        public string UnderwritingMethod { get; set; }

        [DisplayName("Group Referral Remarks")]
        public string Remarks { get; set; }

        [DisplayName("Request Received Date")]
        public DateTime? RequestReceivedDate { get; set; }
        [Required, DisplayName("Request Received Date")]
        public string RequestReceivedDateStr { get; set; }

        [DisplayName("Enquiry to Client Date")]
        public DateTime? EnquiryToClientDate { get; set; }
        public string EnquiryToClientDateStr { get; set; }

        [DisplayName("Client Reply Date")]
        public DateTime? ClientReplyDate { get; set; }
        public string ClientReplyDateStr { get; set; }

        [DisplayName("Quotation Sent Date")]
        public DateTime? QuotationSentDate { get; set; }
        [DisplayName("Quotation Sent Date")]
        public string QuotationSentDateStr { get; set; }

        public int? Score { get; set; }

        public double? AverageScore { get; set; }

        [DisplayName("Declined Risk")]
        public string DeclinedRisk { get; set; }

        [DisplayName("Referred Risk")]
        public string ReferredRisk { get; set; }

        [DisplayName("Per Life Retro")]
        public bool HasPerLifeRetro { get; set; }

        // Remark
        [DisplayName("Quotation Folder")]
        public string QuotationFolder { get; set; }

        [DisplayName("Group Referral Version")]
        public int? ReplyVersionId { get; set; }

        [DisplayName("Reply Template")]
        public int? ReplyTemplateId { get; set; }

        [DisplayName("SharePoint Link")]
        public string ReplySharePointLink { get; set; }
        public string ReplySharePointFolderPath { get; set; }

        [DisplayName("Version")]
        public int? ChecklistVersionId { get; set; }
        public string Checklists { get; set; }
        public string ChecklistDetails { get; set; }
        public string ChecklistRemark { get; set; }
        public bool ChecklistPendingUnderwriting { get; set; }
        public bool ChecklistPendingHealth { get; set; }
        public bool ChecklistPendingClaims { get; set; }
        public bool ChecklistPendingBD { get; set; }
        public bool ChecklistPendingCR { get; set; }
        //public string Benefits { get; set; }
        public string TreatyPricingGroupReferralVersionBenefit { get; set; }

        public int? ActiveTab { get; set; }

        //DropdownList product -> to retrieve product dropdown in group referral add new
        public int TreatyPricingProductId { get; set; }
        public int TreatyPricingProductVersionId { get; set; }
        public int VersionNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductQuotationnName { get; set; }

        //DropdownList uwLimit -> to retrieve UW limit dropdown in group referral edit
        public int TreatyPricingUwLimitId { get; set; }
        public int TreatyPricingUwLimitVersionId { get; set; }
        public string UwLimitId { get; set; }
        public string UwLimitName { get; set; }

        //DropdownList referral version -> to retrieve group referral version dropdown in group referral edit
        public int TreatyPricingGroupReferralVersionId { get; set; }

        public List<TreatyPricingGroupReferralVersionBenefitBo> TreatyPricingGroupReferralVersionBenefitBos { get; set; }

        public TreatyPricingGroupReferralViewModel()
        {
            Set();
        }

        public TreatyPricingGroupReferralViewModel(TreatyPricingGroupReferralBo groupReferralBo)
        {
            Set(groupReferralBo);
            SetVersionObjects(groupReferralBo.TreatyPricingGroupReferralVersionBos);

            //PersonInChargeId = int.Parse(CurrentVersionObject.GetPropertyValue("PersonInChargeId").ToString());
            ModuleId = ModuleService.FindByController(ModuleBo.ModuleController.TreatyPricingGroupReferral.ToString()).Id;
        }

        public void Set(TreatyPricingGroupReferralBo bo = null)
        {
            if (bo == null)
                return;

            Id = bo.Id;
            GRCedantId = bo.CedantId;
            Code = bo.Code;
            Description = bo.Description;
            RiArrangementPickListDetailId = bo.RiArrangementPickListDetailId;
            InsuredGroupId = bo.InsuredGroupNameId;
            Status = bo.Status;
            WorkflowStatus = bo.WorkflowStatus;
            WorkflowStatusName = bo.WorkflowStatus.HasValue ? TreatyPricingGroupReferralBo.GetWorkflowStatusName(bo.WorkflowStatus.Value) : "";
            PrimaryTreatyPricingProductSelect = bo.PrimaryTreatyPricingProductSelect;
            SecondaryTreatyPricingProductSelect = bo.SecondaryTreatyPricingProductSelect;
            GRPolicyNumber = bo.PolicyNumber;
            FirstReferralDate = bo.FirstReferralDate;
            CoverageStartDate = bo.CoverageStartDate;
            CoverageEndDate = bo.CoverageEndDate;
            IndustryNameId = bo.IndustryNamePickListDetailId;
            ReferredTypeId = bo.ReferredTypePickListDetailId;
            WonVersion = bo.WonVersion;
            HasRiGroupSlip = bo.HasRiGroupSlip;
            RiGroupSlipIdCode = bo.RiGroupSlipCode;
            RiGroupSlipStatus = bo.RiGroupSlipStatus;
            RiGroupSlipPersonInChargeId = bo.RiGroupSlipPersonInChargeId;
            RiGroupSlipConfirmationDate = bo.RiGroupSlipConfirmationDate;
            RiGroupSlipVersionId = bo.RiGroupSlipVersionId;
            RiGroupSlipTemplateId = bo.RiGroupSlipTemplateId;
            RiGroupSlipSharePointLink = bo.RiGroupSlipSharePointLink;
            RiGroupSlipSharePointFolderPath = bo.RiGroupSlipSharePointFolderPath;
            QuotationFolder = bo.QuotationPath;
            ReplyVersionId = bo.ReplyVersionId;
            ReplyTemplateId = bo.ReplyTemplateId;
            ReplySharePointLink = bo.ReplySharePointLink;
            ReplySharePointFolderPath = bo.ReplySharePointFolderPath;

            FirstReferralDateStr = bo.FirstReferralDateStr;
            CoverageStartDateStr = bo.CoverageStartDateStr;
            CoverageEndDateStr = bo.CoverageEndDateStr;
            RiGroupSlipConfirmationDateStr = bo.RiGroupSlipConfirmationDateStr;

            PrimaryTreatyPricingProductId = bo.PrimaryTreatyPricingProductId;
            PrimaryTreatyPricingProductVersionId = bo.PrimaryTreatyPricingProductVersionId;
            //PrimaryTreatyPricingProductBo = bo.PrimaryTreatyPricingProductBo;

            SecondaryTreatyPricingProductId = bo.SecondaryTreatyPricingProductId;
            SecondaryTreatyPricingProductVersionId = bo.SecondaryTreatyPricingProductVersionId;
            //SecondaryTreatyPricingProductBo = bo.SecondaryTreatyPricingProductBo;

            GroupMasterLetterId = bo.TreatyPricingGroupMasterLetterId;
            if (bo.TreatyPricingGroupMasterLetterBo != null)
                GroupMasterLetterCode = bo.TreatyPricingGroupMasterLetterBo?.Code;

            //if (bo.PrimaryTreatyPricingProductBo != null)
            //    QuotationnName = bo.PrimaryTreatyPricingProductBo?.QuotationName;
        }

        public static Expression<Func<TreatyPricingGroupReferralVersion, TreatyPricingGroupReferralViewModel>> Expression()
        {
            return entity => new TreatyPricingGroupReferralViewModel
            {
                Id = entity.TreatyPricingGroupReferralId,
                Code = entity.TreatyPricingGroupReferral.Code,
                Description = entity.TreatyPricingGroupReferral.Description,
                GRCedantId = entity.TreatyPricingGroupReferral.CedantId,
                Cedant = entity.TreatyPricingGroupReferral.Cedant,
                Status = entity.TreatyPricingGroupReferral.Status,
                InsuredGroupId = entity.TreatyPricingGroupReferral.InsuredGroupNameId,
                InsuredGroupName = entity.TreatyPricingGroupReferral.InsuredGroupName,
                IndustryNameId = entity.TreatyPricingGroupReferral.IndustryNamePickListDetailId,
                IndustryNamePickListDetail = entity.TreatyPricingGroupReferral.IndustryNamePickListDetail,
                ReferredTypeId = entity.TreatyPricingGroupReferral.ReferredTypePickListDetailId,
                ReferredTypePickListDetail = entity.TreatyPricingGroupReferral.ReferredTypePickListDetail,

                RequestTypePickListDetailId = entity.RequestTypePickListDetailId,
                RequestTypePickListDetail = entity.RequestTypePickListDetail,
                GroupSize = entity.GroupSize,
                GrossRiskPremium = entity.GrossRiskPremium,
                ReinsurancePremium = entity.ReinsurancePremium,
                Version = entity.Version,
                GroupReferralPersonInChargeId = entity.GroupReferralPersonInChargeId,
                GroupReferralPersonInCharge = entity.GroupReferralPIC,
                QuotationTAT = entity.QuotationTAT,
                InternalTAT = entity.InternalTAT,
                AverageScore = entity.TreatyPricingGroupReferral.AverageScore,
                WorkflowStatus = entity.TreatyPricingGroupReferral.WorkflowStatus,
                QuotationSentDate = entity.QuotationSentDate,
                RequestReceivedDate = entity.RequestReceivedDate,
                // Checklist

                FirstReferralDate = entity.TreatyPricingGroupReferral.FirstReferralDate,
                CoverageStartDate = entity.TreatyPricingGroupReferral.CoverageStartDate,
                CoverageEndDate = entity.TreatyPricingGroupReferral.CoverageEndDate,

                HasRiGroupSlip = entity.TreatyPricingGroupReferral.HasRiGroupSlip,
                WonVersion = entity.TreatyPricingGroupReferral.WonVersion,
                RiGroupSlipPersonInChargeId = entity.TreatyPricingGroupReferral.RiGroupSlipPersonInChargeId,
                RiGroupSlipPersonInCharge = entity.TreatyPricingGroupReferral.RiGroupSlipPersonInCharge,
                RiGroupSlipConfirmationDate = entity.TreatyPricingGroupReferral.RiGroupSlipConfirmationDate,
                RiGroupSlipStatus = entity.TreatyPricingGroupReferral.RiGroupSlipStatus,
                RiGroupSlipIdCode = entity.TreatyPricingGroupReferral.RiGroupSlipCode,

                GroupMasterLetterId = entity.TreatyPricingGroupReferral.TreatyPricingGroupMasterLetterId,
                GroupMasterLetter = entity.TreatyPricingGroupReferral.TreatyPricingGroupMasterLetter,

                ChecklistPendingUnderwriting = entity.ChecklistPendingUnderwriting,
                ChecklistPendingHealth = entity.ChecklistPendingHealth,
                ChecklistPendingClaims = entity.ChecklistPendingClaims,
                ChecklistPendingBD = entity.ChecklistPendingBD,
                ChecklistPendingCR = entity.ChecklistPendingCR,
            };
        }

        public TreatyPricingGroupReferralBo FormBo(int createdById, int updatedById)
        {
            return new TreatyPricingGroupReferralBo
            {
                Id = Id,
                CedantId = GRCedantId,
                Code = Code,
                Description = Description,
                RiArrangementPickListDetailId = RiArrangementPickListDetailId,
                InsuredGroupNameId = InsuredGroupId,
                Status = Status,
                WorkflowStatus = WorkflowStatus,
                PrimaryTreatyPricingProductSelect = PrimaryTreatyPricingProductSelect,
                SecondaryTreatyPricingProductSelect = SecondaryTreatyPricingProductSelect,
                PolicyNumber = GRPolicyNumber,
                FirstReferralDate = Util.GetParseDateTime(FirstReferralDateStr),
                CoverageStartDate = Util.GetParseDateTime(CoverageStartDateStr),
                CoverageEndDate = Util.GetParseDateTime(CoverageEndDateStr),
                IndustryNamePickListDetailId = IndustryNameId,
                ReferredTypePickListDetailId = ReferredTypeId,
                WonVersion = WonVersion,
                HasRiGroupSlip = HasRiGroupSlip,
                RiGroupSlipCode = RiGroupSlipIdCode,
                RiGroupSlipStatus = RiGroupSlipStatus,
                RiGroupSlipPersonInChargeId = RiGroupSlipPersonInChargeId,
                RiGroupSlipConfirmationDate = Util.GetParseDateTime(RiGroupSlipConfirmationDateStr),
                RiGroupSlipVersionId = RiGroupSlipVersionId,
                RiGroupSlipTemplateId = RiGroupSlipTemplateId,
                RiGroupSlipSharePointLink = RiGroupSlipSharePointLink,
                RiGroupSlipSharePointFolderPath = RiGroupSlipSharePointFolderPath,
                QuotationPath = QuotationFolder,
                ReplyVersionId = ReplyVersionId,
                ReplyTemplateId = ReplyTemplateId,
                ReplySharePointLink = ReplySharePointLink,
                ReplySharePointFolderPath = ReplySharePointFolderPath,

                FirstReferralDateStr = FirstReferralDateStr,
                CoverageStartDateStr = CoverageStartDateStr,
                CoverageEndDateStr = CoverageEndDateStr,
                RiGroupSlipConfirmationDateStr = RiGroupSlipConfirmationDateStr,

                PrimaryTreatyPricingProductId = PrimaryTreatyPricingProductId,
                PrimaryTreatyPricingProductVersionId = PrimaryTreatyPricingProductVersionId,
                SecondaryTreatyPricingProductId = SecondaryTreatyPricingProductId,
                SecondaryTreatyPricingProductVersionId = SecondaryTreatyPricingProductVersionId,
                TreatyPricingGroupMasterLetterId = GroupMasterLetterId,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public TreatyPricingGroupReferralVersionBo GetVersionBo(TreatyPricingGroupReferralVersionBo bo)
        {
            //bo.Version = Version;
            bo.GroupReferralPersonInChargeId = GroupReferralPersonInChargeId;
            bo.CedantPersonInCharge = CedantPersonInCharge;
            bo.RequestTypePickListDetailId = RequestTypePickListDetailId;
            bo.PremiumTypePickListDetailId = PremiumTypePickListDetailId;
            bo.GrossRiskPremium = Util.StringToDouble(GrossRiskPremiumStr);
            bo.GrossRiskPremiumStr = GrossRiskPremiumStr;
            bo.ReinsurancePremium = Util.StringToDouble(ReinsurancePremiumStr);
            bo.ReinsurancePremiumStr = ReinsurancePremiumStr;
            bo.GrossRiskPremiumGTL = Util.StringToDouble(GrossRiskPremiumGTLStr);
            bo.GrossRiskPremiumGTLStr = GrossRiskPremiumGTLStr;
            bo.ReinsurancePremiumGTL = Util.StringToDouble(ReinsurancePremiumGTLStr);
            bo.ReinsurancePremiumGTLStr = ReinsurancePremiumGTLStr;
            bo.GrossRiskPremiumGHS = Util.StringToDouble(GrossRiskPremiumGHSStr);
            bo.GrossRiskPremiumGHSStr = GrossRiskPremiumGHSStr;
            bo.ReinsurancePremiumGHS = Util.StringToDouble(ReinsurancePremiumGHSStr);
            bo.ReinsurancePremiumGHSStr = ReinsurancePremiumGHSStr;
            bo.AverageSumAssured = Util.StringToDouble(AverageSumAssuredStr);
            bo.AverageSumAssuredStr = AverageSumAssuredStr;
            bo.GroupSize = Util.StringToDouble(GroupSizeStr);
            bo.GroupSizeStr = GroupSizeStr;
            bo.IsCompulsoryOrVoluntary = IsCompulsoryOrVoluntary;
            bo.UnderwritingMethod = UnderwritingMethod;
            bo.Remarks = Remarks;
            bo.RequestReceivedDateStr = RequestReceivedDateStr;
            bo.RequestReceivedDate = Util.GetParseDateTime(RequestReceivedDateStr);
            bo.EnquiryToClientDateStr = EnquiryToClientDateStr;
            bo.EnquiryToClientDate = Util.GetParseDateTime(EnquiryToClientDateStr);
            bo.ClientReplyDateStr = ClientReplyDateStr;
            bo.ClientReplyDate = Util.GetParseDateTime(ClientReplyDateStr);
            bo.QuotationSentDateStr = QuotationSentDateStr;
            bo.QuotationSentDate = Util.GetParseDateTime(QuotationSentDateStr);
            bo.Score = Score;
            bo.HasPerLifeRetro = HasPerLifeRetro;
            bo.ChecklistRemark = ChecklistRemark;
            bo.QuotationTAT = QuotationTAT;
            bo.InternalTAT = InternalTAT;
            bo.QuotationValidityDateStr = QuotationValidityDateStr;
            bo.QuotationValidityDate = QuotationValidityDate;
            bo.QuotationValidityDay = QuotationValidityDay;
            bo.FirstQuotationSentWeek = FirstQuotationSentWeek;
            bo.FirstQuotationSentMonth = FirstQuotationSentMonth;
            bo.FirstQuotationSentQuarter = FirstQuotationSentQuarter;
            bo.FirstQuotationSentYear = FirstQuotationSentYear;
            bo.TreatyPricingGroupReferralVersionBenefit = TreatyPricingGroupReferralVersionBenefit;

            return bo;
        }

        public string GetBenefits(string Benefits, FormCollection form)
        {
            if (!string.IsNullOrEmpty(Benefits))
            {
                int index = 0;
                var bos = JsonConvert.DeserializeObject<List<TreatyPricingGroupReferralVersionBenefitBo>>(Benefits);

                while (!string.IsNullOrWhiteSpace(form.Get(string.Format("benefitId[{0}]", index))))
                {
                    string groupLimitNonCI = form.Get(string.Format("groupLimitNonCI[{0}]", index));
                    string groupLimitCI = form.Get(string.Format("groupLimitCI[{0}]", index));
                    string groupProfitCommission = form.Get(string.Format("groupProfitCommission[{0}]", index));

                    bos[index].GroupFreeCoverLimitNonCI = groupLimitNonCI;
                    bos[index].GroupFreeCoverLimitCI = groupLimitCI;
                    bos[index].GroupProfitCommission = groupProfitCommission;

                    index++;
                }

                TreatyPricingGroupReferralVersionBenefit = JsonConvert.SerializeObject(bos);
            }
            return TreatyPricingGroupReferralVersionBenefit;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            DateTime? start = Util.GetParseDateTime(CoverageStartDateStr);
            DateTime? end = Util.GetParseDateTime(CoverageEndDateStr);

            if (start == null && end != null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "Coverage Start Date Field"),
                    new[] { nameof(CoverageStartDateStr) }));
            }
            else if (start != null && end == null)
            {
                results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "Coverage End Date Field"),
                    new[] { nameof(CoverageEndDateStr) }));
            }
            else if (start != null && end != null)
            {
                if (end <= start)
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.StartDateEarlier, "Coverage Start Date"),
                    new[] { nameof(CoverageStartDateStr) }));
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.EndDateLater, "Coverage End Date"),
                    new[] { nameof(CoverageEndDateStr) }));
                }
            }

            if (FirstQuotationSentYear.HasValue)
            {
                if (!DateTime.TryParseExact(FirstQuotationSentYear.ToString(), "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    results.Add(new ValidationResult(
                    string.Format("The value '{0}' is not a valid year", FirstQuotationSentYear),
                    new[] { nameof(FirstQuotationSentYear) }));
                }
            }

            //if (!string.IsNullOrEmpty(GrossRiskPremium))
            //{
            //    if (!Util.IsValidDouble(GrossRiskPremium, out double? d, out _))
            //    {
            //        results.Add(new ValidationResult(
            //        string.Format("The value '{0}' is not valid amount", GrossRiskPremium),
            //        new[] { nameof(GrossRiskPremium) }));
            //    }
            //}

            //if (!string.IsNullOrEmpty(ReinsurancePremium))
            //{
            //    if (!Util.IsValidDouble(ReinsurancePremium, out double? d, out _))
            //    {
            //        results.Add(new ValidationResult(
            //        string.Format("The value '{0}' is not valid amount", ReinsurancePremium),
            //        new[] { nameof(ReinsurancePremium) }));
            //    }
            //}

            //if (!string.IsNullOrEmpty(GrossRiskPremiumGTL))
            //{
            //    if (!Util.IsValidDouble(GrossRiskPremiumGTL, out double? d, out _))
            //    {
            //        results.Add(new ValidationResult(
            //        string.Format("The value '{0}' is not valid amount", GrossRiskPremiumGTL),
            //        new[] { nameof(GrossRiskPremiumGTL) }));
            //    }
            //}

            //if (!string.IsNullOrEmpty(ReinsurancePremiumGTL))
            //{
            //    if (!Util.IsValidDouble(ReinsurancePremiumGTL, out double? d, out _))
            //    {
            //        results.Add(new ValidationResult(
            //        string.Format("The value '{0}' is not valid amount", ReinsurancePremiumGTL),
            //        new[] { nameof(ReinsurancePremiumGTL) }));
            //    }
            //}

            //if (!string.IsNullOrEmpty(GrossRiskPremiumGHS))
            //{
            //    if (!Util.IsValidDouble(GrossRiskPremiumGHS, out double? d, out _))
            //    {
            //        results.Add(new ValidationResult(
            //        string.Format("The value '{0}' is not valid amount", GrossRiskPremiumGHS),
            //        new[] { nameof(GrossRiskPremiumGHS) }));
            //    }
            //}

            //if (!string.IsNullOrEmpty(ReinsurancePremiumGHS))
            //{
            //    if (!Util.IsValidDouble(ReinsurancePremiumGHS, out double? d, out _))
            //    {
            //        results.Add(new ValidationResult(
            //        string.Format("The value '{0}' is not valid amount", ReinsurancePremiumGHS),
            //        new[] { nameof(ReinsurancePremiumGHS) }));
            //    }
            //}

            //if (!string.IsNullOrEmpty(AverageSumAssured))
            //{
            //    if (!Util.IsValidDouble(AverageSumAssured, out double? d, out _))
            //    {
            //        results.Add(new ValidationResult(
            //        string.Format("The value '{0}' is not valid amount", AverageSumAssured),
            //        new[] { nameof(AverageSumAssured) }));
            //    }
            //}

            if (HasRiGroupSlip && Status == TreatyPricingGroupReferralBo.StatusWon)
            {
                if (!RiGroupSlipVersionId.HasValue)
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "RI Group Slip Version"),
                    new[] { nameof(RiGroupSlipVersionId) }));
                }

                if (!RiGroupSlipTemplateId.HasValue)
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "RI Group Slip Template"),
                    new[] { nameof(RiGroupSlipTemplateId) }));
                }
            }

            if (!string.IsNullOrEmpty(QuotationSentDateStr))
            {
                if (string.IsNullOrEmpty(GrossRiskPremiumStr))
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "Expected Annual Gross / Risk Premium"),
                    new[] { nameof(GrossRiskPremiumStr) }));
                }

                if (string.IsNullOrEmpty(ReinsurancePremiumStr))
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.Required, "Expected Annual Reinsurance Premium"),
                    new[] { nameof(ReinsurancePremiumStr) }));
                }
            }

            return results;
        }

        public static Expression<Func<TreatyPricingProductVersion, TreatyPricingGroupReferralViewModel>> ExpressionProducts()
        {
            return entity => new TreatyPricingGroupReferralViewModel
            {
                TreatyPricingProductId = entity.TreatyPricingProductId,
                ProductCode = entity.TreatyPricingProduct.Code,
                ProductName = entity.TreatyPricingProduct.Name,
                TreatyPricingProductVersionId = entity.Id,
                VersionNo = entity.Version,
                ProductQuotationnName = entity.TreatyPricingProduct.QuotationName,
            };
        }

        public static Expression<Func<TreatyPricingUwLimitVersion, TreatyPricingGroupReferralViewModel>> ExpressionUwLimits()
        {
            return entity => new TreatyPricingGroupReferralViewModel
            {
                TreatyPricingUwLimitId = entity.TreatyPricingUwLimitId,
                UwLimitId = entity.TreatyPricingUwLimit.LimitId,
                UwLimitName = entity.TreatyPricingUwLimit.Name,
                TreatyPricingUwLimitVersionId = entity.Id,
                VersionNo = entity.Version,
            };
        }

        public static Expression<Func<TreatyPricingGroupReferralVersion, TreatyPricingGroupReferralViewModel>> ExpressionVersions()
        {
            return entity => new TreatyPricingGroupReferralViewModel
            {
                TreatyPricingGroupReferralVersionId = entity.Id,
                VersionNo = entity.Version,
            };
        }
    }
}