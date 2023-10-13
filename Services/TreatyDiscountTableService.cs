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
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TreatyDiscountTableService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyDiscountTable)),
                Controller = ModuleBo.ModuleController.TreatyDiscountTable.ToString()
            };
        }

        public static Expression<Func<TreatyDiscountTable, TreatyDiscountTableBo>> Expression()
        {
            return entity => new TreatyDiscountTableBo
            {
                Id = entity.Id,
                Rule = entity.Rule,
                Type = entity.Type,
                Description = entity.Description,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static Expression<Func<TreatyDiscountTableWithDetail, TreatyDiscountTableBo>> ExpressionWithDetail()
        {
            return entity => new TreatyDiscountTableBo
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
                AARFrom = entity.TableDetail.AARFrom,
                AARTo = entity.TableDetail.AARTo,
                Discount = entity.TableDetail.Discount,

                CreatedById = entity.Table.CreatedById,
                UpdatedById = entity.Table.UpdatedById,
            };
        }

        public static TreatyDiscountTableBo FormBo(TreatyDiscountTable entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyDiscountTableBo
            {
                Id = entity.Id,
                Rule = entity.Rule,
                Type = entity.Type,
                Description = entity.Description,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyDiscountTableBo> FormBos(IList<TreatyDiscountTable> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyDiscountTableBo> bos = new List<TreatyDiscountTableBo>() { };
            foreach (TreatyDiscountTable entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyDiscountTable FormEntity(TreatyDiscountTableBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyDiscountTable
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
            return TreatyDiscountTable.IsExists(id);
        }

        public static TreatyDiscountTableBo Find(int? id)
        {
            if (!id.HasValue)
                return null;
            return FormBo(TreatyDiscountTable.Find(id));
        }

        public static TreatyDiscountTableBo FindByRule(string rule)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyDiscountTables.Where(q => q.Rule.Trim() == rule.Trim()).FirstOrDefault());
            }
        }

        public static IList<TreatyDiscountTableBo> Get()
        {
            return FormBos(TreatyDiscountTable.Get());
        }

        public static IList<TreatyDiscountTableBo> GetByType(int type)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyDiscountTables.Where(a => a.Type == type).ToList());
            }
        }

        public static Result Save(ref TreatyDiscountTableBo bo)
        {
            if (!TreatyDiscountTable.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyDiscountTableBo bo, ref TrailObject trail)
        {
            if (!TreatyDiscountTable.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static bool IsDuplicateRule(TreatyDiscountTable treatyDiscountTable)
        {
            return treatyDiscountTable.IsDuplicateRule();
        }

        public static Result Create(ref TreatyDiscountTableBo bo)
        {
            TreatyDiscountTable entity = FormEntity(bo);

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

        public static Result Create(ref TreatyDiscountTableBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyDiscountTableBo bo)
        {
            Result result = Result();

            TreatyDiscountTable entity = TreatyDiscountTable.Find(bo.Id);
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

        public static Result Update(ref TreatyDiscountTableBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyDiscountTableBo bo)
        {
            TreatyDiscountTableDetailService.DeleteByTreatyDiscountTableId(bo.Id);
            TreatyDiscountTable.Delete(bo.Id);
        }

        public static Result Delete(TreatyDiscountTableBo bo, ref TrailObject trail)
        {
            Result result = Result();
            if (DirectRetroConfigurationDetailService.CountByTreatyDiscountTableId(bo.Id) > 0 ||
                RetroTreatyService.CountByTreatyDiscountTableById(bo.Id) > 0)
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                TreatyDiscountTableDetailService.DeleteByTreatyDiscountTableId(bo.Id, ref trail);
                DataTrail dataTrail = TreatyDiscountTable.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }

    public class TreatyDiscountTableWithDetail
    {
        public TreatyDiscountTable Table { get; set; }

        public TreatyDiscountTableDetail TableDetail { get; set; }
    }
}
