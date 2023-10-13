using Shared;
using System;
using System.Collections.Generic;

namespace BusinessObject
{
    public class ClaimAuthorityLimitCedantDetailBo
    {
        public int Id { get; set; }

        public int ClaimAuthorityLimitCedantId { get; set; }

        public ClaimAuthorityLimitCedantBo ClaimAuthorityLimitCedantBo { get; set; }

        public int ClaimCodeId { get; set; }

        public ClaimCodeBo ClaimCodeBo { get; set; }

        public int Type { get; set; }

        public double ClaimAuthorityLimitValue { get; set; }

        public int FundsAccountingTypePickListDetailId { get; set; }

        public PickListDetailBo FundsAccountingTypePickListDetailBo { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedById { get; set; }

        public string ClaimAuthorityLimitValueStr { get; set; }

        public string EffectiveDateStr { get; set; }

        public const int TypeBoth = 1;
        public const int TypeContestable = 2;
        public const int TypeNonContestable = 3;
        public const int MaxType = 3; 
        
        public const int FundAccountingCodeIndividual = 1;
        public const int FundAccountingCodeGroup = 2;

        public ClaimAuthorityLimitCedantDetailBo()
        {
            Type = TypeBoth;
            CreatedAt = DateTime.Now;
        }

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeBoth:
                    return "Both";
                case TypeContestable:
                    return "Contestable";
                case TypeNonContestable:
                    return "Non-Contestable";
                default:
                    return "";
            }
        }

        public List<string> Validate()
        {
            List<string> errors = new List<string> { };

            if (ClaimCodeId == 0)
                errors.Add(string.Format(MessageBag.Required, "Claim Code"));

            if (Type == 0)
                errors.Add(string.Format(MessageBag.Required, "Type"));

            if (string.IsNullOrEmpty(ClaimAuthorityLimitValueStr) || string.IsNullOrWhiteSpace(ClaimAuthorityLimitValueStr))
                errors.Add(string.Format(MessageBag.Required, "Amount"));
            else if (!Util.IsValidDouble(ClaimAuthorityLimitValueStr, out double? d, out _))
                errors.Add(string.Format("Invalid Amount Input: {0}", ClaimAuthorityLimitValueStr));

            if (FundsAccountingTypePickListDetailId == 0)
                errors.Add(string.Format(MessageBag.Required, "Funds Accounting Type Code"));

            if (string.IsNullOrEmpty(EffectiveDateStr))
                errors.Add(string.Format(MessageBag.Required, "Effective Date"));

            return errors;
        }
    }
}
