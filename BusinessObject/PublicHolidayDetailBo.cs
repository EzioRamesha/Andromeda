using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class PublicHolidayDetailBo
    {
        public int Id { get; set; }

        public int PublicHolidayId { get; set; }

        public PublicHolidayBo PublicHolidayBo { get; set; }

        public DateTime PublicHolidayDate { get; set; }

        public string Description { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public string PublicHolidayDateStr { get; set; }

        public List<string> Validate()
        {
            List<string> errors = new List<string> { };

            if (string.IsNullOrEmpty(PublicHolidayDateStr) || string.IsNullOrWhiteSpace(PublicHolidayDateStr))
                errors.Add(string.Format(MessageBag.Required, "Start Date"));
            else if (!Util.TryParseDateTime(PublicHolidayDateStr, out DateTime? datetime, out string error))
                errors.Add(string.Format(MessageBag.InvalidDateFormatWithValue, PublicHolidayDateStr));

            if (string.IsNullOrEmpty(Description) || string.IsNullOrWhiteSpace(Description))
                errors.Add(string.Format(MessageBag.Required, "Description"));

            return errors;
        }
    }
}
