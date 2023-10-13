using Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities.Views
{
    [Table("PricingDashboardDueDateOverviewPIC")]
    public class PricingDashboardDueDateOverviewPIC
    {
        [Key]
        public int Id { get; set; }

        public int? PricingTeamPickListDetailId { get; set; }

        public int? UserId { get; set; }

        public string UserName { get; set; }

        public int? NoOfCaseDueDateBelow5 { get; set; }

        public int? NoOfCaseDueDate6To10 { get; set; }

        public int? NoOfCaseDueDateExceed10 { get; set; }

        public PricingDashboardDueDateOverviewPIC()
        {
        }
    }
}
