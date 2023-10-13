using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ConsoleApp.Commands.RawFiles
{
    public class ReadFileMimeType : Command
    {
        public ReadFileMimeType()
        {
            Title = "ReadFileMimeType";
            Description = "To Read file mime type";
            Arguments = new string[]
            {
                "filepath",
            };
            Options = new string[] {
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

            try
            {
                string filepath = CommandInput.Arguments[0];

                string mimeType = MimeMapping.GetMimeMapping(filepath);

                PrintMessage(mimeType);
            }
            catch (Exception e)
            {
                PrintError(e.Message);
            }

            PrintEnding();
        }
    }
}
