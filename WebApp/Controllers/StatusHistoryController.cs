using BusinessObject;
using Newtonsoft.Json;
using Services;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class StatusHistoryController : BaseController
    {
        public static void Add(int moduleId, int objectId, int status, int authUserId, ref TrailObject trail)
        {
            StatusHistoryBo prevStatusHistoryBo = StatusHistoryService.FindLatestByModuleIdObjectId(moduleId, objectId);
            if (prevStatusHistoryBo != null && prevStatusHistoryBo.Status == status)
                return;

            StatusHistoryBo statusHistoryBo = new StatusHistoryBo
            {
                ModuleId = moduleId,
                ObjectId = objectId,
                Status = status,
                CreatedById = authUserId,
                UpdatedById = authUserId,
            };
            StatusHistoryService.Save(ref statusHistoryBo, ref trail);
        }

        public static StatusHistoryBo Create(StatusHistoryBo statusHistoryBo, int authUserId, string authUserName, ref TrailObject trail)
        {
            statusHistoryBo.CreatedById = authUserId;
            statusHistoryBo.UpdatedById = authUserId;

            ModuleBo moduleBo = ModuleService.Find(statusHistoryBo.ModuleId);

            Result result = StatusHistoryService.Create(ref statusHistoryBo, ref trail);
            if (result.Valid)
            {
                statusHistoryBo.StatusName = StatusHistoryBo.GetStatusName(moduleBo, statusHistoryBo.Status);
                statusHistoryBo.CreatedAtStr = statusHistoryBo.CreatedAt.ToString(Util.GetDateTimeFormat());
                statusHistoryBo.CreatedByName = authUserName;

                if (statusHistoryBo.RemarkBo != null)
                {
                    RemarkBo remarkBo = statusHistoryBo.RemarkBo;
                    remarkBo.StatusHistoryId = statusHistoryBo.Id;
                    remarkBo.CreatedById = authUserId;
                    remarkBo.UpdatedById = authUserId;

                    RemarkService.Create(ref remarkBo, ref trail);

                    statusHistoryBo.RemarkBo = remarkBo;
                }
            }

            return statusHistoryBo;
        }
    }
}