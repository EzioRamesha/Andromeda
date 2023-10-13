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
    public class AccountCodeMappingDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(AccountCodeMappingDetail)),
                Controller = ModuleBo.ModuleController.AccountCodeMappingDetail.ToString()
            };
        }

        public static AccountCodeMappingDetailBo FormBo(AccountCodeMappingDetail entity = null)
        {
            if (entity == null)
                return null;
            return new AccountCodeMappingDetailBo
            {
                Id = entity.Id,
                AccountCodeMappingId = entity.AccountCodeMappingId,
                AccountCodeMappingBo = AccountCodeMappingService.Find(entity.AccountCodeMappingId),
                TreatyType = entity.TreatyType,
                ClaimCode = entity.ClaimCode,
                BusinessOrigin = entity.BusinessOrigin,
                InvoiceField = entity.InvoiceField,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
            };
        }

        public static IList<AccountCodeMappingDetailBo> FormBos(IList<AccountCodeMappingDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<AccountCodeMappingDetailBo> bos = new List<AccountCodeMappingDetailBo>() { };
            foreach (AccountCodeMappingDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static AccountCodeMappingDetail FormEntity(AccountCodeMappingDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new AccountCodeMappingDetail
            {
                Id = bo.Id,
                AccountCodeMappingId = bo.AccountCodeMappingId,
                TreatyType = bo.TreatyType,
                ClaimCode = bo.ClaimCode,
                BusinessOrigin = bo.BusinessOrigin,
                InvoiceField = bo.InvoiceField,
                CreatedById = bo.CreatedById,
                CreatedAt = bo.CreatedAt,
            };
        }

        public static bool IsExists(int id)
        {
            return AccountCodeMappingDetail.IsExists(id);
        }

        public static AccountCodeMappingDetailBo Find(int id)
        {
            return FormBo(AccountCodeMappingDetail.Find(id));
        }

        public static int CountByTreatyTypeId(int treatyTypeId)
        {
            using (var db = new AppDbContext())
            {
                PickListDetailBo treatyTypeBo = PickListDetailService.Find(treatyTypeId);
                if (treatyTypeBo == null)
                    return 0;

                return db.AccountCodeMappingDetails.Where(q => q.TreatyType == treatyTypeBo.Code).Count();
            }
        }

        public static int CountByClaimCodeId(int claimCodeId)
        {
            using (var db = new AppDbContext())
            {
                ClaimCodeBo claimCodeBo = ClaimCodeService.Find(claimCodeId);
                if (claimCodeBo == null)
                    return 0;

                return db.AccountCodeMappingDetails.Where(q => q.ClaimCode == claimCodeBo.Code).Count();
            }
        }

        public static bool IsDuplicate(AccountCodeMappingDetailBo accountCodeMappingDetailBo, AccountCodeMappingBo accountCodeMappingBo)
        {
            return AccountCodeMappingDetail.IsDuplicate(
                accountCodeMappingBo.ReportType,
                accountCodeMappingBo.Type.Value,
                accountCodeMappingBo.IsBalanceSheet,
                accountCodeMappingDetailBo.TreatyType, 
                accountCodeMappingBo.TreatyCodeId,
                accountCodeMappingDetailBo.ClaimCode,
                accountCodeMappingDetailBo.BusinessOrigin,
                accountCodeMappingBo.TransactionTypeCodePickListDetailId,
                accountCodeMappingBo.RetroRegisterFieldPickListDetailId,
                accountCodeMappingBo.ModifiedContractCodeId,
                accountCodeMappingDetailBo.InvoiceField,
                accountCodeMappingBo.Id
            );
        }

        public static Result Save(ref AccountCodeMappingDetailBo bo)
        {
            if (!AccountCodeMappingDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref AccountCodeMappingDetailBo bo, ref TrailObject trail)
        {
            if (!AccountCodeMappingDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref AccountCodeMappingDetailBo bo)
        {
            AccountCodeMappingDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref AccountCodeMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref AccountCodeMappingDetailBo bo)
        {
            Result result = Result();

            AccountCodeMappingDetail entity = AccountCodeMappingDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.AccountCodeMappingId = bo.AccountCodeMappingId;
                entity.TreatyType = bo.TreatyType;
                entity.ClaimCode = bo.ClaimCode;
                entity.BusinessOrigin = bo.BusinessOrigin;
                entity.InvoiceField = bo.InvoiceField;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref AccountCodeMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(AccountCodeMappingDetailBo bo)
        {
            AccountCodeMappingDetail.Delete(bo.Id);
        }

        public static Result Delete(AccountCodeMappingDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = AccountCodeMappingDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteByAccountCodeMappingId(int accountCodeMappingId)
        {
            return AccountCodeMappingDetail.DeleteByAccountCodeMappingId(accountCodeMappingId);
        }

        public static void DeleteByAccountCodeMappingId(int accountCodeMappingId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByAccountCodeMappingId(accountCodeMappingId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(AccountCodeMappingDetail)));
                }
            }
        }
    }
}
