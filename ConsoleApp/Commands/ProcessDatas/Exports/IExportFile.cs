using BusinessObject;
using DataAccess.EntityFramework;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    interface IExportFile
    {
        ExportBo ExportBo { get; set; }

        bool IsTextFile { get; set; }
        Excel Excel { get; set; }
        int RowIndex { get; set; }

        string FilePath { get; set; }
        string FileName { get; set; }
        string PrefixFileName { get; set; }

        int Total { get; set; }
        int Processed { get; set; }
        int Skip { get; set; }
        int Take { get; set; }

        // Range Query
        bool IsRangeQuery { get; set; }
        int MinId { get; set; }

        List<Column> Columns { get; set; }

        AppDbContext Db { get; set; }

        List<Column> GetColumns();
        string GetColumnValue(Column col, object entity);
        object GetEntity();
        IQueryable<object> GetQuery();
        int GetQueryTotal();

        void SetColumns(List<Column> cols);
        void SetExportBo(ExportBo exportBo);
        void SetEntity(object entity);
        void SetQuery(IQueryable<object> query);
        void Init();

        bool WriteHeader();
        void HandleDirectory();
        void HandleTempDirectory(bool appendDateTime = true);

        void Process();
        void ProcessNext();

        void WriteLine(object line);
        void WriteHeaderLine();
        void WriteDataLine();

        void OpenExcel();
        void CloseExcel();
    }
}
