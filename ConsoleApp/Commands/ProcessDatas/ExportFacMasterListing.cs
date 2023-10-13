using BusinessObject;
using Services;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportFacMasterListing : ExportFile
    {
        public FacMasterListingBo FacMasterListingBo { get; set; }
        public IQueryable<FacMasterListingBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "FacMasterListing";

        public override List<Column> GetColumns()
        {
            Columns = FacMasterListingBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);
            return Util.FormatExport(v);
        }

        public override object GetEntity()
        {
            return FacMasterListingBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<FacMasterListingBo> { };
            return Query.OrderBy(q => q.Id).Skip(Skip).Take(Take).ToList();
        }

        public override int GetQueryTotal()
        {
            if (Query == null)
                Total = 0;
            else
                Total = Query.Count();
            return Total;
        }

        public override void SetEntity(object entity)
        {
            FacMasterListingBo = null;
            if (entity is FacMasterListingBo e)
            {
                FacMasterListingBo = e;
            }
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<FacMasterListingBo> q)
                Query = q;
        }
    }
}
