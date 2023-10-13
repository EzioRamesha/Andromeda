using DataAccess.Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class OperationDashboardViewModel
    {
        public IList<object> Report { get; set; }

        public IPagedList<CutOffViewModel> CutOff { get; set; }
    }

    public class CutOffViewModel
    {
        public int Id { get; set; }

        public int Status { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public string Quarter { get; set; }

        public DateTime? CutOffDateTime { get; set; }

        public string CreatedBy { get; set; }

        public static Expression<Func<CutOff, CutOffViewModel>> Expression()
        {
            return entity => new CutOffViewModel
            {
                Id = entity.Id,
                Status = entity.Status,
                Month = entity.Month,
                Year = entity.Year,
                Quarter = entity.Quarter,
                CutOffDateTime = entity.CutOffDateTime,
                CreatedBy = entity.CreatedBy.FullName,
            };
        }
    }
}