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
    public class StandardClaimDataOutputService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(StandardClaimDataOutput)),
                Controller = ModuleBo.ModuleController.StandardClaimDataOutput.ToString()
            };
        }

        public static StandardClaimDataOutputBo FormBo(StandardClaimDataOutput entity = null)
        {
            if (entity == null)
                return null;
            return new StandardClaimDataOutputBo
            {
                Id = entity.Id,
                Type = entity.Type,
                TypeName = StandardClaimDataOutputBo.GetTypeName(entity.Type),
                DataType = entity.DataType,
                DataTypeName = StandardClaimDataOutputBo.GetDataTypeName(entity.DataType),
                Code = entity.Code,
                Name = entity.Name,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<StandardClaimDataOutputBo> FormBos(IList<StandardClaimDataOutput> entities = null)
        {
            if (entities == null)
                return null;
            IList<StandardClaimDataOutputBo> bos = new List<StandardClaimDataOutputBo>() { };
            foreach (StandardClaimDataOutput entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static StandardClaimDataOutput FormEntity(StandardClaimDataOutputBo bo = null)
        {
            if (bo == null)
                return null;
            return new StandardClaimDataOutput
            {
                Id = bo.Id,
                Type = bo.Type,
                DataType = bo.DataType,
                Code = bo.Code,
                Name = bo.Name,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateCode(StandardClaimDataOutput claimOutput)
        {
            using (var db = new AppDbContext())
            {
                var query = db.StandardClaimDataOutputs.Where(q => q.Type == claimOutput.Type);
                if (claimOutput.Id != 0)
                {
                    query = query.Where(q => q.Id != claimOutput.Id);
                }
                return query.Count() > 0;
            }
        }

        public static StandardClaimDataOutputBo Find(int? id)
        {
            return FormBo(StandardClaimDataOutput.Find(id));
        }

        public static IList<StandardClaimDataOutputBo> Get()
        {
            List<StandardClaimDataOutput> entities = new List<StandardClaimDataOutput>() { };
            var all = StandardClaimDataOutput.Get();
            if (all != null)
            {
                entities.AddRange(all);
            }
            return FormBos(entities);
        }

        public static Result Save(ref StandardClaimDataOutputBo bo)
        {
            if (!StandardClaimDataOutput.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref StandardClaimDataOutputBo bo, ref TrailObject trail)
        {
            if (!StandardClaimDataOutput.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref StandardClaimDataOutputBo bo)
        {
            StandardClaimDataOutput entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Code", bo.Type.ToString());
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref StandardClaimDataOutputBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref StandardClaimDataOutputBo bo)
        {
            Result result = Result();

            StandardClaimDataOutput entity = StandardClaimDataOutput.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Code", bo.Type.ToString());
            }

            if (result.Valid)
            {
                entity.Type = bo.Type;
                entity.DataType = bo.DataType;
                entity.Code = bo.Code;
                entity.Name = bo.Name;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref StandardClaimDataOutputBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(StandardClaimDataOutputBo bo)
        {
            StandardClaimDataOutput.Delete(bo.Id);
        }

        public static Result Delete(StandardClaimDataOutputBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = StandardClaimDataOutput.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }
    }
}
