using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities;
using DataAccess.Entities.TreatyPricing;
using Services;
using Services.TreatyPricing;
using Shared;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class TemplateViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(255), Display(Name = "Template Code")]
        public string Code { get; set; }

        [Required, Display(Name = "Ceding Company")]
        public int CedantId { get; set; }

        [Display(Name = "Ceding Company")]
        public CedantBo CedantBo { get; set; }

        public virtual Cedant Cedant { get; set; }

        [Required, StringLength(255), Display(Name = "Document Type")]
        public string DocumentTypeId { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public TemplateViewModel() { }

        public TemplateViewModel(TemplateBo templateBo)
        {
            if (templateBo != null)
            {
                Id = templateBo.Id;
                Code = templateBo.Code;
                CedantId = templateBo.CedantId;
                DocumentTypeId = templateBo.DocumentTypeId;
                Description = templateBo.Description;

                CedantBo = templateBo.CedantBo;
            }
        }

        public TemplateBo FormBo(int createdById, int updatedById)
        {
            return new TemplateBo
            {
                Id = Id,
                Code = Code,
                CedantId = CedantId,
                DocumentTypeId = DocumentTypeId,
                Description = Description,
                CedantBo = CedantService.Find(CedantId),

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<Template, TemplateViewModel>> Expression()
        {
            return entity => new TemplateViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                CedantId = entity.CedantId,
                DocumentTypeId = entity.DocumentTypeId,
                Description = entity.Description,

                Cedant = entity.Cedant,
            };
        }

        public List<TemplateDetailBo> GetTemplateDetails(FormCollection form)
        {
            int index = 0;
            List<TemplateDetailBo> templateDetailBos = new List<TemplateDetailBo> { };
            while (form.AllKeys.Contains(string.Format("version[{0}]", index)))
            {
                string version = form.Get(string.Format("version[{0}]", index));
                string fileName = form.Get(string.Format("fileName[{0}]", index));
                string hashFileName = form.Get(string.Format("hashFileName[{0}]", index));
                string tempFilePath = form.Get(string.Format("tempFilePath[{0}]", index));
                string uploadAt = form.Get(string.Format("uploadAt[{0}]", index));
                string uploadBy = form.Get(string.Format("uploadBy[{0}]", index));
                string uploadByName = form.Get(string.Format("uploadByName[{0}]", index));
                string id = form.Get(string.Format("templateDetailId[{0}]", index));

                var templateDetailBo = new TemplateDetailBo
                {
                    TemplateId = Id,
                    FileName = fileName,
                    HashFileName = hashFileName,
                    TempFilePath = tempFilePath,
                    CreatedAtStr = uploadAt,
                    CreatedById = int.Parse(uploadBy),
                    CreatedByName = uploadByName,
                };

                if (!string.IsNullOrEmpty(version)) templateDetailBo.TemplateVersion = int.Parse(version);
                if (!string.IsNullOrEmpty(id)) templateDetailBo.Id = int.Parse(id);

                templateDetailBos.Add(templateDetailBo);
                index++;
            }
            return templateDetailBos;
        }

        public void ProcessTemplateDetails(List<TemplateDetailBo> templateDetailBos, int authUserId, ref TrailObject trail)
        {
            List<int> savedIds = new List<int> { };
            foreach (TemplateDetailBo templateDetailBo in templateDetailBos)
            {
                templateDetailBo.TemplateId = Id;
                templateDetailBo.CreatedAt = DateTime.Parse(templateDetailBo.CreatedAtStr);
                templateDetailBo.CreatedById = authUserId;
                TemplateDetailService.Save(templateDetailBo, ref trail);

                if (File.Exists(templateDetailBo.TempFilePath))
                {
                    string path = templateDetailBo.GetLocalPath();

                    Util.MakeDir(path);
                    File.Move(templateDetailBo.TempFilePath, path);
                }

                savedIds.Add(templateDetailBo.Id);
            }
            TemplateDetailService.DeleteByTemplateIdExcept(Id, savedIds, ref trail);
        }
    }
}