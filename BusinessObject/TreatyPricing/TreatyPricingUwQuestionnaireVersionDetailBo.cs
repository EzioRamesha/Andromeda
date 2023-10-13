using Shared.ProcessFile;
using System.Collections.Generic;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingUwQuestionnaireVersionDetailBo
    {
        public int Id { get; set; }

        public int TreatyPricingUwQuestionnaireVersionId { get; set; }

        public virtual TreatyPricingUwQuestionnaireVersionBo TreatyPricingUwQuestionnaireVersionBo { get; set; }

        public int UwQuestionnaireCategoryId { get; set; }

        public virtual UwQuestionnaireCategoryBo UwQuestionnaireCategoryBo { get; set; }

        public string Question { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string UwQuestionnaireCategoryCode { get; set; }

        public const int ColumnCode = 1;
        public const int ColumnQuestion = 2;

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "Category",
                    ColIndex = ColumnCode,
                },
                new Column
                {
                    Header = "Question",
                    ColIndex = ColumnQuestion,
                },
            };
        }
    }
}
