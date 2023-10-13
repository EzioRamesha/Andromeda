using BusinessObject;
using Services;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.ReferralClaim
{
    public class ProcessReferralClaimAssessmentBatch : Command
    {
        public IList<ReferralClaimBo> ReferralClaimBos { get; set; }

        public IList<ProcessReferralClaimAssessment> ProcessReferralClaimAssessments { get; set; }

        public bool Test { get; set; } = false;

        public int Take { get; set; } = 100;

        public ProcessReferralClaimAssessmentBatch()
        {
            Title = "ProcessReferralClaimAssessmentBatch";
            Description = "To process Referral Claim Assessment";
            Options = new string[] {
                "--l|logIndex= : Log File Index",
                "--t|test : Test process data",
            };
        }

        public override void Initial()
        {
            base.Initial();

            Test = IsOption("test");
            Take = Util.GetConfigInteger("ProcessReferralClaimAssessmentItems", 100);
        }

        public override void Run()
        {
            if (ReferralClaimService.CountByStatus(ReferralClaimBo.StatusPendingAssessment) == 0)
            {
                Log = false;
                PrintMessage(MessageBag.NoReferralClaimPendingAssessment);
                return;
            }
            PrintStarting();

            while (GetNextBulkReferralClaim())
            {
                Parallel.ForEach(ProcessReferralClaimAssessments, f => f.Process());

                foreach (var r in ProcessReferralClaimAssessments)
                {
                    if (r.Success)
                    {
                        SetProcessCount("Success");
                    }
                    else
                    {
                        SetProcessCount("Failed");
                    }
                    SetProcessCount();
                }

                PrintProcessCount();

                if (Test)
                    break;
            }

            PrintEnding();
        }

        public bool GetNextBulkReferralClaim()
        {
            ReferralClaimBos = new List<ReferralClaimBo> { };
            ProcessReferralClaimAssessments = new List<ProcessReferralClaimAssessment> { };
            int total = ReferralClaimService.CountByStatus(ReferralClaimBo.StatusPendingAssessment);
            if (total == 0)
                return false;

            ReferralClaimBos = ReferralClaimService.GetByStatus(ReferralClaimBo.StatusPendingAssessment, 0, Take);
            foreach (var referralClaimBo in ReferralClaimBos)
            {
                ProcessReferralClaimAssessments.Add(new ProcessReferralClaimAssessment(this, referralClaimBo));
            }

            return true;
        }
    }
}
