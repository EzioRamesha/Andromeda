using Shared;
using Shared.ProcessFile;
using System;
using System.Diagnostics;
using System.IO;
using Row = Shared.ProcessFile.Row;

namespace ConsoleApp.Commands.RawFiles
{
    public class ReadExcel : Command
    {
        public Excel Excel { get; set; }

        public object Worksheet { get; set; }

        public int MaxRow { get; set; }

        public int MaxCol { get; set; }

        public int? StartRow { get; set; }
        public int? EndRow { get; set; }

        public int? StartColumn { get; set; }
        public int? EndColumn { get; set; }

        public bool IsExcelReader { get; set; }

        public ReadExcel()
        {
            Title = "ReadExcel";
            Description = "To read a excel file";
            Arguments = new string[]
            {
                "filepath",
            };
            Options = new string[] {
                "--s|worksheet= : Enter worksheet",
                "--r|maxRow= : Enter maximum Row",
                "--c|maxCol= : Enter maximum Col",
                "--startRow= : Enter Start Row",
                "--endRow= : Enter End Row",
                "--startCol= : Enter Start Col",
                "--endCol= : Enter End Col",
                "--excelReader= : Use Excel Reader",
            };
            Hide = true;
        }

        public override void Initial()
        {
            base.Initial();

            Worksheet = Option("worksheet");
            MaxRow = OptionInteger("maxRow", 20);
            MaxCol = OptionInteger("maxCol", 30);

            StartRow = OptionIntegerNullable("startRow");
            EndRow = OptionIntegerNullable("endRow");

            StartColumn = OptionIntegerNullable("startCol");
            EndColumn = OptionIntegerNullable("endCol");

            IsExcelReader = IsOption("excelReader");
        }

        public override bool Validate()
        {
            string filepath = CommandInput.Arguments[0];
            if (!File.Exists(filepath))
            {
                PrintError(string.Format(MessageBag.FileNotExists, filepath));
                return false;
            }
            return base.Validate();
        }

        public override void Run()
        {
            PrintStarting();

            try
            {
                string filepath = CommandInput.Arguments[0];
                if (IsExcelReader)
                {
                    Excel = new ExcelReader(filepath, worksheet: Worksheet)
                    {
                        MaxRow = MaxRow,
                        MaxCol = MaxCol,
                    };
                    PrintMessage("Using Excel Reader");
                }
                else
                {
                    Excel = new Excel(filepath, worksheet: Worksheet)
                    {
                        MaxRow = MaxRow,
                        MaxCol = MaxCol,
                    };
                    PrintMessage("Using Excel");
                }
                PrintMessage(Excel.GetTotal());

                /*
                while (Excel.GetNextRow() != null)
                {
                    while (Excel.GetNextCol() != null)
                    {
                        PrintMessage(Excel.GetCellDetails());
                    }
                }
                */

                var sw = new Stopwatch();
                int take = 10;

                sw.Start();
                var rows = Excel.GetNextRows(take);
                sw.Stop();

                while (rows != null)
                {
                    PrintMessage(string.Format("Between {0} to {1}", Excel.RowIndex - take + 1, Excel.RowIndex));
                    PrintMessage(string.Format("Elapsed Time: {0}", sw.Elapsed));

                    foreach (Row row in rows)
                    {
                        if (row.IsEmpty)
                            continue;
                        if (row.Columns.Count == 0)
                            continue;
                        if (StartRow != null && row.RowIndex < StartRow)
                            continue;
                        if (EndRow != null && row.RowIndex > EndRow)
                            continue;

                        foreach (Column col in row.Columns)
                        {
                            if (StartColumn != null && col.ColIndex < StartColumn)
                                continue;
                            if (EndColumn != null && col.ColIndex > EndColumn)
                                continue;

                            PrintMessage(col.GetCellDetails());
                        }
                    }

                    sw.Start();
                    rows = Excel.GetNextRows(take);
                    sw.Stop();
                }

                Excel.Close();
            }
            catch (Exception e)
            {
                PrintError(e.Message);
            }

            PrintEnding();
        }
    }
}
