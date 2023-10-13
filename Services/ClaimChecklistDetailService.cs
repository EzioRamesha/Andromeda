using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Newtonsoft.Json;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services
{
    public class ClaimChecklistDetailService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(ClaimChecklistDetail)),
                Controller = ModuleBo.ModuleController.ClaimChecklistDetail.ToString()
            };
        }

        public static Expression<Func<ClaimChecklistDetail, ClaimChecklistDetailBo>> Expression()
        {
            return entity => new ClaimChecklistDetailBo
            {
                Id = entity.Id,
                ClaimChecklistId = entity.ClaimChecklistId,
                Name = entity.Name,
                Remark = entity.Remark,

                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static ClaimChecklistDetailBo FormBo(ClaimChecklistDetail entity = null)
        {
            if (entity == null)
                return null;
            return new ClaimChecklistDetailBo
            {
                Id = entity.Id,
                ClaimChecklistId = entity.ClaimChecklistId,
                Name = entity.Name,
                Remark = entity.Remark,

                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static IList<ClaimChecklistDetailBo> FormBos(IList<ClaimChecklistDetail> entities = null)
        {
            if (entities == null)
                return null;
            IList<ClaimChecklistDetailBo> bos = new List<ClaimChecklistDetailBo>() { };
            foreach (ClaimChecklistDetail entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static ClaimChecklistDetail FormEntity(ClaimChecklistDetailBo bo = null)
        {
            if (bo == null)
                return null;
            return new ClaimChecklistDetail
            {
                Id = bo.Id,
                ClaimChecklistId = bo.ClaimChecklistId,
                Name = bo.Name?.Trim(),
                Remark = bo.Remark?.Trim(),
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return ClaimChecklistDetail.IsExists(id);
        }

        public static ClaimChecklistDetailBo Find(int id)
        {
            return FormBo(ClaimChecklistDetail.Find(id));
        }

        public static IList<ClaimChecklistDetailBo> GetByClaimChecklistId(int claimChecklistId)
        {
            return FormBos(ClaimChecklistDetail.GetByClaimChecklistId(claimChecklistId));
        }
        
        public static string GetJsonByClaimCode(string claimCode)
        {
            using (var db = new AppDbContext())
            {
                IList<ClaimChecklistDetail> details = db.ClaimChecklistDetails.Where(q => q.ClaimChecklist.ClaimCode.Code.Trim() == claimCode.Trim()).ToList();
                Dictionary<string, string> checklists = new Dictionary<string, string>();
                foreach (ClaimChecklistDetail checklistDetail in details)
                {
                    checklists.Add(checklistDetail.Name, ClaimRegisterBo.ChecklistStatusPending.ToString());
                }
                checklists.Add(ClaimChecklistDetailBo.RemarkCode, "");

                return JsonConvert.SerializeObject(checklists);
            }
        }

        public static Result Save(ref ClaimChecklistDetailBo bo)
        {
            if (!ClaimChecklistDetail.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref ClaimChecklistDetailBo bo, ref TrailObject trail)
        {
            if (!ClaimChecklistDetail.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref ClaimChecklistDetailBo bo)
        {
            ClaimChecklistDetail entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref ClaimChecklistDetailBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref ClaimChecklistDetailBo bo)
        {
            Result result = Result();

            ClaimChecklistDetail entity = ClaimChecklistDetail.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.Id = bo.Id;
                entity.ClaimChecklistId = bo.ClaimChecklistId;
                entity.Name = bo.Name;
                entity.Remark = bo.Remark;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref ClaimChecklistDetailBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(ClaimChecklistDetailBo bo)
        {
            ClaimChecklistDetail.Delete(bo.Id);
        }

        public static Result Delete(ClaimChecklistDetailBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = ClaimChecklistDetail.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static Result DeleteByClaimChecklistDetailIdExcept(int claimChecklistId, List<int> saveIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<ClaimChecklistDetail> claimChecklistDetails = ClaimChecklistDetail.GetByClaimChecklistIdExcept(claimChecklistId, saveIds);
            foreach (ClaimChecklistDetail claimChecklistDetail in claimChecklistDetails)
            {
                DataTrail dataTrail = ClaimChecklistDetail.Delete(claimChecklistDetail.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<DataTrail> DeleteByClaimChecklistId(int claimChecklistId)
        {
            return ClaimChecklistDetail.DeleteByClaimChecklistId(claimChecklistId);
        }

        public static void DeleteByClaimChecklistId(int claimChecklistId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteByClaimChecklistId(claimChecklistId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(ClaimChecklistDetail)));
                }
            }
        }
    }
}
