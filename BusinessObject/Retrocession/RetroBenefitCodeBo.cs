using System;

namespace BusinessObject.Retrocession
{
    public class RetroBenefitCodeBo
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime CeaseDate { get; set; }

        public int Status { get; set; }

        public string Remarks { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int StatusActive = 1;
        public const int StatusInactive = 2;

        public const int StatusMax = 2;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusActive:
                    return "Active";
                case StatusInactive:
                    return "Inactive";
                default:
                    return "";
            }
        }

        public static string GetStatusClass(int key)
        {
            switch (key)
            {
                case StatusActive:
                    return "status-success-badge";
                case StatusInactive:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Description))
            {
                return Code;
            }
            return string.Format("{0} - {1}", Code, Description);
        }
    }
}
