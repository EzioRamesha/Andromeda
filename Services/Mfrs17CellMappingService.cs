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
    public class Mfrs17CellMappingService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Mfrs17CellMapping)),
                Controller = ModuleBo.ModuleController.Mfrs17CellMapping.ToString()
            };
        }

        public static Expression<Func<Mfrs17CellMapping, Mfrs17CellMappingBo>> Expression()
        {
            return entity => new Mfrs17CellMappingBo
            {
                Id = entity.Id,
                TreatyCode = entity.TreatyCode,
                ReinsEffDatePolStartDate = entity.ReinsEffDatePolStartDate,
                ReinsEffDatePolEndDate = entity.ReinsEffDatePolEndDate,
                ReinsBasisCodePickListDetailId = entity.ReinsBasisCodePickListDetailId,
                ReinsBasisCode = entity.ReinsBasisCodePickListDetail.Code,
                CedingPlanCode = entity.CedingPlanCode,
                BenefitCode = entity.BenefitCode,
                ProfitCommPickListDetailId = entity.ProfitCommPickListDetailId,
                ProfitComm = entity.ProfitCommPickListDetail.Code,
                BasicRiderPickListDetailId = entity.BasicRiderPickListDetailId,
                BasicRider = entity.BasicRiderPickListDetail.Code,
                CellName = entity.CellName,
                //Mfrs17TreatyCode = entity.Mfrs17TreatyCode,
                Mfrs17ContractCodeDetailId = entity.Mfrs17ContractCodeDetailId,
                Mfrs17ContractCode = entity.Mfrs17ContractCodeDetail.ContractCode,
                LoaCode = entity.LoaCode,
                RateTable = entity.RateTable,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static Expression<Func<Mfrs17CellMapping, Mfrs17CellMapping>> ExpressionDetails()
        {
            return entity => new Mfrs17CellMapping
            {
                Id = entity.Id,
                TreatyCode = entity.TreatyCode,
                ReinsEffDatePolStartDate = entity.ReinsEffDatePolStartDate,
                ReinsEffDatePolEndDate = entity.ReinsEffDatePolEndDate,
                ReinsBasisCodePickListDetailId = entity.ReinsBasisCodePickListDetailId,
                CedingPlanCode = entity.CedingPlanCode,
                BenefitCode = entity.BenefitCode,
                //ProfitComm = entity.ProfitComm,
                ProfitCommPickListDetailId = entity.ProfitCommPickListDetailId,
                BasicRiderPickListDetailId = entity.BasicRiderPickListDetailId,
                CellName = entity.CellName,
                //Mfrs17TreatyCode = entity.Mfrs17TreatyCode,
                LoaCode = entity.LoaCode,
                RateTable = entity.RateTable,
                Mfrs17CellMappingDetails = entity.Mfrs17CellMappingDetails,
            };
        }

        public static Mfrs17CellMappingBo FormBo(Mfrs17CellMapping entity = null)
        {
            if (entity == null)
                return null;
            return new Mfrs17CellMappingBo
            {
                Id = entity.Id,
                TreatyCode = entity.TreatyCode,
                ReinsEffDatePolStartDate = entity.ReinsEffDatePolStartDate,
                ReinsEffDatePolEndDate = entity.ReinsEffDatePolEndDate,
                ReinsBasisCodePickListDetailId = entity.ReinsBasisCodePickListDetailId,
                ReinsBasisCodePickListDetailBo = PickListDetailService.Find(entity.ReinsBasisCodePickListDetailId),
                CedingPlanCode = entity.CedingPlanCode,
                BenefitCode = entity.BenefitCode,
                ProfitComm = entity.ProfitComm,
                ProfitCommPickListDetailId = entity.ProfitCommPickListDetailId,
                ProfitCommPickListDetailBo = PickListDetailService.Find(entity.ProfitCommPickListDetailId),
                BasicRiderPickListDetailId = entity.BasicRiderPickListDetailId,
                BasicRiderPickListDetailBo = PickListDetailService.Find(entity.BasicRiderPickListDetailId),
                CellName = entity.CellName,
                //Mfrs17TreatyCode = entity.Mfrs17TreatyCode,
                Mfrs17ContractCodeDetailId = entity.Mfrs17ContractCodeDetailId,
                Mfrs17ContractCodeDetailBo = Mfrs17ContractCodeDetailService.Find(entity.Mfrs17ContractCodeDetailId),
                LoaCode = entity.LoaCode,
                RateTable = entity.RateTable,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<Mfrs17CellMappingBo> FormBos(IList<Mfrs17CellMapping> entities = null)
        {
            if (entities == null)
                return null;
            IList<Mfrs17CellMappingBo> bos = new List<Mfrs17CellMappingBo>() { };
            foreach (Mfrs17CellMapping entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static Mfrs17CellMapping FormEntity(Mfrs17CellMappingBo bo = null)
        {
            if (bo == null)
                return null;
            return new Mfrs17CellMapping
            {
                Id = bo.Id,
                TreatyCode = bo.TreatyCode,
                ReinsEffDatePolStartDate = bo.ReinsEffDatePolStartDate,
                ReinsEffDatePolEndDate = bo.ReinsEffDatePolEndDate,
                ReinsBasisCodePickListDetailId = bo.ReinsBasisCodePickListDetailId,
                CedingPlanCode = bo.CedingPlanCode,
                BenefitCode = bo.BenefitCode,
                //ProfitComm = bo.ProfitComm,
                ProfitCommPickListDetailId = bo.ProfitCommPickListDetailId,
                BasicRiderPickListDetailId = bo.BasicRiderPickListDetailId,
                CellName = bo.CellName?.Trim(),
                //Mfrs17TreatyCode = bo.Mfrs17TreatyCode,
                Mfrs17ContractCodeDetailId = bo.Mfrs17ContractCodeDetailId,
                LoaCode = bo.LoaCode?.Trim(),
                RateTable = bo.RateTable?.Trim(),
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return Mfrs17CellMapping.IsExists(id);
        }

        public static Mfrs17CellMappingBo Find(int id)
        {
            return FormBo(Mfrs17CellMapping.Find(id));
        }

        public static int CountByCellName(string cellName)
        {
            return Mfrs17CellMapping.CountByCellName(cellName);
        }

        public static int CountByTreatyCodeId(int treatyCodeId)
        {
            return Mfrs17CellMapping.CountByTreatyCodeId(treatyCodeId);
        }

        public static int CountByReinsBasisCodePickListDetailId(int reinsBasisCodePickListDetailId)
        {
            return Mfrs17CellMapping.CountByReinsBasisCodePickListDetailId(reinsBasisCodePickListDetailId);
        }

        public static int CountByBasicRiderPickListDetailId(int basicRiderPickListDetailId)
        {
            return Mfrs17CellMapping.CountByBasicRiderPickListDetailId(basicRiderPickListDetailId);
        }

        public static int CountByMfrs17ContractCodeId(int mfrs17ContractCodeId)
        {
            using (var db = new AppDbContext())
            {
                return db.Mfrs17CellMappings.Where(q => q.Mfrs17ContractCodeDetail.Mfrs17ContractCodeId == mfrs17ContractCodeId).Count();
            }
        }

        public static int CountByMfrs17ContractCodeDetailId(int mfrs17ContractCodeDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.Mfrs17CellMappings.Where(q => q.Mfrs17ContractCodeDetailId == mfrs17ContractCodeDetailId).Count();
            }
        }

        public static IEnumerable<string> GetCellNames()
        {
            using (var db = new AppDbContext())
            {
                return db.Mfrs17CellMappings.Select(q => q.CellName).ToList();
            }
        }

        //public static IEnumerable<string> GetContractCodes()
        //{
        //    using (var db = new AppDbContext())
        //    {
        //        return db.Mfrs17CellMappings.Select(q => q.Mfrs17TreatyCode).ToList();
        //    }
        //}

        public static IEnumerable<string> GetLoaCodes()
        {
            using (var db = new AppDbContext())
            {
                return db.Mfrs17CellMappings.Select(q => q.LoaCode).ToList();
            }
        }

        public static Result Save(ref Mfrs17CellMappingBo bo)
        {
            if (!Mfrs17CellMapping.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref Mfrs17CellMappingBo bo, ref TrailObject trail)
        {
            if (!Mfrs17CellMapping.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref Mfrs17CellMappingBo bo)
        {
            Mfrs17CellMapping entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }
            return result;
        }

        public static Result Create(ref Mfrs17CellMappingBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref Mfrs17CellMappingBo bo)
        {
            Result result = Result();

            Mfrs17CellMapping entity = Mfrs17CellMapping.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.TreatyCode = bo.TreatyCode;
                entity.ReinsEffDatePolStartDate = bo.ReinsEffDatePolStartDate;
                entity.ReinsEffDatePolEndDate = bo.ReinsEffDatePolEndDate;
                entity.ReinsBasisCodePickListDetailId = bo.ReinsBasisCodePickListDetailId;
                entity.CedingPlanCode = bo.CedingPlanCode;
                entity.BenefitCode = bo.BenefitCode;
                //entity.ProfitComm = bo.ProfitComm;
                entity.ProfitCommPickListDetailId = bo.ProfitCommPickListDetailId;
                entity.BasicRiderPickListDetailId = bo.BasicRiderPickListDetailId;
                entity.CellName = bo.CellName;
                //entity.Mfrs17TreatyCode = bo.Mfrs17TreatyCode;
                entity.Mfrs17ContractCodeDetailId = bo.Mfrs17ContractCodeDetailId;
                entity.LoaCode = bo.LoaCode;
                entity.RateTable = bo.RateTable;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref Mfrs17CellMappingBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(Mfrs17CellMappingBo bo)
        {
            Mfrs17CellMappingDetailService.DeleteByMfrs17CellMappingId(bo.Id);
            Mfrs17CellMapping.Delete(bo.Id);
        }

        public static Result Delete(Mfrs17CellMappingBo bo, ref TrailObject trail)
        {
            Result result = Result();

            Mfrs17CellMappingDetailService.DeleteByMfrs17CellMappingId(bo.Id); // DO NOT TRAIL
            DataTrail dataTrail = Mfrs17CellMapping.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result ValidateRange(Mfrs17CellMappingBo bo)
        {
            Result result = new Result();

            if (bo.ReinsEffDatePolEndDate != null && bo.ReinsEffDatePolStartDate == null)
            {
                result.AddError(string.Format(MessageBag.Required, "The Policy Reinsurance Effective Start Date Field"));
            }
            else if (bo.ReinsEffDatePolStartDate != null && bo.ReinsEffDatePolEndDate == null)
            {
                if (DateTime.Parse(Util.GetDefaultEndDate()) <= bo.ReinsEffDatePolStartDate)
                {
                    result.AddError(string.Format(MessageBag.EndDefaultDate, "Policy Reinsurance Effective", Util.GetDefaultEndDate()));
                }
            }
            else if (bo.ReinsEffDatePolStartDate != null && bo.ReinsEffDatePolEndDate != null)
            {
                if (bo.ReinsEffDatePolEndDate <= bo.ReinsEffDatePolStartDate)
                {
                    result.AddError(string.Format(MessageBag.EndDateLater, "Policy Reinsurance Effective"));
                }
            }

            return result;
        }

        public static Result ValidateMapping(Mfrs17CellMappingBo bo)
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

                //if (Mfrs17CellMappingDetailService.CountByCombination(detail.Combination, bo) > 0)
                //{
                //    result.AddError("Existing MFRS17 Cell Mapping Combination Found");
                //    break;
                //}

                if (Mfrs17CellMappingDetailService.CountDuplicateByParams(bo, detail) > 0)
                {
                    result.AddError("Existing MFRS17 Cell Mapping Combination Found");
                    break;
                }
            }
            return result;
        }

        public static Result ValidateMaxLength(Mfrs17CellMappingBo bo)
        {
            Result result = new Result();

            return result;
        }

        public static IList<Mfrs17CellMappingDetailBo> CreateDetails(Mfrs17CellMappingBo bo, int createdById = 0)
        {
            var details = new List<Mfrs17CellMappingDetailBo> { };
            CartesianProduct<string> cellMappings = new CartesianProduct<string>(
                bo.CedingPlanCode.ToArraySplitTrim(),
                bo.BenefitCode.ToArraySplitTrim(),
                bo.TreatyCode.ToArraySplitTrim()
            );
            foreach (var item in cellMappings.Get())
            {
                var cedingPlanCode = item[0];
                var benefitCode = item[1];
                var treatyCode = item[2];
                var items = new List<string>
                {
                    cedingPlanCode,
                    benefitCode,
                    bo.ReinsBasisCodePickListDetailBo != null ? bo.ReinsBasisCodePickListDetailBo.Code : "",
                    treatyCode,
                    bo.ProfitCommPickListDetailBo != null ? bo.ProfitCommPickListDetailBo.Code : "",
                    bo.RateTable
                };
                details.Add(new Mfrs17CellMappingDetailBo
                {
                    Mfrs17CellMappingId = bo.Id,
                    Combination = string.Join("|", items),
                    CreatedById = createdById,
                    CedingPlanCode = string.IsNullOrEmpty(cedingPlanCode) ? null : cedingPlanCode,
                    BenefitCode = string.IsNullOrEmpty(benefitCode) ? null : benefitCode,
                    TreatyCode = string.IsNullOrEmpty(treatyCode) ? null : treatyCode,
                });
            }
            return details;
        }

        public static void TrimMaxLength(ref Mfrs17CellMappingDetailBo detailBo, ref Dictionary<string, List<string>> list)
        {
            var entity = new Mfrs17CellMappingDetail();
            foreach (var property in (typeof(Mfrs17CellMappingDetailBo)).GetProperties())
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

        public static void ProcessMappingDetail(Mfrs17CellMappingBo bo, int authUserId)
        {
            Mfrs17CellMappingDetailService.DeleteByMfrs17CellMappingId(bo.Id);
            foreach (var detail in CreateDetails(bo, authUserId))
            {
                var d = detail;
                Mfrs17CellMappingDetailService.Create(ref d);
            }
        }

        public static void ProcessMappingDetail(Mfrs17CellMappingBo bo, int authUserId, ref TrailObject trail)
        {
            Mfrs17CellMappingDetailService.DeleteByMfrs17CellMappingId(bo.Id);
            foreach (var detail in CreateDetails(bo, authUserId))
            {
                var d = detail;
                Mfrs17CellMappingDetailService.Create(ref d, ref trail);
            }
        }
    }
}
