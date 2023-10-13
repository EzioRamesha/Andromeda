using Shared;
using System;

namespace BusinessObject.Retrocession
{
    public class PerLifeRetroGenderBo
    {
        public int Id { get; set; }

        public int? InsuredGenderCodePickListDetailId { get; set; }

        public PickListDetailBo InsuredGenderCodePickListDetailBo { get; set; }

        public string InsuredGenderCode { get; set; }

        public string Gender { get; set; }

        public DateTime EffectiveStartDate { get; set; }

        public DateTime EffectiveEndDate { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public override string ToString()
        {
            if (InsuredGenderCodePickListDetailBo != null)
                return InsuredGenderCodePickListDetailBo.ToString();

            return null;
        }
    }
}
