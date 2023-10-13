using BusinessObject;
using DataAccess.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class EventCodeViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [DisplayName("MLRe Event Code")]
        public string Code { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        public int Status { get; set; }

        public string Remarks { get; set; }

        public EventCodeViewModel() { }

        public EventCodeViewModel(EventCodeBo eventCodeBo)
        {
            if (eventCodeBo != null)
            {
                Id = eventCodeBo.Id;
                Code = eventCodeBo.Code;
                Status = eventCodeBo.Status;
                Description = eventCodeBo.Description;
            }
        }

        public static Expression<Func<EventCode, EventCodeViewModel>> Expression()
        {
            return entity => new EventCodeViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                Status = entity.Status,
                Description = entity.Description,
            };
        }
    }
}