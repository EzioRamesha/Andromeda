using BusinessObject.Identity;
using BusinessObject.Sanctions;
using DataAccess.Entities.Identity;
using Services.Identity;
using Services.Sanctions;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.Sanction
{
    public class ProcessSanctionFile : Command
    {
        public SourceBo SourceBo;

        public ProcessSanctionFile()
        {
            Title = "ProcessSanctionFile";
            Description = "To process UN OFAC files in directory";
        }

        public override void Run()
        {
            try
            {
                PrintStarting();

                int totalFiles = 0;
                totalFiles += ProcessSource(SourceBo.TypeUN);
                totalFiles += ProcessSource(SourceBo.TypeOFAC);

                PrintMessage(string.Format("Total files uploaded: {0}", totalFiles));
            }
            catch (Exception e)
            {
                PrintError(e.Message);
            }

            PrintEnding();
        }

        public int ProcessSource(int type)
        {
            SourceBo = SourceService.Find(type);
            if (SourceBo == null)
                return 0;

            int fileCount = 0;

            string directory = Util.GetSanctionPath(SourceBo.Name);
            if (!Directory.Exists(directory))
            {
                PrintMessage(string.Format("Directory does not exist: {0}", directory));
                return 0;
            }

            string[] filePaths = Directory.GetFiles(directory, "*.csv", SearchOption.TopDirectoryOnly);
            foreach (string filePath in filePaths)
            {
                CreateSanctionBatch(filePath);
                fileCount++;
            }

            PrintMessage(string.Format("{0} files uploaded: {1}", SourceBo.Name, fileCount));

            return fileCount;
        }

        public void CreateSanctionBatch(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            SanctionBatchBo bo = new SanctionBatchBo
            {
                Method = SanctionBatchBo.MethodReplacement,
                SourceId = SourceBo.Id,
                FileName = fileName,
                Status = SanctionBatchBo.StatusPending,
                UploadedAt = DateTime.Now,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId
            };

            bo.FormatHashFileName();

            string path = bo.GetLocalPath();
            Util.MakeDir(path);
            File.Move(filePath, path);

            var trail = new TrailObject();
            Result result = SanctionBatchService.Create(ref bo, ref trail);
            if (result.Valid)
            {
                var userTrailBo = new UserTrailBo(
                    bo.Id,
                    "Create New Sanction Batch",
                    result,
                    trail,
                    User.DefaultSuperUserId
                );
                UserTrailService.Create(ref userTrailBo);
            }
        }
    }
}
