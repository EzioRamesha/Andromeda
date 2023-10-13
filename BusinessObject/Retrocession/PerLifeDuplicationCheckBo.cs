using Shared.ProcessFile;
using System;
using System.Collections.Generic;

namespace BusinessObject.Retrocession
{
    public class PerLifeDuplicationCheckBo
    {
        public int Id { get; set; }

        public string ConfigurationCode { get; set; }

        public string Description { get; set; }

        public bool Inclusion { get; set; }

        public DateTime? ReinsuranceEffectiveStartDate { get; set; }
        public string ReinsuranceEffectiveStartDateStr { get; set; }

        public DateTime? ReinsuranceEffectiveEndDate { get; set; }
        public string ReinsuranceEffectiveEndDateStr { get; set; }

        public string TreatyCode { get; set; }
        public int NoOfTreatyCode { get; set; }

        public bool EnableReinsuranceBasisCodeCheck { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public static List<Column> GetColumns()
        {
            return new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    Property = "Id",
                },
                new Column
                {
                    Header = "Configuration Code",
                    Property = "ConfigurationCode",
                },
                new Column
                {
                    Header = "Description",
                    Property = "Description",
                },
                new Column
                {
                    Header = "Inclusion",
                    Property = "Inclusion",
                },
                new Column
                {
                    Header = "Reinsurance Effective Start Date",
                    Property = "ReinsuranceEffectiveStartDate",
                },
                new Column
                {
                    Header = "Reinsurance Effective End Date",
                    Property = "ReinsuranceEffectiveEndDate",
                },
                new Column
                {
                    Header = "Enable Reinsurance Basis Code Check",
                    Property = "EnableReinsuranceBasisCodeCheck",
                },
                new Column
                {
                    Header = "No of Treaty Code(s)",
                    Property = "NoOfTreatyCode",
                },
                new Column
                {
                    Header = "Treaty Code(s)",
                    Property = "TreatyCode",
                },
            };
        }
    }
}
