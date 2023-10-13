using BusinessObject;
using BusinessObject.Sanctions;
using DataAccess.Entities;
using DataAccess.Entities.Sanctions;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Sanctions
{
    public class SanctionVerificationService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SanctionVerification)),
                Controller = ModuleBo.ModuleController.SanctionVerification.ToString()
            };
        }

        public static Expression<Func<SanctionVerification, SanctionVerificationBo>> Expression()
        {
            return entity => new SanctionVerificationBo
            {
                Id = entity.Id,
                SourceId = entity.SourceId,
                IsRiData = entity.IsRiData,
                IsClaimRegister = entity.IsClaimRegister,
                IsReferralClaim = entity.IsReferralClaim,
                Type = entity.Type,
                BatchId = entity.BatchId,
                Status = entity.Status,
                Record = entity.Record,
                UnprocessedRecords = entity.UnprocessedRecords,
                ProcessStartAt = entity.ProcessStartAt,
                ProcessEndAt = entity.ProcessEndAt,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static SanctionVerificationBo FormBo(SanctionVerification entity = null)
        {
            if (entity == null)
                return null;
            return new SanctionVerificationBo
            {
                Id = entity.Id,
                SourceId = entity.SourceId,
                IsRiData = entity.IsRiData,
                IsClaimRegister = entity.IsClaimRegister,
                IsReferralClaim = entity.IsReferralClaim,
                Type = entity.Type,
                BatchId = entity.BatchId,
                Status = entity.Status,
                Record = entity.Record,
                UnprocessedRecords = entity.UnprocessedRecords,
                ProcessStartAt = entity.ProcessStartAt,
                ProcessEndAt = entity.ProcessEndAt,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<SanctionVerificationBo> FormBos(IList<SanctionVerification> entities = null)
        {
            if (entities == null)
                return null;
            IList<SanctionVerificationBo> bos = new List<SanctionVerificationBo>() { };
            foreach (SanctionVerification entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static SanctionVerification FormEntity(SanctionVerificationBo bo = null)
        {
            if (bo == null)
                return null;
            return new SanctionVerification
            {
                Id = bo.Id,
                SourceId = bo.SourceId,
                IsRiData = bo.IsRiData,
                IsClaimRegister = bo.IsClaimRegister,
                IsReferralClaim = bo.IsReferralClaim,
                Type = bo.Type,
                BatchId = bo.BatchId,
                Status = bo.Status,
                Record = bo.Record,
                UnprocessedRecords = bo.UnprocessedRecords,
                ProcessStartAt = bo.ProcessStartAt,
                ProcessEndAt = bo.ProcessEndAt,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return SanctionVerification.IsExists(id);
        }

        public static bool IsAutoExists(int sourceId, int type)
        {
            using (var db = new AppDbContext())
            {
                var today = DateTime.Today;
                var query = db.SanctionVerifications.Where(q => q.SourceId == sourceId)
                    .Where(q => q.Type == type)
                    .Where(q => DbFunctions.TruncateTime(q.CreatedAt) == DbFunctions.TruncateTime(today));

                return query.Any();
            }
        }

        public static SanctionVerificationBo Find(int id)
        {
            return FormBo(SanctionVerification.Find(id));
        }

        public static SanctionVerificationBo FindByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.SanctionVerifications.Where(q => q.Status == status).FirstOrDefault());
            }
        }

        public static IList<SanctionVerificationBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionVerifications.ToList());
            }
        }

        public static IList<SanctionVerificationBo> GetBySourceByStatuses(int sourceId, List<int> statuses = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.SanctionVerifications.Where(q => q.SourceId == sourceId);

                if (statuses != null && statuses.Count > 0)
                {
                    query = query.Where(q => statuses.Contains(q.Status));
                }

                return FormBos(query.ToList());
            }
        }

        public static IList<SanctionVerificationBo> GetPotentialCase()
        {
            using (var db = new AppDbContext())
            {
                var bos = db.SanctionVerificationDetails
                    .Where(q => q.IsExactMatch == false)
                    .Where(q => q.IsWhitelist == false)
                    .GroupBy(g => new { g.ModuleId, g.SanctionVerification.SourceId })
                    .Select(r => new SanctionVerificationBo
                    {
                        SourceId = r.Key.SourceId,
                        DetailModuleId = r.Key.ModuleId,
                        DataCount = r.Count(),
                    })
                    .ToList();

                var SanctionVerificationBos = new List<SanctionVerificationBo> { };

                var souceBos = SourceService.FormBos(db.Sources.OrderBy(q => q.Id).ToList());

                var riDataModuleBo = ModuleService.FindByController("RiData");
                var claimRegisterModuleBo = ModuleService.FindByController("ClaimRegister");
                var referralClaimModuleBo = ModuleService.FindByController("ReferralClaim");

                foreach (var sourceBo in souceBos)
                {
                    var riDataCount = bos.Where(q => q.SourceId == sourceBo.Id).Where(q => q.DetailModuleId == riDataModuleBo.Id).Sum(q => q.DataCount);
                    var claimRegisterCount = bos.Where(q => q.SourceId == sourceBo.Id).Where(q => q.DetailModuleId == claimRegisterModuleBo.Id).Sum(q => q.DataCount);
                    var referralClaimCount = bos.Where(q => q.SourceId == sourceBo.Id).Where(q => q.DetailModuleId == referralClaimModuleBo.Id).Sum(q => q.DataCount);

                    SanctionVerificationBos.Add(new SanctionVerificationBo {
                        SourceBo = sourceBo,
                        RiDataCount = riDataCount,
                        ClaimRegisterCount = claimRegisterCount,
                        ReferralClaimCount = referralClaimCount,
                        TotalCount = riDataCount + claimRegisterCount + referralClaimCount,
                    });
                }

                return SanctionVerificationBos;
            }
        }

        public static IList<SanctionVerificationBo> GetClaimSanctionMatchedCases()
        {
            using (var db = new AppDbContext())
            {
                var bos = db.SanctionVerificationDetails
                    .GroupBy(g => new { g.ModuleId, g.SanctionVerification.SourceId })
                    .Select(r => new SanctionVerificationBo
                    {
                        SourceId = r.Key.SourceId,
                        DetailModuleId = r.Key.ModuleId,
                        DataCount = r.Count(),
                    })
                    .ToList();

                var SanctionVerificationBos = new List<SanctionVerificationBo> { };

                var souceBos = SourceService.FormBos(db.Sources.OrderBy(q => q.Id).ToList());

                var riDataModuleBo = ModuleService.FindByController("RiData");

                foreach (var sourceBo in souceBos)
                {
                    var riDataCount = bos.Where(q => q.SourceId == sourceBo.Id).Where(q => q.DetailModuleId == riDataModuleBo.Id).Sum(q => q.DataCount);

                    SanctionVerificationBos.Add(new SanctionVerificationBo
                    {
                        SourceBo = sourceBo,
                        Record = riDataCount,
                    });
                }

                return SanctionVerificationBos;
            }
        }

        public static IList<SanctionVerificationBo> GetRiDataSanctionMatchedCases()
        {
            using (var db = new AppDbContext())
            {
                var bos = db.SanctionVerificationDetails
                    .GroupBy(g => new { g.ModuleId, g.SanctionVerification.SourceId })
                    .Select(r => new SanctionVerificationBo
                    {
                        SourceId = r.Key.SourceId,
                        DetailModuleId = r.Key.ModuleId,
                        DataCount = r.Count(),
                    })
                    .ToList();

                var SanctionVerificationBos = new List<SanctionVerificationBo> { };

                var souceBos = SourceService.FormBos(db.Sources.OrderBy(q => q.Id).ToList());

                var claimRegisterModuleBo = ModuleService.FindByController("ClaimRegister");
                var referralClaimModuleBo = ModuleService.FindByController("ReferralClaim");

                foreach (var sourceBo in souceBos)
                {
                    var claimRegisterCount = bos.Where(q => q.SourceId == sourceBo.Id).Where(q => q.DetailModuleId == claimRegisterModuleBo.Id).Sum(q => q.DataCount);
                    var referralClaimCount = bos.Where(q => q.SourceId == sourceBo.Id).Where(q => q.DetailModuleId == referralClaimModuleBo.Id).Sum(q => q.DataCount);

                    SanctionVerificationBos.Add(new SanctionVerificationBo
                    {
                        SourceBo = sourceBo,
                        Record = claimRegisterCount + referralClaimCount,
                    });
                }

                return SanctionVerificationBos;
            }
        }

        public static int CountByStatus(int status)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionVerifications.Where(q => q.Status == status).Count();
            }
        }

        public static int CountBySourceId(int sourceId)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionVerifications.Where(q => q.SourceId == sourceId).Count();
            }
        }

        public static void CountTotalUnprocessed(ref SanctionVerificationBo bo)
        {
            bo.UnprocessedRecords = SanctionVerificationDetailService.CountUnprocessedByParentId(bo.Id);
        }

        public static Result UpdateTotalUnprocessed(int sanctionVerificationId, int authUserId, ref TrailObject trail)
        {
            SanctionVerificationBo bo = Find(sanctionVerificationId);
            bo.UpdatedById = authUserId;
            CountTotalUnprocessed(ref bo);

            return Update(ref bo, ref trail);
        }

        public static Result Save(ref SanctionVerificationBo bo)
        {
            if (!SanctionVerification.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref SanctionVerificationBo bo, ref TrailObject trail)
        {
            if (!SanctionVerification.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SanctionVerificationBo bo)
        {
            SanctionVerification entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref SanctionVerificationBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SanctionVerificationBo bo)
        {
            Result result = Result();

            SanctionVerification entity = SanctionVerification.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.SourceId = bo.SourceId;
                entity.IsRiData = bo.IsRiData;
                entity.IsClaimRegister = bo.IsClaimRegister;
                entity.IsReferralClaim = bo.IsReferralClaim;
                entity.Type = bo.Type;
                entity.BatchId = bo.BatchId;
                entity.Status = bo.Status;
                entity.Record = bo.Record;
                entity.UnprocessedRecords = bo.UnprocessedRecords;
                entity.ProcessStartAt = bo.ProcessStartAt;
                entity.ProcessEndAt = bo.ProcessEndAt;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SanctionVerificationBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SanctionVerificationBo bo)
        {
            //SanctionVerificationDetailService.DeleteBySanctionVerificationId(bo.Id);
            SanctionVerification.Delete(bo.Id);
        }

        public static Result Delete(SanctionVerificationBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                //SanctionVerificationDetailService.DeleteBySanctionVerificationId(bo.Id, ref trail);
                DataTrail dataTrail = SanctionVerification.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
