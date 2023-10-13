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
    public class StandardSoaDataOutputService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(StandardSoaDataOutput)),
                Controller = ModuleBo.ModuleController.StandardSoaDataOutput.ToString()
            };
        }

        public static StandardSoaDataOutputBo FormBo(StandardSoaDataOutput entity = null)
        {
            if (entity == null)
                return null;
            return new StandardSoaDataOutputBo
            {
                Id = entity.Id,
                Type = entity.Type,
                TypeName = StandardSoaDataOutputBo.GetTypeByName(entity.Type),
                DataType = entity.DataType,
                DataTypeName = StandardSoaDataOutputBo.GetDataTypeName(entity.DataType),
                Code = entity.Code,
                Name = entity.Name,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<StandardSoaDataOutputBo> FormBos(IList<StandardSoaDataOutput> entities = null)
        {
            if (entities == null)
                return null;
            IList<StandardSoaDataOutputBo> bos = new List<StandardSoaDataOutputBo>() { };
            foreach (StandardSoaDataOutput entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static StandardSoaDataOutput FormEntity(StandardSoaDataOutputBo bo = null)
        {
            if (bo == null)
                return null;
            return new StandardSoaDataOutput
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

        public static bool IsDuplicateCode(StandardSoaDataOutput standardSoaDataOutput)
        {
            return standardSoaDataOutput.IsDuplicateCode();
        }

        public static StandardSoaDataOutputBo Find(int? id)
        {
            return FormBo(StandardSoaDataOutput.Find(id));
        }

        public static StandardSoaDataOutputBo FindByCode(string code)
        {
            return FormBo(StandardSoaDataOutput.FindByCode(code));
        }

        public static IList<StandardSoaDataOutputBo> Get()
        {
            List<StandardSoaDataOutput> entities = new List<StandardSoaDataOutput>() { };
            var all = StandardSoaDataOutput.Get();
            if (all != null)
            {
                entities.AddRange(all);
            }
            return FormBos(entities);
        }

        public static Result Save(ref StandardSoaDataOutputBo bo)
        {
            if (!StandardSoaDataOutput.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref StandardSoaDataOutputBo bo, ref TrailObject trail)
        {
            if (!StandardSoaDataOutput.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref StandardSoaDataOutputBo bo)
        {
            StandardSoaDataOutput entity = FormEntity(bo);

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

        public static Result Create(ref StandardSoaDataOutputBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref StandardSoaDataOutputBo bo)
        {
            Result result = Result();

            StandardSoaDataOutput entity = StandardSoaDataOutput.Find(bo.Id);
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

        public static Result Update(ref StandardSoaDataOutputBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(StandardSoaDataOutputBo bo)
        {
            StandardSoaDataOutput.Delete(bo.Id);
        }

        public static Result Delete(StandardSoaDataOutputBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = StandardSoaDataOutput.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }
    }
}

