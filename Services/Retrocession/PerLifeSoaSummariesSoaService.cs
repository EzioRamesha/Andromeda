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
    public class PerLifeSoaSummariesSoaService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeSoaSummariesSoa)),
                Controller = ModuleBo.ModuleController.PerLifeSoaSummariesSoa.ToString()
            };
        }

        public static PerLifeSoaSummariesSoaBo FormBo(PerLifeSoaSummariesSoa entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new PerLifeSoaSummariesSoaBo
            {
                Id = entity.Id,
                PerLifeSoaId = entity.PerLifeSoaId,
                PerLifeSoaBo = foreign ? PerLifeSoaService.Find(entity.PerLifeSoaId) : null,
                PremiumClaim = entity.PremiumClaim,
                RowLabel = entity.RowLabel,
                Individual = entity.Individual ?? 0,
                Group = entity.Group ?? 0,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                IndividualStr = Util.DoubleToString(entity.Individual ?? 0, 2),
                GroupStr = Util.DoubleToString(entity.Group ?? 0, 2),
            };

            var totalSum = bo.Individual.GetValueOrDefault() + bo.Group.GetValueOrDefault();
            bo.Total = totalSum;
            bo.TotalStr = Util.DoubleToString(totalSum, 2);

            if (bo.PremiumClaim == PerLifeSoaSummariesSoaBo.PremiumClaimPremium)
            {
                var prevQuarter = GetPreviousQuarter(bo.RowLabel);
                if (prevQuarter != null)
                {
                    bo.PrevIndividual = prevQuarter.Individual;
                    bo.PrevGroup = prevQuarter.Group;
                    bo.PrevIndividualStr = Util.DoubleToString(prevQuarter.Individual ?? 0, 2);
                    bo.PrevGroupStr = Util.DoubleToString(prevQuarter.Group ?? 0, 2);
                    var prevTotalSum = prevQuarter.Individual.GetValueOrDefault() + prevQuarter.Group.GetValueOrDefault();
                    bo.PrevTotalStr = Util.DoubleToString(prevTotalSum, 2);
                    bo.PrevTotal = prevTotalSum;
                }
                else
                {
                    bo.PrevIndividual = bo.Individual;
                    bo.PrevGroup = bo.Group;
                    bo.PrevIndividualStr = bo.IndividualStr;
                    bo.PrevGroupStr = bo.GroupStr;
                    bo.PrevTotalStr = bo.TotalStr;
                    bo.PrevTotal = bo.Total;
                }

                var totalMovement = bo.Total.GetValueOrDefault() - bo.PrevTotal.GetValueOrDefault();
                bo.Movement = totalMovement;
                bo.MovementStr = Util.DoubleToString(totalMovement, 2);
            }           

            return bo;
        }

        public static IList<PerLifeSoaSummariesSoaBo> FormBos(IList<PerLifeSoaSummariesSoa> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<PerLifeSoaSummariesSoaBo> bos = new List<PerLifeSoaSummariesSoaBo>() { };
            foreach (PerLifeSoaSummariesSoa entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static bool IsExists(int id)
        {
            return PerLifeSoaSummariesSoa.IsExists(id);
        }

        public static PerLifeSoaSummariesSoaBo Find(int? id)
        {
            return FormBo(PerLifeSoaSummariesSoa.Find(id));
        }

        public static IList<PerLifeSoaSummariesSoaBo> GetByPerLifeSoaId(int perLifeSoaId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeSoaSummariesSoa.Where(q => q.PerLifeSoaId == perLifeSoaId).OrderBy(q => q.Id).ToList());
            }
        }

        public static PerLifeSoaSummariesSoaBo GetPreviousQuarter(string quarter)
        {
            using (var db = new AppDbContext())
            {
                int quarterNo = int.Parse(quarter.Substring(6, 1));
                int year = int.Parse(quarter.Substring(0, 4));
                string prevQuarter = string.Format("{0} Q{1}", (quarterNo == 1 ? year - 1 : year), (quarterNo == 1 ? 4 : quarterNo - 1));

                var query = db.PerLifeSoaSummariesSoa
                    .Where(q => q.PremiumClaim == PerLifeSoaSummariesSoaBo.PremiumClaimPremium)
                    .Where(q => q.RowLabel == prevQuarter);

                return FormBo(query.FirstOrDefault());
            }
        }
    }
}
