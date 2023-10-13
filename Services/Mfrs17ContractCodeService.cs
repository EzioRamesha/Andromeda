using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services
{
    public class Mfrs17ContractCodeService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Mfrs17ContractCode)),
                Controller = ModuleBo.ModuleController.Mfrs17ContractCode.ToString()
            };
        }

        public static Expression<Func<Mfrs17ContractCode, Mfrs17ContractCodeBo>> Expression()
        {
            return entity => new Mfrs17ContractCodeBo
            {
                Id = entity.Id,
                CedingCompanyId = entity.CedingCompanyId,
                ModifiedContractCode = entity.ModifiedContractCode,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static Expression<Func<Mfrs17ContractCodeWithDetail, Mfrs17ContractCodeBo>> ExpressionWithDetail()
        {
            return entity => new Mfrs17ContractCodeBo
            {
                Id = entity.ContractCode.Id,
                CedingCompanyId = entity.ContractCode.CedingCompanyId,
                CedantCode = entity.ContractCode.CedingCompany.Code,
                ModifiedContractCode = entity.ContractCode.ModifiedContractCode,
                Mfrs17ContractCodeDetailId = entity.ContractCodeDetail.Id,
                Mfrs17ContractCode = entity.ContractCodeDetail.ContractCode,
                CreatedById = entity.ContractCode.CreatedById,
                UpdatedById = entity.ContractCode.UpdatedById,
            };
        }

        public static Mfrs17ContractCodeBo FormBo(Mfrs17ContractCode entity = null)
        {
            if (entity == null)
                return null;
            return new Mfrs17ContractCodeBo
            {
                Id = entity.Id,
                CedingCompanyId = entity.CedingCompanyId,
                CedingCompanyBo = CedantService.Find(entity.CedingCompanyId, false),
                ModifiedContractCode = entity.ModifiedContractCode,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<Mfrs17ContractCodeBo> FormBos(IList<Mfrs17ContractCode> entities = null)
        {
            if (entities == null)
                return null;
            IList<Mfrs17ContractCodeBo> bos = new List<Mfrs17ContractCodeBo>() { };
            foreach (Mfrs17ContractCode entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static Mfrs17ContractCode FormEntity(Mfrs17ContractCodeBo bo = null)
        {
            if (bo == null)
                return null;
            return new Mfrs17ContractCode
            {
                Id = bo.Id,
                CedingCompanyId = bo.CedingCompanyId,
                ModifiedContractCode = bo.ModifiedContractCode?.Trim(),
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return Mfrs17ContractCode.IsExists(id);
        }

        public static IList<Mfrs17ContractCodeBo> GetByCedingCompanyId(int id)
        {
            return FormBos(Mfrs17ContractCode.GetByCedingCompanyId(id));
        }
        public static IList<Mfrs17ContractCodeBo> Get()
        {
            return FormBos(Mfrs17ContractCode.Get());
        }

        public static Mfrs17ContractCodeBo Find(int id)
        {
            return FormBo(Mfrs17ContractCode.Find(id));
        }

        public static Mfrs17ContractCodeBo FindByCedingCompanyAndModifiedContractCode(int cedingCompanyId, string code)
        {
            return FormBo(Mfrs17ContractCode.FindByCedingCompanyAndModifiedContractCode(cedingCompanyId, code));
        }

        public static Mfrs17ContractCodeBo FindByCedingCompanyModifiedContractCodeId(int cedingCompanyId, string code, int? id = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Mfrs17ContractCodes
                    .Where(q => q.CedingCompanyId == cedingCompanyId)
                    .Where(q => q.ModifiedContractCode.Trim() == code.Trim());

                if (id.HasValue)
                {
                    query = query.Where(q => q.Id != id);
                }

                return FormBo(query.FirstOrDefault());
            }
        }

        public static int CountByCedingCompanyId(int cedingCompanyId)
        {
            return Mfrs17ContractCode.CountByCedingCompanyId(cedingCompanyId);
        }


        public static Result Save(ref Mfrs17ContractCodeBo bo)
        {
            if (!Mfrs17ContractCode.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref Mfrs17ContractCodeBo bo, ref TrailObject trail)
        {
            if (!Mfrs17ContractCode.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref Mfrs17ContractCodeBo bo)
        {
            Mfrs17ContractCode entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                if (bo.ModifiedContractCode != null)
                    result.AddTakenError("Modified Contract Code", bo.ModifiedContractCode.ToString());
                else
                    result.AddTakenError("Modified Contract Code", "empty");
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }
            return result;
        }

        public static Result Create(ref Mfrs17ContractCodeBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref Mfrs17ContractCodeBo bo)
        {
            Result result = Result();

            Mfrs17ContractCode entity = Mfrs17ContractCode.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Modified Contract Code", bo.ModifiedContractCode.ToString());
            }

            if (result.Valid)
            {
                entity.CedingCompanyId = bo.CedingCompanyId;
                entity.ModifiedContractCode = bo.ModifiedContractCode;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref Mfrs17ContractCodeBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(Mfrs17ContractCodeBo bo)
        {
            Mfrs17ContractCodeDetailService.DeleteByMfrs17ContractCodeId(bo.Id);
            Mfrs17ContractCode.Delete(bo.Id);
        }

        public static Result Delete(Mfrs17ContractCodeBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (Mfrs17CellMappingService.CountByMfrs17ContractCodeId(bo.Id) > 0)
            {
                result.AddError("The MFRS17 Contract Code(s) under Modified Contract Code In Use");
            }

            if (result.Valid)
            {
                Mfrs17ContractCodeDetailService.DeleteByMfrs17ContractCodeId(bo.Id); // DO NOT TRAIL
                DataTrail dataTrail = Mfrs17ContractCode.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static bool IsDuplicateCode(Mfrs17ContractCode contractCode)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Mfrs17ContractCodes
                    .Where(q => q.CedingCompanyId == contractCode.CedingCompanyId)
                    .Where(q => q.ModifiedContractCode.Trim().Equals(contractCode.ModifiedContractCode, StringComparison.OrdinalIgnoreCase));
                if (contractCode.Id != 0)
                {
                    query = query.Where(q => q.Id != contractCode.Id);
                }
                return query.Count() > 0;
            }
        }
    }

    public class Mfrs17ContractCodeWithDetail
    {
        public Mfrs17ContractCode ContractCode { get; set; }

        public Mfrs17ContractCodeDetail ContractCodeDetail { get; set; }
    }
}
