using Shared;
using Shared.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class TreatyDiscountTableDetailBo
    {
        public int Id { get; set; }

        public int TreatyDiscountTableId { get; set; }

        public TreatyDiscountTableBo TreatyDiscountTableBo { get; set; }

        public string CedingPlanCode { get; set; }

        public string BenefitCode { get; set; }

        public IList<BenefitBo> BenefitBos { get; set; }

        public int? AgeFrom { get; set; }

        public string AgeFromStr { get; set; }

        public int? AgeTo { get; set; }

        public string AgeToStr { get; set; }
        

        public double? AARFrom { get; set; }

        public string AARFromStr { get; set; }

        public double? AARTo { get; set; }

        public string AARToStr { get; set; }

        public double Discount { get; set; }

        public string DiscountStr { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public Result Validate(int index)
        {
            Result result = new Result();

            int? ageFrom = Util.GetParseInt(AgeFromStr);
            int? ageTo = Util.GetParseInt(AgeToStr);
            double? AARFrom = Util.StringToDouble(AARFromStr);
            double? AARTo = Util.StringToDouble(AARToStr);


            if (string.IsNullOrEmpty(CedingPlanCode) || string.IsNullOrWhiteSpace(CedingPlanCode))
                result.AddError(string.Format(MessageBag.RequiredWithRow, "Ceding Plan Code", index));

            //if (string.IsNullOrEmpty(BenefitCode))
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

            if (AARFrom.HasValue && !AARTo.HasValue)
            {
                result.AddError(string.Format(MessageBag.RequiredWithRow, "AAR To", index));
            }
            else if (!AARFrom.HasValue && AARTo.HasValue)
            {
                result.AddError(string.Format(MessageBag.RequiredWithRow, "AAR From", index));
            }
            else if (AARFrom.HasValue && AARTo.HasValue && AARTo < AARFrom)
            {
                result.AddError(string.Format(MessageBag.GreaterOrEqualToWithRow, "AAR To", "AAR From", index));
            }

            if (string.IsNullOrEmpty(DiscountStr) || string.IsNullOrWhiteSpace(DiscountStr))
            {
                result.AddError(string.Format(MessageBag.RequiredWithRow, "Discount", index));
            }
            else if (!Util.IsValidDouble(DiscountStr, out _, out _))
            {
                result.AddError(string.Format("Discount format is not valid at row #{0}", index));
            }

            return result;
        }
    }

    public class TreatyDiscountAgeRange
    {
        public string CedingPlanCode { get; set; }

        public string BenefitCode { get; set; }

        public int? AgeFrom { get; set; }

        public int? AgeTo { get; set; }

        public double? AARFrom { get; set; }

        public double? AARTo { get; set; }
    }
}
