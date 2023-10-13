using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class AnnuityFactorDetailBo
    {
        public int Id { get; set; }

        public int AnnuityFactorId { get; set; }

        public virtual AnnuityFactorBo AnnuityFactorBo { get; set; }

        public double? PolicyTermRemain { get; set; }

        public string PolicyTermRemainStr { get; set; }

        public int? InsuredGenderCodePickListDetailId { get; set; }

        public PickListDetailBo InsuredGenderCodePickListDetailBo { get; set; }

        public string InsuredGenderCode { get; set; }

        public int? InsuredTobaccoUsePickListDetailId { get; set; }

        public PickListDetailBo InsuredTobaccoUsePickListDetailBo { get; set; }

        public string InsuredTobaccoUse { get; set; }

        public int? InsuredAttainedAge { get; set; }

        public double? PolicyTerm { get; set; }

        public string PolicyTermStr { get; set; }

        public double AnnuityFactorValue { get; set; }

        public string AnnuityFactorValueStr { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int TypeAnnuityFactorDetailId = 1;
        public const int TypePolicyTermRemain = 2;
        public const int TypeInsuredGenderCode = 3;
        public const int TypeInsuredTobaccoUse = 4;
        public const int TypeInsuredAttainedAge = 5;
        public const int TypePolicyTerm = 6;
        public const int TypeAnnuityFactorValue = 7;

        public static List<Column> GetColumns()
        {
            List<Column> Cols = new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    ColIndex = TypeAnnuityFactorDetailId,
                    Property = "Id",
                },
                new Column
                {
                    Header = "Term Remain",
                    ColIndex = TypePolicyTermRemain,
                    Property = "PolicyTermRemain",
                },
                new Column
                {
                    Header = "Insured Gender Code",
                    ColIndex = TypeInsuredGenderCode,
                    Property = "InsuredGenderCode",
                },
                new Column
                {
                    Header = "Insured Tobacco Use",
                    ColIndex = TypeInsuredTobaccoUse,
                    Property = "InsuredTobaccoUse",
                },
                new Column
                {
                    Header = "Insured Attained Age",
                    ColIndex = TypeInsuredAttainedAge,
                    Property = "InsuredAttainedAge",
                },
                new Column
                {
                    Header = "Policy Term",
                    ColIndex = TypePolicyTerm,
                    Property = "PolicyTerm",
                },
                new Column
                {
                    Header = "Annuity Factor",
                    ColIndex = TypeAnnuityFactorValue,
                    Property = "AnnuityFactorValue",
                },
            };

            return Cols;
        }
    }
}
