using BusinessObject.Identity;
using System;
using System.Collections.Generic;

namespace BusinessObject
{
    public class RemarkBo
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        public ModuleBo ModuleBo { get; set; }

        public int ObjectId { get; set; }

        public string SubModuleController { get; set; }

        public int? SubObjectId { get; set; }

        public int? StatusHistoryId { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }

        public string Content { get; set; }

        public string ShortContent { get; set; }

        public string Subject { get; set; }

        public int? Version { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedAtStr { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string CreatedByName { get; set; }

        // Object Permission
        public bool IsPrivate { get; set; }

        public string PermissionName { get; set; }

        public ObjectPermissionBo ObjectPermissionBo { get; set; }

        // Remark Follow Up
        public bool HasFollowUp { get; set; }

        public RemarkFollowUpBo RemarkFollowUpBo { get; set; }

        // Document
        public IList<DocumentBo> DocumentBos { get; set; }

        public RemarkBo()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
