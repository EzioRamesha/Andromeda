using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities.Identity;
using Services.RiDatas;
using Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.RiData
{
    public class ProcessWarehouseRiData
    {
        public ProcessWarehouseRiDataBatch ProcessWarehouseRiDataBatch { get; set; }
        public RiDataBo RiDataBo { get; set; }
        public RiDataWarehouseBo RiDataWarehouseBo { get; set; }
        public RiDataWarehouseBo OriRiDataWarehouseBo { get; set; }
        public bool Success { get; set; }

        public ProcessWarehouseRiData(ProcessWarehouseRiDataBatch processWarehouseRiDataBatch, RiDataBo riDataBo)
        {
            ProcessWarehouseRiDataBatch = processWarehouseRiDataBatch;
            RiDataBo = riDataBo;
        }

        public void ProcessToWarehouse()
        {
            List<string> errors = new List<string>();
            if (!SearchInWarehouse())
            {
                if (RiDataBo.RecordType == RiDataBo.RecordTypeAdjustment)
                {
                    errors.Add(string.Format("Record not found in RI Data Warehouse for RI Data with Record Type: {0}", RiDataBo.GetRecordTypeName(RiDataBo.RecordType)));
                }
                else if (RiDataBo.TransactionTypeCode == PickListDetailBo.TransactionTypeCodeAlteration)
                {
                    errors.Add(string.Format("Record not found in RI Data Warehouse for RI Data with Transaction Type Code: {0}", RiDataBo.TransactionTypeCode));
                }
                else
                {
                    RiDataWarehouseBo = new RiDataWarehouseBo()
                    {
                        CreatedById = User.DefaultSuperUserId,
                        UpdatedById = User.DefaultSuperUserId
                    };
                }
            }

            if (RiDataWarehouseBo != null)
            {
                OriRiDataWarehouseBo = RiDataWarehouseBo;
                CopyData();

                if (RiDataBo.TransactionTypeCode == PickListDetailBo.TransactionTypeCodeAlteration)
                {
                    RiDataWarehouseBo.NetPremium = OriRiDataWarehouseBo.NetPremium + RiDataBo.NetPremium;
                }

                if (RiDataWarehouseBo.AdjEndDate != null)
                {
                    RiDataWarehouseBo.RiskPeriodEndDate = RiDataWarehouseBo.AdjEndDate;
                }

                if (RiDataWarehouseBo.AdjBeginDate != null)
                {
                    RiDataWarehouseBo.RiskPeriodStartDate = RiDataWarehouseBo.AdjBeginDate;
                }

                if (RiDataWarehouseBo.ReportPeriodMonth != null && RiDataWarehouseBo.ReportPeriodYear != null)
                {
                    RiDataWarehouseBo.Quarter = Util.MonthYearToQuarter(RiDataBo.ReportPeriodYear.Value, RiDataBo.ReportPeriodMonth.Value);
                }

                SetEndingPolicyStatus();
                RiDataWarehouseBo.LastUpdatedDate = DateTime.Today;
                SaveWarehouse();
            }

            if (errors.IsNullOrEmpty())
            {
                RiDataBo.ProcessWarehouseStatus = RiDataBo.ProcessWarehouseStatusSuccess;
                Success = true;
            }
            else
            {
                RiDataBo.ProcessWarehouseStatus = RiDataBo.ProcessWarehouseStatusFailed;
                RiDataBo.SetError("ProcessWarehouseError", errors);
                Success = false;
            }

            SaveRiData();
        }

        public void CopyData()
        {
            if (RiDataWarehouseBo == null || RiDataBo == null)
                return;

            foreach (string propertyName in ProcessWarehouseRiDataBatch.PropertyNames)
            {
                object value = RiDataBo.GetPropertyValue(propertyName);
                RiDataWarehouseBo.SetPropertyValue(propertyName, value);
            }
        }

        public void SetEndingPolicyStatus()
        {
            int? status = null;
            if (RiDataWarehouseBo.PolicyStatusCode == PickListDetailBo.PolicyStatusCodeTerminated)
            {
                if (RiDataBo.RecordType == RiDataBo.RecordTypeAdjustment && RiDataWarehouseBo.Aar == OriRiDataWarehouseBo.Aar)
                {
                    status = ProcessWarehouseRiDataBatch.PolicyStatusCodes[PickListDetailBo.PolicyStatusCodeTerminated];
                }
                else
                {
                    status = ProcessWarehouseRiDataBatch.PolicyStatusCodes[PickListDetailBo.PolicyStatusCodeReversal];
                }
            }
            
            if (!string.IsNullOrEmpty(RiDataWarehouseBo.PolicyStatusCode))
            {
                int endingPolicyStatus = ProcessWarehouseRiDataBatch.PolicyStatusCodes[RiDataWarehouseBo.PolicyStatusCode];
                RiDataWarehouseService.UpdateEndingPolicyStatusByPolicyNumber(RiDataWarehouseBo.PolicyNumber, RiDataWarehouseBo.CedingPlanCode, RiDataWarehouseBo.MlreBenefitCode, RiDataWarehouseBo.Id, endingPolicyStatus, User.DefaultSuperUserId);

                if (status == null)
                    status = endingPolicyStatus;
            }

            RiDataWarehouseBo.EndingPolicyStatus = status;
        }

        public bool SearchInWarehouse()
        {
            RiDataWarehouseBo = RiDataWarehouseService.FindByRiData(RiDataBo);
            return RiDataWarehouseBo != null;
        }

        public void SaveWarehouse()
        {
            var bo = RiDataWarehouseBo;
            RiDataWarehouseService.Save(ref bo);
        }

        public void SaveRiData()
        {
            var riData = RiDataBo;
            RiDataService.Update(ref riData);
        }
    }
}
