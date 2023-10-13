using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TreatyOldCodeService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyOldCode)),
            };
        }

        public static TreatyOldCodeBo FormBo(TreatyOldCode entity = null)
        {
            if (entity == null)
                return null;

            TreatyCodeBo treatyCodeBo = TreatyCodeService.Find(entity.TreatyCodeId);
            TreatyCodeBo treatyOldCodeBo = TreatyCodeService.Find(entity.OldTreatyCodeId);
            return new TreatyOldCodeBo
            {
                TreatyCodeId = entity.TreatyCodeId,
                OldTreatyCodeId = entity.OldTreatyCodeId,
                TreatyCodeBo = treatyCodeBo,
                OldTreatyCodeBo = treatyOldCodeBo,
            };
        }

        public static IList<TreatyOldCodeBo> FormBos(IList<TreatyOldCode> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyOldCodeBo> bos = new List<TreatyOldCodeBo>() { };
            foreach (TreatyOldCode entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyOldCode FormEntity(TreatyOldCodeBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyOldCode
            {
                TreatyCodeId = bo.TreatyCodeId,
                OldTreatyCodeId = bo.OldTreatyCodeId,
            };
        }

        public static bool IsDuplicateCode(TreatyCodeBo treatyCode)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(treatyCode.Code))
                {
                    var query = db.TreatyCodes.Where(q => q.Code == treatyCode.Code);
                    if (treatyCode.Id != 0)
                    {
                        query = query.Where(q => q.Id != treatyCode.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static TreatyOldCodeBo Find(int id, int oldId)
        {
            return FormBo(TreatyOldCode.Find(id, oldId));
        }

        public static TreatyOldCodeBo FindByTreatyCodeId(int id)
        {
            return FormBo(TreatyOldCode.FindByTreatyCodeId(id));
        }

        public static int Count(int treatyCodeId, int oldTreatyCodeId)
        {
            return TreatyOldCode.Count(treatyCodeId, oldTreatyCodeId);
        }

        public static int CountByTreatyCodeId(int treatyCodeId)
        {
            return TreatyOldCode.CountByTreatyCodeId(treatyCodeId);
        }

        public static IList<TreatyOldCodeBo> Get(int treatyCodeId, int oldTreatyCodeId)
        {
            return FormBos(TreatyOldCode.Get(treatyCodeId, oldTreatyCodeId));
        }

        public static IList<TreatyOldCodeBo> GetByTreatyCodeId(int treatyCodeId)
        {
            return FormBos(TreatyOldCode.GetByTreatyCodeId(treatyCodeId));
        }

        public static string GetByTreatyCode(string code)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyOldCodes.Where(q => q.TreatyCode.Code.Trim() == code.Trim()).FirstOrDefault();

                if (query == null)
                    return "";

                return query.OldTreatyCode?.Code;
            }
        }

        public static Result Save(TreatyOldCodeBo bo)
        {
            if (!TreatyOldCode.IsExists(bo.TreatyCodeId, bo.OldTreatyCodeId))
            {
                return Create(bo);
            }
            return Update(bo);
        }

        public static Result Save(TreatyOldCodeBo bo, ref TrailObject trail)
        {
            if (!TreatyOldCode.IsExists(bo.TreatyCodeId, bo.OldTreatyCodeId))
            {
                return Create(bo, ref trail);
            }
            return Update(bo, ref trail);
        }

        public static Result Create(TreatyOldCodeBo bo)
        {
            TreatyOldCode entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
            }

            return result;
        }

        public static Result Create(TreatyOldCodeBo bo, ref TrailObject trail)
        {
            Result result = Create(bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table, bo.PrimaryKey());
            }
            return result;
        }

        public static Result Update(TreatyOldCodeBo bo)
        {
            Result result = Result();

            TreatyOldCode entity = TreatyOldCode.Find(bo.TreatyCodeId, bo.OldTreatyCodeId);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            //if (IsDuplicateCode(FormEntity(bo)))
            //{
            //    result.AddTakenError("Code", bo.Code);
            //}

            if (result.Valid)
            {
                entity.TreatyCodeId = bo.TreatyCodeId;
                entity.OldTreatyCodeId = bo.OldTreatyCodeId;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(TreatyOldCodeBo bo, ref TrailObject trail)
        {
            Result result = Update(bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table, bo.PrimaryKey());
            }
            return result;
        }

        public static void Delete(TreatyOldCodeBo bo)
        {
            TreatyOldCode.Delete(bo.TreatyCodeId, bo.OldTreatyCodeId);
        }

        public static Result Delete(TreatyOldCodeBo bo, ref TrailObject trail)
        {
            Result result = Result();
            if (result.Valid)
            {
                DataTrail dataTrail = TreatyOldCode.Delete(bo.TreatyCodeId, bo.OldTreatyCodeId);
                dataTrail.Merge(ref trail, result.Table, bo.PrimaryKey());
            }
            return result;
        }

        public static Result DeleteAllByTreatyCodeIdExcept(List<int> mappingTreatyCodeIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<TreatyOldCode> treatyOldCodes = TreatyOldCode.GetAllByTreatyCodeIdExcept(mappingTreatyCodeIds);
            foreach (TreatyOldCode treatyOldCode in treatyOldCodes)
            {
                DataTrail dataTrail = TreatyOldCode.Delete(treatyOldCode.TreatyCodeId, treatyOldCode.OldTreatyCodeId);
                dataTrail.Merge(ref trail, result.Table, treatyOldCode.PrimaryKey());
            }
            return result;
        }

        public static Result DeleteByTreatyCodeIdExcept(int treatyCodeId, List<int> mappingIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<TreatyOldCode> treatyOldCodes = TreatyOldCode.GetByTreatyCodeIdExcept(treatyCodeId, mappingIds);
            foreach (TreatyOldCode treatyOldCode in treatyOldCodes)
            {
                DataTrail dataTrail = TreatyOldCode.Delete(treatyOldCode.TreatyCodeId, treatyOldCode.OldTreatyCodeId);
                dataTrail.Merge(ref trail, result.Table, treatyOldCode.PrimaryKey());
            }
            return result;
        }
    }
}
