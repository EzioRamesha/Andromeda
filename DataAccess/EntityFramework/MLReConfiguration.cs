using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.EntityFramework
{
    public class MLReConfiguration : DbConfiguration
    {
        public MLReConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new EFExecutionStrategy());
        }
    }

    public static class GlobalProcessRiDataConnectionStrategy
    {
        static int? RiDataBatchId { get; set; }

        public static void SetRiDataBatchId(int value)
        {
            RiDataBatchId = value;
        }

        public static int? GetRiDataBatchId()
        {
            return RiDataBatchId;
        }
    }

    public static class GlobalProcessRowRiDataConnectionStrategy
    {
        static int? RiDataId { get; set; }
        static List<int?> RiDataIds { get; set; } = null;

        public static void SetRiDataId(int id)
        {
            RiDataId = id;
        }

        public static void AddRiDataIds(int id)
        {
            if (RiDataIds.Contains(id))
                return;

            RiDataIds.Add(id);
        }

        public static int? GetRiDataId()
        {
            return RiDataId;
        }

        public static int? GetRiDataIds(int id)
        {
            return RiDataIds.Contains(id) ? id : 0;
        }
    }

    public sealed class Logger
    {
        private static readonly Logger _logger = new Logger();

        public static List<LogObject> Logs { get; set; }

        private Logger()
        {
            Logs = new List<LogObject>();
        }

        public static Logger GetLogger()
        {
            return _logger;
        }

        public void AddLog(LogObject model)
        {
            Logs.Add(model);
        }

        public void WriteLog()
        {
            if (Logs.Count <= 0)
                return;

            var messages = Logs.OrderBy(a => a.Key).ThenBy(a => a.CreatedDate).Select(a => a.Message);
            var stringMessages = string.Join(Environment.NewLine, messages);

            EFExecutionStrategy.LogHelper("---Start multithread retry log---");
            EFExecutionStrategy.LogHelper(stringMessages, true);
            EFExecutionStrategy.LogHelper("---End multithread retry log---");
        }
    }
}

public class LogObject
{
    public string Key { get; set; }
    public string Message { get; set; }
    public DateTime CreatedDate { get; set; }
}