using BusinessObject;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class SalutationViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [DisplayName("Salutation")]
        public string Name { get; set; }

        public SalutationViewModel() { }

        public SalutationViewModel(SalutationBo salutationBo)
        {
            if (salutationBo != null)
            {
                Id = salutationBo.Id;
                Name = salutationBo.Name;
            }
        }

        public static Expression<Func<Salutation, SalutationViewModel>> Expression()
        {
            return entity => new SalutationViewModel
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }
    }
}