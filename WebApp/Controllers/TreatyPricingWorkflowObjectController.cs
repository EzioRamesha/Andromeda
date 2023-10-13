using BusinessObject.TreatyPricing;
using Services.TreatyPricing;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class TreatyPricingWorkflowObjectController : BaseController
    {
        [HttpPost]
        public JsonResult GetTreatyPricingObjects(int? type, int? cedantId = null)
        {
            IList<SelectListItem> items = GetEmptyDropDownList(false);
            string objectTypeName = "";

            if (type.HasValue)
            {
                objectTypeName = TreatyPricingWorkflowObjectBo.GetObjectTypeName(type.Value);
                foreach (object obj in TreatyPricingWorkflowObjectService.GetObjectsByType(type.Value, cedantId))
                {
                    string id = obj.GetPropertyValue("Id").ToString();
                    string code = TreatyPricingWorkflowObjectBo.GetCode(type.Value, obj);
                    string name = TreatyPricingWorkflowObjectBo.GetName(type.Value, obj);
                    items.Add(new SelectListItem() { Text = string.Format("{0} - {1}", code, name), Value = id });
                }
            }
            return Json(new { items, objectTypeName });
        }

        [HttpPost]
        public JsonResult GetTreatyPricingObjectVersions(int? type, int? objectId, List<int> existingVersionIds)
        {
            IList<SelectListItem> items = GetEmptyDropDownList(false);
            string objectCode = "";
            string objectName = "";

            if (type.HasValue && objectId.HasValue)
            {
                object parentObj = TreatyPricingWorkflowObjectService.FindObjectByType(type.Value, objectId.Value);
                objectCode = TreatyPricingWorkflowObjectBo.GetCode(type.Value, parentObj);
                objectName = TreatyPricingWorkflowObjectBo.GetName(type.Value, parentObj);

                foreach (object obj in TreatyPricingWorkflowObjectService.GetObjectVersionsByType(type.Value, objectId.Value, existingVersionIds))
                {
                    string id = obj.GetPropertyValue("Id").ToString();
                    string version = obj.GetPropertyValue("Version").ToString();
                    items.Add(new SelectListItem() { Text = string.Format("{0}.0", version), Value = id });
                }
            }
            return Json(new { items, objectCode, objectName });
        }

        [HttpPost]
        public JsonResult Create(TreatyPricingWorkflowObjectBo workflowObjectBo, bool loadObjectDetails = true, bool loadWorkflowDetails = false)
        {
            TrailObject trail = GetNewTrailObject();

            TreatyPricingWorkflowObjectBo bo = workflowObjectBo;
            bo.ObjectTypeName = TreatyPricingWorkflowObjectBo.GetObjectTypeName(bo.ObjectType);
            bo.CreatedById = AuthUserId;
            bo.UpdatedById = AuthUserId;

            TreatyPricingWorkflowObjectService.Create(ref bo, ref trail);
            if (loadObjectDetails)
                TreatyPricingWorkflowObjectService.LoadObjectDetails(ref bo);
            if (loadWorkflowDetails)
                TreatyPricingWorkflowObjectService.LoadWorkflowDetails(ref bo);

            return Json(new { bo });
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            TrailObject trail = GetNewTrailObject();

            TreatyPricingWorkflowObjectBo bo = TreatyPricingWorkflowObjectService.Find(id);
            if (bo != null)
            {
                TreatyPricingWorkflowObjectService.Delete(bo, ref trail);
            }

            return Json(new { });
        }

        [HttpPost]
        public JsonResult Search(TreatyPricingWorkflowObjectBo workflowObjectBo)
        {
            List<string> errors = new List<string>();
            if (workflowObjectBo.Type == 0)
            {
                errors.Add("Document Type is required");
                return Json(new { errors });
            }

            List<TreatyPricingWorkflowObjectBo> workflowObjectBos = new List<TreatyPricingWorkflowObjectBo>();
            switch (workflowObjectBo.Type)
            {
                case TreatyPricingWorkflowObjectBo.TypeQuotation:
                    workflowObjectBos.AddRange(TreatyPricingQuotationWorkflowService.GetWorkflowObjects(workflowObjectBo));
                    break;
                case TreatyPricingWorkflowObjectBo.TypeTreaty:
                    workflowObjectBos.AddRange(TreatyPricingTreatyWorkflowService.GetWorkflowObjects(workflowObjectBo));
                    break;
                default:
                    break;
            }

            if (workflowObjectBos.Count == 0)
            {
                errors.Add("No workflows found with parameters");
            }

            return Json(new { workflowObjectBos, errors });
        }

        public static void Create(IList<TreatyPricingWorkflowObjectBo> workflowObjectBos, int type, int workflowId, int authUserId, ref TrailObject trail)
        {
            foreach (TreatyPricingWorkflowObjectBo workflowObjectBo in workflowObjectBos)
            {
                TreatyPricingWorkflowObjectBo bo = workflowObjectBo;
                bo.Type = type;
                bo.WorkflowId = workflowId;
                bo.CreatedById = authUserId;
                bo.UpdatedById = authUserId;

                TreatyPricingWorkflowObjectService.Create(ref bo, ref trail);
            }
        }
    }
}