using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class ClaimCodeBo
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

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

        public static int GetStatusKey(string name)
        {
            switch (name)
            {
                case StatusActiveName:
                    return StatusActive;
                case StatusInactiveName:
                    return StatusInactive;
                default:
                    return 0;
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

        public static List<string> GetMedicalClaimCodes()
        {
            return new List<string>
            {
                "HTH",
                "HCB"
            };
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
