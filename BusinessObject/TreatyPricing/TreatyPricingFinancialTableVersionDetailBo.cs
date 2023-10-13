using Shared.ProcessFile;
using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingFinancialTableVersionDetailBo
    {
        public int Id { get; set; }

        public int TreatyPricingFinancialTableVersionId { get; set; }

        public virtual TreatyPricingFinancialTableVersionBo TreatyPricingFinancialTableVersionBo { get; set; }

        public int DistributionTierPickListDetailId { get; set; }

        public PickListDetailBo DistributionTierPickListDetailBo { get; set; }

        public string Description { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string DistributionTier { get; set; }

        public const int ColumnDistributionTier = 1;
        public const int ColumnDescription = 2;

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "Distribution Tier",
                    ColIndex = ColumnDistributionTier,
                },
                new Column
                {
                    Header = "Description",
                    ColIndex = ColumnDescription,
                },
            };
        }
    }
}
