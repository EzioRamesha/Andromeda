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
    public class TreatyPricingPerLifeRetroVersionDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingPerLifeRetroVersionDetail)),
                Controller = ModuleBo.ModuleController.TreatyPricingPerLifeRetroVersionDetail.ToString()
            };
        }

        public static TreatyPricingPerLifeRetroVersionDetailBo FormBo(TreatyPricingPerLifeRetroVersionDetail entity = null)
        {
            if (entity == null)
                return null;

            return new TreatyPricingPerLifeRetroVersionDetailBo
            {
                Id = entity.Id,
                TreatyPricingPerLifeRetroVersionId = entity.TreatyPricingPerLifeRetroVersionId,
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

        public static IList<TreatyPricingPerLifeRetroVersionDetailBo> FormBos(IList<TreatyPricingPerLifeRetroVersionDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingPerLifeRetroVersionDetailBo> bos = new List<TreatyPricingPerLifeRetroVersionDetailBo>() { };
            foreach (TreatyPricingPerLifeRetroVersionDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingPerLifeRetroVersionDetail FormEntity(TreatyPricingPerLifeRetroVersionDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingPerLifeRetroVersionDetail
            {
                Id = bo.Id,
                TreatyPricingPerLifeRetroVersionId = bo.TreatyPricingPerLifeRetroVersionId,
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
            return TreatyPricingPerLifeRetroVersionDetail.IsExists(id);
        }

        public static TreatyPricingPerLifeRetroVersionDetailBo Find(int? id)
        {
            return FormBo(TreatyPricingPerLifeRetroVersionDetail.Find(id));
        }

        public static IList<TreatyPricingPerLifeRetroVersionDetailBo> GetByParent(int parentId, List<int> exceptions = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyPricingPerLifeRetroVersionDetails
                    .Where(q => q.TreatyPricingPerLifeRetroVersionId == parentId);

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
                return db.TreatyPricingPerLifeRetroVersionDetails
                    .Where(q => q.TreatyPricingPerLifeRetroVersionId == parentId)
                    .Count();
            }
        }

        public static Result Save(ref TreatyPricingPerLifeRetroVersionDetailBo bo)
        {
            if (!TreatyPricingPerLifeRetroVersionDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref TreatyPricingPerLifeRetroVersionDetailBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingPerLifeRetroVersionDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingPerLifeRetroVersionDetailBo bo)
        {
            TreatyPricingPerLifeRetroVersionDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingPerLifeRetroVersionDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingPerLifeRetroVersionDetailBo bo)
        {
            Result result = Result();

            TreatyPricingPerLifeRetroVersionDetail entity = TreatyPricingPerLifeRetroVersionDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingPerLifeRetroVersionId = bo.TreatyPricingPerLifeRetroVersionId;
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

        public static Result Update(ref TreatyPricingPerLifeRetroVersionDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingPerLifeRetroVersionDetailBo bo)
        {
            TreatyPricingPerLifeRetroVersionDetail.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingPerLifeRetroVersionDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingPerLifeRetroVersionDetail.Delete(bo.Id);
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

        public static void Save(string json, int parentId, int authUserId, ref TrailObject trail, bool resetId = false)
        {
            List<TreatyPricingPerLifeRetroVersionDetailBo> bos = new List<TreatyPricingPerLifeRetroVersionDetailBo>();
            if (!string.IsNullOrEmpty(json))
                bos = JsonConvert.DeserializeObject<List<TreatyPricingPerLifeRetroVersionDetailBo>>(json);

            foreach (var detailBo in bos)
            {
                var bo = detailBo;
                if (resetId)
                    bo.Id = 0;

                bo.TreatyPricingPerLifeRetroVersionId = parentId;
                bo.UpdatedById = authUserId;

                if (bo.Id == 0)
                    bo.CreatedById = authUserId;

                Save(ref bo, ref trail);
            }
        }
    }
}
