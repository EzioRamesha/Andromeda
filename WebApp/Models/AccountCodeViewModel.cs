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
    public class AccountCodeViewModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Account Code")]
        public string Code { get; set; }

        [Required]
        [DisplayName("Reporting Type")]
        public int ReportingType { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        public int? Type { get; set; }

        public string Remarks { get; set; }

        public AccountCodeViewModel() { }

        public AccountCodeViewModel(AccountCodeBo accountCodeBo)
        {
            if (accountCodeBo != null)
            {
                Id = accountCodeBo.Id;
                Code = accountCodeBo.Code;
                ReportingType = accountCodeBo.ReportingType;
                Description = accountCodeBo.Description;
                Type = accountCodeBo.Type;
            }
        }

        public static Expression<Func<AccountCode, AccountCodeViewModel>> Expression()
        {
            return entity => new AccountCodeViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                ReportingType = entity.ReportingType,
                Description = entity.Description,
                Type = entity.Type,
            };
        }
    }
}