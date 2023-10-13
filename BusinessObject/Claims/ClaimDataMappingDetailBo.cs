using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Claims
{
    public class ClaimDataMappingDetailBo
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int ClaimDataMappingId { get; set; }

        [JsonIgnore]
        public ClaimDataMappingBo ClaimDataMappingBo { get; set; }

        [JsonIgnore]
        public int? PickListDetailId { get; set; }

        public string PickListDetailCode { get; set; }

        [JsonIgnore]
        public PickListDetailBo PickListDetailBo { get; set; }

        public string RawValue { get; set; }

        public bool IsPickDetailIdEmpty { get; set; } = false;

        public bool IsRawValueEmpty { get; set; } = false;

        [JsonIgnore]
        public int CreatedById { get; set; }

        [JsonIgnore]
        public int? UpdatedById { get; set; }

        public List<string> Validate(ref List<string> rawValues)
        {
            List<string> errors = new List<string> { };

            if (!IsPickDetailIdEmpty && PickListDetailId == null)
                errors.Add(string.Format(MessageBag.Required, "Value"));

            if (!IsRawValueEmpty && string.IsNullOrEmpty(RawValue))
                errors.Add(string.Format(MessageBag.Required, "Raw Value"));

            if (!string.IsNullOrEmpty(RawValue))
            {
                if (rawValues.Count > 0 && rawValues.Contains(RawValue))
                {
                    errors.Add(string.Format(MessageBag.DuplicateRawValue, RawValue));
                }
                rawValues.Add(RawValue);
            }

            return errors;
        }
    }
}
