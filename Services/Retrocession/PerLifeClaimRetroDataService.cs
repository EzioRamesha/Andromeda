using BusinessObject;
using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Retrocession
{
    public class PerLifeClaimRetroDataService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeClaimRetroData)),
                Controller = ModuleBo.ModuleController.PerLifeClaimRetroData.ToString()
            };
        }

        public static Expression<Func<PerLifeClaimRetroData, PerLifeClaimRetroDataBo>> Expression()
        {
            return entity => new PerLifeClaimRetroDataBo
            {
                Id = entity.Id,
                PerLifeClaimDataId = entity.PerLifeClaimDataId,
                MlreShare = entity.MlreShare,
                RetroClaimRecoveryAmount = entity.RetroClaimRecoveryAmount,
                LateInterest = entity.LateInterest,
                ExGratia = entity.ExGratia,
                RetroRecoveryId = entity.RetroRecoveryId,
                RetroTreatyId = entity.RetroTreatyId,
                RetroRatio = entity.RetroRatio,
                Aar = entity.Aar,
                ComputedRetroRecoveryAmount = entity.ComputedRetroRecoveryAmount,
                ComputedRetroLateInterest = entity.ComputedRetroLateInterest,
                ComputedRetroExGratia = entity.ComputedRetroExGratia,
                ReportedSoaQuarter = entity.ReportedSoaQuarter,
                RetroRecoveryAmount = entity.RetroRecoveryAmount,
                RetroLateInterest = entity.RetroLateInterest,
                RetroExGratia = entity.RetroExGratia,
                ComputedClaimCategory = entity.ComputedClaimCategory,
                ClaimCategory = entity.ClaimCategory,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static PerLifeClaimRetroDataBo FormBo(PerLifeClaimRetroData entity = null)
        {
            if (entity == null)
                return null;
            return new PerLifeClaimRetroDataBo
            {
                Id = entity.Id,
                PerLifeClaimDataId = entity.PerLifeClaimDataId,
                PerLifeClaimDataBo = PerLifeClaimDataService.Find(entity.PerLifeClaimDataId),
                MlreShare = entity.MlreShare,
                RetroClaimRecoveryAmount = entity.RetroClaimRecoveryAmount,
                LateInterest = entity.LateInterest,
                ExGratia = entity.ExGratia,
                RetroRecoveryId = entity.RetroRecoveryId,
                RetroTreatyId = entity.RetroTreatyId,
                RetroTreatyBo = RetroTreatyService.Find(entity.RetroTreatyId),
                RetroRatio = entity.RetroRatio,
                Aar = entity.Aar,
                ComputedRetroRecoveryAmount = entity.ComputedRetroRecoveryAmount,
                ComputedRetroLateInterest = entity.ComputedRetroLateInterest,
                ComputedRetroExGratia = entity.ComputedRetroExGratia,
                ReportedSoaQuarter = entity.ReportedSoaQuarter,
                RetroRecoveryAmount = entity.RetroRecoveryAmount,
                RetroLateInterest = entity.RetroLateInterest,
                RetroExGratia = entity.RetroExGratia,
                ComputedClaimCategory = entity.ComputedClaimCategory,
                ComputedClaimCategoryStr = PerLifeClaimDataBo.GetClaimCategoryName(entity.ComputedClaimCategory),
                ClaimCategory = entity.ClaimCategory,
                ClaimCategoryStr = PerLifeClaimDataBo.GetClaimCategoryName(entity.ClaimCategory),
                ClaimRegisterOffsetStatusStr = ClaimRegisterBo.GetOffsetStatusName(PerLifeClaimDataService.Find(entity.PerLifeClaimDataId).ClaimRegisterHistoryBo.OffsetStatus),
                ClaimRecoveryStatusStr = PerLifeClaimDataBo.GetClaimRetroDataRecovered(entity.ClaimCategory, PerLifeClaimDataService.Find(entity.PerLifeClaimDataId).PerLifeClaimBo.SoaQuarter),
                ClaimRecoveryDecisionStr = PerLifeClaimDataBo.GetClaimRecoveryDecisionName(PerLifeClaimDataService.Find(entity.PerLifeClaimDataId).ClaimRecoveryDecision),

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<PerLifeClaimRetroDataBo> FormBos(IList<PerLifeClaimRetroData> entities = null)
        {
            if (entities == null)
                return null;
            IList<PerLifeClaimRetroDataBo> bos = new List<PerLifeClaimRetroDataBo>() { };
            foreach (PerLifeClaimRetroData entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PerLifeClaimRetroData FormEntity(PerLifeClaimRetroDataBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeClaimRetroData
            {
                Id = bo.Id,
                PerLifeClaimDataId = bo.PerLifeClaimDataId,
                MlreShare = bo.MlreShare,
                RetroClaimRecoveryAmount = bo.RetroClaimRecoveryAmount,
                LateInterest = bo.LateInterest,
                ExGratia = bo.ExGratia,
                RetroRecoveryId = bo.RetroRecoveryId,
                RetroTreatyId = bo.RetroTreatyId,
                RetroRatio = bo.RetroRatio,
                Aar = bo.Aar,
                ComputedRetroRecoveryAmount = bo.ComputedRetroRecoveryAmount,
                ComputedRetroLateInterest = bo.ComputedRetroLateInterest,
                ComputedRetroExGratia = bo.ComputedRetroExGratia,
                ReportedSoaQuarter = bo.ReportedSoaQuarter,
                RetroRecoveryAmount = bo.RetroRecoveryAmount,
                RetroLateInterest = bo.RetroLateInterest,
                RetroExGratia = bo.RetroExGratia,
                ComputedClaimCategory = bo.ComputedClaimCategory,
                ClaimCategory = bo.ClaimCategory,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeClaimRetroData.IsExists(id);
        }

        public static PerLifeClaimRetroDataBo Find(int? id)
        {
            return FormBo(PerLifeClaimRetroData.Find(id));
        }

        public static IList<PerLifeClaimRetroDataBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeClaimRetroData.ToList());
            }
        }

        public static IList<PerLifeClaimRetroDataBo> GetByPerLifeClaimDataId(int perLifeClaimDataId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeClaimRetroData.Where(q => q.PerLifeClaimDataId == perLifeClaimDataId).ToList());
            }
        }

        public static int CountByRetroTreatyId(int retroTreatyId)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeClaimRetroData.Where(q => q.RetroTreatyId == retroTreatyId).Count();
            }
        }

        public static Result Save(ref PerLifeClaimRetroDataBo bo)
        {
            if (!PerLifeClaimRetroData.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeClaimRetroDataBo bo, ref TrailObject trail)
        {
            if (!PerLifeClaimRetroData.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref PerLifeClaimRetroDataBo bo)
        {
            PerLifeClaimRetroData entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeClaimRetroDataBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeClaimRetroDataBo bo)
        {
            Result result = Result();

            PerLifeClaimRetroData entity = PerLifeClaimRetroData.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.PerLifeClaimDataId = bo.PerLifeClaimDataId;
                entity.MlreShare = bo.MlreShare;
                entity.RetroClaimRecoveryAmount = bo.RetroClaimRecoveryAmount;
                entity.LateInterest = bo.LateInterest;
                entity.ExGratia = bo.ExGratia;
                entity.RetroRecoveryId = bo.RetroRecoveryId;
                entity.RetroTreatyId = bo.RetroTreatyId;
                entity.RetroRatio = bo.RetroRatio;
                entity.Aar = bo.Aar;
                entity.ComputedRetroRecoveryAmount = bo.ComputedRetroRecoveryAmount;
                entity.ComputedRetroLateInterest = bo.ComputedRetroLateInterest;
                entity.ComputedRetroExGratia = bo.ComputedRetroExGratia;
                entity.ReportedSoaQuarter = bo.ReportedSoaQuarter;
                entity.RetroRecoveryAmount = bo.RetroRecoveryAmount;
                entity.RetroLateInterest = bo.RetroLateInterest;
                entity.RetroExGratia = bo.RetroExGratia;
                entity.ComputedClaimCategory = bo.ComputedClaimCategory;
                entity.ClaimCategory = bo.ClaimCategory;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeClaimRetroDataBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeClaimRetroDataBo bo)
        {
            PerLifeClaimRetroData.Delete(bo.Id);
        }

        public static Result Delete(PerLifeClaimRetroDataBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                DataTrail dataTrail = PerLifeClaim.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByPerLifeClaimDataId(int perLifeClaimDataId)
        {
            return PerLifeClaimRetroData.DeleteAllByPerLifeClaimDataId(perLifeClaimDataId);
        }

        public static void DeleteAllByPerLifeClaimDataId(int perLifeClaimDataId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByPerLifeClaimDataId(perLifeClaimDataId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(PerLifeClaimRetroData)));
                }
            }
        }

        public static PerLifeClaimRetroDataBo FindPreviousQuarter(int? id)
        {
            PerLifeClaimRetroDataBo currentClaimDataBo = FormBo(PerLifeClaimRetroData.Find(id));
            int perLifeClaimId = currentClaimDataBo.PerLifeClaimDataBo.PerLifeClaimId;
            string currentSoaQuarter = PerLifeClaimService.Find(perLifeClaimId).SoaQuarter;

            int currentYear = Int32.Parse(currentSoaQuarter.Substring(0, 4));
            int currentQuarter = Int32.Parse(currentSoaQuarter.Substring(6, 1));
            string previousSoaQuarter;

            if (currentQuarter == 1)
            {
                previousSoaQuarter = (currentYear - 1).ToString() + " Q4";
            }
            else
            {
                previousSoaQuarter = currentYear.ToString() + " Q" + (currentQuarter - 1).ToString();
            }

            PerLifeClaimRetroDataBo previousClaimDataBo = FindBySoaQuarter(previousSoaQuarter, currentClaimDataBo.PerLifeClaimDataBo.ClaimRegisterHistoryBo.ClaimId);

            return previousClaimDataBo;
        }

        public static PerLifeClaimRetroDataBo FindBySoaQuarterAndId(string soaQuarter, string claimId)
        {
            if (PerLifeClaimService.FindBySoaQuarter(soaQuarter) != null)
            {
                int perLifeClaimId = FindBySoaQuarter(soaQuarter, claimId).Id;
                return Get().Where(q => q.PerLifeClaimDataBo.PerLifeClaimId == perLifeClaimId).FirstOrDefault();
            }
            else
            {
                return new PerLifeClaimRetroDataBo();
            }

        }

        public static PerLifeClaimRetroDataBo FindBySoaQuarter(string soaQuarter, string claimId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.PerLifeClaimRetroData
                    .Where(q => q.PerLifeClaimData.PerLifeClaim.SoaQuarter == soaQuarter && q.PerLifeClaimData.ClaimRegisterHistory.ClaimId == claimId)
                    .FirstOrDefault());
            }
        }
    }
}
