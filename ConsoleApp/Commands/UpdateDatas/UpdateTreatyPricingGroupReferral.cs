using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Services.TreatyPricing;
using System.Data.Entity;
using System.Linq;

namespace ConsoleApp.Commands.UpdateDatas
{
    class UpdateTreatyPricingGroupReferral : Command
    {
        public bool IsUpdateTat { get; set; }

        public UpdateTreatyPricingGroupReferral()
        {
            Title = "UpdateTreatyPricingGroupReferral";
            Description = "To update Treaty Pricing Group Referral";
            Options = new string[] {
                "--t|updateTat : Update TAT",
            };
            Hide = true;
        }

        public override void Initial()
        {
            base.Initial();

            IsUpdateTat = IsOption("updateTat");
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                // the calculation for TAT refreshed each day that involves "Today's date"
                var groupReferralVersions = db.TreatyPricingGroupReferralVersions.ToList();
                int total = groupReferralVersions.Count();
                int processed = 0;

                while (processed < total)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }
                    SetProcessCount();

                    var groupReferralVersion = groupReferralVersions.OrderBy(q => q.Id).Skip(processed).FirstOrDefault();
                    if (groupReferralVersion != null)
                    {
                        if (IsUpdateTat)
                        {
                            UpdateTat(ref groupReferralVersion);
                            SetProcessCount("Updated TAT");
                        }

                        if (IsUpdateTat)
                            db.Entry(groupReferralVersion).State = EntityState.Modified;
                    }

                    processed++;
                }
                db.SaveChanges();

                if (IsUpdateTat)
                {
                    var groupReferrals = db.TreatyPricingGroupReferrals.OrderBy(q => q.Id).ToList();
                    foreach (var groupReferral in groupReferrals)
                    {
                        var entity = groupReferral;
                        UpdateAverageScore(ref entity);
                        db.Entry(groupReferral).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }                    

                if (processed > 0)
                    PrintProcessCount();
            }

            PrintProcessCount();

            PrintEnding();
        }

        public void UpdateTat(ref TreatyPricingGroupReferralVersion groupReferralVersion)
        {
            var verBo = TreatyPricingGroupReferralVersionService.FormBo(groupReferralVersion);
            groupReferralVersion.QuotationTAT = TreatyPricingGroupReferralVersionService.GenerateQuotationTat(verBo);
            // For "Current Internal TAT", if there is no "Client Reply Date" and "Enquiry to Client Date", follow "Current Quotation TAT"
            if (!string.IsNullOrEmpty(verBo.ClientReplyDateStr) && !string.IsNullOrEmpty(verBo.EnquiryToClientDateStr)) 
                groupReferralVersion.InternalTAT = TreatyPricingGroupReferralVersionService.GenerateInternalTat(verBo);
            else
                groupReferralVersion.InternalTAT = groupReferralVersion.QuotationTAT;
            groupReferralVersion.QuotationValidityDate = TreatyPricingGroupReferralVersionService.GenerateQuotationValidityDate(verBo);
            groupReferralVersion.FirstQuotationSentWeek = TreatyPricingGroupReferralVersionService.GenerateFirstQuotationSentWeek(verBo);
            groupReferralVersion.FirstQuotationSentMonth = TreatyPricingGroupReferralVersionService.GenerateFirstQuotationSentMonth(verBo);
            groupReferralVersion.FirstQuotationSentQuarter = TreatyPricingGroupReferralVersionService.GenerateFirstQuotationSentQuarter(verBo);
            groupReferralVersion.FirstQuotationSentYear = TreatyPricingGroupReferralVersionService.GenerateFirstQuotationSentYear(verBo);
            groupReferralVersion.Score = TreatyPricingGroupReferralVersionService.GenerateScore(groupReferralVersion.InternalTAT);
        }

        public void UpdateAverageScore(ref TreatyPricingGroupReferral groupReferral)
        {
            groupReferral.AverageScore = TreatyPricingGroupReferralVersionService.CalculateAverageScore(groupReferral.Id);
        }
    }
}
