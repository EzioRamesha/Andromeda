using Shared.ProcessFile;
using System;
using System.IO;

namespace ConsoleApp.Commands.RawFiles
{
    class WriteExcel : Command
    {
        public Excel Excel { get; set; }

        public WriteExcel()
        {
            Title = "WriteExcel";
            Description = "To write a excel file";
            Arguments = new string[]
            {
                "filepath",
            };
            Hide = true;
        }

        public override void Run()
        {
            PrintStarting();

            try
            {
                string filepath = CommandInput.Arguments[0];
                Excel = new Excel(filepath, false);

                Excel.WriteCell(1, 1, "1.1");
                Excel.WriteCell(2, 3, "2.3");
                Excel.WriteCell(4, 5, "3.5");
                Excel.WriteCell(7, 7, "7.7");
                Excel.Save();
            }
            catch (Exception e)
            {
                PrintError(e.Message);
            }

            PrintEnding();
        }
    }
}
