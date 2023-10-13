using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.RiDatas
{
    public class RiDataCorrectionBo
    {
        public int Id { get; set; }

        public int CedantId { get; set; }

        public CedantBo CedantBo { get; set; }

        public string CedantCode { get; set; }

        public int? TreatyCodeId { get; set; }

        public TreatyCodeBo TreatyCodeBo { get; set; }

        public string TreatyCode { get; set; }

        public string PolicyNumber { get; set; }

        public string InsuredRegisterNo { get; set; }

        public string InsuredGenderCode { get; set; }

        public int? InsuredGenderCodePickListDetailId { get; set; }

        public PickListDetailBo InsuredGenderCodePickListDetailBo { get; set; }

        public DateTime? InsuredDateOfBirth { get; set; }

        public string InsuredName { get; set; }

        public string CampaignCode { get; set; }

        public int? ReinsBasisCodePickListDetailId { get; set; }

        public PickListDetailBo ReinsBasisCodePickListDetailBo { get; set; }

        public string ReinsBasisCode { get; set; }

        public double? ApLoading { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int ColumnId = 1;
        public const int ColumnCedantCode = 2;
        public const int ColumnTreatyCode = 3;
        public const int ColumnPolicyNumber = 4;
        public const int ColumnInsuredRegisterNo = 5;
        public const int ColumnInsuredGenderCode = 6;
        public const int ColumnInsuredDateOfBirth = 7;
        public const int ColumnInsuredName = 8;
        public const int ColumnCampaignCode = 9;
        public const int ColumnReinsBasisCode = 10;
        public const int ColumnApLoading = 11;
        public const int ColumnAction = 12;

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
                    Header = "Ceding Company",
                    ColIndex = ColumnCedantCode,
                    Property = "CedantCode",
                },
                new Column
                {
                    Header = "Treaty Code",
                    ColIndex = ColumnTreatyCode,
                    Property = "TreatyCode",
                },
                new Column
                {
                    Header = "Policy No.",
                    ColIndex = ColumnPolicyNumber,
                    Property = "PolicyNumber",
                },
                new Column
                {
                    Header = "Insured Reg. No.",
                    ColIndex = ColumnInsuredRegisterNo,
                    Property = "InsuredRegisterNo",
                },
                new Column
                {
                    Header = "Insured Gender Code",
                    ColIndex = ColumnInsuredGenderCode,
                    Property = "InsuredGenderCode",
                },
                new Column
                {
                    Header = "Insured Date Of Birth",
                    ColIndex = ColumnInsuredDateOfBirth,
                    Property = "InsuredDateOfBirth",
                },
                new Column
                {
                    Header = "Insured Name",
                    ColIndex = ColumnInsuredName,
                    Property = "InsuredName",
                },
                new Column
                {
                    Header = "Campaign Code",
                    ColIndex = ColumnCampaignCode,
                    Property = "CampaignCode",
                },
                new Column
                {
                    Header = "Reins Basis Code",
                    ColIndex = ColumnReinsBasisCode,
                    Property = "ReinsBasisCode",
                },
                new Column
                {
                    Header = "AP Loading",
                    ColIndex = ColumnApLoading,
                    Property = "ApLoading",
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
