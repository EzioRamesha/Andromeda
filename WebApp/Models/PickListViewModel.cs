using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities;
using Services;
using Services.Identity;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class PickListViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Display(Name = "Department")]
        public Department Department { get; set; }

        [Display(Name = "Standard Output")]
        public int? StandardOutputId { get; set; }

        [Display(Name = "Standard Output")]
        public StandardOutput StandardOutput { get; set; }

        [Required, StringLength(128), Display(Name = "Field Name")]
        public string FieldName { get; set; }

        [DisplayName("Used By")]
        public string UsedBy { get; set; }

        public bool IsEditable { get; set; }

        public virtual ICollection<PickListDepartment> PickListDepartments { get; set; }

        public PickListViewModel() { }

        public PickListViewModel(PickListBo pickListBo)
        {
            if (pickListBo != null)
            {
                Id = pickListBo.Id;
                DepartmentId = pickListBo.DepartmentId;
                StandardOutputId = pickListBo.StandardOutputId;
                FieldName = pickListBo.FieldName;
                IsEditable = pickListBo.IsEditable;
            }
        }

        public PickListBo FormBo(int createdById, int updatedById)
        {
            return new PickListBo
            {
                Id = Id,
                DepartmentId = DepartmentId,
                DepartmentBo = DepartmentService.Find(DepartmentId),
                StandardOutputId = StandardOutputId,
                FieldName = FieldName?.Trim(),
                IsEditable = IsEditable,

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<PickList, PickListViewModel>> Expression()
        {
            return entity => new PickListViewModel
            {
                Id = entity.Id,
                DepartmentId = entity.DepartmentId,
                Department = entity.Department,
                StandardOutputId = entity.StandardOutputId,
                FieldName = entity.FieldName,
                IsEditable = entity.IsEditable,
                PickListDepartments = entity.PickListDepartments

            };
        }

        public List<PickListDetailBo> GetPickListDetails(FormCollection form, ref Result result)
        {
            int index = 0;
            int maxIndex = int.Parse(form.Get("pickListDetailMaxIndex"));
            List<PickListDetailBo> pickListDetailBos = new List<PickListDetailBo> { };
            List<string> codes = new List<string> { };

            while (index <= maxIndex)
            {
                string code = form.Get(string.Format("code[{0}]", index))?.Trim();
                string description = form.Get(string.Format("description[{0}]", index))?.Trim();
                string id = form.Get(string.Format("id[{0}]", index));

                int pickListDetailId = 0;
                if (!string.IsNullOrEmpty(id))
                    pickListDetailId = int.Parse(id);

                if (string.IsNullOrEmpty(code) && string.IsNullOrEmpty(description) && pickListDetailId == 0)
                {
                    index++;
                    continue;
                }

                if (string.IsNullOrEmpty(code) || string.IsNullOrWhiteSpace(code))
                {
                    result.AddError(string.Format("Code cannot be empty or white space at row #{0}", index + 1));
                }
                else
                {
                    if (codes.Contains(code.ToLower()))
                    {
                        result.AddError(string.Format("Duplicate code at row #{0}", index + 1));
                    }
                    codes.Add(code.ToLower());
                }

                if (string.IsNullOrEmpty(description) || string.IsNullOrWhiteSpace(description))
                {
                    result.AddError(string.Format("Description cannot be empty or white space at row #{0}", index + 1));
                }

                PickListDetailBo pickListDetailBo = new PickListDetailBo
                {
                    PickListId = Id,
                    SortIndex = index + 1,
                    Code = code,
                    Description = description,
                };

                if (pickListDetailId != 0)
                {
                    pickListDetailBo.Id = pickListDetailId;
                }

                pickListDetailBos.Add(pickListDetailBo);
                index++;
            }
            return pickListDetailBos;
        }

        public void ProcessPickListDetails(List<PickListDetailBo> pickListDetailBos, int authUserId, ref TrailObject trail)
        {
            List<int> savedIds = new List<int> { };

            foreach (PickListDetailBo bo in pickListDetailBos)
            {
                PickListDetailBo pickListDetailBo = bo;
                pickListDetailBo.PickListId = Id;
                pickListDetailBo.CreatedById = authUserId;
                pickListDetailBo.UpdatedById = authUserId;

                PickListDetailService.Save(pickListDetailBo, ref trail);
                savedIds.Add(pickListDetailBo.Id);
            }
            PickListDetailService.DeleteByPickListDetailIdExcept(Id, savedIds, ref trail);
        }
    }
}