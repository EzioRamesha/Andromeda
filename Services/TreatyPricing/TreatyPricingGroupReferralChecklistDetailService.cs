using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.TreatyPricing
{
    public class TreatyPricingGroupReferralChecklistDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingGroupReferralChecklistDetail)),
                Controller = ModuleBo.ModuleController.TreatyPricingGroupReferralChecklistDetail.ToString()
            };
        }

        public static TreatyPricingGroupReferralChecklistDetailBo FormBo(TreatyPricingGroupReferralChecklistDetail entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingGroupReferralChecklistDetailBo
            {
                Id = entity.Id,
                TreatyPricingGroupReferralVersionId = entity.TreatyPricingGroupReferralVersionId,
                InternalItem = entity.InternalItem,
                Underwriting = entity.Underwriting,
                Health = entity.Health,
                Claim = entity.Claim,
                BD = entity.BD,
                CnR = entity.CnR,
                UltimateApprover = entity.UltimateApprover,
                GroupTeamApprover = entity.GroupTeamApprover,
                ReviewerApprover = entity.ReviewerApprover,
                HODApprover = entity.HODApprover,
                CEOApprover = entity.CEOApprover,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingGroupReferralChecklistDetailBo> FormBos(IList<TreatyPricingGroupReferralChecklistDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingGroupReferralChecklistDetailBo> bos = new List<TreatyPricingGroupReferralChecklistDetailBo>() { };
            foreach (TreatyPricingGroupReferralChecklistDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingGroupReferralChecklistDetail FormEntity(TreatyPricingGroupReferralChecklistDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingGroupReferralChecklistDetail
            {
                Id = bo.Id,
                TreatyPricingGroupReferralVersionId = bo.TreatyPricingGroupReferralVersionId,
                InternalItem = bo.InternalItem,
                Underwriting = bo.Underwriting,
                Health = bo.Health,
                Claim = bo.Claim,
                BD = bo.BD,
                CnR = bo.CnR,
                UltimateApprover = bo.UltimateApprover,
                GroupTeamApprover = bo.GroupTeamApprover,
                ReviewerApprover = bo.ReviewerApprover,
                HODApprover = bo.HODApprover,
                CEOApprover = bo.CEOApprover,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingGroupReferralChecklistDetail.IsExists(id);
        }

        public static TreatyPricingGroupReferralChecklistDetailBo Find(int id)
        {
            return FormBo(TreatyPricingGroupReferralChecklistDetail.Find(id));
        }

        public static IList<TreatyPricingGroupReferralChecklistDetailBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingGroupReferralChecklistDetails.ToList());
            }
        }

        public static IList<TreatyPricingGroupReferralChecklistDetailBo> GetByTreatyPricingGroupReferralVersionId(int treatyPricingGroupReferralVersionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingGroupReferralChecklistDetails
                    .Where(q => q.TreatyPricingGroupReferralVersionId == treatyPricingGroupReferralVersionId)
                    .ToList());
            }
        }

        public static Result Save(ref TreatyPricingGroupReferralChecklistDetailBo bo)
        {
            if (!TreatyPricingGroupReferralChecklistDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref TreatyPricingGroupReferralChecklistDetailBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingGroupReferralChecklistDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingGroupReferralChecklistDetailBo bo)
        {
            TreatyPricingGroupReferralChecklistDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingGroupReferralChecklistDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingGroupReferralChecklistDetailBo bo)
        {
            Result result = Result();

            TreatyPricingGroupReferralChecklistDetail entity = TreatyPricingGroupReferralChecklistDetail.Find(bo.Id);
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

        public static Result Update(ref TreatyPricingGroupReferralChecklistDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingGroupReferralChecklistDetailBo bo)
        {
            TreatyPricingGroupReferralChecklistDetail.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingGroupReferralChecklistDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingGroupReferralChecklistDetail.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static void Save(string json, int parentId, int authUserId, ref TrailObject trail, bool resetId = false)
        {
            List<TreatyPricingGroupReferralChecklistDetailBo> bos = new List<TreatyPricingGroupReferralChecklistDetailBo>();
            if (!string.IsNullOrEmpty(json))
                bos = JsonConvert.DeserializeObject<List<TreatyPricingGroupReferralChecklistDetailBo>>(json);

            foreach (var detailBo in bos)
            {
                var bo = detailBo;
                if (resetId)
                    bo.Id = 0;

                bo.TreatyPricingGroupReferralVersionId = parentId;
                bo.UpdatedById = authUserId;

                if (bo.Id == 0)
                    bo.CreatedById = authUserId;

                Save(ref bo, ref trail);
            }
        }

        public static void Update(string json, int authUserId, ref TrailObject trail, bool resetId = false)
        {
            List<TreatyPricingGroupReferralChecklistDetailBo> bos = new List<TreatyPricingGroupReferralChecklistDetailBo>();
            if (!string.IsNullOrEmpty(json))
                bos = JsonConvert.DeserializeObject<List<TreatyPricingGroupReferralChecklistDetailBo>>(json);

            foreach (var detailBo in bos)
            {
                var bo = detailBo;
                if (resetId)
                    bo.Id = 0;

                bo.UpdatedById = authUserId;

                if (bo.Id == 0)
                    bo.CreatedById = authUserId;

                Save(ref bo, ref trail);
            }
        }
    }
}
