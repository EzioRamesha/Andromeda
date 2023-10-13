using BusinessObject;
using DataAccess.Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class ExportViewModel
    {
        public ExportBo ExportBo { get; set; }
        public int Id { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public int? ObjectId { get; set; }
        public int Total { get; set; }
        public int Processed { get; set; }
        public string Parameters { get; set; }
        public dynamic ParameterObject { get; set; }
        public Dictionary<string, object> ParameterDic { get; set; }
        [DisplayName("Generate Start At")]
        public DateTime? GenerateStartAt { get; set; }
        [DisplayName("Generate Start At")]
        public string GenerateStartAtStr { get; set; }
        [DisplayName("Generate End At")]
        public DateTime? GenerateEndAt { get; set; }
        [DisplayName("Generate End At")]
        public string GenerateEndAtStr { get; set; }
        [DisplayName("Requested At")]
        public DateTime CreatedAt { get; set; }
        [DisplayName("Requested At")]
        public string CreatedAtStr { get; set; }
        public int CreatedById { get; set; }

        [DisplayName("Type")]
        public string TypeName { get; set; }
        [DisplayName("Status")]
        public string StatusName { get; set; }

        public ExportViewModel()
        {
        }

        public ExportViewModel(ExportBo bo)
        {
            ExportBo = bo;

            Id = bo.Id;
            Type = bo.Type;
            Status = bo.Status;
            ObjectId = bo.ObjectId;
            Total = bo.Total;
            Processed = bo.Processed;
            Parameters = bo.Parameters;
            ParameterObject = bo.ParameterObject;
            ParameterDic = bo.ParameterDic;
            GenerateStartAt = bo.GenerateStartAt;
            GenerateStartAtStr = bo.GenerateStartAt?.ToString(Util.GetDateTimeFormat());
            GenerateEndAt = bo.GenerateEndAt;
            GenerateEndAtStr = bo.GenerateEndAt?.ToString(Util.GetDateTimeFormat());
            CreatedAt = bo.CreatedAt;
            CreatedAtStr = bo.CreatedAt.ToString(Util.GetDateTimeFormat());
            CreatedById = bo.CreatedById;

            TypeName = ExportBo.GetTypeName(Type);
            StatusName = ExportBo.GetStatusName(Status);
        }

        public static Expression<Func<Export, ExportViewModel>> Expression()
        {
            return entity => new ExportViewModel
            {
                Id = entity.Id,
                Type = entity.Type,
                Status = entity.Status,
                ObjectId = entity.ObjectId,
                Total = entity.Total,
                Processed = entity.Processed,
                GenerateStartAt = entity.GenerateStartAt,
                GenerateEndAt = entity.GenerateEndAt,
                CreatedAt = entity.CreatedAt,
                CreatedById = entity.CreatedById,
            };
        }
    }
}