using BusinessObject;
using BusinessObject.Retrocession;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportRetroTreaty : ExportFile
    {
        public RetroTreatyBo RetroTreatyBo { get; set; }
        public IQueryable<RetroTreatyBo> Query { get; set; }
        public override string PrefixFileName { get; set; } = "PremiumSpreadTable";

        public override List<Column> GetColumns()
        {
            Columns = RetroTreatyBo.GetColumns();
            return Columns;
        }

        public override string GetColumnValue(Column col, object entity)
        {
            object v = entity.GetPropertyValue(col.Property);

            switch (col.Property)
            {
                case "Status":
                    return RetroTreatyBo.GetStatusName(int.Parse(v.ToString()));
                case "LineOfBusiness":
                    bool isLobAutomatic = false;
                    bool isLobFacultative = false;
                    bool isLobAdvantageProgram = false;
                    if (bool.TryParse(entity.GetPropertyValue("IsLobAutomatic").ToString(), out bool b1))
                        isLobAutomatic = b1;
                    if (bool.TryParse(entity.GetPropertyValue("IsLobFacultative").ToString(), out bool b2))
                        isLobFacultative = b2;
                    if (bool.TryParse(entity.GetPropertyValue("IsLobAdvantageProgram").ToString(), out bool b3))
                        isLobAdvantageProgram = b3;

                    List<string> lineOfBusiness = new List<string> { };
                    if (isLobAutomatic)
                        lineOfBusiness.Add("AUTO");
                    if (isLobFacultative)
                        lineOfBusiness.Add("FAC");
                    if (isLobAdvantageProgram)
                        lineOfBusiness.Add("AP");

                    return string.Join(",", lineOfBusiness);
                case "DetailConfigIsToAggregate":
                    if (v == null)
                        return "";

                    bool isToAggregate = false;
                    if (bool.TryParse(v.ToString(), out bool b))
                        isToAggregate = b;

                    return isToAggregate ? "Y" : "N";
                default:
                    return Util.FormatExport(v);
            }
        }

        public override object GetEntity()
        {
            return RetroTreatyBo;
        }

        public override IQueryable<object> GetQuery()
        {
            return Query;
        }

        public override IEnumerable<object> GetQueryNext()
        {
            if (Query == null)
                return new List<RetroTreatyBo> { };
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
            RetroTreatyBo = null;
            if (entity is RetroTreatyBo e)
                RetroTreatyBo = e;
        }

        public override void SetQuery(IQueryable<object> query)
        {
            Query = null;
            if (query is IQueryable<RetroTreatyBo> q)
                Query = q;
        }
    }
}
