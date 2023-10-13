using BusinessObject;
using BusinessObject.TreatyPricing;
using DataAccess.Entities;
using DataAccess.Entities.Identity;
using DataAccess.Entities.TreatyPricing;
using DataAccess.EntityFramework;
using Services.TreatyPricing;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class TreatyPricingGroupMasterLetterViewModel
    {
        public int Id { get; set; }

        [Required, Display(Name = "Ceding Company")]
        public int CedantId { get; set; }
        public CedantBo CedantBo { get; set; }
        public virtual Cedant Cedant { get; set; }

        [Display(Name = "Group Master Letter ID")]
        public string Code { get; set; }

        [Display(Name = "No Of Ri Group Slip")]
        public int NoOfRiGroupSlip { get; set; }

        public TreatyPricingGroupMasterLetterViewModel() { Set(); }

        public TreatyPricingGroupMasterLetterViewModel(TreatyPricingGroupMasterLetterBo groupMasterLetterBo)
        {
            Set(groupMasterLetterBo);
        }

        public void Set(TreatyPricingGroupMasterLetterBo groupMasterLetterBo = null)
        {
            if (groupMasterLetterBo != null)
            {
                Id = groupMasterLetterBo.Id;

                CedantId = groupMasterLetterBo.CedantId;
                CedantBo = groupMasterLetterBo.CedantBo;

                Code = groupMasterLetterBo.Code;
                NoOfRiGroupSlip = groupMasterLetterBo.NoOfRiGroupSlip;
            }
        }

        public TreatyPricingGroupMasterLetterBo FormBo(int createdById, int updatedById)
        {
            return new TreatyPricingGroupMasterLetterBo
            {
                Id = Id,
                Code = Code,
                CedantId = CedantId,
                CedantBo = CedantBo,
                NoOfRiGroupSlip = NoOfRiGroupSlip,
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<TreatyPricingGroupMasterLetter, TreatyPricingGroupMasterLetterViewModel>> Expression()
        {
            return entity => new TreatyPricingGroupMasterLetterViewModel
            {
                Id = entity.Id,
                Code = entity.Code,
                CedantId = entity.CedantId,
                Cedant = entity.Cedant,
                NoOfRiGroupSlip = entity.NoOfRiGroupSlip,
            };
        }

        public List<TreatyPricingGroupMasterLetterGroupReferralBo> GetGroupMasterLetterDetails(FormCollection form)
        {
            int index = 0;
            int maxIndex = int.Parse(form.Get("GroupMasterLetterDetailMaxIndex"));
            List<TreatyPricingGroupMasterLetterGroupReferralBo> groupMasterLetterDetailBos = new List<TreatyPricingGroupMasterLetterGroupReferralBo> { };

            while (form.AllKeys.Contains(string.Format("groupReferralId[{0}]", index)))
            {
                string groupReferralId = form.Get(string.Format("groupReferralId[{0}]", index));
                string id = form.Get(string.Format("detailId[{0}]", index));

                var bo = new TreatyPricingGroupMasterLetterGroupReferralBo()
                {
                    TreatyPricingGroupMasterLetterId = Id,
                    TreatyPricingGroupReferralId = int.Parse(groupReferralId),
                };

                if (!string.IsNullOrEmpty(id)) bo.Id = int.Parse(id);

                groupMasterLetterDetailBos.Add(bo);
                index++;
            }
            return groupMasterLetterDetailBos;
        }

        public void ProcessGroupMasterLetterDetails(List<TreatyPricingGroupMasterLetterGroupReferralBo> groupMasterLetterDetailBos, int authUserId, ref TrailObject trail)
        {
            List<int> savedIds = new List<int> { };
            List<int> referralIds = new List<int> { };

            foreach (TreatyPricingGroupMasterLetterGroupReferralBo bo in groupMasterLetterDetailBos)
            {
                TreatyPricingGroupMasterLetterGroupReferralBo groupMasterLetterDetailBo = bo;
                if (groupMasterLetterDetailBo.Id == 0)
                {
                    groupMasterLetterDetailBo.TreatyPricingGroupMasterLetterId = Id;
                    groupMasterLetterDetailBo.CreatedById = authUserId;
                    TreatyPricingGroupMasterLetterGroupReferralService.Create(ref groupMasterLetterDetailBo, ref trail);
                }

                // Set Group Master Letter ID at Group Referral
                using (var db = new AppDbContext())
                {
                    db.Database.ExecuteSqlCommand("UPDATE [TreatyPricingGroupReferrals] SET [TreatyPricingGroupMasterLetterId] = {0}, [UpdatedById] = {1}, [UpdatedAt] = {2} WHERE [Id] = {3}",
                        groupMasterLetterDetailBo.TreatyPricingGroupMasterLetterId, User.DefaultSuperUserId, DateTime.Now, groupMasterLetterDetailBo.TreatyPricingGroupReferralId);
                    db.SaveChanges();
                }

                savedIds.Add(groupMasterLetterDetailBo.Id);
                referralIds.Add(groupMasterLetterDetailBo.TreatyPricingGroupReferralId);
            }
            TreatyPricingGroupMasterLetterGroupReferralService.DeleteByGroupMasterLetterIdExcept(Id, savedIds, ref trail);
            TreatyPricingGroupReferralService.UpdateGroupMasterLetterIdExcept(Id, referralIds, ref trail);

            
        }
    }
}