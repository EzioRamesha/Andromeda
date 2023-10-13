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
    public class Mfrs17ReportingService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Mfrs17Reporting)),
                Controller = ModuleBo.ModuleController.Mfrs17Reporting.ToString()
            };
        }

        public static Mfrs17ReportingBo FormBo(Mfrs17Reporting entity = null)
        {
            if (entity == null)
                return null;
            var bo = new Mfrs17ReportingBo
            {
                Id = entity.Id,
                Quarter = entity.Quarter,
                Status = entity.Status,
                TotalRecord = entity.TotalRecord,
                GenerateType = entity.GenerateType,
                GenerateModifiedOnly = entity.GenerateModifiedOnly,
                GeneratePercentage = entity.GeneratePercentage,
                CutOffId = entity.CutOffId,
                IsResume = entity.IsResume,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
            bo.GetQuarterObject();
            return bo;
        }

        public static IList<Mfrs17ReportingBo> FormBos(IList<Mfrs17Reporting> entities = null)
        {
            if (entities == null)
                return null;
            IList<Mfrs17ReportingBo> bos = new List<Mfrs17ReportingBo>() { };
            foreach (Mfrs17Reporting entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static Mfrs17Reporting FormEntity(Mfrs17ReportingBo bo = null)
        {
            if (bo == null)
                return null;
            return new Mfrs17Reporting
            {
                Id = bo.Id,
                Quarter = bo.Quarter,
                Status = bo.Status,
                TotalRecord = bo.TotalRecord,
                GenerateType = bo.GenerateType,
                GenerateModifiedOnly = bo.GenerateModifiedOnly,
                GeneratePercentage = bo.GeneratePercentage,
                CutOffId = bo.CutOffId,
                IsResume = bo.IsResume,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return Mfrs17Reporting.IsExists(id);
        }

        public static bool IsDuplicate(Mfrs17Reporting mfrs17Reporting)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                if (!string.IsNullOrEmpty(mfrs17Reporting.Quarter))
                {
                    EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("Mfrs17ReportingService");
                    return connectionStrategy.Execute(() =>
                    {
                        var query = db.Mfrs17Reportings.Where(q => q.Quarter == mfrs17Reporting.Quarter);
                        if (mfrs17Reporting.Id != 0)
                        {
                            query = query.Where(q => q.Id != mfrs17Reporting.Id);
                        }
                        return query.Count() > 0;
                    });
                }
                return false;
            }
        }

        public static Mfrs17ReportingBo Find(int id)
        {
            return FormBo(Mfrs17Reporting.Find(id));
        }

        public static Mfrs17ReportingBo FindByQuarter(string quarter)
        {
            return FormBo(Mfrs17Reporting.FindByQuarter(quarter));
        }

        public static Mfrs17ReportingBo FindByStatus(int status)
        {
            return FormBo(Mfrs17Reporting.FindByStatus(status));
        }

        public static int CountByStatus(int status)
        {
            return Mfrs17Reporting.CountByStatus(status);
        }

        public static Result Save(ref Mfrs17ReportingBo bo)
        {
            if (!Mfrs17Reporting.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref Mfrs17ReportingBo bo, ref TrailObject trail)
        {
            if (!Mfrs17Reporting.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref Mfrs17ReportingBo bo)
        {
            Mfrs17Reporting entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicate(entity))
            {
                result.AddTakenError("Quarter", bo.Quarter);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }
            return result;
        }

        public static Result Create(ref Mfrs17ReportingBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref Mfrs17ReportingBo bo)
        {
            Result result = Result();

            Mfrs17Reporting entity = Mfrs17Reporting.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (IsDuplicate(FormEntity(bo)))
            {
                result.AddTakenError("Quarter", bo.Quarter);
            }

            if (result.Valid)
            {
                entity.Quarter = bo.Quarter;
                entity.Status = bo.Status;
                entity.TotalRecord = bo.TotalRecord;
                entity.GenerateType = bo.GenerateType;
                entity.GenerateModifiedOnly = bo.GenerateModifiedOnly;
                entity.GeneratePercentage = bo.GeneratePercentage;
                entity.CutOffId = bo.CutOffId;
                entity.IsResume = bo.IsResume;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref Mfrs17ReportingBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(Mfrs17ReportingBo bo)
        {
            Mfrs17Reporting.Delete(bo.Id);
        }

        public static Result Delete(Mfrs17ReportingBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = Mfrs17Reporting.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result ValidateGenerateType(Mfrs17ReportingBo bo)
        {
            Result result = new Result();

            if (bo.Status == Mfrs17ReportingBo.StatusPendingGenerate && bo.GenerateType == null)
            {
                result.AddError(string.Format(MessageBag.Required, "Generate Type"));
            }

            return result;
        }
    }
}
