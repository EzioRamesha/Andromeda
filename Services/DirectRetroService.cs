using BusinessObject;
using BusinessObject.SoaDatas;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Services.SoaDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DirectRetroService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(DirectRetro)),
                Controller = ModuleBo.ModuleController.DirectRetro.ToString()
            };
        }

        public static Expression<Func<DirectRetro, DirectRetroBo>> Expression()
        {
            return entity => new DirectRetroBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                TreatyCodeId = entity.TreatyCodeId,
                SoaQuarter = entity.SoaQuarter,
                SoaDataBatchId = entity.SoaDataBatchId,
                RetroStatus = entity.RetroStatus,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static DirectRetroBo FormBo(DirectRetro entity = null)
        {
            if (entity == null)
                return null;
            return new DirectRetroBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                CedantBo = CedantService.Find(entity.CedantId),
                TreatyCodeId = entity.TreatyCodeId,
                TreatyCodeBo = TreatyCodeService.Find(entity.TreatyCodeId),
                SoaQuarter = entity.SoaQuarter,
                SoaDataBatchId = entity.SoaDataBatchId,
                SoaDataBatchBo = SoaDataBatchService.Find(entity.SoaDataBatchId),
                RetroStatus = entity.RetroStatus,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<DirectRetroBo> FormBos(IList<DirectRetro> entities = null)
        {
            if (entities == null)
                return null;
            IList<DirectRetroBo> bos = new List<DirectRetroBo>() { };
            foreach (DirectRetro entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static DirectRetro FormEntity(DirectRetroBo bo = null)
        {
            if (bo == null)
                return null;
            return new DirectRetro
            {
                Id = bo.Id,
                CedantId = bo.CedantId,
                TreatyCodeId = bo.TreatyCodeId,
                SoaQuarter = bo.SoaQuarter,
                SoaDataBatchId = bo.SoaDataBatchId,
                RetroStatus = bo.RetroStatus,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return DirectRetro.IsExists(id);
        }

        public static DirectRetroBo Find(int id)
        {
            return FormBo(DirectRetro.Find(id));
        }

        public static DirectRetroBo FindByRetroStatus(int retroStatus)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.DirectRetro.Where(q => q.RetroStatus == retroStatus).FirstOrDefault());
            }
        }

        public static IList<DirectRetroBo> FindByCedantIdTreatyCodeIdQuarter(int? cedantId, int? treatyCodeId, string quarter)
        {
            using (var db = new AppDbContext(false))
            {
                List<int> ids = db.RetroRegisterBatchDirectRetros.Select(q => q.DirectRetroId).ToList();

                var query = db.DirectRetro
                    .Where(q => q.CedantId == cedantId)
                    .Where(q => q.RetroStatus == DirectRetroBo.RetroStatusApproved);

                if (treatyCodeId.HasValue)
                    query = query.Where(q => q.TreatyCodeId == treatyCodeId);

                if (!string.IsNullOrEmpty(quarter))
                    query = query.Where(q => q.SoaQuarter == quarter);

                if (ids != null)
                    query = query.Where(q => !ids.Contains(q.Id));

                return FormBos(query.OrderBy(q => q.Cedant.Name).ThenBy(q => q.SoaQuarter).ToList());
            }
        }

        public static DirectRetroBo FindByClaimRegisterParam(int soaDataBactchId, string treatyCode)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBo(db.DirectRetro.Where(q => q.SoaDataBatchId == soaDataBactchId && q.TreatyCode.Code == treatyCode).FirstOrDefault());
            }
        }

        public static int CountByRetroStatus(int retroStatus)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetro.Where(q => q.RetroStatus == retroStatus).Count();
            }
        }

        public static int CountBySoaDataBatchIdByExceptRetroStatus(int soaDataBatchId, int retroStatus)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetro
                    .Where(q => q.SoaDataBatchId == soaDataBatchId)
                    .Where(q => q.RetroStatus != retroStatus)
                    .Count();
            }
        }

        public static int CountByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetro.Where(q => q.CedantId == cedantId).Count();
            }
        }

        public static int CountByTreatyCodeId(int treatyCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetro.Where(q => q.TreatyCodeId == treatyCodeId).Count();
            }
        }

        public static int CountBySoaDataBatchId(int soaDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetro.Where(q => q.SoaDataBatchId == soaDataBatchId).Count();
            }
        }

        public static int CountByQuarterByExceptRetroStatusCompleted(string quarter)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetro.Where(q => q.SoaQuarter == quarter && q.RetroStatus != DirectRetroBo.RetroStatusCompleted).Count();
            }
        }

        public static IList<DirectRetroBo> GetByIds(List<int> ids)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBos(db.DirectRetro.Where(q => ids.Contains(q.Id)).ToList());
            }
        }

        public static IList<DirectRetroBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.DirectRetro.ToList());
            }
        }

        public static Result ValidateDuplicate(DirectRetroBo bo)
        {
            Result result = new Result();
            int count = 0; 

            using (var db = new AppDbContext())
            {
                count = db.DirectRetro
                    .Where(q => q.CedantId == bo.CedantId)
                    .Where(q => q.TreatyCodeId == bo.TreatyCodeId)
                    .Where(q => q.SoaQuarter == bo.SoaQuarter)
                    .Where(q => q.SoaDataBatchId == bo.SoaDataBatchId)
                    .Count();
            }

            if (count > 0)
                result.AddError("Existing Direct Retro Found");

            return result;
        }

        public static Result MapSoaDataBatch(ref DirectRetroBo bo)
        {
            Result result = new Result();
            if (bo.TreatyCodeBo == null)
            {
                result.AddError("Treaty Code Not Found");
                return result;
            }

            int? soaDatabatch = SoaDataBatchService.FindIdByTreatyIdQuarterExceptStatus(bo.TreatyCodeBo.TreatyId, bo.SoaQuarter, SoaDataBatchBo.StatusPendingDelete);

            if (!soaDatabatch.HasValue || (soaDatabatch.HasValue && soaDatabatch.Value == 0))
            {
                result.AddError(string.Format("SOA Data Batch Not Found with Treaty Id: {0} and Quarter: {1}", bo.TreatyCodeBo.TreatyId, bo.SoaQuarter));
                return result;
            }

            bo.SoaDataBatchId = soaDatabatch.Value;
            return result;
        }

        public static Result Save(ref DirectRetroBo bo)
        {
            if (!DirectRetro.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref DirectRetroBo bo, ref TrailObject trail)
        {
            if (!DirectRetro.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref DirectRetroBo bo)
        {
            DirectRetro entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref DirectRetroBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref DirectRetroBo bo)
        {
            Result result = Result();

            DirectRetro entity = DirectRetro.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.CedantId = bo.CedantId;
                entity.TreatyCodeId = bo.TreatyCodeId;
                entity.SoaQuarter = bo.SoaQuarter;
                entity.SoaDataBatchId = bo.SoaDataBatchId;
                entity.RetroStatus = bo.RetroStatus;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref DirectRetroBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(DirectRetroBo bo)
        {
            RetroSummaryService.DeleteByDirectRetroId(bo.Id);
            RetroStatementService.DeleteByDirectRetroId(bo.Id);
            DirectRetro.Delete(bo.Id);
        }

        public static Result Delete(DirectRetroBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (RetroStatementService.CountByDirectRetroIdByStatus(bo.Id, RetroStatementBo.StatusFinalised) > 0)
            {
                result.AddError(string.Format("Retro Statement(s) belongs to the Direct Retro have been Finalised", bo.Id));
            }

            if (result.Valid)
            {
                RetroSummaryService.DeleteByDirectRetroId(bo.Id, ref trail);
                RetroStatementService.DeleteByDirectRetroId(bo.Id, ref trail);
                DataTrail dataTrail = DirectRetro.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
