using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.TreatyPricing
{
    public class TreatyPricingGroupMasterLetterService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyPricingGroupMasterLetter)),
                Controller = ModuleBo.ModuleController.TreatyPricingGroupMasterLetter.ToString()
            };
        }

        public static TreatyPricingGroupMasterLetterBo FormBo(TreatyPricingGroupMasterLetter entity = null)
        {
            if (entity == null)
                return null;
            return new TreatyPricingGroupMasterLetterBo
            {
                Id = entity.Id,
                Code = entity.Code,
                CedantId = entity.CedantId,
                CedantBo = CedantService.Find(entity.CedantId),
                NoOfRiGroupSlip = entity.NoOfRiGroupSlip,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<TreatyPricingGroupMasterLetterBo> FormBos(IList<TreatyPricingGroupMasterLetter> entities = null)
        {
            if (entities == null)
                return null;
            IList<TreatyPricingGroupMasterLetterBo> bos = new List<TreatyPricingGroupMasterLetterBo>() { };
            foreach (TreatyPricingGroupMasterLetter entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static TreatyPricingGroupMasterLetter FormEntity(TreatyPricingGroupMasterLetterBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyPricingGroupMasterLetter
            {
                Id = bo.Id,
                Code = bo.Code,
                CedantId = bo.CedantId,
                NoOfRiGroupSlip = bo.NoOfRiGroupSlip,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsDuplicateCode(TreatyPricingGroupMasterLetter TreatyPricingGroupMasterLetter)
        {
            using (var db = new AppDbContext())
            {
                if (TreatyPricingGroupMasterLetter.CedantId != 0)
                {
                    var query = db.TreatyPricingGroupMasterLetters
                        .Where(q => q.CedantId == TreatyPricingGroupMasterLetter.CedantId)
                        .Where(q => q.CreatedAt.Year == DateTime.Now.Year);

                    if (TreatyPricingGroupMasterLetter.Id != 0)
                    {
                        query = query.Where(q => q.Id != TreatyPricingGroupMasterLetter.Id);
                    }
                    return query.Count() > 0;
                }
                return false;
            }
        }

        public static bool IsExists(int id)
        {
            return TreatyPricingGroupMasterLetter.IsExists(id);
        }

        public static TreatyPricingGroupMasterLetterBo Find(int? id)
        {
            return FormBo(TreatyPricingGroupMasterLetter.Find(id));
        }

        public static TreatyPricingGroupMasterLetterBo FindByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                return FormBo(db.TreatyPricingGroupMasterLetters.Where(q => q.Code == code).FirstOrDefault());
            }
        }

        public static IList<TreatyPricingGroupMasterLetterBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.TreatyPricingGroupMasterLetters.ToList());
            }
        }

        public static string GetNextCodeNo(int year, string cedantCode)
        {
            using (var db = new AppDbContext())
            {
                string prefix = string.Format("{0}-GRP-ML-{1}-", cedantCode, year);

                var treatyPricingGroupMasterLetter = db.TreatyPricingGroupMasterLetters.Where(q => q.CreatedAt.Year == year)
                    .Where(q => q.Cedant.Code == cedantCode)
                    .Where(q => !string.IsNullOrEmpty(q.Code) && q.Code.StartsWith(prefix))
                    .OrderByDescending(a => a.Code.Length)
                    .ThenByDescending(a => a.Code)
                    .FirstOrDefault();

                int count = 0;
                if (treatyPricingGroupMasterLetter != null)
                {
                    string referenceNo = treatyPricingGroupMasterLetter.Code;
                    string[] referenceNoInfo = referenceNo.Split('-');

                    if (referenceNoInfo.Length == 5)
                    {
                        string countStr = referenceNoInfo[4];
                        int.TryParse(countStr, out count);
                    }
                }
                count++;
                string nextCountStr = count.ToString();

                return prefix + nextCountStr.PadLeft(3, '0');
            }
        }

        public static Result Save(ref TreatyPricingGroupMasterLetterBo bo)
        {
            if (!TreatyPricingGroupMasterLetter.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref TreatyPricingGroupMasterLetterBo bo, ref TrailObject trail)
        {
            if (!TreatyPricingGroupMasterLetter.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref TreatyPricingGroupMasterLetterBo bo)
        {
            TreatyPricingGroupMasterLetter entity = FormEntity(bo);

            Result result = Result();
            //if (IsDuplicateCode(entity))
            //{
            //    result.AddTakenError("Ceding Company", bo.CedantBo.Code);
            //}

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref TreatyPricingGroupMasterLetterBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref TreatyPricingGroupMasterLetterBo bo)
        {
            Result result = Result();

            TreatyPricingGroupMasterLetter entity = TreatyPricingGroupMasterLetter.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }
            //if (IsDuplicateCode(FormEntity(bo)))
            //{
            //    result.AddTakenError("Ceding Company", bo.CedantBo.Code);
            //}

            if (result.Valid)
            {
                entity.Code = bo.Code;
                entity.CedantId = bo.CedantId;
                entity.NoOfRiGroupSlip = bo.NoOfRiGroupSlip;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref TreatyPricingGroupMasterLetterBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyPricingGroupMasterLetterBo bo)
        {
            TreatyPricingGroupMasterLetter.Delete(bo.Id);
        }

        public static Result Delete(TreatyPricingGroupMasterLetterBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                TreatyPricingGroupReferralService.UpdateGroupMasterLetterIdExcept(bo.Id, null, ref trail);
                TreatyPricingGroupMasterLetterGroupReferralService.DeleteByGroupMasterLetterId(bo.Id, ref trail);
                DataTrail dataTrail = TreatyPricingGroupMasterLetter.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }
    }
}
