using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class UwQuestionnaireCategoryViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(255), DisplayName("Code")]
        public string Code { get; set; }

        [Required, StringLength(255), DisplayName("Name")]
        public string Name { get; set; }

        public UwQuestionnaireCategoryViewModel() { }

        public UwQuestionnaireCategoryViewModel(UwQuestionnaireCategoryBo uwQuestionnaireCategoryBo)
        {
            Set(uwQuestionnaireCategoryBo);
        }

        public void Set(UwQuestionnaireCategoryBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                Code = bo.Code;
                Name = bo.Name;
            }
        }

        public UwQuestionnaireCategoryBo FormBo(int createdById, int updatedById)
        {
            return new UwQuestionnaireCategoryBo
            {
                Code = Code,
                Name = Name,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<UwQuestionnaireCategory, UwQuestionnaireCategoryViewModel>> Expression()
        {
            return entity => new UwQuestionnaireCategoryViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = entity.Name,
            };
        }
    }
}