using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingTreatyWorkflowVersionBo
    {
        public int Id { get; set; }

        public int TreatyPricingTreatyWorkflowId { get; set; }

        public virtual TreatyPricingTreatyWorkflowBo TreatyPricingTreatyWorkflowBo { get; set; }
        
        public int Version { get; set; }

        #region General
        public DateTime? RequestDate { get; set; }
        public string RequestDateStr { get; set; }

        public DateTime? TargetSentDate { get; set; }
        public string TargetSentDateStr { get; set; }

        public DateTime? DateSentToReviewer1st { get; set; }
        public string DateSentToReviewer1stStr { get; set; }

        public DateTime? DateSentToClient1st { get; set; }
        public string DateSentToClient1stStr { get; set; }

        public DateTime? LatestRevisionDate { get; set; }
        public string LatestRevisionDateStr { get; set; }

        public DateTime? SignedDate { get; set; }
        public string SignedDateStr { get; set; }

        public DateTime? ReportedDate { get; set; }
        public string ReportedDateStr { get; set; }

        public int? PersonInChargeId { get; set; }

        public string PersonInChargeName { get; set; }

        public string TriggerDateStr { get; set; }
        #endregion

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public TreatyPricingTreatyWorkflowVersionBo()
        {

        }

        public TreatyPricingTreatyWorkflowVersionBo(TreatyPricingTreatyWorkflowVersionBo bo)
        {
            TreatyPricingTreatyWorkflowId = bo.TreatyPricingTreatyWorkflowId;

            //General
            RequestDate = bo.RequestDate;
            RequestDateStr = bo.RequestDateStr;
            TargetSentDate = bo.TargetSentDate;
            TargetSentDateStr = bo.TargetSentDateStr;
            DateSentToReviewer1st = bo.DateSentToReviewer1st;
            DateSentToReviewer1stStr = bo.DateSentToReviewer1stStr;
            DateSentToClient1st = bo.DateSentToClient1st;
            DateSentToClient1stStr = bo.DateSentToClient1stStr;
            LatestRevisionDate = bo.LatestRevisionDate;
            LatestRevisionDateStr = bo.LatestRevisionDateStr;
            SignedDate = bo.SignedDate;
            SignedDateStr = bo.SignedDateStr;
            ReportedDate = bo.ReportedDate;
            ReportedDateStr = bo.ReportedDateStr;
            PersonInChargeId = bo.PersonInChargeId;
        }
    }
}
