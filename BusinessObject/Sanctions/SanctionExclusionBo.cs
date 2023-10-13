using System;

namespace BusinessObject.Sanctions
{
    public class SanctionExclusionBo
    {
        public int Id { get; set; }

        public string Keyword { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }
    }
}
