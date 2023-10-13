using BusinessObject;
using BusinessObject.InvoiceRegisters;
using DataAccess.Entities.InvoiceRegisters;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;

namespace Services.InvoiceRegisters
{
    public class InvoiceRegisterValuationService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(InvoiceRegisterValuation)),
            };
        }

        public static InvoiceRegisterValuationBo FormBo(InvoiceRegisterValuation entity = null)
        {
            if (entity == null)
                return null;
            return new InvoiceRegisterValuationBo
            {
                Id = entity.Id,
                InvoiceRegisterId = entity.InvoiceRegisterId,
                ValuationBenefitCodeId = entity.ValuationBenefitCodeId,
                Amount = entity.Amount,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                InvoiceRegisterBo = InvoiceRegisterService.Find(entity.InvoiceRegisterId),
                ValuationBenefitCodeBo = BenefitService.Find(entity.ValuationBenefitCodeId),
            };
        }

        public static IList<InvoiceRegisterValuationBo> FormBos(IList<InvoiceRegisterValuation> entities = null)
        {
            if (entities == null)
                return null;
            IList<InvoiceRegisterValuationBo> bos = new List<InvoiceRegisterValuationBo>() { };
            foreach (InvoiceRegisterValuation entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static InvoiceRegisterValuation FormEntity(InvoiceRegisterValuationBo bo = null)
        {
            if (bo == null)
                return null;
            return new InvoiceRegisterValuation
            {
                Id = bo.Id,
                InvoiceRegisterId = bo.InvoiceRegisterId,
                ValuationBenefitCodeId = bo.ValuationBenefitCodeId,
                Amount = bo.Amount,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return InvoiceRegisterValuation.IsExists(id);
        }

        public static InvoiceRegisterValuationBo Find(int id)
        {
            return FormBo(InvoiceRegisterValuation.Find(id));
        }

        public static IList<InvoiceRegisterValuationBo> Get()
        {
            return FormBos(InvoiceRegisterValuation.Get());
        }

        public static IList<InvoiceRegisterValuationBo> GetByInvoiceRegisterId(int invoiceRegisterId)
        {
            return FormBos(InvoiceRegisterValuation.GetByInvoiceRegisterId(invoiceRegisterId));
        }

        public static Result Save(ref InvoiceRegisterValuationBo bo)
        {
            if (!InvoiceRegisterValuation.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref InvoiceRegisterValuationBo bo, ref TrailObject trail)
        {
            if (!InvoiceRegisterValuation.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref InvoiceRegisterValuationBo bo)
        {
            InvoiceRegisterValuation entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref InvoiceRegisterValuationBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref InvoiceRegisterValuationBo bo)
        {
            Result result = Result();

            InvoiceRegisterValuation entity = InvoiceRegisterValuation.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.InvoiceRegisterId = bo.InvoiceRegisterId;
                entity.ValuationBenefitCodeId = bo.ValuationBenefitCodeId;
                entity.Amount = bo.Amount;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref InvoiceRegisterValuationBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(InvoiceRegisterValuationBo bo)
        {
            InvoiceRegisterValuation.Delete(bo.Id);
        }

        public static Result Delete(InvoiceRegisterValuationBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                DataTrail dataTrail = InvoiceRegisterValuation.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteAllByInvoiceRegisterId(int invoiceRegisterId)
        {
            return InvoiceRegisterValuation.DeleteAllByInvoiceRegisterId(invoiceRegisterId);
        }

        public static void DeleteAllByInvoiceRegisterId(int invoiceRegisterId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByInvoiceRegisterId(invoiceRegisterId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(InvoiceRegisterValuation)));
                }
            }
        }
    }
}
