using DataAccess.Entities.SoaDatas;
using DataAccess.EntityFramework;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccess.Entities.InvoiceRegisters
{
    [Table("InvoiceRegisterBatchSoaDatas")]
    public class InvoiceRegisterBatchSoaData
    {
        [Key, Column(Order = 0)]
        public int InvoiceRegisterBatchId { get; set; }

        [ExcludeTrail]
        public virtual InvoiceRegisterBatch InvoiceRegisterBatch { get; set; }

        [Key, Column(Order = 1)]
        public int SoaDataBatchId { get; set; }

        [ExcludeTrail]
        public virtual SoaDataBatch SoaDataBatch { get; set; }

        public string PrimaryKey()
        {
            return string.Format("{0}|{1}", InvoiceRegisterBatchId, SoaDataBatchId);
        }

        public static bool IsExists(int invoiceRegisterBatchId, int soaDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchSoaDatas
                    .Any(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId && q.SoaDataBatchId == soaDataBatchId);
            }
        }

        public static InvoiceRegisterBatchSoaData Find(int invoiceRegisterBatchId, int soaDataBatchId)
        {
            if (invoiceRegisterBatchId == 0 || soaDataBatchId == 0)
                return null;

            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchSoaDatas
                    .Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId && q.SoaDataBatchId == soaDataBatchId)
                    .FirstOrDefault();
            }
        }

        public static int CountByInvoiceRegisterBatchId(int invoiceRegisterBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.InvoiceRegisterBatchSoaDatas.Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId).Count();
            }
        }

        public static List<int> GetIdsByInvoiceRegisterBatchId(int invoiceRegisterBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                return db.InvoiceRegisterBatchSoaDatas
                    .Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId)
                    .OrderBy(q => q.SoaDataBatchId)
                    .Select(q => q.SoaDataBatchId)
                    .ToList();
            }
        }

        public static IList<InvoiceRegisterBatchSoaData> GetByInvoiceRegisterBatchId(int invoiceRegisterBatchId, int skip = 0, int take = 50)
        {
            using (var db = new AppDbContext(false))
            {
                return db.InvoiceRegisterBatchSoaDatas
                    .Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId)
                    .OrderBy(q => q.SoaDataBatchId)
                    .Skip(skip)
                    .Take(take)
                    .ToList();
            }
        }

        public static int CountBySoaDataBatchId(int soaDataBatchId)
        {
            using (var db = new AppDbContext())
            {
                return db.InvoiceRegisterBatchSoaDatas.Where(q => q.SoaDataBatchId == soaDataBatchId).Count();
            }
        }

        public DataTrail Create()
        {
            using (var db = new AppDbContext(false))
            {
                DataTrail trail = new DataTrail(this);

                db.InvoiceRegisterBatchSoaDatas.Add(this);
                db.SaveChanges();

                return trail;
            }
        }

        public void Create(AppDbContext db)
        {
            db.InvoiceRegisterBatchSoaDatas.Add(this);
        }

        public static IList<DataTrail> DeleteAllByInvoiceRegisterBatchId(int invoiceRegisterBatchId)
        {
            using (var db = new AppDbContext(false))
            {
                List<DataTrail> trails = new List<DataTrail>();

                db.Database.ExecuteSqlCommand("DELETE FROM [InvoiceRegisterBatchSoaDatas] WHERE [InvoiceRegisterBatchId] = {0}", invoiceRegisterBatchId);
                db.SaveChanges();

                return trails;
            }
        }

        public static void DeleteAllByInvoiceRegisterBatchId(int invoiceRegisterBatchId, ref TrailObject trail)
        {
            using (var db = new AppDbContext())
            {
                var query = db.InvoiceRegisterBatchSoaDatas.Where(q => q.InvoiceRegisterBatchId == invoiceRegisterBatchId);

                foreach (InvoiceRegisterBatchSoaData invoiceRegisterBatchSoaData in query.ToList())
                {
                    var dataTrail = new DataTrail(invoiceRegisterBatchSoaData, true);
                    dataTrail.Merge(ref trail, UtilAttribute.GetTableName(typeof(InvoiceRegisterBatch)), invoiceRegisterBatchSoaData.PrimaryKey());

                    db.Entry(invoiceRegisterBatchSoaData).State = EntityState.Deleted;
                    db.InvoiceRegisterBatchSoaDatas.Remove(invoiceRegisterBatchSoaData);
                }

                db.SaveChanges();
            }
        }
    }
}
