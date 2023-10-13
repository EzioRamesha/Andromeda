using BusinessObject;
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
    public class StandardOutputService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(StandardOutput)),
                Controller = ModuleBo.ModuleController.StandardOutput.ToString()
            };
        }

        public static StandardOutputBo FormBo(StandardOutput entity = null)
        {
            if (entity == null)
                return null;
            return new StandardOutputBo
            {
                Id = entity.Id,
                Type = entity.Type,
                TypeName = StandardOutputBo.GetTypeName(entity.Type),
                DataType = entity.DataType,
                DataTypeName = StandardOutputBo.GetDataTypeName(entity.DataType),
                Code = entity.Code,
                Name = entity.Name,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<StandardOutputBo> FormBos(IList<StandardOutput> entities = null)
        {
            if (entities == null)
                return null;
            IList<StandardOutputBo> bos = new List<StandardOutputBo>() { };
            foreach (StandardOutput entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static StandardOutput FormEntity(StandardOutputBo bo = null)
        {
            if (bo == null)
                return null;
            return new StandardOutput
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

        public static bool IsDuplicateCode(StandardOutput standardOutput)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(standardOutput.Type.ToString()) && standardOutput.Type != StandardOutputBo.TypeCustomField)
                {
                    var query = db.StandardOutputs.Where(q => q.Type == standardOutput.Type);
                    if (standardOutput.Id != 0)
                    {
                        query = query.Where(q => q.Id != standardOutput.Id);
                    }
                    return query.Count() > 0;
                } 
                else if (!string.IsNullOrEmpty(standardOutput.Type.ToString()) && standardOutput.Type == StandardOutputBo.TypeCustomField)
                {
                    var query = db.StandardOutputs.Where(q => q.Code == standardOutput.Code);
                    if (standardOutput.Id != 0)
                    {
                        query = query.Where(q => q.Id != standardOutput.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static StandardOutputBo Find(int? id)
        {
            return FormBo(StandardOutput.Find(id));
        }

        public static IList<StandardOutputBo> Get()
        {
            List<StandardOutput> entities = new List<StandardOutput>() { };
            StandardOutput customField = StandardOutput.FindByType(StandardOutputBo.TypeCustomField);
            if (customField != null)
            {
                entities.Add(customField);
            }

            var all = StandardOutput.Get();
            if (all != null)
            {
                entities.AddRange(all);
            }
            return FormBos(entities);
        }
        
        public static IList<StandardOutputBo> GetWarehouseOnly()
        {
            using (var db = new AppDbContext())
            {
                List<int> excludedTypes = StandardOutputBo.GetWarehouseExcludedTypes();
                return FormBos(db.StandardOutputs.Where(q => !excludedTypes.Contains(q.Type)).ToList());
            }
        }

        public static Result Save(ref StandardOutputBo bo)
        {
            if (!StandardOutput.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref StandardOutputBo bo, ref TrailObject trail)
        {
            if (!StandardOutput.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref StandardOutputBo bo)
        {
            StandardOutput entity = FormEntity(bo);

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

        public static Result Create(ref StandardOutputBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref StandardOutputBo bo)
        {
            Result result = Result();

            StandardOutput entity = StandardOutput.Find(bo.Id);
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

        public static Result Update(ref StandardOutputBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(StandardOutputBo bo)
        {
            StandardOutput.Delete(bo.Id);
        }

        public static Result Delete(StandardOutputBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = StandardOutput.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }
    }
}
