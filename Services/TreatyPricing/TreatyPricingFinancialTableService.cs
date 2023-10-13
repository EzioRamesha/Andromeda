using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.TreatyPricing
{
    public class TreatyPricingFinancialTableService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingFinancialTable)),
                Controller = ModuleBo.ModuleController.TreatyPricingFinancialTable.ToString()
            };
        }

        public static Expression<Func<TreatyPricingFinancialTable, TreatyPricingFinancialTableBo>> Expression()
        {
            return entity => new TreatyPricingFinancialTableBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                FinancialTableId = entity.FinancialTableId,
                Status = entity.Status,
                Name = entity.Name,
                Description = entity.Description,
                BenefitCode = entity.BenefitCode,
                DistributionChannel = entity.DistributionChannel,
                CurrencyCode = entity.CurrencyCode,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingFinancialTableBo FormBo(TreatyPricingFinancialTable entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            return new TreatyPricingFinancialTableBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                TreatyPricingCedantBo = foreign ? TreatyPricingCedantService.Find(entity.TreatyPricingCedantId) : null,
                FinancialTableId = entity.FinancialTableId,
                Status = entity.Status,
                Name = entity.Name,
                Description = entity.Description,
                BenefitCode = entity.BenefitCode,
                DistributionChannel = entity.DistributionChannel,
                CurrencyCode = entity.CurrencyCode,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                TreatyPricingFinancialTableVersionBos = foreign ? TreatyPricingFinancialTableVersionService.GetByTreatyPricingFinancialTableId(entity.Id) : null,

                StatusName = TreatyPricingFinancialTableBo.GetStatusName(entity.Status),
            };
        }

        public static IList<TreatyPricingFinancialTableBo> FormBos(IList<TreatyPricingFinancialTable> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingFinancialTableBo> bos = new List<TreatyPricingFinancialTableBo>() { };
            foreach (TreatyPricingFinancialTable entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingFinancialTable FormEntity(TreatyPricingFinancialTableBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingFinancialTable
            {
                Id = bo.Id,
                TreatyPricingCedantId = bo.TreatyPricingCedantId,
                FinancialTableId = bo.FinancialTableId,
                Status = bo.Status,
                Name = bo.Name,
                Description = bo.Description,
                BenefitCode = bo.BenefitCode,
                DistributionChannel = bo.DistributionChannel,
                CurrencyCode = bo.CurrencyCode,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingFinancialTable.IsExists(id);
        }

        public static TreatyPricingFinancialTableBo Find(int? id)
        {
            return FormBo(TreatyPricingFinancialTable.Find(id));
        }

        public static IList<TreatyPricingFinancialTableBo> FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingFinancialTables.Where(q => q.Status == status).ToList());
            }
        }

        public static IList<TreatyPricingFinancialTableBo> GetByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingFinancialTables
                    .Where(q => q.TreatyPricingCedantId == treatyPricingCedantId)
                    .ToList());
            }
        }

        public static List<string> GetCodeByTreatyPricingCedantIdProductName(int treatyPricingCedantId, string productName)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingProductVersions
                    .Where(q => q.TreatyPricingProduct.TreatyPricingCedantId == treatyPricingCedantId)
                    .Where(q => q.TreatyPricingProduct.Name.Contains(productName))
                    .Where(q => q.TreatyPricingFinancialTableId.HasValue)
                    .GroupBy(q => q.TreatyPricingFinancialTable.Id)
                    .Select(g => g.FirstOrDefault())
                    .Select(q => q.TreatyPricingFinancialTable.Id.ToString())
                    .ToList();
            }
        }

        public static string GetNextObjectId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                var tpCedant = db.TreatyPricingCedants.Where(q => q.Id == treatyPricingCedantId).FirstOrDefault();
                string cedantCode = tpCedant.Code;
                int year = DateTime.Today.Year;

                string prefix = string.Format("{0}_UWF_{1}_", cedantCode, year);

                var entity = db.TreatyPricingFinancialTables.Where(q => !string.IsNullOrEmpty(q.FinancialTableId) && q.FinancialTableId.StartsWith(prefix)).OrderByDescending(q => q.FinancialTableId).FirstOrDefault();

                int nextIndex = 0;
                if (entity != null)
                {
                    string code = entity.FinancialTableId;
                    string currentIndexStr = code.Substring(code.LastIndexOf('_') + 1);

                    int.TryParse(currentIndexStr, out nextIndex);
                }
                nextIndex++;
                string nextIndexStr = nextIndex.ToString().PadLeft(3, '0');

                return prefix + nextIndexStr;
            }
        }

        #region Financial Table Comparison Report
        public static List<int> GetIdByFinancialTableIdsTreatyPricingCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingFinancialTables
                    .Where(q => q.TreatyPricingCedantId == cedantId)
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public static List<int> GetIdByFinancialTableIdsBenefitCode(List<int> financialTableIds, int benefitId)
        {
            string benefitCode = BenefitService.Find(benefitId).Code;

            using (var db = new AppDbContext())
            {
                return db.TreatyPricingFinancialTables
                    .Where(q => financialTableIds.Contains(q.Id))
                    .Where(q => q.BenefitCode.Contains(benefitCode))
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public static List<int> GetIdByFinancialTableIdsDistributionChannel(List<int> financialTableIds, int distributionChannel)
        {
            string distributionChannelCode = PickListDetailService.Find(distributionChannel).Description;

            using (var db = new AppDbContext())
            {
                return db.TreatyPricingFinancialTables
                    .Where(q => financialTableIds.Contains(q.Id))
                    .Where(q => q.DistributionChannel.Contains(distributionChannelCode))
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public static List<int> GetIdByFinancialTableIdsStatus(List<int> financialTableIds, int status)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingFinancialTables
                    .Where(q => financialTableIds.Contains(q.Id))
                    .Where(q => q.Status == status)
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public static IList<BenefitBo> GetDistinctBenefitCodeByCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                var bos = GetByTreatyPricingCedantId(cedantId);
                List<string> benefitCodeList = new List<string>();

                foreach (var bo in bos)
                {
                    if (bo.BenefitCode != null)
                    {
                        var benefitCodes = bo.BenefitCode.Split(',').ToList().Select(s => s.Trim());

                        foreach (string benefitCode in benefitCodes)
                        {
                            benefitCodeList.Add(benefitCode);
                        }
                    }
                }

                List<string> distinctBenefitCodeList = new List<string>();
                distinctBenefitCodeList.AddRange(benefitCodeList.Distinct());

                IList<Benefit> entity = new List<Benefit>();
                foreach (string distinctBenefitCode in distinctBenefitCodeList)
                {
                    entity.Add(Benefit.FindByCode(distinctBenefitCode));
                }

                var benefitBos = BenefitService.FormBos(entity);

                return benefitBos;
            }
        }

        public static IList<PickListDetailBo> GetDistinctDistributionChannelByBenefitCode(List<int> financialTableIds, int benefitId)
        {
            using (var db = new AppDbContext())
            {
                string benefitCode = Benefit.Find(benefitId).Code;
                IList<TreatyPricingFinancialTableBo> financialTableBos = FormBos(db.TreatyPricingFinancialTables
                    .Where(q => financialTableIds.Contains(q.Id))
                    .Where(q => q.BenefitCode.Contains(benefitCode))
                    .Select(q => q)
                    .ToList());

                List<string> distributionChannelList = new List<string>();

                foreach (var bo in financialTableBos)
                {
                    if (bo.DistributionChannel != null)
                    {
                        var distributionChannels = bo.DistributionChannel.Split(',').ToList().Select(s => s.Trim());

                        foreach (string distributionChannel in distributionChannels)
                        {
                            distributionChannelList.Add(distributionChannel);
                        }
                    }
                }

                List<string> distinctDistributionChannelList = new List<string>();
                distinctDistributionChannelList.AddRange(distributionChannelList.Distinct());

                int pickListId = PickListBo.DistributionChannel;
                IList<PickListDetail> entity = new List<PickListDetail>();
                foreach (string distinctDistributionChannel in distinctDistributionChannelList)
                {
                    if (db.PickListDetails.Where(q => q.PickListId == pickListId && q.Description == distinctDistributionChannel).Count() > 0)
                        entity.Add(db.PickListDetails.Where(q => q.PickListId == pickListId && q.Description == distinctDistributionChannel).FirstOrDefault());
                }

                var distributionChannelBos = PickListDetailService.FormBos(entity);

                return distributionChannelBos;
            }
        }

        public static IList<StatusObject> GetDistinctStatusByDistributionChannel(List<int> financialTableIds, int distributionChannelId)
        {
            using (var db = new AppDbContext())
            {
                string distributionChannel = PickListDetailService.Find(distributionChannelId).Description;
                IList<TreatyPricingFinancialTableBo> financialTableBos = FormBos(db.TreatyPricingFinancialTables
                    .Where(q => financialTableIds.Contains(q.Id))
                    .Where(q => q.DistributionChannel.Contains(distributionChannel))
                    .Select(q => q)
                    .ToList());

                List<int> statuses = new List<int>();
                List<int> distinctStatuses = new List<int>();
                List<StatusObject> distinctStatusObjects = new List<StatusObject>();
                foreach (var financialTableBo in financialTableBos)
                {
                    statuses.Add(financialTableBo.Status);
                }

                distinctStatuses.AddRange(statuses.Distinct());
                foreach (int distinctStatus in distinctStatuses)
                {
                    distinctStatusObjects.Add(new StatusObject(distinctStatus, TreatyPricingFinancialTableBo.GetStatusName(distinctStatus)));
                }

                return distinctStatusObjects;
            }
        }

        public static IList<TreatyPricingFinancialTableBo> GetFinancialTablesByFinancialTableIds(List<int> financialTableIds)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingFinancialTables
                    .Where(q => financialTableIds.Contains(q.Id))
                    .Select(q => q)
                    .ToList());
            }
        }
        #endregion

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingFinancialTables.Where(q => q.Status == status).Count();
            }
        }

        public static Result Save(ref TreatyPricingFinancialTableBo bo)
        {
            if (!TreatyPricingFinancialTable.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingFinancialTableBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingFinancialTable.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingFinancialTableBo bo)
        {
            TreatyPricingFinancialTable entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingFinancialTableBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingFinancialTableBo bo)
        {
            Result result = Result();

            TreatyPricingFinancialTable entity = TreatyPricingFinancialTable.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingCedantId = bo.TreatyPricingCedantId;
                entity.FinancialTableId = bo.FinancialTableId;
                entity.Status = bo.Status;
                entity.Name = bo.Name;
                entity.Description = bo.Description;
                entity.BenefitCode = bo.BenefitCode;
                entity.DistributionChannel = bo.DistributionChannel;
                entity.CurrencyCode = bo.CurrencyCode;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingFinancialTableBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingFinancialTableBo bo)
        {
            TreatyPricingFinancialTable.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingFinancialTableBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingFinancialTable.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            return TreatyPricingFinancialTable.DeleteAllByTreatyPricingCedantId(treatyPricingCedantId);
        }

        public static void DeleteAllByTreatyPricingCedantId(int treatyPricingCedantId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingCedantId(treatyPricingCedantId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingFinancialTable)));
                }
            }
        }
    }
}
