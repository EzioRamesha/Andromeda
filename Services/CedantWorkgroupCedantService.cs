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
    public class CedantWorkgroupCedantService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(CedantWorkgroupCedant)),
            };
        }

        public static CedantWorkgroupCedantBo FormBo(CedantWorkgroupCedant entity = null)
        {
            if (entity == null)
                return null;

            CedantBo cedantBo = CedantService.Find(entity.CedantId);
            return new CedantWorkgroupCedantBo
            {
                CedantWorkgroupId = entity.CedantWorkgroupId,
                CedantId = entity.CedantId,
                CedantName = cedantBo.ToString()
            };
        }

        public static IList<CedantWorkgroupCedantBo> FormBos(IList<CedantWorkgroupCedant> entities = null)
        {
            if (entities == null)
                return null;
            IList<CedantWorkgroupCedantBo> bos = new List<CedantWorkgroupCedantBo>() { };
            foreach (CedantWorkgroupCedant entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static CedantWorkgroupCedant FormEntity(CedantWorkgroupCedantBo entity = null)
        {
            if (entity == null)
                return null;
            return new CedantWorkgroupCedant
            {
                CedantWorkgroupId = entity.CedantWorkgroupId,
                CedantId = entity.CedantId,
            };
        }

        public static bool IsExists(int cedantWorkgroupId, int cedantId)
        {
            return CedantWorkgroupCedant.IsExists(cedantWorkgroupId, cedantId);
        }
        
        public static bool IsDuplicate(int cedantWorkgroupId, int cedantId)
        {
            return CedantWorkgroupCedant.IsDuplicate(cedantWorkgroupId, cedantId);
        }
        
        public static bool CheckWorkgroupPower(int userId, int cedantId)
        {
            using (var db = new AppDbContext())
            {
                var subQuery = db.CedantWorkgroupUsers.Where(u => u.UserId == userId).Select(u => u.CedantWorkgroupId);
                return db.CedantWorkgroupCedants.Any(q => q.CedantId == cedantId && subQuery.Contains(q.CedantWorkgroupId));
            }
        }

        public static CedantWorkgroupCedantBo Find(int cedantWorkgroupId, int cedantId)
        {
            return FormBo(CedantWorkgroupCedant.Find(cedantWorkgroupId, cedantId));
        }

        public static IList<CedantWorkgroupCedantBo> GetByCedantWorkgroupId(int cedantWorkgroupId)
        {
            return FormBos(CedantWorkgroupCedant.GetByCedantWorkgroupId(cedantWorkgroupId));
        }
        
        public static IList<CedantWorkgroupCedantBo> GetByCedantWorkgroupIdExcept(int cedantWorkgroupId, List<int> cedantIds)
        {
            return FormBos(CedantWorkgroupCedant.GetByCedantWorkgroupIdExcept(cedantWorkgroupId, cedantIds));
        }

        public static Result Create(CedantWorkgroupCedantBo bo)
        {
            CedantWorkgroupCedant entity = FormEntity(bo);
            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
            }
            return result;
        }

        public static Result Create(CedantWorkgroupCedantBo bo, ref TrailObject trail)
        {
            Result result = Create(bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table, bo.PrimaryKey());
            }
            return result;
        }

        public static DataTrail Delete(CedantWorkgroupCedantBo bo)
        {
            CedantWorkgroupCedant entity = FormEntity(bo);
            return entity.Delete();
        }

        public static void Delete(CedantWorkgroupCedantBo bo, ref TrailObject trail)
        {
            DataTrail dataTrail = Delete(bo);
            dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(CedantWorkgroupCedant)), bo.PrimaryKey());
        }

        public static void DeleteAllByCedantWorkgroupId(int cedantWorkgroupId, ref TrailObject trail)
        {
            foreach (CedantWorkgroupCedantBo bo in GetByCedantWorkgroupId(cedantWorkgroupId))
            {
                DataTrail dataTrail = Delete(bo);
                dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(CedantWorkgroupCedant)), bo.PrimaryKey());
            }
        }
        
        public static void DeleteByCedantWorkgroupIdExcept(int cedantWorkgroupId, List<int> cedantIds, ref TrailObject trail)
        {
            foreach (CedantWorkgroupCedantBo bo in GetByCedantWorkgroupIdExcept(cedantWorkgroupId, cedantIds))
            {
                DataTrail dataTrail = Delete(bo);
                dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(CedantWorkgroupCedant)), bo.PrimaryKey());
            }
        }
    }
}
