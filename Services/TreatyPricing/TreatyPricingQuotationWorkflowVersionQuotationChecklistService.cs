using BusinessObject;
using BusinessObject.Identity;
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

namespace Services.TreatyPricing
{
    public class TreatyPricingQuotationWorkflowVersionQuotationChecklistService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingQuotationWorkflowVersionQuotationChecklist)),
                Controller = ModuleBo.ModuleController.TreatyPricingQuotationWorkflowVersionQuotationChecklist.ToString()
            };
        }

        public static Expression<Func<TreatyPricingQuotationWorkflowVersionQuotationChecklist, TreatyPricingQuotationWorkflowVersionQuotationChecklistBo>> Expression()
        {
            return entity => new TreatyPricingQuotationWorkflowVersionQuotationChecklistBo
            {
                Id = entity.Id,
                TreatyPricingQuotationWorkflowVersionId = entity.TreatyPricingQuotationWorkflowVersionId,
                InternalTeam = entity.InternalTeam,
                InternalTeamPersonInCharge = entity.InternalTeamPersonInCharge,
                Status = entity.Status,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingQuotationWorkflowVersionQuotationChecklistBo FormBo(TreatyPricingQuotationWorkflowVersionQuotationChecklist entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingQuotationWorkflowVersionQuotationChecklistBo
            {
                Id = entity.Id,
                TreatyPricingQuotationWorkflowVersionId = entity.TreatyPricingQuotationWorkflowVersionId,
                TreatyPricingQuotationWorkflowVersionBo = TreatyPricingQuotationWorkflowVersionService.Find(entity.TreatyPricingQuotationWorkflowVersionId),
                InternalTeam = entity.InternalTeam,
                InternalTeamPersonInCharge = entity.InternalTeamPersonInCharge,
                Status = entity.Status,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                DisableButtons = true,

                StatusName = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetStatusName(entity.Status),
            };
        }

        public static IList<TreatyPricingQuotationWorkflowVersionQuotationChecklistBo> FormBos(IList<TreatyPricingQuotationWorkflowVersionQuotationChecklist> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingQuotationWorkflowVersionQuotationChecklistBo> bos = new List<TreatyPricingQuotationWorkflowVersionQuotationChecklistBo>() { };
            foreach (TreatyPricingQuotationWorkflowVersionQuotationChecklist entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingQuotationWorkflowVersionQuotationChecklist FormEntity(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingQuotationWorkflowVersionQuotationChecklist
            {
                Id = bo.Id,
                TreatyPricingQuotationWorkflowVersionId = bo.TreatyPricingQuotationWorkflowVersionId,
                InternalTeam = bo.InternalTeam,
                InternalTeamPersonInCharge = bo.InternalTeamPersonInCharge,
                Status = bo.Status,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingQuotationWorkflowVersionQuotationChecklist.IsExists(id);
        }

        public static TreatyPricingQuotationWorkflowVersionQuotationChecklistBo Find(int id)
        {
            return FormBo(TreatyPricingQuotationWorkflowVersionQuotationChecklist.Find(id));
        }

        public static IList<TreatyPricingQuotationWorkflowVersionQuotationChecklistBo> GetByTreatyPricingQuotationWorkflowVersionId(int treatyPricingQuotationWorkflowVersionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingQuotationWorkflowVersionQuotationChecklists
                    .Where(q => q.TreatyPricingQuotationWorkflowVersionId == treatyPricingQuotationWorkflowVersionId)
                    .ToList());
            }
        }
        
        public static IList<TreatyPricingQuotationWorkflowVersionQuotationChecklistBo> GetAll()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingQuotationWorkflowVersionQuotationChecklists
                    .ToList());
            }
        }

        public static List<int> GetPendingInternalDashboard(UserBo user)
        {
            using (var db = new AppDbContext())
            {
               return db.TreatyPricingQuotationWorkflowVersionQuotationChecklists
                    .Where(a => a.Status == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.StatusRequested || a.Status == TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.StatusPendingSignOff)
                    //.Where(a => a.InternalTeam == user.DepartmentBo.Name)
                    .Where(a => !string.IsNullOrEmpty(a.InternalTeamPersonInCharge) && a.InternalTeamPersonInCharge.Contains(user.UserName))
                    .Select(a => a.TreatyPricingQuotationWorkflowVersionId)
                    .Distinct()
                    .ToList();
            }
        }

        public static List<TreatyPricingQuotationWorkflowVersionQuotationChecklistStatusForDashboardBo> GetChecklistStatusBosForDashboard()
        {
            List<TreatyPricingQuotationWorkflowVersionQuotationChecklistStatusForDashboardBo> bos = new List<TreatyPricingQuotationWorkflowVersionQuotationChecklistStatusForDashboardBo>();
            
            for (int i=2; i<=5; i++)
            {
                TreatyPricingQuotationWorkflowVersionQuotationChecklistStatusForDashboardBo bo = new TreatyPricingQuotationWorkflowVersionQuotationChecklistStatusForDashboardBo
                {
                    Status = i,
                    StatusName = TreatyPricingQuotationWorkflowVersionQuotationChecklistBo.GetStatusName(i)
                };

                bos.Add(bo);
            }

            return bos;
        }

        public static TreatyPricingQuotationWorkflowVersionQuotationChecklistBo GetByDepartmentAndVersion(string department, int versionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingQuotationWorkflowVersionQuotationChecklists
                    .Where(q => q.InternalTeam == department && q.TreatyPricingQuotationWorkflowVersionId == versionId)
                    .FirstOrDefault());
            }
        }

        public static Result Save(ref TreatyPricingQuotationWorkflowVersionQuotationChecklistBo bo)
        {
            if (!TreatyPricingQuotationWorkflowVersionQuotationChecklist.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingQuotationWorkflowVersionQuotationChecklistBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingQuotationWorkflowVersionQuotationChecklist.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingQuotationWorkflowVersionQuotationChecklistBo bo)
        {
            TreatyPricingQuotationWorkflowVersionQuotationChecklist entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingQuotationWorkflowVersionQuotationChecklistBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingQuotationWorkflowVersionQuotationChecklistBo bo)
        {
            Result result = Result();

            TreatyPricingQuotationWorkflowVersionQuotationChecklist entity = TreatyPricingQuotationWorkflowVersionQuotationChecklist.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingQuotationWorkflowVersionId = bo.TreatyPricingQuotationWorkflowVersionId;
                entity.InternalTeam = bo.InternalTeam;
                entity.InternalTeamPersonInCharge = bo.InternalTeamPersonInCharge;
                entity.Status = bo.Status;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingQuotationWorkflowVersionQuotationChecklistBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo bo)
        {
            TreatyPricingQuotationWorkflowVersionQuotationChecklist.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingQuotationWorkflowVersionQuotationChecklistBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingQuotationWorkflowVersionQuotationChecklist.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingQuotationWorkflowVersionId(int treatyPricingQuotationWorkflowVersionId)
        {
            return TreatyPricingQuotationWorkflowVersionQuotationChecklist.DeleteAllByTreatyPricingQuotationWorkflowVersionId(treatyPricingQuotationWorkflowVersionId);
        }

        public static void DeleteAllByTreatyPricingQuotationWorkflowVersionId(int treatyPricingQuotationWorkflowVersionId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingQuotationWorkflowVersionId(treatyPricingQuotationWorkflowVersionId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingQuotationWorkflowVersionQuotationChecklist)));
                }
            }
        }
    }
}
