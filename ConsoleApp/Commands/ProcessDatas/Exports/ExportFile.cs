using BusinessObject;
using DataAccess.EntityFramework;
using Shared;
using Shared.ProcessFile;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp.Commands.ProcessDatas.Exports
{
    public class ExportFile : IExportFile
    {
        public ExportBo ExportBo { get; set; }

        public virtual bool IsTextFile { get; set; } = true;
        public Excel Excel { get; set; }
        public int RowIndex { get; set; } = 1;

        public string FilePath { get; set; }
        public string FileName { get; set; }
        public virtual string PrefixFileName { get; set; } = "File";

        public int Total { get; set; } = 0;
        public int Processed { get; set; } = 0;
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 500;

        public bool WritableHeader { get; set; } = true;

        // Range Query
        public bool IsRangeQuery { get; set; } = false;
        public int MinId { get; set; } = 0;

        public List<Column> Columns { get; set; }

        public AppDbContext Db { get; set; }

        public virtual List<Column> GetColumns()
        {
            Columns = new List<Column> { };
            return Columns;
        }

        public virtual string GetColumnValue(Column col, object entity)
        {
            return Util.FormatExport(entity.GetPropertyValue(col.Property), IsTextFile);
        }

        public virtual object GetEntity()
        {
            return null;
        }

        public virtual IQueryable<object> GetQuery()
        {
            return null;
        }

        public virtual IEnumerable<object> GetQueryNext()
        {
            return null;
        }

        public virtual int GetQueryTotal()
        {
            Total = 0;
            return 0;
        }

        public void SetColumns(List<Column> cols)
        {
            Columns = cols;
        }

        public void SetExportBo(ExportBo exportBo)
        {
            ExportBo = exportBo;
        }

        public virtual void SetEntity(object entity)
        {
        }

        public virtual void SetQuery(IQueryable<object> entity)
        {
        }

        public virtual void Init()
        {
        }

        public virtual bool WriteHeader()
        {
            if (ExportBo != null)
                return ExportBo.WriteHeader(WritableHeader);
            return WritableHeader; // default true
        }

        public virtual void HandleDirectory()
        {
            if (ExportBo == null)
                return;

            FilePath = ExportBo.GetPath();
            if (File.Exists(FilePath))
                File.Delete(FilePath);
            Util.MakeDir(FilePath);

            if (!IsTextFile)
            {
                OpenExcel();
            }
        }

        public void HandleTempDirectory(bool appendDateTime = true)
        {
            string extension = ".csv";
            if (!IsTextFile)
                extension = ".xlsx";

            var directory = Util.GetTemporaryPath();
            if (appendDateTime)
                FileName = PrefixFileName.AppendDateTimeFileName(extension);
            else
                FileName = string.Format("{0}{1}", PrefixFileName, extension); 
            FilePath = Path.Combine(directory, FileName);
            Util.MakeDir(FilePath);

            // Delete all previous files
            Util.DeleteFiles(directory, $@"{PrefixFileName}*");
        }

        public virtual void Process()
        {
            GetColumns();
            WriteHeaderLine();
            if (GetQuery() == null)
                return;

            GetQueryTotal();
            for (Skip = 0; Skip < Total + Take; Skip += Take)
            {
                if (Skip >= Total)
                    break;

                ProcessNext();
            }
        }

        public virtual void ProcessNext()
        {
            var list = GetQueryNext();
            if (list == null)
                return;

            foreach (var entity in list)
            {
                SetEntity(entity);
                WriteDataLine();
                Processed++;
            }
        }

        public void WriteLine(object line)
        {
            using (var textFile = new TextFile(FilePath, true, true))
            {
                textFile.WriteLine(line);
            }
        }

        public virtual void WriteHeaderLine()
        {
            if (Columns.IsNullOrEmpty())
                return;
            if (!WriteHeader())
                return;

            if (IsTextFile)
                WriteLine(string.Join(",", Columns.Select(p => p.Header.FormatWithQuotation())));
            else
            {
                if (Excel == null)
                    return;

                int colIndex;
                int index = 1;
                foreach (var col in Columns)
                {
                    colIndex = index;
                    if (col.ColIndex.HasValue)
                        colIndex = col.ColIndex.Value;

                    Excel.WriteCell(RowIndex, colIndex, col.Header);

                    index++;
                }

                RowIndex++;
            }
        }

        public virtual void WriteDataLine()
        {
            object entity = GetEntity();
            if (entity != null)
            {
                if (IsTextFile)
                {
                    List<string> values = new List<string> { };
                    foreach (var col in Columns)
                    {
                        if (string.IsNullOrEmpty(col.Property))
                        {
                            values.Add("");
                            continue;
                        }

                        values.Add(GetColumnValue(col, entity));
                    }

                    WriteLine(Util.FormatExportLine(values));
                }
                else
                {
                    if (Excel == null)
                        return;

                    int colIndex;
                    int index = 1;
                    foreach (var col in Columns)
                    {
                        colIndex = index;
                        if (col.ColIndex.HasValue)
                            colIndex = col.ColIndex.Value;

                        Excel.WriteCell(RowIndex, colIndex, GetColumnValue(col, entity));

                        index++;
                    }

                    RowIndex++;
                }
            }
        }

        public void OpenExcel()
        {
            Excel = new Excel(FilePath, false);
        }

        public void CloseExcel()
        {
            if (Excel != null)
                Excel.Save();
        }
    }
}
