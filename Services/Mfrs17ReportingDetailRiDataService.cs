using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Services.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;

namespace Services
{
    public class Mfrs17ReportingDetailRiDataService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Mfrs17ReportingDetailRiData)),
            };
        }

        public static Mfrs17ReportingDetailRiDataBo FormBo(Mfrs17ReportingDetailRiData entity = null)
        {
            if (entity == null)
                return null;
            return new Mfrs17ReportingDetailRiDataBo
            {
                Mfrs17ReportingDetailId = entity.Mfrs17ReportingDetailId,
                RiDataWarehouseId = entity.RiDataWarehouseId.Value,
                CutOffId = entity.CutOffId.Value,
                RiDataWarehouseHistoryBo = RiDataWarehouseHistoryService.Find(entity.RiDataWarehouseId, entity.CutOffId, false),
            };
        }

        public static Mfrs17ReportingDetailRiDataBo FormSimplifiedBo(Mfrs17ReportingDetailRiData entity = null)
        {
            if (entity == null)
                return null;
            return new Mfrs17ReportingDetailRiDataBo
            {
                Mfrs17ReportingDetailId = entity.Mfrs17ReportingDetailId,
                RiDataWarehouseId = entity.RiDataWarehouseId.Value,
                CutOffId = entity.CutOffId.Value,
            };
        }

        public static IList<Mfrs17ReportingDetailRiDataBo> FormBos(IList<Mfrs17ReportingDetailRiData> entities = null)
        {
            if (entities == null)
                return null;
            IList<Mfrs17ReportingDetailRiDataBo> bos = new List<Mfrs17ReportingDetailRiDataBo>() { };
            foreach (Mfrs17ReportingDetailRiData entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static IList<Mfrs17ReportingDetailRiDataBo> FormSimplifiedBos(IList<Mfrs17ReportingDetailRiData> entities = null)
        {
            if (entities == null)
                return null;
            IList<Mfrs17ReportingDetailRiDataBo> bos = new List<Mfrs17ReportingDetailRiDataBo>() { };
            foreach (Mfrs17ReportingDetailRiData entity in entities)
            {
                bos.Add(FormSimplifiedBo(entity));
            }
            return bos;
        }

        public static Mfrs17ReportingDetailRiData FormEntity(Mfrs17ReportingDetailRiDataBo bo = null)
        {
            if (bo == null)
                return null;
            return new Mfrs17ReportingDetailRiData
            {
                Mfrs17ReportingDetailId = bo.Mfrs17ReportingDetailId,
                RiDataWarehouseId = bo.RiDataWarehouseId,
                CutOffId = bo.CutOffId,
            };
        }

        public static bool IsExists(int id)
        {
            return PickListDetail.IsExists(id);
        }

        public static Mfrs17ReportingDetailRiDataBo Find(int mfrs17ReportingDetailId, int cutOffId, int riDataWarehouseId)
        {
            return FormBo(Mfrs17ReportingDetailRiData.Find(mfrs17ReportingDetailId, cutOffId, riDataWarehouseId));
        }

        public static int CountByMfrs17ReportingDetailId(int mfrs17ReportingDetailId)
        {
            return Mfrs17ReportingDetailRiData.CountByMfrs17ReportingDetailId(mfrs17ReportingDetailId);
        }

        public static List<int> GetIdsByMfrs17ReportingDetailId(int mfrs17ReportingDetailId, int skip, int take)
        {
            return Mfrs17ReportingDetailRiData.GetIdsByMfrs17ReportingDetailId(mfrs17ReportingDetailId, skip, take);
        }

        public static IList<Mfrs17ReportingDetailRiDataBo> GetByMfrs17ReportingDetailId(int mfrs17ReportingDetailId, int skip, int take)
        {
            return FormBos(Mfrs17ReportingDetailRiData.GetByMfrs17ReportingDetailId(mfrs17ReportingDetailId, skip, take));
        }

        public static int CountByMfrs17ReportingIdMfrs17TreatyCode(int mfrs17ReportingDetailId, string mfrs17TreatyCode)
        {
            return Mfrs17ReportingDetailRiData.CountByMfrs17ReportingIdMfrs17TreatyCode(mfrs17ReportingDetailId, mfrs17TreatyCode);
        }

        public static IList<Mfrs17ReportingDetailRiDataBo> GetByMfrs17ReportingIdMfrs17TreatyCode(int mfrs17ReportingDetailId, string mfrs17TreatyCode, int skip, int take)
        {
            return FormBos(Mfrs17ReportingDetailRiData.GetByMfrs17ReportingIdMfrs17TreatyCode(mfrs17ReportingDetailId, mfrs17TreatyCode, skip, take));
        }

        public static IList<Mfrs17ReportingDetailRiDataBo> GetSimplifiedByMfrs17ReportingIdMfrs17TreatyCode(int mfrs17ReportingDetailId, string mfrs17TreatyCode, int skip, int take)
        {
            return FormSimplifiedBos(Mfrs17ReportingDetailRiData.GetByMfrs17ReportingIdMfrs17TreatyCode(mfrs17ReportingDetailId, mfrs17TreatyCode, skip, take));
        }

        public static List<int> GetIdsByMfrs17ReportingDetailIdPage(int mfrs17ReportingDetailId, int skip, int take, int? page = null)
        {
            return Mfrs17ReportingDetailRiData.GetIdsByMfrs17ReportingDetailIdPage(mfrs17ReportingDetailId, skip, take, page);
        }

        public static IList<Mfrs17ReportingDetailRiDataBo> GetSimplifiedByMfrs17ReportingIdMfrs17TreatyCodePage(int mfrs17ReportingDetailId, string mfrs17TreatyCode, int skip, int take, int? page = null)
        {
            return FormSimplifiedBos(Mfrs17ReportingDetailRiData.GetByMfrs17ReportingIdMfrs17TreatyCodePage(mfrs17ReportingDetailId, mfrs17TreatyCode, skip, take, page));
        }

        public static Result Create(ref Mfrs17ReportingDetailRiDataBo bo)
        {
            Mfrs17ReportingDetailRiData entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
            }
            return result;
        }

        public static void Create(ref Mfrs17ReportingDetailRiDataBo bo, AppDbContext db)
        {
            Mfrs17ReportingDetailRiData entity = FormEntity(bo);
            entity.Create(db);
        }

        public static Result Create(ref Mfrs17ReportingDetailRiDataBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table, bo.PrimaryKey());
            }
            return result;
        }

        public static IList<DataTrail> DeleteAllByMfrs17ReportingDetailId(int mfrs17ReportingDetailId)
        {
            return Mfrs17ReportingDetailRiData.DeleteAllByMfrs17ReportingDetailId(mfrs17ReportingDetailId);
        }

        public static void DeleteAllByMfrs17ReportingDetailId(int mfrs17ReportingDetailId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByMfrs17ReportingDetailId(mfrs17ReportingDetailId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(Mfrs17ReportingDetailRiData)));
                }
            }
        }
    }
}
