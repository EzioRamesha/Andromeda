using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using Services.Retrocession;
using System;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class PerLifeAggregationDetailTreatyViewModel
    {
        public int Id { get; set; }

        public int PerLifeAggregationDetailId { get; set; }

        public PerLifeAggregationDetailBo PerLifeAggregationDetailBo { get; set; }

        public PerLifeAggregationDetail PerLifeAggregationDetail { get; set; }

        public string TreatyCode { get; set; }

        // Additional
        public string RiskQuarter { get; set; }

        public int Count { get; set; }

        public double? TotalAar { get; set; }

        public double? TotalGrossPremium { get; set; }

        public double? TotalNetPremium { get; set; }

        public int Count2 { get; set; }

        public double? TotalAar2 { get; set; }

        public double? TotalGrossPremium2 { get; set; }

        public double? TotalNetPremium2 { get; set; }

        public int Count3 { get; set; }

        public double? TotalRetroAmount3 { get; set; }

        public double? TotalGrossPremium3 { get; set; }

        public double? TotalNetPremium3 { get; set; }

        public double? TotalDiscount3 { get; set; }

        // Treaty Summary
        public int RiskMonth { get; set; }

        public string TransactionTypeCode { get; set; }

        public int PolicyCount { get; set; }

        public double? Aar { get; set; }

        public double? GrossPremium { get; set; }

        public double? NetPremium { get; set; }

        public double? StandardPremium { get; set; }

        public double? SubstandardPremium { get; set; }

        public double? FlatExtraPremium { get; set; }

        public double? StandardDiscount{ get; set; }

        public double? SubstandardDiscount{ get; set; }

        public PerLifeAggregationDetailTreatyViewModel() { }

        public PerLifeAggregationDetailTreatyViewModel(PerLifeAggregationDetailTreatyBo perLifeAggregationDetailTreatyBo)
        {
            Set(perLifeAggregationDetailTreatyBo);
        }

        public void Set(PerLifeAggregationDetailTreatyBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                PerLifeAggregationDetailId = bo.PerLifeAggregationDetailId;
                PerLifeAggregationDetailBo = bo.PerLifeAggregationDetailBo;
                TreatyCode = bo.TreatyCode;
            }
        }

        public PerLifeAggregationDetailTreatyBo FormBo(int createdById, int updatedById)
        {
            return new PerLifeAggregationDetailTreatyBo
            {
                Id = Id,
                PerLifeAggregationDetailId = PerLifeAggregationDetailId,
                PerLifeAggregationDetailBo = PerLifeAggregationDetailService.Find(PerLifeAggregationDetailId),
                TreatyCode = TreatyCode,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<PerLifeAggregationDetailTreaty, PerLifeAggregationDetailTreatyViewModel>> Expression()
        {
            return entity => new PerLifeAggregationDetailTreatyViewModel
            {
                Id = entity.Id,
                PerLifeAggregationDetailId = entity.PerLifeAggregationDetailId,
                PerLifeAggregationDetail = entity.PerLifeAggregationDetail,
                TreatyCode = entity.TreatyCode,
            };
        }

        //public static Expression<Func<PerLifeAggregationDetailData, PerLifeAggregationDetailTreatyViewModel>> SummaryExpression()
        //{
        //    return entity => new PerLifeAggregationDetailTreatyViewModel
        //    {
        //        Id = entity.Id,
        //        RiskQuarter = entity.PerLifeAggregationDetailTreaty.PerLifeAggregationDetail.RiskQuarter,
        //        TreatyCode = entity.PerLifeAggregationDetailTreaty.TreatyCode,
        //        Count = entity
        //    };
        //}
    }
}