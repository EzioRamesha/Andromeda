using Newtonsoft.Json;
using Shared;
using System.Collections.Generic;

namespace BusinessObject.RiDatas
{
    public class RiDataMappingDetailBo
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int RiDataMappingId { get; set; }

        [JsonIgnore]
        public RiDataMappingBo RiDataMappingBo { get; set; }

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

        public List<string> Validate()
        {
            List<string> errors = new List<string> { };

            if (!IsPickDetailIdEmpty && PickListDetailId == null)
                errors.Add(string.Format(MessageBag.Required, "Value"));

            if (!IsRawValueEmpty && string.IsNullOrEmpty(RawValue))
                errors.Add(string.Format(MessageBag.Required, "Raw Value"));

            return errors;
        }
    }
}
