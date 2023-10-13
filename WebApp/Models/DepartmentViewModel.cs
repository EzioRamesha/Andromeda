using BusinessObject.Identity;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using Services.Identity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Code { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [DisplayName("HOD User")]
        public int? HodUserId { get; set; }

        public virtual User HodUser { get; set; }

        public DepartmentViewModel() { }

        public DepartmentViewModel(DepartmentBo departmentBo)
        {
            if (departmentBo != null)
            {
                Id = departmentBo.Id;
                Code = departmentBo.Code;
                Name = departmentBo.Name;
                HodUserId = departmentBo.HodUserId;
            }
        }

        public static Expression<Func<Department, DepartmentViewModel>> Expression()
        {
            return entity => new DepartmentViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
                HodUserId = entity.HodUserId,
                HodUser = entity.HodUser,
            };
        }
    }
}