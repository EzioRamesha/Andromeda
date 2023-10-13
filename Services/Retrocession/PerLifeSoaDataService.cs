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
    public class PerLifeSoaDataService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeSoaData)),
                Controller = ModuleBo.ModuleController.PerLifeSoaData.ToString()
            };
        }

        public static Expression<Func<PerLifeSoaData, PerLifeSoaDataBo>> Expression()
        {
            return entity => new PerLifeSoaDataBo
            {
                Id = entity.Id,
                PerLifeSoaId = entity.PerLifeSoaId,
                PerLifeAggregationDetailDataId = entity.PerLifeAggregationDetailDataId,
                PerLifeClaimDataId = entity.PerLifeClaimDataId,
            };
        }

        public static Expression<Func<PerLifeClaimRetroData, PerLifeSoaDataBo>> ClaimExpression()
        {
            return entity => new PerLifeSoaDataBo
            {
                Id = entity.Id,
                PerLifeClaimDataId = entity.PerLifeClaimDataId,
                LateInterest = entity.PerLifeClaimData.ClaimRegisterHistory.LateInterest,
                ExGratia = entity.PerLifeClaimData.ClaimRegisterHistory.ExGratia,
                RetroSoaQuarter = entity.ReportedSoaQuarter,
                RetroRecoveryAmount = entity.RetroClaimRecoveryAmount,
                RetroLateInterest = entity.LateInterest,
                RetroExGratia = entity.ExGratia,
                ClaimCategory = entity.ClaimCategory,

                EntryNo = entity.PerLifeClaimData.ClaimRegisterHistory.EntryNo,
                ClaimId = entity.PerLifeClaimData.ClaimRegisterHistory.ClaimId,
                ClaimTransactionType = entity.PerLifeClaimData.ClaimRegisterHistory.ClaimTransactionType,
                InsuredName = entity.PerLifeClaimData.ClaimRegisterHistory.InsuredName,
                InsuredDateOfBirth = entity.PerLifeClaimData.ClaimRegisterHistory.InsuredDateOfBirth,
                InsuredGenderCode = entity.PerLifeClaimData.ClaimRegisterHistory.InsuredGenderCode,
                PolicyNumber = entity.PerLifeClaimData.ClaimRegisterHistory.PolicyNumber,
                CedantDateOfNotification = entity.PerLifeClaimData.ClaimRegisterHistory.CedantDateOfNotification,
                ReinsBasisCode = entity.PerLifeClaimData.ClaimRegisterHistory.ReinsBasisCode,
                ReinsEffDatePol = entity.PerLifeClaimData.ClaimRegisterHistory.ReinsEffDatePol,
                TreatyCode = entity.PerLifeClaimData.ClaimRegisterHistory.TreatyCode,
                ClaimCode = entity.PerLifeClaimData.ClaimRegisterHistory.ClaimCode,
                MlreBenefitCode = entity.PerLifeClaimData.ClaimRegisterHistory.MlreBenefitCode,
                ClaimRecoveryAmt = entity.PerLifeClaimData.ClaimRegisterHistory.ClaimRecoveryAmt,
                MlreRetainAmount = entity.PerLifeClaimData.ClaimRegisterHistory.MlreRetainAmount,
                CauseOfEvent = entity.PerLifeClaimData.ClaimRegisterHistory.CauseOfEvent,
                DateOfEvent = entity.PerLifeClaimData.ClaimRegisterHistory.DateOfEvent,
                ClaimStatus = entity.PerLifeClaimData.ClaimRegisterHistory.ClaimStatus,
                OffsetStatus = entity.PerLifeClaimData.ClaimRegisterHistory.OffsetStatus,
            };
        }

        public static PerLifeSoaDataBo FormBo(PerLifeSoaData entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new PerLifeSoaDataBo
            {
                Id = entity.Id,
                PerLifeSoaId = entity.PerLifeSoaId,
                PerLifeAggregationDetailDataId = entity.PerLifeAggregationDetailDataId,
                PerLifeClaimDataId = entity.PerLifeClaimDataId,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
            if (foreign)
            {
                bo.PerLifeSoaBo = PerLifeSoaService.Find(entity.PerLifeSoaId);
                if (entity.PerLifeAggregationDetailDataId.HasValue)
                    bo.PerLifeAggregationDetailDataBo = PerLifeAggregationDetailDataService.Find(entity.PerLifeAggregationDetailDataId);
                if (entity.PerLifeClaimDataId.HasValue)
                    bo.PerLifeClaimDataBo = PerLifeClaimDataService.Find(entity.PerLifeClaimDataId);
            }
            return bo;
        }

        public static IList<PerLifeSoaDataBo> FormBos(IList<PerLifeSoaData> entities = null)
        {
            if (entities == null)
                return null;
            IList<PerLifeSoaDataBo> bos = new List<PerLifeSoaDataBo>() { };
            foreach (PerLifeSoaData entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PerLifeSoaData FormEntity(PerLifeSoaDataBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeSoaData
            {
                Id = bo.Id,
                PerLifeSoaId = bo.PerLifeSoaId,
                PerLifeAggregationDetailDataId = bo.PerLifeAggregationDetailDataId,
                PerLifeClaimDataId = bo.PerLifeClaimDataId,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeSoaData.IsExists(id);
        }

        public static PerLifeSoaDataBo Find(int? id)
        {
            return FormBo(PerLifeSoaData.Find(id));
        }

        public static IList<PerLifeSoaDataBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeSoaData.OrderBy(q => q.Id).ToList());
            }
        }

        public static IList<PerLifeSoaDataBo> GetByPerLifeSoaIdCaimCategory(int perLifeSoaId, int claimCategory)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeSoaData.Where(q => q.PerLifeSoaId == perLifeSoaId && q.PerLifeClaimData.ClaimCategory == claimCategory).OrderBy(q => q.Id).ToList());
            }
        }

        public static IList<PerLifeSoaDataBo> GetByPerLifeSoaIdCaimCategoryWMOM(int perLifeSoaId, int claimCategory, int wmom)
        {
            using (var db = new AppDbContext())
            {
                var query = db.PerLifeSoaData.Where(q => q.PerLifeSoaId == perLifeSoaId && q.PerLifeClaimData.ClaimCategory == claimCategory);
                if (wmom == PerLifeSoaSummariesBo.WMOMWithin)
                    query = query.Where(q => q.PerLifeClaimData.ClaimRegisterHistory.ClaimId.StartsWith("CL"));
                else if(wmom == PerLifeSoaSummariesBo.WMOMOutside)
                    query = query.Where(q => q.PerLifeClaimData.ClaimRegisterHistory.ClaimId.StartsWith("OCL"));

                return FormBos(query.OrderBy(q => q.Id).ToList());
            }
        }

        public static IList<PerLifeSoaDataBo> GetAllByPerLifeSoaId(int perLifeSoaId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeSoaData.Where(q => q.PerLifeSoaId == perLifeSoaId).OrderBy(q => q.Id).ToList());
            }
        }

        public static Result Save(ref PerLifeSoaDataBo bo)
        {
            if (!PerLifeSoaData.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeSoaDataBo bo, ref TrailObject trail)
        {
            if (!PerLifeSoaData.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref PerLifeSoaDataBo bo)
        {
            PerLifeSoaData entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeSoaDataBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeSoaDataBo bo)
        {
            Result result = Result();

            PerLifeSoaData entity = PerLifeSoaData.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.PerLifeSoaId = bo.PerLifeSoaId;
                entity.PerLifeAggregationDetailDataId = bo.PerLifeAggregationDetailDataId;
                entity.PerLifeClaimDataId = bo.PerLifeClaimDataId;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeSoaDataBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeSoaDataBo bo)
        {
            PerLifeSoaData.Delete(bo.Id);
        }

        public static Result Delete(PerLifeSoaDataBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = PerLifeSoaData.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
