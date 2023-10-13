using BusinessObject;
using DataAccess.Entities;
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
    public class UpdateMfrs17ReportingDetail : Command
    {
        public bool IsUpdateTreatyCode { get; set; }

        public bool IsUpdateStatus { get; set; }

        public UpdateMfrs17ReportingDetail()
        {
            Title = "UpdateMfrs17ReportingDetail";
            Description = "To update MFRS17 Reporting Detail";
            Options = new string[] {
                "--t|updateTreatyCode : Update Treaty Code",
                "--s|updateStatus : Update Status",
            };
            Hide = true;
        }
        public override void Initial()
        {
            base.Initial();

            IsUpdateTreatyCode = IsOption("updateTreatyCode");
            IsUpdateStatus = IsOption("updateStatus");
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                int total = db.Mfrs17ReportingDetails.Count();
                int processed = 0;

                while (processed < total)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }
                    SetProcessCount();

                    var mfrs17ReportingDetail = db.Mfrs17ReportingDetails.OrderBy(q => q.Id).Skip(processed).FirstOrDefault();
                    if (mfrs17ReportingDetail != null)
                    {
                        if (IsUpdateTreatyCode)
                        {
                            UpdateTreatyCode(ref mfrs17ReportingDetail);
                            SetProcessCount("Updated Treaty Code");
                        }

                        if (IsUpdateStatus)
                        {
                            UpdateStatus(ref mfrs17ReportingDetail);
                            SetProcessCount("Updated Status");
                        }

                        if (IsUpdateTreatyCode || IsUpdateStatus)
                            db.Entry(mfrs17ReportingDetail).State = EntityState.Modified;
                    }

                    processed++;
                }
                db.SaveChanges();

                if (processed > 0)
                    PrintProcessCount();
            }

            PrintEnding();
        }

        public void UpdateTreatyCode(ref Mfrs17ReportingDetail mfrs17ReportingDetail)
        {
            var treatyCodeBo = TreatyCodeService.Find(mfrs17ReportingDetail.TreatyCodeId);
            if (treatyCodeBo != null)
            {
                mfrs17ReportingDetail.TreatyCode = treatyCodeBo.Code;
                mfrs17ReportingDetail.TreatyCodeId = null;
            }
        }

        public void UpdateStatus(ref Mfrs17ReportingDetail mfrs17ReportingDetail)
        {
            mfrs17ReportingDetail.Status = Mfrs17ReportingDetailBo.StatusProcessed;
        }
    }
}
