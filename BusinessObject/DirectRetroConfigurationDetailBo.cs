using Shared;
using Shared.DataAccess;
using System;

namespace BusinessObject
{
    public class DirectRetroConfigurationDetailBo
    {
        public int Id { get; set; }

        public int DirectRetroConfigurationId { get; set; }

        public DirectRetroConfigurationBo DirectRetroConfigurationBo { get; set; }

        public DateTime? RiskPeriodStartDate { get; set; }

        public string RiskPeriodStartDateStr { get; set; }

        public DateTime? RiskPeriodEndDate { get; set; }

        public string RiskPeriodEndDateStr { get; set; }

        public DateTime? IssueDatePolStartDate { get; set; }

        public string IssueDatePolStartDateStr { get; set; }

        public DateTime? IssueDatePolEndDate { get; set; }

        public string IssueDatePolEndDateStr { get; set; }

        public DateTime? ReinsEffDatePolStartDate { get; set; }

        public string ReinsEffDatePolStartDateStr { get; set; }

        public DateTime? ReinsEffDatePolEndDate { get; set; }

        public string ReinsEffDatePolEndDateStr { get; set; }

        public bool IsDefault { get; set; } = false;

        public int RetroPartyId { get; set; }

        public RetroPartyBo RetroPartyBo { get; set; }

        public string TreatyNo { get; set; }

        public string Schedule { get; set; }

        public double Share { get; set; }

        public string ShareStr { get; set; }

        public int? PremiumSpreadTableId { get; set; }

        public PremiumSpreadTableBo PremiumSpreadTableBo { get; set; }

        public int? TreatyDiscountTableId { get; set; }

        public TreatyDiscountTableBo TreatyDiscountTableBo { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public Result Validate(int index)
        {
            Result result = new Result();

            //if (string.IsNullOrEmpty(RiskPeriodStartDateStr) || string.IsNullOrWhiteSpace(RiskPeriodStartDateStr))
            //    result.AddError(string.Format(MessageBag.RequiredWithRow, "Risk Effective Start Date", index));
            //

            //if (!string.IsNullOrEmpty(RiskPeriodStartDateStr) && !RiskPeriodStartDate.HasValue)
            //    result.AddError(string.Format(MessageBag.InvalidDateFormatWithNameWithRow, "Risk Effective Start Date", index));

            //if (!string.IsNullOrEmpty(RiskPeriodStartDateStr) && !RiskPeriodEndDate.HasValue)
            //    result.AddError(string.Format(MessageBag.InvalidDateFormatWithNameWithRow, "Risk Effective End Date", index));


            if (RiskPeriodStartDate.HasValue && !RiskPeriodEndDate.HasValue)
            {
                result.AddError(string.Format(MessageBag.RequiredWithRow, "Risk Effective End Date", index));
            }
            else if (!RiskPeriodStartDate.HasValue && RiskPeriodEndDate.HasValue)
            {
                result.AddError(string.Format(MessageBag.RequiredWithRow, "Risk Effective Start Date", index));
            }
            else if (RiskPeriodStartDate.HasValue && RiskPeriodEndDate.HasValue && RiskPeriodEndDate < RiskPeriodStartDate)
            {
                result.AddError(string.Format(MessageBag.GreaterOrEqualToWithRow, "Risk Effective End Date", "Risk Effective Start Date", index));
            }

            if (IssueDatePolStartDate.HasValue && !IssueDatePolEndDate.HasValue)
            {
                result.AddError(string.Format(MessageBag.RequiredWithRow, "Policy Issue End Date", index));
            }
            else if (!IssueDatePolStartDate.HasValue && IssueDatePolEndDate.HasValue)
            {
                result.AddError(string.Format(MessageBag.RequiredWithRow, "Policy Issue Start Date", index));
            }
            else if (IssueDatePolStartDate.HasValue && IssueDatePolEndDate.HasValue && IssueDatePolEndDate < IssueDatePolStartDate)
            {
                result.AddError(string.Format(MessageBag.GreaterOrEqualToWithRow, "Policy Issue End Date", "Policy Issue Start Date", index));
            }

            if (ReinsEffDatePolStartDate.HasValue && !ReinsEffDatePolEndDate.HasValue)
            {
                result.AddError(string.Format(MessageBag.RequiredWithRow, "Reinsurance Effective End Date", index));
            }
            else if (!ReinsEffDatePolStartDate.HasValue && ReinsEffDatePolEndDate.HasValue)
            {
                result.AddError(string.Format(MessageBag.RequiredWithRow, "Reinsurance Effective Start Date", index));
            }
            else if (ReinsEffDatePolStartDate.HasValue && ReinsEffDatePolEndDate.HasValue && ReinsEffDatePolEndDate < ReinsEffDatePolStartDate)
            {
                result.AddError(string.Format(MessageBag.GreaterOrEqualToWithRow, "Reinsurance Effective End Date", "Reinsurance Effective Start Date", index));
            }

            if (RetroPartyId == 0)
                result.AddError(string.Format(MessageBag.RequiredWithRow, "Retro Party", index));

            if (string.IsNullOrEmpty(ShareStr) || string.IsNullOrWhiteSpace(ShareStr))
            {
                result.AddError(string.Format(MessageBag.RequiredWithRow, "Share", index));
            }
            else if (!Util.IsValidDouble(ShareStr, out _, out _))
            {
                result.AddError(string.Format("Share format is not valid at row #{0}", index));
            }

            if (!PremiumSpreadTableId.HasValue || PremiumSpreadTableId == 0)
                result.AddError(string.Format(MessageBag.RequiredWithRow, "Premium Spread Table", index));

            //if (TreatyDiscountTableId == 0)
            //    result.AddError(string.Format(MessageBag.RequiredWithRow, "Treaty Discount Table", index));

            return result;
        }
    }

    public class DirectRetroConfigurationDateRange
    {
        public DateTime? RiskPeriodStartDate { get; set; }

        public DateTime? RiskPeriodEndDate { get; set; }

        public DateTime? IssueDatePolStartDate { get; set; }

        public DateTime? IssueDatePolEndDate { get; set; }

        public DateTime? ReinsEffDatePolStartDate { get; set; }

        public DateTime? ReinsEffDatePolEndDate { get; set; }
    }
}
