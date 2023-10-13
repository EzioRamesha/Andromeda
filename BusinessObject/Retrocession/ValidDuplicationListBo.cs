using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.Retrocession
{
    public class ValidDuplicationListBo
    {
        public int Id { get; set; }

        public string TreatyCode { get; set; }
        public int? TreatyCodeId { get; set; }
        public TreatyCodeBo TreatyCodeBo { get; set; }

        public string CedantPlanCode { get; set; }

        public string InsuredName { get; set; }

        public DateTime? InsuredDateOfBirth { get; set; }
        public string InsuredDateOfBirthStr { get; set; }

        public string PolicyNumber { get; set; }

        public string InsuredGenderCodePickList { get; set; }
        public int? InsuredGenderCodePickListDetailId { get; set; }
        public PickListDetailBo InsuredGenderCodePickListDetailBo { get; set; }

        public string MLReBenefitCode { get; set; }
        public int? MLReBenefitCodeId { get; set; }
        public BenefitBo MLReBenefitCodeBo { get; set; }

        public string CedingBenefitRiskCode { get; set; }

        public string CedingBenefitTypeCode { get; set; }

        public DateTime? ReinsuranceEffectiveDate { get; set; }
        public string ReinsuranceEffectiveDateStr { get; set; }

        public string FundsAccountingTypePickList { get; set; }
        public int? FundsAccountingTypePickListDetailId { get; set; }
        public PickListDetailBo FundsAccountingTypePickListDetailBo { get; set; }

        public string ReinsBasisCodePickList { get; set; }
        public int? ReinsBasisCodePickListDetailId { get; set; }
        public PickListDetailBo ReinsBasisCodePickListDetailBo { get; set; }

        public string TransactionTypePickList { get; set; }
        public int? TransactionTypePickListDetailId { get; set; }
        public PickListDetailBo TransactionTypePickListDetailBo { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }


        public const int ColumnId = 1;
        public const int ColumnTreatyCode = 2;
        public const int ColumnCedantPlanCode = 3;
        public const int ColumnInsuredName = 4;
        public const int ColumnInsuredDateOfBirth = 5;
        public const int ColumnPolicyNumber = 6;
        public const int ColumnInsuredGenderCodePickList = 7;
        public const int ColumnMLReBenefitCode = 8;
        public const int ColumnCedingBenefitRiskCode = 9;
        public const int ColumnCedingBenefitTypeCode = 10;
        public const int ColumnReinsuranceEffectiveDate = 11;
        public const int ColumnFundsAccountingTypePickList = 12;
        public const int ColumnReinsBasisCodePickList = 13;
        public const int ColumnTransactionTypePickList = 14;

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    ColIndex = ColumnId,
                    Property = "Id",
                },
                new Column
                {
                    Header = "Treaty Code",
                    ColIndex = ColumnTreatyCode,
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "Cedant Plan Code",
                    ColIndex = ColumnCedantPlanCode,
                    Property = "CedantPlanCode",
                },
                new Column
                {
                    Header = "Insured Name",
                    ColIndex = ColumnInsuredName,
                    Property = "InsuredName",
                },
                new Column
                {
                    Header = "Insured Date of Birth",
                    ColIndex = ColumnInsuredDateOfBirth,
                    Property = "InsuredDateOfBirth",
                },
                new Column
                {
                    Header = "Policy Number",
                    ColIndex = ColumnPolicyNumber,
                    Property = "PolicyNumber",
                },
                new Column
                {
                    Header = "Insured Gender Code",
                    ColIndex = ColumnInsuredGenderCodePickList,
                    Property = "InsuredGenderCodePickList",
                },
                new Column
                {
                    Header = "MLRe Benefit Code",
                    ColIndex = ColumnMLReBenefitCode,
                    Property = "MLReBenefitCode",
                },
                new Column
                {
                    Header = "Ceding Benefit Risk Code",
                    ColIndex = ColumnCedingBenefitRiskCode,
                    Property = "CedingBenefitRiskCode",
                },
                new Column
                {
                    Header = "Ceding Benefit Type Code",
                    ColIndex = ColumnCedingBenefitTypeCode,
                    Property = "CedingBenefitTypeCode",
                },
                new Column
                {
                    Header = "Reinsurance Effective Date",
                    ColIndex = ColumnReinsuranceEffectiveDate,
                    Property = "ReinsuranceEffectiveDate",
                },
                new Column
                {
                    Header = "FDS Accounting Type",
                    ColIndex = ColumnFundsAccountingTypePickList,
                    Property = "FundsAccountingTypePickList",
                },
                new Column
                {
                    Header = "Reinsurance Risk Basis",
                    ColIndex = ColumnReinsBasisCodePickList,
                    Property = "ReinsBasisCodePickList",
                },
                new Column
                {
                    Header = "Transaction Type",
                    ColIndex = ColumnTransactionTypePickList,
                    Property = "TransactionTypePickList",
                },
            };
        }
    }
}
