using BusinessObject;
using DataAccess.Entities;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DirectRetroConfigurationMappingService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(DirectRetroConfigurationMapping)),
            };
        }

        public static DirectRetroConfigurationMappingBo FormBo(DirectRetroConfigurationMapping entity = null)
        {
            if (entity == null)
                return null;
            return new DirectRetroConfigurationMappingBo
            {
                Id = entity.Id,
                DirectRetroConfigurationId = entity.DirectRetroConfigurationId,
                DirectRetroConfigurationBo = DirectRetroConfigurationService.Find(entity.DirectRetroConfigurationId),
                Combination = entity.Combination,
                RetroParty = entity.RetroParty,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
            };
        }

        public static IList<DirectRetroConfigurationMappingBo> FormBos(IList<DirectRetroConfigurationMapping> entities = null)
        {
            if (entities == null)
                return null;
            IList<DirectRetroConfigurationMappingBo> bos = new List<DirectRetroConfigurationMappingBo>() { };
            foreach (DirectRetroConfigurationMapping entity in entities)
            {
                bos.Add(FormBo(entity));
            }
            return bos;
        }

        public static DirectRetroConfigurationMapping FormEntity(DirectRetroConfigurationMappingBo bo = null)
        {
            if (bo == null)
                return null;
            return new DirectRetroConfigurationMapping
            {
                Id = bo.Id,
                DirectRetroConfigurationId = bo.DirectRetroConfigurationId,
                Combination = bo.Combination,
                RetroParty = bo.RetroParty,
                CreatedById = bo.CreatedById,
                CreatedAt = bo.CreatedAt,
            };
        }

        public static bool IsExists(int id)
        {
            return DirectRetroConfigurationMapping.IsExists(id);
        }

        public static DirectRetroConfigurationMappingBo Find(int id)
        {
            return FormBo(DirectRetroConfigurationMapping.Find(id));
        }

        public static DirectRetroConfigurationMappingBo FindByCombination(
            string combination,
            int? directRetroConfigurationId = null
        )
        {
            return FormBo(DirectRetroConfigurationMapping.FindByCombination(
                combination,
                directRetroConfigurationId
            ));
        }

        public static DirectRetroConfigurationMappingBo FindByCombination(string combination, DirectRetroConfigurationBo directRetroConfigurationBo)
        {
            return FindByCombination(
                combination,
                directRetroConfigurationBo.Id
            );
        }

        public static DirectRetroConfigurationMappingBo FindByParams(
            string retroParty,
            string treatyCode,
            bool groupById = false
        )
        {
            return FormBo(DirectRetroConfigurationMapping.FindByParams(
                retroParty,
                treatyCode,
                groupById
            ));
        }

        public static int CountByCombination(
            string combination,
            int? directRetroConfigurationId = null
        )
        {
            return DirectRetroConfigurationMapping.CountByCombination(
                combination,
                directRetroConfigurationId
            );
        }

        public static int CountByCombination(string combination, DirectRetroConfigurationBo directRetroConfigurationBo)
        {
            return CountByCombination(
                combination,
                directRetroConfigurationBo.Id
            );
        }

        public static int CountByParams(
            string retroParty,
            string treatyCode,
            bool groupById = false
        )
        {
            return DirectRetroConfigurationMapping.CountByParams(
                retroParty,
                treatyCode,
                groupById
            );
        }

        public static IList<DirectRetroConfigurationMappingBo> GetByParams(
            string retroParty,
            string treatyCode,
            bool groupById = false
        )
        {
            return FormBos(DirectRetroConfigurationMapping.GetByParams(
                retroParty,
                treatyCode,
                groupById
            ));
        }

        public static Result Save(ref DirectRetroConfigurationMappingBo bo)
        {
            if (!DirectRetroConfigurationMapping.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public Result Save(ref DirectRetroConfigurationMappingBo bo, ref TrailObject trail)
        {
            if (!DirectRetroConfigurationMapping.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref DirectRetroConfigurationMappingBo bo)
        {
            DirectRetroConfigurationMapping entity = FormEntity(bo);

            Result result = Result();
            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref DirectRetroConfigurationMappingBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref DirectRetroConfigurationMappingBo bo)
        {
            Result result = Result();

            DirectRetroConfigurationMapping entity = DirectRetroConfigurationMapping.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.DirectRetroConfigurationId = bo.DirectRetroConfigurationId;
                entity.Combination = bo.Combination;
                entity.RetroParty = bo.RetroParty;
                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref DirectRetroConfigurationMappingBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(DirectRetroConfigurationMappingBo bo)
        {
            DirectRetroConfigurationMapping.Delete(bo.Id);
        }

        public static Result Delete(DirectRetroConfigurationMappingBo bo, ref TrailObject trail)
        {
            Result result = Result();

            DataTrail dataTrail = DirectRetroConfigurationMapping.Delete(bo.Id);
            dataTrail.Merge(ref trail, result.Table);

            return result;
        }

        public static IList<DataTrail> DeleteByDirectRetroConfigurationId(int directRetroConfigurationId)
        {
            return DirectRetroConfigurationMapping.DeleteByDirectRetroConfigurationId(directRetroConfigurationId);
        }

        public static void DeleteByDirectRetroConfigurationId(int directRetroConfigurationId, ref TrailObject trail)
        {
            Result result = Result();
            IList<DataTrail> dataTrails = DeleteByDirectRetroConfigurationId(directRetroConfigurationId);
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
