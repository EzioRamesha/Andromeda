using BusinessObject.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class CedantWorkgroupBo
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public readonly List<string> RestrictedControllers = new List<string>
        {
            ModuleBo.ModuleController.RiData.ToString(),
            ModuleBo.ModuleController.RiDataConfig.ToString(),
            ModuleBo.ModuleController.ClaimData.ToString(),
            ModuleBo.ModuleController.ClaimDataConfig.ToString(),
        };
    }
}
