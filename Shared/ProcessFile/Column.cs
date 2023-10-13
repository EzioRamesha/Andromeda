namespace Shared.ProcessFile
{
    public class Column
    {
        public string Header { get; set; }

        public int Type { get; set; }

        public int? RowIndex { get; set; }

        public int? ColIndex { get; set; }

        public string ColName { get; set; }

        public string Property { get; set; }

        public int Length { get; set; }

        public dynamic Value { get; set; }

        public dynamic Value2 { get; set; }

        public bool Editable { get; set; } = true;

        public bool Required { get; set; } = false;

        public bool IsExcel { get; set; } = false;

        public bool IsValueExist { get; set; } = false;

        public string GetDetails()
        {
            return string.Format(
                "\nR: {0}\nC: {1}\nV: {2}\nVDataType: {3}\nV2: {4}\nV2DataType: {5}",
                RowIndex,
                ColIndex,
                Value,
                Value != null ? Value.GetType() : Util.Null,
                Value2,
                Value2 != null ? Value2.GetType() : Util.Null
            );
        }

        public string GetCellDetails()
        {
            if (IsExcel)
            {
                return string.Format(
                    "R: {0}\nC: {1}\nCName: {2}\nV: {3}\nVDataType: {4}\nV2: {5}\nV2DataType: {6}",
                    RowIndex,
                    ColIndex,
                    ColName,
                    Value != null ? Value : Util.Null,
                    Value != null ? Value.GetType() : Util.Null,
                    Value2 != null ? Value2 : Util.Null,
                    Value2 != null ? Value2.GetType() : Util.Null
                );
            }
            return GetDetails();
        }
    }
}
