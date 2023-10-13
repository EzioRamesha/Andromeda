using BusinessObject;
using DataAccess.EntityFramework;
using Services;
using Services.TreatyPricing;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.UpdateDatas
{
    public class UpdateTreatyPricingPickListDetail : Command
    {
        public UpdateTreatyPricingPickListDetail()
        {
            Title = "UpdateTreatyPricingPickListDetail";
            Description = "To update code to store code - description if available";
            Hide = true;
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                int count = db.TreatyPricingPickListDetails.Count();
                int processed = 0;

                while (processed < count)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }
                    SetProcessCount();

                    var tpPickListDetail = db.TreatyPricingPickListDetails.OrderBy(q => q.Id).Skip(processed).FirstOrDefault();
                    var pickListDetail = db.PickListDetails
                        .Where(q => q.PickListId == tpPickListDetail.PickListId)
                        .Where(q => q.Code == tpPickListDetail.PickListDetailCode || q.Description == tpPickListDetail.PickListDetailCode)
                        .FirstOrDefault();

                    if (pickListDetail != null)
                    {
                        var pickListDetailBo = PickListDetailService.FormBo(pickListDetail);
                        var pickListDetailCode = pickListDetailBo.ToString();

                        if (tpPickListDetail.PickListDetailCode != pickListDetailCode)
                        {
                            tpPickListDetail.PickListDetailCode = pickListDetailCode;
                            db.Entry(tpPickListDetail).State = EntityState.Modified;

                            SetProcessCount("Updated");
                        }
                    }


                    processed++;
                }
                db.SaveChanges();

                PrintProcessCount();
            }

            PrintEnding();
        }
    }
}
