using System.Collections.Generic;

namespace BusinessObject
{
    public class PickListDetailBo
    {
        public int Id { get; set; }

        public int PickListId { get; set; }
        public PickListBo PickListBo { get; set; }

        public int SortIndex { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }

        public const string BusinessOriginCodeWithinMalaysia = "WM";
        public const string BusinessOriginCodeOutsideMalaysia = "OM";
        public const string BusinessOriginCodeServiceFee = "SF";

        public const string PremiumFrequencyCodeMonthly = "M";
        public const string PremiumFrequencyCodeQuarter = "Q";
        public const string PremiumFrequencyCodeSemiAnnual = "S";
        public const string PremiumFrequencyCodeAnnual = "A";

        public const string TransactionTypeCodeNewBusiness = "NB";
        public const string TransactionTypeCodeRenewal = "RN";
        public const string TransactionTypeCodeAlteration = "AL";
        public const string TransactionTypeCodeNoClaimBonus = "NC";

        public const string TreatyTypeGroup = "L-GRP";
        public const string TreatyTypeIndividual = "L-IND";
        public const string TreatyTypeFacultative = "L-FAC"; // Code to be confirmed
        public const string TreatyTypeTakaful = "L-WAK";

        public const string ClaimTransactionTypeNew = "NEW";
        public const string ClaimTransactionTypeAdjustment = "ADJ";
        public const string ClaimTransactionTypeBulk = "BULK";

        public const string ReinsBasisCodeAutomatic = "AUTO";
        public const string ReinsBasisCodeFacultative = "FAC";
        public const string ReinsBasisCodeAdvantageProgram = "AP";

        public const string PolicyStatusCodeInforce = "IF";
        public const string PolicyStatusCodeTerminated = "T";
        public const string PolicyStatusCodeReversal = "R";
        public const string PolicyStatusCodeReinstatement = "RST";
        public const string PolicyStatusCodeFullyPaid = "FP";

        public const string FundsAccountingTypeCodeIndividual = "INDIVIDUAL";
        public const string FundsAccountingTypeCodeGroup = "GROUP";
        
        public const string RecordTypeBordx = "BORDX";
        public const string RecordTypeExGratia = "EX-GRATIA";
        
        public const string AgeBasisANrB = "ANrB";
        public const string AgeBasisANxB = "ANxB";
        public const string AgeBasisALB = "ALB";

        public const string TableTypeHips = "HIPS";
        public const string TableTypeGtlClaim = "GTLClaim";
        public const string TableTypeGtlRate = "GTLUnitRates";
        public const string TableTypeGtlAge = "GTLAgeBanded";
        public const string TableTypeGtlSa = "GTLBasisSA";
        public const string TableTypeGhsClaim = "GHSClaim";

        public const string CurrencyCodeMyr = "MYR";

        public const string RetroRegisterFieldClaim = "Claims";

        public const string RiArrangementYRT = "YRT";
        public const string RiArrangementCoinsuranceYRT = "CoinYRT";
        public const string RiArrangementCoinsurance = "Coin";

        public static string GetBusinessOriginName(string code)
        {
            switch (code)
            {
                case BusinessOriginCodeWithinMalaysia:
                    return "Within Malaysia";
                case BusinessOriginCodeOutsideMalaysia:
                    return "Outside Malaysia";
                case BusinessOriginCodeServiceFee:
                    return "Service Fee";
                default:
                    return "";
            }
        }

        public static string GetPremiumFrequencyCodeName(string code)
        {
            switch (code)
            {
                case PremiumFrequencyCodeMonthly:
                    return "Monthly";
                case PremiumFrequencyCodeQuarter:
                    return "Quarter";
                case PremiumFrequencyCodeSemiAnnual:
                    return "Semi Annual";
                case PremiumFrequencyCodeAnnual:
                    return "Annual";
                default:
                    return "";
            }
        }

        public static string GetTransactionTypeCodeName(string code)
        {
            switch (code)
            {
                case TransactionTypeCodeNewBusiness:
                    return "New Business Premium";
                case TransactionTypeCodeRenewal:
                    return "Renewal Premium";
                case TransactionTypeCodeAlteration:
                    return "Alteration Premium";
                default:
                    return "";
            }
        }

        public static List<string> GetMfrs17EndingPolicyStatus()
        {
            return new List<string>
            {
                PolicyStatusCodeInforce,
                PolicyStatusCodeReinstatement,
                PolicyStatusCodeFullyPaid
            };
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Description) || Description == Code)
            {
                return Code;
            }
            if (string.IsNullOrEmpty(Code))
            {
                return Description;
            }
            return string.Format("{0} - {1}", Code, Description);
        }
    }
}
