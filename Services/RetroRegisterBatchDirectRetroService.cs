using BusinessObject;
using BusinessObject.InvoiceRegisters;
using DataAccess.Entities;
using DataAccess.Entities.InvoiceRegisters;
using DataAccess.EntityFramework;
using Services.SoaDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class RetroRegisterBatchDirectRetroService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RetroRegisterBatchDirectRetro)),
            };
        }

        public static RetroRegisterBatchDirectRetroBo FormBo(RetroRegisterBatchDirectRetro entity = null)
        {
            if (entity == null)
                return null;
            return new RetroRegisterBatchDirectRetroBo
            {
                RetroRegisterBatchId = entity.RetroRegisterBatchId,
                DirectRetroId = entity.DirectRetroId,
                DirectRetroBo = DirectRetroService.Find(entity.DirectRetroId),
            };
        }

        public static RetroRegisterBatchDirectRetroBo FormSimplifiedBo(RetroRegisterBatchDirectRetro entity = null)
        {
            if (entity == null)
                return null;
            return new RetroRegisterBatchDirectRetroBo
            {
                RetroRegisterBatchId = entity.RetroRegisterBatchId,
                DirectRetroId = entity.DirectRetroId,
            };
        }

        public static IList<RetroRegisterBatchDirectRetroBo> FormBos(IList<RetroRegisterBatchDirectRetro> entities = null)
        {
            if (entities == null)
                return null;
            IList<RetroRegisterBatchDirectRetroBo> bos = new List<RetroRegisterBatchDirectRetroBo>() { };
            foreach (RetroRegisterBatchDirectRetro entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static IList<RetroRegisterBatchDirectRetroBo> FormSimplifiedBos(IList<RetroRegisterBatchDirectRetro> entities = null)
        {
            if (entities == null)
                return null;
            IList<RetroRegisterBatchDirectRetroBo> bos = new List<RetroRegisterBatchDirectRetroBo>() { };
            foreach (RetroRegisterBatchDirectRetro entity in entities)
            {
                bos.Add(FormSimplifiedBo(entity));
            }
            return bos;
        }

        public static RetroRegisterBatchDirectRetro FormEntity(RetroRegisterBatchDirectRetroBo bo = null)
        {
            if (bo == null)
                return null;
            return new RetroRegisterBatchDirectRetro
            {
                RetroRegisterBatchId = bo.RetroRegisterBatchId,
                DirectRetroId = bo.DirectRetroId,
            };
        }

        public static RetroRegisterBatchDirectRetroBo Find(int retroRegisterBatchId, int directRetroId)
        {
            return FormBo(RetroRegisterBatchDirectRetro.Find(retroRegisterBatchId, directRetroId));
        }

        public static int CountByRetroRegisterBatchId(int retroRegisterBatchId)
        {
            return RetroRegisterBatchDirectRetro.CountByRetroRegisterBatchId(retroRegisterBatchId);
        }

        public static List<int> GetIdsByRetroRegisterBatchId(int retroRegisterBatchId)
        {
            return RetroRegisterBatchDirectRetro.GetIdsByRetroRegisterBatchId(retroRegisterBatchId);
        }

        public static IList<RetroRegisterBatchDirectRetroBo> GetByRetroRegisterBatchId(int retroRegisterBatchId)
        {
            return FormBos(RetroRegisterBatchDirectRetro.GetByRetroRegisterBatchId(retroRegisterBatchId));
        }

        public static IList<RetroRegisterBatchDirectRetroBo> GetByRetroRegisterBatchId(int retroRegisterBatchId, int skip, int take)
        {
            return FormBos(RetroRegisterBatchDirectRetro.GetByRetroRegisterBatchId(retroRegisterBatchId, skip, take));
        }

        public static RetroRegisterBatchDirectRetroBo GetDirectRetroId(int retroRegisterBatchId, int? treatyCodeId, string soaQuarter)
        {
            using (var db = new AppDbContext(false))
            {
                return FormBo(db.RetroRegisterBatchDirectRetros
                    .Where(q => q.RetroRegisterBatchId == retroRegisterBatchId)
                    .Where(q => q.DirectRetro.TreatyCodeId == treatyCodeId)
                    .Where(q => q.DirectRetro.SoaQuarter == soaQuarter)
                    .FirstOrDefault());
            }
        }

        public static Result Create(ref RetroRegisterBatchDirectRetroBo bo)
        {
            RetroRegisterBatchDirectRetro entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
            }
            return result;
        }

        public static void Create(ref RetroRegisterBatchDirectRetroBo bo, AppDbContext db)
        {
            RetroRegisterBatchDirectRetro entity = FormEntity(bo);
            entity.Create(db);
        }

        public static Result Create(ref RetroRegisterBatchDirectRetroBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table, bo.PrimaryKey());
            }
            return result;
        }

        public static IList<DataTrail> DeleteAllByRetroRegisterBatchId(int retroRegisterBatchId)
        {
            return RetroRegisterBatchDirectRetro.DeleteAllByRetroRegisterBatchId(retroRegisterBatchId);
        }

        public static void DeleteAllByRetroRegisterBatchId(int retroRegisterBatchId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByRetroRegisterBatchId(retroRegisterBatchId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(RetroRegisterBatchDirectRetro)));
                }
            }
        }
    }
}
