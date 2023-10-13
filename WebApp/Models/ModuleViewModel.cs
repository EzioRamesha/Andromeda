using BusinessObject;
using DataAccess.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class ModuleViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        [StringLength(64)]
        public string Controller { get; set; }

        public bool Editable { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [StringLength(64)]
        [Display(Name = "Report Path")]
        public string ReportPath { get; set; }

        public ModuleViewModel() { }

        public ModuleViewModel(ModuleBo moduleBo)
        {
            Type = ModuleBo.TypeReport;
            if (moduleBo != null)
            {
                Id = moduleBo.Id;
                Type = moduleBo.Type;
                Controller = moduleBo.Controller;
                Editable = moduleBo.Editable;
                Name = moduleBo.Name;
                ReportPath = moduleBo.ReportPath;
                DepartmentId = moduleBo.DepartmentId;
            }
        }

        public static Expression<Func<Module, ModuleViewModel>> Expression()
        {
            return entity => new ModuleViewModel
            {
                Id = entity.Id,
                Type = entity.Type,
                Controller = entity.Controller,
                Editable = entity.Editable,
                Name = entity.Name,
                ReportPath = entity.ReportPath,
                DepartmentId = entity.DepartmentId,
            };
        }
    }
}