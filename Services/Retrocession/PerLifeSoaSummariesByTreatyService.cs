using BusinessObject;
using BusinessObject.Retrocession;
using DataAccess.Entities.Retrocession;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace Services.Retrocession
{
    public class PerLifeSoaSummariesByTreatyService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeSoaSummariesByTreaty)),
                Controller = ModuleBo.ModuleController.PerLifeSoaSummariesByTreaty.ToString()
            };
        }

        public static PerLifeSoaSummariesByTreatyBo FormBo(PerLifeSoaSummariesByTreaty entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new PerLifeSoaSummariesByTreatyBo
            {
                Id = entity.Id,
                PerLifeSoaId = entity.PerLifeSoaId,
                PerLifeSoaBo = foreign ? PerLifeSoaService.Find(entity.PerLifeSoaId) : null,
                TreatyCode = entity.TreatyCode,
                ProcessingPeriod = entity.ProcessingPeriod,
                TotalRetroAmount = entity.TotalRetroAmount,
                TotalGrossPremium = entity.TotalGrossPremium,
                TotalNetPremium = entity.TotalNetPremium,
                TotalDiscount = entity.TotalDiscount,
                TotalPolicyCount = entity.TotalPolicyCount,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                TotalRetroAmountStr = Util.DoubleToString(entity.TotalRetroAmount ?? 0, 2),
                TotalGrossPremiumStr = Util.DoubleToString(entity.TotalGrossPremium ?? 0, 2),
                TotalNetPremiumStr = Util.DoubleToString(entity.TotalNetPremium ?? 0, 2),
                TotalDiscountStr = Util.DoubleToString(entity.TotalDiscount ?? 0, 2),
            };

            if (!string.IsNullOrEmpty(bo.ProcessingPeriod))
                bo.ProcessingPeriodYear = bo.ProcessingPeriod.Substring(0, 4);

            var prevQuarter = GetPreviousQuarter(bo.TreatyCode, bo.ProcessingPeriod);
            if (prevQuarter != null)
            {
                bo.PrevTotalRetroAmount = prevQuarter.TotalRetroAmount;
                bo.PrevTotalGrossPremium = prevQuarter.TotalGrossPremium;
                bo.PrevTotalNetPremium = prevQuarter.TotalNetPremium;
                bo.PrevTotalDiscount = prevQuarter.TotalDiscount;
                bo.PrevTotalPolicyCount = prevQuarter.TotalPolicyCount;
                bo.PrevTotalRetroAmountStr = Util.DoubleToString(prevQuarter.TotalRetroAmount ?? 0, 2);
                bo.PrevTotalGrossPremiumStr = Util.DoubleToString(prevQuarter.TotalGrossPremium ?? 0, 2);
                bo.PrevTotalDiscountStr = Util.DoubleToString(prevQuarter.PrevTotalDiscount ?? 0, 2);
                bo.PrevTotalNetPremiumStr = Util.DoubleToString(prevQuarter.TotalNetPremium, 2);
            }
            else
            {
                bo.PrevTotalRetroAmount = 0;
                bo.PrevTotalGrossPremium = 0;
                bo.PrevTotalNetPremium = 0;
                bo.PrevTotalDiscount = 0;
                bo.PrevTotalPolicyCount = 0;
                bo.PrevTotalRetroAmountStr = "0.00";
                bo.PrevTotalGrossPremiumStr = "0.00";
                bo.PrevTotalDiscountStr = "0.00";
                bo.PrevTotalNetPremiumStr = "0.00";
            }

            var totalRetroAmountMovement = bo.TotalRetroAmount.GetValueOrDefault() - bo.PrevTotalRetroAmount.GetValueOrDefault();
            var totalGrossPremiumMovement = bo.TotalGrossPremium.GetValueOrDefault() - bo.PrevTotalGrossPremium.GetValueOrDefault();
            var totalNetPremiumMovement = bo.TotalNetPremium.GetValueOrDefault() - bo.PrevTotalNetPremium.GetValueOrDefault();
            var totalDiscountMovement = bo.TotalDiscount.GetValueOrDefault() - bo.PrevTotalDiscount.GetValueOrDefault();
            var totalPolicyCountMovement = bo.TotalPolicyCount.GetValueOrDefault() - bo.PrevTotalPolicyCount.GetValueOrDefault();

            bo.MovementTotalRetroAmount = totalRetroAmountMovement;
            bo.MovementTotalGrossPremium = totalGrossPremiumMovement;
            bo.MovementTotalNetPremium = totalNetPremiumMovement;
            bo.MovementTotalDiscount = totalDiscountMovement;
            bo.MovementTotalPolicyCount = totalPolicyCountMovement;
            bo.MovementTotalRetroAmountStr = Util.DoubleToString(totalRetroAmountMovement, 2);
            bo.MovementTotalGrossPremiumStr = Util.DoubleToString(totalGrossPremiumMovement, 2);
            bo.MovementTotalNetPremiumStr = Util.DoubleToString(totalNetPremiumMovement, 2);
            bo.MovementTotalDiscountStr = Util.DoubleToString(totalDiscountMovement, 2);

            return bo;
        }

        public static IList<PerLifeSoaSummariesByTreatyBo> FormBos(IList<PerLifeSoaSummariesByTreaty> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<PerLifeSoaSummariesByTreatyBo> bos = new List<PerLifeSoaSummariesByTreatyBo>() { };
            foreach (PerLifeSoaSummariesByTreaty entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static bool IsExists(int id)
        {
            return PerLifeSoaSummariesByTreaty.IsExists(id);
        }

        public static PerLifeSoaSummariesByTreatyBo Find(int? id)
        {
            return FormBo(PerLifeSoaSummariesByTreaty.Find(id));
        }

        public static IList<PerLifeSoaSummariesByTreatyBo> GetByPerLifeSoaId(int perLifeSoaId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeSoaSummariesByTreaty.Where(q => q.PerLifeSoaId == perLifeSoaId).OrderBy(q => q.Id).ToList());
            }
        }

        public static PerLifeSoaSummariesByTreatyBo GetPreviousQuarter(string treatyCode, string quarter)
        {
            using (var db = new AppDbContext())
            {
                int quarterNo = int.Parse(quarter.Substring(6, 1));
                int year = int.Parse(quarter.Substring(0, 4));
                string prevQuarter = string.Format("{0} Q{1}", (quarterNo == 1 ? year - 1 : year), (quarterNo == 1 ? 4 : quarterNo - 1));

                var query = db.PerLifeSoaSummariesByTreaty
                    .Where(q => q.TreatyCode == treatyCode)
                    .Where(q => q.ProcessingPeriod == prevQuarter);

                return FormBo(query.FirstOrDefault());
            }
        }
    }
}
