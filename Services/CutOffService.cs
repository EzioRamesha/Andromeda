using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class CutOffService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(CutOff)),
                Controller = ModuleBo.ModuleController.CutOff.ToString()
            };
        }

        public static CutOffBo FormBo(CutOff entity = null)
        {
            if (entity == null)
                return null;
            return new CutOffBo
            {
                Id = entity.Id,
                Status = entity.Status,
                Month = entity.Month,
                Year = entity.Year,
                Quarter = entity.Quarter,
                CutOffDateTime = entity.CutOffDateTime,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<CutOffBo> FormBos(IList<CutOff> entities = null)
        {
            if (entities == null)
                return null;
            var bos = new List<CutOffBo>() { };
            foreach (var entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static CutOff FormEntity(CutOffBo bo = null)
        {
            if (bo == null)
                return null;
            return new CutOff
            {
                Id = bo.Id,
                Status = bo.Status,
                Month = bo.Month,
                Year = bo.Year,
                Quarter = bo.Quarter,
                CutOffDateTime = bo.CutOffDateTime,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return CutOff.IsExists(id);
        }

        public static CutOffBo Find(int id)
        {
            return FormBo(CutOff.Find(id));
        }

        public static bool IsCutOffProcessing()
        {
            using (var db = new AppDbContext())
            {
                return db.CutOff.Any(q => q.Status == CutOffBo.StatusProcessing);
            }
        }

        public static CutOffBo FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.CutOff.Where(q => q.Status == status).FirstOrDefault());
            }
        }

        public static CutOffBo FindByQuarter(string quarter)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.CutOff.Where(q => q.Quarter == quarter).FirstOrDefault());
            }
        }

        public static CutOffBo FindLatestByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.CutOff.Where(q => q.Status == status).LastOrDefault());
            }
        }

        public static CutOffBo FindByMonthYear(int month, int year)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.CutOff.Where(q => q.Month == month && q.Year == year).FirstOrDefault());
            }
        }

        public static IList<CutOffBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.CutOff
                    .Where(q => q.Status == CutOffBo.StatusCompleted)
                    .OrderByDescending(q => q.Year)
                    .ThenByDescending(q => q.Month)
                    .ToList());
            }
        }

        public static IList<CutOffBo> GetCutOffQuarter()
        {
            List<int> quarterEndMonth = new List<int> { 3, 6, 9, 12 };
            using (var db = new AppDbContext())
            {
                return FormBos(db.CutOff
                    .Where(q => q.Status == CutOffBo.StatusCompleted)
                    .Where(q => quarterEndMonth.Contains(q.Month))
                    .OrderByDescending(q => q.Year)
                    .ThenByDescending(q => q.Month)
                    .ToList());
            }
        }

        public static bool GetInCutOffStatusProcessing(string controller, int? id = null)
        {
            // RiDataWarehouse
            // InvoiceRegister
            // ClaimRegister
            // RetroRegister
            // SoaDataBatch
            using (var db = new AppDbContext())
            {
                var subQuery = db.CutOff.Where(u => u.Status == CutOffBo.StatusProcessing).Select(u => u.Id);

                var query = Enumerable.Empty<object>();
                switch (controller)
                {
                    case "InvoiceRegister":
                        if (id.HasValue)
                            query = db.InvoiceRegisterHistories.Where(q => q.InvoiceRegisterBatchId == id.Value && subQuery.Contains(q.CutOffId));
                        else
                            query = db.InvoiceRegisterHistories.Where(q => subQuery.Contains(q.CutOffId));
                        break;
                    case "RetroRegister":
                        if (id.HasValue)
                            query = db.RetroRegisterHistories.Where(q => q.RetroRegisterId == id.Value && subQuery.Contains(q.CutOffId));
                        else
                            query = db.RetroRegisterHistories.Where(q => subQuery.Contains(q.CutOffId));
                        break;
                    case "ClaimRegister":
                        if (id.HasValue)
                            query = db.ClaimRegisterHistories.Where(q => q.ClaimRegisterId == id.Value && subQuery.Contains(q.CutOffId));
                        else
                            query = db.ClaimRegisterHistories.Where(q => subQuery.Contains(q.CutOffId));
                        break;
                    case "RiDataWarehouse":
                        query = db.RiDataWarehouseHistories.Where(q => q.RiDataWarehouseId == id.Value && subQuery.Contains(q.CutOffId));
                        break;
                    case "SoaData":
                        if (id.HasValue)
                            query = db.SoaDataBatchHistories.Where(q => q.SoaDataBatchId == id.Value && subQuery.Contains(q.CutOffId));
                        else
                            query = db.SoaDataBatchHistories.Where(q => subQuery.Contains(q.CutOffId));
                        break;
                    case "RiData":
                        query = db.RiDataWarehouseHistories.Select(q => new { q.Id, q.CutOffId }).Where(q => subQuery.Contains(q.CutOffId));
                        break;
                    case "ClaimData":
                        query = db.ClaimRegisterHistories.Where(q => subQuery.Contains(q.CutOffId));
                        break;
                }

                return query.Count() > 0;
            }
        }

        public static Result Save(ref CutOffBo bo)
        {
            if (!CutOff.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref CutOffBo bo, ref TrailObject trail)
        {
            if (!CutOff.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref CutOffBo bo)
        {
            var entity = FormEntity(bo);
            var result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }
            return result;
        }

        public static Result Create(ref CutOffBo bo, ref TrailObject trail)
        {
            var result = Create(ref bo);
            if (result.Valid)
                result.DataTrail.Merge(ref trail, result.Table);
            return result;
        }

        public static Result Update(ref CutOffBo bo)
        {
            var result = Result();
            var entity = CutOff.Find(bo.Id);
            if (entity == null)
                throw new Exception(MessageBag.NoRecordFound);

            if (result.Valid)
            {
                entity.Status = bo.Status;
                entity.Month = bo.Month;
                entity.Year = bo.Year;
                entity.Quarter = bo.Quarter;
                entity.CutOffDateTime = bo.CutOffDateTime;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref CutOffBo bo, ref TrailObject trail)
        {
            var result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
                result.DataTrail.Merge(ref trail, result.Table);
            return result;
        }

        public static void Delete(CutOffBo bo)
        {
            CutOff.Delete(bo.Id);
        }

        public static Result Delete(CutOffBo bo, ref TrailObject trail)
        {
            var result = Result();
            if (result.Valid)
            {
                var dataTrail = CutOff.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }
    }
}
