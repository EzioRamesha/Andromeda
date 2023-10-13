using Shared;
using Shared.DataAccess;
using System;

namespace BusinessObject
{
    public class PremiumSpreadTableDetailBo
    {
        public int Id { get; set; }

        public int PremiumSpreadTableId { get; set; }

        public PremiumSpreadTableBo PremiumSpreadTableBo { get; set; }

        public string CedingPlanCode { get; set; }

        public string BenefitCode { get; set; }

        public int? AgeFrom { get; set; }

        public string AgeFromStr { get; set; }

        public int? AgeTo { get; set; }

        public string AgeToStr { get; set; }

        public double PremiumSpread { get; set; }

        public string PremiumSpreadStr { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public Result Validate(int index)
        {
            Result result = new Result();

            int? ageFrom = Util.GetParseInt(AgeFromStr);
            int? ageTo = Util.GetParseInt(AgeToStr);

            if (string.IsNullOrEmpty(CedingPlanCode) || string.IsNullOrWhiteSpace(CedingPlanCode))
                result.AddError(string.Format(MessageBag.RequiredWithRow, "Ceding Plan Code", index));

            //if (BenefitId == 0)
            //    result.AddError(string.Format(MessageBag.RequiredWithRow, "MLRe Benefit Code", index));

            //if (string.IsNullOrEmpty(BenefitCode) || string.IsNullOrWhiteSpace(BenefitCode))
            //    result.AddError(string.Format(MessageBag.RequiredWithRow, "MLRe Benefit Code", index));

            if (ageFrom.HasValue && !ageTo.HasValue)
            {
                result.AddError(string.Format(MessageBag.RequiredWithRow, "Age To", index));
            }
            else if (!ageFrom.HasValue && ageTo.HasValue)
            {
                result.AddError(string.Format(MessageBag.RequiredWithRow, "Age From", index));
            }
            else if (ageFrom.HasValue && ageTo.HasValue && ageTo < ageFrom)
            {
                result.AddError(string.Format(MessageBag.GreaterOrEqualToWithRow, "Age To", "Age From", index));
            }

            if (string.IsNullOrEmpty(PremiumSpreadStr) || string.IsNullOrWhiteSpace(PremiumSpreadStr))
            {
                result.AddError(string.Format(MessageBag.RequiredWithRow, "Premium Spread", index));
            }
            else if (!Util.IsValidDouble(PremiumSpreadStr, out _, out _))
            {
                result.AddError(string.Format("Premium Spread format is not valid at row #{0}", index));
            }

            return result;
        }
    }

    public class PremiumSpreadAgeRange
    {
        public string CedingPlanCode { get; set; }

        public string BenefitCode { get; set; }

        public int? AgeFrom { get; set; }

        public int? AgeTo { get; set; }
    }
}
