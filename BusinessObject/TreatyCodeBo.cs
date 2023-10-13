using Shared;
using System;
using System.Collections.Generic;

namespace BusinessObject
{
    public class TreatyCodeBo
    {
        public int Id { get; set; }

        public int TreatyId { get; set; }        

        public string Code { get; set; }

        public int? OldTreatyCodeId { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public virtual TreatyBo TreatyBo { get; set; }

        // Phase 2
        public string AccountFor { get; set; }

        public int? TreatyTypePickListDetailId { get; set; }

        public int? TreatyStatusPickListDetailId { get; set; }

        public PickListDetailBo TreatyTypePickListDetailBo { get; set; }

        public PickListDetailBo TreatyStatusPickListDetailBo { get; set; }

        public string TreatyNo { get; set; }

        public int? LineOfBusinessPickListDetailId { get; set; }

        public PickListDetailBo LineOfBusinessPickListDetailBo { get; set; }


        // For Selection purpose
        public string TreatyTypeCode { get; set; }
        public string TreatyType { get; set; }

        public string CedingCompanyType { get; set; }

        public string CedingCompany { get; set; }

        public string CreatedAtStr { get; set; }

        public const int StatusActive = 1;
        public const int StatusInactive = 2;

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

        public string ToString(bool withDescription = true)
        {
            if (string.IsNullOrEmpty(Description) || !withDescription)
            {
                return Code;
            }
            return string.Format("{0} - {1}", Code, Description);
        }

        public List<string> Validate()
        {
            List<string> errors = new List<string> { };

            if (string.IsNullOrEmpty(Code))
                errors.Add(string.Format(MessageBag.Required, "Treaty Code"));

            int maxLengthCode = 35;
            if (!string.IsNullOrEmpty(Code) && Code.Length > maxLengthCode)
                errors.Add(string.Format(MessageBag.MaxLength, "Treaty Code", maxLengthCode));

            int maxLengthDesc = 128;
            if (!string.IsNullOrEmpty(Description) && Description.Length > maxLengthDesc)
                errors.Add(string.Format(MessageBag.MaxLength, "Description", maxLengthDesc));

            if (Status == 0)
                errors.Add(string.Format(MessageBag.Required, "Status"));

            if (!string.IsNullOrEmpty(TreatyNo) && TreatyNo.Length > maxLengthDesc)
                errors.Add(string.Format(MessageBag.MaxLength, "Treaty Number", maxLengthDesc));

            return errors;
        }
    }
}
