using BusinessObject;
using DataAccess.Entities;
using DataAccess.Entities.RiDatas;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class TreatyCodeService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(TreatyCode)),
            };
        }

        public static TreatyCodeBo FormBo(TreatyCode entity = null, bool foreign = true)
        {
            if (entity == null)
                return null;

            var bo = new TreatyCodeBo
            {
                Id = entity.Id,
                TreatyId = entity.TreatyId,
                Code = entity.Code,
                OldTreatyCodeId = entity.OldTreatyCodeId,
                Description = entity.Description,
                Status = entity.Status,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
                AccountFor = entity.AccountFor,
                TreatyTypePickListDetailId = entity.TreatyTypePickListDetailId,
                TreatyStatusPickListDetailId = entity.TreatyStatusPickListDetailId,
                LineOfBusinessPickListDetailId = entity.LineOfBusinessPickListDetailId,
                TreatyNo = entity.TreatyNo,
            };

            if (foreign)
            {
                bo.TreatyBo = TreatyService.Find(entity.TreatyId);
                bo.TreatyTypePickListDetailBo = PickListDetailService.Find(entity.TreatyTypePickListDetailId);
                bo.TreatyStatusPickListDetailBo = PickListDetailService.Find(entity.TreatyStatusPickListDetailId);
                bo.LineOfBusinessPickListDetailBo = PickListDetailService.Find(entity.LineOfBusinessPickListDetailId);
            }

            return bo;
        }

        public static IList<TreatyCodeBo> FormBos(IList<TreatyCode> entities = null, bool foreign = true)
        {
            if (entities == null)
                return null;
            IList<TreatyCodeBo> bos = new List<TreatyCodeBo>() { };
            foreach (TreatyCode entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static TreatyCode FormEntity(TreatyCodeBo bo = null)
        {
            if (bo == null)
                return null;
            return new TreatyCode
            {
                Id = bo.Id,
                TreatyId = bo.TreatyId,
                Code = bo.Code,
                OldTreatyCodeId = bo.OldTreatyCodeId,
                Description = bo.Description,
                Status = bo.Status,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
                AccountFor = bo.AccountFor,
                TreatyTypePickListDetailId = bo.TreatyTypePickListDetailId,
                TreatyStatusPickListDetailId = bo.TreatyStatusPickListDetailId,
                LineOfBusinessPickListDetailId = bo.LineOfBusinessPickListDetailId,
                TreatyNo = bo.TreatyNo,
            };
        }

        public static bool IsExists(int id)
        {
            return TreatyCode.IsExists(id);
        }

        public static bool IsDuplicatePrefix(TreatyCodeBo treatyCode, int? cedantId)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(treatyCode.Code?.Trim()) && cedantId.HasValue)
                {
                    int pos = treatyCode.Code.IndexOf('-');
                    var code = treatyCode.Code.Substring(0, pos);
                    code += "-";

                    return db.TreatyCodes
                        .Where(q => q.Treaty.CedantId != cedantId.Value)
                        .Where(q => q.Code.StartsWith(code))
                        .Count() > 0;
                }
                return false;
            }
        }

        public static bool IsDuplicateCode(TreatyCodeBo treatyCode)
        {
            using (var db = new AppDbContext())
            {
                if (!string.IsNullOrEmpty(treatyCode.Code?.Trim()))
                {
                    if (treatyCode.TreatyId != 0)
                    {
                        var query = db.TreatyCodes
                            .Where(q => q.Code.Trim().Equals(treatyCode.Code.Trim(), StringComparison.OrdinalIgnoreCase))
                            .Where(q => q.TreatyId == treatyCode.TreatyId);
                        if (treatyCode.Id != 0)
                        {
                            query = query.Where(q => q.Id != treatyCode.Id);
                        }
                        return query.Count() > 0;
                    }
                }
                return false;
            }
        }

        public static bool IsPrefixModified(TreatyCodeBo treatyCode)
        {
            using (var db = new AppDbContext())
            {
                int pos = treatyCode.Code.IndexOf('-');
                var code = treatyCode.Code.Substring(0, pos);
                code += "-";

                return db.TreatyCodes
                        .Where(q => q.Id == treatyCode.Id)
                        .Where(q => q.Code.StartsWith(code))
                        .Any() == false;
            }
        }

        public static TreatyCodeBo Find(int id)
        {
            return FormBo(TreatyCode.Find(id));
        }

        public static TreatyCodeBo Find(int? id)
        {
            return FormBo(TreatyCode.Find(id));
        }

        public static TreatyCodeBo FindByCode(string code, bool foreign = true)
        {
            return FormBo(TreatyCode.FindByCode(code), foreign);
        }

        public static TreatyCodeBo FindByTreatyIdCode(int treatyId, string code)
        {
            return FormBo(TreatyCode.FindByTreatyIdCode(treatyId, code));
        }

        public static TreatyCodeBo FindByCedantIdCode(int cedantId, string code)
        {
            return FormBo(TreatyCode.FindByCedantIdCode(cedantId, code));
        }

        public static int CountDistinctByCedantId(int cedantId)
        {
            return TreatyCode.CountDistinctByCedantId(cedantId);
        }

        public static int CountByTreatyId(int treatyId)
        {
            return TreatyCode.CountByTreatyId(treatyId);
        }

        public static int CountByCodeStatus(string code, int? status = null)
        {
            return TreatyCode.CountByCodeStatus(code, status);
        }

        public static int CountByCedantIdCodeStatus(int cedantId, string code, int? status = null)
        {
            return TreatyCode.CountByCedantIdCodeStatus(cedantId, code, status);
        }

        public static int CountByCode(string code)
        {
            return TreatyCode.CountByCode(code);
        }

        public static int CountByStatus(int status)
        {
            return TreatyCode.CountByStatus(status);
        }

        public static int CountByTreatyTypePickListDetailId(int treatyTypePickListDetailId)
        {
            return TreatyCode.CountByTreatyTypePickListDetailId(treatyTypePickListDetailId);
        }

        public static int CountByTreatyStatusPickListDetailId(int treatyStatusPickListDetailId)
        {
            return TreatyCode.CountByTreatyStatusPickListDetailId(treatyStatusPickListDetailId);
        }

        public static int CountByLineOfBusinessPickListDetailId(int lineOfBusinessPickListDetailId)
        {
            using (var db = new AppDbContext())
            {
                return db.TreatyCodes.Where(q => q.LineOfBusinessPickListDetailId == lineOfBusinessPickListDetailId).Count();
            }
        }

        public static string GetBusinessOriginByCode(string code)
        {
            using (var db = new AppDbContext())
            {
                var treatyCode = db.TreatyCodes.Where(q => q.Code.Trim() == code.Trim()).FirstOrDefault();
                if (treatyCode == null)
                    return null;

                if (!treatyCode.Treaty.BusinessOriginPickListDetailId.HasValue)
                    return null;

                return treatyCode.Treaty.BusinessOriginPickListDetail.Code;
            }
        }

        public static IList<TreatyCodeBo> Get()
        {
            return FormBos(TreatyCode.Get());
        }

        public static IList<TreatyCodeBo> GetByStatus(int? status = null, int? selectedId = null, bool isUniqueCode = false, bool foreign = true)
        {
            return FormBos(TreatyCode.GetByStatus(status, selectedId, isUniqueCode), foreign);
        }

        public static IList<TreatyCodeBo> GetForList(int? status = null)
        {
            using (var db = new AppDbContext())
            {
                var entities = db.TreatyCodes.Where(q => q.Status == status).OrderBy(q => q.Code).ToList();
                if (entities == null)
                    return null;

                IList<TreatyCodeBo> bos = new List<TreatyCodeBo>() { };
                foreach (TreatyCode entity in entities)
                {
                    bos.Add(new TreatyCodeBo()
                    {
                        Id = entity.Id,
                        Code = entity.Code,
                        TreatyTypeCode = entity.TreatyTypePickListDetail?.Code,
                        TreatyType = entity.TreatyTypePickListDetail?.Description,
                        CedingCompanyType = entity.Treaty.Cedant.CedingCompanyTypePickListDetail?.Description,
                        CedingCompany = string.Format("{0} - {1}", entity.Treaty.Cedant.Code, entity.Treaty.Cedant.Name),
                        CreatedAtStr = entity.CreatedAt.ToString(Util.GetDateFormat()),
                    });
                }

                return bos;
            }
        }

        public static IList<TreatyCodeBo> GetByTreatyId(int treatyId)
        {
            return FormBos(TreatyCode.GetByTreatyId(treatyId));
        }

        public static IList<TreatyCodeBo> GetByTreatyId(int treatyId, int status)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyCodes
                    .Where(q => q.TreatyId == treatyId)
                    .Where(q => q.Status == status);

                return FormBos(query.OrderBy(q => q.Code).ToList());
            }
        }

        public static IList<TreatyCodeBo> GetByCedantId(int cedantId, int? status = null, int? selectedId = null, bool isUniqueCode = false, bool foreign = true)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyCodes
                    .Where(q => q.Treaty.CedantId == cedantId);

                if (status != null)
                {
                    if (selectedId != null)
                        query = query.Where(q => q.Status == status || q.Id == selectedId);
                    else
                        query = query.Where(q => q.Status == status);
                }

                if (isUniqueCode)
                    return FormBos(query.GroupBy(q => q.Code).Select(q => q.FirstOrDefault()).OrderBy(q => q.Code).ToList(), foreign);

                return FormBos(query.OrderBy(q => q.Code).ToList(), foreign);
            }
        }

        public static IList<TreatyCodeBo> GetByCedantCode(string cedantCode, int? status = null, string selectedCode = null)
        {
            using (var db = new AppDbContext())
            {
                var query = db.TreatyCodes
                    .Where(q => q.Treaty.Cedant.Code.Trim() == cedantCode.Trim());

                if (status != null)
                {
                    if (!string.IsNullOrEmpty(selectedCode))
                        query = query.Where(q => q.Status == status || q.Code.Trim() == selectedCode.Trim());
                    else
                        query = query.Where(q => q.Status == status);
                }

                return FormBos(query.OrderBy(q => q.Code).ToList());
            }
        }

        public static IEnumerable<string> GetTreatyCodesByTreatyId(int treatyId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.TreatyCodes.Where(q => q.Treaty.Id == treatyId).Select(q => q.Code).ToList();
            }
        }

        public static IEnumerable<string> GetTreatyCodesByCedantId(int cedantId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.TreatyCodes.Where(q => q.Treaty.CedantId == cedantId).Select(q => q.Code).ToList();
            }
        }

        public static IList<TreatyCodeBo> GetDistinctByCedantId(int cedantId, int skip, int take)
        {
            return FormBos(TreatyCode.GetDistinctByCedantId(cedantId, skip, take));
        }

        public static IList<TreatyCodeBo> GetByCode(string code)
        {
            return FormBos(TreatyCode.GetByCode(code));
        }

        public static IList<TreatyCodeBo> GetIndexByCedantId(int cedantId, bool foreign = true)
        {
            return FormBos(TreatyCode.GetIndexByCedantId(cedantId), foreign);
        }

        public static Result Save(TreatyCodeBo bo)
        {
            if (!TreatyCode.IsExists(bo.Id))
            {
                return Create(bo);
            }
            return Update(bo);
        }

        public static Result Save(TreatyCodeBo bo, ref TrailObject trail)
        {
            if (!TreatyCode.IsExists(bo.Id))
            {
                return Create(bo, ref trail);
            }
            return Update(bo, ref trail);
        }

        public static Result Create(TreatyCodeBo bo)
        {
            TreatyCode entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;

                // stored procudure
                var enablePartitioning = Util.GetConfigBoolean("EnablePartitioning", true);
                if (enablePartitioning)
                {
                    StoredProcedure storedProcedure = new StoredProcedure(StoredProcedure.AddRiDataPartition);
                    var filepath = Util.GetConfig("PartitioningFilePath");
                    var databaseName = Util.GetConfig("DatabaseName");
                    var riDataFileCount = Util.GetConfigInteger("RiDataFileCount", 3);
                    var riDataWarehouseFileCount = Util.GetConfigInteger("RiDataWarehouseFileCount", 3);
                    try
                    {
                        storedProcedure.AddParameter("DatabaseName", databaseName);
                        storedProcedure.AddParameter("TreatyCodeId", bo.Id);
                        storedProcedure.AddParameter("FilePath", filepath);
                        storedProcedure.AddParameter("RiDataFileCount", riDataFileCount);
                        storedProcedure.AddParameter("RiDataWarehouseFileCount", riDataWarehouseFileCount);
                        storedProcedure.Execute(true);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }

            return result;
        }

        public static Result Create(TreatyCodeBo bo, ref TrailObject trail)
        {
            Result result = Create(bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(TreatyCodeBo bo)
        {
            Result result = Result();

            TreatyCode entity = TreatyCode.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            //if (IsDuplicateCode(entity))
            //{
            //    result.AddTakenError("Code", bo.Code);
            //}

            if (result.Valid)
            {
                entity.TreatyId = bo.TreatyId;
                entity.Code = bo.Code;
                entity.OldTreatyCodeId = bo.OldTreatyCodeId;
                entity.AccountFor = bo.AccountFor;
                entity.TreatyTypePickListDetailId = bo.TreatyTypePickListDetailId;
                entity.TreatyStatusPickListDetailId = bo.TreatyStatusPickListDetailId;
                entity.TreatyNo = bo.TreatyNo;
                entity.Description = bo.Description;
                entity.Status = bo.Status;
                entity.LineOfBusinessPickListDetailId = bo.LineOfBusinessPickListDetailId;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(TreatyCodeBo bo, ref TrailObject trail)
        {
            Result result = Update(bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(TreatyCodeBo bo)
        {
            TreatyCode.Delete(bo.Id);
        }

        public static Result Delete(TreatyCodeBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (
                RiDataCorrection.CountByTreatyCodeId(bo.Id) > 0 ||
                TreatyBenefitCodeMapping.CountByTreatyCodeId(bo.Id) > 0 ||
                Mfrs17CellMappingDetail.CountByTreatyCodeId(bo.Id) > 0 ||
                Mfrs17ReportingDetail.CountByTreatyCodeId(bo.Id) > 0 ||
                TreatyOldCode.CountByTreatyCodeId(bo.Id) > 0 ||
                DirectRetroService.CountByTreatyCodeId(bo.Id) > 0 ||
                ItemCodeMappingDetailService.CountByTreatyCodeId(bo.Id) > 0
            )
            {
                result.AddErrorRecordInUsed();
            }

            if (result.Valid)
            {
                DataTrail dataTrail = TreatyCode.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static IList<DataTrail> DeleteAllByTreatyId(int treatyId)
        {
            return TreatyCode.DeleteAllByTreatyId(treatyId);
        }

        public static void DeleteAllByTreatyId(int treatyId, ref TrailObject trail)
        {
            IList<DataTrail> dataTrails = DeleteAllByTreatyId(treatyId);
            if (dataTrails.Count > 0)
            {
                foreach (DataTrail dataTrail in dataTrails)
                {
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(PickListDetail)));
                }
            }
        }

        public static Result DeleteByTreatyCodeIdExcept(int treatyId, List<int> mappingIds, ref TrailObject trail)
        {
            Result result = Result();
            IList<TreatyCode> treatyCodes = TreatyCode.GetByTreatyIdExcept(treatyId, mappingIds);
            foreach (TreatyCode treatyCode in treatyCodes)
            {
                DataTrail dataTrail = TreatyCode.Delete(treatyCode.Id);
                dataTrail.Merge(ref trail, result.Table);

                // stored procedure
                var enablePartitioning = Util.GetConfigBoolean("EnablePartitioning", true);
                if (enablePartitioning)
                {
                    StoredProcedure storedProcedure = new StoredProcedure(StoredProcedure.RemoveRiDataPartition);
                    try
                    {
                        storedProcedure.AddParameter("TreatyCode", treatyCode.Code);
                        storedProcedure.Execute(true);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            return result;
        }
    }
}
