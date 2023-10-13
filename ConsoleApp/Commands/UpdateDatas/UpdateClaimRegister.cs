using BusinessObject;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.UpdateDatas
{
    public class UpdateClaimRegister : Command
    {
        public bool IsAddChecklist { get; set; }
        public bool IsMapSoaData { get; set; }
        public bool IsSetDirectRetroProvisionStatus { get; set; }
        public bool IsSetCeoStatus { get; set; }

        public UpdateClaimRegister()
        {
            Title = "UpdateClaimRegister";
            Description = "To update claim registers";
            Options = new string[] {
                "--c|addChecklist : Add Checklist",
                "--s|mapSoaData : Map SOA Data Batch",
                "--d|setDrStatus : Set Direct Retro Provision Status",
                "--r|resetCeoStatus : Reset Status based on newly added statuses",
            };
            Hide = true;
        }

        public override void Initial()
        {
            base.Initial();

            IsAddChecklist = IsOption("addChecklist");
            IsMapSoaData = IsOption("mapSoaData");
            IsSetDirectRetroProvisionStatus = IsOption("setDrStatus");
            IsSetCeoStatus = IsOption("resetCeoStatus");
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                int total = db.ClaimRegister.Count();
                int processed = 0;
                int moduleId = db.Modules.Where(q => q.Controller == ModuleBo.ModuleController.ClaimRegister.ToString()).FirstOrDefault().Id;

                while (processed < total)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }
                    SetProcessCount();

                    var claimRegister = db.ClaimRegister.OrderBy(q => q.Id).Skip(processed).FirstOrDefault();
                    if (claimRegister != null)
                    {
                        if (IsAddChecklist && !string.IsNullOrEmpty(claimRegister.ClaimCode))
                        {
                            claimRegister.Checklist = ClaimChecklistDetailService.GetJsonByClaimCode(claimRegister.ClaimCode);

                            db.Entry(claimRegister).State = EntityState.Modified;

                            SetProcessCount("Added Checklist");
                        }

                        if (IsMapSoaData && !claimRegister.SoaDataBatchId.HasValue && claimRegister.ClaimDataBatchId.HasValue)
                        {
                            var claimDataBatch = claimRegister.ClaimDataBatch;
                            if (claimDataBatch.SoaDataBatchId.HasValue)
                            {
                                claimRegister.SoaDataBatchId = claimDataBatch.SoaDataBatchId;

                                db.Entry(claimRegister).State = EntityState.Modified;

                                SetProcessCount("Mapped SOA Data");
                            }
                        }

                        if (IsSetDirectRetroProvisionStatus)
                        {
                            if (string.IsNullOrEmpty(claimRegister.RetroParty1))
                            {
                                claimRegister.DrProvisionStatus = ClaimRegisterBo.DrProvisionStatusPending;
                                SetProcessCount("DR Provision Status Pending");
                            }
                            else
                            {
                                claimRegister.DrProvisionStatus = ClaimRegisterBo.DrProvisionStatusSuccess;
                                SetProcessCount("DR Provision Status Success");
                            }

                            db.Entry(claimRegister).State = EntityState.Modified;
                        }

                        if (IsSetCeoStatus)
                        {
                            if (claimRegister.ClaimStatus > 11)
                            {
                                bool reverted = false;
                                var statusHistory = db.StatusHistories.Where(q => q.ModuleId == moduleId && q.ObjectId == claimRegister.Id).OrderByDescending(q => q.CreatedAt).FirstOrDefault();
                                if (statusHistory.Status != claimRegister.ClaimStatus)
                                {
                                    claimRegister.ClaimStatus = statusHistory.Status;
                                    db.Entry(claimRegister).State = EntityState.Modified;
                                    SetProcessCount("Status Reverted");

                                    reverted = true;
                                }

                                if (statusHistory.CreatedAt > DateTime.Today || claimRegister.ClaimStatus == 19)
                                {
                                    processed++;
                                    continue;
                                }
                                
                                if (claimRegister.ClaimStatus == 17)
                                    claimRegister.ClaimStatus += 2;
                                else
                                    claimRegister.ClaimStatus++;

                                //if (statusHistory.Status != claimRegister.ClaimStatus)
                                //{
                                //    var newStatusHistory = new StatusHistory
                                //    {
                                //        ModuleId = moduleId,
                                //        ObjectId = claimRegister.Id,
                                //        Status = claimRegister.ClaimStatus,
                                //        CreatedById = User.DefaultSuperUserId,
                                //        UpdatedById = User.DefaultSuperUserId,
                                //    };

                                //    db.StatusHistories.Add(newStatusHistory);
                                //}

                                db.Entry(claimRegister).State = EntityState.Modified;

                                SetProcessCount("Status Updated");
                            }
                        }
                    }

                    processed++;
                }
                db.SaveChanges();

                if (processed > 0)
                    PrintProcessCount();
            }

            PrintEnding();
        }
    }
}
