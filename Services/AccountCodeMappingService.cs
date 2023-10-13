using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Services
{
    public class AccountCodeMappingService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(AccountCodeMapping)),
                Controller = ModuleBo.ModuleController.AccountCodeMapping.ToString()
            };
        }

        public static AccountCodeMappingBo FormBo(AccountCodeMapping entity = null)
        {
            if (entity == null)
                return null;

            return new AccountCodeMappingBo
            {
                Id = entity.Id,
                ReportType = entity.ReportType,
                Type = entity.Type,
                TreatyType = entity.TreatyType,
                TreatyNumber = entity.TreatyNumber,
                ClaimCode = entity.ClaimCode,
                TreatyCodeId = entity.TreatyCodeId,
                BusinessOrigin = entity.BusinessOrigin,
                AccountCodeId = entity.AccountCodeId,
                DebitCreditIndicatorPositive = entity.DebitCreditIndicatorPositive,
                DebitCreditIndicatorNegative = entity.DebitCreditIndicatorNegative,
                TransactionTypeCodePickListDetailId = entity.TransactionTypeCodePickListDetailId,
                RetroRegisterFieldPickListDetailId = entity.RetroRegisterFieldPickListDetailId,
                ModifiedContractCodeId = entity.ModifiedContractCodeId,
                ModifiedContractCodeBo = entity.ModifiedContractCodeId.HasValue ? Mfrs17ContractCodeService.Find(entity.ModifiedContractCodeId.Value) : null,
                InvoiceField = entity.InvoiceField,
                IsBalanceSheet = entity.IsBalanceSheet,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                AccountCodeBo = AccountCodeService.Find(entity.AccountCodeId),
                TransactionTypeCodePickListDetailBo = PickListDetailService.Find(entity.TransactionTypeCodePickListDetailId),
                RetroRegisterFieldPickListDetailBo = PickListDetailService.Find(entity.RetroRegisterFieldPickListDetailId),
            };
        }

        public static IList<AccountCodeMappingBo> FormBos(IList<AccountCodeMapping> entities = null)
        {
            if (entities == null)
                return null;
            IList<AccountCodeMappingBo> bos = new List<AccountCodeMappingBo>() { };
            foreach (AccountCodeMapping entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static AccountCodeMapping FormEntity(AccountCodeMappingBo bo = null)
        {
            if (bo == null)
                return null;
            return new AccountCodeMapping
            {
                Id = bo.Id,
                ReportType = bo.ReportType,
                Type = bo.Type,
                TreatyType = bo.TreatyType,
                TreatyNumber = bo.TreatyNumber,
                ClaimCode = bo.ClaimCode,
                TreatyCodeId = bo.TreatyCodeId,
                BusinessOrigin = bo.BusinessOrigin,
                AccountCodeId = bo.AccountCodeId,
                DebitCreditIndicatorPositive = bo.DebitCreditIndicatorPositive,
                DebitCreditIndicatorNegative = bo.DebitCreditIndicatorNegative,
                TransactionTypeCodePickListDetailId = bo.TransactionTypeCodePickListDetailId,
                RetroRegisterFieldPickListDetailId = bo.RetroRegisterFieldPickListDetailId,
                ModifiedContractCodeId = bo.ModifiedContractCodeId,
                InvoiceField = bo.InvoiceField,
                IsBalanceSheet = bo.IsBalanceSheet,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        //public static bool IsDuplicateCode(AccountCodeMapping accountCodeMapping)
        //{
        //    using (var db = new AppDbContext())
        //    {
        //        var query = db.AccountCodeMappings
        //            .Where(q => q.Type == accountCodeMapping.Type)
        //            .Where(q => q.TreatyTypePickListDetailId == accountCodeMapping.TreatyTypePickListDetailId);

        //        if (accountCodeMapping.ClaimCodeId.HasValue)
        //        {
        //            query = query.Where(q => q.ClaimCodeId == accountCodeMapping.ClaimCodeId || q.ClaimCodeId == null);
        //        }

        //        //if (accountCodeMapping.TreatyCodeId.HasValue)
        //        //{
        //        //    query = query.Where(q => q.TreatyCodeId == accountCodeMapping.TreatyCodeId || q.TreatyCodeId == null);
        //        //}

        //        if (accountCodeMapping.TransactionTypeCodePickListDetailId.HasValue)
        //        {
        //            query = query.Where(q => q.TransactionTypeCodePickListDetailId == accountCodeMapping.TransactionTypeCodePickListDetailId || q.TransactionTypeCodePickListDetailId == null);
        //        }

        //        if (accountCodeMapping.Id != 0)
        //        {
        //            query = query.Where(q => q.Id != accountCodeMapping.Id);
        //        }

        //        return query.Count() > 0;
        //    }
        //}

        public static bool IsExists(int id)
        {
            return AccountCodeMapping.IsExists(id);
        }

        public static AccountCodeMappingBo Find(int id)
        {
            return FormBo(AccountCodeMapping.Find(id));
        }

        public static bool IsDuplicate(AccountCodeMappingBo accountCodeMappingBo)
        {
            using (var db = new AppDbContext())
            {
                var query = db.AccountCodeMappings
                    .Where(q => q.ReportType == accountCodeMappingBo.ReportType)
                    .Where(q => q.Type == accountCodeMappingBo.Type)
                    .Where(q => q.TreatyType == null)
                    .Where(q => q.ClaimCode == null);

                if (accountCodeMappingBo.TransactionTypeCodePickListDetailId.HasValue)
                {
                    query = query.Where(q =>
                    (q.TransactionTypeCodePickListDetailId.HasValue && q.TransactionTypeCodePickListDetailId == accountCodeMappingBo.TransactionTypeCodePickListDetailId) ||
                    !q.TransactionTypeCodePickListDetailId.HasValue);
                }

                if (accountCodeMappingBo.RetroRegisterFieldPickListDetailId.HasValue)
                {
                    query = query.Where(q =>
                    (q.RetroRegisterFieldPickListDetailId.HasValue && q.RetroRegisterFieldPickListDetailId == accountCodeMappingBo.RetroRegisterFieldPickListDetailId) ||
                    !q.RetroRegisterFieldPickListDetailId.HasValue);
                }

                return (query.Where(q => q.Id != accountCodeMappingBo.Id).Any());
            }
        }

        public static bool IsDuplicateBalanceSheetForClaimProvisionAndRecoveryIfrs4(AccountCodeMappingBo bo)
        {
            using (var db = new AppDbContext())
            {
                var query = db.AccountCodeMappings
                    .Where(q => q.ReportType == bo.ReportType)
                    .Where(q => q.Type == bo.Type)
                    .Where(q => q.TreatyType == null)
                    .Where(q => !q.TreatyCodeId.HasValue)
                    .Where(q => q.ClaimCode == null)
                    .Where(q => q.BusinessOrigin == null)
                    .Where(q => !q.TransactionTypeCodePickListDetailId.HasValue);

                return (query.Where(q => q.Id != bo.Id).Any());
            }
        }

        public static bool IsDuplicateCombinationForRetroIfrs4(AccountCodeMappingBo bo)
        {
            using (var db = new AppDbContext())
            {
                var query = db.AccountCodeMappings
                    .Where(q => q.ReportType == bo.ReportType)
                    .Where(q => q.Type == bo.Type)
                    .Where(q => q.TreatyType == null)
                    .Where(q => q.ClaimCode == null)
                    .Where(q => q.BusinessOrigin == null);


                if (bo.TreatyCodeId.HasValue)
                {
                    query = query.Where(q => (q.TreatyCodeId.HasValue && q.TreatyCodeId == bo.TreatyCodeId) || !q.TreatyCodeId.HasValue);
                }

                if (bo.TransactionTypeCodePickListDetailId.HasValue)
                {
                    query = query.Where(q => (q.TransactionTypeCodePickListDetailId.HasValue && q.TransactionTypeCodePickListDetailId == bo.TransactionTypeCodePickListDetailId) ||
                    !q.TransactionTypeCodePickListDetailId.HasValue);
                }

                if (bo.RetroRegisterFieldPickListDetailId.HasValue)
                {
                    query = query.Where(q => (q.RetroRegisterFieldPickListDetailId.HasValue && q.RetroRegisterFieldPickListDetailId == bo.RetroRegisterFieldPickListDetailId) ||
                    !q.RetroRegisterFieldPickListDetailId.HasValue);
                }

                query = query.Where(q => q.Id != bo.Id);

                return query.Any();
            }
        }

        public static bool IsDuplicateBalanceSheetForClaimProvisionAndRecoveryIfrs17(AccountCodeMappingBo bo)
        {
            using (var db = new AppDbContext())
            {
                var query = db.AccountCodeMappings
                    .Where(q => q.ReportType == bo.ReportType)
                    .Where(q => q.Type == bo.Type)
                    .Where(q => q.IsBalanceSheet == bo.IsBalanceSheet)
                    .Where(q => q.ModifiedContractCodeId == bo.ModifiedContractCodeId);

                return (query.Where(q => q.Id != bo.Id).Any());
            }
        }

        public static bool IsDuplicateCombinationForClaimProvisionAndRecoveryIfrs17(AccountCodeMappingBo bo)
        {
            using (var db = new AppDbContext())
            {
                var balanceSheet = db.AccountCodeMappings
                    .Where(q => q.ReportType == bo.ReportType)
                    .Where(q => q.Type == bo.Type)
                    .Where(q => !q.ModifiedContractCodeId.HasValue).FirstOrDefault();

                var query = db.AccountCodeMappings
                    .Where(q => q.ReportType == bo.ReportType)
                    .Where(q => q.Type == bo.Type);

                if (bo.ModifiedContractCodeId.HasValue)
                {
                    if (bo.ModifiedContractCodeId.HasValue)
                    {
                        query = query.Where(q => (q.ModifiedContractCodeId.HasValue && q.ModifiedContractCodeId == bo.ModifiedContractCodeId) ||
                        !q.ModifiedContractCodeId.HasValue);
                    }
                }

                if (balanceSheet != null)
                {
                    query = query.Where(q => q.Id != balanceSheet.Id);
                }

                return query.Any();
            }
        }

        public static bool IsDuplicateBalanceSheetForCedantAccountCodeIfrs17(AccountCodeMappingBo bo)
        {
            using (var db = new AppDbContext())
            {
                var query = db.AccountCodeMappings
                    .Where(q => q.ReportType == bo.ReportType)
                    .Where(q => q.Type == bo.Type)
                    .Where(q => q.IsBalanceSheet == bo.IsBalanceSheet)
                    .Where(q => q.ModifiedContractCodeId == bo.ModifiedContractCodeId)
                    .Where(q => q.InvoiceField == null);

                return (query.Where(q => q.Id != bo.Id).Any());
            }
        }

        public static bool AtLeast1InRetro(AccountCodeMappingBo bo)
        {
            if (bo.Type == AccountCodeMappingBo.TypeDirectRetro || bo.Type == AccountCodeMappingBo.TypePerLifeRetro)
            {
                if (bo.ReportType == AccountCodeMappingBo.ReportTypeIfrs4)
                {
                    if (string.IsNullOrEmpty(bo.TreatyType) && !bo.TreatyCodeId.HasValue && string.IsNullOrEmpty(bo.ClaimCode) && string.IsNullOrEmpty(bo.BusinessOrigin) && !bo.TransactionTypeCodePickListDetailId.HasValue && !bo.RetroRegisterFieldPickListDetailId.HasValue)
                    {
                        return false;
                    }
                }
                else if (bo.ReportType == AccountCodeMappingBo.ReportTypeIfrs17)
                {
                    if (!bo.ModifiedContractCodeId.HasValue && !bo.RetroRegisterFieldPickListDetailId.HasValue && !bo.IsBalanceSheet)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static int CountByAccountCodeId(int accountCodeId)
        {
            return AccountCodeMapping.CountByAccountCodeId(accountCodeId);
        }

        public static IList<AccountCodeMappingBo> FindByTreatyTypeIdClaimCodeTransactionType(string treatyType, List<string> claimCodes = null, List<string> transactionTypeCodes = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.AccountCodeMappingDetails
                    .Where(q => q.AccountCodeMapping.Type == AccountCodeMappingBo.TypeDirectRetro)
                    .Where(q => q.TreatyType != null && q.TreatyType == treatyType
                    && ((q.ClaimCode != null && claimCodes.Contains(q.ClaimCode))
                    || (q.AccountCodeMapping.TransactionTypeCodePickListDetailId != null && transactionTypeCodes.Contains(q.AccountCodeMapping.TransactionTypeCodePickListDetail.Code))));

                return FormBos(query.GroupBy(q => q.AccountCodeMappingId).Select(q => q.FirstOrDefault()).OrderBy(q => q.AccountCodeMappingId).Select(q => q.AccountCodeMapping).ToList());

                //var query2 = db.AccountCodeMappings
                //    .Where(q => q.Type == AccountCodeMappingBo.TypeDirectRetro)
                //    .Where(q => q.TreatyType == null)
                //    .Where(q => q.ClaimCode == null)
                //    .Where(q => q.TransactionTypeCodePickListDetailId != null && transactionTypeCodes.Contains(q.TransactionTypeCodePickListDetail.Code));

                //return FormBos(query2.ToList());
            }
        }

        public static IList<AccountCodeMappingBo> FindByE2Ifrs4Param(string treatyType, List<int> retroRegisterFieldIds = null, List<string> claimCodes = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.AccountCodeMappingDetails
                    .Where(q => q.AccountCodeMapping.Type == AccountCodeMappingBo.TypeDirectRetro)
                    .Where(q => q.AccountCodeMapping.ReportType == AccountCodeMappingBo.ReportTypeIfrs4)
                    .Where(q => q.AccountCodeMapping.RetroRegisterFieldPickListDetailId.HasValue && retroRegisterFieldIds.Contains(q.AccountCodeMapping.RetroRegisterFieldPickListDetailId.Value));

                if (!string.IsNullOrEmpty(treatyType))
                    query = query.Where(q => q.TreatyType == treatyType);

                return FormBos(query.GroupBy(q => q.AccountCodeMappingId).Select(q => q.FirstOrDefault()).OrderBy(q => q.AccountCodeMappingId).Select(q => q.AccountCodeMapping).ToList());
            }
        }

        public static AccountCodeMappingBo FindClaimEntryAccountCode()
        {
            using (var db = new AppDbContext())
            {
                // to include claim entry account code (414161) 
                return FormBo(db.AccountCodeMappings
                    .Where(q => q.Type == AccountCodeMappingBo.TypeDirectRetro)
                    .Where(q => q.RetroRegisterFieldPickListDetailId.HasValue && q.RetroRegisterFieldPickListDetail.Code == PickListDetailBo.RetroRegisterFieldClaim)
                    .FirstOrDefault());
            }
        }

        public static IList<AccountCodeMappingBo> FindByE1Ifrs17Param(List<string> modifiedContractCode = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.AccountCodeMappings
                    .Where(q => q.Type == AccountCodeMappingBo.TypeCedantAccountCode);

                if (modifiedContractCode != null && modifiedContractCode.Count != 0)
                    query = query.Where(q => q.ModifiedContractCodeId.HasValue && modifiedContractCode.Contains(q.ModifiedContractCode.ModifiedContractCode));

                return FormBos(query.OrderBy(q => q.Id).ToList());
            }
        }

        public static IList<AccountCodeMappingBo> FindByE2Ifrs17Param(string modifiedContractCode = "", string treatyNumber = "", List<int> retroRegisterFieldIds = null, bool isBalanceSheet = false)
        {
            using (var db = new AppDbContext())
            {
                var query = db.AccountCodeMappings
                    .Where(q => q.Type == AccountCodeMappingBo.TypeDirectRetro)
                    .Where(q => q.ReportType == AccountCodeMappingBo.ReportTypeIfrs17)
                    //.Where(q => q.ModifiedContractCode.ModifiedContractCode == modifiedContractCode)
                    //.Where(q => q.RetroRegisterFieldPickListDetailId.HasValue && retroRegisterFieldIds.Contains(q.RetroRegisterFieldPickListDetailId.Value))
                    .Where(q => q.IsBalanceSheet == isBalanceSheet);

                if (!string.IsNullOrEmpty(modifiedContractCode))
                    query = query.Where(q => q.ModifiedContractCode.ModifiedContractCode == modifiedContractCode);
                else
                    query = query.Where(q => !q.ModifiedContractCodeId.HasValue);

                if (retroRegisterFieldIds != null)
                    query = query.Where(q => q.RetroRegisterFieldPickListDetailId.HasValue && retroRegisterFieldIds.Contains(q.RetroRegisterFieldPickListDetailId.Value));

                if (!string.IsNullOrEmpty(treatyNumber))
                    query = query.Where(q => q.TreatyNumber == treatyNumber);

                return FormBos(query.OrderBy(q => q.Id).ToList());
            }
        }

        public static AccountCodeMappingBo FindByE3E4Ifrs17Param(int type, string modifiedContractCode)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(modifiedContractCode))
                {
                    return FormBo(db.AccountCodeMappings
                        .Where(q => q.ReportType == AccountCodeMappingBo.ReportTypeIfrs17)
                        .Where(q => q.Type == type)
                        .Where(q => q.ModifiedContractCode.ModifiedContractCode == modifiedContractCode)
                        .Where(q => q.IsBalanceSheet == false)
                        .FirstOrDefault());
                }

                // Prevent get balance sheet account code
                return FormBo(null);
            }
        }

        public static AccountCodeMappingBo FindByE3E4Ifrs4Param(int type, string treatyType, string claimCode)
        {
            using (var db = new AppDbContext())
            {
                if (string.IsNullOrEmpty(treatyType) && string.IsNullOrEmpty(claimCode))
                {
                    // Prevent get balance sheet account code
                    return FormBo(null);
                }
                else
                {
                    var query = db.AccountCodeMappingDetails
                    .Where(q => q.AccountCodeMapping.ReportType == AccountCodeMappingBo.ReportTypeIfrs4)
                    .Where(q => q.AccountCodeMapping.Type == type)
                    .Where(q => q.AccountCodeMapping.IsBalanceSheet == false);

                    if (!string.IsNullOrEmpty(treatyType))
                    {
                        query = query.Where(q => q.TreatyType == treatyType || q.TreatyType == null);
                    }
                    else
                    {
                        query = query.Where(q => q.TreatyType == null);
                    }

                    if (!string.IsNullOrEmpty(claimCode))
                    {
                        query = query.Where(q => q.ClaimCode == claimCode || q.ClaimCode == null);
                    }
                    else
                    {
                        query = query.Where(q => q.ClaimCode == null);
                    }

                    return FormBo(query.GroupBy(q => q.AccountCodeMappingId).Select(q => q.FirstOrDefault()).OrderBy(q => q.AccountCodeMappingId).Select(q => q.AccountCodeMapping).FirstOrDefault());
                }
            }
        }

        public static AccountCodeMappingBo FindIfrs4BalanceSheet(int type)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.AccountCodeMappings
                    .Where(q => q.ReportType == AccountCodeMappingBo.ReportTypeIfrs4)
                    .Where(q => q.Type == type)
                    //.Where(q => q.TreatyCodeId == null)
                    //.Where(q => q.ClaimCode == null)
                    //.Where(q => q.BusinessOrigin == null)
                    //.Where(q => q.TransactionTypeCodePickListDetailId == null)
                    .Where(q => q.IsBalanceSheet == true)
                    .FirstOrDefault());
            }
        }

        public static AccountCodeMappingBo FindIfrs17BalanceSheet(int type, string modifiedContractCode)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.AccountCodeMappings
                    .Where(q => q.ReportType == AccountCodeMappingBo.ReportTypeIfrs17)
                    .Where(q => q.Type == type)
                    .Where(q => q.ModifiedContractCode.ModifiedContractCode == modifiedContractCode)
                    .Where(q => q.IsBalanceSheet == true)
                    .FirstOrDefault());
            }
        }

        public static AccountCodeMappingBo FindCedantAccountCodeByModifiedContractCode(string modifiedContractCode)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.AccountCodeMappings
                    .Where(q => q.Type == AccountCodeMappingBo.TypeCedantAccountCode)
                    .Where(q => q.ModifiedContractCode.ModifiedContractCode == modifiedContractCode)
                    .Where(q => q.IsBalanceSheet == false)
                    .FirstOrDefault());
            }
        }

        public static IList<AccountCodeMappingBo> Get()
        {
            return FormBos(AccountCodeMapping.Get());
        }

        public static Result Save(ref AccountCodeMappingBo bo)
        {
            if (!AccountCodeMapping.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref AccountCodeMappingBo bo, ref TrailObject trail)
        {
            if (!AccountCodeMapping.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref AccountCodeMappingBo bo)
        {
            AccountCodeMapping entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref AccountCodeMappingBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref AccountCodeMappingBo bo)
        {
            Result result = Result();

            AccountCodeMapping entity = AccountCodeMapping.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.ReportType = bo.ReportType;
                entity.Type = bo.Type;
                entity.TreatyType = bo.TreatyType;
                entity.TreatyNumber = bo.TreatyNumber;
                entity.ClaimCode = bo.ClaimCode;
                entity.TreatyCodeId = bo.TreatyCodeId;
                entity.BusinessOrigin = bo.BusinessOrigin;
                entity.AccountCodeId = bo.AccountCodeId;
                entity.DebitCreditIndicatorPositive = bo.DebitCreditIndicatorPositive;
                entity.DebitCreditIndicatorNegative = bo.DebitCreditIndicatorNegative;
                entity.TransactionTypeCodePickListDetailId = bo.TransactionTypeCodePickListDetailId;
                entity.RetroRegisterFieldPickListDetailId = bo.RetroRegisterFieldPickListDetailId;
                entity.ModifiedContractCodeId = bo.ModifiedContractCodeId;
                entity.InvoiceField = bo.InvoiceField;
                entity.IsBalanceSheet = bo.IsBalanceSheet;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref AccountCodeMappingBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(AccountCodeMappingBo bo)
        {
            AccountCodeMappingDetailService.DeleteByAccountCodeMappingId(bo.Id);
            AccountCodeMapping.Delete(bo.Id);
        }

        public static Result Delete(AccountCodeMappingBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                AccountCodeMappingDetailService.DeleteByAccountCodeMappingId(bo.Id); // DO NOT TRAIL
                DataTrail dataTrail = AccountCodeMapping.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static Result ValidateMapping(AccountCodeMappingBo bo)
        {
            Result result = new Result();
            var list = new Dictionary<string, List<string>> { };

            if (bo.ReportType == AccountCodeMappingBo.ReportTypeIfrs4)
            {
                if (bo.Type == AccountCodeMappingBo.TypeClaimProvision || bo.Type == AccountCodeMappingBo.TypeClaimRecovery)
                {
                    if (string.IsNullOrEmpty(bo.TreatyType) && !bo.TreatyCodeId.HasValue && string.IsNullOrEmpty(bo.ClaimCode) && string.IsNullOrEmpty(bo.BusinessOrigin) && !bo.TransactionTypeCodePickListDetailId.HasValue)
                    {
                        if (IsDuplicateBalanceSheetForClaimProvisionAndRecoveryIfrs4(bo))
                        {
                            result.AddError("Existing Balance Sheet Found");
                        }
                        else
                        {
                            bo.IsBalanceSheet = true;
                        }
                        
                        return result;
                    }
                }
            }
            else if (bo.ReportType == AccountCodeMappingBo.ReportTypeIfrs17)
            {
                if (bo.Type == AccountCodeMappingBo.TypeClaimProvision || bo.Type == AccountCodeMappingBo.TypeClaimRecovery)
                {
                    if (IsDuplicateBalanceSheetForClaimProvisionAndRecoveryIfrs17(bo))
                    {
                        if (bo.IsBalanceSheet == true)
                            result.AddError("Existing Balance Sheet Found");
                        else
                            result.AddError("Existing Combination Found");
                    }
                    return result;
                }
                else if (bo.Type == AccountCodeMappingBo.TypeCedantAccountCode)
                {
                    if (string.IsNullOrEmpty(bo.InvoiceField))
                    {
                        if (IsDuplicateBalanceSheetForCedantAccountCodeIfrs17(bo))
                        {
                            if (bo.IsBalanceSheet == true)
                                result.AddError("Existing Balance Sheet Found");
                            else
                                result.AddError("Existing Combination Found");
                        }
                        return result;
                    }
                }

            }

            if (!AtLeast1InRetro(bo))
            {
                result.AddError("At least 1 field has to be filled");
                return result;
            }

            foreach (var detail in CreateDetails(bo))
            {
                var d = detail;
                TrimMaxLength(ref d, ref list);
                if (list.Count > 0)
                {
                    foreach (var prop in list)
                    {
                        result.AddError(string.Format("Exceeded Max Length: {0}", prop.Key));
                    }
                    break;
                }

                if (AccountCodeMappingDetailService.IsDuplicate(detail, bo))
                {
                    result.AddError("Existing Account Code Mapping Combination Found");
                    break;
                }
            }
            return result;
        }

        public static IList<AccountCodeMappingDetailBo> CreateDetails(AccountCodeMappingBo bo, int createdById = 0)
        {
            var details = new List<AccountCodeMappingDetailBo> { };
            CartesianProduct<string> ccountCodeMappings = new CartesianProduct<string>(
                bo.TreatyType.ToArraySplitTrim(),
                bo.ClaimCode.ToArraySplitTrim(),
                bo.BusinessOrigin.ToArraySplitTrim(),
                bo.InvoiceField.ToArraySplitTrim()
            );
            foreach (var item in ccountCodeMappings.Get())
            {
                var treatyType = item[0];
                var claimCode = item[1];
                var businessOrigin = item[2];
                var invoiceField = item[3];
                details.Add(new AccountCodeMappingDetailBo
                {
                    AccountCodeMappingId = bo.Id,
                    TreatyType = string.IsNullOrEmpty(treatyType) ? null : treatyType,
                    ClaimCode = string.IsNullOrEmpty(claimCode) ? null : claimCode,
                    BusinessOrigin = string.IsNullOrEmpty(businessOrigin) ? null : businessOrigin,
                    InvoiceField = string.IsNullOrEmpty(invoiceField) ? null : invoiceField,
                    CreatedById = createdById,
                });
            }
            return details;
        }

        public static void TrimMaxLength(ref AccountCodeMappingDetailBo detailBo, ref Dictionary<string, List<string>> list)
        {
            var entity = new AccountCodeMappingDetail();
            foreach (var property in (typeof(AccountCodeMappingDetailBo)).GetProperties())
            {
                var maxLengthAttr = entity.GetAttributeFrom<MaxLengthAttribute>(property.Name);
                if (maxLengthAttr != null)
                {
                    var value = property.GetValue(detailBo, null);
                    if (value != null && value is string @string && !string.IsNullOrEmpty(@string))
                    {
                        if (@string.Length > maxLengthAttr.Length)
                        {
                            string propName = string.Format("{0}({1})", property.Name, maxLengthAttr.Length);

                            if (!list.ContainsKey(propName))
                                list.Add(propName, new List<string> { });

                            var oldValue = @string;
                            var newValue = @string.Substring(0, maxLengthAttr.Length);
                            var formatValue = string.Format("{0}|{1}", oldValue, newValue);

                            if (!list[propName].Contains(formatValue))
                                list[propName].Add(formatValue);

                            property.SetValue(detailBo, newValue);
                        }
                    }
                }
            }
        }

        public static void ProcessMappingDetail(AccountCodeMappingBo bo, int authUserId)
        {
            AccountCodeMappingDetailService.DeleteByAccountCodeMappingId(bo.Id);

            if ((string.IsNullOrEmpty(bo.TreatyType) && string.IsNullOrEmpty(bo.ClaimCode)) && string.IsNullOrEmpty(bo.InvoiceField))
                return;

            foreach (var detail in CreateDetails(bo, authUserId))
            {
                var d = detail;
                AccountCodeMappingDetailService.Create(ref d);
            }
        }

        public static void ProcessMappingDetail(AccountCodeMappingBo bo, int authUserId, ref TrailObject trail)
        {
            AccountCodeMappingDetailService.DeleteByAccountCodeMappingId(bo.Id);

            if (string.IsNullOrEmpty(bo.TreatyType) && string.IsNullOrEmpty(bo.ClaimCode))
                return;

            foreach (var detail in CreateDetails(bo, authUserId))
            {
                var d = detail;
                AccountCodeMappingDetailService.Create(ref d, ref trail);
            }
        }
    }
}
