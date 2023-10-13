using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class ObjectLockController : BaseController
    {
        [HttpPost]
        public JsonResult ReleaseObjectLock(string controller, int objectId)
        {
            DeleteObjectLock(controller, objectId, AuthUserId);

            return Json(new { });
        }

        public static void DeleteObjectLock(string controller, int objectId, int authUserId)
        {
            var objectLockBo = ObjectLockService.Find(controller, objectId, authUserId);
            if (objectLockBo != null)
            {
                ObjectLockService.Delete(objectLockBo);
            }
        }

        public static void RefreshExpiryByUser(int authUserId)
        {
            var objectLockBos = ObjectLockService.GetByLockedUser(authUserId);
            foreach (var bo in objectLockBos)
            {
                var objectLockBo = bo;
                objectLockBo.RefreshExpiry(authUserId);
                ObjectLockService.Save(ref objectLockBo);
            } 
        }
    }
}