using Newtonsoft.Json;
using System.Collections.Generic;

namespace Shared
{
    public class LogLine
    {
        [JsonIgnore]
        public bool Enabled { get; set; } = true;
        [JsonIgnore]
        public int DetailWidth { get; set; } = 30;

        public List<string> Lines { get; set; }

        public LogLine()
        {
            Lines = new List<string> { };
        }

        public void Add(object value = null)
        {
            if (!Enabled)
                return;
            if (value is null)
                value = "";
            Lines.Add(value.ToString());
        }

        public void AddLines(List<string> lines)
        {
            if (!Enabled)
                return;
            Lines.AddRange(lines);
        }

        public void LineDelimiter(int width = 60, char mark = '-')
        {
            if (!Enabled)
                return;
            Add("".PadRight(width, mark));
        }

        public void Title(string title = "")
        {
            if (!Enabled)
                return;
            LineDelimiter();
            Add(title);
            Add();
        }

        public void Detail(string name, object value, string mark = "")
        {
            if (!Enabled)
                return;
            if (value == null)
                value = Util.Null;

            name = string.Format("{0}{1}", mark, name);
            Add(string.Format("{0}: {1}", name.PadRight(DetailWidth, ' '), value ?? Util.Null));
        }

        public void Before(object value)
        {
            Detail("Before", value);
        }

        public void After(object value)
        {
            Detail("After", value);
        }

        public void Property(object value)
        {
            Detail("Property", value);
        }

        public void Success(object value = null)
        {
            if (value == null)
                Add("Success");
            else
                Detail("Success", value);
        }

        public void Failed(object value = null)
        {
            if (value == null)
                Add("Failed");
            else
                Detail("Failed", value);
        }

        public void Found()
        {
            Add("Found");
        }

        public void NotFound()
        {
            Add("Not Found");
        }

        public void Parameters()
        {
            Add("Parameters");
        }

        public void Match()
        {
            Add("Match");
        }

        public void NotMatch()
        {
            Add("Not Match");
        }

        public void ValueIsNull()
        {
            Failed(MessageBag.ValueIsNull);
        }

        public void Error(object error)
        {
            if (!Enabled)
                return;
            if (error == null)
                return;
            Detail("Error", error);
        }

        public void Errors(List<string> errors)
        {
            if (!Enabled)
                return;
            int i = 1;
            foreach (var error in errors)
            {
                Lines.Add(string.Format("  ERROR#{0} > {1}", i, error));
                i++;
            }
        }


        public void Parameter(string name, object value, string mark = " * ")
        {
            Detail(name, value, mark);
        }

        public void MappedValue(string name, object value, string mark = " > ")
        {
            Detail(name, value, mark);
        }
    }
}
