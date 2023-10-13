using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Services.Retrocession;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services
{
    public class PremiumSpreadTableService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PremiumSpreadTable)),
                Controller = ModuleBo.ModuleController.PremiumSpreadTable.ToString()
            };
        }

        public static Expression<Func<PremiumSpreadTable, PremiumSpreadTableBo>> Expression()
        {
            return entity => new PremiumSpreadTableBo
            {
                Id = entity.Id,
                Rule = entity.Rule,
                Type = entity.Type,
                Description = entity.Description,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static Expression<Func<PremiumSpreadTableWithDetail, PremiumSpreadTableBo>> ExpressionWithDetail()
        {
            return entity => new PremiumSpreadTableBo
            {
                Id = entity.Table.Id,
                Rule = entity.Table.Rule,
                Type = entity.Table.Type,
                Description = entity.Table.Description,

                DetailId = entity.TableDetail.Id,
                CedingPlanCode = entity.TableDetail.CedingPlanCode,
                BenefitCode = entity.TableDetail.BenefitCode,
                AgeFrom = entity.TableDetail.AgeFrom,
                AgeTo = entity.TableDetail.AgeTo,
                PremiumSpread = entity.TableDetail.PremiumSpread,

                CreatedById = entity.Table.CreatedById,
                UpdatedById = entity.Table.UpdatedById,
            };
        }

        public static PremiumSpreadTableBo FormBo(PremiumSpreadTable entity = null)
        {
            if (entity == null)
                return null;
            return new PremiumSpreadTableBo
            {
                Id = entity.Id,
                Rule = entity.Rule,
                Type = entity.Type,
                Description = entity.Description,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<PremiumSpreadTableBo> FormBos(IList<PremiumSpreadTable> entities = null)
        {
            if (entities == null)
                return null;
            IList<PremiumSpreadTableBo> bos = new List<PremiumSpreadTableBo>() { };
            foreach (PremiumSpreadTable entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static PremiumSpreadTable FormEntity(PremiumSpreadTableBo bo = null)
        {
            if (bo == null)
                return null;
            return new PremiumSpreadTable
            {
                Id = bo.Id,
                Rule = bo.Rule?.Trim(),
                Type = bo.Type,
                Description = bo.Description,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PremiumSpreadTable.IsExists(id);
        }

        public static PremiumSpreadTableBo Find(int? id)
        {
            return FormBo(PremiumSpreadTable.Find(id));
        }

        public static PremiumSpreadTableBo FindByRule(string rule)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.PremiumSpreadTables.Where(q => q.Rule.Trim() == rule.Trim()).FirstOrDefault());
            }
        }

        public static IList<PremiumSpreadTableBo> Get()
        {
            return FormBos(PremiumSpreadTable.Get());
        }

        public static IList<PremiumSpreadTableBo> GetByType(int type)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PremiumSpreadTables.Where(a => a.Type == type).ToList());
            }
        }

        public static Result Save(ref PremiumSpreadTableBo bo)
        {
            if (!PremiumSpreadTable.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref PremiumSpreadTableBo bo, ref TrailObject trail)
        {
            if (!PremiumSpreadTable.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicateRule(PremiumSpreadTable premiumSpreadTable)
        {
            return premiumSpreadTable.IsDuplicateRule();
        }

        public static Result Create(ref PremiumSpreadTableBo bo)
        {
            PremiumSpreadTable entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateRule(entity))
            {
                result.AddTakenError("Rule", entity.Rule);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref PremiumSpreadTableBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref PremiumSpreadTableBo bo)
        {
            Result result = Result();

            PremiumSpreadTable entity = PremiumSpreadTable.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (IsDuplicateRule(FormEntity(bo)))
            {
                result.AddTakenError("Rule", bo.Rule);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.Rule = bo.Rule;
                entity.Type = bo.Type;
                entity.Description = bo.Description;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref PremiumSpreadTableBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PremiumSpreadTableBo bo)
        {
            PremiumSpreadTableDetailService.DeleteByPremiumSpreadTableId(bo.Id);
            PremiumSpreadTable.Delete(bo.Id);
        }

        public static Result Delete(PremiumSpreadTableBo bo, ref TrailObject trail)
        {
            Result result = Result();
            if (DirectRetroConfigurationDetailService.CountByPremiumSpreadTableId(bo.Id) > 0 ||
                RetroTreatyDetailService.CountByPremiumSpreadTableId(bo.Id) > 0)
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                PremiumSpreadTableDetailService.DeleteByPremiumSpreadTableId(bo.Id, ref trail);
                DataTrail dataTrail = PremiumSpreadTable.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }

    public class PremiumSpreadTableWithDetail
    {
        public PremiumSpreadTable Table { get; set; }

        public PremiumSpreadTableDetail TableDetail { get; set; }
    }
}
