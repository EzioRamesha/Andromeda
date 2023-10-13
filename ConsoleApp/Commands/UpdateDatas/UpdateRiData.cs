using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Newtonsoft.Json.Serialization;
using Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.UpdateDatas
{
    public class UpdateRiData : Command
    {
        public UpdateRiData()
        {
            Title = "UpdateRiData";
            Description = "To update amount value from integer to double";
            Options = new string[] {
            };
            Hide = true;
        }

        public override void Run()
        {
            PrintStarting();

            string subSql = null;
            foreach (int a in StandardOutputBo.GetAmountTypes())
            {
                string propertyName = StandardOutputBo.GetPropertyNameByType(a);
                if (string.IsNullOrEmpty(subSql))
                {
                    subSql = string.Format("[{0}] = ([{0}] / 100)", propertyName);
                }
                else
                {
                    subSql += string.Format(", [{0}] = ([{0}] / 100)", propertyName);
                }
            }

            using (var db = new AppDbContext())
            {
                int total = db.RiData.Count();
                int processed = 0;
                int buffer = 10000;
                CommitLimit = buffer;

                while (processed < total)
                {
                    PrintCommitBuffer();
                    SetProcessCount(number: buffer, acc: true);

                    var innerQuery = "SELECT [Id] FROM [RiData] ORDER BY [Id] OFFSET " + processed + " ROWS FETCH NEXT " + buffer + " ROWS ONLY";
                    db.Database.ExecuteSqlCommand(string.Format("UPDATE [RiData] SET {0} WHERE [Id] IN ({1})", subSql, innerQuery));
                    processed += buffer;
                }

                if (processed > 0)
                    PrintProcessCount();
            }

            PrintEnding();
        }
    }
}
