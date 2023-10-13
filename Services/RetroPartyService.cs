using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Services.Retrocession;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class RetroPartyService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RetroParty)),
                Controller = ModuleBo.ModuleController.RetroParty.ToString()
            };
        }

        public static RetroPartyBo FormBo(RetroParty entity = null)
        {
            if (entity == null)
                return null;
            return new RetroPartyBo
            {
                Id = entity.Id,
                Party = entity.Party,
                Code = entity.Code,
                Name = entity.Name,
                CountryCodePickListDetailId = entity.CountryCodePickListDetailId,
                CountryCodePickListDetailBo = PickListDetailService.Find(entity.CountryCodePickListDetailId),
                Description = entity.Description,
                Status = entity.Status,
                IsDirectRetro = entity.IsDirectRetro,
                IsPerLifeRetro = entity.IsPerLifeRetro,
                AccountCode = entity.AccountCode,
                AccountCodeDescription = entity.AccountCodeDescription,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RetroPartyBo> FormBos(IList<RetroParty> entities = null)
        {
            if (entities == null)
                return null;
            IList<RetroPartyBo> bos = new List<RetroPartyBo>() { };
            foreach (RetroParty entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RetroParty FormEntity(RetroPartyBo bo = null)
        {
            if (bo == null)
                return null;
            return new RetroParty
            {
                Id = bo.Id,
                Party = bo.Party?.Trim(),
                Code = bo.Code?.Trim(),
                Name = bo.Name?.Trim(),
                CountryCodePickListDetailId = bo.CountryCodePickListDetailId,
                Description = bo.Description?.Trim(),
                Status = bo.Status,
                IsDirectRetro = bo.IsDirectRetro,
                IsPerLifeRetro = bo.IsPerLifeRetro,
                AccountCode = bo.AccountCode?.Trim(),
                AccountCodeDescription = bo.AccountCodeDescription?.Trim(),
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateParty(RetroParty retroParty)
        {
            return retroParty.IsDuplicateParty();
        }

        public static bool IsExists(int id)
        {
            return RetroParty.IsExists(id);
        }

        public static RetroPartyBo Find(int? id)
        {
            return FormBo(RetroParty.Find(id));
        }

        public static RetroPartyBo FindByCode(string code)
        {
            return FormBo(RetroParty.FindByCode(code));
        }

        public static RetroPartyBo FindByParty(string party)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.RetroParties.Where(q => q.Party == party).FirstOrDefault());
            }
        }

        public static int CountByCodeStatus(string code, int? status = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroParties.Where(q => q.Code == code);
                if (status != null)
                    query = query.Where(q => q.Status == status);
                return query.Count();
            }
        }

        public static int CountByPartyStatus(string party, int? status = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroParties.Where(q => q.Party == party);
                if (status != null)
                    query = query.Where(q => q.Status == status);
                return query.Count();
            }
        }

        public static int CountByCountryCodePickListDetailId(int countryCodePickListDetailId)
        {
            return RetroParty.CountByCountryCodePickListDetailId(countryCodePickListDetailId);
        }

        public static IList<RetroPartyBo> Get()
        {
            return FormBos(RetroParty.Get());
        }

        public static IList<RetroPartyBo> GetByStatus(int? status = null, int? selectedId = null)
        {
            return FormBos(RetroParty.GetByStatus(status, selectedId));
        }

        public static IList<RetroPartyBo> GetDirectRetro()
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroParties.Where(q => q.IsDirectRetro);
                return FormBos(query.ToList());
            }
        }

        public static Result Save(ref RetroPartyBo bo)
        {
            if (!RetroParty.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref RetroPartyBo bo, ref TrailObject trail)
        {
            if (!RetroParty.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RetroPartyBo bo)
        {
            RetroParty entity = FormEntity(bo);

            Result result = Result();
            if (IsDuplicateParty(entity))
            {
                result.AddTakenError("Party", bo.Party);
            }

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RetroPartyBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RetroPartyBo bo)
        {
            Result result = Result();

            RetroParty entity = RetroParty.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            if (IsDuplicateParty(FormEntity(bo)))
            {
                result.AddTakenError("Party", bo.Party);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.Party = bo.Party;
                entity.Code = bo.Code;
                entity.Name = bo.Name;
                entity.CountryCodePickListDetailId = bo.CountryCodePickListDetailId;
                entity.Description = bo.Description;
                entity.Status = bo.Status;
                entity.IsDirectRetro = bo.IsDirectRetro;
                entity.IsPerLifeRetro = bo.IsPerLifeRetro;
                entity.AccountCode = bo.AccountCode;
                entity.AccountCodeDescription = bo.AccountCodeDescription;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RetroPartyBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RetroPartyBo bo)
        {
            RetroParty.Delete(bo.Id);
        }

        public static Result Delete(RetroPartyBo bo, ref TrailObject trail)
        {
            Result result = Result();
            if (DirectRetroConfigurationDetailService.CountByRetroPartyId(bo.Id) > 0 ||
                RetroStatementService.CountByRetroPartyId(bo.Id) > 0 ||
                RetroRegisterService.CountByRetroPartyId(bo.Id) > 0 ||
                RetroRegisterHistory.CountByRetroPartyId(bo.Id) > 0 ||
                PerLifeSoaService.CountByRetroPartyId(bo.Id) > 0 ||
                PerLifeRetroStatementService.CountByRetroPartyId(bo.Id) > 0)
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                DataTrail dataTrail = RetroParty.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
