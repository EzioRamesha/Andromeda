using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.Retrocession
{
    public class PerLifeDataCorrectionBo
    {
        public int Id { get; set; }

        public int TreatyCodeId { get; set; }

        public TreatyCodeBo TreatyCodeBo { get; set; }

        public string TreatyCode { get; set; }

        public string InsuredName { get; set; }

        public DateTime InsuredDateOfBirth { get; set; }

        public string PolicyNumber { get; set; }

        public int InsuredGenderCodePickListDetailId { get; set; }

        public PickListDetailBo InsuredGenderCodePickListDetailBo { get; set; }

        public string InsuredGenderCode { get; set; }

        public int TerritoryOfIssueCodePickListDetailId { get; set; }

        public PickListDetailBo TerritoryOfIssueCodePickListDetailBo { get; set; }

        public string TerritoryOfIssueCode { get; set; }

        public int PerLifeRetroGenderId { get; set; }

        public PerLifeRetroGenderBo PerLifeRetroGenderBo { get; set; }

        public string PerLifeRetroGenderStr { get; set; }

        public int PerLifeRetroCountryId { get; set; }

        public PerLifeRetroCountryBo PerLifeRetroCountryBo { get; set; }

        public string PerLifeRetroCountryStr { get; set; }

        public DateTime DateOfPolicyExist { get; set; }

        public bool IsProceedToAggregate { get; set; }

        public DateTime DateOfExceptionDetected { get; set; }

        public int ExceptionStatusPickListDetailId { get; set; }

        public PickListDetailBo ExceptionStatusPickListDetailBo { get; set; }

        public string ExceptionStatus { get; set; }

        public string Remark { get; set; }

        public DateTime DateUpdated { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int ColumnId = 1;
        public const int ColumnTreatyCode = 2;
        public const int ColumnInsuredName = 3;
        public const int ColumnInsuredDateOfBirth = 4;
        public const int ColumnPolicyNumber = 5;
        public const int ColumnInsuredGenderCode = 6;
        public const int ColumnTerritoryOfIssueCode = 7;
        public const int ColumnPerLifeRetroGender = 8;
        public const int ColumnPerLifeRetroCountry = 9;
        public const int ColumnDateOfExceptionDetected = 10;
        public const int ColumnDateOfPolicyExist = 11;
        public const int ColumnIsProceedToAggregate = 12;
        public const int ColumnDateUpdated = 13;
        public const int ColumnExceptionStatus = 14;
        public const int ColumnRemark = 15;
        public const int ColumnAction = 16;

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
                    Header = "Org Gender Code",
                    ColIndex = ColumnInsuredGenderCode,
                    Property = "InsuredGenderCode",
                },
                new Column
                {
                    Header = "Org Territory of Issue ID",
                    ColIndex = ColumnTerritoryOfIssueCode,
                    Property = "TerritoryOfIssueCode",
                },
                new Column
                {
                    Header = "Expected Gender Code",
                    ColIndex = ColumnPerLifeRetroGender,
                    Property = "PerLifeRetroGenderStr",
                },
                new Column
                {
                    Header = "Expected Territory of Issue ID",
                    ColIndex = ColumnPerLifeRetroCountry,
                    Property = "PerLifeRetroCountryStr",
                },
                new Column
                {
                    Header = "Date of Exception Detected",
                    ColIndex = ColumnDateOfExceptionDetected,
                    Property = "DateOfExceptionDetected",
                },
                new Column
                {
                    Header = "1st Date of The Policy Exist In The System",
                    ColIndex = ColumnDateOfPolicyExist,
                    Property = "DateOfPolicyExist",
                },
                new Column
                {
                    Header = "Proceed to Aggregate",
                    ColIndex = ColumnIsProceedToAggregate,
                    Property = "IsProceedToAggregate",
                },
                new Column
                {
                    Header = "Date Updated",
                    ColIndex = ColumnDateUpdated,
                    Property = "DateUpdated",
                },
                new Column
                {
                    Header = "Exception Status",
                    ColIndex = ColumnExceptionStatus,
                    Property = "ExceptionStatus",
                },
                new Column
                {
                    Header = "Remark",
                    ColIndex = ColumnRemark,
                    Property = "Remark",
                },
                new Column
                {
                    Header = "Action",
                    ColIndex = ColumnAction,
                },
            };
        }
    }
}
