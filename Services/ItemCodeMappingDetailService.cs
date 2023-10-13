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
    public class ItemCodeMappingDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ItemCodeMappingDetail)),
                Controller = ModuleBo.ModuleController.ItemCodeMappingDetail.ToString()
            };
        }

        public static ItemCodeMappingDetailBo FormBo(ItemCodeMappingDetail entity = null)
        {
            if (entity == null)
                return null;
            return new ItemCodeMappingDetailBo
            {
                Id = entity.Id,
                ItemCodeMappingId = entity.ItemCodeMappingId,
                ItemCodeMappingBo = ItemCodeMappingService.Find(entity.ItemCodeMappingId),
                TreatyType = entity.TreatyType,
                TreatyCode = entity.TreatyCode,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
            };
        }

        public static IList<ItemCodeMappingDetailBo> FormBos(IList<ItemCodeMappingDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<ItemCodeMappingDetailBo> bos = new List<ItemCodeMappingDetailBo>() { };
            foreach (ItemCodeMappingDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ItemCodeMappingDetail FormEntity(ItemCodeMappingDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new ItemCodeMappingDetail
            {
                Id = bo.Id,
                ItemCodeMappingId = bo.ItemCodeMappingId,
                TreatyType = bo.TreatyType,
                TreatyCode = bo.TreatyCode,
                CreatedById = bo.CreatedById,
                CreatedAt = bo.CreatedAt,
            };
        }

        public static bool IsExists(int id)
        {
            return ItemCodeMappingDetail.IsExists(id);
        }

        public static ItemCodeMappingDetailBo Find(int id)
        {
            return FormBo(ItemCodeMappingDetail.Find(id));
        }

        public static int CountByTreatyTypeId(int treatyTypeId)
        {
            using (var db = new AppDbContext())
            {
                PickListDetailBo treatyTypeBo = PickListDetailService.Find(treatyTypeId);
                if (treatyTypeBo == null)
                    return 0;

                return db.ItemCodeMappingDetails.Where(q => q.TreatyType == treatyTypeBo.Code).Count();
            }
        }

        public static int CountByTreatyCodeId(int treatyCodeId)
        {
            using (var db = new AppDbContext())
            {
                TreatyCodeBo treatyCodeBo = TreatyCodeService.Find(treatyCodeId);
                if (treatyCodeBo == null)
                    return 0;

                return db.ItemCodeMappingDetails.Where(q => q.TreatyCode == treatyCodeBo.Code).Count();
            }
        }

        public static bool IsDuplicate(ItemCodeMappingDetailBo itemCodeMappingDetailBo, ItemCodeMappingBo itemCodeMappingBo)
        {
            return ItemCodeMappingDetail.IsDuplicate(
                itemCodeMappingBo.InvoiceFieldPickListDetailId,
                itemCodeMappingDetailBo.TreatyType,
                itemCodeMappingDetailBo.TreatyCode,
                itemCodeMappingBo.BusinessOriginPickListDetailId,
                itemCodeMappingBo.Id,
                itemCodeMappingBo.ReportingType
            );
        }

        public static Result Save(ref ItemCodeMappingDetailBo bo)
        {
            if (!ItemCodeMappingDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref ItemCodeMappingDetailBo bo, ref TrailObject trail)
        {
            if (!ItemCodeMappingDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ItemCodeMappingDetailBo bo)
        {
            ItemCodeMappingDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ItemCodeMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ItemCodeMappingDetailBo bo)
        {
            Result result = Result();

            ItemCodeMappingDetail entity = ItemCodeMappingDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.ItemCodeMappingId = bo.ItemCodeMappingId;
                entity.TreatyType = bo.TreatyType;
                entity.TreatyCode = bo.TreatyCode;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ItemCodeMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ItemCodeMappingDetailBo bo)
        {
            ItemCodeMappingDetail.Delete(bo.Id);
        }

        public static Result Delete(ItemCodeMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = ItemCodeMappingDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteByItemCodeMappingId(int itemCodeMappingId)
        {
            return ItemCodeMappingDetail.DeleteByItemCodeMappingId(itemCodeMappingId);
        }

        public static void DeleteByItemCodeMappingId(int itemCodeMappingId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByItemCodeMappingId(itemCodeMappingId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(ItemCodeMappingDetail)));
                }
            }
        }
    }
}
