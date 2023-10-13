using System.Collections.Generic;
using System.Linq;

namespace Shared.Argument
{
    public class CommandInput
    {
        public string Command { get; set; }

        public IList<string> Arguments { get; set; }

        public IList<Option> Optionals { get; set; }

        public string Line;

        public CommandInput(string line)
        {
            Line = line.Trim();

            Arguments = new List<string> { };
            Optionals = new List<Option> { };

            Process();
        }

        public CommandInput(string[] args)
        {
            Line = string.Join(" ", args.ToArray());

            Arguments = new List<string> { };
            Optionals = new List<Option> { };

            ProcessArguments(args);
        }

        public void Process(IList<Option> optionals = null)
        {
            Line = Line.Trim();
            Optionals = new List<Option> { };

            List<char> newChars = new List<char>() { };
            char[] chars = Line.ToCharArray();
            bool inQuote = false;
            bool inHyphen = false;

            int index;
            for (index = 0; index < chars.Length; index++)
            {
                char currentChar = chars[index];
                char previousChar = (index - 1) >= 0 ? chars[index - 1] : '\n';

                switch (currentChar)
                {
                    case ' ':
                        if (previousChar == ' ')
                            continue;
                        if (!inQuote)
                            currentChar = '\n';
                        if (inHyphen)
                            inHyphen = false;
                        break;
                    case '"':
                        inQuote = !inQuote;
                        continue;
                    case '-':
                        inHyphen = true;
                        break;
                }

                newChars.Add(currentChar);
            }

            ProcessArguments((new string(newChars.ToArray())).Split('\n'), optionals);
        }

        public void ProcessArguments(string[] args, IList<Option> optionals = null)
        {
            int index;
            char[] chars;
            string arg;
            string action;
            string nextArg;
            for (index = 0; index < args.Length; index++)
            {
                arg = args[index];
                nextArg = (args.Length > (index + 1)) ? args[index + 1].Trim('"') : null;
                if (index == 0)
                {
                    Command = arg;
                }
                else
                {
                    if (IsOptional(arg))
                    {
                        action = arg.Substring(2);

                        if (action.Contains("="))
                        {
                            string[] opts = action.Split(new char[] { '=' }, 2);

                            Optionals.Add(new Option()
                            {
                                Name = opts[0],
                                Value = opts[1].Trim('"'),
                            });
                        }
                        else if (!IsOptional(nextArg))
                        {
                            if (nextArg == null)
                            {
                                Optionals.Add(new Option()
                                {
                                    Name = action,
                                    Value = "1",
                                });
                            }
                            else
                            {
                                Optionals.Add(new Option()
                                {
                                    Name = action,
                                    Value = nextArg,
                                });
                            }
                            index++;
                        }
                        else
                        {
                            Optionals.Add(new Option()
                            {
                                Name = action,
                                Value = "1",
                            });
                        }
                    }
                    else if (arg.StartsWith("-"))
                    {
                        action = arg.Substring(1);
                        bool added = false;

                        chars = action.ToCharArray();
                        if (optionals != null && chars.Length > 0)
                        {
                            var foundOption = optionals.Where(o => o.Code == chars[0]).FirstOrDefault();
                            if (foundOption != null && foundOption.IsValue)
                            {
                                string value = action.Substring(1);
                                if (action.Length == 1)
                                {
                                    value = nextArg;
                                    index++;
                                }

                                Optionals.Add(new Option()
                                {
                                    Code = foundOption.Code,
                                    Value = value,
                                });
                                added = true;
                            }
                        }

                        if (!added)
                        {
                            AddCodeOptions(chars);
                        }
                    }
                    else
                    {
                        Arguments.Add(arg);
                    }
                }
            }
        }

        public bool IsOptional(string str)
        {
            if (str == null)
                return false;
            return str.StartsWith("--");
        }

        public void AddCodeOptions(char[] chars)
        {
            foreach (char c in chars)
            {
                Optionals.Add(new Option()
                {
                    Code = c,
                    Value = "1",
                });
            }
        }

        public bool IsHelp()
        {
            var option = Optionals.Where(o => o.Name == "help" || o.Code == 'h').FirstOrDefault();
            return option != null;
        }
    }
}
