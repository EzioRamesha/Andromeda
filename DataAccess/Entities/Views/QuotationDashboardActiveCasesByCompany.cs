using Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities.TreatyPricing
{
    [Table("QuotationDashboardActiveCasesByCompany")]
    public class QuotationDashboardActiveCasesByCompany
    {
        [Key]
        public int Id { get; set; }

        public string CedantName { get; set; }

        public int? CedantId { get; set; }

        public string ReinsuranceTypeName { get; set; }

        public int? ReinsuranceTypeId { get; set; }

        public int? QuotingCaseCount { get; set; }

        public int? QuotedExceeded14Days { get; set; }

        public double? ExpectedRiPremium { get; set; }

        public QuotationDashboardActiveCasesByCompany()
        {
        }
    }
}