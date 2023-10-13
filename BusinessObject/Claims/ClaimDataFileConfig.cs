using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Claims
{
    public class ClaimDataFileConfig
    {
        public const int ROW_TYPE_HEADER = 1;
        public const int ROW_TYPE_DATA = 2;

        public bool HasHeader { get; set; } = false;

        public int? HeaderRow { get; set; }

        public string Worksheet { get; set; }

        public int? Delimiter { get; set; }

        public string DelimiterName { get; set; }

        public bool RemoveQuote { get; set; } = false;

        public int? StartRow { get; set; }

        public int? EndRow { get; set; }

        public int? StartColumn { get; set; }

        public int? EndColumn { get; set; }

        public bool IsColumnToRowMapping { get; set; }

        public int? NumberOfRowMapping { get; set; }

        public bool IsDataCorrection { get; set; }

        public bool IsAbleColumnToRowMapping()
        {
            return IsColumnToRowMapping && NumberOfRowMapping > 1;
        }

        public bool IsDefineHeaderRow()
        {
            return HasHeader && HeaderRow != null && HeaderRow != 0;
        }

        public bool IsDefineStartRow()
        {
            return StartRow != null && StartRow != 0;
        }

        public bool IsDefineEndRow()
        {
            return EndRow != null && EndRow != 0;
        }

        public bool IsToSkipRow(int? row)
        {
            if (IsDefineHeaderRow())
            {
                if (row < HeaderRow)
                {
                    return true;
                }
            }
            if (row != HeaderRow && IsDefineStartRow())
            {
                if (row < StartRow)
                {
                    return true;
                }
            }
            return false;
        }

        public int DefineRowType(int? row)
        {
            if (IsDefineHeaderRow() && IsDefineStartRow())
            {
                if (row < HeaderRow)
                {
                    return 0;
                }
                else if (row == HeaderRow)
                {
                    return ROW_TYPE_HEADER;
                }
                else
                {
                    if (row >= StartRow)
                    {
                        if (IsDefineEndRow() && row > EndRow)
                        {
                            return 0;
                        }
                        return ROW_TYPE_DATA;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else if (IsDefineHeaderRow())
            {
                if (row < HeaderRow)
                {
                    return 0;
                }
                else if (row == HeaderRow)
                {
                    return ROW_TYPE_HEADER;
                }
                else if (IsDefineEndRow() && row > EndRow)
                {
                    return 0;
                }
                else
                {
                    return ROW_TYPE_DATA;
                }
            }
            else if (IsDefineStartRow())
            {
                if (row >= StartRow)
                {
                    return ROW_TYPE_DATA;
                }
                else if (IsDefineEndRow() && row > EndRow)
                {
                    return 0;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (IsDefineEndRow() && row > EndRow)
                {
                    return 0;
                }
                return ROW_TYPE_DATA;
            }
        }

        public string GetNoDataMessage()
        {
            string message;
            if (StartRow > 0 && EndRow > 0)
                message = string.Format("There are no data found between {0} to {1} row", StartRow, EndRow);
            else if (StartRow > 0)
                message = string.Format("There are no data found start from {0} row", StartRow);
            else
                message = string.Format("There are no data found from file");
            return message;
        }
    }
}
