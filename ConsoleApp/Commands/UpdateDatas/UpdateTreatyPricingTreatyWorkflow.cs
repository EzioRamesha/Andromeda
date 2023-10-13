using BusinessObject.TreatyPricing;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Services.TreatyPricing;
using System.Data.Entity;
using System.Linq;

namespace ConsoleApp.Commands.UpdateDatas
{
    class UpdateTreatyPricingTreatyWorkflow : Command
    {
        public bool IsUpdateOrionGroup { get; set; }

        public UpdateTreatyPricingTreatyWorkflow()
        {
            Title = "UpdateTreatyPricingTreatyWorkflow";
            Description = "To update Treaty Pricing Treaty Workflow";
            Options = new string[] {
                "--t|updateOrionGroup : Update ORION Group",
            };
            Hide = true;
        }

        public override void Initial()
        {
            base.Initial();

            IsUpdateOrionGroup = IsOption("updateOrionGroup");
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                // the calculation for TAT refreshed each day that involves "Today's date"
                var treatyWorkflows = db.TreatyPricingTreatyWorkflows.ToList();
                int total = treatyWorkflows.Count();
                int processed = 0;

                while (processed < total)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }
                    SetProcessCount();

                    var treatyWorkflow = treatyWorkflows.OrderBy(q => q.Id).Skip(processed).FirstOrDefault();
                    if (treatyWorkflow != null)
                    {
                        if (IsUpdateOrionGroup)
                        {
                            UpdateOrionGroup(ref treatyWorkflow);
                            SetProcessCount("Updated ORION Group");
                        }

                        if (IsUpdateOrionGroup)
                            db.Entry(treatyWorkflow).State = EntityState.Modified;
                    }

                    processed++;
                }
                db.SaveChanges();

                if (processed > 0)
                    PrintProcessCount();
            }

            PrintProcessCount();

            PrintEnding();
        }

        public void UpdateOrionGroup(ref TreatyPricingTreatyWorkflow treatyWorkflow)
        {
            var bo = TreatyPricingTreatyWorkflowService.FormBo(treatyWorkflow);
            if (bo.DocumentStatus != TreatyPricingTreatyWorkflowBo.DocumentStatusSigned)
                treatyWorkflow.OrionGroupStr = TreatyPricingTreatyWorkflowService.GenerateOrionGroupStr(bo.EffectiveAt);
            else
                treatyWorkflow.OrionGroupStr = "";
        }
    }
}
