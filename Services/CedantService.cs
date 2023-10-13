using BusinessObject;
using DataAccess.Entities;
using DataAccess.Entities.RiDatas;
using DataAccess.EntityFramework;
using Services.SoaDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class CedantService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Cedant)),
                Controller = ModuleBo.ModuleController.Cedant.ToString()
            };
        }

        public static CedantBo FormBo(Cedant entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            return new CedantBo
            {
                Id = entity.Id,
                CedingCompanyTypePickListDetailId = entity.CedingCompanyTypePickListDetailId,
                CedingCompanyTypePickListDetailBo = foreign ? PickListDetailService.Find(entity.CedingCompanyTypePickListDetailId) : null,
                Name = entity.Name,
                Code = entity.Code,
                PartyCode = entity.PartyCode,
                Status = entity.Status,
                Remarks = entity.Remarks,
                AccountCode = entity.AccountCode,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<CedantBo> FormBos(IList<Cedant> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<CedantBo> bos = new List<CedantBo>() { };
            foreach (Cedant entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static Cedant FormEntity(CedantBo bo = null)
        {
            if (bo == null)
                return null;
            return new Cedant
            {
                Id = bo.Id,
                CedingCompanyTypePickListDetailId = bo.CedingCompanyTypePickListDetailId,
                Name = bo.Name?.Trim(),
                Code = bo.Code?.Trim(),
                PartyCode = bo.PartyCode?.Trim(),
                Status = bo.Status,
                Remarks = bo.Remarks?.Trim(),
                AccountCode = bo.AccountCode?.Trim(),
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateCode(Cedant cedant)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(cedant.Code))
                {
                    var query = db.Cedants.Where(q => q.Code.Trim().Equals(cedant.Code.Trim(), StringComparison.OrdinalIgnoreCase));
                    if (cedant.Id != 0)
                    {
                        query = query.Where(q => q.Id != cedant.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static bool IsExists(int id)
        {
            return Cedant.IsExists(id);
        }

        public static CedantBo Find(int? id, bool foreign = true)
        {
            return FormBo(Cedant.Find(id), foreign);
        }

        public static CedantBo FindByCode(string code)
        {
            return FormBo(Cedant.FindByCode(code));
        }

        public static CedantBo FindByName(string name)
        {
            return FormBo(Cedant.FindByName(name));
        }

        public static int Count()
        {
            return Cedant.Count();
        }

        public static int CountByCedingCompanyTypePickListDetailId(int cedingCompanyTypePickListDetailId)
        {
            return Cedant.CountByCedingCompanyTypePickListDetailId(cedingCompanyTypePickListDetailId);
        }

        public static IList<CedantBo> Get()
        {
            return FormBos(Cedant.Get());
        }

        public static IList<CedantBo> Get(int skip, int take)
        {
            return FormBos(Cedant.Get(skip, take));
        }

        public static IList<CedantBo> GetByStatus(int? status = null, int? selectedId = null, string selectedCode = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.Cedants.AsQueryable();
                if (status != null)
                {
                    if (selectedId != null)
                        query = query.Where(q => q.Status == status || q.Id == selectedId);
                    else if (!string.IsNullOrEmpty(selectedCode))
                        query = query.Where(q => q.Status == status || q.Code.Trim() == selectedCode.Trim());
                    else
                        query = query.Where(q => q.Status == status);
                }

                return FormBos(query.OrderBy(q => q.Code).ToList());
            }
        }

        public static IList<CedantBo> GetByStatusWorkgroup(int workgroupUserId, int? status = null, int? selectedId = null, string selectedCode = null)
        {
            using (var db = new AppDbContext())
            {
                IQueryable<int> subQuery = db.CedantWorkgroupCedants
                        .Where(c => db.CedantWorkgroupUsers
                            .Where(u => u.UserId == workgroupUserId)
                            .Select(u => u.CedantWorkgroupId)
                            .Contains(c.CedantWorkgroupId)
                        ).Select(c => c.CedantId);

                var query = db.Cedants.AsQueryable();
                if (status != null)
                {
                    if (selectedId != null)
                        query = query.Where(q => (q.Status == status && subQuery.Contains(q.Id)) || q.Id == selectedId);
                    else if (!string.IsNullOrEmpty(selectedCode))
                        query = query.Where(q => (q.Status == status && subQuery.Contains(q.Id)) || q.Code.Trim() == selectedCode.Trim());
                    else
                        query = query.Where(q => q.Status == status && subQuery.Contains(q.Id));
                }

                return FormBos(query.OrderBy(q => q.Code).ToList());
            }
        }

        public static IList<CedantBo> GetNotInCedantWorkgroup(int cedantWorkgroupId)
        {
            return FormBos(Cedant.GetNotInCedantWorkgroup(cedantWorkgroupId));
        }

        public static Result Save(ref CedantBo bo)
        {
            if (!Cedant.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref CedantBo bo, ref TrailObject trail)
        {
            if (!Cedant.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref CedantBo bo)
        {
            Cedant entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateCode(entity))
            {
                result.AddTakenError("Code", bo.Code);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref CedantBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref CedantBo bo)
        {
            Result result = Result();

            Cedant entity = Cedant.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateCode(FormEntity(bo)))
            {
                result.AddTakenError("Code", bo.Code);
            }

            if (result.Valid)
            {
                entity.CedingCompanyTypePickListDetailId = bo.CedingCompanyTypePickListDetailId;
                entity.Name = bo.Name;
                entity.Code = bo.Code;
                entity.PartyCode = bo.PartyCode;
                entity.Status = bo.Status;
                entity.Remarks = bo.Remarks;
                entity.AccountCode = bo.AccountCode;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref CedantBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(CedantBo bo)
        {
            Cedant.Delete(bo.Id);
        }

        public static Result Delete(CedantBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (
                Treaty.CountByCedantId(bo.Id) > 0 ||
                RiDataConfig.CountByCedantId(bo.Id) > 0 ||
                RiDataBatch.CountByCedantId(bo.Id) > 0 ||
                RiDataCorrection.CountByCedantId(bo.Id) > 0 ||
                TreatyBenefitCodeMapping.CountByCedantId(bo.Id) > 0 ||
                Mfrs17ReportingDetail.CountByCedantId(bo.Id) > 0 ||
                CedantWorkgroupCedant.CountByCedantId(bo.Id) > 0 ||
                FacMasterListing.CountByCedantId(bo.Id) > 0 ||
                RateTable.CountByCedantId(bo.Id) > 0 ||
                DiscountTable.CountByCedantId(bo.Id) > 0 ||
                DirectRetroService.CountByCedantId(bo.Id) > 0 ||
                SoaDataBatchService.CountByCedantId(bo.Id) > 0
            )
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                DataTrail dataTrail = Cedant.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
