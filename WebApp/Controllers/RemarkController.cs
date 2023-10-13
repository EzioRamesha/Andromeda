using BusinessObject;
using BusinessObject.Identity;
using Services;
using Services.Identity;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class RemarkController : BaseController
    {
        // GET: Remark
        public JsonResult Index()
        {
            IList<RemarkBo> remarkBos = RemarkService.Get();
            return Json(new { remarkBos });
        }

        [HttpPost]
        public JsonResult Create(RemarkBo remarkBo)
        {
            remarkBo.CreatedById = AuthUserId;
            remarkBo.UpdatedById = AuthUserId;

            TrailObject trail = GetNewTrailObject();
            Result = RemarkService.Create(ref remarkBo, ref trail);
            if (Result.Valid)
            {
                if (remarkBo.RemarkFollowUpBo != null)
                {
                    var followUpBo = remarkBo.RemarkFollowUpBo;
                    followUpBo.RemarkId = remarkBo.Id;
                    followUpBo.FollowUpAt = Util.GetParseDateTime(followUpBo.FollowUpAtStr);
                    followUpBo.CreatedById = AuthUserId;
                    followUpBo.UpdatedById = AuthUserId;
                    RemarkFollowUpService.Create(ref followUpBo, ref trail);
                }

                if (remarkBo.IsPrivate && AuthUser.DepartmentId.HasValue)
                {
                    var permission = new ObjectPermissionBo()
                    {
                        ObjectId = remarkBo.Id,
                        Type = ObjectPermissionBo.TypeRemark,
                        DepartmentId = AuthUser.DepartmentId.Value,
                        CreatedById = AuthUserId
                    };
                    ObjectPermissionService.Create(ref permission, ref trail);
                }

                CreateTrail(
                    remarkBo.Id,
                    "Create Remark"
                );

                ModuleBo moduleBo = ModuleService.Find(remarkBo.ModuleId);
                remarkBo.StatusName = StatusHistoryBo.GetStatusName(moduleBo, remarkBo.Status);
                remarkBo.PermissionName = ObjectPermissionBo.GetPermissionName(remarkBo.IsPrivate);
                remarkBo.CreatedAtStr = remarkBo.CreatedAt.ToString("dd MMM yyyy hh:mm:ss tt");
                remarkBo.ShortContent = remarkBo.Content.Length > 100 ? remarkBo.Content.Substring(0, 97) + "..." : remarkBo.Content;

                UserBo userBo = UserService.Find(remarkBo.CreatedById);
                if (userBo != null)
                {
                    remarkBo.CreatedByName = userBo.FullName;
                }

                return Json(new { remarkBo });
            }
            return Json(new { error = Result.MessageBag.Errors });
        }

        [HttpPost]
        public JsonResult CompleteFollowUp(int remarkFollowUpId)
        {
            RemarkFollowUpBo remarkFollowUpBo = RemarkFollowUpService.Find(remarkFollowUpId);
            remarkFollowUpBo.Status = RemarkFollowUpBo.StatusCompleted;
            remarkFollowUpBo.UpdatedById = AuthUserId;
            RemarkFollowUpService.Update(ref remarkFollowUpBo);

            return Json(new {  });
        }

        public static List<RemarkBo> GetRemarks(FormCollection form, string documentDownloadUrl = "", string prefix = "remark")
        {
            int index = 0;
            List<RemarkBo> remarkBos = new List<RemarkBo> { };
            List<DocumentBo> documentBos = DocumentController.GetDocuments(form, documentDownloadUrl);

            string content = form.Get(string.Format("{0}.Content[{1}]", prefix, index));
            while (!string.IsNullOrWhiteSpace(content))
            {
                // Get Values from Form
                string createdByName = form.Get(string.Format("{0}.CreatedByName[{1}]", prefix, index));
                string status = form.Get(string.Format("{0}.Status[{1}]", prefix, index));
                string statusName = form.Get(string.Format("{0}.StatusName[{1}]", prefix, index));
                string createdAtStr = form.Get(string.Format("{0}.CreatedAtStr[{1}]", prefix, index));
                DateTime? createdAt = Util.GetParseDateTime(createdAtStr);

                string hasFollowUpStr = form.Get(string.Format("{0}.HasFollowUp[{1}]", prefix, index));
                RemarkFollowUpBo followUpBo = null;
                if (!string.IsNullOrEmpty(hasFollowUpStr))
                {
                    bool hasFollowUp = bool.Parse(hasFollowUpStr);
                    if (hasFollowUp)
                    {
                        string followUpAtStr = form.Get(string.Format("{0}.FollowUpAtStr[{1}]", prefix, index));
                        string followUpStatusStr = form.Get(string.Format("{0}.FollowUpStatus[{1}]", prefix, index));
                        string followUpUserIdStr = form.Get(string.Format("{0}.FollowUpUserId[{1}]", prefix, index));
                        int followUpStatus = int.Parse(followUpStatusStr);
                        followUpBo = new RemarkFollowUpBo()
                        {
                            Status = followUpStatus,
                            StatusName = RemarkFollowUpBo.GetStatusName(followUpStatus),
                            FollowUpAt = Util.GetParseDateTime(followUpAtStr),
                            FollowUpUserId = int.Parse(followUpUserIdStr)
                        };
                    }
                }

                string isPrivateStr = form.Get(string.Format("{0}.IsPrivate[{1}]", prefix, index));
                bool isPrivate = string.IsNullOrEmpty(isPrivateStr) ? false : bool.Parse(isPrivateStr);

                RemarkBo remarkBo = new RemarkBo
                {
                    Status = int.Parse(status),
                    StatusName = statusName,
                    Content = content,
                    CreatedByName = createdByName,
                    CreatedAt = createdAt ?? DateTime.Now,
                    CreatedAtStr = createdAtStr,
                    UpdatedAt = createdAt ?? DateTime.Now,
                    IsPrivate = isPrivate,
                    PermissionName = ObjectPermissionBo.GetPermissionName(isPrivate),
                    RemarkFollowUpBo = followUpBo,
                    HasFollowUp = followUpBo != null,
                    DocumentBos = documentBos.Where(q => q.RemarkIndex == index).ToList()
                };

                remarkBos.Add(remarkBo);

                index++;
                content = form.Get(string.Format("{0}.Content[{1}]", prefix, index));
            }

            return remarkBos;
        }

        public static void SaveRemarks(List<RemarkBo> remarkBos, int moduleId, int objectId, int authUserId, ref TrailObject trail, int? departmentId = null)
        {
            foreach (RemarkBo bo in remarkBos)
            {
                RemarkBo remarkBo = bo;
                remarkBo.ModuleId = moduleId;
                remarkBo.ObjectId = objectId;
                remarkBo.CreatedById = authUserId;
                remarkBo.UpdatedById = authUserId;

                RemarkService.Save(ref remarkBo, ref trail);
                if (remarkBo.RemarkFollowUpBo != null)
                {
                    var followUpBo = remarkBo.RemarkFollowUpBo;
                    followUpBo.RemarkId = remarkBo.Id;
                    followUpBo.CreatedById = authUserId;
                    followUpBo.UpdatedById = authUserId;
                    RemarkFollowUpService.Create(ref followUpBo, ref trail);
                }

                if (remarkBo.IsPrivate && departmentId.HasValue)
                {
                    var permission = new ObjectPermissionBo()
                    {
                        ObjectId = remarkBo.Id,
                        Type = ObjectPermissionBo.TypeRemark,
                        DepartmentId = departmentId.Value,
                        CreatedById = authUserId
                    };
                    ObjectPermissionService.Create(ref permission, ref trail);
                }

                if (!remarkBo.DocumentBos.IsNullOrEmpty())
                    DocumentController.SaveDocuments(remarkBo.DocumentBos, moduleId, objectId, authUserId, ref trail, remarkBo.Id, departmentId);
            }
        }
    }
}