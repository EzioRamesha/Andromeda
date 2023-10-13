using System.Collections.Generic;

namespace Shared.ProcessFile
{
    public interface IProcessFile
    {
        int? GetNextRow();

        List<Row> GetNextRows(int noOfRows = 1);

        int? GetNextCol();

        dynamic GetValue();

        dynamic GetValue2();

        int? GetRowIndex();

        int? GetColIndex();

        int? GetTotalCol();

        int? GetDelimiter();

        void ResetColLengths();

        void AddColLengths(int length);

        void Close();
    }
}
