using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities;
using Services;
using Services.Identity;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class AccessGroupViewModel
    {
        public int Id { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        [Required]
        [StringLength(20)]
        public string Code { get; set; }

        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        public List<HttpPostedFileBase> Files { get; set; }

        public int ModuleId { get; set; }

        public AccessGroupViewModel()
        {
            Set();
        }

        public AccessGroupViewModel(AccessGroupBo accessGroupBo)
        {
            Set(accessGroupBo);
        }

        public void Set(AccessGroupBo accessGroupBo = null)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.AccessGroup.ToString());
            ModuleId = moduleBo.Id;
            if (accessGroupBo != null)
            {
                Id = accessGroupBo.Id;
                DepartmentId = accessGroupBo.DepartmentId;
                Code = accessGroupBo.Code;
                Name = accessGroupBo.Name;
            }
            else
            {
                Id = 0;
                DepartmentId = 0;
                Code = null;
                Name = null;
            }
        }

        public static Expression<Func<AccessGroup, AccessGroupViewModel>> Expression()
        {
            return entity => new AccessGroupViewModel
            {
                Id = entity.Id,
                DepartmentId = entity.DepartmentId,
                Department = entity.Department,
                Code = entity.Code,
                Name = entity.Name,
            };
        }

        public IList<AccessMatrixBo> GetMatrices(FormCollection form)
        {
            string formName;
            string formValue;
            bool checkedValue;

            IList<AccessMatrixBo> dbAccessMatrices = new List<AccessMatrixBo>() { };
            for (var i = 1; i <= ModuleBo.TypeMax; i++)
            {
                if (ModuleService.CountByType(i) == 0)
                {
                    continue;
                }

                foreach (ModuleBo moduleBo in ModuleService.GetByType(i))
                {
                    AccessMatrixBo dbAccessMatrix = new AccessMatrixBo
                    {
                        AccessGroupId = Id,
                        ModuleId = moduleBo.Id,
                        Powers = new List<string>(),
                        DepartmentName = moduleBo.DepartmentBo.Name,
                        ModuleBo = moduleBo
                    };

                    foreach (string power in moduleBo.GetPowers())
                    {
                        formName = moduleBo.GetCheckBoxName(power);
                        string tempFormValue = form.Get(formName);
                        formValue = form[formName];
                        string[] formattedValues = formValue.Split(',');

                        if (Code == AccessGroup.DefaultSuperCode && formattedValues.Length == 1)
                        {
                            checkedValue = true;
                        } else
                        {
                            checkedValue = formValue != null ? Convert.ToBoolean(formattedValues[0]) : false;
                        }
                        

                        if (checkedValue)
                        {
                            dbAccessMatrix.Powers.Add(power);
                        }
                    }

                    foreach (string power in moduleBo.GetPowerAdditionals())
                    {
                        formName = moduleBo.GetAdditionalCheckBoxName(power);
                        formValue = form[formName];
                        checkedValue = formValue != null ? Convert.ToBoolean(formValue.Split(',')[0]) : false;

                        if (checkedValue)
                        {
                            dbAccessMatrix.Powers.Add(power);
                        }
                    }

                    dbAccessMatrix.SetPowerFromPowers();
                    dbAccessMatrices.Add(dbAccessMatrix);
                }
            }

            return dbAccessMatrices;
        }

        public void ProcessMatrices(FormCollection form, ref TrailObject trail)
        {
            IList<AccessMatrixBo> dbAccessMatrices = GetMatrices(form);

            foreach (AccessMatrixBo dbAccessMatrix in dbAccessMatrices)
            {
                if (string.IsNullOrEmpty(dbAccessMatrix.Power))
                {
                    if (AccessMatrixService.IsExists(dbAccessMatrix.ModuleId, dbAccessMatrix.AccessGroupId))
                        AccessMatrixService.Delete(dbAccessMatrix, ref trail);
                }
                else
                    AccessMatrixService.Save(dbAccessMatrix, ref trail);
            }
        }

        public void ProcessDocuments(FormCollection form, int authUserId, ref TrailObject trail)
        {
            int documentMaxIndex = int.Parse(form.Get("documentMaxIndex"));
            for (int i = 0; i <= documentMaxIndex; i++)
            {
                string documentId = form.Get(string.Format("documentId[{0}]", i));
                if (!string.IsNullOrEmpty(documentId))
                    continue;

                string tempFilePath = form.Get(string.Format("tempFilePath[{0}]", i));
                if (string.IsNullOrEmpty(tempFilePath))
                    continue;

                int type = int.Parse(form.Get(string.Format("type[{0}]", i)));
                string fileName = form.Get(string.Format("filename[{0}]", i));
                string hashFileName = form.Get(string.Format("hashFileName[{0}]", i));
                string description = form.Get(string.Format("description[{0}]", i));
                string createdAtStr = form.Get(string.Format("createdAtStr[{0}]", i));

                DateTime dateTime = DateTime.Parse(createdAtStr, new CultureInfo("fr-FR", false));
                ModuleBo moduleBo = ModuleService.Find(ModuleId);
                DocumentBo documentBo = new DocumentBo
                {
                    Type = type,
                    ModuleId = ModuleId,
                    ModuleBo = moduleBo,
                    ObjectId = Id,
                    FileName = fileName,
                    HashFileName = hashFileName,
                    Description = description,
                    CreatedById = authUserId,
                    UpdatedById = authUserId,
                    CreatedAt = dateTime,
                    UpdatedAt = dateTime,
                };

                string path = documentBo.GetLocalPath();
                Util.MakeDir(path);
                string tempPath = documentBo.GetTempPath("Uploads");
                File.Move(tempPath, path);

                DocumentService.Save(ref documentBo);
            }
        }
    }

    public class RequestAccessGroupViewModel : AccessGroupViewModel
    {
        [Required]
        [DisplayName("Type Of Request")]
        public int RequestType { get; set; }

        [DisplayName("Name")]
        public int? SelectId { get; set; }

        [DisplayName("Request Date")]
        public string RequestDate { get; set; }

        [Required, DisplayName("Requested Date")]
        public string RequestDateStr { get; set; }

        [Required, DisplayName("Requested By")]
        public string RequestUserName { get; set; }

        public bool Download { get; set; } = false;

        public RequestAccessGroupViewModel() {
            RequestDateStr = DateTime.Now.ToString(Util.GetDateFormat());
        }
    }
}