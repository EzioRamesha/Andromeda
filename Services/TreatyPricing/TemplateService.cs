using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Services.TreatyPricing
{
    public class TemplateService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Template)),
                Controller = ModuleBo.ModuleController.Template.ToString()
            };
        }

        public static TemplateBo FormBo(Template entity = null)
        {
            if (entity == null)
                return null;
            return new TemplateBo
            {
                Id = entity.Id,
                Code = entity.Code,
                CedantId = entity.CedantId,
                DocumentTypeId = entity.DocumentTypeId,
                Description = entity.Description,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                CedantBo = CedantService.Find(entity.CedantId),
            };
        }

        public static IList<TemplateBo> FormBos(IList<Template> entities = null)
        {
            if (entities == null)
                return null;
            IList<TemplateBo> bos = new List<TemplateBo>() { };
            foreach (Template entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static Template FormEntity(TemplateBo bo = null)
        {
            if (bo == null)
                return null;
            return new Template
            {
                Id = bo.Id,
                Code = bo.Code,
                CedantId = bo.CedantId,
                DocumentTypeId = bo.DocumentTypeId,
                Description = bo.Description,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateCode(Template Template)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(Template.Code))
                {
                    var query = db.Templates.Where(q => q.Code == Template.Code);
                    if (Template.Id != 0)
                    {
                        query = query.Where(q => q.Id != Template.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static bool IsExists(int id)
        {
            return Template.IsExists(id);
        }

        public static TemplateBo Find(int id)
        {
            return FormBo(Template.Find(id));
        }

        public static TemplateBo FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.Templates.Where(q => q.Code == code).FirstOrDefault());
            }
        }

        public static IList<TemplateBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.Templates.ToList());
            }
        }

        public static IList<TemplateBo> GetByDocumentType(string documentType)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.Templates.Where(q => q.DocumentTypeId == documentType).ToList());
            }
        }

        public static IList<TemplateBo> GetByCedantAndDocumentType(int cedantId, string documentType)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.Templates.Where(q => q.CedantId == cedantId && q.DocumentTypeId == documentType).ToList());
            }
        }

        public static Result Save(ref TemplateBo bo)
        {
            if (!Template.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TemplateBo bo, ref TrailObject trail)
        {
            if (!Template.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TemplateBo bo)
        {
            Template entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Code", bo.Code.ToString());
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TemplateBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TemplateBo bo)
        {
            Result result = Result();

            Template entity = Template.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Code", bo.Code.ToString());
            }

            if (result.Valid)
            {
                entity.Code = bo.Code;
                entity.CedantId = bo.CedantId;
                entity.DocumentTypeId = bo.DocumentTypeId;
                entity.Description = bo.Description;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TemplateBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TemplateBo bo)
        {
            var templateFiles = TemplateDetailService.GetByTemplateId(bo.Id);
            foreach (var templateFile in templateFiles)
            {
                if (File.Exists(templateFile.GetLocalPath()))
                    File.Delete(templateFile.GetLocalPath());
            }
            TemplateDetailService.DeleteAllByTemplateId(bo.Id);
            Template.Delete(bo.Id);
        }

        public static Result Delete(TemplateBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (TreatyPricingGroupReferralService.CountByTemplateId(bo.Id) > 0)
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                var templateFiles = TemplateDetailService.GetByTemplateId(bo.Id);
                foreach (var templateFile in templateFiles)
                {
                    if (File.Exists(templateFile.GetLocalPath()))
                        File.Delete(templateFile.GetLocalPath());
                }
                TemplateDetailService.DeleteAllByTemplateId(bo.Id, ref trail);

                DataTrail dataTrail = Template.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
