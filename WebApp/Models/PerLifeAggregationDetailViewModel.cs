using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using Services.Retrocession;
using Shared;
using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class PerLifeAggregationDetailViewModel
    {
        public int Id { get; set; }

        public int PerLifeAggregationId { get; set; }

        public PerLifeAggregationBo PerLifeAggregationBo { get; set; }

        public PerLifeAggregation PerLifeAggregation { get; set; }

        [DisplayName("Risk Quarter")]
        public string RiskQuarter { get; set; }

        [DisplayName("Processing Date")]
        public DateTime? ProcessingDate { get; set; }

        public string ProcessingDateStr { get; set; }

        [DisplayName("Status")]
        public int Status { get; set; }

        // Form variables

        public int? ActiveTab { get; set; }

        public string SelectedExceptionIds { get; set; }

        public PerLifeAggregationDetailViewModel() { }

        public PerLifeAggregationDetailViewModel(PerLifeAggregationDetailBo perLifeAggregationDetailBo)
        {
            Set(perLifeAggregationDetailBo);
        }

        public void Set(PerLifeAggregationDetailBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                PerLifeAggregationId = bo.PerLifeAggregationId;
                PerLifeAggregationBo = bo.PerLifeAggregationBo;
                RiskQuarter = bo.RiskQuarter;
                ProcessingDate = bo.ProcessingDate;
                ProcessingDateStr = bo.ProcessingDate?.ToString(Util.GetDateFormat());
                Status = bo.Status;
            }
        }

        public PerLifeAggregationDetailBo FormBo(int createdById, int updatedById)
        {
            var bo = new PerLifeAggregationDetailBo
            {
                Id = Id,
                PerLifeAggregationId = PerLifeAggregationId,
                PerLifeAggregationBo = PerLifeAggregationService.Find(PerLifeAggregationId),
                RiskQuarter = RiskQuarter,
                ProcessingDate = ProcessingDate,
                Status = Status,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };

            if (!string.IsNullOrEmpty(ProcessingDateStr))
            {
                bo.ProcessingDate = DateTime.Parse(ProcessingDateStr);
            }

            return bo;
        }

        public static Expression<Func<PerLifeAggregationDetail, PerLifeAggregationDetailViewModel>> Expression()
        {
            return entity => new PerLifeAggregationDetailViewModel
            {
                Id = entity.Id,
                PerLifeAggregationId = entity.PerLifeAggregationId,
                PerLifeAggregation = entity.PerLifeAggregation,
                RiskQuarter = entity.RiskQuarter,
                ProcessingDate = entity.ProcessingDate,
                Status = entity.Status,
            };
        }
    }
}