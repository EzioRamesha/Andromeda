using BusinessObject;
using DataAccess.EntityFramework;
using Services;
using Shared;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace ConsoleApp.Commands.RawFiles
{
    public class ProcessRawFile : Command
    {
        public RawFileBo RawFileBo { get; set; }

        public AppDbContext DB { get; set; }

        public bool Delete { get; set; } = false;

        public bool NoBackup { get; set; } = false;

        public ProcessRawFile()
        {
            Title = "ProcessRawFile";
            Description = "To process raw file";
            Options = new string[] {
                "--d|delete : To delete missing file",
                "--n|noBackup : No backup to Temporary folder",
            };
            Hide = true;
        }

        public override void Initial()
        {
            base.Initial();

            Delete = IsOption("delete");
            NoBackup = IsOption("noBackup");
        }

        public override void Run()
        {
            PrintStarting();

            DB = new AppDbContext();

            CopyToTemporary();
            DeleteAllFiles();
            CopyFromTemporary();
            DeleteAllFilesFromTemporary();

            DB.Dispose();
            PrintEnding();
        }

        public void CopyToTemporary()
        {
            ResetProcessCount();

            PrintMessage();
            PrintMessage("CopyToTemporary");

            int count = DB.RawFiles.Count();
            int processed = 0;
            DB.RawFiles.AsQueryable();

            while (processed < count)
            {
                if (PrintCommitBuffer())
                {
                }
                SetProcessCount();

                var rawFile = DB.RawFiles.OrderBy(q => q.Id).Skip(processed).FirstOrDefault();
                if (rawFile != null)
                {
                    RawFileBo = RawFileService.FormBo(rawFile);
                    var localPath = RawFileBo.GetLocalPath();
                    var tempPath = RawFileBo.GetTemporaryPath();

                    if (File.Exists(localPath))
                    {
                        Util.MakeDir(tempPath);
                        File.Copy(localPath, tempPath, true);

                        SetProcessCount("Copied");
                    }
                    else
                    {
                        SetProcessCount("RawFile Not Exist");
                    }
                }

                processed++;
            }
            PrintProcessCount();
        }

        public void CopyFromTemporary()
        {
            if (!Delete)
                return;

            ResetProcessCount();

            PrintMessage();
            PrintMessage("CopyFromTemporary");

            int count = DB.RawFiles.Count();
            int processed = 0;
            while (processed < count)
            {
                if (PrintCommitBuffer())
                {
                }
                SetProcessCount();

                var rawFile = DB.RawFiles.OrderBy(q => q.Id).Skip(processed).FirstOrDefault();
                if (rawFile != null)
                {
                    RawFileBo = RawFileService.FormBo(rawFile);
                    var localPath = RawFileBo.GetLocalPath();
                    var tempPath = RawFileBo.GetTemporaryPath();

                    Util.MakeDir(localPath);
                    Util.MakeDir(tempPath);
                    File.Copy(tempPath, localPath, true);

                    SetProcessCount("Copied From Temporary");

                    int countFileExists = 0;
                    switch (RawFileBo.Type)
                    {
                        case RawFileBo.TypeRiData:
                            countFileExists = DB.RiDataFiles.Where(rdf => rdf.RawFileId == rawFile.Id).Count();
                            break;
                    }
                    bool delete = countFileExists == 0;

                    if (delete)
                    {
                        File.Delete(localPath);
                        DB.Entry(rawFile).State = EntityState.Deleted;
                        DB.RawFiles.Remove(rawFile);
                        DB.SaveChanges();

                        SetProcessCount(string.Format("{0} Deleted", RawFileBo.GetTypeName(RawFileBo.Type)));
                    }
                }

                var newCount = DB.RawFiles.Count();
                if (newCount != count)
                {
                    count = newCount;
                }
                else
                {
                    processed++;
                }
            }
            PrintProcessCount();
        }

        public void DeleteAllFiles()
        {
            if (!Delete)
                return;

            ResetProcessCount();

            PrintMessage();
            PrintMessage("DeleteAllFiles");

            var riDataDirectory = Util.GetRawFilePath(RawFileBo.GetTypeName(RawFileBo.TypeRiData));
            string[] fileEntries = Directory.GetFiles(riDataDirectory);
            foreach (string fileName in fileEntries)
            {
                var fileInfo = new FileInfo(fileName);
                fileInfo.Delete();

                SetProcessCount("Deleted From RawFile\\RiData folder");
            }
            PrintProcessCount();
        }

        public void DeleteAllFilesFromTemporary()
        {
            if (!Delete)
                return;

            ResetProcessCount();

            PrintMessage();
            PrintMessage("DeleteAllFilesFromTemporary");

            var riDataDirectory = Util.GetTemporaryPath(RawFileBo.GetTypeName(RawFileBo.TypeRiData));
            string[] fileEntries = Directory.GetFiles(riDataDirectory);
            foreach (string fileName in fileEntries)
            {
                var fileInfo = new FileInfo(fileName);
                fileInfo.Delete();

                SetProcessCount("Deleted From Temporary\\RiData folder");
            }
            PrintProcessCount();

            Directory.Delete(riDataDirectory);
        }
    }
}
