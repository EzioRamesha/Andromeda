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
    public class PerLifeClaimDataService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeClaimData)),
                Controller = ModuleBo.ModuleController.PerLifeClaimData.ToString()
            };
        }

        public static Expression<Func<PerLifeClaimData, PerLifeClaimDataBo>> Expression()
        {
            return entity => new PerLifeClaimDataBo
            {
                Id = entity.Id,
                PerLifeClaimId = entity.PerLifeClaimId,
                ClaimRegisterHistoryId = entity.ClaimRegisterHistoryId,
                PerLifeAggregationDetailDataId = entity.PerLifeAggregationDetailDataId,
                IsException = entity.IsException,
                ClaimCategory = entity.ClaimCategory,
                IsExcludePerformClaimRecovery = entity.IsExcludePerformClaimRecovery,
                ClaimRecoveryStatus = entity.ClaimRecoveryStatus,
                ClaimRecoveryDecision = entity.ClaimRecoveryDecision,
                MovementType = entity.MovementType,
                PerLifeRetro = entity.PerLifeRetro,
                RetroOutputId = entity.RetroOutputId,
                RetainPoolId = entity.RetainPoolId,
                NoOfRetroTreaty = entity.NoOfRetroTreaty,
                RetroRecoveryId = entity.RetroRecoveryId,
                IsLateInterestShare = entity.IsLateInterestShare,
                IsExGratiaShare = entity.IsExGratiaShare,
                Errors = entity.Errors,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static PerLifeClaimDataBo FormBo(PerLifeClaimData entity = null)
        {
            if (entity == null)
                return null;
            return new PerLifeClaimDataBo
            {
                Id = entity.Id,
                PerLifeClaimId = entity.PerLifeClaimId,
                PerLifeClaimBo = PerLifeClaimService.Find(entity.PerLifeClaimId),
                ClaimRegisterHistoryId = entity.ClaimRegisterHistoryId,
                ClaimRegisterHistoryBo = ClaimRegisterHistoryService.Find(entity.ClaimRegisterHistoryId),
                PerLifeAggregationDetailDataId = entity.PerLifeAggregationDetailDataId,
                PerLifeAggregationDetailDataBo = PerLifeAggregationDetailDataService.Find(entity.PerLifeAggregationDetailDataId),
                IsException = entity.IsException,
                ClaimCategory = entity.ClaimCategory,
                ClaimCategoryStr = PerLifeClaimDataBo.GetClaimCategoryName(entity.ClaimCategory),
                IsExcludePerformClaimRecovery = entity.IsExcludePerformClaimRecovery,
                ClaimRecoveryStatus = entity.ClaimRecoveryStatus,
                ClaimRecoveryStatusStr = PerLifeClaimDataBo.GetClaimRecoveryStatusName(entity.ClaimRecoveryStatus),
                ClaimRecoveryStatusRecoveredStr = entity.ClaimRecoveryStatus == PerLifeClaimDataBo.ClaimRecoveryStatusProcessingSuccess ? "Recovered" : "Won't Recover",
                ClaimRecoveryDecision = entity.ClaimRecoveryDecision,
                ClaimRecoveryDecisionStr = PerLifeClaimDataBo.GetClaimRecoveryDecisionName(entity.ClaimRecoveryDecision),
                MovementType = entity.MovementType,
                PerLifeRetro = entity.PerLifeRetro,
                RetroOutputId = entity.RetroOutputId,
                RetainPoolId = entity.RetainPoolId,
                NoOfRetroTreaty = entity.NoOfRetroTreaty,
                RetroRecoveryId = entity.RetroRecoveryId,
                IsLateInterestShare = entity.IsLateInterestShare,
                IsExGratiaShare = entity.IsExGratiaShare,
                Errors = entity.Errors,
                ClaimRegisterOffsetStatus = ClaimRegisterService.Find(ClaimRegisterHistoryService.Find(entity.ClaimRegisterHistoryId).ClaimRegisterId).OffsetStatus,
                ClaimRegisterOffsetStatusStr = ClaimRegisterBo.GetOffsetStatusName(ClaimRegisterService.Find(ClaimRegisterHistoryService.Find(entity.ClaimRegisterHistoryId).ClaimRegisterId).OffsetStatus),

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<PerLifeClaimDataBo> FormBos(IList<PerLifeClaimData> entities = null)
        {
            if (entities == null)
                return null;
            IList<PerLifeClaimDataBo> bos = new List<PerLifeClaimDataBo>() { };
            foreach (PerLifeClaimData entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PerLifeClaimData FormEntity(PerLifeClaimDataBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeClaimData
            {
                Id = bo.Id,
                PerLifeClaimId = bo.PerLifeClaimId,
                ClaimRegisterHistoryId = bo.ClaimRegisterHistoryId,
                PerLifeAggregationDetailDataId = bo.PerLifeAggregationDetailDataId,
                IsException = bo.IsException,
                ClaimCategory = bo.ClaimCategory,
                IsExcludePerformClaimRecovery = bo.IsExcludePerformClaimRecovery,
                ClaimRecoveryStatus = bo.ClaimRecoveryStatus,
                ClaimRecoveryDecision = bo.ClaimRecoveryDecision,
                MovementType = bo.MovementType,
                PerLifeRetro = bo.PerLifeRetro,
                RetroOutputId = bo.RetroOutputId,
                RetainPoolId = bo.RetainPoolId,
                NoOfRetroTreaty = bo.NoOfRetroTreaty,
                RetroRecoveryId = bo.RetroRecoveryId,
                IsLateInterestShare = bo.IsLateInterestShare,
                IsExGratiaShare = bo.IsExGratiaShare,
                Errors = bo.Errors,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeClaimData.IsExists(id);
        }

        public static PerLifeClaimDataBo Find(int? id)
        {
            return FormBo(PerLifeClaimData.Find(id));
        }

        public static PerLifeClaimDataBo FindPreviousQuarter(int? id)
        {
            PerLifeClaimDataBo currentClaimDataBo = FormBo(PerLifeClaimData.Find(id));
            int perLifeClaimId = currentClaimDataBo.PerLifeClaimId;
            int claimRegisterHistoryId = currentClaimDataBo.ClaimRegisterHistoryId;
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

            PerLifeClaimDataBo previousClaimDataBo = FindByClaimRegisterHistoryIdSoaQuarter(claimRegisterHistoryId, previousSoaQuarter);

            return previousClaimDataBo;
        }

        public static PerLifeClaimDataBo FindByClaimRegisterHistoryIdSoaQuarter(int claimRegisterHistoryId, string soaQuarter)
        {
            int claimRegisterId = ClaimRegisterHistoryService.Find(claimRegisterHistoryId).ClaimRegisterId;
            if (CutOffService.FindByQuarter(soaQuarter) != null)
            {
                int cutOffId = CutOffService.FindByQuarter(soaQuarter).Id;
                int previousClaimRegisterHistoryId = ClaimRegisterHistoryService.Find(cutOffId, claimRegisterId).Id;
                return Get().Where(q => q.ClaimRegisterHistoryId == previousClaimRegisterHistoryId).FirstOrDefault();
            }
            else
            {
                return new PerLifeClaimDataBo();
            }

        }

        public static IList<PerLifeClaimDataBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeClaimData.ToList());
            }
        }

        public static Result Save(ref PerLifeClaimDataBo bo)
        {
            if (!PerLifeClaimData.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeClaimDataBo bo, ref TrailObject trail)
        {
            if (!PerLifeClaimData.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref PerLifeClaimDataBo bo)
        {
            PerLifeClaimData entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeClaimDataBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeClaimDataBo bo)
        {
            Result result = Result();

            PerLifeClaimData entity = PerLifeClaimData.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.PerLifeClaimId = bo.PerLifeClaimId;
                entity.ClaimRegisterHistoryId = bo.ClaimRegisterHistoryId;
                entity.PerLifeAggregationDetailDataId = bo.PerLifeAggregationDetailDataId;
                entity.IsException = bo.IsException;
                entity.ClaimCategory = bo.ClaimCategory;
                entity.IsExcludePerformClaimRecovery = bo.IsExcludePerformClaimRecovery;
                entity.ClaimRecoveryStatus = bo.ClaimRecoveryStatus;
                entity.ClaimRecoveryDecision = bo.ClaimRecoveryDecision;
                entity.MovementType = bo.MovementType;
                entity.PerLifeRetro = bo.PerLifeRetro;
                entity.RetroOutputId = bo.RetroOutputId;
                entity.RetainPoolId = bo.RetainPoolId;
                entity.NoOfRetroTreaty = bo.NoOfRetroTreaty;
                entity.RetroRecoveryId = bo.RetroRecoveryId;
                entity.IsLateInterestShare = bo.IsLateInterestShare;
                entity.IsExGratiaShare = bo.IsExGratiaShare;
                entity.Errors = bo.Errors;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeClaimDataBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeClaimDataBo bo)
        {
            PerLifeClaimRetroDataService.DeleteAllByPerLifeClaimDataId(bo.Id);
            PerLifeClaimData.Delete(bo.Id);
        }

        public static Result Delete(PerLifeClaimDataBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                PerLifeClaimRetroDataService.DeleteAllByPerLifeClaimDataId(bo.Id, ref trail);
                DataTrail dataTrail = PerLifeClaimData.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByPerLifeClaimId(int perLifeClaimId)
        {
            return PerLifeClaimData.DeleteAllByPerLifeClaimId(perLifeClaimId);
        }

        public static void DeleteAllByPerLifeClaimId(int perLifeClaimId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByPerLifeClaimId(perLifeClaimId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(PerLifeClaimData)));
                }
            }
        }
    }
}
