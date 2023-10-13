using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shared.ProcessFile
{
    public class TextFile : IProcessFile, IDisposable
    {
        public string FilePath { get; set; }
        public Stream Stream { get; set; }

        public int? RowIndex { get; set; }
        public int? ColIndex { get; set; }

        public int? MaxRow { get; set; }
        public int? MaxCol { get; set; }

        public string Line { get; set; }

        public bool IsLineContainDelimiter { get; set; }

        public bool HandleNewLine { get; set; } = false;

        public char Delimiter { get; set; } = ',';

        public List<int> ColLengths { get; set; }

        public List<string> Columns { get; set; }

        public StreamReader Reader { get; set; }

        public StreamWriter Writer { get; set; }

        // To detect redundant calls
        private bool _disposed = false;

        ~TextFile() => Dispose(false);

        public TextFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                throw new Exception(string.Format(MessageBag.FileNotExists, filepath));
            }

            FilePath = filepath;
            Reader = new StreamReader(FilePath);
        }

        public TextFile(string filepath, bool writer, bool append = false, Encoding encoding = null)
        {
            if (writer)
            {
                FilePath = filepath;
                Writer = (encoding != null) ? new StreamWriter(FilePath, append, encoding) : new StreamWriter(FilePath, append);
            }
            else
            {
                if (!File.Exists(filepath))
                {
                    throw new Exception(string.Format(MessageBag.FileNotExists, filepath));
                }

                FilePath = filepath;
                Reader = new StreamReader(FilePath);
            }
        }

        public TextFile(Stream stream)
        {
            Reader = new StreamReader(stream);
        }

        public TextFile(string filepath, char delimiter) : this(filepath)
        {
            Delimiter = delimiter;
        }

        public TextFile(Stream stream, char delimiter) : this(stream)
        {
            Delimiter = delimiter;
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
                Close();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            _disposed = true;
        }

        public int? GetNextRow()
        {
            if (RowIndex == null)
            {
                RowIndex = 0;
            }
            RowIndex++;

            IsLineContainDelimiter = false;

            if (Reader.EndOfStream)
                return null;

            if (MaxRow != 0 && MaxRow < RowIndex)
                return null;

            ReadLine();
            return RowIndex;
        }

        public List<Row> GetNextRows(int noOfRows = 1)
        {
            bool everyRowEmpty = true;
            var rows = new List<Row> { };
            for (int i = 0; i < noOfRows; i++)
            {
                var r = GetNextRow();
                if (r != null)
                {
                    everyRowEmpty = false;

                    int? rowIndex = GetRowIndex();
                    bool isEmpty = true;

                    var cols = new List<Column> { };
                    while (GetNextCol() != null)
                    {
                        bool hasData = true;
                        var value = GetValue();
                        if (value == null || (value is string && string.IsNullOrEmpty(value)))
                        {
                            hasData = false;
                        }
                        if (hasData)
                        {
                            isEmpty = false;
                        }

                        int? colIndex = GetColIndex();
                        cols.Add(new Column
                        {
                            RowIndex = rowIndex != null ? rowIndex.Value : 0,
                            ColIndex = colIndex != null ? colIndex.Value : 0,
                            Value = value,
                            Value2 = GetValue2(),
                        });
                    }

                    if (isEmpty)
                    {
                        rows.Add(new Row
                        {
                            IsEmpty = true,
                        });
                    }
                    else
                    {
                        rows.Add(new Row
                        {
                            RowIndex = rowIndex != null ? rowIndex.Value : 0,
                            Columns = cols,
                            IsEmpty = false,
                            IsLineContainDelimiter = IsLineContainDelimiter,
                        });
                    }
                }
                else
                {
                    rows.Add(new Row
                    {
                        IsEmpty = true,
                        IsEnd = true,
                    });
                }
            }
            if (everyRowEmpty)
                return null;

            return rows;
        }

        public void ReadLine()
        {
            Line = Reader.ReadLine();
            if (HandleNewLine)
            {
                while (Line.CountOccurence("\"") % 2 != 0)
                {
                    Line += Reader.ReadLine();
                }
            }
            ProcessColumns();
        }

        public int? GetNextCol()
        {
            if (Columns == null || Columns.Count() == 0)
                return null;

            if (ColIndex == null)
            {
                ColIndex = 0;
            }
            ColIndex++;

            if (MaxCol != 0 && MaxCol < ColIndex)
            {
                ColIndex = null;
                return null;
            }

            if (ColIndex <= Columns.Count())
            {
                return ColIndex;
            }

            ColIndex = null;
            return null;
        }

        public int? GetRowIndex()
        {
            return RowIndex;
        }

        public int? GetColIndex()
        {
            return ColIndex;
        }

        public int? GetTotalCol()
        {
            if (Columns == null)
                return 0;
            return Columns.Count;
        }

        public int? GetDelimiter()
        {
            return Delimiter;
        }

        public void ResetColLengths()
        {
            ColLengths = new List<int> { };
        }

        public void AddColLengths(int length)
        {
            ColLengths.Add(length);
        }

        public dynamic GetColValue(int col)
        {
            if (col != 0 && Columns != null && Columns.Count() > col - 1)
                return Columns.ToList()[col - 1];
            return null;
        }

        public dynamic GetValue()
        {
            return GetColValue(ColIndex.Value);
        }

        public dynamic GetValue2()
        {
            return GetValue();
        }

        public string GetDetails()
        {
            var value = GetValue();
            return string.Format(
                "Row: {0}, Col: {1}, Value: {2}",
                RowIndex,
                ColIndex,
                value
            );
        }

        public void ProcessColumns()
        {
            if (ColLengths != null && ColLengths.Count() > 0)
            {
                ProcessFixedLength();
            }
            else
            {
                Columns = new List<string> { };

                switch (Delimiter)
                {
                    case ',':
                        Columns.AddRange(Line.ToColumns(Delimiter));
                        break;
                    default:
                        Columns.AddRange(Line.Split(Delimiter).Select(q => q.Trim('"')));
                        break;
                }

                IsLineContainDelimiter = Line.Contains(Delimiter);
            }
        }

        public void ProcessFixedLength()
        {
            int sum = ColLengths.Sum();
            if (sum > Line.Length)
                throw new Exception("The total number of characters is less than columns length config, line: " + RowIndex.ToString());

            Columns = new List<string> { };

            string l = Line;
            int index = 0;
            foreach (int length in ColLengths)
            {
                Columns.Add(l.Substring(0, length));
                l = l.Substring(length, l.Length - length);
                index++;
            }
        }

        public void WriteLine(object value = null)
        {
            if (Writer != null)
            {
                if (value == null)
                    Writer.WriteLine("");
                else
                    Writer.WriteLine(value);
            }
        }

        public void Close()
        {
            if (Reader != null)
                Reader.Close();
            if (Writer != null)
                Writer.Close();
        }
    }
}
