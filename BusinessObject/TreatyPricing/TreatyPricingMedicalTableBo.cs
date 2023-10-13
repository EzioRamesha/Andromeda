using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingMedicalTableBo : ObjectVersion
    {
        public int Id { get; set; }

        public int TreatyPricingCedantId { get; set; }

        public virtual TreatyPricingCedantBo TreatyPricingCedantBo { get; set; }

        public string MedicalTableId { get; set; }

        public int Status { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string BenefitCode { get; set; }

        public string DistributionChannel { get; set; }

        [MaxLength(3)]
        public string CurrencyCode { get; set; }

        public int? AgeDefinitionPickListDetailId { get; set; }

        public PickListDetailBo AgeDefinitionPickListDetailBo { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public bool IsDuplicateExisting { get; set; }
        public int? DuplicateTreatyPricingMedicalTableId { get; set; }
        public int? DuplicateTreatyPricingMedicalTableVersionId { get; set; }
        public bool DuplicateFromList { get; set; } = false;

        public IList<TreatyPricingMedicalTableVersionBo> TreatyPricingMedicalTableVersionBos { get; set; }

        public string StatusName { get; set; }

        public const int StatusActive = 1;
        public const int StatusInactive = 2;
        public const int StatusMax = 2;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusActive:
                    return "Active";
                case StatusInactive:
                    return "Inactive";
                default:
                    return "";
            }
        }

        public static string GetStatusClass(int key)
        {
            switch (key)
            {
                case StatusActive:
                    return "status-success-badge";
                case StatusInactive:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        #region Medical Table Comparison Report
        public TreatyPricingMedicalTableVersionBo VersionBo { get; set; }

        public TreatyPricingMedicalTableVersionDetailBo DetailBo { get; set; }

        public TreatyPricingMedicalTableUploadColumnBo ColumnBo { get; set; }

        public TreatyPricingMedicalTableUploadRowBo RowBo { get; set; }

        public TreatyPricingMedicalTableUploadCellBo CellBo { get; set; }

        public List<MedicalTableDetailComparison> MedicalTableDetailComparisons { get; set; }

        public string CedantInfo { get; set; }

        public string LinkedProducts { get; set; }

        public string Legends { get; set; }
        #endregion
    }

    public class MedicalTableDetailComparison
    {
        public List<string> Items { get; set; }

        public MedicalTableDetailComparison(List<string> items)
        {
            Items = items;
        }
    }
}
