using BusinessObject;
using DataAccess.Entities;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Services
{
    public class PickListService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PickList)),
                Controller = ModuleBo.ModuleController.PickList.ToString()
            };
        }

        public static Expression<Func<PickList, PickListBo>> Expression()
        {
            return entity => new PickListBo
            {
                Id = entity.Id,
                DepartmentId = entity.DepartmentId,
                StandardOutputId = entity.StandardOutputId,
                FieldName = entity.FieldName,
                IsEditable = entity.IsEditable,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static PickListBo FormBo(PickList entity = null)
        {
            if (entity == null)
                return null;
            return new PickListBo
            {
                Id = entity.Id,
                DepartmentId = entity.DepartmentId,
                DepartmentBo = DepartmentService.Find(entity.DepartmentId),
                StandardOutputId = entity.StandardOutputId,
                StandardOutputBo = StandardOutputService.Find(entity.StandardOutputId),
                FieldName = entity.FieldName?.Trim(),
                IsEditable = entity.IsEditable,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<PickListBo> FormBos(IList<PickList> entities = null)
        {
            if (entities == null)
                return null;
            IList<PickListBo> bos = new List<PickListBo>() { };
            foreach (PickList entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PickList FormEntity(PickListBo bo = null)
        {
            if (bo == null)
                return null;
            return new PickList
            {
                Id = bo.Id,
                DepartmentId = bo.DepartmentId,
                StandardOutputId = bo.StandardOutputId,
                FieldName = bo.FieldName,
                IsEditable = bo.IsEditable,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PickList.IsExists(id);
        }

        public static PickListBo Find(int id)
        {
            return FormBo(PickList.Find(id));
        }

        public static Result Save(ref PickListBo bo)
        {
            if (!PickList.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PickListBo bo, ref TrailObject trail)
        {
            if (!PickList.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref PickListBo bo)
        {
            PickList entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PickListBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PickListBo bo)
        {
            Result result = Result();

            PickList entity = PickList.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.DepartmentId = bo.DepartmentId;
                entity.StandardOutputId = bo.StandardOutputId;
                entity.FieldName = bo.FieldName;
                entity.IsEditable = bo.IsEditable;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PickListBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PickListBo bo)
        {
            PickListDetailService.DeleteAllByPickListId(bo.Id);
            PickList.Delete(bo.Id);
        }

        public static Result Delete(PickListBo bo, ref TrailObject trail)
        {
            Result result = Result();

            PickListDetailService.DeleteAllByPickListId(bo.Id, ref trail);
            DataTrail dataTrail = PickList.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }
    }
}
