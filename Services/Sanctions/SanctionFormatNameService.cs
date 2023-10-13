using BusinessObject;
using BusinessObject.Sanctions;
using DataAccess.Entities.Sanctions;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Sanctions
{
    public class SanctionFormatNameService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SanctionFormatName)),
                Controller = ModuleBo.ModuleController.SanctionFormatName.ToString()
            };
        }

        public static SanctionFormatNameBo FormBo(SanctionFormatName entity = null)
        {
            if (entity == null)
                return null;
            return new SanctionFormatNameBo
            {
                Id = entity.Id,
                SanctionId = entity.SanctionId,
                SanctionNameId = entity.SanctionNameId,
                Type = entity.Type,
                TypeIndex = entity.TypeIndex,
                Name = entity.Name,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
            };
        }

        public static IList<SanctionFormatNameBo> FormBos(IList<SanctionFormatName> entities = null)
        {
            if (entities == null)
                return null;
            IList<SanctionFormatNameBo> bos = new List<SanctionFormatNameBo>() { };
            foreach (SanctionFormatName entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static SanctionFormatName FormEntity(SanctionFormatNameBo bo = null)
        {
            if (bo == null)
                return null;
            return new SanctionFormatName
            {
                Id = bo.Id,
                SanctionId = bo.SanctionId,
                SanctionNameId = bo.SanctionNameId,
                Type = bo.Type,
                TypeIndex = bo.TypeIndex,
                Name = bo.Name,
                CreatedById = bo.CreatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return SanctionFormatName.IsExists(id);
        }

        public static SanctionFormatNameBo Find(int id)
        {
            return FormBo(SanctionFormatName.Find(id));
        }

        public static IList<SanctionFormatNameBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionFormatNames.ToList());
            }
        }

        public static IList<SanctionFormatNameBo> GetBySanctionId(int sanctionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionFormatNames.Where(q => q.SanctionId == sanctionId).ToList());
            }
        }

        public static IList<int> GetByNameType(string name, int type, IList<int> sanctionIds)
        {
            using (var db = new AppDbContext())
            {
                var query = db.SanctionFormatNames.Where(q => q.Name == name)
                    .Where(q => q.Type == type);

                if (sanctionIds.Count() != 0)
                {
                    query = query.Where(q => sanctionIds.Contains(q.SanctionId));
                }

                return query.Select(q => q.SanctionId).ToList();
            }
        }

        public static IList<int> GetByGroupNameType(List<string> name, IList<int> sanctionIds)
        {
            using (var db = new AppDbContext())
            {
                var query = db.SanctionFormatNames.Where(q => name.Contains(q.Name))
                    .Where(q => q.Type == SanctionFormatNameBo.TypeGroupKeyword);

                if (sanctionIds.Count() != 0)
                {
                    query = query.Where(q => sanctionIds.Contains(q.SanctionId));
                }

                return query.GroupBy(q => q.SanctionNameId).Where(g => g.Count() >= 3).Select(q => q.FirstOrDefault().SanctionId).ToList();
            }
        }

        public static Result Create(ref SanctionFormatNameBo bo)
        {
            SanctionFormatName entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref SanctionFormatNameBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SanctionFormatNameBo bo)
        {
            SanctionFormatName.Delete(bo.Id);
        }

        public static Result Delete(SanctionFormatNameBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                DataTrail dataTrail = SanctionFormatName.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteBySanctionId(int sanctionId)
        {
            return SanctionFormatName.DeleteBySanctionId(sanctionId);
        }

        public static void DeleteByIds(List<int> ids)
        {
            using (var db = new AppDbContext())
            {
                var query = db.SanctionFormatNames.Where(q => ids.Contains(q.Id));

                foreach (SanctionFormatName entity in query.ToList())
                {
                    db.Entry(entity).State = EntityState.Deleted;
                    db.SanctionFormatNames.Remove(entity);
                }

                db.SaveChanges();
            }
        }

        public static void DeleteBySanctionIdName(int sanctionNameId, int type, int? typeIndex, string name)
        {
            using (var db = new AppDbContext())
            {
                name = name.ToUpper();
                SanctionFormatName entity = db.SanctionFormatNames.Where(q => q.SanctionNameId == sanctionNameId && q.Type == type && q.TypeIndex == typeIndex && q.Name.ToUpper() == name).FirstOrDefault();
                if (entity == null)
                    return;

                db.Entry(entity).State = EntityState.Deleted;
                db.SanctionFormatNames.Remove(entity);
                db.SaveChanges();
            }
        }

        public static void DeleteBySanctionId(int sanctionId, ref TrailObject trail)
        {
            Result result = Result();
            IList<DataTrail> dataTrails = DeleteBySanctionId(sanctionId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, result.Table);
                }
            }
        }

        public static IList<DataTrail> DeleteBySanctionBatchId(int sanctionBatchId)
        {
            return SanctionFormatName.DeleteBySanctionBatchId(sanctionBatchId);
        }

        public static void DeleteBySanctionBatchId(int sanctionBatchId, ref TrailObject trail)
        {
            Result result = Result();
            IList<DataTrail> dataTrails = DeleteBySanctionBatchId(sanctionBatchId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, result.Table);
                }
            }
        }
    }
}
