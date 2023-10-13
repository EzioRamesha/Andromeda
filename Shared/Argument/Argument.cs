using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Argument
{
    public class Argument
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool Required { get; set; } = true;

        public string Default { get; set; } = "";

        public string Value { get; set; }
    }
}
