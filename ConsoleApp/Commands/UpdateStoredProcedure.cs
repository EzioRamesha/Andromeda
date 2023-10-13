using Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands
{
    class UpdateStoredProcedure : Command
    {
        public SqlConnection Connection { get; set; }

        public UpdateStoredProcedure()
        {
            Title = "UpdateStoredProcedure";
            Description = "To Execute SQL Files in Stored Procedure folder";
            Hide = true;
        }

        public override void Initial()
        {
            base.Initial();

            string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            Connection = new SqlConnection(connectionString);
        }

        public override void Run()
        {
            string directory = Util.GetConfig("StoredProcedurePath");
            if (!Directory.Exists(directory))
            {
                Log = false;
                PrintMessage(string.Format("Directory does not exist: {0}", directory));
                return;
            }

            PrintStarting();
            using (SqlCommand cmd = Connection.CreateCommand())
            {
                Connection.Open();
                Connection.InfoMessage += GetInfoMessage;

                string[] subFolders = Directory.GetDirectories(directory).OrderByDescending(q => q).ToArray();
                foreach (string folder in subFolders)
                {
                    PrintLine();

                    string folderName = Path.GetFileName(folder);
                    PrintMessage(string.Format("Entering Folder: {0}", folderName));

                    string[] filePaths = Directory.GetFiles(folder, "*.sql", SearchOption.TopDirectoryOnly);
                    foreach (string filePath in filePaths)
                    {
                        string fileName = Path.GetFileName(filePath);
                        if (fileName.StartsWith("PartitionExisting"))
                            continue;

                        try
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.Text;

                            cmd.CommandText = File.ReadAllText(filePath);
                            cmd.ExecuteNonQuery();

                            PrintMessage(string.Format("File Run Completed: {0}", fileName));
                        }
                        catch (Exception e)
                        {
                            PrintError(string.Format("File Contains Error: {0}", fileName));
                            PrintError(e.Message);
                        }
                    }
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
