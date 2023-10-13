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
    public class TreatyPricingMedicalTableService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingMedicalTable)),
                Controller = ModuleBo.ModuleController.TreatyPricingMedicalTable.ToString()
            };
        }

        public static Expression<Func<TreatyPricingMedicalTable, TreatyPricingMedicalTableBo>> Expression()
        {
            return entity => new TreatyPricingMedicalTableBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                MedicalTableId = entity.MedicalTableId,
                Status = entity.Status,
                Name = entity.Name,
                Description = entity.Description,
                BenefitCode = entity.BenefitCode,
                DistributionChannel = entity.DistributionChannel,
                CurrencyCode = entity.CurrencyCode,
                AgeDefinitionPickListDetailId = entity.AgeDefinitionPickListDetailId,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static TreatyPricingMedicalTableBo FormBo(TreatyPricingMedicalTable entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            return new TreatyPricingMedicalTableBo
            {
                Id = entity.Id,
                TreatyPricingCedantId = entity.TreatyPricingCedantId,
                TreatyPricingCedantBo = foreign ? TreatyPricingCedantService.Find(entity.TreatyPricingCedantId) : null,
                MedicalTableId = entity.MedicalTableId,
                Status = entity.Status,
                Name = entity.Name,
                Description = entity.Description,
                BenefitCode = entity.BenefitCode,
                DistributionChannel = entity.DistributionChannel,
                CurrencyCode = entity.CurrencyCode,
                AgeDefinitionPickListDetailId = entity.AgeDefinitionPickListDetailId,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                TreatyPricingMedicalTableVersionBos = foreign ? TreatyPricingMedicalTableVersionService.GetByTreatyPricingMedicalTableId(entity.Id) : null,
                AgeDefinitionPickListDetailBo = foreign ? PickListDetailService.Find(entity.AgeDefinitionPickListDetailId) : null,

                StatusName = TreatyPricingMedicalTableBo.GetStatusName(entity.Status),
            };
        }

        public static IList<TreatyPricingMedicalTableBo> FormBos(IList<TreatyPricingMedicalTable> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingMedicalTableBo> bos = new List<TreatyPricingMedicalTableBo>() { };
            foreach (TreatyPricingMedicalTable entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyPricingMedicalTable FormEntity(TreatyPricingMedicalTableBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingMedicalTable
            {
                Id = bo.Id,
                TreatyPricingCedantId = bo.TreatyPricingCedantId,
                MedicalTableId = bo.MedicalTableId,
                Status = bo.Status,
                Name = bo.Name,
                Description = bo.Description,
                BenefitCode = bo.BenefitCode,
                DistributionChannel = bo.DistributionChannel,
                CurrencyCode = bo.CurrencyCode,
                AgeDefinitionPickListDetailId = bo.AgeDefinitionPickListDetailId,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingMedicalTable.IsExists(id);
        }

        public static TreatyPricingMedicalTableBo Find(int? id)
        {
            return FormBo(TreatyPricingMedicalTable.Find(id));
        }

        public static IList<TreatyPricingMedicalTableBo> FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingMedicalTables.Where(q => q.Status == status).ToList());
            }
        }

        public static IList<TreatyPricingMedicalTableBo> GetByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingMedicalTables
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
                    .Where(q => q.TreatyPricingMedicalTableId.HasValue)
                    .GroupBy(q => q.TreatyPricingMedicalTable.Id)
                    .Select(g => g.FirstOrDefault())
                    .Select(q => q.TreatyPricingMedicalTable.Id.ToString())
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

                string prefix = string.Format("{0}_UWM_{1}_", cedantCode, year);

                var entity = db.TreatyPricingMedicalTables.Where(q => !string.IsNullOrEmpty(q.MedicalTableId) && q.MedicalTableId.StartsWith(prefix)).OrderByDescending(q => q.MedicalTableId).FirstOrDefault();

                int nextIndex = 0;
                if (entity != null)
                {
                    string code = entity.MedicalTableId;
                    string currentIndexStr = code.Substring(code.LastIndexOf('_') + 1);

                    int.TryParse(currentIndexStr, out nextIndex);
                }
                nextIndex++;
                string nextIndexStr = nextIndex.ToString().PadLeft(3, '0');

                return prefix + nextIndexStr;
            }
        }

        #region Medical Table Comparison Report
        public static List<int> GetIdByMedicalTableIdsTreatyPricingCedantId(int cedantId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingMedicalTables
                    .Where(q => q.TreatyPricingCedantId == cedantId)
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public static List<int> GetIdByMedicalTableIdsBenefitCode(List<int> medicalTableIds, int benefitId)
        {
            string benefitCode = BenefitService.Find(benefitId).Code;

            using (var db = new AppDbContext())
            {
                return db.TreatyPricingMedicalTables
                    .Where(q => medicalTableIds.Contains(q.Id))
                    .Where(q => q.BenefitCode.Contains(benefitCode))
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public static List<int> GetIdByMedicalTableIdsDistributionChannel(List<int> medicalTableIds, int distributionChannel)
        {
            string distributionChannelCode = PickListDetailService.Find(distributionChannel).Description;

            using (var db = new AppDbContext())
            {
                return db.TreatyPricingMedicalTables
                    .Where(q => medicalTableIds.Contains(q.Id))
                    .Where(q => q.DistributionChannel.Contains(distributionChannelCode))
                    .Select(q => q.Id)
                    .ToList();
            }
        }

        public static List<int> GetIdByMedicalTableIdsStatus(List<int> medicalTableIds, int status)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingMedicalTables
                    .Where(q => medicalTableIds.Contains(q.Id))
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

        public static IList<PickListDetailBo> GetDistinctDistributionChannelByBenefitCode(List<int> medicalTableIds, int benefitId)
        {
            using (var db = new AppDbContext())
            {
                string benefitCode = Benefit.Find(benefitId).Code;
                IList<TreatyPricingMedicalTableBo> medicalTableBos = FormBos(db.TreatyPricingMedicalTables
                    .Where(q => medicalTableIds.Contains(q.Id))
                    .Where(q => q.BenefitCode.Contains(benefitCode))
                    .Select(q => q)
                    .ToList());
                
                List<string> distributionChannelList = new List<string>();

                foreach (var bo in medicalTableBos)
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

        public static IList<StatusObject> GetDistinctStatusByDistributionChannel(List<int> medicalTableIds, int distributionChannelId)
        {
            using (var db = new AppDbContext())
            {
                string distributionChannel = PickListDetailService.Find(distributionChannelId).Description;
                IList<TreatyPricingMedicalTableBo> medicalTableBos = FormBos(db.TreatyPricingMedicalTables
                    .Where(q => medicalTableIds.Contains(q.Id))
                    .Where(q => q.DistributionChannel.Contains(distributionChannel))
                    .Select(q => q)
                    .ToList());

                List<int> statuses = new List<int>();
                List<int> distinctStatuses = new List<int>();
                List<StatusObject> distinctStatusObjects = new List<StatusObject>();
                foreach (var medicalTableBo in medicalTableBos)
                {
                    statuses.Add(medicalTableBo.Status);
                }

                distinctStatuses.AddRange(statuses.Distinct());
                foreach (int distinctStatus in distinctStatuses)
                {
                    distinctStatusObjects.Add(new StatusObject(distinctStatus, TreatyPricingMedicalTableBo.GetStatusName(distinctStatus)));
                }

                return distinctStatusObjects;
            }
        }

        public static IList<TreatyPricingMedicalTableBo> GetMedicalTablesByMedicalTableIds(List<int> medicalTableIds)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingMedicalTables
                    .Where(q => medicalTableIds.Contains(q.Id))
                    .Select(q => q)
                    .ToList());
            }
        }
        #endregion

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyPricingMedicalTables.Where(q => q.Status == status).Count();
            }
        }

        public static Result Save(ref TreatyPricingMedicalTableBo bo)
        {
            if (!TreatyPricingMedicalTable.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingMedicalTableBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingMedicalTable.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingMedicalTableBo bo)
        {
            TreatyPricingMedicalTable entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingMedicalTableBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingMedicalTableBo bo)
        {
            Result result = Result();

            TreatyPricingMedicalTable entity = TreatyPricingMedicalTable.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.TreatyPricingCedantId = bo.TreatyPricingCedantId;
                entity.MedicalTableId = bo.MedicalTableId;
                entity.Status = bo.Status;
                entity.Name = bo.Name;
                entity.Description = bo.Description;
                entity.BenefitCode = bo.BenefitCode;
                entity.DistributionChannel = bo.DistributionChannel;
                entity.CurrencyCode = bo.CurrencyCode;
                entity.AgeDefinitionPickListDetailId = bo.AgeDefinitionPickListDetailId;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingMedicalTableBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingMedicalTableBo bo)
        {
            TreatyPricingMedicalTable.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingMedicalTableBo bo, ref TrailObject trail)
        {
            Result result = Result();

            // TODO: In Use Validation

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyPricingMedicalTable.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyPricingCedantId(int treatyPricingCedantId)
        {
            return TreatyPricingMedicalTable.DeleteAllByTreatyPricingCedantId(treatyPricingCedantId);
        }

        public static void DeleteAllByTreatyPricingCedantId(int treatyPricingCedantId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyPricingCedantId(treatyPricingCedantId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(TreatyPricingMedicalTable)));
                }
            }
        }
    }

    public class StatusObject
    {
        public int Status { get; set; }

        public string StatusName { get; set; }

        public StatusObject(int status, string statusName)
        {
            Status = status;
            StatusName = statusName;
        }
    }
}
