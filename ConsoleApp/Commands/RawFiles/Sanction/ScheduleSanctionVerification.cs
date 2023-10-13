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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.Sanction
{
    public class ScheduleSanctionVerification : Command
    {
        DateTime Today { get; set; }

        IList<SourceBo> SourceBos { get; set; }

        public ScheduleSanctionVerification()
        {
            Title = "ScheduleSanctionVerification";
            Description = "To schedule half yearly auto sanction verifications";
            Today = DateTime.Today;
        }

        public override void Run()
        {
            bool createVerfication = false;
            string dates = Util.GetConfig("AutoSanctionVerificationDates");
            if (!string.IsNullOrEmpty(dates))
            {
                string[] splitDates = dates.Split(',');
                foreach (string partialDate in  dates.Split(','))
                {
                    DateTime? date = Util.GetParseDateTime(string.Format("{0}/{1}", partialDate, Today.Year));
                    if (date.HasValue && date == Today)
                    {
                        createVerfication = true;
                    }
                }
            }

            if (!createVerfication)
            {
                Log = false;
                PrintMessage(MessageBag.NotTimeToCreateAutoVerification);
                return;
            }


            PrintStarting();

            LoadSources();
            foreach (var source in SourceBos)
            {
                int sourceId = source.Id;
                int type = SanctionVerificationBo.TypeAuto;

                if (SanctionVerificationService.IsAutoExists(sourceId, type))
                    continue;

                SanctionVerificationBo verificationBo = new SanctionVerificationBo()
                {
                    SourceId = sourceId,
                    IsRiData = true,
                    IsClaimRegister = true,
                    IsReferralClaim = true,
                    Type = type,
                    Status = SanctionVerificationBo.StatusPending,
                    CreatedById = User.DefaultSuperUserId,
                    UpdatedById = User.DefaultSuperUserId,
                };

                CreateVerification(verificationBo);

                PrintMessage(string.Format("Created Verification for Source: {0}", source.Name));
            }

            PrintEnding();
        }

        public void LoadSources()
        {
            SourceBos = SourceService.GetForAutoSchedule();
        }

        public void CreateVerification(SanctionVerificationBo verificationBo)
        {
            var trail = new TrailObject();

            Result result = SanctionVerificationService.Create(ref verificationBo, ref trail);
            if (result.Valid)
            {
                var userTrailBo = new UserTrailBo(
                    verificationBo.Id,
                    "Create Sanction Verification",
                    result,
                    trail,
                    User.DefaultSuperUserId
                );
                UserTrailService.Create(ref userTrailBo);
            }

        }
    }
}
