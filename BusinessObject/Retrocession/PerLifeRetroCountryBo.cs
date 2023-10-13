using System;

namespace BusinessObject.Retrocession
{
    public class PerLifeRetroCountryBo
    {
        public int Id { get; set; }

        public int? TerritoryOfIssueCodePickListDetailId { get; set; }

        public PickListDetailBo TerritoryOfIssueCodePickListDetailBo { get; set; }

        public string TerritoryOfIssueCode { get; set; }

        public string Country { get; set; }

        public DateTime EffectiveStartDate { get; set; }

        public DateTime EffectiveEndDate { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public override string ToString()
        {
            if (TerritoryOfIssueCodePickListDetailBo != null)
                return TerritoryOfIssueCodePickListDetailBo.ToString();

            return null;
        }
    }
}
