using BusinessObject;
using BusinessObject.Identity;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Services.TreatyPricing
{
    public class TreatyPricingTreatyWorkflowVersionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingTreatyWorkflowVersion)),
                Controller = ModuleBo.ModuleController.TreatyPricingTreatyWorkflowVersion.ToString()
            };
        }

        public static TreatyPricingTreatyWorkflowVersionBo FormBo(TreatyPricingTreatyWorkflowVersion entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new TreatyPricingTreatyWorkflowVersionBo
            {
                Id = entity.Id,
                TreatyPricingTreatyWorkflowId = entity.TreatyPricingTreatyWorkflowId,
                Version = entity.Version,

                // General tab
                RequestDate = entity.RequestDate,
                RequestDateStr = entity.RequestDate.HasValue ? entity.RequestDate.Value.ToString(Util.GetDateFormat()) : "",
                TargetSentDate = entity.TargetSentDate,
                TargetSentDateStr = entity.TargetSentDate.HasValue ? entity.TargetSentDate.Value.ToString(Util.GetDateFormat()) : "",
                DateSentToReviewer1st = entity.DateSentToReviewer1st,
                DateSentToReviewer1stStr = entity.DateSentToReviewer1st.HasValue ? entity.DateSentToReviewer1st.Value.ToString(Util.GetDateFormat()) : "",
                DateSentToClient1st = entity.DateSentToClient1st,
                DateSentToClient1stStr = entity.DateSentToClient1st.HasValue ? entity.DateSentToClient1st.Value.ToString(Util.GetDateFormat()) : "",
                LatestRevisionDate = entity.LatestRevisionDate,
                LatestRevisionDateStr = entity.LatestRevisionDate.HasValue ? entity.LatestRevisionDate.Value.ToString(Util.GetDateFormat()) : "",
                SignedDate = entity.SignedDate,
                SignedDateStr = entity.SignedDate.HasValue ? entity.SignedDate.Value.ToString(Util.GetDateFormat()) : "",
                ReportedDate = entity.ReportedDate,
                ReportedDateStr = entity.ReportedDate.HasValue ? entity.ReportedDate.Value.ToString(Util.GetDateFormat()) : "",
                PersonInChargeId = entity.PersonInChargeId,
                PersonInChargeName = UserService.Find(entity.PersonInChargeId)?.FullName,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };

            if (foreign)
            {
                bo.TreatyPricingTreatyWorkflowBo = TreatyPricingTreatyWorkflowService.Find(entity.TreatyPricingTreatyWorkflowId);
            }

            return bo;
        }

        public static TreatyPricingTreatyWorkflowVersionBo FormBoForTreatyDashboard(TreatyPricingTreatyWorkflowVersion entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;
            return new TreatyPricingTreatyWorkflowVersionBo
            {
                Id = entity.Id,
                TreatyPricingTreatyWorkflowId = entity.TreatyPricingTreatyWorkflowId,
                TreatyPricingTreatyWorkflowBo = foreign ? TreatyPricingTreatyWorkflowService.FindForTreatyDashboard(entity.TreatyPricingTreatyWorkflowId) : null,
                Version = entity.Version,
                RequestDate = entity.RequestDate,
                RequestDateStr = entity.RequestDate.HasValue ? entity.RequestDate.Value.ToString(Util.GetDateFormat()) : "",
                TargetSentDate = entity.TargetSentDate,
                TargetSentDateStr = entity.TargetSentDate.HasValue ? entity.TargetSentDate.Value.ToString(Util.GetDateFormat()) : "",
                DateSentToReviewer1st = entity.DateSentToReviewer1st,
                DateSentToReviewer1stStr = entity.DateSentToReviewer1st.HasValue ? entity.DateSentToReviewer1st.Value.ToString(Util.GetDateFormat()) : "",
                DateSentToClient1st = entity.DateSentToClient1st,
                DateSentToClient1stStr = entity.DateSentToClient1st.HasValue ? entity.DateSentToClient1st.Value.ToString(Util.GetDateFormat()) : "",
                LatestRevisionDate = entity.LatestRevisionDate,
                LatestRevisionDateStr = entity.LatestRevisionDate.HasValue ? entity.LatestRevisionDate.Value.ToString(Util.GetDateFormat()) : "",
                SignedDate = entity.SignedDate,
                SignedDateStr = entity.SignedDate.HasValue ? entity.SignedDate.Value.ToString(Util.GetDateFormat()) : "",
                ReportedDate = entity.ReportedDate,
                ReportedDateStr = entity.ReportedDate.HasValue ? entity.ReportedDate.Value.ToString(Util.GetDateFormat()) : "",
                PersonInChargeId = entity.PersonInChargeId,
            };
        }

        public static IList<TreatyPricingTreatyWorkflowVersionBo> FormBos(IList<TreatyPricingTreatyWorkflowVersion> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingTreatyWorkflowVersionBo> bos = new List<TreatyPricingTreatyWorkflowVersionBo>() { };
            foreach (TreatyPricingTreatyWorkflowVersion entity in entities.OrderBy(i => i.Version))
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static IList<TreatyPricingTreatyWorkflowVersionBo> FormBosForTreatyDashboard(IList<TreatyPricingTreatyWorkflowVersion> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingTreatyWorkflowVersionBo> bos = new List<TreatyPricingTreatyWorkflowVersionBo>() { };
            foreach (TreatyPricingTreatyWorkflowVersion entity in entities)
            {
                bos.Add(FormBoForTreatyDashboard(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingTreatyWorkflowVersion FormEntity(TreatyPricingTreatyWorkflowVersionBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingTreatyWorkflowVersion
            {
                Id = bo.Id,
                TreatyPricingTreatyWorkflowId = bo.TreatyPricingTreatyWorkflowId,
                Version = bo.Version,

                // General tab
                RequestDate = bo.RequestDate,
                TargetSentDate = bo.TargetSentDate,
                DateSentToReviewer1st = bo.DateSentToReviewer1st,
                DateSentToClient1st = bo.DateSentToClient1st,
                LatestRevisionDate = bo.LatestRevisionDate,
                SignedDate = bo.SignedDate,
                ReportedDate = bo.ReportedDate,
                PersonInChargeId = bo.PersonInChargeId,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingTreatyWorkflowVersion.IsExists(id);
        }

        public static TreatyPricingTreatyWorkflowVersionBo Find(int id)
        {
            return FormBo(TreatyPricingTreatyWorkflowVersion.Find(id));
        }

        public static TreatyPricingTreatyWorkflowVersionBo FindByTreatyPricingTreatyWorkflowIdVersion(int treatyPricingTreatyWorkflowId, int version)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingTreatyWorkflowVersions.Where(q => q.TreatyPricingTreatyWorkflowId == treatyPricingTreatyWorkflowId).Where(q => q.Version == version).FirstOrDefault());
            }
        }

        public static IList<TreatyPricingTreatyWorkflowVersionBo> GetByTreatyPricingTreatyWorkflowId(int treatyPricingTreatyWorkflowId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingTreatyWorkflowVersions
                    .Where(q => q.TreatyPricingTreatyWorkflowId == treatyPricingTreatyWorkflowId)
                    .OrderByDescending(q => q.Version)
                    .ToList());
            }
        }

        public static TreatyPricingTreatyWorkflowVersionBo GetLatestVersionByTreatyPricingTreatyWorkflowId(int treatyPricingTreatyWorkflowId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingTreatyWorkflowVersions
                    .Where(q => q.TreatyPricingTreatyWorkflowId == treatyPricingTreatyWorkflowId)
                    .OrderByDescending(q => q.Version)
                    .FirstOrDefault());
            }
        }

        public static TreatyPricingTreatyWorkflowVersionBo GetLatestVersionByTreatyPricingTreatyWorkflowIdForTreatyDashboard(int treatyPricingTreatyWorkflowId)
        {
            using (var db = new AppDbContext())
            {
                return FormBoForTreatyDashboard(db.TreatyPricingTreatyWorkflowVersions
                    .Where(q => q.TreatyPricingTreatyWorkflowId == treatyPricingTreatyWorkflowId)
                    .OrderByDescending(q => q.Version)
                    .FirstOrDefault());
            }
        }

        public static IList<TreatyPricingTreatyWorkflowVersionBo> GetLatestVersionOfTreatyWorkflow()
        {
            using (var db = new AppDbContext())
            {
                var treatyWorkflowDistinct = db.TreatyPricingTreatyWorkflowVersions.Select(q => q.TreatyPricingTreatyWorkflowId).Distinct().ToList();
                List<TreatyPricingTreatyWorkflowVersion> treatyWorkflowLatestVersion = new List<TreatyPricingTreatyWorkflowVersion>();

                foreach (var treatyWorkflow in treatyWorkflowDistinct)
                {
                    treatyWorkflowLatestVersion.Add(db.TreatyPricingTreatyWorkflowVersions
                        .Where(q => q.TreatyPricingTreatyWorkflowId == treatyWorkflow)
                        .OrderByDescending(q => q.Version)
                        .FirstOrDefault());
                }

                return FormBosForTreatyDashboard(treatyWorkflowLatestVersion, true);
            }
        }

        public static int GetVersionId(int TreatyWorkflowId, int TreatyWorkflowVersion)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingTreatyWorkflowVersions
                    .FirstOrDefault(q => q.TreatyPricingTreatyWorkflowId == TreatyWorkflowId
                        && q.Version == TreatyWorkflowVersion).Id;
            }
        }

        public static List<TreatyPricingTreatyWorkflowVersionBo> GetPendingInternalDashboard(UserBo user)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingTreatyWorkflowVersions
                    .Where(a => a.TreatyPricingTreatyWorkflow.DraftingStatusCategory == TreatyPricingTreatyWorkflowBo.DraftingStatusCategoryInternalReview)
                    .Where(a => (!string.IsNullOrEmpty(a.TreatyPricingTreatyWorkflow.Reviewer) && a.TreatyPricingTreatyWorkflow.Reviewer.Contains(user.Id.ToString().Trim())) || (a.PersonInChargeId.HasValue && a.PersonInChargeId == user.Id))
                    .ToList(), true).ToList();
            }
        }

        public static Result Save(ref TreatyPricingTreatyWorkflowVersionBo bo)
        {
            if (!TreatyPricingTreatyWorkflowVersion.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingTreatyWorkflowVersionBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingTreatyWorkflowVersion.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingTreatyWorkflowVersionBo bo)
        {
            TreatyPricingTreatyWorkflowVersion entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingTreatyWorkflowVersionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingTreatyWorkflowVersionBo bo)
        {
            Result result = Result();

            TreatyPricingTreatyWorkflowVersion entity = TreatyPricingTreatyWorkflowVersion.Find(bo.Id);
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

        public static Result Update(ref TreatyPricingTreatyWorkflowVersionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingTreatyWorkflowVersionBo bo)
        {
            TreatyPricingTreatyWorkflowVersion.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingTreatyWorkflowVersionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingTreatyWorkflowVersion.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingTreatyWorkflowId(int treatyPricingTreatyWorkflowId)
        {
            return TreatyPricingTreatyWorkflowVersion.DeleteAllByTreatyPricingTreatyWorkflowId(treatyPricingTreatyWorkflowId);
        }

        public static void DeleteAllByTreatyPricingTreatyWorkflowId(int treatyPricingTreatyWorkflowId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingTreatyWorkflowId(treatyPricingTreatyWorkflowId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingTreatyWorkflowVersion)));
                }
            }
        }

        #region Treaty Weekly/Monthly/Quarterly Report
        public static List<TreatyPricingTreatyWorkflowVersionBo> GetTreatyWorkflowLatestVersionBetweenDates(DateTime startDateSentToClient1st, DateTime endDateSentToClient1st)
        {
            var treatyWorkflowLatestVersionBo = GetLatestVersionOfTreatyWorkflow();

            return treatyWorkflowLatestVersionBo
                .Where(q => q.DateSentToClient1st.HasValue)
                .Where(q => q.DateSentToClient1st > startDateSentToClient1st && q.DateSentToClient1st < endDateSentToClient1st)
                .ToList();
        }

        public static List<TreatyPricingTreatyWorkflowVersionBo> GetTreatyWorkflowLatestVersionWithoutDateSentToClient1st()
        {
            var treatyWorkflowLatestVersionBo = GetLatestVersionOfTreatyWorkflow();

            return treatyWorkflowLatestVersionBo
                .Where(q => !q.DateSentToClient1st.HasValue)
                .ToList();
        }
        #endregion
    }
}
