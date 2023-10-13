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
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DirectRetroConfigurationService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(DirectRetroConfiguration)),
                Controller = ModuleBo.ModuleController.DirectRetroConfiguration.ToString()
            };
        }

        public static Expression<Func<DirectRetroConfiguration, DirectRetroConfigurationBo>> Expression()
        {
            return entity => new DirectRetroConfigurationBo
            {
                Id = entity.Id,
                Name = entity.Name,
                TreatyCodeId = entity.TreatyCodeId,
                RetroParty = entity.RetroParty,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static DirectRetroConfigurationBo FormBo(DirectRetroConfiguration entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            return new DirectRetroConfigurationBo
            {
                Id = entity.Id,
                Name = entity.Name,
                TreatyCodeId = entity.TreatyCodeId,
                TreatyCodeBo = foreign ? TreatyCodeService.Find(entity.TreatyCodeId) : null,
                RetroParty = entity.RetroParty,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<DirectRetroConfigurationBo> FormBos(IList<DirectRetroConfiguration> entities = null)
        {
            if (entities == null)
                return null;
            IList<DirectRetroConfigurationBo> bos = new List<DirectRetroConfigurationBo>() { };
            foreach (DirectRetroConfiguration entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static DirectRetroConfiguration FormEntity(DirectRetroConfigurationBo bo = null)
        {
            if (bo == null)
                return null;
            return new DirectRetroConfiguration
            {
                Id = bo.Id,
                Name = bo.Name?.Trim(),
                TreatyCodeId = bo.TreatyCodeId,
                RetroParty = bo.RetroParty,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return DirectRetroConfiguration.IsExists(id);
        }

        public static DirectRetroConfigurationBo Find(int id)
        {
            return FormBo(DirectRetroConfiguration.Find(id));
        }

        public static DirectRetroConfigurationBo FindByTreatyCodeId(int treatyCodeId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.DirectRetroConfigurations.Where(q => q.TreatyCodeId == treatyCodeId).FirstOrDefault());
            }
        }

        public static DirectRetroConfigurationBo FindByTreatyCode(string code, bool foreign = true)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.DirectRetroConfigurations.Where(q => q.TreatyCode.Code == code).FirstOrDefault(), foreign);
            }
        }

        public static int CountByTreatyCodeId(int treatyCodeId)
        {
            return DirectRetroConfiguration.CountByTreatyCodeId(treatyCodeId);
        }

        public static int CountByTreatyId(int treatyId)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetroConfigurations.Where(q => q.TreatyCode.TreatyId == treatyId).Count();
            }
        }

        public static Result Save(ref DirectRetroConfigurationBo bo)
        {
            if (!DirectRetroConfiguration.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref DirectRetroConfigurationBo bo, ref TrailObject trail)
        {
            if (!DirectRetroConfiguration.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref DirectRetroConfigurationBo bo)
        {
            DirectRetroConfiguration entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref DirectRetroConfigurationBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref DirectRetroConfigurationBo bo)
        {
            Result result = Result();

            DirectRetroConfiguration entity = DirectRetroConfiguration.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Name = bo.Name;
                entity.TreatyCodeId = bo.TreatyCodeId;
                entity.RetroParty = bo.RetroParty;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref DirectRetroConfigurationBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(DirectRetroConfigurationBo bo)
        {
            DirectRetroConfigurationDetailService.DeleteByDirectRetroConfigurationId(bo.Id);
            DirectRetroConfigurationMappingService.DeleteByDirectRetroConfigurationId(bo.Id);
            DirectRetroConfiguration.Delete(bo.Id);
        }

        public static Result Delete(DirectRetroConfigurationBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation 

            DirectRetroConfigurationDetailService.DeleteByDirectRetroConfigurationId(bo.Id, ref trail);
            DirectRetroConfigurationMappingService.DeleteByDirectRetroConfigurationId(bo.Id); // DO NOT TRAIL
            DataTrail dataTrail = DirectRetroConfiguration.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result ValidateMapping(DirectRetroConfigurationBo bo)
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

                if (DirectRetroConfigurationMappingService.CountByCombination(detail.Combination, bo) > 0)
                {
                    result.AddError("Existing Direct Retro Configuration Combination Found");
                    break;
                }
            }
            return result;
        }

        public static IList<DirectRetroConfigurationMappingBo> CreateDetails(DirectRetroConfigurationBo bo, int createdById = 0)
        {
            var details = new List<DirectRetroConfigurationMappingBo> { };
            CartesianProduct<string> directRetroConfigurationMappings = new CartesianProduct<string>(
               bo.RetroParty.ToArraySplitTrim()
            );
            foreach (var item in directRetroConfigurationMappings.Get())
            {
                var retroParty = item[0];
                var items = new List<string>
                {
                    retroParty,
                    bo.TreatyCodeBo != null ? bo.TreatyCodeBo.Code : "",
                };
                details.Add(new DirectRetroConfigurationMappingBo
                {
                    DirectRetroConfigurationId = bo.Id,
                    Combination = string.Join("|", items),
                    RetroParty = string.IsNullOrEmpty(retroParty) ? null : retroParty,
                    CreatedById = createdById,
                });
            }
            return details;
        }

        public static void TrimMaxLength(ref DirectRetroConfigurationMappingBo detailBo, ref Dictionary<string, List<string>> list)
        {
            var entity = new DirectRetroConfigurationMapping();
            foreach (var property in (typeof(DirectRetroConfigurationMappingBo)).GetProperties())
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

        public static void ProcessMappingDetail(DirectRetroConfigurationBo bo, int createdById)
        {
            DirectRetroConfigurationMappingService.DeleteByDirectRetroConfigurationId(bo.Id);
            foreach (var detail in CreateDetails(bo, createdById))
            {
                var d = detail;
                DirectRetroConfigurationMappingService.Create(ref d);
            }
        }

        public static void ProcessMappingDetail(DirectRetroConfigurationBo bo, int createdById, ref TrailObject trail)
        {
            DirectRetroConfigurationMappingService.DeleteByDirectRetroConfigurationId(bo.Id);
            foreach (var detail in CreateDetails(bo, createdById))
            {
                var d = detail;
                DirectRetroConfigurationMappingService.Create(ref d, ref trail);
            }
        }
    }
}
