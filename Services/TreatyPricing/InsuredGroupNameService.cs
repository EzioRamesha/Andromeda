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
    public class InsuredGroupNameService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(InsuredGroupName)),
                Controller = ModuleBo.ModuleController.InsuredGroupName.ToString()
            };
        }

        public static InsuredGroupNameBo FormBo(InsuredGroupName entity = null)
        {
            if (entity == null)
                return null;
            return new InsuredGroupNameBo
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Status = entity.Status,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<InsuredGroupNameBo> FormBos(IList<InsuredGroupName> entities = null)
        {
            if (entities == null)
                return null;
            IList<InsuredGroupNameBo> bos = new List<InsuredGroupNameBo>() { };
            foreach (InsuredGroupName entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static InsuredGroupName FormEntity(InsuredGroupNameBo bo = null)
        {
            if (bo == null)
                return null;
            return new InsuredGroupName
            {
                Id = bo.Id,
                Name = bo.Name,
                Description = bo.Description,
                Status = bo.Status,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateCode(InsuredGroupName InsuredGroupName)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(InsuredGroupName.Name))
                {
                    var query = db.InsuredGroupNames.Where(q => q.Name == InsuredGroupName.Name);
                    if (InsuredGroupName.Id != 0)
                    {
                        query = query.Where(q => q.Id != InsuredGroupName.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static bool IsExists(int id)
        {
            return InsuredGroupName.IsExists(id);
        }

        public static InsuredGroupNameBo Find(int id)
        {
            return FormBo(InsuredGroupName.Find(id));
        }

        public static InsuredGroupNameBo FindByCode(string name)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.InsuredGroupNames.Where(q => q.Name == name).FirstOrDefault());
            }
        }

        public static IList<InsuredGroupNameBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.InsuredGroupNames.ToList());
            }
        }

        public static IList<InsuredGroupNameBo> GetByStatus(int? status = null, int? selectedId = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.InsuredGroupNames.AsQueryable();

                if (status != null)
                {
                    if (selectedId != null)
                        query = query.Where(q => q.Status == status || q.Id == selectedId);
                    else
                        query = query.Where(q => q.Status == status);
                }

                return FormBos(query.OrderBy(q => q.Name).ToList());
            }
        }

        public static Result Save(ref InsuredGroupNameBo bo)
        {
            if (!InsuredGroupName.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref InsuredGroupNameBo bo, ref TrailObject trail)
        {
            if (!InsuredGroupName.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref InsuredGroupNameBo bo)
        {
            InsuredGroupName entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Name", bo.Name.ToString());
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref InsuredGroupNameBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref InsuredGroupNameBo bo)
        {
            Result result = Result();

            InsuredGroupName entity = InsuredGroupName.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Name", bo.Name.ToString());
            }

            if (result.Valid)
            {
                entity.Name = bo.Name;
                entity.Description = bo.Description;
                entity.Status = bo.Status;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref InsuredGroupNameBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(InsuredGroupNameBo bo)
        {
            InsuredGroupName.Delete(bo.Id);
        }

        public static Result Delete(InsuredGroupNameBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (TreatyPricingGroupReferralService.CountByInsuredGroupNameId(bo.Id) > 0)
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                DataTrail dataTrail = InsuredGroupName.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
