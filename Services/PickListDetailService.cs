using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class PickListDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(PickListDetail)),
            };
        }

        public static PickListDetailBo FormBo(PickListDetail entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;
            return new PickListDetailBo
            {
                Id = entity.Id,
                PickListId = entity.PickListId,
                PickListBo = foreign ? PickListService.Find(entity.PickListId) : null,
                SortIndex = entity.SortIndex,
                Code = entity.Code,
                Description = entity.Description,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<PickListDetailBo> FormBos(IList<PickListDetail> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<PickListDetailBo> bos = new List<PickListDetailBo>() { };
            foreach (PickListDetail entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static PickListDetail FormEntity(PickListDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new PickListDetail
            {
                Id = bo.Id,
                PickListId = bo.PickListId,
                SortIndex = bo.SortIndex,
                Code = bo.Code?.Trim(),
                Description = bo.Description?.Trim(),
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return PickListDetail.IsExists(id);
        }

        public static PickListDetailBo Find(int id)
        {
            return FormBo(PickListDetail.Find(id));
        }

        public static PickListDetailBo Find(int? id, bool foreign = true)
        {
            if (id == null)
                return null;
            return FormBo(PickListDetail.Find(id.Value), foreign);
        }

        public static PickListDetailBo FindByPickListIdCode(int pickListId, string code)
        {
            return FormBo(PickListDetail.FindByPickListIdCode(pickListId, code));
        }

        public static PickListDetailBo FindByStandardOutputIdCode(int standardOutputId, string code)
        {
            return FormBo(PickListDetail.FindByStandardOutputIdCode(standardOutputId, code));
        }

        public static PickListDetailBo FindByStandardClaimDataOutputIdCode(int standardClaimDataOutputId, string code)
        {
            return FormBo(PickListDetail.FindByStandardClaimDataOutputIdCode(standardClaimDataOutputId, code));
        }

        public static string GetNameByPickListIdCode(int pickListId, string code)
        {
            var bo = FormBo(PickListDetail.FindByPickListIdCode(pickListId, code));
            if (bo != null)
            {
                return bo.ToString();
            }
            return "";
        }

        public static int CountByStandardOutputIdCode(int standardOutputId, string code)
        {
            return PickListDetail.CountByStandardOutputIdCode(standardOutputId, code);
        }

        public static int CountByStandardClaimDataOutputIdCode(int standardClaimDataOutputId, string code)
        {
            using (var db = new AppDbContext())
            {
                return db.PickListDetails
                    .Where(q => q.PickList.StandardClaimDataOutputId == standardClaimDataOutputId)
                    .Where(q => q.Code.Trim() == code.Trim())
                    .Count();
            }
        }

        public static int CountByPickListIdSortIndex(int pickListId, int sortIndex)
        {
            return PickListDetail.CountByPickListIdSortIndex(pickListId, sortIndex);
        }

        public static IList<PickListDetailBo> GetByPickListId(int pickListId)
        {
            return FormBos(PickListDetail.GetByPickListId(pickListId));
        }

        public static IList<PickListDetailBo> GetByStandardOutputId(int standardOutputId, bool foreign = true)
        {
            return FormBos(PickListDetail.GetByStandardOutputId(standardOutputId), foreign);
        }

        public static IList<string> GetCodeByType(int type)
        {
            using (var db = new AppDbContext())
            {
                return db.PickListDetails.Where(q => q.PickListId == type).Select(q => q.Code ?? q.Description).ToList();
            }
        }

        public static IList<PickListDetailBo> GetByStandardClaimDataOutputId(int standardClaimDataOutputId)
        {
            return FormBos(PickListDetail.GetByStandardClaimDataOutputId(standardClaimDataOutputId));
        }

        public static Result Save(PickListDetailBo bo)
        {
            if (!PickListDetail.IsExists(bo.Id))
            {
                return Create(bo);
            }
            return Update(bo);
        }

        public static Result Save(PickListDetailBo bo, ref TrailObject trail)
        {
            if (!PickListDetail.IsExists(bo.Id))
            {
                return Create(bo, ref trail);
            }
            return Update(bo, ref trail);
        }

        public static Result Create(PickListDetailBo bo)
        {
            PickListDetail entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(PickListDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(PickListDetailBo bo)
        {
            Result result = Result();

            PickListDetail entity = PickListDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.PickListId = bo.PickListId;
                entity.SortIndex = bo.SortIndex;
                entity.Code = bo.Code;
                entity.Description = bo.Description;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(PickListDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(PickListDetailBo bo)
        {
            PickListDetail.Delete(bo.Id);
        }

        public static Result Delete(PickListDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = PickListDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteAllByPickListId(int pickListId)
        {
            return PickListDetail.DeleteAllByPickListId(pickListId);
        }

        public static void DeleteAllByPickListId(int pickListId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByPickListId(pickListId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(PickListDetail)));
                }
            }
        }

        public static Result DeleteByPickListDetailIdExcept(int pickListId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<PickListDetail> pickListDetails = PickListDetail.GetByPickListIdExcept(pickListId, saveIds);
            foreach (PickListDetail pickListDetail in pickListDetails)
            {
                DataTrail dataTrail = PickListDetail.Delete(pickListDetail.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }
    }
}
