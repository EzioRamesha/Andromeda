using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace BusinessObject
{
    public class StoredProcedure
    {
        public int Type { get; set; }

        public string TypeName { get; set; }

        public SqlConnection Connection { get; set; }

        public List<SqlParameter> Parameters { get; set; }

        public Dictionary<string, string> Outputs { get; set; }

        public static Action<object, bool, bool?> PrintFunction { get; set; }

        public string Result { get; set; }

        public Dictionary<string, int> ResultList { get; set; }

        public string ReturnResult { get; set; }

        public bool Success { get; set; }

        public const int RiDataFinalise = 1;
        public const int RiDataMigrateWarehouse = 2;
        public const int SanctionVerification = 3;
        public const int SanctionVerificationSearch = 4;
        public const int PerLifeDataValidation = 5;
        public const int PerLifeSplitData = 6;
        public const int PerLifeAggregate = 7;
        public const int PerLifeDetailDataValidation = 8;
        public const int PerLifeClaimsProcessing = 9;
        public const int PerLifeClaimDataValidation = 10;
        public const int PerLifeClaimsRetroRecoveryProcess = 11;
        public const int PerLifeSoaProcessing = 12;
        public const int DeletePerLifeAggregation = 13;
        public const int AddRiDataPartition = 14;
        public const int RemoveRiDataPartition = 15;
        public const int AddRiDataWarehouseHistoryPartition = 16;
        public const int ProcessCutOffRiDataWarehouse = 17;
        public const int ProcessCutOffRiDataWarehouseRecover = 18;

        public StoredProcedure(int type)
        {
            Type = type;
            TypeName = GetTypeName(type);

            string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            Connection = new SqlConnection(connectionString);
        }

        public static string GetTypeName(int type)
        {
            switch (type)
            {
                case RiDataFinalise:
                    return "RiDataFinalise";
                case RiDataMigrateWarehouse:
                    return "RiDataMigrateWarehouse";
                case SanctionVerification:
                    return "SanctionVerification";
                case SanctionVerificationSearch:
                    return "SanctionVerificationSearch";
                case PerLifeDataValidation:
                    return "PerLifeDataValidation";
                case PerLifeSplitData:
                    return "PerLifeSplitData";
                case PerLifeAggregate:
                    return "PerLifeAggregate";
                case PerLifeDetailDataValidation:
                    return "ValidatePerLifeDetail";
                case PerLifeClaimsProcessing:
                    return "PerLifeClaimsProcessing";
                case PerLifeClaimDataValidation:
                    return "PerLifeClaimDataValidation";
                case PerLifeClaimsRetroRecoveryProcess:
                    return "PerLifeClaimsRetroRecoveryProcess";
                case PerLifeSoaProcessing:
                    return "PerLifeSoaProcessing";
                case DeletePerLifeAggregation:
                    return "DeletePerLifeAggregation";
                case AddRiDataPartition:
                    return "AddRiDataPartition";
                case RemoveRiDataPartition:
                    return "RemoveRiDataPartition";
                case AddRiDataWarehouseHistoryPartition:
                    return "AddRiDataWarehouseHistoryPartition";
                case ProcessCutOffRiDataWarehouse:
                    return "ProcessCutOffRiDataWarehouse";
                case ProcessCutOffRiDataWarehouseRecover:
                    return "ProcessCutOffRiDataWarehouseRecover";
                default:
                    return "";
            }
        }

        public bool IsExists()
        {
            bool isExists = false;
            string query = string.Format("SELECT COUNT(*) FROM SYSOBJECTS WHERE type='P' AND name='{0}'", TypeName);
            using (SqlCommand cmd = Connection.CreateCommand())
            {
                Connection.Open();

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = query;
                int count = (int)cmd.ExecuteScalar();

                isExists = count > 0;

                Connection.Close();
            }


            return isExists;
        }

        public void Execute(bool hasResult = false, Action printFunction = null)
        {
            using (SqlCommand cmd = Connection.CreateCommand())
            {
                cmd.CommandTimeout = 0;
                try
                {
                    Success = true;
                    Connection.InfoMessage += new SqlInfoMessageEventHandler(InfoMessage);
                    Connection.Open();

                    if (hasResult)
                        AddParameter("Result", isOutputParam: true);

                    if (Parameters != null)
                        cmd.Parameters.AddRange(Parameters.ToArray());

                    var returnResult = cmd.Parameters.Add("ReturnResult", SqlDbType.VarChar);
                    returnResult.Direction = ParameterDirection.ReturnValue;

                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = TypeName;
                    if (PrintFunction != null)
                        cmd.ExecuteReader();
                    else
                        cmd.ExecuteNonQuery();

                    RetrieveOutput(cmd);
                    if (hasResult)
                        Result = Outputs["Result"];
                    ReturnResult = returnResult?.Value.ToString();

                    Connection.Close();
                }
                catch (Exception e)
                {
                    Success = false;
                    Result = e.Message;
                    Connection.Close();
                }
            }
        }

        public void RetrieveOutput(SqlCommand cmd)
        {
            Outputs = new Dictionary<string, string>();

            var outputParams = Parameters.Where(q => q.Direction == ParameterDirection.Output).ToList();
            if (outputParams.IsNullOrEmpty())
                return;

            foreach (var param in outputParams)
            {
                object value = cmd.Parameters[param.ParameterName]?.Value;
                if (value == null)
                    continue;

                string output = value.ToString();
                Outputs.Add(param.ParameterName, output);
            }
        }

        public void AddParameter(string name, object value = null, bool isOutputParam = false)
        {
            if (Parameters == null)
                Parameters = new List<SqlParameter>();

            SqlParameter sqlParameter;
            if (isOutputParam)
            {
                sqlParameter = new SqlParameter(name, SqlDbType.VarChar, -1);
                sqlParameter.Direction = ParameterDirection.Output;
            }
            else
            {
                sqlParameter = new SqlParameter(name, value);
            }

            Parameters = Parameters.Where(q => q.ParameterName != name).ToList();
            Parameters.Add(sqlParameter);
        }

        public string GetOutput(string name)
        {
            if (Outputs == null || !Outputs.ContainsKey(name))
                return null;

            return Outputs[name];
        }

        public bool ParseResult()
        {
            if (string.IsNullOrEmpty(Result))
                return false;

            try
            {
                ResultList = JsonConvert.DeserializeObject<Dictionary<string, int>>(Result);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            if (PrintFunction == null)
                return;

            PrintFunction.Invoke(e.Message, true, null);
        }
    }
}
