using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ConsoleApp.Commands
{
    public class ExecuteSqlFile : Command
    {
        public SqlConnection Connection { get; set; }

        public string FilePath { get; set; }

        public ExecuteSqlFile()
        {
            Title = "ExecuteSqlFile";
            Description = "To Execute SQL Files";
            Arguments = new string[] {
                "--path= : SQL File Path",
            };
            Hide = true;
        }

        public override void Initial()
        {
            base.Initial();

            FilePath = Option("path");

            string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            Connection = new SqlConnection(connectionString);
        }

        public override void Run()
        {
            if (!File.Exists(FilePath))
            {
                PrintMessage(string.Format("File does not exist: {0}", FilePath));
                return;
            }

            PrintStarting();
            using (SqlCommand cmd = Connection.CreateCommand())
            {
                Connection.Open();
                Connection.InfoMessage += GetInfoMessage;

                try
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = File.ReadAllText(FilePath);
                    cmd.ExecuteNonQuery();

                    PrintMessage(string.Format("File Run Completed: {0}", FilePath));
                }
                catch (Exception e)
                {
                    PrintMessage(e.Message);
                }

                Connection.Close();
            }

            PrintEnding();
        }

        void GetInfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            PrintMessage(e.Message);
        }
    }
}
