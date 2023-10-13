using BusinessObject;
using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Services.Retrocession
{
    public class PerLifeDuplicationCheckService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeDuplicationCheck)),
                Controller = ModuleBo.ModuleController.PerLifeDuplicationCheck.ToString()
            };
        }

        public static Expression<Func<PerLifeDuplicationCheck, PerLifeDuplicationCheckBo>> Expression()
        {
            return entity => new PerLifeDuplicationCheckBo
            {
                Id = entity.Id,
                ConfigurationCode = entity.ConfigurationCode,
                Description = entity.Description,
                ReinsuranceEffectiveStartDate = entity.ReinsuranceEffectiveStartDate,
                ReinsuranceEffectiveEndDate = entity.ReinsuranceEffectiveEndDate,
                TreatyCode = entity.TreatyCode,
                NoOfTreatyCode = entity.NoOfTreatyCode,
                EnableReinsuranceBasisCodeCheck = entity.EnableReinsuranceBasisCodeCheck,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static PerLifeDuplicationCheckBo FormBo(PerLifeDuplicationCheck entity = null)
        {
            if (entity == null)
                return null;
            return new PerLifeDuplicationCheckBo
            {
                Id = entity.Id,
                ConfigurationCode = entity.ConfigurationCode,
                Description = entity.Description,
                Inclusion = entity.Inclusion,
                ReinsuranceEffectiveStartDate = entity.ReinsuranceEffectiveStartDate,
                ReinsuranceEffectiveEndDate = entity.ReinsuranceEffectiveEndDate,
                TreatyCode = entity.TreatyCode,
                NoOfTreatyCode = entity.NoOfTreatyCode,
                EnableReinsuranceBasisCodeCheck = entity.EnableReinsuranceBasisCodeCheck,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<PerLifeDuplicationCheckBo> FormBos(IList<PerLifeDuplicationCheck> entities = null)
        {
            if (entities == null)
                return null;
            IList<PerLifeDuplicationCheckBo> bos = new List<PerLifeDuplicationCheckBo>() { };
            foreach (PerLifeDuplicationCheck entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PerLifeDuplicationCheck FormEntity(PerLifeDuplicationCheckBo bo = null)
        {
            if (bo == null)
                return null;
            return new PerLifeDuplicationCheck
            {
                Id = bo.Id,
                ConfigurationCode = bo.ConfigurationCode,
                Description = bo.Description,
                Inclusion = bo.Inclusion,
                ReinsuranceEffectiveStartDate = bo.ReinsuranceEffectiveStartDate,
                ReinsuranceEffectiveEndDate = bo.ReinsuranceEffectiveEndDate,
                TreatyCode = bo.TreatyCode,
                NoOfTreatyCode = bo.NoOfTreatyCode,
                EnableReinsuranceBasisCodeCheck = bo.EnableReinsuranceBasisCodeCheck,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PerLifeDuplicationCheck.IsExists(id);
        }

        public static PerLifeDuplicationCheckBo Find(int? id)
        {
            return FormBo(PerLifeDuplicationCheck.Find(id));
        }

        public static IList<PerLifeDuplicationCheckBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeDuplicationChecks.OrderBy(q => q.ConfigurationCode).ToList());
            }
        }

        public static Result Save(ref PerLifeDuplicationCheckBo bo)
        {
            if (!PerLifeDuplicationCheck.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PerLifeDuplicationCheckBo bo, ref TrailObject trail)
        {
            if (!PerLifeDuplicationCheck.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicateCode(PerLifeDuplicationCheck PerLifeDuplicationCheckCode)
        {
            return PerLifeDuplicationCheckCode.IsDuplicateCode();
        }

        public static Result Create(ref PerLifeDuplicationCheckBo bo)
        {
            PerLifeDuplicationCheck entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Code", bo.ConfigurationCode);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PerLifeDuplicationCheckBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PerLifeDuplicationCheckBo bo)
        {
            Result result = Result();

            PerLifeDuplicationCheck entity = PerLifeDuplicationCheck.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Code", bo.ConfigurationCode);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.ConfigurationCode = bo.ConfigurationCode;
                entity.Description = bo.Description;
                entity.Inclusion = bo.Inclusion;
                entity.ReinsuranceEffectiveStartDate = bo.ReinsuranceEffectiveStartDate;
                entity.ReinsuranceEffectiveEndDate = bo.ReinsuranceEffectiveEndDate;
                entity.TreatyCode = bo.TreatyCode;
                entity.NoOfTreatyCode = bo.NoOfTreatyCode;
                entity.EnableReinsuranceBasisCodeCheck = bo.EnableReinsuranceBasisCodeCheck;

                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PerLifeDuplicationCheckBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PerLifeDuplicationCheckBo bo)
        {
            PerLifeDuplicationCheckDetailService.DeleteByPerLifeDuplicationCheckId(bo.Id);
            PerLifeDuplicationCheck.Delete(bo.Id);
        }

        public static Result Delete(PerLifeDuplicationCheckBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                PerLifeDuplicationCheckDetailService.DeleteByPerLifeDuplicationCheckId(bo.Id);
                DataTrail dataTrail = PerLifeDuplicationCheck.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static Result ValidateTreatyCode(PerLifeDuplicationCheckBo bo)
        {
            Result result = new Result();

            if (bo.TreatyCode.Contains(','))
            {
                foreach (var treatyCode in bo.TreatyCode.Split(','))
                {
                    if (PerLifeDuplicationCheckDetailService.CountByTreatyCode(treatyCode.Trim(), bo) > 0)
                    {
                        result.AddError("Existing Per Life Duplication Check Treaty Code Found");
                        break;
                    }
                }
            }
            else
            {
                if (PerLifeDuplicationCheckDetailService.CountByTreatyCode(bo.TreatyCode, bo) > 0)
                {
                    result.AddError("Existing Per Life Duplication Check Treaty Code Found");
                }
            }

            return result;
        }

        public static void ProcessDuplicationCheckDetail(PerLifeDuplicationCheckBo bo, int authUserId)
        {
            var existingBo = Find(bo.Id);
            var existingTreatyCode = PerLifeDuplicationCheckDetailService.GetByPerLifeDuplicationCheckId(bo.Id).Select(a => a.TreatyCode);


            if (bo.TreatyCode.Contains(','))
            {
                var boTreatyCodes = bo.TreatyCode.Split(',');
                var treatyCodesToBeAdded = boTreatyCodes.Except(existingTreatyCode).ToList();

                if (treatyCodesToBeAdded != null)
                {
                    foreach (var treatyCode in treatyCodesToBeAdded)
                    {
                        PerLifeDuplicationCheckDetailBo detailBo = new PerLifeDuplicationCheckDetailBo()
                        {
                            PerLifeDuplicationCheckId = bo.Id,
                            TreatyCode = treatyCode.Trim(),
                            CreatedById = authUserId
                        };
                        PerLifeDuplicationCheckDetailService.Create(ref detailBo);
                    }
                }

                var treatyCodesToBeDeleted = existingTreatyCode.Except(boTreatyCodes).ToList();

                if (treatyCodesToBeDeleted != null)
                {
                    foreach (var treatyCode in treatyCodesToBeDeleted)
                    {
                        var detailBo = PerLifeDuplicationCheckDetailService.GetByPerLifeDuplicationCheckId(bo.Id).Where(a => a.TreatyCode.Contains(treatyCode.Trim())).FirstOrDefault();
                        PerLifeDuplicationCheckDetailService.Delete(detailBo);
                    }
                }
            }
            else
            {
                var boTreatyCodes = new string[] { bo.TreatyCode };
                var treatyCodesToBeAdded = boTreatyCodes.Except(existingTreatyCode).ToList();
                if (treatyCodesToBeAdded != null)
                {
                    foreach (var treatyCode in treatyCodesToBeAdded)
                    {
                        PerLifeDuplicationCheckDetailBo detailBo = new PerLifeDuplicationCheckDetailBo()
                        {
                            PerLifeDuplicationCheckId = bo.Id,
                            TreatyCode = treatyCode.Trim(),
                            CreatedById = authUserId
                        };
                        PerLifeDuplicationCheckDetailService.Create(ref detailBo);
                    }
                }

                var treatyCodesToBeDeleted = existingTreatyCode.Except(boTreatyCodes).ToList();

                if (treatyCodesToBeDeleted != null)
                {
                    foreach (var treatyCode in treatyCodesToBeDeleted)
                    {
                        var detailBo = PerLifeDuplicationCheckDetailService.GetByPerLifeDuplicationCheckId(bo.Id).Where(a => a.TreatyCode.Contains(treatyCode.Trim())).FirstOrDefault();
                        PerLifeDuplicationCheckDetailService.Delete(detailBo);
                    }
                }
            }
        }
    }
}
