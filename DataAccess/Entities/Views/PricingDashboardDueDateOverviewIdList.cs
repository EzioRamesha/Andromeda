using Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities.Views
{
    [Table("PricingDashboardDueDateOverviewIdList")]
    public class PricingDashboardDueDateOverviewIdList
    {
        [Key]
        public int Id { get; set; }

        public int? PricingTeamPickListDetailId { get; set; }

        public int? DueDateOverviewType { get; set; }

        public int? QuotationId { get; set; }

        public PricingDashboardDueDateOverviewIdList()
        {
        }
    }
}
