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
    public class DirectRetroProvisioningTransactionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(DirectRetroProvisioningTransaction)),
                Controller = ModuleBo.ModuleController.DirectRetroProvisioningTransaction.ToString(),
            };
        }

        public static DirectRetroProvisioningTransactionBo FormBo(DirectRetroProvisioningTransaction entity = null, int? precision = null)
        {
            if (entity == null)
                return null;

            return new DirectRetroProvisioningTransactionBo
            {
                Id = entity.Id,
                ClaimRegisterId = entity.ClaimRegisterId,
                ClaimRegisterBo = ClaimRegisterService.Find(entity.ClaimRegisterId),
                FinanceProvisioningId = entity.FinanceProvisioningId,
                FinanceProvisioningStatus = FinanceProvisioningService.Find(entity.FinanceProvisioningId).Status,
                IsLatestProvision = entity.IsLatestProvision,
                ClaimId = entity.ClaimId,
                CedingCompany = entity.CedingCompany,
                EntryNo = entity.EntryNo,
                Quarter = entity.Quarter,
                RetroParty = entity.RetroParty,
                RetroRecovery = entity.RetroRecovery,
                RetroRecoveryStr = Util.DoubleToString(entity.RetroRecovery, precision),
                RetroStatementId = entity.RetroStatementId,
                RetroStatementDate = entity.RetroStatementDate,
                RetroStatementDateStr = entity.RetroStatementDate?.ToString(Util.GetDateFormat()),

                TreatyCode = entity.TreatyCode,
                TreatyType = entity.TreatyType,
                ClaimCode = entity.ClaimCode,

                CreatedAt = entity.CreatedAt,
                CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateFormat()),
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static DirectRetroProvisioningTransaction FormEntity(DirectRetroProvisioningTransactionBo bo = null)
        {
            if (bo == null)
                return null;
            return new DirectRetroProvisioningTransaction
            {
                Id = bo.Id,
                ClaimRegisterId = bo.ClaimRegisterId,
                FinanceProvisioningId = bo.FinanceProvisioningId,
                IsLatestProvision = bo.IsLatestProvision,
                ClaimId = bo.ClaimId,
                CedingCompany = bo.CedingCompany,
                EntryNo = bo.EntryNo,
                Quarter = bo.Quarter,
                RetroParty = bo.RetroParty,
                RetroRecovery = bo.RetroRecovery,
                RetroStatementId = bo.RetroStatementId,
                RetroStatementDate = bo.RetroStatementDate,

                TreatyCode = bo.TreatyCode,
                TreatyType = bo.TreatyType,
                ClaimCode = bo.ClaimCode,

                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static IList<DirectRetroProvisioningTransactionBo> FormBos(IList<DirectRetroProvisioningTransaction> entities = null, int? precision = null)
        {
            if (entities == null)
                return null;
            IList<DirectRetroProvisioningTransactionBo> bos = new List<DirectRetroProvisioningTransactionBo>() { };
            foreach (DirectRetroProvisioningTransaction entity in entities)
            {
                bos.Add(FormBo(entity, precision));
            }

            return bos;
        }

        public static DirectRetroProvisioningTransactionBo Find(int id)
        {
            return FormBo(DirectRetroProvisioningTransaction.Find(id));
        }

        public static IList<DirectRetroProvisioningTransactionBo> GetByClaimRegisterId(int claimRegisterId, bool latestProvisionOnly = true, int? precision = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.DirectRetroProvisioningTransactions.Where(q => q.ClaimRegisterId == claimRegisterId);

                if (latestProvisionOnly)
                    query = query.Where(q => q.IsLatestProvision == true);

                return FormBos(query.ToList(), precision);
            }
        }

        public static IList<DirectRetroProvisioningTransactionBo> GetByClaimRegisterIdByFinanceProvisioningId(int claimRegisterId, int financeProvisioningId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.DirectRetroProvisioningTransactions
                    .Where(q => q.ClaimRegisterId == claimRegisterId)
                    .Where(q => q.FinanceProvisioningId == financeProvisioningId)
                    .ToList());
            }
        }

        public static List<int> GetClaimRegisterIdByFinanceProvisioningId(int financeProvisioningId)
        {
            using (var db = new AppDbContext())
            {
                return db.DirectRetroProvisioningTransactions
                    .Where(q => q.FinanceProvisioningId == financeProvisioningId)
                    .GroupBy(q => q.ClaimRegisterId)
                    .Select(q => q.FirstOrDefault())
                    .Select(q => q.ClaimRegisterId)
                    .ToList();
            }
        }

        public static Result Create(ref DirectRetroProvisioningTransactionBo bo)
        {
            DirectRetroProvisioningTransaction entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref DirectRetroProvisioningTransactionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref DirectRetroProvisioningTransactionBo bo)
        {
            Result result = Result();

            DirectRetroProvisioningTransaction entity = DirectRetroProvisioningTransaction.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity = FormEntity(bo);
                result.DataTrail = entity.Update();
            }

            return result;
        }

        public static Result Update(ref DirectRetroProvisioningTransactionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }
    }
}
