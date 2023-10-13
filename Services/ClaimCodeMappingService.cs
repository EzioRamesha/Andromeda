using BusinessObject;
using DataAccess.Entities;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ClaimCodeMappingService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimCodeMapping)),
                Controller = ModuleBo.ModuleController.ClaimCodeMapping.ToString()
            };
        }

        public static ClaimCodeMappingBo FormBo(ClaimCodeMapping entity = null)
        {
            if (entity == null)
                return null;
            return new ClaimCodeMappingBo
            {
                Id = entity.Id,
                MlreEventCode = entity.MlreEventCode,
                MlreBenefitCode = entity.MlreBenefitCode,
                ClaimCodeId = entity.ClaimCodeId,
                ClaimCodeBo = ClaimCodeService.Find(entity.ClaimCodeId),
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<ClaimCodeMappingBo> FormBos(IList<ClaimCodeMapping> entities = null)
        {
            if (entities == null)
                return null;
            IList<ClaimCodeMappingBo> bos = new List<ClaimCodeMappingBo>() { };
            foreach (ClaimCodeMapping entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ClaimCodeMapping FormEntity(ClaimCodeMappingBo bo = null)
        {
            if (bo == null)
                return null;
            return new ClaimCodeMapping
            {
                Id = bo.Id,
                MlreEventCode = bo.MlreEventCode,
                MlreBenefitCode = bo.MlreBenefitCode,
                ClaimCodeId = bo.ClaimCodeId,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return ClaimCodeMapping.IsExists(id);
        }

        public static ClaimCodeMappingBo Find(int id)
        {
            return FormBo(ClaimCodeMapping.Find(id));
        }

        public static int CountByClaimCodeId(int claimCodeId)
        {
            return ClaimCodeMapping.CountByClaimCodeId(claimCodeId);
        }

        public static Result Save(ref ClaimCodeMappingBo bo)
        {
            if (!ClaimCodeMapping.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref ClaimCodeMappingBo bo, ref TrailObject trail)
        {
            if (!ClaimCodeMapping.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ClaimCodeMappingBo bo)
        {
            ClaimCodeMapping entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ClaimCodeMappingBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ClaimCodeMappingBo bo)
        {
            Result result = Result();

            ClaimCodeMapping entity = ClaimCodeMapping.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.MlreEventCode = bo.MlreEventCode;
                entity.MlreBenefitCode = bo.MlreBenefitCode;
                entity.ClaimCodeId = bo.ClaimCodeId;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();

            }
            return result;
        }

        public static Result Update(ref ClaimCodeMappingBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ClaimCodeMappingBo bo)
        {
            ClaimCodeMappingDetailService.DeleteByClaimCodeMappingId(bo.Id);
            ClaimCodeMapping.Delete(bo.Id);
        }

        public static Result Delete(ClaimCodeMappingBo bo, ref TrailObject trail)
        {
            Result result = Result();

            ClaimCodeMappingDetailService.DeleteByClaimCodeMappingId(bo.Id); // DO NOT TRAIL
            DataTrail dataTrail = ClaimCodeMapping.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result ValidateMapping(ClaimCodeMappingBo bo)
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

                if (ClaimCodeMappingDetailService.CountByCombination(detail.Combination, bo) > 0)
                {
                    result.AddError("Existing Claim Code Mapping Combination Found");
                    break;
                }
            }
            return result;
        }

        public static IList<ClaimCodeMappingDetailBo> CreateDetails(ClaimCodeMappingBo bo, int createdById = 0)
        {
            var details = new List<ClaimCodeMappingDetailBo> { };
            CartesianProduct<string> claimCodeMappings = new CartesianProduct<string>(
               bo.MlreEventCode.ToArraySplitTrim(),
               bo.MlreBenefitCode.ToArraySplitTrim()
            );
            foreach (var item in claimCodeMappings.Get())
            {
                var mlreEventCode = item[0];
                var mlreBenefitCode = item[1];
                var items = new List<string>
                {
                    mlreEventCode,
                    mlreBenefitCode
                };
                details.Add(new ClaimCodeMappingDetailBo
                {
                    ClaimCodeMappingId = bo.Id,
                    Combination = string.Join("|", items),
                    MlreEventCode = string.IsNullOrEmpty(mlreEventCode) ? null : mlreEventCode,
                    MlreBenefitCode = string.IsNullOrEmpty(mlreBenefitCode) ? null : mlreBenefitCode,
                    CreatedById = createdById,
                });
            }
            return details;
        }

        public static void TrimMaxLength(ref ClaimCodeMappingDetailBo detailBo, ref Dictionary<string, List<string>> list)
        {
            var entity = new ClaimCodeMappingDetail();
            foreach (var property in (typeof(ClaimCodeMappingDetailBo)).GetProperties())
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

        public static void ProcessMappingDetail(ClaimCodeMappingBo bo, int createdById)
        {
            ClaimCodeMappingDetailService.DeleteByClaimCodeMappingId(bo.Id);
            foreach (var detail in CreateDetails(bo, createdById))
            {
                var d = detail;
                ClaimCodeMappingDetailService.Create(ref d);
            }
        }

        public static void ProcessMappingDetail(ClaimCodeMappingBo bo, int createdById, ref TrailObject trail)
        {
            ClaimCodeMappingDetailService.DeleteByClaimCodeMappingId(bo.Id);
            foreach (var detail in CreateDetails(bo, createdById))
            {
                var d = detail;
                ClaimCodeMappingDetailService.Create(ref d, ref trail);
            }
        }
    }
}
