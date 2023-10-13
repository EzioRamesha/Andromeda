using BusinessObject.Identity;
using System;
using System.Collections.Generic;

namespace BusinessObject.InvoiceRegisters
{
    public class InvoiceRegisterBatchRemarkBo
    {
        public int Id { get; set; }

        public int InvoiceRegisterBatchId { get; set; }

        public InvoiceRegisterBatchBo InvoiceRegisterBatchBo { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }

        public bool RemarkPermission { get; set; }

        public string Content { get; set; }

        public bool FilePermission { get; set; }

        public bool FollowUp { get; set; }

        public int? FollowUpStatus { get; set; }

        public DateTime? FollowUpDate { get; set; }

        public string FollowUpDateStr { get; set; }

        public int? FollowUpUserId { get; set; }

        public UserBo FollowUpUserBo { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedAtStr { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string CreatedByName { get; set; }

        public ICollection<InvoiceRegisterBatchRemarkDocumentBo> InvoiceRegisterBatchRemarkDocumentBos { get; set; }

        public InvoiceRegisterBatchRemarkBo()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
