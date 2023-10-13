using System;

namespace BusinessObject.TreatyPricing
{
    public class UwQuestionnaireCategoryBo
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }
    }
}
