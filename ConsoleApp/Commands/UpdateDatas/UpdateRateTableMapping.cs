using BusinessObject;
using DataAccess.Entities;
using DataAccess.EntityFramework;
using Services;
using Shared;
using System.Data.Entity;
using System.Linq;

namespace ConsoleApp.Commands.UpdateDatas
{
    public class UpdateRateTableMapping : Command
    {
        public bool IsUpdateAmount { get; set; }

        public bool IsUpdateCedantId { get; set; }

        public bool IsUpdateDiscountCode { get; set; }

        public UpdateRateTableMapping()
        {
            Title = "UpdateRateTableMapping";
            Description = "To update amount value from integer to double";
            Options = new string[] {
                "--a|updateAmount : Update Amount",
                "--c|updateCedantId : Update Cedant ID",
                "--d|updateDiscountCode : Update Discount Code",
            };
            Hide = true;
        }

        public override void Initial()
        {
            base.Initial();

            IsUpdateAmount = IsOption("updateAmount");
            IsUpdateCedantId = IsOption("updateCedantId");
            IsUpdateDiscountCode = IsOption("updateDiscountCode");
        }

        public override void Run()
        {
            PrintStarting();

            using (var db = new AppDbContext())
            {
                int total = db.RateTables.Count();
                int processed = 0;

                while (processed < total)
                {
                    if (PrintCommitBuffer())
                    {
                        db.SaveChanges();
                    }
                    SetProcessCount();

                    var rateTable = db.RateTables.OrderBy(q => q.Id).Skip(processed).FirstOrDefault();
                    if (rateTable != null)
                    {
                        if (IsUpdateAmount)
                        {
                            UpdateAmount(ref rateTable);
                            SetProcessCount("Updated Amount");
                        }

                        if (IsUpdateCedantId)
                        {
                            UpdateCedantId(ref rateTable);
                            SetProcessCount("Updated Cedant Id");
                        }

                        if (IsUpdateDiscountCode)
                        {
                            UpdateDiscountCode(ref rateTable);
                            SetProcessCount("Updated Discount Code");
                        }

                        if (IsUpdateAmount || IsUpdateCedantId || IsUpdateDiscountCode)
                            db.Entry(rateTable).State = EntityState.Modified;
                    }

                    processed++;
                }
                db.SaveChanges();

                if (processed > 0)
                    PrintProcessCount();
            }

            PrintEnding();
        }

        public void UpdateAmount(ref RateTable rateTable)
        {
            rateTable.PolicyAmountFrom = rateTable.PolicyAmountFrom.HasValue ? rateTable.PolicyAmountFrom / 100 : null;
            rateTable.PolicyAmountTo = rateTable.PolicyAmountTo.HasValue ? rateTable.PolicyAmountTo / 100 : null;
            rateTable.AarFrom = rateTable.AarFrom.HasValue ? rateTable.AarFrom / 100 : null;
            rateTable.AarTo = rateTable.AarTo.HasValue ? rateTable.AarTo / 100 : null;
        }

        public void UpdateCedantId(ref RateTable rateTable)
        {
            if (!string.IsNullOrEmpty(rateTable.TreatyCode))
            {
                string[] treatyCodes = rateTable.TreatyCode.ToArraySplitTrim();
                TreatyCodeBo treatyCodeBo = TreatyCodeService.FindByCode(treatyCodes[0]);
                if (treatyCodeBo != null)
                {
                    rateTable.CedantId = treatyCodeBo?.TreatyBo?.CedantId;
                }
            }
        }

        public void UpdateDiscountCode(ref RateTable rateTable)
        {
            if (rateTable.RiDiscountId.HasValue)
            {
                var bo = RiDiscountService.Find(rateTable.RiDiscountId);
                if (bo != null)
                {
                    rateTable.RiDiscountCode = bo.DiscountCode;
                }
            }
            if (rateTable.LargeDiscountId.HasValue)
            {
                var bo = LargeDiscountService.Find(rateTable.LargeDiscountId);
                if (bo != null)
                {
                    rateTable.LargeDiscountCode = bo.DiscountCode;
                }
            }
            if (rateTable.GroupDiscountId.HasValue)
            {
                var bo = GroupDiscountService.Find(rateTable.GroupDiscountId);
                if (bo != null)
                {
                    rateTable.GroupDiscountCode = bo.DiscountCode;
                }
            }
            rateTable.RiDiscountId = null;
            rateTable.LargeDiscountId = null;
            rateTable.GroupDiscountId = null;
        }
    }
}
