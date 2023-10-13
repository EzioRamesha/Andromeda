using System;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingCedantBo
    {
        public int Id { get; set; }

        public int CedantId { get; set; }

        public CedantBo CedantBo { get; set; }

        public int ReinsuranceTypePickListDetailId { get; set; }

        public PickListDetailBo ReinsuranceTypePickListDetailBo { get; set; }

        public string Code { get; set; }

        public int NoOfProduct { get; set; }

        public int NoOfDocument { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int IndexProduct = 1;
        public const int IndexRateTableGroup = 2;
        public const int IndexUwLimit = 3;
        public const int IndexMedicalTable = 4;
        public const int IndexFinancialTable = 5;
        public const int IndexUwQuestionnaire = 6;
        public const int IndexAdvantageProgram = 7;
        public const int IndexClaimApprovalLimit = 8;
        public const int IndexDefinitionAndExclusion = 9;
        public const int IndexCampaign = 10;
        public const int IndexProfitCommission = 11;
        public const int IndexCustomOther = 12;

        public const int ObjectProduct = 1;
        public const int ObjectProductVersion = 2;

        public override string ToString()
        {
            return Code;
        }
    }
}
