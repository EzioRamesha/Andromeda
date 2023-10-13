using BusinessObject;
using DataAccess.Entities;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DiscountTableService
    {
        public static Result Result()
        {
            return new Result 
            {
                Table = UtilAttribute.GetTableName(typeof(DiscountTable)),
                Controller = ModuleBo.ModuleController.DiscountTable.ToString()
            };
        }

        public static Expression<Func<DiscountTable, DiscountTableBo>> Expression()
        {
            return entity => new DiscountTableBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static DiscountTableBo FormBo(DiscountTable entity = null)
        {
            if (entity == null)
                return null;
            return new DiscountTableBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                CedantBo = CedantService.Find(entity.CedantId),

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<DiscountTableBo> FormBos(IList<DiscountTable> entities = null)
        {
            if (entities == null)
                return null;
            IList<DiscountTableBo> bos = new List<DiscountTableBo>() { };
            foreach (DiscountTable entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static DiscountTable FormEntity(DiscountTableBo bo = null)
        {
            if (bo == null)
                return null;
            return new DiscountTable
            {
                Id = bo.Id,
                CedantId = bo.CedantId,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return DiscountTable.IsExists(id);
        }

        public static DiscountTableBo Find(int id)
        {
            return FormBo(DiscountTable.Find(id));
        }

        public static DiscountTableBo FindByCedantId(int cedantId)
        {
            return FormBo(DiscountTable.FindByCedantId(cedantId));
        }

        public static int CountByCedantId(int cedantId)
        {
            return DiscountTable.CountByCedantId(cedantId);
        }

        public static IList<DiscountTableBo> Get()
        {
            return FormBos(DiscountTable.Get());
        }

        public static Result Save(ref DiscountTableBo bo)
        {
            if (!DiscountTable.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref DiscountTableBo bo, ref TrailObject trail)
        {
            if (!DiscountTable.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicateCedant(DiscountTable discountTable)
        {
            return discountTable.IsDuplicateCedant();
        }

        public static Result Create(ref DiscountTableBo bo)
        {
            DiscountTable entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCedant(entity))
            {
                CedantBo cedantBo = CedantService.Find(bo.CedantId);
                result.AddTakenError("Ceding Company", cedantBo?.Code);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref DiscountTableBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref DiscountTableBo bo)
        {
            Result result = Result();

            DiscountTable entity = DiscountTable.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (IsDuplicateCedant(FormEntity(bo)))
            {
                CedantBo cedantBo = CedantService.Find(bo.CedantId);
                result.AddTakenError("Ceding Comapny", cedantBo.Code);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.CedantId = bo.CedantId;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref DiscountTableBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(DiscountTableBo bo)
        {
            RiDiscountService.DeleteAllByDiscountTableId(bo.Id);
            LargeDiscountService.DeleteAllByDiscountTableId(bo.Id);
            GroupDiscountService.DeleteAllByDiscountTableId(bo.Id);
            DiscountTable.Delete(bo.Id);
        }

        public static Result Delete(DiscountTableBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (RateTableService.CountByCedantId(bo.CedantId) > 0)
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                RiDiscountService.DeleteAllByDiscountTableId(bo.Id, ref trail);
                LargeDiscountService.DeleteAllByDiscountTableId(bo.Id, ref trail);
                GroupDiscountService.DeleteAllByDiscountTableId(bo.Id, ref trail);
                DataTrail dataTrail = DiscountTable.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
