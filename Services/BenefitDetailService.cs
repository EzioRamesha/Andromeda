using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class BenefitDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(BenefitDetail)),
            };
        }

        public static BenefitDetailBo FormBo(BenefitDetail entity = null)
        {
            if (entity == null)
                return null;
            return new BenefitDetailBo
            {
                Id = entity.Id,
                BenefitId = entity.BenefitId,
                BenefitBo = BenefitService.Find(entity.BenefitId),
                EventCodeId = entity.EventCodeId,
                EventCodeBo = EventCodeService.Find(entity.EventCodeId),
                ClaimCodeId = entity.ClaimCodeId,
                ClaimCodeBo = ClaimCodeService.Find(entity.ClaimCodeId),
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<BenefitDetailBo> FormBos(IList<BenefitDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<BenefitDetailBo> bos = new List<BenefitDetailBo>() { };
            foreach (BenefitDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static BenefitDetail FormEntity(BenefitDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new BenefitDetail
            {
                Id = bo.Id,
                BenefitId = bo.BenefitId,
                EventCodeId = bo.EventCodeId,
                ClaimCodeId = bo.ClaimCodeId,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return BenefitDetail.IsExists(id);
        }

        public static IList<BenefitDetailBo> IsDuplicateCombination(IList<BenefitDetailBo> bos)
        {
            //return bos.GroupBy(x => new { x.EventCodeId, x.ClaimCodeId }).Where(x => x.Skip(1).Any()).Any();

            return bos.GroupBy(x => new { x.EventCodeId, x.ClaimCodeId })
                .Where(x => (x.Key.EventCodeId != 0 && x.Key.ClaimCodeId != 0) && x.Skip(1).Any())
                .Select(x => new BenefitDetailBo { EventCodeId = x.Key.EventCodeId, ClaimCodeId = x.Key.ClaimCodeId })
                .ToList();
        }

        public static int CountByParams(string mlreEventCode = null, string mlreBenefitCode = null)
        {
            using (var db = new AppDbContext())
            {
                IQueryable<BenefitDetail> query = BenefitDetail.QueryByParams(db, mlreEventCode, mlreBenefitCode);
                return query.Count();
            }
        }

        public static BenefitDetailBo FindByParams(string mlreEventCode = null, string mlreBenefitCode = null)
        {
            using (var db = new AppDbContext())
            {
                IQueryable<BenefitDetail> query = BenefitDetail.QueryByParams(db, mlreEventCode, mlreBenefitCode);
                return FormBo(query.FirstOrDefault());
            }
        }

        public static BenefitDetailBo FindByEventCode(string mlreEventCode = null)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.BenefitDetails.Where(q => q.EventCode.Code == mlreEventCode).FirstOrDefault());
            }
        }

        public static List<string> GetBenefitCodeByEventCode(string mlreEventCode = null)
        {
            using (var db = new AppDbContext())
            {
                return db.BenefitDetails.Where(q => q.EventCode.Code == mlreEventCode).GroupBy(q => q.BenefitId).Select(q => q.FirstOrDefault().Benefit.Code).ToList();
            }
        }

        public static BenefitDetailBo Find(int id)
        {
            return FormBo(BenefitDetail.Find(id));
        }

        public static BenefitDetailBo Find(int? id)
        {
            if (id == null)
                return null;
            return FormBo(BenefitDetail.Find(id.Value));
        }

        public static IList<BenefitDetailBo> GetByBenefitId(int benefitId)
        {
            return FormBos(BenefitDetail.GetByBenefitId(benefitId));
        }

        public static Result Save(BenefitDetailBo bo)
        {
            if (!BenefitDetail.IsExists(bo.Id))
            {
                return Create(bo);
            }
            return Update(bo);
        }

        public static Result Save(BenefitDetailBo bo, ref TrailObject trail)
        {
            if (!BenefitDetail.IsExists(bo.Id))
            {
                return Create(bo, ref trail);
            }
            return Update(bo, ref trail);
        }

        public static Result Create(BenefitDetailBo bo)
        {
            BenefitDetail entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(BenefitDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(BenefitDetailBo bo)
        {
            Result result = Result();

            BenefitDetail entity = BenefitDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            /*
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Code", bo.Code);
            }
            */

            if (result.Valid)
            {
                entity.BenefitId = bo.BenefitId;
                entity.EventCodeId = bo.EventCodeId;
                entity.ClaimCodeId = bo.ClaimCodeId;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(BenefitDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(BenefitDetailBo bo)
        {
            BenefitDetail.Delete(bo.Id);
        }

        public static Result Delete(BenefitDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = BenefitDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteAllByBenefitId(int benefitId)
        {
            return BenefitDetail.DeleteAllByBenefitId(benefitId);
        }

        public static void DeleteAllByBenefitId(int benefitId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByBenefitId(benefitId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(BenefitDetail)));
                }
            }
        }

        public static Result DeleteByBenefitIdExcept(int benefitId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<BenefitDetail> BenefitDetails = BenefitDetail.GetByBenefitIdExcept(benefitId, saveIds);
            foreach (BenefitDetail BenefitDetail in BenefitDetails)
            {
                DataTrail dataTrail = BenefitDetail.Delete(BenefitDetail.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Validate(IList<BenefitDetailBo> bos)
        {
            Result result = Result();

            List<string> errors = new List<string>();
            foreach (BenefitDetailBo bo in bos)
            {
                errors = bo.Validate();
            }

            // check for combination(MLRe Event Code & Claim Code) duplication
            foreach (BenefitDetailBo bo in IsDuplicateCombination(bos))
            {
                string EventCode = EventCodeService.Find(bo.EventCodeId).Code;
                string ClaimCode = ClaimCodeService.Find(bo.ClaimCodeId).Code;

                errors.Add(string.Format("Combination {0} '{1}' and {2} '{3}' is already taken.", 
                    "MLRe Event Code", EventCode, "Claim Code", ClaimCode));
            }

            foreach (string error in errors)
            {
                result.AddError(error);
            }
            return result;
        }
    }
}
