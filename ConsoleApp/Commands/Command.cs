using Shared;
using Shared.Argument;
using Shared.ProcessFile;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;

namespace ConsoleApp.Commands
{
    public class Command
    {
        public CommandInput CommandInput { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string[] Arguments { get; set; }

        public string[] Options { get; set; }

        public IList<Option> ObjOption { get; set; }

        public int CommitLimit { get; set; } = 50;

        public Dictionary<string, int> ProcessCount { get; set; }

        public bool Hide { get; set; } = false;

        public bool Log { get; set; } = true;

        public int? LogIndex { get; set; } = null;

        public Command()
        {
            ResetProcessCount();
        }

        public bool ValidRequiredArguments()
        {
            bool valid = true;
            if (Arguments != null && Arguments.Length > 0)
            {
                if (CommandInput.Arguments == null)
                {
                    valid = false;
                }
                if (Arguments.Length > CommandInput.Arguments.Count)
                {
                    valid = false;
                    int skip = CommandInput.Arguments.Count;

                    string[] requiredArgs;
                    if (skip > 0)
                        requiredArgs = Arguments.Skip(skip).ToArray();
                    else
                        requiredArgs = Arguments.ToArray();

                    PrintError(string.Format("Not enough arguments(missing: \"{0}\").", string.Join(", ", requiredArgs.ToArray())));
                }
            }
            return valid;
        }

        public virtual void Initial()
        {
            ObjOption = new List<Option> { };
            if (Options != null && Options.Length > 0)
            {
                int index;
                string str;

                string[] parts;
                for (index = 0; index < Options.Length; index++)
                {
                    str = Options[index];
                    if (!str.Contains("--"))
                    {
                        throw new Exception("Option format not correct and must start with --");
                    }

                    str = str.Substring(2);

                    string action = str;
                    string description = "";
                    char code = '\0'; // empty char
                    string name = null;
                    bool isValue = false;

                    if (str.Contains(":"))
                    {
                        parts = str.Split(new char[] { ':' }, 2);
                        action = (parts.Length >= 1) ? parts[0].Trim() : "";
                        description = (parts.Length >= 2) ? parts[1].Trim() : "";
                    }

                    name = action;
                    if (action.Length == 1)
                    {
                        code = Convert.ToChar(action);
                        name = null;
                    }
                    else if (action.Contains("|"))
                    {
                        parts = action.Split(new char[] { '|' }, 2);
                        code = Convert.ToChar(parts[0][0]);
                        name = parts[1].Trim('=');

                        if (parts[1].EndsWith("="))
                        {
                            isValue = true;
                        }
                    }
                    else if (action.EndsWith("="))
                    {
                        isValue = true;
                        action = action.Trim('=');

                        if (action.Length == 1)
                        {
                            code = Convert.ToChar(action);
                        }
                        else
                        {
                            name = action;
                        }
                    }

                    if (code != '\0' && ObjOption.Where(o => o.Code == code).Count() > 0)
                        continue;
                    if (ObjOption.Where(o => o.Name == name).Count() > 0)
                        continue;

                    ObjOption.Add(new Option
                    {
                        Code = code,
                        Name = name,
                        Description = description,
                        IsValue = isValue,
                    });
                }

                if (CommandInput != null)
                    CommandInput.Process(ObjOption);
            }

            LogIndex = OptionInteger("logIndex", 0);
        }

        public virtual bool Validate()
        {
            return true;
        }

        public virtual void Run()
        {
        }

        public virtual void ResetProcessCount()
        {
            ProcessCount = new Dictionary<string, int> { };
            ProcessCount["Processed"] = 0;
        }

        public virtual int GetProcessCount(string name = "Processed")
        {
            if (ProcessCount.ContainsKey(name))
                return ProcessCount[name];

            return 0;
        }

        public virtual void SetProcessCount(string name = "Processed", int? number = null, bool acc = false)
        {
            if (!ProcessCount.ContainsKey(name))
                ProcessCount[name] = 0;

            if (number is null)
                ProcessCount[name]++;
            else if (acc)
                ProcessCount[name] += number.Value;
            else
                ProcessCount[name] = number.Value;
        }

        public bool IsCommitBuffer(string name = "Processed")
        {
            int processed = GetProcessCount(name);
            return processed != 0 && processed % CommitLimit == 0;
        }

        public bool PrintCommitBuffer(string name = "Processed")
        {
            if (IsCommitBuffer(name))
            {
                PrintProcessCount();
                return true;
            }
            return false;
        }

        public virtual void PrintProcessCount()
        {
            Collection<string> output = new Collection<string>();
            foreach (string key in ProcessCount.Keys)
            {
                int count = ProcessCount[key];
                if (count > 0)
                {
                    output.Add(string.Format("{0}: {1}", key, count));
                }
            }
            PrintMessage(string.Format("{0} - {1}", Title, string.Join(", ", output.ToArray())));
        }

        public virtual void PrintDetail(string name, object value, string mark = "", int width = 30)
        {
            PrintMessage(Util.FormatDetail(name, value, mark, width));
        }

        public virtual void PrintDetailWithArrow(string name, object value)
        {
            PrintDetail(name, value, mark: " > ");
        }

        public virtual void PrintMessageOnly(object message, bool datetime = true)
        {
            PrintMessage(message, datetime, false);
        }

        public virtual void PrintMessage(bool datetime = true, bool? log = null)
        {
            PrintMessage("", datetime, log);
        }

        public virtual void PrintMessage(object message, bool datetime = true, bool? log = null)
        {
            if (message == null)
                return;

            int space = 0;
            string output;
            string dt = "";
            if (datetime)
            {
                dt = DateTime.Now.ToString(Util.DateTimeFormat) + "   ";
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(dt);
                Console.ResetColor();

                space = dt.Length;
            }

            if (space > 0)
                message = message.ToString().Replace("\n", "\n".PadRight(space + 1, ' '));

            Console.WriteLine(message);

            output = string.Format("{0}{1}", dt, message);
            if (log == null)
                log = Log;
            if (log == true)
                WriteLog(output);
        }

        public virtual void PrintMessages(object messages, bool datetime = true, bool? log = null)
        {
            if (messages is IList items)
            {
                foreach (var item in items)
                {
                    PrintMessage(item, datetime, log);
                }
            }
        }

        public void PrintHelp()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Description:");
            Console.ResetColor();
            Console.WriteLine("  " + Description);

            if (Arguments != null && Arguments.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("");
                Console.WriteLine("Arguments:");
                Console.ResetColor();
                foreach (string arg in Arguments)
                {
                    Console.WriteLine("  " + arg);
                }
            }

            if (ObjOption != null && ObjOption.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("");
                Console.WriteLine("Options:");
                Console.ResetColor();

                string name;
                int padRightWidth = 0;
                foreach (Option option in ObjOption)
                {
                    name = option.PrintName();
                    if (name.Length > padRightWidth)
                    {
                        padRightWidth = name.Length;
                    }
                }

                foreach (Option option in ObjOption)
                {
                    name = option.PrintName();
                    Console.Write("  ");
                    Console.Write(option.PrintCode());
                    Console.Write(name.PadRight(padRightWidth + 5, ' '));
                    Console.Write(option.Description + "\n");
                }
            }
            Console.WriteLine("");
        }

        public void PrintTitle()
        {
            PrintMessage(Title);
        }

        public void PrintOutputTitle(string title, int width = 30, bool datetime = true, bool? log = null)
        {
            PrintMessage("".PadLeft(width, '-'), datetime, log);
            PrintMessage(title, datetime, log);
            PrintMessage("", datetime, log);
        }

        public void PrintStart()
        {
            PrintMessage("---START---");
        }

        public void PrintStarting()
        {
            PrintStart();
            PrintTitle();
            PrintMessage();
        }

        public void PrintEnd()
        {
            PrintMessage("----END----");
        }

        public void PrintEnding()
        {
            PrintMessage();
            PrintEnd();
        }

        public void PrintLine(int width = 90, char line = '-')
        {
            PrintMessage("".PadRight(width, line));
        }

        public void PrintError(string error, bool datetime = false)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;

            if (datetime)
            {
                var dt = DateTime.Now.ToString(Util.DateTimeFormat) + "   ";
                Console.Write(dt);
            }

            Console.Write(error + "\n");
            Console.ResetColor();
        }

        public string Option(string name)
        {
            var findOption = ObjOption.Where(o => o.Name == name).FirstOrDefault();
            if (findOption != null)
            {
                var option = CommandInput.Optionals.Where(o => o.Name == findOption.Name || (o.Code != '\0' && o.Code == findOption.Code)).FirstOrDefault();
                if (option != null)
                    return option.Value;
            }
            return null;
        }

        public int OptionInteger(string name, int value = 0)
        {
            var str = Option(name);
            if (!string.IsNullOrEmpty(str) && int.TryParse(str, out int result))
            {
                return result;
            }
            return value;
        }

        public int? OptionIntegerNullable(string name, int? value = null)
        {
            var str = Option(name);
            if (!string.IsNullOrEmpty(str) && int.TryParse(str, out int result))
            {
                return result;
            }
            return value;
        }

        public bool IsOption(string name, bool value = false)
        {
            var option = Option(name);
            if (option != null && option == "1")
                return true;
            return value;
        }

        public bool? IsOptionNullable(string name, bool? value = null)
        {
            var option = Option(name);
            if (option != null && option == "1")
                return true;
            return value;
        }

        public string LogFilePath(string title = null)
        {
            string filename = Title;
            if (!string.IsNullOrEmpty(title))
                filename = title;

            if (!string.IsNullOrEmpty(filename))
            {
                string stackType = Util.GetConfig("LogFileStackType");
                string path;
                switch (stackType.ToLower())
                {
                    case "year":
                    case "yearly":
                    case "annual":
                    case "y":
                    case "a":
                        if (LogIndex != 0)
                            path = string.Format("{0}/{1}/{2}.{3}.log.txt", Util.GetLogPath(), DateTime.Now.ToString("yyyy"), filename, LogIndex);
                        else
                            path = string.Format("{0}/{1}/{2}.log.txt", Util.GetLogPath(), DateTime.Now.ToString("yyyy"), filename);
                        break;
                    case "month":
                    case "monthly":
                    case "m":
                        if (LogIndex != 0)
                            path = string.Format("{0}/{1}/{2}.{3}.log.txt", Util.GetLogPath(), DateTime.Now.ToString("yyyyMM"), filename, LogIndex);
                        else
                            path = string.Format("{0}/{1}/{2}.log.txt", Util.GetLogPath(), DateTime.Now.ToString("yyyyMM"), filename);
                        break;
                    case "day":
                    case "daily":
                    case "d":
                        if (LogIndex != 0)
                            path = string.Format("{0}/{1}/{2}.{3}.{4}.log.txt", Util.GetLogPath(), DateTime.Now.ToString("yyyyMM"), DateTime.Now.ToString("yyyyMMdd"), filename, LogIndex);
                        else
                            path = string.Format("{0}/{1}/{2}.{3}.log.txt", Util.GetLogPath(), DateTime.Now.ToString("yyyyMM"), DateTime.Now.ToString("yyyyMMdd"), filename);
                        break;
                    default:
                        if (LogIndex != 0)
                            path = string.Format("{0}/{1}.{2}.log.txt", Util.GetLogPath(), filename, LogIndex);
                        else
                            path = string.Format("{0}/{1}.log.txt", Util.GetLogPath(), filename);
                        break;
                }
                Util.MakeDir(path);
                return path;
            }
            return null;
        }

        public void WriteLog(object log)
        {
            if (bool.Parse(Util.GetConfig("UseAppendLogging")))
            {
                AppendWriteLog(log);
            }
            else
            {
                using (var logFile = new TextFile(LogFilePath(), true, true))
                {
                    logFile.WriteLine(log);
                }
            }
        }

        public void AppendWriteLog(object log)
        {
            //var dt = DateTime.Now.ToString(Util.DateTimeFormat) + "   ";
            //var message = log.ToString().Replace("\n", "\n".PadRight(dt.Length + 1, ' '));

            //var argument = Environment.GetCommandLineArgs();
            //var process = argument.Length > 1 ? argument[1] : Title;

            File.AppendAllText(LogFilePath(), log.ToString() + "\n" /*String.Format("{0}[{1}] {2}\n", dt, process, message*/);
        }
    }
}