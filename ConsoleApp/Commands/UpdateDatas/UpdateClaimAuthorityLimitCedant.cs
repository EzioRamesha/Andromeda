using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.UpdateDatas
{
    public class UpdateClaimAuthorityLimitCedant : Command
    {
        public UpdateClaimAuthorityLimitCedant()
        {
            Title = "UpdateClaimAuthorityLimitCedant";
            Description = "To update claim authority limit - cedant";
            Hide = true;
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                int groupId = (PickListDetail.FindByStandardOutputIdCode(StandardOutputBo.TypeFundsAccountingTypeCode, PickListDetailBo.FundsAccountingTypeCodeGroup)).Id;
                int individualId = (PickListDetail.FindByStandardOutputIdCode(StandardOutputBo.TypeFundsAccountingTypeCode, PickListDetailBo.FundsAccountingTypeCodeIndividual)).Id;

                int total = db.ClaimAuthorityLimitCedantDetails.Count();
                int processed = 0;
                int take = 50;

                while (processed < total)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }

                    var details = db.ClaimAuthorityLimitCedantDetails.OrderBy(q => q.Id).Skip(processed).Take(take).ToList();
                    foreach (var detail in details)
                    {
                        bool isUpdated = false;
                        if (detail.FundsAccountingTypePickListDetailId == ClaimAuthorityLimitCedantDetailBo.FundAccountingCodeGroup)
                        {
                            detail.FundsAccountingTypePickListDetailId = groupId;
                            isUpdated = true;
                        }
                        else if (detail.FundsAccountingTypePickListDetailId == ClaimAuthorityLimitCedantDetailBo.FundAccountingCodeIndividual)
                        {
                            detail.FundsAccountingTypePickListDetailId = individualId;
                            isUpdated = true;
                        }

                        if (isUpdated)
                        {
                            db.Entry(detail).State = EntityState.Modified;
                            SetProcessCount("Updated");
                        }

                        SetProcessCount();
                    }

                    processed += take;
                }
                db.SaveChanges();

                if (processed > 0)
                    PrintProcessCount();
            }

            PrintEnding();
        }
    }
}
