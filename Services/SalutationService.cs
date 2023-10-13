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
    public class SalutationService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Salutation)),
                Controller = ModuleBo.ModuleController.Salutation.ToString()
            };
        }

        public static SalutationBo FormBo(Salutation entity = null)
        {
            if (entity == null)
                return null;
            return new SalutationBo
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<SalutationBo> FormBos(IList<Salutation> entities = null)
        {
            if (entities == null)
                return null;
            IList<SalutationBo> bos = new List<SalutationBo>() { };
            foreach (Salutation entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static Salutation FormEntity(SalutationBo bo = null)
        {
            if (bo == null)
                return null;
            return new Salutation
            {
                Id = bo.Id,
                Name = bo.Name?.Trim(),
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateCode(Salutation salutation)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(salutation.Name?.Trim()))
                {
                    var query = db.Salutations.Where(q => q.Name.Trim().Equals(salutation.Name.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (salutation.Id != 0)
                    {
                        query = query.Where(q => q.Id != salutation.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static bool IsExists(int id)
        {
            return Salutation.IsExists(id);
        }

        public static SalutationBo Find(int id)
        {
            return FormBo(Salutation.Find(id));
        }

        public static SalutationBo FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.Salutations.Where(q => q.Name.Trim() == code.Trim()).FirstOrDefault());
            }
        }

        public static IList<SalutationBo> Get()
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;
                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("SalutationService");
                return connectionStrategy.Execute(() => FormBos(db.Salutations.OrderBy(q => q.Name).ToList()));
            }
        }

        public static Result Save(ref SalutationBo bo)
        {
            if (!Salutation.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref SalutationBo bo, ref TrailObject trail)
        {
            if (!Salutation.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SalutationBo bo)
        {
            Salutation entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Salutation", bo.Name);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref SalutationBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SalutationBo bo)
        {
            Result result = Result();

            Salutation entity = Salutation.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Salutation", bo.Name);
            }

            if (result.Valid)
            {
                entity.Name = bo.Name;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SalutationBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SalutationBo bo)
        {
            Salutation.Delete(bo.Id);
        }

        public static Result Delete(SalutationBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                DataTrail dataTrail = Salutation.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
