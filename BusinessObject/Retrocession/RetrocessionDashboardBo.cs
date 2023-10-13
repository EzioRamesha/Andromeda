using System.Collections.Generic;

namespace BusinessObject.Retrocession
{
    public class RetrocessionDashboardBo
    {
        public string RetroTreaty { get; set; }

        public string Task { get; set; }

        public string Q1 { get; set; }

        public string Q2 { get; set; }

        public string Q3 { get; set; }

        public string Q4 { get; set; }

        public int CedantId { get; set; }

        public string CedingCompany { get; set; }

        public string SoaQuarter { get; set; }

        public int? NoOfCompletedSOA { get; set; }

        public int? NoOfIncompleteRetroStatement { get; set; }

        public string TreatyCode { get; set; }

        public int DirectRetroStatus { get; set; }

        public int DirectRetroId { get; set; }

        public RetrocessionDashboardBo() { }

        public RetrocessionDashboardBo(string retroTreatyCode, int task)
        {
            RetroTreaty = retroTreatyCode;
            Task = GetTaskName(task);
        }

        public const int TaskRiDataWarehouseSnapshot = 1;
        public const int TaskClaimRegisterSnapshot = 2;
        public const int TaskPremiumAggregation = 3;
        public const int TaskClaimProcessing = 4;
        public const int TaskSoa = 5;
        public const int TaskRetroRegister = 6;

        public const int TaskMax = 6;

        public static string GetTaskName(int key)
        {
            switch (key)
            {
                case TaskRiDataWarehouseSnapshot:
                    return "RI Warehouse Snapshot";
                case TaskClaimRegisterSnapshot:
                    return "Claim Register Snapshot";
                case TaskPremiumAggregation:
                    return "Premium Aggregation";
                case TaskClaimProcessing:
                    return "Claim Processing";
                case TaskSoa:
                    return "SOA";
                case TaskRetroRegister:
                    return "Retro Register";
                default:
                    return "";
            }
        }

        public static List<int> GetTaskList()
        {
            return new List<int>
            {
                TaskRiDataWarehouseSnapshot,
                TaskClaimRegisterSnapshot,
                TaskPremiumAggregation,
                TaskClaimProcessing,
                TaskSoa,
                TaskRetroRegister
            };
        }
    }
}
