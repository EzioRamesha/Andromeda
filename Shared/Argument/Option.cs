using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Argument
{
    public class Option
    {
        public char Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Default { get; set; } = "";

        public string Value { get; set; }

        // required enter value (not boolean)
        public bool IsValue { get; set; }

        public string PrintCode()
        {
            if (Code != '\0')
                return "-" + Code + ", ";
            return "    ";
        }

        public string PrintName()
        {
            string name = "--" + Name;
            if (IsValue)
                name += "=[" + Name.ToUpper() + "]";
            return name;
        }
    }
}
