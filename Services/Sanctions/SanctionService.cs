using BusinessObject;
using BusinessObject.Sanctions;
using DataAccess.Entities.Sanctions;
using DataAccess.EntityFramework;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Sanctions
{
    public class SanctionService
    {
        public static Result Result()
        {
            return new Result
            {
                Table = UtilAttribute.GetTableName(typeof(Sanction)),
                Controller = ModuleBo.ModuleController.Sanction.ToString()
            };
        }

        public static Expression<Func<Sanction, SanctionBo>> Expression()
        {
            return entity => new SanctionBo
            {
                Id = entity.Id,
                SanctionBatchId = entity.SanctionBatchId,
                PublicationInformation = entity.PublicationInformation,
                Category = entity.Category,
                RefNumber = entity.RefNumber,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static SanctionBo FormBo(Sanction entity = null, bool foreign = false)
        {
            if (entity == null)
                return null;

            SanctionBo bo = new SanctionBo
            {
                Id = entity.Id,
                SanctionBatchId = entity.SanctionBatchId,
                PublicationInformation = entity.PublicationInformation,
                Category = entity.Category,
                RefNumber = entity.RefNumber,
                CreatedById = entity.CreatedById,
                CreatedAt = entity.CreatedAt,
                UpdatedById = entity.UpdatedById,
            };

            if (foreign)
            {
                bo.SanctionBatchBo = SanctionBatchService.Find(entity.SanctionBatchId);
                bo.SanctionNameBos = SanctionNameService.FormBos(entity.SanctionNames.ToList());
                bo.SanctionBirthDateBos = SanctionBirthDateService.FormBos(entity.SanctionBirthDates.ToList());
                bo.SanctionCommentBos = SanctionCommentService.FormBos(entity.SanctionComments.ToList());
                bo.SanctionCountryBos = SanctionCountryService.FormBos(entity.SanctionCountries.ToList());
                bo.SanctionIdentityBos = SanctionIdentityService.FormBos(entity.SanctionIdentities.ToList());
                bo.SanctionFormatNameBos = SanctionFormatNameService.FormBos(entity.SanctionFormatNames.ToList());
                bo.SanctionAddressBos = SanctionAddressService.FormBos(entity.SanctionAddresses.ToList());
            }

            return bo;
        }

        public static IList<SanctionBo> FormBos(IList<Sanction> entities = null, bool foreign = false)
        {
            if (entities == null)
                return null;
            IList<SanctionBo> bos = new List<SanctionBo>() { };
            foreach (Sanction entity in entities)
            {
                bos.Add(FormBo(entity, foreign));
            }
            return bos;
        }

        public static Sanction FormEntity(SanctionBo bo = null)
        {
            if (bo == null)
                return null;
            return new Sanction
            {
                Id = bo.Id,
                SanctionBatchId = bo.SanctionBatchId,
                PublicationInformation = bo.PublicationInformation,
                Category = bo.Category,
                RefNumber = bo.RefNumber,
                CreatedById = bo.CreatedById,
                UpdatedById = bo.UpdatedById,
            };
        }

        public static bool IsExists(int id)
        {
            return Sanction.IsExists(id);
        }

        public static SanctionBo Find(int id)
        {
            return FormBo(Sanction.Find(id));
        }

        public static IList<SanctionBo> Get()
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.Sanctions.ToList());
            }
        }

        public static IList<SanctionBo> Get(int skip, int take)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.Sanctions
                    .OrderBy(q => q.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToList(), false);
            }
        }

        public static IList<SanctionBo> GetBySanctionBatchId(int sanctionBatchId)
        {
            using (var db = new AppDbContext())
            {
                return FormBos(db.Sanctions.Where(q => q.SanctionBatchId == sanctionBatchId).ToList());
            }
        }

        public static IList<SanctionBo> GetBySanctionId(int[] sanctionIds)
        {
            using (var db = new AppDbContext())
            {
                db.Database.CommandTimeout = 0;

                EFExecutionStrategy connectionStrategy = EFExecutionStrategy.GetNewInstance("SanctionService");

                return connectionStrategy.Execute(() => FormBos(db.Sanctions.Where(q => sanctionIds.Contains(q.Id)).ToList(), true));
            }
        }

        public static IList<SanctionBo> GetBySanctionBatchId(int sanctionBatchId, int skip = 0, int take = 50)
        {
            using (var db = new AppDbContext(false))
            {
                var query = db.Sanctions.Where(q => q.SanctionBatchId == sanctionBatchId).OrderBy(q => q.Id);
                return FormBos(query.Skip(skip).Take(take).ToList());
            }
        }

        public static int Count()
        {
            using (var db = new AppDbContext(false))
            {
                return db.Sanctions.Count();
            }
        }

        public static int CountBySanctionBatchId(int sanctionBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.Sanctions.Where(q => q.SanctionBatchId == sanctionBatchId).OrderBy(q => q.Id).Count();
            }
        }

        public static Result Save(ref SanctionBo bo)
        {
            if (!Sanction.IsExists(bo.Id))
            {
                return Create(ref bo);
            }
            return Update(ref bo);
        }

        public static Result Save(ref SanctionBo bo, ref TrailObject trail)
        {
            if (!Sanction.IsExists(bo.Id))
            {
                return Create(ref bo, ref trail);
            }
            return Update(ref bo, ref trail);
        }

        public static Result Create(ref SanctionBo bo)
        {
            Sanction entity = FormEntity(bo);

            Result result = Result();

            if (result.Valid)
            {
                result.DataTrail = entity.Create();
                bo.Id = entity.Id;
            }

            return result;
        }

        public static Result Create(ref SanctionBo bo, ref TrailObject trail)
        {
            Result result = Create(ref bo);
            if (result.Valid)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static Result Update(ref SanctionBo bo)
        {
            Result result = Result();

            Sanction entity = Sanction.Find(bo.Id);
            if (entity == null)
            {
                throw new Exception(MessageBag.NoRecordFound);
            }

            if (result.Valid)
            {
                entity.SanctionBatchId = bo.SanctionBatchId;
                entity.PublicationInformation = bo.PublicationInformation;
                entity.Category = bo.Category;
                entity.RefNumber = bo.RefNumber;
                entity.UpdatedAt = DateTime.Now;
                entity.UpdatedById = bo.UpdatedById ?? entity.UpdatedById;

                result.DataTrail = entity.Update();
            }
            return result;
        }

        public static Result Update(ref SanctionBo bo, ref TrailObject trail)
        {
            Result result = Update(ref bo);
            if (result.Valid && result.DataTrail != null)
            {
                result.DataTrail.Merge(ref trail, result.Table);
            }
            return result;
        }

        public static void Delete(SanctionBo bo)
        {
            SanctionNameService.DeleteBySanctionId(bo.Id);
            Sanction.Delete(bo.Id);
        }

        public static Result Delete(SanctionBo bo, ref TrailObject trail)
        {
            Result result = Result();

            if (result.Valid)
            {
                SanctionNameService.DeleteBySanctionId(bo.Id, ref trail);
                DataTrail dataTrail = Sanction.Delete(bo.Id);
                dataTrail.Merge(ref trail, result.Table);
            }

            return result;
        }

        public static IList<DataTrail> DeleteBySanctionBatchId(int sanctionBatchId)
        {
            return Sanction.DeleteBySanctionBatchId(sanctionBatchId);
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
