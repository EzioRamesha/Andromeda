using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AccountCodeService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(AccountCode)),
                Controller = ModuleBo.ModuleController.AccountCode.ToString()
            };
        }

        public static AccountCodeBo FormBo(AccountCode entity = null)
        {
            if (entity == null)
                return null;
            return new AccountCodeBo
            {
                Id = entity.Id,
                Code = entity.Code,
                ReportingType = entity.ReportingType,
                Description = entity.Description,
                Type = entity.Type,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<AccountCodeBo> FormBos(IList<AccountCode> entities = null)
        {
            if (entities == null)
                return null;
            IList<AccountCodeBo> bos = new List<AccountCodeBo>() { };
            foreach (AccountCode entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static AccountCode FormEntity(AccountCodeBo bo = null)
        {
            if (bo == null)
                return null;
            return new AccountCode
            {
                Id = bo.Id,
                Code = bo.Code?.Trim(),
                ReportingType = bo.ReportingType,
                Description = bo.Description?.Trim(),
                Type = bo.Type,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateCode(AccountCode accountCode)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(accountCode.Code?.Trim()))
                {
                    var query = db.AccountCodes
                        .Where(q => q.Type == accountCode.Type)
                        .Where(q => q.Code.Trim().Equals(accountCode.Code.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (accountCode.Id != 0)
                    {
                        query = query.Where(q => q.Id != accountCode.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static bool IsExists(int id)
        {
            return AccountCode.IsExists(id);
        }

        public static AccountCodeBo Find(int id)
        {
            return FormBo(AccountCode.Find(id));
        }

        public static AccountCodeBo Find(int? id)
        {
            return FormBo(AccountCode.Find(id));
        }

        public static AccountCodeBo FindByCode(string code)
        {
            return FormBo(AccountCode.FindByCode(code));
        }

        public static AccountCodeBo FindByCodeType(string code, int type)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.AccountCodes
                    .Where(q => q.Code == code)
                    .Where(q => q.Type == type)
                    .FirstOrDefault());
            }
        }

        public static IList<AccountCodeBo> Get()
        {
            return FormBos(AccountCode.Get());
        }

        public static IList<AccountCodeBo> GetByType(int type)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.AccountCodes.Where(q => q.Type == type).OrderBy(q => q.Code).ToList());
            }
        }

        public static IList<AccountCodeBo> GetByReportType(int reportType)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.AccountCodes.Where(q => q.ReportingType == reportType).OrderBy(q => q.Code).ToList());
            }
        }

        public static Result Save(ref AccountCodeBo bo)
        {
            if (!AccountCode.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref AccountCodeBo bo, ref TrailObject trail)
        {
            if (!AccountCode.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref AccountCodeBo bo)
        {
            AccountCode entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Code", bo.Code.ToString());
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref AccountCodeBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref AccountCodeBo bo)
        {
            Result result = Result();

            AccountCode entity = AccountCode.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Code", bo.Code.ToString());
            }

            if (result.Valid)
            {
                entity.Code = bo.Code;
                entity.ReportingType = bo.ReportingType;
                entity.Description = bo.Description;
                entity.Type = bo.Type;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref AccountCodeBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(AccountCodeBo bo)
        {
            AccountCode.Delete(bo.Id);
        }

        public static Result Delete(AccountCodeBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (AccountCodeMappingService.CountByAccountCodeId(bo.Id) > 0)
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                DataTrail dataTrail = AccountCode.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
