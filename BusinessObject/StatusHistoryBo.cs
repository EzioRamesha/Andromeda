using System;
using System.Reflection;

namespace BusinessObject
{
    public class StatusHistoryBo
    {
        public int Id { get; set; }

        public int ModuleId { get; set; }

        public ModuleBo ModuleBo { get; set; }

        public int ObjectId { get; set; }

        public string SubModuleController { get; set; }

        public int? SubObjectId { get; set; }

        public int? Version { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedAtStr { get; set; }

        public string UpdatedAtStr { get; set; }

        public int CreatedById { get; set; }

        public string CreatedByName { get; set; }

        public int? UpdatedById { get; set; }

        public string UpdatedByName { get; set; }

        // Remark
        public RemarkBo RemarkBo { get; set; }

        // Recipient
        public string RecipientNames { get; set; }
        public string Emails { get; set; }

        // Quotation Workflow Checklist
        public string Department { get; set; }
        public string PersonInCharge { get; set; }

        public static string GetStatusName(ModuleBo moduleBo, int status)
        {
            string boFormat = "BusinessObject.{0}{1}Bo";
            Type bo;
            if (moduleBo.Controller == ModuleBo.ModuleController.RiData.ToString())
            {
                bo  = Type.GetType(string.Format(boFormat, "RiDatas.", "RiDataBatch"));
            }
            else if (moduleBo.Controller == ModuleBo.ModuleController.ClaimData.ToString())
            {
                bo = Type.GetType(string.Format(boFormat, "Claims.", "ClaimDataBatch"));
            }
            else if (moduleBo.Controller == ModuleBo.ModuleController.SoaData.ToString())
            {
                bo = Type.GetType(string.Format(boFormat, "SoaDatas.", "SoaDataBatch"));
            }
            else if (moduleBo.Controller == ModuleBo.ModuleController.InvoiceRegister.ToString())
            {
                bo = Type.GetType(string.Format(boFormat, "InvoiceRegisters.", "InvoiceRegisterBatch"));
            }
            else
            {
                string subDir = "";
                if (moduleBo.Controller.Contains("RiData"))
                {
                    subDir = "RiDatas.";
                }
                else if (moduleBo.Controller.Contains("ClaimData"))
                {
                    subDir = "Claims.";
                }
                else if (moduleBo.Controller.Contains("SoaData"))
                {
                    subDir = "SoaDatas.";
                }
                else if (moduleBo.Controller.Contains("InvoiceRegister"))
                {
                    subDir = "InvoiceRegisters.";
                }
                else if (moduleBo.Controller.Contains("TreatyPricing"))
                {
                    subDir = "TreatyPricing.";
                }
                else if (moduleBo.Controller.Contains("PerLife"))
                {
                    subDir = "Retrocession.";
                }

                bo = Type.GetType(string.Format(boFormat, subDir, moduleBo.Controller));
                if (bo == null)
                {
                    subDir = "Identity.";
                    bo = Type.GetType(string.Format(boFormat, subDir, moduleBo.Controller));
                    if (bo == null)
                    {
                        return null;
                    }
                }
            }

            string methodName = "GetStatusName";
            if (moduleBo.Controller == ModuleBo.ModuleController.DirectRetro.ToString())
            {
                methodName = "GetRetroStatusName";
            }
            if (moduleBo.Controller == ModuleBo.ModuleController.TreatyPricingTreatyWorkflow.ToString())
            {
                methodName = "GetDraftingStatusName";
            }

            MethodInfo getStatusName = bo.GetMethod(methodName, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static);
            if (getStatusName == null)
                return null;

            string statusName =  (string)getStatusName.Invoke(bo, new object[] { status });
            return statusName;
        }
    }
}
