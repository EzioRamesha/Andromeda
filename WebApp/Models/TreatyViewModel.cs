using BusinessObject;
using DataAccess.Entities;
using Services;
using Services.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class TreatyViewModel
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(30)]
        [DisplayName("Treaty ID")]
        public string TreatyIdCode { get; set; }

        [ValidateCedantId]
        [DisplayName("Ceding Company")]
        public int CedantId { get; set; }

        [DisplayName("Ceding Company")]
        public virtual Cedant Cedant { get; set; }

        [DisplayName("Ceding Company")]
        public CedantBo CedantBo { get; set; }

        [StringLength(128)]
        public string Description { get; set; }

        [StringLength(128)]
        public string Remarks { get; set; }

        [Required, Display(Name = "Business Origin")]
        public int? BusinessOriginPickListDetailId { get; set; }

        public virtual PickListDetail BusinessOriginPickListDetail { get; set; }

        public PickListDetailBo BusinessOriginPickListDetailBo { get; set; }

        [StringLength(128), DisplayName("Block Description")]
        public string BlockDescription { get; set; }

        public TreatyViewModel() { }

        public TreatyViewModel(TreatyBo treatieBo)
        {
            if (treatieBo != null)
            {
                Id = treatieBo.Id;
                TreatyIdCode = treatieBo.TreatyIdCode;
                CedantId = treatieBo.CedantId;
                CedantBo = treatieBo.CedantBo;
                Description = treatieBo.Description;
                Remarks = treatieBo.Remarks;
                BusinessOriginPickListDetailId = treatieBo.BusinessOriginPickListDetailId;
                BusinessOriginPickListDetailBo = treatieBo.BusinessOriginPickListDetailBo;
                BlockDescription = treatieBo.BlockDescription;
            }
        }

        public TreatyBo FormBo(int createdById, int updatedById)
        {
            return new TreatyBo
            {
                Id = Id,
                TreatyIdCode = TreatyIdCode?.Trim(),
                CedantId = CedantId,
                CedantBo = CedantService.Find(CedantId),
                Description = Description?.Trim(),
                Remarks = Remarks?.Trim(),
                BusinessOriginPickListDetailId = BusinessOriginPickListDetailId,
                BusinessOriginPickListDetailBo = PickListDetailService.Find(BusinessOriginPickListDetailId),
                BlockDescription = BlockDescription?.Trim(),

                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<Treaty, TreatyViewModel>> Expression()
        {
            return entity => new TreatyViewModel
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                Cedant = entity.Cedant,
                TreatyIdCode = entity.TreatyIdCode,
                Description = entity.Description,
            };
        }

        public List<TreatyCodeBo> GetTreatyCodes(FormCollection form, ref Result result)
        {
            int index = 0;
            List<TreatyCodeBo> treatyCodeBos = new List<TreatyCodeBo> { };
            int? cedantId = Util.GetParseInt(form.Get("CedantId"));

            while (form.AllKeys.Contains(string.Format("code[{0}]", index)))
            {
                string code = form.Get(string.Format("code[{0}]", index))?.Trim();
                string description = form.Get(string.Format("description[{0}]", index))?.Trim();
                string id = form.Get(string.Format("codeId[{0}]", index));
                string status = form.Get(string.Format("status[{0}]", index));
                string oldTreatyCodeId = form.Get(string.Format("oldCodeId[{0}]", index));
                string treatyStatus = form.Get(string.Format("treatyStatus[{0}]", index));
                string treatyType = form.Get(string.Format("treatyType[{0}]", index));
                string accounts = form.Get(string.Format("account[{0}]", index))?.Trim();
                string treatyNo = form.Get(string.Format("treatyNo[{0}]", index))?.Trim();
                string lineOfBusiness = form.Get(string.Format("lineOfBusiness[{0}]", index));

                TreatyCodeBo treatyCodeBo = new TreatyCodeBo
                {
                    TreatyId = Id,
                    Code = code,
                    Description = description,
                    AccountFor = accounts,
                    TreatyNo = treatyNo,
                };

                if (!string.IsNullOrEmpty(status)) treatyCodeBo.Status = int.Parse(status);
                if (!string.IsNullOrEmpty(treatyStatus)) treatyCodeBo.TreatyStatusPickListDetailId = int.Parse(treatyStatus);
                if (!string.IsNullOrEmpty(treatyType)) treatyCodeBo.TreatyTypePickListDetailId = int.Parse(treatyType);
                if (!string.IsNullOrEmpty(lineOfBusiness)) treatyCodeBo.LineOfBusinessPickListDetailId = int.Parse(lineOfBusiness);

                if (!string.IsNullOrEmpty(id)) treatyCodeBo.Id = int.Parse(id);

                List<string> errors = treatyCodeBo.Validate();
                if (!string.IsNullOrEmpty(code))
                {
                    //if (TreatyCodeService.IsDuplicateCode(treatyCodeBo) == true)
                    Regex rgx = new Regex(@"^([A-Za-z]+)[-]");
                    if (!rgx.IsMatch(code))
                    {
                        errors.Add(string.Format("Incorrect Treaty Code: {0}. Acceptable Format: \"Prefix-value\", Prefix should be in Alphabet", treatyCodeBo.Code));
                    }
                    else if (TreatyCodeService.IsDuplicatePrefix(treatyCodeBo, cedantId) == true)
                    {
                        errors.Add(string.Format("The prefix of Treaty Code: {0} already used by another Cedant", treatyCodeBo.Code));
                    }
                    else if (treatyCodeBo.Id != 0 && TreatyCodeService.IsPrefixModified(treatyCodeBo))
                    {
                        errors.Add(string.Format("Unable update to Treaty Code: {0}. Partition already created", treatyCodeBo.Code));
                    }
                    else if (TreatyCodeService.IsDuplicateCode(treatyCodeBo) == true)
                    {
                        errors.Add(string.Format(MessageBag.AlreadyTaken, "Treaty Code", treatyCodeBo.Code));
                    }
                }
                foreach (string error in errors)
                {
                    result.AddError(error);
                }

                treatyCodeBos.Add(treatyCodeBo);
                index++;
            }
            return treatyCodeBos;
        }

        public void ProcessTreatyCodes(FormCollection form, int authUserId, ref TrailObject trail)
        {
            int index = 0;
            List<int> savedIds = new List<int> { };

            while (!string.IsNullOrWhiteSpace(form.Get(string.Format("code[{0}]", index))))
            {
                string code = form.Get(string.Format("code[{0}]", index))?.Trim();
                string description = form.Get(string.Format("description[{0}]", index))?.Trim();
                string id = form.Get(string.Format("codeId[{0}]", index));
                string status = form.Get(string.Format("status[{0}]", index));
                string oldTreatyCodeId = form.Get(string.Format("oldCodeId[{0 }]", index));
                string treatyStatus = form.Get(string.Format("treatyStatus[{0}]", index));
                string treatyType = form.Get(string.Format("treatyType[{0}]", index));
                string accounts = form.Get(string.Format("account[{0}]", index))?.Trim();
                string treatyNo = form.Get(string.Format("treatyNo[{0}]", index))?.Trim();
                string lineOfBusiness = form.Get(string.Format("lineOfBusiness[{0}]", index));

                TreatyCodeBo treatyCodeBo = new TreatyCodeBo
                {              
                    TreatyId = Id,
                    Code = code,
                    Description = description,
                    Status = int.Parse(status),
                    AccountFor = accounts,
                    TreatyNo = treatyNo,
                    CreatedById = authUserId,
                    UpdatedById = authUserId,
                };

                if (!string.IsNullOrEmpty(treatyStatus)) treatyCodeBo.TreatyStatusPickListDetailId = int.Parse(treatyStatus);
                if (!string.IsNullOrEmpty(treatyType)) treatyCodeBo.TreatyTypePickListDetailId = int.Parse(treatyType);
                if (!string.IsNullOrEmpty(lineOfBusiness)) treatyCodeBo.LineOfBusinessPickListDetailId = int.Parse(lineOfBusiness);

                if (!string.IsNullOrEmpty(id)) treatyCodeBo.Id = int.Parse(id);

                TreatyCodeService.Save(treatyCodeBo, ref trail);

                if (!string.IsNullOrEmpty(oldTreatyCodeId))
                {
                    List<int> oldTreatyCodeIds = oldTreatyCodeId.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x)).ToList();
                    foreach (int oldId in oldTreatyCodeIds)
                    {
                        TreatyOldCodeBo treatyOldCodeBo = new TreatyOldCodeBo
                        {
                            OldTreatyCodeId = oldId,
                            TreatyCodeId = treatyCodeBo.Id,
                        };
                        TreatyOldCodeService.Save(treatyOldCodeBo, ref trail);
                    }
                    TreatyOldCodeService.DeleteByTreatyCodeIdExcept(treatyCodeBo.Id, oldTreatyCodeIds, ref trail);
                }

                savedIds.Add(treatyCodeBo.Id);
                index++;
            }
            TreatyOldCodeService.DeleteAllByTreatyCodeIdExcept(savedIds, ref trail);
            TreatyCodeService.DeleteByTreatyCodeIdExcept(Id, savedIds, ref trail);
        }
    }
}