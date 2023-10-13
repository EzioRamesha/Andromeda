using Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities.Views
{
    [Table("PricingDashboardOutstandingPricingOverviewPIC")]
    public class PricingDashboardOutstandingPricingOverviewPIC
    {
        [Key]
        public int Id { get; set; }

        public int? PricingTeamPickListDetailId { get; set; }

        public int? UserId { get; set; }

        public string UserName { get; set; }

        public int? Unassigned { get; set; }

        public int? AssessmentInProgress { get; set; }

        public int? PendingTechReview { get; set; }

        public int? PendingPeerReview { get; set; }

        public int? PendingPricingAuthorityReview { get; set; }

        public int? ToUpdateRepo { get; set; }

        public int? UpdatedRepo { get; set; }

        public PricingDashboardOutstandingPricingOverviewPIC()
        {
        }
    }
}
