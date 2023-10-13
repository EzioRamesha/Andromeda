using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Services
{
    public class ItemCodeMappingService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ItemCodeMapping)),
                Controller = ModuleBo.ModuleController.ItemCodeMapping.ToString()
            };
        }

        public static Expression<Func<ItemCodeMapping, ItemCodeMappingBo>> Expression()
        {
            return entity => new ItemCodeMappingBo
            {
                Id = entity.Id,
                ItemCodeId = entity.ItemCodeId,
                ItemCode = entity.ItemCode.Code,
                InvoiceFieldPickListDetailId = entity.InvoiceFieldPickListDetailId,
                InvoiceField = entity.InvoiceFieldPickListDetail.Code,
                TreatyType = entity.TreatyType,
                TreatyCode = entity.TreatyCode,
                BusinessOriginPickListDetailId = entity.BusinessOriginPickListDetailId,
                BusinessOrigin = entity.BusinessOriginPickListDetail.Code,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static ItemCodeMappingBo FormBo(ItemCodeMapping entity = null)
        {
            if (entity == null)
                return null;

            return new ItemCodeMappingBo
            {
                Id = entity.Id,
                ItemCodeId = entity.ItemCodeId,
                InvoiceFieldPickListDetailId = entity.InvoiceFieldPickListDetailId,
                TreatyType = entity.TreatyType,
                TreatyCode = entity.TreatyCode,
                BusinessOriginPickListDetailId = entity.BusinessOriginPickListDetailId,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                ItemCodeBo = ItemCodeService.Find(entity.ItemCodeId),
                InvoiceFieldPickListDetailBo = PickListDetailService.Find(entity.InvoiceFieldPickListDetailId),
                BusinessOriginPickListDetailBo = PickListDetailService.Find(entity.BusinessOriginPickListDetailId),
            };
        }

        public static IList<ItemCodeMappingBo> FormBos(IList<ItemCodeMapping> entities = null)
        {
            if (entities == null)
                return null;
            IList<ItemCodeMappingBo> bos = new List<ItemCodeMappingBo>() { };
            foreach (ItemCodeMapping entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ItemCodeMapping FormEntity(ItemCodeMappingBo bo = null)
        {
            if (bo == null)
                return null;
            return new ItemCodeMapping
            {
                Id = bo.Id,
                ItemCodeId = bo.ItemCodeId,
                InvoiceFieldPickListDetailId = bo.InvoiceFieldPickListDetailId,
                TreatyType = bo.TreatyType,
                TreatyCode = bo.TreatyCode,
                BusinessOriginPickListDetailId = bo.BusinessOriginPickListDetailId,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return ItemCodeMapping.IsExists(id);
        }

        public static ItemCodeMappingBo Find(int id)
        {
            return FormBo(ItemCodeMapping.Find(id));
        }

        public static int CountByItemCodeId(int itemCodeId)
        {
            return ItemCodeMapping.CountByItemCodeId(itemCodeId);
        }

        public static IList<ItemCodeMappingBo> FindByE1Param(string treatyType, int businessOrigin, int reportingType, List<int> invoiceFieldIds = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.ItemCodeMappingDetails.Where(q => q.TreatyType == treatyType)
                    .Where(q => q.ItemCodeMapping.BusinessOriginPickListDetailId.HasValue && q.ItemCodeMapping.BusinessOriginPickListDetailId == businessOrigin)
                    .Where(q => q.ItemCodeMapping.InvoiceFieldPickListDetailId.HasValue && invoiceFieldIds.Contains(q.ItemCodeMapping.InvoiceFieldPickListDetailId.Value))
                    .Where(q => q.ItemCodeMapping.ItemCode.ReportingType == reportingType);

                return FormBos(query.GroupBy(q => q.ItemCodeMappingId).Select(q => q.FirstOrDefault()).OrderBy(q => q.ItemCodeMappingId).Select(q => q.ItemCodeMapping).ToList());
            }
        }

        public static Result Save(ref ItemCodeMappingBo bo)
        {
            if (!ItemCodeMapping.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref ItemCodeMappingBo bo, ref TrailObject trail)
        {
            if (!ItemCodeMapping.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ItemCodeMappingBo bo)
        {
            ItemCodeMapping entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ItemCodeMappingBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ItemCodeMappingBo bo)
        {
            Result result = Result();

            ItemCodeMapping entity = ItemCodeMapping.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.ItemCodeId = bo.ItemCodeId;
                entity.InvoiceFieldPickListDetailId = bo.InvoiceFieldPickListDetailId;
                entity.TreatyType = bo.TreatyType;
                entity.TreatyCode = bo.TreatyCode;
                entity.BusinessOriginPickListDetailId = bo.BusinessOriginPickListDetailId;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ItemCodeMappingBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ItemCodeMappingBo bo)
        {
            ItemCodeMappingDetailService.DeleteByItemCodeMappingId(bo.Id);
            ItemCodeMapping.Delete(bo.Id);
        }

        public static Result Delete(ItemCodeMappingBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                ItemCodeMappingDetailService.DeleteByItemCodeMappingId(bo.Id); // DO NOT TRAIL
                DataTrail dataTrail = ItemCodeMapping.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static Result ValidateMapping(ItemCodeMappingBo bo)
        {
            Result result = new Result();
            var list = new Dictionary<string, List<string>> { };

            foreach (var detail in CreateDetails(bo))
            {
                var d = detail;
                TrimMaxLength(ref d, ref list);
                if (list.Count > 0)
                {
                    foreach (var prop in list)
                    {
                        result.AddError(string.Format("Exceeded Max Length: {0}", prop.Key));
                    }
                    break;
                }

                if (ItemCodeMappingDetailService.IsDuplicate(detail, bo))
                {
                    result.AddError("Existing Item Code Mapping Combination Found");
                    break;
                }
            }
            return result;
        }

        public static IList<ItemCodeMappingDetailBo> CreateDetails(ItemCodeMappingBo bo, int createdById = 0)
        {
            var details = new List<ItemCodeMappingDetailBo> { };
            CartesianProduct<string> ccountCodeMappings = new CartesianProduct<string>(
                bo.TreatyType.ToArraySplitTrim(),
                bo.TreatyCode.ToArraySplitTrim()
            );
            foreach (var item in ccountCodeMappings.Get())
            {
                var treatyType = item[0];
                var treatyCode = item[1];
                details.Add(new ItemCodeMappingDetailBo
                {
                    ItemCodeMappingId = bo.Id,
                    TreatyType = string.IsNullOrEmpty(treatyType) ? null : treatyType,
                    TreatyCode = string.IsNullOrEmpty(treatyCode) ? null : treatyCode,
                    CreatedById = createdById,
                });
            }
            return details;
        }

        public static void TrimMaxLength(ref ItemCodeMappingDetailBo detailBo, ref Dictionary<string, List<string>> list)
        {
            var entity = new ItemCodeMappingDetail();
            foreach (var property in (typeof(ItemCodeMappingDetailBo)).GetProperties())
            {
                var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>(property.Name);
                if (maxLengthAttr != null)
                {
                    var value = property.GetValue(detailBo, null);
                    if (value != null && value is string @string && !string.IsNullOrEmpty(@string))
                    {
                        if (@string.Length > maxLengthAttr.Length)
                        {
                            string propName = string.Format("{0}({1})", property.Name, maxLengthAttr.Length);

                            if (!list.ContainsKey(propName))
                                list.Add(propName, new List<string> { });

                            var oldValue = @string;
                            var newValue = @string.Substring(0, maxLengthAttr.Length);
                            var formatValue = string.Format("{0}|{1}", oldValue, newValue);

                            if (!list[propName].Contains(formatValue))
                                list[propName].Add(formatValue);

                            property.SetValue(detailBo, newValue);
                        }
                    }
                }
            }
        }

        public static void ProcessMappingDetail(ItemCodeMappingBo bo, int authUserId)
        {
            ItemCodeMappingDetailService.DeleteByItemCodeMappingId(bo.Id);
            foreach (var detail in CreateDetails(bo, authUserId))
            {
                var d = detail;
                ItemCodeMappingDetailService.Create(ref d);
            }
        }

        public static void ProcessMappingDetail(ItemCodeMappingBo bo, int authUserId, ref TrailObject trail)
        {
            ItemCodeMappingDetailService.DeleteByItemCodeMappingId(bo.Id);
            foreach (var detail in CreateDetails(bo, authUserId))
            {
                var d = detail;
                ItemCodeMappingDetailService.Create(ref d, ref trail);
            }
        }
    }

}
