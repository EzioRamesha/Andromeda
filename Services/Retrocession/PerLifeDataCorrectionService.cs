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
    public class PerLifeDataCorrectionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeDataCorrection)),
                Controller = ModuleBo.ModuleController.PerLifeDataCorrection.ToString()
            };
        }

        public static Expression<Func<PerLifeDataCorrection, PerLifeDataCorrectionBo>> Expression()
        {
            return entity => new PerLifeDataCorrectionBo
            {
                Id = entity.Id,
                TreatyCodeId = entity.TreatyCodeId,
                InsuredName = entity.InsuredName,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                PolicyNumber = entity.PolicyNumber,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                TerritoryOfIssueCodePickListDetailId = entity.TerritoryOfIssueCodePickListDetailId,
                PerLifeRetroGenderId = entity.PerLifeRetroGenderId,
                PerLifeRetroCountryId = entity.PerLifeRetroCountryId,
                DateOfPolicyExist = entity.DateOfPolicyExist,
                IsProceedToAggregate = entity.IsProceedToAggregate,
                DateOfExceptionDetected = entity.DateOfExceptionDetected,
                ExceptionStatusPickListDetailId = entity.ExceptionStatusPickListDetailId,
                Remark = entity.Remark,
                DateUpdated = entity.DateUpdated,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static PerLifeDataCorrectionBo FormBo(PerLifeDataCorrection entity = null)
        {
            if (entity == null)
                return null;
            return new PerLifeDataCorrectionBo
            {
                Id = entity.Id,
                TreatyCodeId = entity.TreatyCodeId,
                TreatyCodeBo = TreatyCodeService.Find(entity.TreatyCodeId),
                InsuredName = entity.InsuredName,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                PolicyNumber = entity.PolicyNumber,
                InsuredGenderCodePickListDetailId = entity.InsuredGenderCodePickListDetailId,
                InsuredGenderCodePickListDetailBo = PickListDetailService.Find(entity.InsuredGenderCodePickListDetailId),
                TerritoryOfIssueCodePickListDetailId = entity.TerritoryOfIssueCodePickListDetailId,
                TerritoryOfIssueCodePickListDetailBo = PickListDetailService.Find(entity.TerritoryOfIssueCodePickListDetailId),
                PerLifeRetroGenderId = entity.PerLifeRetroGenderId,
                PerLifeRetroGenderBo = PerLifeRetroGenderService.Find(entity.PerLifeRetroGenderId),
                PerLifeRetroCountryId = entity.PerLifeRetroCountryId,
                PerLifeRetroCountryBo = PerLifeRetroCountryService.Find(entity.PerLifeRetroCountryId),
                DateOfPolicyExist = entity.DateOfPolicyExist,
                IsProceedToAggregate = entity.IsProceedToAggregate,
                DateOfExceptionDetected = entity.DateOfExceptionDetected,
                ExceptionStatusPickListDetailId = entity.ExceptionStatusPickListDetailId,
                ExceptionStatusPickListDetailBo = PickListDetailService.Find(entity.ExceptionStatusPickListDetailId),
                Remark = entity.Remark,
                DateUpdated = entity.DateUpdated,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<PerLifeDataCorrectionBo> FormBos(IList<PerLifeDataCorrection> entities = null)
        {
            if (entities == null)
                return null;
            IList<PerLifeDataCorrectionBo> bos = new List<PerLifeDataCorrectionBo>() { };
            foreach (PerLifeDataCorrection entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PerLifeDataCorrection FormEntity(PerLifeDataCorrectionBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeDataCorrection
            {
                Id = bo.Id,
                TreatyCodeId = bo.TreatyCodeId,
                InsuredName = bo.InsuredName,
                InsuredDateOfBirth = bo.InsuredDateOfBirth,
                PolicyNumber = bo.PolicyNumber,
                InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId,
                TerritoryOfIssueCodePickListDetailId = bo.TerritoryOfIssueCodePickListDetailId,
                PerLifeRetroGenderId = bo.PerLifeRetroGenderId,
                PerLifeRetroCountryId = bo.PerLifeRetroCountryId,
                DateOfPolicyExist = bo.DateOfPolicyExist,
                IsProceedToAggregate = bo.IsProceedToAggregate,
                DateOfExceptionDetected = bo.DateOfExceptionDetected,
                ExceptionStatusPickListDetailId = bo.ExceptionStatusPickListDetailId,
                Remark = bo.Remark,
                DateUpdated = bo.DateUpdated,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeDataCorrection.IsExists(id);
        }

        public static PerLifeDataCorrectionBo Find(int? id)
        {
            return FormBo(PerLifeDataCorrection.Find(id));
        }

        public static IList<PerLifeDataCorrectionBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeDataCorrections.ToList());
            }
        }

        public static int CountByTreatyCodeId(int treatyCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeDataCorrections.Where(q => q.TreatyCodeId == treatyCodeId).Count();
            }
        }

        public static int CountByInsuredGenderCodePickListDetailId(int insuredGenderCodePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeDataCorrections.Where(q => q.InsuredGenderCodePickListDetailId == insuredGenderCodePickListDetailId).Count();
            }
        }

        public static int CountByTerritoryOfIssueCodePickListDetailId(int territoryOfIssueCodePickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeDataCorrections.Where(q => q.TerritoryOfIssueCodePickListDetailId == territoryOfIssueCodePickListDetailId).Count();
            }
        }

        public static int CountByPerLifeRetroGenderId(int perLifeRetroGenderId)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeDataCorrections.Where(q => q.PerLifeRetroGenderId == perLifeRetroGenderId).Count();
            }
        }

        public static int CountByPerLifeRetroCountryId(int perLifeRetroCountryId)
        {
            using (var db = new AppDbContext())
            {
                return db.PerLifeDataCorrections.Where(q => q.PerLifeRetroCountryId == perLifeRetroCountryId).Count();
            }
        }

        public static Result Save(ref PerLifeDataCorrectionBo bo)
        {
            if (!PerLifeDataCorrection.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeDataCorrectionBo bo, ref TrailObject trail)
        {
            if (!PerLifeDataCorrection.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicate(PerLifeDataCorrection perLifeDataCorrection)
        {
            return perLifeDataCorrection.IsDuplicate();
        }

        public static Result Create(ref PerLifeDataCorrectionBo bo)
        {
            PerLifeDataCorrection entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicate(entity))
            {
                result.AddError("Existing Per Life Data Correction found");
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeDataCorrectionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeDataCorrectionBo bo)
        {
            Result result = Result();

            PerLifeDataCorrection entity = PerLifeDataCorrection.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicate(FormEntity(bo)))
            {
                result.AddError("Existing Per Life Data Correction found");
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyCodeId = bo.TreatyCodeId;
                entity.InsuredName = bo.InsuredName;
                entity.InsuredDateOfBirth = bo.InsuredDateOfBirth;
                entity.PolicyNumber = bo.PolicyNumber;
                entity.InsuredGenderCodePickListDetailId = bo.InsuredGenderCodePickListDetailId;
                entity.TerritoryOfIssueCodePickListDetailId = bo.TerritoryOfIssueCodePickListDetailId;
                entity.PerLifeRetroGenderId = bo.PerLifeRetroGenderId;
                entity.PerLifeRetroCountryId = bo.PerLifeRetroCountryId;
                entity.DateOfPolicyExist = bo.DateOfPolicyExist;
                entity.IsProceedToAggregate = bo.IsProceedToAggregate;
                entity.DateOfExceptionDetected = bo.DateOfExceptionDetected;
                entity.ExceptionStatusPickListDetailId = bo.ExceptionStatusPickListDetailId;
                entity.Remark = bo.Remark;
                entity.DateUpdated = bo.DateUpdated;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeDataCorrectionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeDataCorrectionBo bo)
        {
            PerLifeDataCorrection.Delete(bo.Id);
        }

        public static Result Delete(PerLifeDataCorrectionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = PerLifeDataCorrection.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
