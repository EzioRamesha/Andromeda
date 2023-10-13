using Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities.Views
{
    [Table("PricingDashboardQuotingCasesByCompany")]
    public class PricingDashboardQuotingCasesByCompany
    {
        [Key]
        public int Id { get; set; }

        public string CedantName { get; set; }

        public int? CedantId { get; set; }

        public int? PricingTeamPickListDetailId { get; set; }

        public int? QuotingCaseCount { get; set; }

        public double? ExpectedRiPremium { get; set; }

        public PricingDashboardQuotingCasesByCompany()
        {
        }
    }
}
