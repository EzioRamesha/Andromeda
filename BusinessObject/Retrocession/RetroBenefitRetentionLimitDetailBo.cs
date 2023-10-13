using Shared;
using Shared.DataAccess;
using System;

namespace BusinessObject.Retrocession
{
    public class RetroBenefitRetentionLimitDetailBo
    {
        public int Id { get; set; }

        public int RetroBenefitRetentionLimitId { get; set; }

        public RetroBenefitRetentionLimitBo RetroBenefitRetentionLimitBo { get; set; }

        public int MinIssueAge { get; set; }

        public string MinIssueAgeStr { get; set; }

        public int MaxIssueAge { get; set; }

        public string MaxIssueAgeStr { get; set; }

        public double MortalityLimitFrom { get; set; }

        public string MortalityLimitFromStr { get; set; }

        public double MortalityLimitTo { get; set; }

        public string MortalityLimitToStr { get; set; }

        public DateTime ReinsEffStartDate { get; set; }

        public string ReinsEffStartDateStr { get; set; }

        public DateTime ReinsEffEndDate { get; set; }

        public string ReinsEffEndDateStr { get; set; }

        public double MlreRetentionAmount { get; set; }

        public string MlreRetentionAmountStr { get; set; }

        public double MinReinsAmount { get; set; }

        public string MinReinsAmountStr { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public Result Validate(int index)
        {
            Result result = new Result();

            int? minIssueAge = Util.GetParseInt(MinIssueAgeStr);
            int? maxIssueAge = Util.GetParseInt(MaxIssueAgeStr);

            double? mortalityLimitFrom = Util.StringToDouble(MortalityLimitFromStr);
            double? mortalityLimitTo = Util.StringToDouble(MortalityLimitToStr);

            DateTime? reinsEffStartDate = Util.GetParseDateTime(ReinsEffStartDateStr);
            DateTime? reinsEffEndDate = Util.GetParseDateTime(ReinsEffEndDateStr);

            double? mlreRetentionAmount = Util.StringToDouble(MlreRetentionAmountStr);
            double? minReinsAmount = Util.StringToDouble(MinReinsAmountStr);

            if (string.IsNullOrEmpty(MinIssueAgeStr))
                result.AddError(string.Format("{0} is required at row #{1}", "Min Issue Age", index));
            else if (!string.IsNullOrEmpty(MinIssueAgeStr) && !minIssueAge.HasValue)
                result.AddError(string.Format("{0} is invalid numeric at row #{1}", "Min Issue Age", index));
            else if (!string.IsNullOrEmpty(MinIssueAgeStr) && minIssueAge.HasValue)
                MinIssueAge = minIssueAge.Value;

            if (string.IsNullOrEmpty(MaxIssueAgeStr))
                result.AddError(string.Format("{0} is required at row #{1}", "Min Issue Age", index));
            if (!string.IsNullOrEmpty(MaxIssueAgeStr) && !maxIssueAge.HasValue)
                result.AddError(string.Format("{0} is invalid numeric at row #{1}", "Max Issue Age", index));
            else if (!string.IsNullOrEmpty(MaxIssueAgeStr) && maxIssueAge.HasValue)
                MaxIssueAge = maxIssueAge.Value;

            if (minIssueAge.HasValue && maxIssueAge .HasValue && minIssueAge > maxIssueAge)
                result.AddError(string.Format("Max Issue Age must greater or equal to Min Issue Date at row #{1}", "Max Issue Age", index));

            if (string.IsNullOrEmpty(MortalityLimitFromStr))
                result.AddError(string.Format("{0} is required at row #{1}", "Mortality Limit From", index));
            else if (!string.IsNullOrEmpty(MortalityLimitFromStr) && !mortalityLimitFrom.HasValue)
                result.AddError(string.Format("{0} is invalid double format at row #{1}", "Mortality Limit From", index));
            else if (!string.IsNullOrEmpty(MortalityLimitFromStr) && mortalityLimitFrom.HasValue)
                MortalityLimitFrom = mortalityLimitFrom.Value;

            if (string.IsNullOrEmpty(MortalityLimitToStr))
                result.AddError(string.Format("{0} is required at row #{1}", "Mortality Limit To", index));
            else if (!string.IsNullOrEmpty(MortalityLimitToStr) && !mortalityLimitTo.HasValue)
                result.AddError(string.Format("{0} is invalid double format at row #{1}", "Mortality Limit To", index));
            else if (!string.IsNullOrEmpty(MortalityLimitToStr) && mortalityLimitTo.HasValue)
                MortalityLimitTo = mortalityLimitTo.Value;

            //if (mortalityLimitFrom.HasValue && mortalityLimitTo.HasValue && mortalityLimitFrom > mortalityLimitTo)
            //    result.AddError(string.Format("Mortality Limit To must greater or equal to Mortality Limit From at row #{1}", "Max Issue Age", index));

            if (string.IsNullOrEmpty(ReinsEffStartDateStr))
                result.AddError(string.Format("{0} is required at row #{1}", "Reinsurance Effective Start Date", index));
            else if (!string.IsNullOrEmpty(ReinsEffStartDateStr) && !reinsEffStartDate.HasValue)
                result.AddError(string.Format("{0} is invalid date format at row #{1}", "Reinsurance Effective Start Date", index));
            else if (!string.IsNullOrEmpty(ReinsEffStartDateStr) && reinsEffStartDate.HasValue)
                ReinsEffStartDate = reinsEffStartDate.Value;

            if (string.IsNullOrEmpty(ReinsEffEndDateStr))
                result.AddError(string.Format("{0} is required at row #{1}", "Reinsurance Effective End Date", index));
            else if (!string.IsNullOrEmpty(ReinsEffEndDateStr) && !reinsEffEndDate.HasValue)
                result.AddError(string.Format("{0} is invalid date format at row #{1}", "Reinsurance Effective End Date", index));
            else if (!string.IsNullOrEmpty(ReinsEffEndDateStr) && reinsEffEndDate.HasValue)
                ReinsEffEndDate = reinsEffEndDate.Value;

            if (reinsEffStartDate.HasValue && reinsEffEndDate.HasValue &&  reinsEffStartDate > reinsEffEndDate)
                result.AddError(string.Format("Reinsurance Effective End Date must greater or equal to Reinsurance Effective Start Date at row #{1}", "Max Issue Age", index));

            if (string.IsNullOrEmpty(MlreRetentionAmountStr))
                result.AddError(string.Format("{0} is required at row #{1}", "MLRe Retention Amount", index));
            else if (!string.IsNullOrEmpty(MlreRetentionAmountStr) && !mlreRetentionAmount.HasValue)
                result.AddError(string.Format("{0} is invalid double format at row #{1}", "MLRe Retention Amount", index));
            else if (!string.IsNullOrEmpty(MlreRetentionAmountStr) && mlreRetentionAmount.HasValue)
                MlreRetentionAmount = mlreRetentionAmount.Value;

            if (string.IsNullOrEmpty(MinReinsAmountStr))
                result.AddError(string.Format("{0} is required at row #{1}", "Minimum Reinsurance Amount", index));
            else if (!string.IsNullOrEmpty(MinReinsAmountStr) && !minReinsAmount.HasValue)
                result.AddError(string.Format("{0} is invalid double format at row #{1}", "Minimum Reinsurance Amount", index));
            else if (!string.IsNullOrEmpty(MinReinsAmountStr) && minReinsAmount.HasValue)
                MinReinsAmount = minReinsAmount.Value;

            return result;
        }
    }
}
