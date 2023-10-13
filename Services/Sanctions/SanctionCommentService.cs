using BusinessObject;
using BusinessObject.Sanctions;
using DataAccess.Entities.Sanctions;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Sanctions
{
    public class SanctionCommentService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(SanctionComment)),
                Controller = ModuleBo.ModuleController.SanctionComment.ToString()
            };
        }

        public static Expression<Func<SanctionComment, SanctionCommentBo>> Expression()
        {
            return entity => new SanctionCommentBo
            {
                Id = entity.Id,
                SanctionId = entity.SanctionId,
                Comment = entity.Comment,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static SanctionCommentBo FormBo(SanctionComment entity = null)
        {
            if (entity == null)
                return null;
            return new SanctionCommentBo
            {
                Id = entity.Id,
                SanctionId = entity.SanctionId,
                Comment = entity.Comment,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<SanctionCommentBo> FormBos(IList<SanctionComment> entities = null)
        {
            if (entities == null)
                return null;
            IList<SanctionCommentBo> bos = new List<SanctionCommentBo>() { };
            foreach (SanctionComment entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static SanctionComment FormEntity(SanctionCommentBo bo = null)
        {
            if (bo == null)
                return null;
            return new SanctionComment
            {
                Id = bo.Id,
                SanctionId = bo.SanctionId,
                Comment = bo.Comment,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return SanctionComment.IsExists(id);
        }

        public static SanctionCommentBo Find(int id)
        {
            return FormBo(SanctionComment.Find(id));
        }

        public static IList<SanctionCommentBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionComments.ToList());
            }
        }

        public static IList<SanctionCommentBo> GetBySanctionId(int sanctionId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.SanctionComments.Where(q => q.SanctionId == sanctionId).ToList());
            }
        }

        public static int CountMaxRowBySanctionBatchId(int sanctionBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.SanctionComments
                    .Where(q => q.Sanction.SanctionBatchId == sanctionBatchId)
                    .GroupBy(q => q.SanctionId)
                    .Max(q => (int?)q.Count()) ?? 0;
            }
        }

        public static Result Save(ref SanctionCommentBo bo)
        {
            if (!SanctionComment.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref SanctionCommentBo bo, ref TrailObject trail)
        {
            if (!SanctionComment.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SanctionCommentBo bo)
        {
            SanctionComment entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref SanctionCommentBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SanctionCommentBo bo)
        {
            Result result = Result();

            SanctionComment entity = SanctionComment.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.SanctionId = bo.SanctionId;
                entity.Comment = bo.Comment;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SanctionCommentBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SanctionCommentBo bo)
        {
            SanctionComment.Delete(bo.Id);
        }

        public static Result Delete(SanctionCommentBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                DataTrail dataTrail = SanctionComment.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteBySanctionId(int sanctionId)
        {
            return SanctionComment.DeleteBySanctionId(sanctionId);
        }

        public static void DeleteBySanctionId(int sanctionId, ref TrailObject trail)
        {
            Result result = Result();
            IList<DataTrail> dataTrails = DeleteBySanctionId(sanctionId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, result.Table);
                }
            }
        }

        public static IList<DataTrail> DeleteBySanctionBatchId(int sanctionBatchId)
        {
            return SanctionComment.DeleteBySanctionBatchId(sanctionBatchId);
        }

        public static void DeleteBySanctionBatchId(int sanctionBatchId, ref TrailObject trail)
        {
            Result result = Result();
            IList<DataTrail> dataTrails = DeleteBySanctionBatchId(sanctionBatchId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, result.Table);
                }
            }
        }
    }
}
