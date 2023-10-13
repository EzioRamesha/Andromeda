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
    public class PerLifeSoaSummariesService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PerLifeSoaSummaries)),
                Controller = ModuleBo.ModuleController.PerLifeSoaSummaries.ToString()
            };
        }

        public static PerLifeSoaSummariesBo FormBo(PerLifeSoaSummaries entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            var bo = new PerLifeSoaSummariesBo
            {
                Id = entity.Id,
                PerLifeSoaId = entity.PerLifeSoaId,
                PerLifeSoaBo = foreign ? PerLifeSoaService.Find(entity.PerLifeSoaId) : null,
                RowLabel = entity.RowLabel,
                WMOM = entity.WMOM,
                Automatic = entity.Automatic,
                Facultative = entity.Facultative,
                Advantage = entity.Advantage,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,

                AutomaticStr = Util.DoubleToString(entity.Automatic, 2),
                FacultativeStr = Util.DoubleToString(entity.Facultative, 2),
                AdvantageStr = Util.DoubleToString(entity.Advantage, 2)
            };

            var totalSum = bo.Automatic + bo.Facultative + bo.Advantage;
            bo.Total = totalSum;
            bo.TotalStr = Util.DoubleToString(totalSum, 2);
            return bo;
        }

        public static IList<PerLifeSoaSummariesBo> FormBos(IList<PerLifeSoaSummaries> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<PerLifeSoaSummariesBo> bos = new List<PerLifeSoaSummariesBo>() { };
            foreach (PerLifeSoaSummaries entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static bool IsExists(int id)
        {
            return PerLifeSoaSummaries.IsExists(id);
        }

        public static PerLifeSoaSummariesBo Find(int? id)
        {
            return FormBo(PerLifeSoaSummaries.Find(id));
        }

        public static IList<PerLifeSoaSummariesBo> GetByPerLifeSoaId(int perLifeSoaId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.PerLifeSoaSummaries.Where(q => q.PerLifeSoaId == perLifeSoaId).OrderBy(q => q.Id).ToList());
            }
        }
    }
}
