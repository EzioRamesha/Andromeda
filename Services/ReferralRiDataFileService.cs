using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class ReferralRiDataFileService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ReferralRiDataFile)),
                Controller = ModuleBo.ModuleController.ClaimRegisterFile.ToString(),
            };
        }

        public static ReferralRiDataFileBo FormBo(ReferralRiDataFile entity = null)
        {
            if (entity == null)
                return null;
            ReferralRiDataFileBo bo = new ReferralRiDataFileBo
            {
                Id = entity.Id,
                RawFileId = entity.RawFileId,
                RawFileBo = RawFileService.Find(entity.RawFileId),
                Records = entity.Records,
                UpdatedRecords = entity.UpdatedRecords,
                Error = entity.Error,
                CreatedAt = entity.CreatedAt,
                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateTimeFormat()),
                CreatedById = entity.CreatedById,
                CreatedByName = UserService.Find(entity.CreatedById)?.UserName,
                UpdatedById = entity.UpdatedById,
            };

            if (bo.RawFileBo.Status == RawFileBo.StatusCompletedFailed && !string.IsNullOrEmpty(bo.Error))
            {
                bo.Errors = JsonConvert.DeserializeObject<List<string>>(bo.Error);
            }

            return bo;
        }

        public static IList<ReferralRiDataFileBo> FormBos(IList<ReferralRiDataFile> entities = null)
        {
            if (entities == null)
                return null;
            IList<ReferralRiDataFileBo> bos = new List<ReferralRiDataFileBo>() { };
            foreach (ReferralRiDataFile entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ReferralRiDataFile FormEntity(ReferralRiDataFileBo bo = null)
        {
            if (bo == null)
                return null;
            return new ReferralRiDataFile
            {
                Id = bo.Id,
                RawFileId = bo.RawFileId,
                Records = bo.Records,
                UpdatedRecords = bo.UpdatedRecords,
                Error = bo.Error,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static ReferralRiDataFileBo Find(int? id)
        {
            return FormBo(ReferralRiDataFile.Find(id));
        }

        public static ReferralRiDataFileBo FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.ReferralRiDataFiles.Where(q => q.RawFile.Status == status).FirstOrDefault());
            }
        }

        public static IList<ReferralRiDataFileBo> Get()
        {
            return FormBos(ReferralRiDataFile.Get());
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.ReferralRiDataFiles.Where(q => q.RawFile.Status == status).Count();
            }
        }

        public static Result Create(ref ReferralRiDataFileBo bo)
        {
            ReferralRiDataFile entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ReferralRiDataFileBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ReferralRiDataFileBo bo)
        {
            Result result = Result();

            ReferralRiDataFile entity = ReferralRiDataFile.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.RawFileId = bo.RawFileId;
                entity.Records = bo.Records;
                entity.UpdatedRecords = bo.UpdatedRecords;
                entity.Error = bo.Error;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ReferralRiDataFileBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }
    }
}
