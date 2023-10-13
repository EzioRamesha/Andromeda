using Shared;
using Shared.ProcessFile;
using System.Diagnostics;
using System.IO;
using Row = Shared.ProcessFile.Row;

namespace ConsoleApp.Commands.RawFiles
{
    public class ReadTextFile : Command
    {
        public TextFile TextFile { get; set; }

        public ReadTextFile()
        {
            Title = "ReadTextFile";
            Description = "To read a text file";
            Arguments = new string[]
            {
                "filepath",
            };
            Hide = true;
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

            string filepath = CommandInput.Arguments[0];
            TextFile = new TextFile(filepath, '\t')
            {
                //MaxRow = 15,
                //MaxCol = 30,
            };

            /*
            while (TextFile.GetNextRow() != null)
            {
                while (TextFile.GetNextCol() != null)
                {
                    PrintMessage(TextFile.GetDetails());
                }
            }
            */

            var sw = new Stopwatch();
            int take = 5;

            sw.Start();
            var rows = TextFile.GetNextRows(take);
            sw.Stop();

            while (rows != null)
            {
                PrintMessage(string.Format("Between {0} to {1}", TextFile.RowIndex - take + 1, TextFile.RowIndex));

                foreach (Row row in rows)
                {
                    if (row.IsEmpty)
                        continue;
                    if (row.Columns.Count == 0)
                        continue;

                    foreach (Column col in row.Columns)
                    {
                        PrintMessage(col.GetCellDetails());
                    }
                }

                sw.Start();
                rows = TextFile.GetNextRows(take);
                sw.Stop();
            }

            TextFile.Close();

            PrintEnding();
        }
    }
}
