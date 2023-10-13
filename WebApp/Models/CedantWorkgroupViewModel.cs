using BusinessObject;
using DataAccess.Entities;
using Services;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class CedantWorkgroupViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [DisplayName("Code")]
        public string Code { get; set; }

        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }

        public int ModuleId { get; set; }

        private IList<CedantWorkgroupCedantBo> CedantWorkgroupCedantBos { get; set; }

        private IList<CedantWorkgroupUserBo> CedantWorkgroupUserBos { get; set; }

        public CedantWorkgroupViewModel()
        {
            Set();
        }

        public CedantWorkgroupViewModel(CedantWorkgroupBo cedantWorkgroupBo)
        {
            Set(cedantWorkgroupBo);
        }

        public void Set(CedantWorkgroupBo cedantWorkgroupBo = null)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.CedantWorkgroup.ToString());
            ModuleId = moduleBo.Id;
            if (cedantWorkgroupBo != null)
            {
                Id = cedantWorkgroupBo.Id;
                Code = cedantWorkgroupBo.Code;
                Description = cedantWorkgroupBo.Description;
            }
        }

        public void Get(ref CedantWorkgroupBo cedantWorkgroupBo)
        {
            cedantWorkgroupBo.Id = Id;
            cedantWorkgroupBo.Code = Code?.Trim();
            cedantWorkgroupBo.Description = Description?.Trim();
        }

        public static Expression<Func<CedantWorkgroup, CedantWorkgroupViewModel>> Expression()
        {
            return entity => new CedantWorkgroupViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                Description = entity.Description,
            };
        }

        public void GetCedants(FormCollection form)
        {
            IList<CedantWorkgroupCedantBo> cedantWorkgroupCedantBos = new List<CedantWorkgroupCedantBo>();

            int maxIndex = int.Parse(form.Get("Cedant.MaxIndex"));
            int index = 0;
            while (index <= maxIndex)
            {
                int cedantId = int.Parse(form.Get(string.Format("Cedant.CedantId[{0}]", index)));
                string cedantName = form.Get(string.Format("Cedant.CedantName[{0}]", index));
                cedantWorkgroupCedantBos.Add(new CedantWorkgroupCedantBo
                {
                    CedantWorkgroupId = Id,
                    CedantId = cedantId,
                    CedantName = cedantName,
                });

                index++;
            }

            CedantWorkgroupCedantBos = cedantWorkgroupCedantBos;
        }

        public void GetUsers(FormCollection form)
        {
            IList<CedantWorkgroupUserBo> cedantWorkgroupUserBos = new List<CedantWorkgroupUserBo>();

            int maxIndex = int.Parse(form.Get("User.MaxIndex"));
            int index = 0;
            while (index <= maxIndex)
            {
                int userId = int.Parse(form.Get(string.Format("User.UserId[{0}]", index)));
                string userName = form.Get(string.Format("User.UserName[{0}]", index));
                cedantWorkgroupUserBos.Add(new CedantWorkgroupUserBo
                {
                    CedantWorkgroupId = Id,
                    UserId = userId,
                    UserName = userName,
                });

                index++;
            }

            CedantWorkgroupUserBos = cedantWorkgroupUserBos;
        }

        public void ProcessCedants(ref TrailObject trail)
        {
            List<int> latestCedantIds = new List<int>();
            foreach(CedantWorkgroupCedantBo cedantWorkgroupCedantBo in CedantWorkgroupCedantBos)
            {
                latestCedantIds.Add(cedantWorkgroupCedantBo.CedantId);
                if (CedantWorkgroupCedantService.IsExists(Id, cedantWorkgroupCedantBo.CedantId))
                    continue;

                cedantWorkgroupCedantBo.CedantWorkgroupId = Id;
                CedantWorkgroupCedantService.Create(cedantWorkgroupCedantBo, ref trail);
            }
            CedantWorkgroupCedantService.DeleteByCedantWorkgroupIdExcept(Id, latestCedantIds, ref trail);
        }
        
        public void ProcessUsers(ref TrailObject trail)
        {
            List<int> latestUserIds = new List<int>();
            foreach(CedantWorkgroupUserBo cedantWorkgroupUserBo in CedantWorkgroupUserBos)
            {
                latestUserIds.Add(cedantWorkgroupUserBo.UserId);
                if (CedantWorkgroupUserService.IsExists(Id, cedantWorkgroupUserBo.UserId))
                    continue;

                cedantWorkgroupUserBo.CedantWorkgroupId = Id;
                CedantWorkgroupUserService.Create(cedantWorkgroupUserBo, ref trail);
            }
            CedantWorkgroupUserService.DeleteByCedantWorkgroupIdExcept(Id, latestUserIds, ref trail);
        }

        public object GetChildBos(string name)
        {
            return this.GetPropertyValue(string.Format("{0}Bos", name), BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}