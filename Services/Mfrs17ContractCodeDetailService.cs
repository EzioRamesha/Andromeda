using BusinessObject;
using BusinessObject.RiDatas;
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
    public class Mfrs17ContractCodeDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Mfrs17ContractCodeDetail)),
                Controller = ModuleBo.ModuleController.Mfrs17ContractCodeDetail.ToString()
            };
        }

        public static Mfrs17ContractCodeDetailBo FormBo(Mfrs17ContractCodeDetail entity = null)
        {
            if (entity == null)
                return null;
            return new Mfrs17ContractCodeDetailBo
            {
                Id = entity.Id,
                Mfrs17ContractCodeId = entity.Mfrs17ContractCodeId,
                Mfrs17ContractCodeBo = Mfrs17ContractCodeService.Find(entity.Mfrs17ContractCodeId),
                ContractCode = entity.ContractCode,
            };
        }

        public static IList<Mfrs17ContractCodeDetailBo> FormBos(IList<Mfrs17ContractCodeDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<Mfrs17ContractCodeDetailBo> bos = new List<Mfrs17ContractCodeDetailBo>() { };
            foreach (Mfrs17ContractCodeDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static Mfrs17ContractCodeDetail FormEntity(Mfrs17ContractCodeDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new Mfrs17ContractCodeDetail
            {
                Id = bo.Id,
                Mfrs17ContractCodeId = bo.Mfrs17ContractCodeId,
                ContractCode = bo.ContractCode?.Trim(),
                CreatedById = bo.CreatedById,
                CreatedAt = bo.CreatedAt,
            };
        }

        public static IList<Mfrs17ContractCodeDetailBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.Mfrs17ContractCodeDetails.OrderBy(q => q.ContractCode).ToList());
            }
        }

        public static IList<Mfrs17ContractCodeDetailBo> GetByMfrs17ContractCode(int mfrs17ContractCodeId)
        {
            return FormBos(Mfrs17ContractCodeDetail.GetByMfrs17ContractCodeId(mfrs17ContractCodeId));
        }

        public static IEnumerable<string> GetContractCode()
        {
            using (var db = new AppDbContext())
            {
                return db.Mfrs17ContractCodeDetails.Select(q => q.ContractCode).ToList();
            }
        }

        public static bool IsExists(int id)
        {
            return Mfrs17ContractCodeDetail.IsExists(id);
        }

        public static Mfrs17ContractCodeDetailBo Find(int? id)
        {
            return FormBo(Mfrs17ContractCodeDetail.Find(id));
        }

        public static Mfrs17ContractCodeDetailBo FindByContractCode(string contractCode)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.Mfrs17ContractCodeDetails.Where(q => q.ContractCode == contractCode).FirstOrDefault());
            }
        }

        public static Mfrs17ContractCodeDetailBo FindByMfsr17ContractCodeIdContractCodeId(int mfrs17ContractCodeId, string contractCode, int? id = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Mfrs17ContractCodeDetails
                    .Where(q => q.Mfrs17ContractCodeId == mfrs17ContractCodeId)
                    .Where(q => q.ContractCode == contractCode);

                if (id.HasValue)
                {
                    query = query.Where(q => q.Id != id);
                }

                return FormBo(query.FirstOrDefault());
            }
        }

        public static Result Save(Mfrs17ContractCodeDetailBo bo)
        {
            if (!Mfrs17ContractCodeDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(Mfrs17ContractCodeDetailBo bo, ref TrailObject trail)
        {
            if (!Mfrs17ContractCodeDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref Mfrs17ContractCodeDetailBo bo)
        {
            Mfrs17ContractCodeDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref Mfrs17ContractCodeDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref Mfrs17ContractCodeDetailBo bo)
        {
            Result result = Result();

            Mfrs17ContractCodeDetail entity = Mfrs17ContractCodeDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Mfrs17ContractCodeId = bo.Mfrs17ContractCodeId;
                entity.ContractCode = bo.ContractCode;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref Mfrs17ContractCodeDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(Mfrs17ContractCodeDetailBo bo)
        {
            Mfrs17ContractCodeDetail.Delete(bo.Id);
        }

        public static Result Delete(Mfrs17ContractCodeDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = Mfrs17ContractCodeDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteByMfrs17ContractCodeId(int mfrs17ContractCodeId)
        {
            return Mfrs17ContractCodeDetail.DeleteByMfrs17ContractCodeId(mfrs17ContractCodeId);
        }

        public static void DeleteByMfrs17ContractCodeId(int mfrs17ContractCodeId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByMfrs17ContractCodeId(mfrs17ContractCodeId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(Mfrs17ContractCodeDetail)));
                }
            }
        }

        public static Result DeleteByMfrs17ContractCodeDetailIdExcept(int mfrs17ContractCodeId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<Mfrs17ContractCodeDetail> mfrs17ContractCodeDetails = Mfrs17ContractCodeDetail.GetByMfrs17ContractCodeIdExcept(mfrs17ContractCodeId, saveIds);
            foreach (Mfrs17ContractCodeDetail mfrs17ContractCodeDetail in mfrs17ContractCodeDetails)
            {
                DataTrail dataTrail = Mfrs17ContractCodeDetail.Delete(mfrs17ContractCodeDetail.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }
    }
}
