using BusinessObject;
using BusinessObject.Identity;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using Services;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class PublicHolidayViewModel : IValidatableObject
    {
        public int Id { get; set; }

        public int Year { get; set; }

        [Required, DisplayName("Year")]
        public string YearStr { get; set; }

        public PublicHolidayViewModel() 
        {
            YearStr = DateTime.Now.Year.ToString();
        }

        public PublicHolidayViewModel(PublicHolidayBo publicHolidayBo)
        {
            if (publicHolidayBo != null)
            {
                Id = publicHolidayBo.Id;
                YearStr = publicHolidayBo.Year.ToString();
            }
        }

        public PublicHolidayBo FormBo(int createdById, int updatedById)
        {
            return new PublicHolidayBo
            {
                Id = Id,
                Year = int.Parse(YearStr),

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<PublicHoliday, PublicHolidayViewModel>> Expression()
        {
            return entity => new PublicHolidayViewModel
            {
                Id = entity.Id,
                Year = entity.Year,
            };
        }

        public List<PublicHolidayDetailBo> GetPublicHolidayDetails(FormCollection form)
        {
            int index = 0;
            List<PublicHolidayDetailBo> phDetailBos = new List<PublicHolidayDetailBo> { };
            while (form.AllKeys.Contains(string.Format("publicHolidayDate[{0}]", index)))
            {
                string date = form.Get(string.Format("publicHolidayDate[{0}]", index));
                string description = form.Get(string.Format("description[{0}]", index));
                string id = form.Get(string.Format("calCedantDetailId[{0}]", index));

                PublicHolidayDetailBo phDetailBo = new PublicHolidayDetailBo
                {
                    PublicHolidayId = Id,
                    PublicHolidayDateStr = date,
                    Description = description
                };
                if (!string.IsNullOrEmpty(id)) phDetailBo.Id = int.Parse(id);

                phDetailBos.Add(phDetailBo);
                index++;
            }
            return phDetailBos;
        }

        public void ProcessPublicHolidayDetails(List<PublicHolidayDetailBo> phDetailBos, int authUserId, ref TrailObject trail)
        {
            List<int> savedIds = new List<int> { };
            foreach (PublicHolidayDetailBo phDetailBo in phDetailBos)
            {
                phDetailBo.PublicHolidayId = Id;
                phDetailBo.PublicHolidayDate = DateTime.Parse(phDetailBo.PublicHolidayDateStr);
                phDetailBo.Description = phDetailBo.Description;
                phDetailBo.CreatedById = authUserId;
                phDetailBo.UpdatedById = authUserId;
                PublicHolidayDetailService.Create(phDetailBo, ref trail);

                savedIds.Add(phDetailBo.Id);
            }
            PublicHolidayDetailService.DeleteByPublicHolidayIdExcept(Id, savedIds, ref trail);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            PublicHoliday entity = new PublicHoliday()
            {
                Id = Id,
                Year = int.Parse(YearStr)
            };

            if (!string.IsNullOrEmpty(YearStr) && PublicHolidayService.IsDuplicateYear(entity))
            {
                results.Add(new ValidationResult("Public Holiday for " + YearStr + " have already been Created Previously",
                    new[] { nameof(YearStr) }));
            }

            return results;
        }
    }
}