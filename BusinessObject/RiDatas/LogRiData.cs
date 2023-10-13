using Shared;

namespace BusinessObject.RiDatas
{
    public class LogRiData
    {
        public int Row { get; set; }
        public int Index { get; set; }
        public int RiDataBatchId { get; set; }
        public int RiDataFileId { get; set; }

        public LogLine Mapping { get; set; } = new LogLine();
        public LogLine MappingCustomField { get; set; } = new LogLine();
        public LogLine FixedValue { get; set; } = new LogLine();
        public LogLine OverrideProperty { get; set; } = new LogLine();
        public LogLine RemoveSalutation { get; set; } = new LogLine();
        public LogLine DataCorrection { get; set; } = new LogLine();
        public LogLine Computation { get; set; } = new LogLine();
        public LogLine Formula { get; set; } = new LogLine();
        public LogLine Validation { get; set; } = new LogLine();

        public void SetEnabled(bool enabled = true)
        {
            Mapping.Enabled = enabled;
            MappingCustomField.Enabled = enabled;
            FixedValue.Enabled = enabled;
            OverrideProperty.Enabled = enabled;
            RemoveSalutation.Enabled = enabled;
            DataCorrection.Enabled = enabled;
            Computation.Enabled = enabled;
            Formula.Enabled = enabled;
            Validation.Enabled = enabled;
        }

        public void SetDetailWidth(int width = 30)
        {
            Mapping.DetailWidth = width;
            MappingCustomField.DetailWidth = width;
            FixedValue.DetailWidth = width;
            OverrideProperty.DetailWidth = width;
            DataCorrection.DetailWidth = width;
            Computation.DetailWidth = width;
            Formula.DetailWidth = width;
            Validation.DetailWidth = width;
        }
    }
}
