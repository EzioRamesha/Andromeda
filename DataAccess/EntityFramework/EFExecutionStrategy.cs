using Shared;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace DataAccess.EntityFramework
{
    public class EFExecutionStrategy : DbExecutionStrategy
    {
        static int maxRetryCount = Util.GetConfigInteger("RetryCount") == 0 ? 5 : Util.GetConfigInteger("RetryCount");
        static int maxRetryInterval = Util.GetConfigInteger("RetryInterval") == 0 ? 20000 : Util.GetConfigInteger("RetryInterval");

        public int retryCount { get; set; }
        public string Process { get; set; }

        public bool RiDataProcessingFlag { get; set; } = false;
        public bool RiDataRowProcessingFlag { get; set; } = false;

        public int LineNumber { get; set; } = 0;

        public string ConnectionStrategyKey { get; set; }

        public static Logger multiThreadLogger;

        /// <summary>
        /// The default retry limit is 5, which means that the total amount of time spent 
        /// between retries is 26 seconds plus the random factor.
        /// </summary>
        public EFExecutionStrategy() : base(maxRetryCount, new TimeSpan(maxRetryInterval))
        {
        }

        /// <summary>
        /// Creates a new instance of "PharylonExecutionStrategy" with the specified limits for
        /// number of retries and the delay between retries.
        /// </summary>
        /// <param name="maxRetryCount"> The maximum number of retry attempts. </param>
        /// <param name="maxDelay"> The maximum delay in milliseconds between retries. </param>
        public EFExecutionStrategy(int maxRetryCount, TimeSpan maxDelay)
            : base(maxRetryCount, maxDelay)
        {
        }

        public static EFExecutionStrategy GetNewInstance(string process = null, int lineNumber = 0)
        {
            EFExecutionStrategy connectionStrategy = new EFExecutionStrategy();
            connectionStrategy.Process = process;
            connectionStrategy.Reset(lineNumber);

            return connectionStrategy;
        }

        public static EFExecutionStrategy GetNewRiDataProcessingInstance(int lineNumber = 0, string process = "RiData", bool riDataFlag = false)
        {
            EFExecutionStrategy connectionStrategy = GetNewInstance(process, lineNumber);
            connectionStrategy.RiDataProcessingFlag = true;
            connectionStrategy.ConnectionStrategyKey = Guid.NewGuid().ToString();

            if (riDataFlag)
            {
                connectionStrategy.RiDataRowProcessingFlag = riDataFlag;
                multiThreadLogger = Logger.GetLogger();
            }

            return connectionStrategy;
        }

        public static EFExecutionStrategy GetNewRiDataRowProcessingInstance(int lineNumber = 0, string process = "RiData")
        {
            EFExecutionStrategy connectionStrategy = GetNewRiDataProcessingInstance(lineNumber, process, true);
            return connectionStrategy;
        }

        public void Reset(int? lineNumber = null)
        {
            retryCount = 0;
            if (lineNumber != null)
            {
                LineNumber = lineNumber ?? 0;
            }
        }

        protected override bool ShouldRetryOn(Exception ex)
        {
            string refMsg = string.Empty;
            bool retry = false;
            if (ex.InnerException != null && ex is AggregateException aex
                && ((aex.InnerExceptions.Any(a => a is TimeoutException))
                || (aex.InnerExceptions.Any(a => a is SqlException))
                || (aex.InnerExceptions.Any(a => a is CommitFailedException || (a.HResult == -2146232032 || a.HResult == -2146233087)))))
            {
                retry = true;
            }
            else
            {
                SqlException sqlException = ex as SqlException;
                CommitFailedException comException = ex as CommitFailedException;

                if (sqlException != null || ex is TimeoutException || (comException != null && (comException?.HResult == -2146232032 || comException?.HResult == -2146233087)))
                {
                    retry = true;
                }
            }

            if (RiDataProcessingFlag)
            {
                var batchId = GlobalProcessRiDataConnectionStrategy.GetRiDataBatchId() == null ? 0 : GlobalProcessRiDataConnectionStrategy.GetRiDataBatchId();
                refMsg += $" Batch Id : {batchId}, ";

                if (RiDataRowProcessingFlag)
                {
                    var riDataId = GlobalProcessRowRiDataConnectionStrategy.GetRiDataId() == null ? 0 : GlobalProcessRowRiDataConnectionStrategy.GetRiDataId();
                    refMsg += $"Ri Data Id : {riDataId} ";
                }
            }

            if (retry)
            {
                string logMessage = $" Retrying method : {Process}, line : {LineNumber}. {refMsg} \n. Retry for ({retryCount}) times. Exception : {ex} \n";

                if (!RiDataRowProcessingFlag)
                    LogHelper(logMessage);
                else
                {
                    LogObject log = new LogObject()
                    {
                        Key = ConnectionStrategyKey,
                        CreatedDate = DateTime.Now,
                        Message = DateTime.Now.ToString("dd MMM yyyy HH:mm:ss:ffff") + logMessage
                    };
                    multiThreadLogger.AddLog(log);
                }
                retryCount++;
            }
            else
            {
                LogHelper($"Cant retry on error : {ex.HResult}, {ex.Message} \n . On Method {Process}, line : {LineNumber}. {refMsg} \n Retry ended at {retryCount} times \n");
                retryCount = 0;
            }
            return retry;
        }

        protected override TimeSpan? GetNextDelay(Exception ex)
        {
            if (retryCount < maxRetryCount)
            {
                return TimeSpan.FromMilliseconds(maxRetryInterval);
            }

            return null;
        }

        public static void LogHelper(string message, bool dateOverrideFlag = false)
        {
            string filePath = string.Format("{0}/Resiliency".AppendDateFileName(".txt"), Util.GetLogPath("RetryLog"));

            Util.MakeDir(filePath);

            if (Util.GetConfigBoolean("ConnectionStrategyLogging"))
            {
                try
                {
                    File.AppendAllText(filePath, dateOverrideFlag ? message : DateTime.Now.ToString("dd MMM yyyy HH:mm:ss:ffff") + " " + message /*String.Format("{0}[{1}] {2}\n", dt, process, message*/);
                    File.AppendAllText(filePath, "\n");
                    File.AppendAllText(filePath, "\n");
                }
                catch { }
            }
        }

        #region archieves
        //protected override bool ShouldRetryOn(Exception ex)
        //{
        //    bool retry = false;
        //    SqlException sqlException = ex as SqlException;

        //    if (sqlException != null)
        //    {
        //        int[] errorsToRetry =
        //        {
        //            1205,        //Deadlock
        //            -2,          //Timeout
        //            -2146232060, // network - related error
        //            22840,
        //            23414,
        //            10060,
        //            10053,      // transport - level
        //            10054,
        //            19
        //        };
        //        if (sqlException.Errors.Cast<SqlError>().Any(x => errorsToRetry.Contains(x.Number)))
        //        {
        //            retry = true;
        //        }
        //        else
        //        {
        //            //Add some error logging on this line for errors we aren't retrying.
        //            //Make sure you record the Number property of sqlError. 
        //            //If you see an error pop up that you want to retry, you can look in 
        //            //your log and add that number to the list above.

        //            LogHelper($"{sqlException.ErrorCode}, {sqlException.Message}, {sqlException.StackTrace}");
        //        }
        //    }
        //    else
        //    {
        //        //Add some error logging on this line for errors we aren't retrying.
        //        //Make sure you record the Number property of sqlError. 
        //        //If you see an error pop up that you want to retry, you can look in 
        //        //your log and add that number to the list above.

        //        LogHelper($"{ex.HResult}, {ex.Message}, {ex.StackTrace}");
        //    }

        //    if (ex is TimeoutException)
        //    {
        //        retry = true;
        //    }

        //    if (retry)
        //        LogHelper($"Retrying method on {new StackTrace(ex).GetFrame(0).GetMethod().Name}, Exception : {ex}");

        //    return retry;
        //}

        //protected override TimeSpan? GetNextDelay(Exception ex)
        //{
        //    _exceptionsEncountered.Add(lastException);

        //    var currentRetryCount = _exceptionsEncountered.Count - 1;
        //    if (currentRetryCount < _maxRetryCount)
        //    {
        //        var delta = (Math.Pow(DefaultExponentialBase, currentRetryCount) - 1.0)
        //                    * (1.0 + _random.NextDouble() * (DefaultRandomFactor - 1.0));

        //        var delay = Math.Min(
        //            DefaultCoefficient.TotalMilliseconds * delta,
        //            _maxDelay.TotalMilliseconds);

        //        return TimeSpan.FromMilliseconds(delay);
        //    }

        //    return null;
        //}
        #endregion
    }
}