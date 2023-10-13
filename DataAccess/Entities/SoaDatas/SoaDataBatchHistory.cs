using Shared.Trails.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DataAccess.Entities.SoaDatas
{
    [Table("SoaDataBatchHistories")]
    public class SoaDataBatchHistory
    {
        [Key]
        public int Id { get; set; }

        [Required, Index]
        public int CutOffId { get; set; }
        [ExcludeTrail]
        public virtual CutOff CutOff { get; set; }

        [Required, Index]
        public int SoaDataBatchId { get; set; }
        [Required, Index]
        public int CedantId { get; set; }
        [Index]
        public int? TreatyId { get; set; }


        [MaxLength(64), Index]
        public string Quarter { get; set; }

        public int? CurrencyCodePickListDetailId { get; set; }

        public double? CurrencyRate { get; set; }

        [Index]
        public int Type { get; set; }

        [Required, Index]
        public int Status { get; set; }

        [Required, Index]
        public int DataUpdateStatus { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime StatementReceivedAt { get; set; }

        [Index]
        public int? RiDataBatchId { get; set; }
        [Index]
        public int? ClaimDataBatchId { get; set; }

        public int DirectStatus { get; set; }
        public int InvoiceStatus { get; set; }

        public int TotalRecords { get; set; }
        public int TotalMappingFailedStatus { get; set; }

        public bool IsAutoCreate { get; set; } = false;
        public bool IsClaimDataAutoCreate { get; set; } = false;

        public bool IsProfitCommissionData { get; set; } = false;

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }
        [Required]
        public int CreatedById { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime UpdatedAt { get; set; }
        public int? UpdatedById { get; set; }

        public static string Script(int cutOffId)
        {
            string[] fields = Fields();

            string script = "INSERT INTO [dbo].[SoaDataBatchHistories]";
            string field = string.Format("({0})", string.Join(",", fields));
            string select = string.Format("SELECT {0},[Id],{1} FROM [dbo].[SoaDataBatches]", cutOffId, string.Join(",", fields.Skip(2).ToArray()));

            return string.Format("{0}\n{1}\n{2};", script, field, select);
        }

        public static string[] Fields()
        {
            return new string[]
            {
                "[CutOffId]",
                "[SoaDataBatchId]",
                "[CedantId]",
                "[TreatyId]",
                "[Quarter]",
                "[CurrencyCodePickListDetailId]",
                "[CurrencyRate]",
                "[Type]",
                "[Status]",
                "[DataUpdateStatus]",
                "[StatementReceivedAt]",
                "[RiDataBatchId]",
                "[ClaimDataBatchId]",
                "[DirectStatus]",
                "[InvoiceStatus]",
                "[TotalRecords]",
                "[TotalMappingFailedStatus]",
                "[IsAutoCreate]",
                "[IsClaimDataAutoCreate]",
                "[IsProfitCommissionData]",
                "[CreatedAt]",
                "[CreatedById]",
                "[UpdatedAt]",
                "[UpdatedById]",
            };
        }
    }
}
