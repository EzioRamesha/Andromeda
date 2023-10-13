using BusinessObject;
using DataAccess.Entities;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AnnuityFactorService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(AnnuityFactor)),
                Controller = ModuleBo.ModuleController.AnnuityFactor.ToString()
            };
        }

        public static Expression<Func<AnnuityFactor, AnnuityFactorBo>> Expression()
        {
            return entity => new AnnuityFactorBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                CedantCode = entity.Cedant.Code,
                CedingPlanCode = entity.CedingPlanCode,
                ReinsEffDatePolStartDate = entity.ReinsEffDatePolStartDate,
                ReinsEffDatePolEndDate = entity.ReinsEffDatePolEndDate,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static AnnuityFactorBo FormBo(AnnuityFactor entity = null)
        {
            if (entity == null)
                return null;
            return new AnnuityFactorBo
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                CedantBo = CedantService.Find(entity.CedantId),
                CedingPlanCode = entity.CedingPlanCode,
                ReinsEffDatePolStartDate = entity.ReinsEffDatePolStartDate,
                ReinsEffDatePolEndDate = entity.ReinsEffDatePolEndDate,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<AnnuityFactorBo> FormBos(IList<AnnuityFactor> entities = null)
        {
            if (entities == null)
                return null;
            IList<AnnuityFactorBo> bos = new List<AnnuityFactorBo>() { };
            foreach (AnnuityFactor entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static AnnuityFactor FormEntity(AnnuityFactorBo bo = null)
        {
            if (bo == null)
                return null;
            return new AnnuityFactor
            {
                Id = bo.Id,
                CedantId = bo.CedantId,
                CedingPlanCode = bo.CedingPlanCode,
                ReinsEffDatePolStartDate = bo.ReinsEffDatePolStartDate,
                ReinsEffDatePolEndDate = bo.ReinsEffDatePolEndDate,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return AnnuityFactor.IsExists(id);
        }

        public static AnnuityFactorBo Find(int id)
        {
            return FormBo(AnnuityFactor.Find(id));
        }

        public static int CountByCedantId(int cedantId)
        {
            return AnnuityFactor.CountByCedantId(cedantId);
        }

        public static Result Save(ref AnnuityFactorBo bo)
        {
            if (!AnnuityFactor.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref AnnuityFactorBo bo, ref TrailObject trail)
        {
            if (!AnnuityFactor.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref AnnuityFactorBo bo)
        {
            AnnuityFactor entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref AnnuityFactorBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref AnnuityFactorBo bo)
        {
            Result result = Result();

            AnnuityFactor entity = AnnuityFactor.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.CedantId = bo.CedantId;
                entity.CedingPlanCode = bo.CedingPlanCode;
                entity.ReinsEffDatePolStartDate = bo.ReinsEffDatePolStartDate;
                entity.ReinsEffDatePolEndDate = bo.ReinsEffDatePolEndDate;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref AnnuityFactorBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(AnnuityFactorBo bo)
        {
            AnnuityFactorDetailService.DeleteByAnnuityFactorId(bo.Id);
            AnnuityFactorMappingService.DeleteByAnnuityFactorId(bo.Id);
            AnnuityFactor.Delete(bo.Id);
        }

        public static Result Delete(AnnuityFactorBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation 

            AnnuityFactorDetailService.DeleteByAnnuityFactorId(bo.Id, ref trail);
            AnnuityFactorMappingService.DeleteByAnnuityFactorId(bo.Id); // DO NOT TRAIL
            DataTrail dataTrail = AnnuityFactor.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result ValidateRange(AnnuityFactorBo bo)
        {
            Result result = new Result();

            if (bo.ReinsEffDatePolEndDate != null && bo.ReinsEffDatePolStartDate == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The Effective Start Date Field"));
            }
            else if (bo.ReinsEffDatePolStartDate != null && bo.ReinsEffDatePolEndDate == null)
            {
                if (DateTime.Parse(Util.GetDefaultEndDate()) <= bo.ReinsEffDatePolStartDate)
                {
                    result.AddError(string.Format(MessageBag.EndDefaultDate, "Effective", Util.GetDefaultEndDate()));
                }
            }
            else if (bo.ReinsEffDatePolStartDate != null && bo.ReinsEffDatePolEndDate != null)
            {
                if (bo.ReinsEffDatePolEndDate <= bo.ReinsEffDatePolStartDate)
                {
                    result.AddError(string.Format(MessageBag.EndDateLater, "Effective"));
                }
            }

            return result;
        }

        public static Result ValidateMapping(AnnuityFactorBo bo)
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

                if (AnnuityFactorMappingService.CountByCombination(detail.Combination, bo) > 0)
                {
                    result.AddError("Existing Annuity Factor Combination Found");
                    break;
                }
            }
            return result;
        }

        public static IList<AnnuityFactorMappingBo> CreateDetails(AnnuityFactorBo bo, int createdById = 0)
        {
            var details = new List<AnnuityFactorMappingBo> { };
            CartesianProduct<string> annuityFactorMappings = new CartesianProduct<string>(
               bo.CedingPlanCode.ToArraySplitTrim()
            );
            foreach (var item in annuityFactorMappings.Get())
            {
                var cedingPlanCode = item[0];
                var items = new List<string>
                {
                    cedingPlanCode,
                };
                details.Add(new AnnuityFactorMappingBo
                {
                    AnnuityFactorId = bo.Id,
                    Combination = string.Join("|", items),
                    CedingPlanCode = string.IsNullOrEmpty(cedingPlanCode) ? null : cedingPlanCode,
                    CreatedById = createdById,
                });
            }
            return details;
        }

        public static void TrimMaxLength(ref AnnuityFactorMappingBo detailBo, ref Dictionary<string, List<string>> list)
        {
            var entity = new AnnuityFactorMapping();
            foreach (var property in (typeof(AnnuityFactorMappingBo)).GetProperties())
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

        public static void ProcessMappingDetail(AnnuityFactorBo bo, int createdById)
        {
            AnnuityFactorMappingService.DeleteByAnnuityFactorId(bo.Id);
            foreach (var detail in CreateDetails(bo, createdById))
            {
                var d = detail;
                AnnuityFactorMappingService.Create(ref d);
            }
        }

        public static void ProcessMappingDetail(AnnuityFactorBo bo, int createdById, ref TrailObject trail)
        {
            AnnuityFactorMappingService.DeleteByAnnuityFactorId(bo.Id);
            foreach (var detail in CreateDetails(bo, createdById))
            {
                var d = detail;
                AnnuityFactorMappingService.Create(ref d, ref trail);
            }
        }
    }
}
