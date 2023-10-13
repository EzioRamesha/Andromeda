using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class RetroPartyBo
    {
        public int Id { get; set; }

        public string Party { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int CountryCodePickListDetailId { get; set; }

        public PickListDetailBo CountryCodePickListDetailBo { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public bool IsDirectRetro { get; set; }

        public bool IsPerLifeRetro { get; set; }

        public string AccountCode { get; set; }

        public string AccountCodeDescription { get; set; }

        public const int StatusActive = 1;
        public const int StatusInactive = 2;
        public const int MaxStatus = 2;

        public const string StatusActiveName = "Active";
        public const string StatusInactiveName = "Inactive";

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusActive:
                    return StatusActiveName;
                case StatusInactive:
                    return StatusInactiveName;
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
            if (string.IsNullOrEmpty(Name))
            {
                return Party;
            }
            return string.Format("{0} - {1}", Party, Name);
        }
    }
}
