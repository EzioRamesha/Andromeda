using BusinessObject;
using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Retrocession
{
    public class RetroTreatyService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(RetroTreaty)),
                Controller = ModuleBo.ModuleController.RetroTreaty.ToString()
            };
        }

        public static Expression<Func<RetroTreatyWithDetail, RetroTreatyBo>> ExpressionWithDetail()
        {
            return entity => new RetroTreatyBo
            {
                Id = entity.Treaty.Id,
                RetroPartyId = entity.Treaty.RetroPartyId,
                RetroPartyParty = entity.Treaty.RetroParty.Party,
                Status = entity.Treaty.Status,
                Code = entity.Treaty.Code,
                TreatyTypePickListDetailId = entity.Treaty.TreatyTypePickListDetailId,
                TreatyType = entity.Treaty.TreatyTypePickListDetail.Code,
                IsLobAutomatic = entity.Treaty.IsLobAutomatic,
                IsLobFacultative = entity.Treaty.IsLobFacultative,
                IsLobAdvantageProgram = entity.Treaty.IsLobAdvantageProgram,
                RetroShare = entity.Treaty.RetroShare,
                TreatyDiscountTableId = entity.Treaty.TreatyDiscountTableId,
                TreatyDiscountRule = entity.Treaty.TreatyDiscountTable.Rule,
                EffectiveStartDate = entity.Treaty.EffectiveStartDate,
                EffectiveEndDate = entity.Treaty.EffectiveEndDate,

                DetailId = entity.Detail.Id,
                // PerLifeRetroConfigurationTreaty
                DetailConfigTreatyCode = entity.Detail.PerLifeRetroConfigurationTreaty.TreatyCode.Code,
                DetailConfigTreatyType = entity.Detail.PerLifeRetroConfigurationTreaty.TreatyTypePickListDetail.Code,
                DetailConfigFundsAccountingType = entity.Detail.PerLifeRetroConfigurationTreaty.FundsAccountingTypePickListDetail.Code,
                DetailConfigEffectiveStartDate = entity.Detail.PerLifeRetroConfigurationTreaty.ReinsEffectiveStartDate,
                DetailConfigEffectiveEndDate = entity.Detail.PerLifeRetroConfigurationTreaty.ReinsEffectiveEndDate,
                DetailConfigRiskQuarterStartDate = entity.Detail.PerLifeRetroConfigurationTreaty.RiskQuarterStartDate,
                DetailConfigRiskQuarterEndDate = entity.Detail.PerLifeRetroConfigurationTreaty.RiskQuarterEndDate,
                DetailConfigIsToAggregate = entity.Detail.PerLifeRetroConfigurationTreaty.IsToAggregate,
                DetailConfigRemark = entity.Detail.PerLifeRetroConfigurationTreaty.Remark,
                // End
                DetailPremiumSpreadRule = entity.Detail.PremiumSpreadTable.Rule,
                DetailTreatyDiscountRule = entity.Detail.TreatyDiscountTable.Rule,
                DetailMlreShare = entity.Detail.MlreShare,
                DetailGrossRetroPremium = entity.Detail.GrossRetroPremium,
                DetailTreatyDiscount = entity.Detail.TreatyDiscount,
                DetailNetRetroPremium = entity.Detail.NetRetroPremium,
                DetailRemark = entity.Detail.Remark,

                CreatedById = entity.Treaty.CreatedById,
                UpdatedById = entity.Treaty.UpdatedById,
            };
        }

        public static RetroTreatyBo FormBo(RetroTreaty entity = null)
        {
            if (entity == null)
                return null;
            return new RetroTreatyBo
            {
                Id = entity.Id,
                RetroPartyId = entity.RetroPartyId,
                RetroPartyBo = RetroPartyService.Find(entity.RetroPartyId),
                Status = entity.Status,
                //Party = entity.Party,
                Code = entity.Code,
                TreatyTypePickListDetailId = entity.TreatyTypePickListDetailId,
                TreatyTypePickListDetailBo = PickListDetailService.Find(entity.TreatyTypePickListDetailId),
                IsLobAutomatic = entity.IsLobAutomatic,
                IsLobFacultative = entity.IsLobFacultative,
                IsLobAdvantageProgram = entity.IsLobAdvantageProgram,
                RetroShare = entity.RetroShare,
                TreatyDiscountTableId = entity.TreatyDiscountTableId,
                TreatyDiscountTableBo = TreatyDiscountTableService.Find(entity.TreatyDiscountTableId),
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveEndDate = entity.EffectiveEndDate,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<RetroTreatyBo> FormBos(IList<RetroTreaty> entities = null)
        {
            if (entities == null)
                return null;
            IList<RetroTreatyBo> bos = new List<RetroTreatyBo>() { };
            foreach (RetroTreaty entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static RetroTreaty FormEntity(RetroTreatyBo bo = null)
        {
            if (bo == null)
                return null;
            return new RetroTreaty
            {
                Id = bo.Id,
                RetroPartyId = bo.RetroPartyId,
                Status = bo.Status,
                //Party = bo.Party,
                Code = bo.Code,
                TreatyTypePickListDetailId = bo.TreatyTypePickListDetailId,
                IsLobAutomatic = bo.IsLobAutomatic,
                IsLobFacultative = bo.IsLobFacultative,
                IsLobAdvantageProgram = bo.IsLobAdvantageProgram,
                RetroShare = bo.RetroShare,
                TreatyDiscountTableId = bo.TreatyDiscountTableId,
                EffectiveStartDate = bo.EffectiveStartDate,
                EffectiveEndDate = bo.EffectiveEndDate,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateParty(RetroTreaty retroTreaty)
        {
            return retroTreaty.IsDuplicateParty();
        }

        public static bool IsExists(int id)
        {
            return RetroTreaty.IsExists(id);
        }

        public static RetroTreatyBo Find(int? id)
        {
            return FormBo(RetroTreaty.Find(id));
        }

        public static RetroTreatyBo FindByParams(RetroTreatyBo retroTreatyBo)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.RetroTreaties
                    .Where(q => q.RetroPartyId == retroTreatyBo.RetroPartyId)
                    .Where(q => q.Status == retroTreatyBo.Status)
                    .Where(q => q.Code == retroTreatyBo.Code)
                    .Where(q => q.TreatyTypePickListDetailId == retroTreatyBo.TreatyTypePickListDetailId)
                    .Where(q => q.IsLobAutomatic == retroTreatyBo.IsLobAutomatic)
                    .Where(q => q.IsLobFacultative == retroTreatyBo.IsLobFacultative)
                    .Where(q => q.IsLobAdvantageProgram == retroTreatyBo.IsLobAdvantageProgram)
                    .Where(q => q.RetroShare == retroTreatyBo.RetroShare)
                    .Where(q => q.TreatyDiscountTableId == retroTreatyBo.TreatyDiscountTableId)
                    .Where(q => q.EffectiveStartDate == retroTreatyBo.EffectiveStartDate)
                    .Where(q => q.EffectiveEndDate == retroTreatyBo.EffectiveEndDate)
                    .FirstOrDefault());
            }
        }

        public static IList<RetroTreatyBo> GetByStatus(int? status = null, int? selectedId = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.RetroTreaties.AsQueryable();

                if (status != null)
                {
                    if (selectedId != null)
                        query = query.Where(q => q.Status == status || q.Id == selectedId);
                    else
                        query = query.Where(q => q.Status == status);
                }

                return FormBos(query.OrderBy(q => q.Code).ToList());
            }
        }

        public static IList<RetroTreatyBo> GetByRetroParty(int? retroPartyId)
        {
            using (var db = new AppDbContext())
            {
                if (retroPartyId != null)
                {
                    var query = db.RetroTreaties.Where(q => q.RetroPartyId == retroPartyId);
                    return FormBos(query.OrderBy(q => q.Code).ToList());
                }
                return FormBos();
            }
        }

        public static Result Save(ref RetroTreatyBo bo)
        {
            if (!RetroTreaty.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref RetroTreatyBo bo, ref TrailObject trail)
        {
            if (!RetroTreaty.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref RetroTreatyBo bo)
        {
            RetroTreaty entity = FormEntity(bo);

            Result result = Result();
            //if (IsDuplicateParty(entity))
            //{
            //    result.AddTakenError("Party", bo.Party);
            //}

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref RetroTreatyBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref RetroTreatyBo bo)
        {
            Result result = Result();

            RetroTreaty entity = RetroTreaty.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            entity = FormEntity(bo);
            //if (IsDuplicateParty(entity))
            //{
            //    result.AddTakenError("Party", bo.Party);
            //}

            if (result.Valid)
            {
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref RetroTreatyBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(RetroTreatyBo bo)
        {
            RetroTreaty.Delete(bo.Id);
        }

        public static Result Delete(RetroTreatyBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (PerLifeClaimRetroDataService.CountByRetroTreatyId(bo.Id) > 0 ||
                PerLifeSoaService.CountByRetroTreatyId(bo.Id) > 0)
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                RetroTreatyDetailService.DeleteByRetroTreatyId(bo.Id, ref trail);
                DataTrail dataTrail = RetroTreaty.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static int CountByTreatyDiscountTableById(int treatyDiscountTableId)
        {
            return RetroTreaty.CountByTreatyDiscountTableById(treatyDiscountTableId);
        }
    }

    public class RetroTreatyWithDetail
    {
        public RetroTreaty Treaty { get; set; }

        public RetroTreatyDetail Detail { get; set; }
    }
}
