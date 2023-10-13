using System.Collections.Generic;

namespace Shared.ProcessFile
{
    public class Row
    {
        public int RowIndex { get; set; }

        public List<Column> Columns { get; set; }

        public bool IsEmpty { get; set; }

        public bool IsLineContainDelimiter { get; set; } = false;

        public bool IsEnd { get; set; } = false;
    }
}
