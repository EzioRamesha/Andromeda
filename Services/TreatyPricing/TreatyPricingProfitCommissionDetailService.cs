using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.TreatyPricing
{
    public class TreatyPricingProfitCommissionDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingProfitCommissionDetail)),
                Controller = ModuleBo.ModuleController.TreatyPricingProfitCommissionDetail.ToString()
            };
        }

        public static TreatyPricingProfitCommissionDetailBo FormBo(TreatyPricingProfitCommissionDetail entity = null)
        {
            if (entity == null)
                return null;

            return new TreatyPricingProfitCommissionDetailBo
            {
                Id = entity.Id,
                TreatyPricingProfitCommissionVersionId = entity.TreatyPricingProfitCommissionVersionId,
                SortIndex = entity.SortIndex,
                Item = entity.Item,
                Component = entity.Component,
                IsComponentEditable = entity.IsComponentEditable,
                ComponentDescription = entity.ComponentDescription,
                IsComponentDescriptionEditable = entity.IsComponentDescriptionEditable,
                IsDropDown = entity.IsDropDown,
                DropDownValue = entity.DropDownValue,
                IsEnabled = entity.IsEnabled,
                IsNetGrossRequired = entity.IsNetGrossRequired,
                IsNetGross = entity.IsNetGross,
                Value = entity.Value,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingProfitCommissionDetailBo> FormBos(IList<TreatyPricingProfitCommissionDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingProfitCommissionDetailBo> bos = new List<TreatyPricingProfitCommissionDetailBo>() { };
            foreach (TreatyPricingProfitCommissionDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingProfitCommissionDetail FormEntity(TreatyPricingProfitCommissionDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingProfitCommissionDetail
            {
                Id = bo.Id,
                TreatyPricingProfitCommissionVersionId = bo.TreatyPricingProfitCommissionVersionId,
                SortIndex = bo.SortIndex,
                Item = bo.Item,
                Component = bo.Component,
                IsComponentEditable = bo.IsComponentEditable,
                ComponentDescription = bo.ComponentDescription,
                IsComponentDescriptionEditable = bo.IsComponentDescriptionEditable,
                IsDropDown = bo.IsDropDown,
                DropDownValue = bo.DropDownValue,
                IsEnabled = bo.IsEnabled,
                IsNetGrossRequired = bo.IsNetGrossRequired,
                IsNetGross = bo.IsNetGross,
                Value = bo.Value,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingProfitCommissionDetail.IsExists(id);
        }

        public static TreatyPricingProfitCommissionDetailBo Find(int? id)
        {
            return FormBo(TreatyPricingProfitCommissionDetail.Find(id));
        }

        public static IList<TreatyPricingProfitCommissionDetailBo> GetByParent(int parentId, List<int> exceptions = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingProfitCommissionDetails
                    .Where(q => q.TreatyPricingProfitCommissionVersionId == parentId);

                if (exceptions != null)
                {
                    query = query.Where(q => !exceptions.Contains(q.Id));
                }

                return FormBos(query.OrderBy(q => q.SortIndex).ToList());
            }
        }

        public static IList<TreatyPricingProfitCommissionDetailBo> GetByParentEnabledOnly(int parentId, List<int> exceptions = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingProfitCommissionDetails
                    .Where(q => q.TreatyPricingProfitCommissionVersionId == parentId
                    && q.IsEnabled == true);

                if (exceptions != null)
                {
                    query = query.Where(q => !exceptions.Contains(q.Id));
                }

                return FormBos(query.OrderBy(q => q.SortIndex).ToList());
            }
        }

        public static string GetJsonByParent(int parentId)
        {
            using (var db = new AppDbContext())
            {
                var bos = GetByParent(parentId);

                return JsonConvert.SerializeObject(bos);
            }
        }

        public static int CountByParent(int parentId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProfitCommissionDetails
                    .Where(q => q.TreatyPricingProfitCommissionVersionId == parentId)
                    .Count();
            }
        }

        public static Result Save(ref TreatyPricingProfitCommissionDetailBo bo)
        {
            if (!TreatyPricingProfitCommissionDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref TreatyPricingProfitCommissionDetailBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingProfitCommissionDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingProfitCommissionDetailBo bo)
        {
            TreatyPricingProfitCommissionDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingProfitCommissionDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingProfitCommissionDetailBo bo)
        {
            Result result = Result();

            TreatyPricingProfitCommissionDetail entity = TreatyPricingProfitCommissionDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingProfitCommissionVersionId = bo.TreatyPricingProfitCommissionVersionId;
                entity.SortIndex = bo.SortIndex;
                entity.Item = bo.Item;
                entity.Component = bo.Component;
                entity.IsComponentEditable = bo.IsComponentEditable;
                entity.ComponentDescription = bo.ComponentDescription;
                entity.IsComponentDescriptionEditable = bo.IsComponentDescriptionEditable;
                entity.IsDropDown = bo.IsDropDown;
                entity.DropDownValue = bo.DropDownValue;
                entity.IsEnabled = bo.IsEnabled;
                entity.IsNetGrossRequired = bo.IsNetGrossRequired;
                entity.IsNetGross = bo.IsNetGross;
                entity.Value = bo.Value;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingProfitCommissionDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingProfitCommissionDetailBo bo)
        {
            TreatyPricingProfitCommissionDetail.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingProfitCommissionDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingProfitCommissionDetail.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static void DeleteByParentExcept(int parentId, List<int> exceptions, ref TrailObject trail)
        {
            foreach (var bo in GetByParent(parentId, exceptions))
            {
                Delete(bo, ref trail);
            }
        }
    }
}
