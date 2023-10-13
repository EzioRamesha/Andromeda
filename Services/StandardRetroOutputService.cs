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
    public class StandardRetroOutputService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(StandardRetroOutput)),
                Controller = ModuleBo.ModuleController.StandardRetroOutput.ToString()
            };
        }

        public static StandardRetroOutputBo FormBo(StandardRetroOutput entity = null)
        {
            if (entity == null)
                return null;
            return new StandardRetroOutputBo
            {
                Id = entity.Id,
                Type = entity.Type,
                TypeName = StandardRetroOutputBo.GetTypeName(entity.Type),
                DataType = entity.DataType,
                DataTypeName = StandardRetroOutputBo.GetDataTypeName(entity.DataType),
                Code = entity.Code,
                Name = entity.Name,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<StandardRetroOutputBo> FormBos(IList<StandardRetroOutput> entities = null)
        {
            if (entities == null)
                return null;
            IList<StandardRetroOutputBo> bos = new List<StandardRetroOutputBo>() { };
            foreach (StandardRetroOutput entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static StandardRetroOutput FormEntity(StandardRetroOutputBo bo = null)
        {
            if (bo == null)
                return null;
            return new StandardRetroOutput
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

        public static bool IsDuplicateCode(StandardRetroOutput entity)
        {
            return entity.IsDuplicateCode();
        }

        public static StandardRetroOutputBo Find(int? id)
        {
            return FormBo(StandardRetroOutput.Find(id));
        }

        public static IList<StandardRetroOutputBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.StandardRetroOutputs.OrderBy(q => q.Code).ToList());
            }
        }

        public static Result Save(ref StandardRetroOutputBo bo)
        {
            if (!StandardRetroOutput.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref StandardRetroOutputBo bo, ref TrailObject trail)
        {
            if (!StandardRetroOutput.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref StandardRetroOutputBo bo)
        {
            StandardRetroOutput entity = FormEntity(bo);

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

        public static Result Create(ref StandardRetroOutputBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref StandardRetroOutputBo bo)
        {
            Result result = Result();

            StandardRetroOutput entity = StandardRetroOutput.Find(bo.Id);
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

        public static Result Update(ref StandardRetroOutputBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(StandardRetroOutputBo bo)
        {
            StandardRetroOutput.Delete(bo.Id);
        }

        public static Result Delete(StandardRetroOutputBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = StandardRetroOutput.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }
    }
}
