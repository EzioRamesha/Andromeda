using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Services.TreatyPricing
{
    public class TreatyPricingWorkflowObjectService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingWorkflowObject)),
                Controller = ModuleBo.ModuleController.TreatyPricingWorkflowObject.ToString()
            };
        }

        public static TreatyPricingWorkflowObjectBo FormBo(TreatyPricingWorkflowObject entity = null, bool loadObjectDetails = false, bool loadWorkflowDetails = false)
        {
            if (entity == null)
                return null;
            TreatyPricingWorkflowObjectBo bo = new TreatyPricingWorkflowObjectBo
            {
                Id = entity.Id,
                Type = entity.Type,
                WorkflowId = entity.WorkflowId,
                ObjectType = entity.ObjectType,
                ObjectId = entity.ObjectId,
                ObjectVersionId = entity.ObjectVersionId,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };

            bo.TypeName = TreatyPricingWorkflowObjectBo.GetTypeName(bo.Type);
            if (loadObjectDetails)
            {
                LoadObjectDetails(ref bo);
            }

            if (loadWorkflowDetails)
            {
                LoadWorkflowDetails(ref bo);
            }

            return bo;
        }

        public static IList<TreatyPricingWorkflowObjectBo> FormBos(IList<TreatyPricingWorkflowObject> entities = null, bool loadObjectDetails = false, bool loadWorkflowDetails = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingWorkflowObjectBo> bos = new List<TreatyPricingWorkflowObjectBo>() { };
            foreach (TreatyPricingWorkflowObject entity in entities.OrderBy(i => i.ObjectType))
            {
                bos.Add(FormBo(entity, loadObjectDetails, loadWorkflowDetails));
            }
            return bos;
        }

        public static TreatyPricingWorkflowObject FormEntity(TreatyPricingWorkflowObjectBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingWorkflowObject
            {
                Id = bo.Id,
                Type = bo.Type,
                WorkflowId = bo.WorkflowId,
                ObjectType = bo.ObjectType,
                ObjectId = bo.ObjectId,
                ObjectVersionId = bo.ObjectVersionId,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingWorkflowObject.IsExists(id);
        }

        public static TreatyPricingWorkflowObjectBo Find(int id)
        {
            return FormBo(TreatyPricingWorkflowObject.Find(id));
        }

        public static TreatyPricingWorkflowObjectBo FindByObject(int objectType, int objectId, int objectVersionId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingWorkflowObjects
                    .Where(q => q.ObjectType == objectType)
                    .Where(q => q.ObjectId == objectId)
                    .Where(q => q.ObjectVersionId == objectVersionId);

                return FormBo(query.FirstOrDefault(), false, true);
            }
        }

        public static void LoadObjectDetails(ref TreatyPricingWorkflowObjectBo bo)
        {
            using (var db = new AppDbContext())
            {
                bo.ObjectClassName = TreatyPricingWorkflowObjectBo.GetObjectTypeClassName(bo.ObjectType);
                Type tableType = Type.GetType(string.Format("DataAccess.Entities.TreatyPricing.{0}, DataAccess", bo.ObjectClassName));
                Type versionTableType = Type.GetType(string.Format("DataAccess.Entities.TreatyPricing.{0}Version, DataAccess", bo.ObjectClassName));

                object obj = db.Set(tableType).Find(bo.ObjectId);
                object versionObj = db.Set(versionTableType).Find(bo.ObjectVersionId);
                if (obj != null && versionObj != null)
                {
                    bo.SetObjectDetails(obj, versionObj);
                }
            }
        }

        public static void LoadWorkflowDetails(ref TreatyPricingWorkflowObjectBo bo)
        {
            using (var db = new AppDbContext())
            {
                if (bo.Type == TreatyPricingWorkflowObjectBo.TypeQuotation)
                {
                    var quotationWorkflowBo = TreatyPricingQuotationWorkflowService.Find(bo.WorkflowId);
                    if (quotationWorkflowBo != null)
                    {
                        bo.WorkflowCode = quotationWorkflowBo.QuotationId;
                        bo.WorkflowStatus = quotationWorkflowBo.StatusName;
                    }
                }
                else if (bo.Type == TreatyPricingWorkflowObjectBo.TypeTreaty)
                {
                    var treatyWorkflowBo = TreatyPricingTreatyWorkflowService.Find(bo.WorkflowId);
                    if (treatyWorkflowBo != null)
                    {
                        bo.WorkflowCode = treatyWorkflowBo.DocumentId;
                        bo.WorkflowStatus = treatyWorkflowBo.DocumentStatusName;
                    }
                }
            }
        }

        public static IList<object> GetObjectsByType(int objectType, int? cedantId = null)
        {
            using (var db = new AppDbContext())
            {
                string objectTypeClassName = TreatyPricingWorkflowObjectBo.GetObjectTypeClassName(objectType);
                string typeName = string.Format("DataAccess.Entities.TreatyPricing.{0}, DataAccess", objectTypeClassName);

                Type type = Type.GetType(typeName);
                var objects = ((IQueryable<object>)db.Set(type)).ToList();
                if (!cedantId.HasValue || cedantId.Value == 0)
                {
                    return objects;
                }

                string cedantIdStr = cedantId.ToString();
                return objects.Where(q => q.GetPropertyValue("TreatyPricingCedantId").ToString() == cedantIdStr).ToList();
            }
        }

        public static object FindObjectByType(int objectType, int objectId)
        {
            using (var db = new AppDbContext())
            {
                string objectTypeClassName = TreatyPricingWorkflowObjectBo.GetObjectTypeClassName(objectType);
                string typeName = string.Format("DataAccess.Entities.TreatyPricing.{0}, DataAccess", objectTypeClassName);

                Type type = Type.GetType(typeName);
                return db.Set(type).Find(objectId);
            }
        }
        
        public static IList<object> GetObjectVersionsByType(int objectType, int objectId, List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                List<TreatyPricingWorkflowObject> workflowObjects = db.TreatyPricingWorkflowObjects
                    .Where(q => q.ObjectType == objectType)
                    .Where(q => q.ObjectId == objectId)
                    //.Select(q => q.ObjectVersionId)
                    .ToList();

                if (ids == null)
                    ids = new List<int>();
                foreach (var workflowObject in workflowObjects)
                {
                    if (workflowObject.Type == TreatyPricingWorkflowObjectBo.TypeQuotation)
                    {
                        int quotationStatus = db.TreatyPricingQuotationWorkflows.Where(q => q.Id == workflowObject.WorkflowId).Select(q => q.Status).FirstOrDefault();
                        if (quotationStatus == TreatyPricingQuotationWorkflowBo.StatusQuoted || quotationStatus == TreatyPricingQuotationWorkflowBo.StatusQuoting)
                            ids.Add(workflowObject.ObjectVersionId);
                    }
                    else if (workflowObject.Type == TreatyPricingWorkflowObjectBo.TypeTreaty)
                    {
                        int treatyStatus = db.TreatyPricingTreatyWorkflows.Where(q => q.Id == workflowObject.WorkflowId).Select(q => q.DocumentStatus ?? 0).FirstOrDefault();
                        if (treatyStatus != TreatyPricingTreatyWorkflowBo.DocumentStatusNotUsed && treatyStatus != TreatyPricingTreatyWorkflowBo.DocumentStatusSigned)
                            ids.Add(workflowObject.ObjectVersionId);
                    }
                }

                IQueryable<object> query;
                switch (objectType)
                {
                    case TreatyPricingWorkflowObjectBo.ObjectTypeAdvantageProgram:
                        query = db.TreatyPricingAdvantageProgramVersions.Where(q => q.TreatyPricingAdvantageProgramId == objectId).Where(q => !ids.Contains(q.Id));
                        break;
                    case TreatyPricingWorkflowObjectBo.ObjectTypeCampaign:
                        query = db.TreatyPricingCampaignVersions.Where(q => q.TreatyPricingCampaignId == objectId).Where(q => !ids.Contains(q.Id));
                        break;
                    case TreatyPricingWorkflowObjectBo.ObjectTypeClaimApprovalLimit:
                        query = db.TreatyPricingClaimApprovalLimitVersions.Where(q => q.TreatyPricingClaimApprovalLimitId == objectId).Where(q => !ids.Contains(q.Id));
                        break;
                    case TreatyPricingWorkflowObjectBo.ObjectTypeCustomOther:
                        query = db.TreatyPricingCustomOtherVersions.Where(q => q.TreatyPricingCustomOtherId == objectId).Where(q => !ids.Contains(q.Id));
                        break;
                    case TreatyPricingWorkflowObjectBo.ObjectTypeDefinitionAndExclusion:
                        query = db.TreatyPricingDefinitionAndExclusionVersions.Where(q => q.TreatyPricingDefinitionAndExclusionId == objectId).Where(q => !ids.Contains(q.Id));
                        break;
                    case TreatyPricingWorkflowObjectBo.ObjectTypeFinancialTable:
                        query = db.TreatyPricingFinancialTableVersions.Where(q => q.TreatyPricingFinancialTableId == objectId).Where(q => !ids.Contains(q.Id));
                        break;
                    case TreatyPricingWorkflowObjectBo.ObjectTypeMedicalTable:
                        query = db.TreatyPricingMedicalTableVersions.Where(q => q.TreatyPricingMedicalTableId == objectId).Where(q => !ids.Contains(q.Id));
                        break;
                    case TreatyPricingWorkflowObjectBo.ObjectTypeProduct:
                        query = db.TreatyPricingProductVersions.Where(q => q.TreatyPricingProductId == objectId).Where(q => !ids.Contains(q.Id));
                        break;
                    case TreatyPricingWorkflowObjectBo.ObjectTypeProfitCommission:
                        query = db.TreatyPricingProfitCommissionVersions.Where(q => q.TreatyPricingProfitCommissionId == objectId).Where(q => !ids.Contains(q.Id));
                        break;
                    case TreatyPricingWorkflowObjectBo.ObjectTypeRateTable:
                        query = db.TreatyPricingRateTableVersions.Where(q => q.TreatyPricingRateTableId == objectId).Where(q => !ids.Contains(q.Id));
                        break;
                    case TreatyPricingWorkflowObjectBo.ObjectTypeUwLimit:
                        query = db.TreatyPricingUwLimitVersions.Where(q => q.TreatyPricingUwLimitId == objectId).Where(q => !ids.Contains(q.Id));
                        break;
                    case TreatyPricingWorkflowObjectBo.ObjectTypeUwQuestionnaire:
                        query = db.TreatyPricingUwQuestionnaireVersions.Where(q => q.TreatyPricingUwQuestionnaireId == objectId).Where(q => !ids.Contains(q.Id));
                        break;
                    default:
                        return null;
                }

                return query.ToList();
            }
        }

        public static IList<TreatyPricingWorkflowObjectBo> GetByTypeWorkflowId(int type, int workflowId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingWorkflowObjects
                    .Where(q => q.Type == type)
                    .Where(q => q.WorkflowId == workflowId)
                    .OrderBy(q => q.ObjectType)
                    .ThenBy(q => q.ObjectId)
                    .ThenBy(q => q.ObjectVersionId);

                return FormBos(query.ToList(), true);
            }
        }

        public static TreatyPricingWorkflowObjectBo GetByTypeWorkflowIdObjectType(int type, int workflowId, int objectType)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingWorkflowObjects
                    .Where(q => q.Type == type)
                    .Where(q => q.WorkflowId == workflowId)
                    .Where(q => q.ObjectType == objectType)
                    .FirstOrDefault();

                return FormBo(query, true);
            }
        }

        public static TreatyPricingWorkflowObjectBo GetByTypeObjectTypeObjectIdObjectVersionId(int type, int objectType, int objectId, int objectVersionId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingWorkflowObjects
                    .Where(q => q.Type == type)
                    .Where(q => q.ObjectType == objectType)
                    .Where(q => q.ObjectId == objectId)
                    .Where(q => q.ObjectVersionId == objectVersionId)
                    .FirstOrDefault();

                return FormBo(query, true);
            }
        }

        public static IList<TreatyPricingWorkflowObjectBo> GetByObjectTypeObjectId(int objectType, int objectId, bool loadWorkflowDetails = false)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingWorkflowObjects
                    .Where(q => q.ObjectType == objectType)
                    .Where(q => q.ObjectId == objectId)
                    .ToList();

                return FormBos(query, true, loadWorkflowDetails);
            }
        }

        public static TreatyPricingWorkflowObjectBo FindLatestByObjectTypeObjectId(int objectType, int objectId)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingWorkflowObjects
                    .Where(q => q.ObjectType == objectType)
                    .Where(q => q.ObjectId == objectId)
                    .OrderByDescending(q => q.ObjectVersionId)
                    .FirstOrDefault();

                return FormBo(query, loadWorkflowDetails: true);
            }
        }

        public static Result Save(ref TreatyPricingWorkflowObjectBo bo)
        {
            if (!TreatyPricingWorkflowObject.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingWorkflowObjectBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingWorkflowObject.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingWorkflowObjectBo bo)
        {
            TreatyPricingWorkflowObject entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingWorkflowObjectBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingWorkflowObjectBo bo)
        {
            Result result = Result();

            TreatyPricingWorkflowObject entity = TreatyPricingWorkflowObject.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity = FormEntity(bo);
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingWorkflowObjectBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingWorkflowObjectBo bo)
        {
            TreatyPricingWorkflowObject.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingWorkflowObjectBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingWorkflowObject.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTypeWorkflowId(int type, int workflowId)
        {
            return TreatyPricingWorkflowObject.DeleteAllByTypeWorkflowId(type, workflowId);
        }

        public static void DeleteAllByTreatyQuotationWorkflowId(int type, int workflowId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTypeWorkflowId(type, workflowId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingWorkflowObject)));
                }
            }
        }
    }
}
