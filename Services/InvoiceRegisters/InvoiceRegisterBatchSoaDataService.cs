using BusinessObject.InvoiceRegisters;
using DataAccess.Entities.InvoiceRegisters;
using DataAccess.EntityFramework;
using Services.SoaDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System.Collections.Generic;

namespace Services.InvoiceRegisters
{
    public class InvoiceRegisterBatchSoaDataService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(InvoiceRegisterBatchSoaData)),
            };
        }

        public static InvoiceRegisterBatchSoaDataBo FormBo(InvoiceRegisterBatchSoaData entity = null)
        {
            if (entity == null)
                return null;
            return new InvoiceRegisterBatchSoaDataBo
            {
                InvoiceRegisterBatchId = entity.InvoiceRegisterBatchId,
                //InvoiceRegisterBatchDetailBo = InvoiceRegisterBatchDetailService.Find(entity.InvoiceRegisterBatchId),
                SoaDataBatchId = entity.SoaDataBatchId,
                SoaDataBatchBo = SoaDataBatchService.Find(entity.SoaDataBatchId),
            };
        }

        public static InvoiceRegisterBatchSoaDataBo FormSimplifiedBo(InvoiceRegisterBatchSoaData entity = null)
        {
            if (entity == null)
                return null;
            return new InvoiceRegisterBatchSoaDataBo
            {
                InvoiceRegisterBatchId = entity.InvoiceRegisterBatchId,
                SoaDataBatchId = entity.SoaDataBatchId,
            };
        }

        public static IList<InvoiceRegisterBatchSoaDataBo> FormBos(IList<InvoiceRegisterBatchSoaData> entities = null)
        {
            if (entities == null)
                return null;
            IList<InvoiceRegisterBatchSoaDataBo> bos = new List<InvoiceRegisterBatchSoaDataBo>() { };
            foreach (InvoiceRegisterBatchSoaData entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static IList<InvoiceRegisterBatchSoaDataBo> FormSimplifiedBos(IList<InvoiceRegisterBatchSoaData> entities = null)
        {
            if (entities == null)
                return null;
            IList<InvoiceRegisterBatchSoaDataBo> bos = new List<InvoiceRegisterBatchSoaDataBo>() { };
            foreach (InvoiceRegisterBatchSoaData entity in entities)
            {
                bos.Add(FormSimplifiedBo(entity));
            }
            return bos;
        }

        public static InvoiceRegisterBatchSoaData FormEntity(InvoiceRegisterBatchSoaDataBo bo = null)
        {
            if (bo == null)
                return null;
            return new InvoiceRegisterBatchSoaData
            {
                InvoiceRegisterBatchId = bo.InvoiceRegisterBatchId,
                SoaDataBatchId = bo.SoaDataBatchId,
            };
        }

        public static InvoiceRegisterBatchSoaDataBo Find(int invoiceRegisterBatchDetailId, int soaDataId)
        {
            return FormBo(InvoiceRegisterBatchSoaData.Find(invoiceRegisterBatchDetailId, soaDataId));
        }

        public static int CountByInvoiceRegisterBatchId(int invoiceRegisterBatchDetailId)
        {
            return InvoiceRegisterBatchSoaData.CountByInvoiceRegisterBatchId(invoiceRegisterBatchDetailId);
        }

        public static int CountBySoaDataBatchId(int soaDataBatchId)
        {
            return InvoiceRegisterBatchSoaData.CountBySoaDataBatchId(soaDataBatchId);
        }

        public static List<int> GetIdsByInvoiceRegisterBatchId(int invoiceRegisterBatchDetailId)
        {
            return InvoiceRegisterBatchSoaData.GetIdsByInvoiceRegisterBatchId(invoiceRegisterBatchDetailId);
        }

        public static IList<InvoiceRegisterBatchSoaDataBo> GetByInvoiceRegisterBatchId(int invoiceRegisterBatchDetailId, int skip, int take)
        {
            return FormBos(InvoiceRegisterBatchSoaData.GetByInvoiceRegisterBatchId(invoiceRegisterBatchDetailId, skip, take));
        }

        public static Result Create(ref InvoiceRegisterBatchSoaDataBo bo)
        {
            InvoiceRegisterBatchSoaData entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
            }
            return result;
        }

        public static void Create(ref InvoiceRegisterBatchSoaDataBo bo, AppDbContext db)
        {
            InvoiceRegisterBatchSoaData entity = FormEntity(bo);
            entity.Create(db);
        }

        public static Result Create(ref InvoiceRegisterBatchSoaDataBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table, bo.PrimaryKey());
            }
            return result;
        }

        public static IList<DataTrail> DeleteAllByInvoiceRegisterBatchId(int invoiceRegisterBatchDetailId)
        {
            return InvoiceRegisterBatchSoaData.DeleteAllByInvoiceRegisterBatchId(invoiceRegisterBatchDetailId);
        }

        public static void DeleteAllByInvoiceRegisterBatchId(int invoiceRegisterBatchDetailId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByInvoiceRegisterBatchId(invoiceRegisterBatchDetailId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(InvoiceRegisterBatchSoaData)));
                }
            }
        }
    }
}
