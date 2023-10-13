using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Services
{
    public class DirectRetroConfigurationDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(DirectRetroConfigurationDetail)),
                Controller = ModuleBo.ModuleController.DirectRetroConfigurationDetail.ToString()
            };
        }

        public static Expression<Func<DirectRetroConfigurationDetail, DirectRetroConfigurationDetailBo>> Expression()
        {
            return entity => new DirectRetroConfigurationDetailBo
            {
                Id = entity.Id,
                DirectRetroConfigurationId = entity.DirectRetroConfigurationId,
                RiskPeriodStartDate = entity.RiskPeriodStartDate,
                RiskPeriodEndDate = entity.RiskPeriodEndDate,
                IssueDatePolStartDate = entity.IssueDatePolStartDate,
                IssueDatePolEndDate = entity.IssueDatePolEndDate,
                ReinsEffDatePolStartDate = entity.ReinsEffDatePolStartDate,
                ReinsEffDatePolEndDate = entity.ReinsEffDatePolEndDate,
                IsDefault = entity.IsDefault,
                RetroPartyId = entity.RetroPartyId,
                TreatyNo = entity.TreatyNo,
                Schedule = entity.Schedule,
                Share = entity.Share,
                PremiumSpreadTableId = entity.PremiumSpreadTableId,
                TreatyDiscountTableId = entity.TreatyDiscountTableId,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static DirectRetroConfigurationDetailBo FormBo(DirectRetroConfigurationDetail entity = null, bool foreign = true)
        {
            string dateFormat = Util.GetDateFormat();
            if (entity == null)
                return null;
            DirectRetroConfigurationDetailBo directRetroConfigurationDetailBo = new DirectRetroConfigurationDetailBo
            {
                Id = entity.Id,
                DirectRetroConfigurationId = entity.DirectRetroConfigurationId,
                RiskPeriodStartDate = entity.RiskPeriodStartDate,
                RiskPeriodStartDateStr = entity.RiskPeriodStartDate?.ToString(dateFormat),
                RiskPeriodEndDate = entity.RiskPeriodEndDate,
                RiskPeriodEndDateStr = entity.RiskPeriodEndDate?.ToString(dateFormat),
                IssueDatePolStartDate = entity.IssueDatePolStartDate,
                IssueDatePolStartDateStr = entity.IssueDatePolStartDate?.ToString(dateFormat),
                IssueDatePolEndDate = entity.IssueDatePolEndDate,
                IssueDatePolEndDateStr = entity.IssueDatePolEndDate?.ToString(dateFormat),
                ReinsEffDatePolStartDate = entity.ReinsEffDatePolStartDate,
                ReinsEffDatePolStartDateStr = entity.ReinsEffDatePolStartDate?.ToString(dateFormat),
                ReinsEffDatePolEndDate = entity.ReinsEffDatePolEndDate,
                ReinsEffDatePolEndDateStr = entity.ReinsEffDatePolEndDate?.ToString(dateFormat),
                IsDefault = entity.IsDefault,
                RetroPartyId = entity.RetroPartyId,
                RetroPartyBo = RetroPartyService.Find(entity.RetroPartyId),
                Share = entity.Share,
                TreatyNo = entity.TreatyNo,
                Schedule = entity.Schedule,
                ShareStr = Util.DoubleToString(entity.Share),
                PremiumSpreadTableId = entity.PremiumSpreadTableId,
                TreatyDiscountTableId = entity.TreatyDiscountTableId,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };

            if (foreign)
            {
                directRetroConfigurationDetailBo.DirectRetroConfigurationBo = DirectRetroConfigurationService.Find(entity.DirectRetroConfigurationId);
                directRetroConfigurationDetailBo.PremiumSpreadTableBo = PremiumSpreadTableService.Find(entity.PremiumSpreadTableId);
                directRetroConfigurationDetailBo.TreatyDiscountTableBo = TreatyDiscountTableService.Find(entity.TreatyDiscountTableId);
            }

            return directRetroConfigurationDetailBo;
        }

        public static IList<DirectRetroConfigurationDetailBo> FormBos(IList<DirectRetroConfigurationDetail> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<DirectRetroConfigurationDetailBo> bos = new List<DirectRetroConfigurationDetailBo>() { };
            foreach (DirectRetroConfigurationDetail entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static DirectRetroConfigurationDetail FormEntity(DirectRetroConfigurationDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new DirectRetroConfigurationDetail
            {
                Id = bo.Id,
                DirectRetroConfigurationId = bo.DirectRetroConfigurationId,
                RiskPeriodStartDate = bo.RiskPeriodStartDate,
                RiskPeriodEndDate = bo.RiskPeriodEndDate,
                IssueDatePolStartDate = bo.IssueDatePolStartDate,
                IssueDatePolEndDate = bo.IssueDatePolEndDate,
                ReinsEffDatePolStartDate = bo.ReinsEffDatePolStartDate,
                ReinsEffDatePolEndDate = bo.ReinsEffDatePolEndDate,
                IsDefault = bo.IsDefault,
                RetroPartyId = bo.RetroPartyId,
                TreatyNo = bo.TreatyNo?.Trim(),
                Schedule = bo.Schedule?.Trim(),
                Share = bo.Share,
                PremiumSpreadTableId = bo.PremiumSpreadTableId,
                TreatyDiscountTableId = bo.TreatyDiscountTableId,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return DirectRetroConfigurationDetail.IsExists(id);
        }

        public static DirectRetroConfigurationDetailBo Find(int id)
        {
            return FormBo(DirectRetroConfigurationDetail.Find(id));
        }

        public static DirectRetroConfigurationDetailBo FindByTreatyCodeIdRetroPartyId(int treatyCodeId, int retroPartyId)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.DirectRetroConfigurationDetails
                    .Where(q => q.DirectRetroConfiguration.TreatyCodeId == treatyCodeId)
                    .Where(q => q.RetroPartyId == retroPartyId)
                    .FirstOrDefault());
            }
        }

        public static int CountByRetroPartyId(int retroPartyId)
        {
            return DirectRetroConfigurationDetail.CountByRetroPartyId(retroPartyId);
        }

        public static int CountByPremiumSpreadTableId(int premiumSpreadTableId)
        {
            return DirectRetroConfigurationDetail.CountByPremiumSpreadTableId(premiumSpreadTableId);
        }

        public static int CountByTreatyDiscountTableId(int treatyDiscountTableId)
        {
            return DirectRetroConfigurationDetail.CountByTreatyDiscountTableId(treatyDiscountTableId);
        }

        public static IList<DirectRetroConfigurationDetailBo> GetByDirectRetroConfigurationId(int directRetroConfigurationId, bool foreign = true)
        {
            return FormBos(DirectRetroConfigurationDetail.GetByDirectRetroConfigurationId(directRetroConfigurationId), foreign);
        }

        public static IList<DirectRetroConfigurationDetailBo> GetDefaultByParentId(int directRetroConfigurationId, bool foreign = true)
        {
            using (var db = new AppDbContext())
            {
                var query = db.DirectRetroConfigurationDetails
                    .Where(q => q.DirectRetroConfigurationId == directRetroConfigurationId)
                    .Where(q => q.IsDefault);

                return FormBos(query.ToList(), foreign);
            }
        }

        public static IList<DirectRetroConfigurationDetailBo> GetByClaimProvisioningParam(int directRetroConfigurationId, DateTime? dateOfEvent, DateTime? issueDatePol, DateTime? reinsEffDatePol, bool foreign = true)
        {
            using (var db = new AppDbContext())
            {
                var query = db.DirectRetroConfigurationDetails
                    .Where(q => q.DirectRetroConfigurationId == directRetroConfigurationId);
                //.Where(q => (q.RiskPeriodStartDate.Year < riskPeriodYear || (q.RiskPeriodStartDate.Year == riskPeriodYear && q.RiskPeriodStartDate.Month <= riskPeriodMonth)))
                //.Where(q => (q.RiskPeriodEndDate.Year > riskPeriodYear || (q.RiskPeriodEndDate.Year == riskPeriodYear && q.RiskPeriodEndDate.Month >= riskPeriodMonth)))

                if (dateOfEvent.HasValue)
                {
                    query = query.Where(q =>
                        (
                            DbFunctions.TruncateTime(q.RiskPeriodStartDate) <= DbFunctions.TruncateTime(dateOfEvent)
                            && DbFunctions.TruncateTime(q.RiskPeriodEndDate) >= DbFunctions.TruncateTime(dateOfEvent)
                        )
                        ||
                        (q.RiskPeriodStartDate == null && q.RiskPeriodEndDate == null));
                }
                else
                {
                    query = query.Where(q => q.RiskPeriodStartDate == null && q.RiskPeriodEndDate == null);
                }

                if (issueDatePol.HasValue)
                {
                    query = query.Where(q =>
                        (
                            DbFunctions.TruncateTime(q.IssueDatePolStartDate) <= DbFunctions.TruncateTime(issueDatePol)
                            && DbFunctions.TruncateTime(q.IssueDatePolEndDate) >= DbFunctions.TruncateTime(issueDatePol)
                        )
                        ||
                        (q.IssueDatePolStartDate == null && q.IssueDatePolEndDate == null)
                    );
                }
                else
                {
                    query = query.Where(q => q.IssueDatePolStartDate == null && q.IssueDatePolEndDate == null);
                }

                if (reinsEffDatePol.HasValue)
                {
                    query = query.Where(q =>
                        (
                            DbFunctions.TruncateTime(q.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePol)
                            && DbFunctions.TruncateTime(q.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePol)
                        )
                        ||
                        (q.ReinsEffDatePolStartDate == null && q.ReinsEffDatePolEndDate == null)
                    );
                }
                else
                {
                    query = query.Where(q => q.ReinsEffDatePolStartDate == null && q.ReinsEffDatePolEndDate == null);
                }

                return FormBos(query.ToList(), foreign);
            }
        }

        public static IList<DirectRetroConfigurationDetailBo> GetByRiDataParam(int directRetroConfigurationId, DateTime? riskPeriodStartDate, DateTime? issueDatePol, DateTime? reinsEffDatePol, bool foreign = true)
        {
            using (var db = new AppDbContext())
            {
                var query = db.DirectRetroConfigurationDetails
                    .Where(q => q.DirectRetroConfigurationId == directRetroConfigurationId);
                //.Where(q => q.RiskPeriodStartDate == riskPeriodStartDate)
                //.Where(q => q.RiskPeriodEndDate == riskPeriodendDate)
                //.Where(q => q.IssueDatePolStartDate <= issueDatePol)
                //.Where(q => q.IssueDatePolEndDate >= issueDatePol)
                //.Where(q => q.ReinsEffDatePolStartDate <= reinsEffDatePol)
                //.Where(q => q.ReinsEffDatePolEndDate >= reinsEffDatePol)

                if (riskPeriodStartDate.HasValue)
                {
                    query = query.Where(q =>
                        (
                            DbFunctions.TruncateTime(q.RiskPeriodStartDate) <= DbFunctions.TruncateTime(riskPeriodStartDate)
                            && DbFunctions.TruncateTime(q.RiskPeriodEndDate) >= DbFunctions.TruncateTime(riskPeriodStartDate)
                        )
                        ||
                        (q.RiskPeriodStartDate == null && q.RiskPeriodEndDate == null)
                    );
                }
                else
                {
                    query = query.Where(q => q.RiskPeriodStartDate == null && q.RiskPeriodEndDate == null);
                }

                if (issueDatePol.HasValue)
                {
                    query = query.Where(q =>
                        (
                            DbFunctions.TruncateTime(q.IssueDatePolStartDate) <= DbFunctions.TruncateTime(issueDatePol)
                            && DbFunctions.TruncateTime(q.IssueDatePolEndDate) >= DbFunctions.TruncateTime(issueDatePol)
                        )
                        ||
                        (q.IssueDatePolStartDate == null && q.IssueDatePolEndDate == null)
                    );
                }
                else
                {
                    query = query.Where(q => q.IssueDatePolStartDate == null && q.IssueDatePolEndDate == null);
                }

                if (reinsEffDatePol.HasValue)
                {
                    query = query.Where(q =>
                        (
                            DbFunctions.TruncateTime(q.ReinsEffDatePolStartDate) <= DbFunctions.TruncateTime(reinsEffDatePol)
                            && DbFunctions.TruncateTime(q.ReinsEffDatePolEndDate) >= DbFunctions.TruncateTime(reinsEffDatePol)
                        )
                        ||
                        (q.ReinsEffDatePolStartDate == null && q.ReinsEffDatePolEndDate == null)
                    );
                }
                else
                {
                    query = query.Where(q => q.ReinsEffDatePolStartDate == null && q.ReinsEffDatePolEndDate == null);
                }

                return FormBos(query.ToList(), foreign);
            }
        }

        public static Result Save(ref DirectRetroConfigurationDetailBo bo)
        {
            if (!DirectRetroConfigurationDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref DirectRetroConfigurationDetailBo bo, ref TrailObject trail)
        {
            if (!DirectRetroConfigurationDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref DirectRetroConfigurationDetailBo bo)
        {
            DirectRetroConfigurationDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref DirectRetroConfigurationDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref DirectRetroConfigurationDetailBo bo)
        {
            Result result = Result();

            DirectRetroConfigurationDetail entity = DirectRetroConfigurationDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.DirectRetroConfigurationId = bo.DirectRetroConfigurationId;
                entity.RiskPeriodStartDate = bo.RiskPeriodStartDate;
                entity.RiskPeriodEndDate = bo.RiskPeriodEndDate;
                entity.IssueDatePolStartDate = bo.IssueDatePolStartDate;
                entity.IssueDatePolEndDate = bo.IssueDatePolEndDate;
                entity.ReinsEffDatePolStartDate = bo.ReinsEffDatePolStartDate;
                entity.ReinsEffDatePolEndDate = bo.ReinsEffDatePolEndDate;
                entity.IsDefault = bo.IsDefault;
                entity.RetroPartyId = bo.RetroPartyId;
                entity.TreatyNo = bo.TreatyNo;
                entity.Schedule = bo.Schedule;
                entity.Share = bo.Share;
                entity.PremiumSpreadTableId = bo.PremiumSpreadTableId;
                entity.TreatyDiscountTableId = bo.TreatyDiscountTableId;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref DirectRetroConfigurationDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(DirectRetroConfigurationDetailBo bo)
        {
            DirectRetroConfigurationDetail.Delete(bo.Id);
        }

        public static Result Delete(DirectRetroConfigurationDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = DirectRetroConfigurationDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result DeleteByDirectRetroConfigurationIdExcept(int directRetroConfigurationId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<DirectRetroConfigurationDetail> directRetroConfigurationDetails = DirectRetroConfigurationDetail.GetByDirectRetroConfigurationIdExcept(directRetroConfigurationId, saveIds);
            foreach (DirectRetroConfigurationDetail directRetroConfigurationDetail in directRetroConfigurationDetails)
            {
                DataTrail dataTrail = DirectRetroConfigurationDetail.Delete(directRetroConfigurationDetail.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<DataTrail> DeleteByDirectRetroConfigurationId(int directRetroConfigurationId)
        {
            return DirectRetroConfigurationDetail.DeleteByDirectRetroConfigurationId(directRetroConfigurationId);
        }

        public static void DeleteByDirectRetroConfigurationId(int directRetroConfigurationId, ref TrailObject trail)
        {
            Result result = Result();
            IList<DataTrail> dataTrails = DeleteByDirectRetroConfigurationId(directRetroConfigurationId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, result.Table);
                }
            }
        }
    }
}
