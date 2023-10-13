using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shared
{
    public class ConsoleHandler
    {
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        public static void Set()
        {
            WriteLog("Console Opens");

            Thread.GetDomain().UnhandledException +=
                (sender, eventArgs) =>
                {
                    WriteLog("Unhandled Exception: " + eventArgs.ExceptionObject.ToString(), "ConsoleException");
                };

            handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(handler, true);
        }

        private static bool Handler(CtrlType sig)
        {
            WriteLog("Console Closes: " + sig);

            Thread.Sleep(5000);

            //shutdown right away so there are no lingering threads
            Environment.Exit(-1);

            return true;
        }

        public static string LogFilePath(string name)
        {
            var now = DateTime.Now;
            var path = string.Format("{0}/{1}/{2}.{3}.log.txt", Util.GetLogPath(), now.ToString("yyyyMM"), now.ToString("yyyyMMdd"), name);


            Util.MakeDir(path);

            return path;
        }

        public static void WriteLog(object log, string name = "Console")
        {
            var dt = DateTime.Now.ToString(Util.DateTimeFormat) + "   ";
            var message = log.ToString().Replace("\n", "\n".PadRight(dt.Length + 1, ' '));

            var argument = Environment.GetCommandLineArgs();
            var process = argument.Length > 1 ? argument[1] : "ConsoleApp";

            File.AppendAllText(LogFilePath(name), String.Format("{0}[{1}] {2}\n", dt, process, message));
        }

    }
}
