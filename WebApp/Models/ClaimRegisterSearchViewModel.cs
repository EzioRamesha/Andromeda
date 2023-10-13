using DataAccess.Entities;
using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class ClaimRegisterSearchViewModel
    {
        public int Id { get; set; }

        public int ClaimRegisterId { get; set; }

        public int? ClaimRegisterHistoryId { get; set; }

        public bool IsSnapShotVersion { get; set; }

        public int? CutOffId { get; set; }

        public int SortIndex { get; set; }

        [DisplayName("Claim ID")]
        public string ClaimId { get; set; }

        public string InsuredName { get; set; }

        public DateTime? InsuredDateOfBirth { get; set; }

        [DisplayName("Policy Number")]
        public string PolicyNumber { get; set; }

        [DisplayName("Treaty Code")]
        public string TreatyCode { get; set; }

        public bool WriteHeader { get; set; }

        public bool IsWithAdjustmentDetail { get; set; }

        public string EntryNo { get; set; }

        public string SoaQuarter { get; set; }

        public string ClaimTransactionType { get; set; }

        public double? ClaimRecoveryAmount { get; set; }

        public bool IsReferralCase { get; set; }

        [DisplayName("RI Data")]
        public int? RiDataWarehouseId { get; set; }

        public string RecordType { get; set; }

        public string CedingCompany { get; set; }

        public bool HasRedFlag { get; set; }

        public int FinanceProvisioningStatus { get; set; }

        public DateTime? ProvisionAt { get; set; }

        public DateTime? CutOffAt { get; set; }

        public static Expression<Func<ClaimRegister, ClaimRegisterSearchViewModel>> Expression()
        {
            return entity => new ClaimRegisterSearchViewModel
            {
                Id = entity.Id,
                ClaimRegisterId = entity.Id,
                SortIndex = 0,
                ClaimId = entity.ClaimId,
                InsuredName = entity.InsuredName,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                PolicyNumber = entity.PolicyNumber,
                TreatyCode = entity.TreatyCode,
                EntryNo = entity.EntryNo,
                SoaQuarter = entity.SoaQuarter,
                ClaimTransactionType = entity.ClaimTransactionType,
                ClaimRecoveryAmount = entity.ClaimRecoveryAmt,
                IsReferralCase = entity.IsReferralCase,
                RiDataWarehouseId = entity.RiDataWarehouseId,
                RecordType = entity.RecordType,
                CedingCompany = entity.CedingCompany,
                HasRedFlag = entity.HasRedFlag,
            };
        }

        public static Expression<Func<FinanceProvisioningTransaction, ClaimRegisterSearchViewModel>> AdjustmentExpression()
        {
            return entity => new ClaimRegisterSearchViewModel
            {
                Id = entity.Id,
                ClaimRegisterId = entity.ClaimRegisterId,
                SortIndex = entity.SortIndex,
                ClaimId = entity.ClaimId,
                InsuredName = entity.ClaimRegister.InsuredName,
                InsuredDateOfBirth = entity.ClaimRegister.InsuredDateOfBirth,
                PolicyNumber = entity.PolicyNumber,
                TreatyCode = entity.TreatyCode,
                EntryNo = entity.SortIndex == 0 ? entity.EntryNo : entity.EntryNo + " - " + entity.SortIndex,
                SoaQuarter = entity.Quarter,
                ClaimTransactionType = entity.SortIndex == 0 ? entity.ClaimRegister.ClaimTransactionType : "ADJ",
                ClaimRecoveryAmount = entity.ClaimRecoveryAmount,
                IsReferralCase = entity.ClaimRegister.IsReferralCase,
                RiDataWarehouseId = entity.ClaimRegister.RiDataWarehouseId,
                RecordType = entity.ClaimRegister.RecordType,
                CedingCompany = entity.CedingCompany,
                HasRedFlag = entity.ClaimRegister.HasRedFlag,
                ProvisionAt = entity.FinanceProvisioning.ProvisionAt ?? DateTime.Today,
            };
        }

        public static Expression<Func<ClaimRegisterHistoryTransaction, ClaimRegisterSearchViewModel>> HistoryAdjustmentExpression()
        {
            return entity => new ClaimRegisterSearchViewModel
            {
                Id = entity.Transaction.Id,
                ClaimRegisterId = entity.Transaction.ClaimRegisterId,
                ClaimRegisterHistoryId = entity.History.Id,
                SortIndex = entity.Transaction.SortIndex,
                CutOffId = entity.History.CutOffId,
                ClaimId = entity.Transaction.ClaimId,
                InsuredName = entity.History.InsuredName,
                InsuredDateOfBirth = entity.History.InsuredDateOfBirth,
                PolicyNumber = entity.Transaction.PolicyNumber,
                TreatyCode = entity.Transaction.TreatyCode,
                EntryNo = entity.Transaction.SortIndex == 0 ? entity.Transaction.EntryNo : entity.Transaction.EntryNo + " - " + entity.Transaction.SortIndex,
                SoaQuarter = entity.Transaction.Quarter,
                ClaimTransactionType = entity.Transaction.SortIndex == 0 ? entity.History.ClaimTransactionType : "ADJ",
                ClaimRecoveryAmount = entity.Transaction.ClaimRecoveryAmount,
                IsReferralCase = entity.History.IsReferralCase,
                RiDataWarehouseId = entity.History.RiDataWarehouseId,
                RecordType = entity.History.RecordType,
                CedingCompany = entity.Transaction.CedingCompany,
                HasRedFlag = entity.History.HasRedFlag,
                FinanceProvisioningStatus = entity.Transaction.FinanceProvisioning.Status,
                ProvisionAt = entity.Transaction.FinanceProvisioning.ProvisionAt,
                CutOffAt = entity.History.CutOff.CutOffDateTime
            };
        }

        public static Expression<Func<ClaimRegisterHistory, ClaimRegisterSearchViewModel>> HistoryExpression()
        {
            return entity => new ClaimRegisterSearchViewModel
            {
                Id = entity.Id,
                ClaimRegisterId = entity.ClaimRegisterId,
                ClaimRegisterHistoryId = entity.Id,
                SortIndex = 0,
                CutOffId = entity.CutOffId,
                ClaimId = entity.ClaimId,
                InsuredName = entity.InsuredName,
                InsuredDateOfBirth = entity.InsuredDateOfBirth,
                PolicyNumber = entity.PolicyNumber,
                TreatyCode = entity.TreatyCode,
                EntryNo = entity.EntryNo,
                SoaQuarter = entity.SoaQuarter,
                ClaimTransactionType = entity.ClaimTransactionType,
                IsReferralCase = entity.IsReferralCase,
                RiDataWarehouseId = entity.RiDataWarehouseId,
                RecordType = entity.RecordType,
                CedingCompany = entity.CedingCompany,
                HasRedFlag = entity.HasRedFlag,
            };
        }
    }
}