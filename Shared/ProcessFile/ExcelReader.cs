using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ProcessFile
{
    public class ExcelReader : Excel
    {
        public FileStream FileStream { get; set; }

        public IExcelDataReader Reader { get; set; }

        public ExcelReader(string filepath, bool readOnly = true, object worksheet = null, int rowRead = 0)
        {
            if (readOnly)
            {
                if (!File.Exists(filepath))
                {
                    throw new Exception(string.Format(MessageBag.FileNotExists, filepath));
                }
            }
            else
            {
                var finfo = new FileInfo(filepath);
                if (!Directory.Exists(finfo.Directory.FullName))
                {
                    throw new Exception(string.Format(MessageBag.DirectoryNotExists, finfo.Directory.FullName));
                }
            }

            FilePath = filepath;

            FileStream = File.Open(FilePath, FileMode.Open, FileAccess.Read);
            Reader = ExcelReaderFactory.CreateReader(FileStream);

            IsOpen = true;

            if (worksheet is string @string)
            {
                if (!@string.StartsWith("0") && int.TryParse(@string, out int sheetIndex))
                {
                    if (sheetIndex < 1000) // e.g. 201911 can not convert to int. Because it is string.
                        worksheet = sheetIndex;
                }
            }

            if (worksheet != null)
            {
                bool sheetFound = false;
                do
                {
                    if (Reader.Name == worksheet.ToString())
                    {
                        sheetFound = true;
                        break;
                    }
                }
                while (Reader.NextResult());

                if (!sheetFound)
                {
                    Close();
                    throw new Exception(string.Format("Worksheet not found: {0}", worksheet ?? 1));
                }
            }

            TotalRow = Reader.RowCount;
            TotalCol = Reader.FieldCount;

            if (rowRead != 0)
                RowRead = rowRead;
        }

        public override int? GetNextRow()
        {
            if (RowIndex == 0 && TotalRow > 0)
            {
                RowIndex = 1;
                Reader.Read();
                return RowIndex;
            }
            RowIndex++;

            if (MaxRow != 0 && MaxRow < RowIndex)
                return null;

            if (RowIndex <= TotalRow)
            {
                Reader.Read();
                return RowIndex;
            }

            return null;
        }

        public override int? GetNextCol()
        {
            if (RowIndex == 0)
                return null;
            if (TotalRow <= 0)
                return null;

            if (ColIndex == 0 && TotalCol > 0)
            {
                ColIndex = 1;
                return ColIndex;
            }

            ColIndex++;

            if (MaxCol != 0 && MaxCol < ColIndex)
            {
                ColIndex = 0;
                return null;
            }

            if (ColIndex <= TotalCol)
            {
                return ColIndex;
            }

            ColIndex = 0;
            return null;
        }

        public override dynamic GetValue()
        {
            return Reader.GetValue(ColIndex - 1);
        }

        public override void Close()
        {
            if (!IsOpen)
                return;

            if (Reader != null)
            {
                Reader.Close();
            }

            if (FileStream != null)
            {
                FileStream.Close();
            }

            IsOpen = false;
        }
    }
}
