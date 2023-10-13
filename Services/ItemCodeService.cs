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
    public class ItemCodeService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ItemCode)),
                Controller = ModuleBo.ModuleController.ItemCode.ToString()
            };
        }

        public static ItemCodeBo FormBo(ItemCode entity = null)
        {
            if (entity == null)
                return null;
            return new ItemCodeBo
            {
                Id = entity.Id,
                Code = entity.Code,
                ReportingType = entity.ReportingType,
                Description = entity.Description,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                BusinessOriginPickListDetailId = entity.BusinessOriginPickListDetailId,
                BusinessOriginPickListDetailBo = PickListDetailService.Find(entity.BusinessOriginPickListDetailId),
            };
        }

        public static IList<ItemCodeBo> FormBos(IList<ItemCode> entities = null)
        {
            if (entities == null)
                return null;
            IList<ItemCodeBo> bos = new List<ItemCodeBo>() { };
            foreach (ItemCode entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ItemCode FormEntity(ItemCodeBo bo = null)
        {
            if (bo == null)
                return null;
            return new ItemCode
            {
                Id = bo.Id,
                Code = bo.Code?.Trim(),
                ReportingType = bo.ReportingType,
                Description = bo.Description?.Trim(),
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
                BusinessOriginPickListDetailId = bo.BusinessOriginPickListDetailId,
            };
        }

        public static bool IsDuplicate(ItemCode itemCode)
        {
            return itemCode.IsDuplicate();
        }

        public static bool IsExists(int id)
        {
            return ItemCode.IsExists(id);
        }

        public static ItemCodeBo Find(int id)
        {
            return FormBo(ItemCode.Find(id));
        }

        public static ItemCodeBo FindByCode(string code)
        {
            return FormBo(ItemCode.FindByCode(code));
        }

        public static ItemCodeBo FindByCodeBusinessOriginCodeReportingType(string code, string businessOriginCode, int reportingType)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.ItemCodes.Where(q => q.Code == code).Where(q => q.BusinessOriginPickListDetail.Code == businessOriginCode).Where(q => q.ReportingType == reportingType).FirstOrDefault());
            }
        }

        public static IList<ItemCodeBo> Get()
        {
            return FormBos(ItemCode.Get());
        }

        public static int CountByBusinessOriginPickListDetailId(int businessOriginPickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.ItemCodes.Where(q => q.BusinessOriginPickListDetailId == businessOriginPickListDetailId).Count();
            }
        }

        public static Result Save(ref ItemCodeBo bo)
        {
            if (!ItemCode.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref ItemCodeBo bo, ref TrailObject trail)
        {
            if (!ItemCode.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ItemCodeBo bo)
        {
            ItemCode entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicate(entity))
            {
                result.AddError("Duplicate Item Code found with Business Origin found and Reporting Type");
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ItemCodeBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ItemCodeBo bo)
        {
            Result result = Result();

            ItemCode entity = ItemCode.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicate(FormEntity(bo)))
            {
                result.AddError("Duplicate Item Code found with Business Origin and Reporting Type");
            }

            if (result.Valid)
            {
                entity.Code = bo.Code;
                entity.ReportingType = bo.ReportingType;
                entity.Description = bo.Description;
                entity.BusinessOriginPickListDetailId = bo.BusinessOriginPickListDetailId;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ItemCodeBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ItemCodeBo bo)
        {
            ItemCode.Delete(bo.Id);
        }

        public static Result Delete(ItemCodeBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (ItemCodeMapping.CountByItemCodeId(bo.Id) > 0)
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                DataTrail dataTrail = ItemCode.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
