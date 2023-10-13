using Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities.Views
{
    [Table("PricingDashboardDueDateOverview")]
    public class PricingDashboardDueDateOverview
    {
        [Key]
        public int Id { get; set; }

        public int? PricingTeamPickListDetailId { get; set; }

        public int? DueDateOverviewType { get; set; }

        public int? NoOfCase { get; set; }

        public PricingDashboardDueDateOverview()
        {
        }
    }
}
