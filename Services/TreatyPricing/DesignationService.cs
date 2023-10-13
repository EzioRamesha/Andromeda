using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.TreatyPricing
{
    public class DesignationService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Designation)),
                Controller = ModuleBo.ModuleController.Designation.ToString()
            };
        }

        public static DesignationBo FormBo(Designation entity = null)
        {
            if (entity == null)
                return null;
            return new DesignationBo
            {
                Id = entity.Id,
                Code = entity.Code,
                Description = entity.Description,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<DesignationBo> FormBos(IList<Designation> entities = null)
        {
            if (entities == null)
                return null;
            IList<DesignationBo> bos = new List<DesignationBo>() { };
            foreach (Designation entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static Designation FormEntity(DesignationBo bo = null)
        {
            if (bo == null)
                return null;
            return new Designation
            {
                Id = bo.Id,
                Code = bo.Code,
                Description = bo.Description,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateCode(Designation Designation)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(Designation.Code))
                {
                    var query = db.Designations.Where(q => q.Code == Designation.Code);
                    if (Designation.Id != 0)
                    {
                        query = query.Where(q => q.Id != Designation.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static bool IsExists(int id)
        {
            return Designation.IsExists(id);
        }

        public static DesignationBo Find(int? id)
        {
            return FormBo(Designation.Find(id));
        }

        public static DesignationBo FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.Designations.Where(q => q.Code == code).FirstOrDefault());
            }
        }

        public static IList<DesignationBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.Designations.ToList());
            }
        }

        public static Result Save(ref DesignationBo bo)
        {
            if (!Designation.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref DesignationBo bo, ref TrailObject trail)
        {
            if (!Designation.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref DesignationBo bo)
        {
            Designation entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Code", bo.Code.ToString());
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref DesignationBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref DesignationBo bo)
        {
            Result result = Result();

            Designation entity = Designation.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Code", bo.Code.ToString());
            }

            if (result.Valid)
            {
                entity.Code = bo.Code;
                entity.Description = bo.Description;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref DesignationBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(DesignationBo bo)
        {
            Designation.Delete(bo.Id);
        }

        public static Result Delete(DesignationBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                DataTrail dataTrail = Designation.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
