using BusinessObject;
using DataAccess.Entities;
using Services;
using Shared;
using Shared.DataAccess;
using Shared.Forms.Attributes;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class Mfrs17ContractCodeViewModel
    {
        public int Id { get; set; }

        [DisplayName("Ceding Company")]
        [Required]
        public int CedingCompanyId { get; set; }
        public CedantBo CedingCompanyBo { get; set; }

        [DisplayName("Modified Contract Code")]
        public string ModifiedContractCode { get; set; }

        [DisplayName("MFRS17 Contract Code")]
        public string Mfrs17ContractCode { get; set; }

        [DisplayName("Date Created")]
        public DateTime? CreatedAt { get; set; }

        [DisplayName("Date Created")]
        public string CreatedAtStr { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public Mfrs17ContractCodeViewModel() { }

        public Mfrs17ContractCodeViewModel(Mfrs17ContractCodeBo mfrs17ContractCodeBo)
        {
            Set(mfrs17ContractCodeBo);
        }

        public void Set(Mfrs17ContractCodeBo mfrs17ContractCodeBo)
        {
            if (mfrs17ContractCodeBo != null)
            {
                Id = mfrs17ContractCodeBo.Id;
                CedingCompanyId = mfrs17ContractCodeBo.CedingCompanyId;
                ModifiedContractCode = mfrs17ContractCodeBo.ModifiedContractCode;
            }
        }

        public static Expression<Func<Mfrs17ContractCode, Mfrs17ContractCodeViewModel>> Expression()
        {
            return entity => new Mfrs17ContractCodeViewModel
            {
                Id = entity.Id,
                CedingCompanyId = entity.CedingCompanyId,
                ModifiedContractCode = entity.ModifiedContractCode,
                CreatedById = entity.CreatedById,
                UpdatedById = entity.UpdatedById,
            };
        }

        public static Expression<Func<Mfrs17ContractCodeWithDetail, Mfrs17ContractCodeViewModel>> ExpressionWithDetail()
        {
            return entity => new Mfrs17ContractCodeViewModel
            {
                Id = entity.ContractCode.Id,
                CedingCompanyId = entity.ContractCode.CedingCompanyId,
                ModifiedContractCode = entity.ContractCode.ModifiedContractCode,
                Mfrs17ContractCode = entity.ContractCodeDetail.ContractCode,
                CreatedById = entity.ContractCode.CreatedById,
                UpdatedById = entity.ContractCode.UpdatedById,
            };
        }

        public List<Mfrs17ContractCodeDetailBo> GetMfrs17ContractCodeDetails(FormCollection form, ref Result result)
        {
            int index = 0;
            int maxIndex = int.Parse(form.Get("mfrs17ContractCodeDetailMaxIndex"));
            List<Mfrs17ContractCodeDetailBo> mfrs17ContractCodeDetailBos = new List<Mfrs17ContractCodeDetailBo>();
            List<string> contractCodes = new List<string>();

            if (maxIndex < 0)
            {
                result.AddError(string.Format("At least 1 Contract Code has to be added"));
            }

            while (index <= maxIndex)
            {
                string contractCode = form.Get(string.Format("code[{0}]", index))?.Trim();
                string id = form.Get(string.Format("id[{0}]", index));

                int mfrs17ContractCodeDetailId = 0;
                if (!string.IsNullOrEmpty(id))
                    mfrs17ContractCodeDetailId = int.Parse(id);

                if (string.IsNullOrEmpty(contractCode) && mfrs17ContractCodeDetailId == 0)
                {
                    index++;
                    continue;
                }

                if (string.IsNullOrEmpty(contractCode) || string.IsNullOrWhiteSpace(contractCode))
                {
                    result.AddError(string.Format("Contract Code cannot be empty or white space at row #{0}", index + 1));
                }
                else
                {
                    if (contractCodes.Contains(contractCode.ToLower()))
                    {
                        result.AddError(string.Format("Duplicate code at row #{0}", index + 1));
                    }
                    contractCodes.Add(contractCode.ToLower());
                }

                Mfrs17ContractCodeDetailBo mfrs17ContractCodeDetailBo = new Mfrs17ContractCodeDetailBo
                {
                    Mfrs17ContractCodeId = Id,
                    ContractCode = contractCode,
                };

                if (mfrs17ContractCodeDetailId != 0)
                {
                    mfrs17ContractCodeDetailBo.Id = mfrs17ContractCodeDetailId;
                }

                mfrs17ContractCodeDetailBos.Add(mfrs17ContractCodeDetailBo);
                index++;
            }
            return mfrs17ContractCodeDetailBos;
        }

        public void ProcessMfrs17ContractCodeDetails(List<Mfrs17ContractCodeDetailBo> Mfrs17ContractCodeDetailBos, int authUserId, ref TrailObject trail)
        {
            List<int> savedIds = new List<int> { };

            foreach (Mfrs17ContractCodeDetailBo bo in Mfrs17ContractCodeDetailBos)
            {
                Mfrs17ContractCodeDetailBo Mfrs17ContractCodeDetailBo = bo;
                Mfrs17ContractCodeDetailBo.Mfrs17ContractCodeId = Id;
                Mfrs17ContractCodeDetailBo.CreatedById = authUserId;
                Mfrs17ContractCodeDetailBo.UpdatedById = authUserId;

                Mfrs17ContractCodeDetailService.Save(Mfrs17ContractCodeDetailBo, ref trail);
                savedIds.Add(Mfrs17ContractCodeDetailBo.Id);
            }
            Mfrs17ContractCodeDetailService.DeleteByMfrs17ContractCodeDetailIdExcept(Id, savedIds, ref trail);
        }
    }
}
