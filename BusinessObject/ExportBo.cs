using BusinessObject.RiDatas;
using Newtonsoft.Json;
using Shared;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;

namespace BusinessObject
{
    public class ExportBo
    {
        public int Id { get; set; }

        public int Type { get; set; }

        public int Status { get; set; }

        public int? ObjectId { get; set; }

        public int Total { get; set; }

        public int Processed { get; set; }

        public string Parameters { get; set; }

        public dynamic ParameterObject { get; set; }

        public Dictionary<string, object> ParameterDic { get; set; }

        public DateTime? GenerateStartAt { get; set; }

        public DateTime? GenerateEndAt { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public bool TempFlag { get; set; }

        public const int TypeRiData = 1;
        public const int TypeClaimData = 2;
        public const int TypeClaimRegister = 3;
        public const int TypeRiDataWarehouse = 4;
        public const int TypeRiDataSearch = 5;
        public const int TypeClaimRegisterSearch = 6;
        public const int TypeReferralClaim = 7;
        public const int TypeInvoiceRegister = 8;
        public const int TypeRetroRegister = 9;
        public const int TypeClaimRegisterHistorySearch = 10;
        public const int TypeTreatyWorkflow = 11;
        public const int TypeQuotationWorkflow = 12;
        public const int TypeValidDuplicationList = 13;
        public const int TypeRiDataWarehouseHistory = 14;
        public const int TypeGroupReferral = 15;
        public const int TypePerLifeClaimSummaryPaidClaims = 16;
        public const int TypePerLifeClaimSummaryPendingClaims = 17;
        public const int TypePerLifeClaimSummaryClaimsRemoved = 18;
        public const int TypePerLifeAggregationConflictListing = 19;
        public const int TypePerLifeAggregationDuplicationListing = 20;
        public const int TypePerLifeAggregationRiData = 21;
        public const int TypePerLifeAggregationException = 22;
        public const int TypePerLifeAggregationRetroRiData = 23;
        public const int TypePerLifeAggregationRetentionPremium = 24;
        public const int TypePerLifeAggregationRetroSummaryExcludedRecord = 25;
        public const int TypePerLifeAggregationRetroSummaryRetro = 26;
        public const int TypePerLifeAggregationSummaryExcludedRecord = 27;
        public const int TypePerLifeAggregationSummaryRetro = 28;
        public const int TypeGroupReferralTrackingCase = 29;
        public const int TypeFacMasterListing = 30;
        public const int TypeRateDetail = 31;
        public const int MaxType = 31;

        public const int StatusPending = 1;
        public const int StatusGenerating = 2;
        public const int StatusCompleted = 3;
        public const int StatusSuspended = 4;
        public const int StatusFailed = 5;
        public const int StatusCancelled = 6;
        public const int MaxStatus = 6;

        public ExportBo()
        {
            Status = StatusPending;
        }

        public static string GetTypeName(int key)
        {
            switch (key)
            {
                case TypeRiData:
                    return "RI Data";
                case TypeClaimData:
                    return "Claim Data";
                case TypeClaimRegister:
                    return "Claim Register";
                case TypeRiDataWarehouse:
                case TypeRiDataWarehouseHistory:
                    return "RI Data Warehouse";
                case TypeRiDataSearch:
                    return "RI Data Search";
                case TypeClaimRegisterSearch:
                case TypeClaimRegisterHistorySearch:
                    return "Claim Register Search";
                case TypeReferralClaim:
                    return "Referral Claim";
                case TypeInvoiceRegister:
                    return "Invoice Register";
                case TypeRetroRegister:
                    return "Retro Register";
                case TypeTreatyWorkflow:
                    return "Treaty Pricing Treaty Workflow";
                case TypeQuotationWorkflow:
                    return "Treaty Pricing Quotation Workflow";
                case TypeValidDuplicationList:
                    return "Valid Duplication List";
                case TypeGroupReferral:
                    return "Treaty Pricing Group Referral";
                case TypePerLifeClaimSummaryPaidClaims:
                    return "Per Life Claim Summary Paid Claims";
                case TypePerLifeClaimSummaryPendingClaims:
                    return "Per Life Claim Summary Pending Claims";
                case TypePerLifeClaimSummaryClaimsRemoved:
                    return "Per Life Claim Summary Claims Removed";
                case TypePerLifeAggregationConflictListing:
                    return "Per Life Aggregation Conflict Listing";
                case TypePerLifeAggregationDuplicationListing:
                    return "Per Life Aggregation Duplication Listing";
                case TypePerLifeAggregationRiData:
                    return "Per Life Aggregation RI Data";
                case TypePerLifeAggregationException:
                    return "Per Life Aggregation Exception";
                case TypePerLifeAggregationRetroRiData:
                    return "Per Life Aggregation Retro RI Data";
                case TypePerLifeAggregationRetentionPremium:
                    return "Per Life Aggregation Retention Premium";
                case TypePerLifeAggregationRetroSummaryExcludedRecord:
                    return "Per Life Aggregation Retro Summary (Excluded Record)";
                case TypePerLifeAggregationRetroSummaryRetro:
                    return "Per Life Aggregation Retro Summary (Retro)";
                case TypePerLifeAggregationSummaryExcludedRecord:
                    return "Per Life Aggregation Summary (Excluded Record)";
                case TypePerLifeAggregationSummaryRetro:
                    return "Per Life Aggregation Summary (Retro)";
                case TypeGroupReferralTrackingCase:
                    return "Treaty Pricing Group Referral (Tracking Case)";
                case TypeFacMasterListing:
                    return "Fac Master Listing";
                case TypeRateDetail:
                    return "Rate Details";
                default:
                    return "";
            }
        }

        public static string GetTypeFileName(int key)
        {
            switch (key)
            {
                case TypeRiData:
                    return "RiData";
                case TypeClaimData:
                    return "ClaimData";
                case TypeClaimRegister:
                    return "ClaimRegister";
                case TypeRiDataWarehouse:
                case TypeRiDataWarehouseHistory:
                    return "RiDataWarehouse";
                case TypeRiDataSearch:
                    return "RiDataSearch";
                case TypeClaimRegisterSearch:
                case TypeClaimRegisterHistorySearch:
                    return "ClaimRegisterSearch";
                case TypeReferralClaim:
                    return "ReferralClaim";
                case TypeInvoiceRegister:
                    return "InvoiceRegister";
                case TypeRetroRegister:
                    return "RetroRegister";
                case TypeTreatyWorkflow:
                    return "TreatyPricingTreatyWorkflow";
                case TypeQuotationWorkflow:
                    return "TreatyPricingQuotationWorkflow";
                case TypeValidDuplicationList:
                    return "ValidDuplicationList";
                case TypeGroupReferral:
                    return "TreatyPricingGroupReferral";
                case TypePerLifeClaimSummaryPaidClaims:
                    return "PerLifeClaimSummaryPaidClaims";
                case TypePerLifeClaimSummaryPendingClaims:
                    return "PerLifeClaimSummaryPendingClaims";
                case TypePerLifeClaimSummaryClaimsRemoved:
                    return "PerLifeClaimSummaryClaimsRemoved";
                case TypePerLifeAggregationConflictListing:
                    return "PerLifeAggregationConflictListing";
                case TypePerLifeAggregationDuplicationListing:
                    return "PerLifeAggregationDuplicationListing";
                case TypePerLifeAggregationRiData:
                    return "PerLifeAggregationRiData";
                case TypePerLifeAggregationException:
                    return "PerLifeAggregationException";
                case TypePerLifeAggregationRetroRiData:
                    return "PerLifeAggregationRetroRiData";
                case TypePerLifeAggregationRetentionPremium:
                    return "PerLifeAggregationRetentionPremium";
                case TypePerLifeAggregationRetroSummaryExcludedRecord:
                    return "PerLifeAggregationRetroSummary-ExcludedRecord";
                case TypePerLifeAggregationRetroSummaryRetro:
                    return "PerLifeAggregationRetroSummary-Retro";
                case TypePerLifeAggregationSummaryExcludedRecord:
                    return "PerLifeAggregationSummary-ExcludedRecord";
                case TypePerLifeAggregationSummaryRetro:
                    return "PerLifeAggregationSummary-Retro";
                case TypeGroupReferralTrackingCase:
                    return "TreatyPricingGroupReferral-TrackingCase";
                case TypeFacMasterListing:
                    return "FacMasterListing";
                case TypeRateDetail:
                    return "RateDetails";
                default:
                    return "";
            }
        }

        public static string GetTypeFileExtension(int key)
        {
            switch (key)
            {
                case TypePerLifeAggregationRetroSummaryExcludedRecord:
                case TypePerLifeAggregationRetroSummaryRetro:
                case TypePerLifeAggregationSummaryExcludedRecord:
                case TypePerLifeAggregationSummaryRetro:
                    return "zip";
                case TypeGroupReferral:
                case TypeGroupReferralTrackingCase:
                    return "xlsx";
                default:
                    return "csv";
            }
        }

        public static string GetTypeContentType(int key)
        {
            switch (key)
            {
                case TypePerLifeAggregationRetroSummaryExcludedRecord:
                case TypePerLifeAggregationRetroSummaryRetro:
                case TypePerLifeAggregationSummaryExcludedRecord:
                case TypePerLifeAggregationSummaryRetro:
                    return "application/zip";
                case TypeGroupReferral:
                case TypeGroupReferralTrackingCase:
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                default:
                    return "text/csv";
            }
        }

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusPending:
                    return "Pending";
                case StatusGenerating:
                    return "Generating";
                case StatusCompleted:
                    return "Completed";
                case StatusSuspended:
                    return "Suspended";
                case StatusFailed:
                    return "Failed";
                case StatusCancelled:
                    return "Cancelled";
                default:
                    return "";
            }
        }

        public static string GetStatusClass(int key)
        {
            switch (key)
            {
                case StatusPending:
                    return "status-pending-badge";
                case StatusGenerating:
                    return "status-processing-badge";
                case StatusCompleted:
                    return "status-success-badge";
                case StatusSuspended:
                case StatusFailed:
                case StatusCancelled:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        public static string GetValue(string key, object value)
        {
            if (value == null)
                return null;
            int v;
            switch (key)
            {
                case "MappingStatus":
                    if (int.TryParse(value.ToString(), out v))
                        return RiDataBo.GetMappingStatusName(v);
                    break;
                case "PreComputation1Status":
                    if (int.TryParse(value.ToString(), out v))
                        return RiDataBo.GetPreComputation1StatusName(v);
                    break;
                case "PreComputation2Status":
                    if (int.TryParse(value.ToString(), out v))
                        return RiDataBo.GetPreComputation2StatusName(v);
                    break;
                case "PreValidationStatus":
                    if (int.TryParse(value.ToString(), out v))
                        return RiDataBo.GetPreValidationStatusName(v);
                    break;
                case "PostComputationStatus":
                    if (int.TryParse(value.ToString(), out v))
                        return RiDataBo.GetPostComputationStatusName(v);
                    break;
                case "PostValidationStatus":
                    if (int.TryParse(value.ToString(), out v))
                        return RiDataBo.GetPostValidationStatusName(v);
                    break;
                case "FinaliseStatus":
                    if (int.TryParse(value.ToString(), out v))
                        return RiDataBo.GetFinaliseStatusName(v);
                    break;
            }
            return value.ToString();
        }

        public void ConvertParametersObject()
        {
            if (!string.IsNullOrEmpty(Parameters))
                ParameterObject = JsonConvert.DeserializeObject<ExpandoObject>(Parameters);
        }

        public void ConvertParametersDic()
        {
            if (!string.IsNullOrEmpty(Parameters))
                ParameterDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(Parameters);
        }

        public void ConvertParameters()
        {
            if (ParameterObject != null)
                Parameters = JsonConvert.SerializeObject(ParameterObject);
        }

        public bool IsSuspended()
        {
            return Status == StatusSuspended;
        }

        public string GetDirectory()
        {
            return Util.GetExportPath();
        }

        public string GetFileNameSuffix()
        {
            switch (Type)
            {
                case TypePerLifeAggregationSummaryExcludedRecord:
                case TypePerLifeAggregationSummaryRetro:
                    ConvertParametersObject();
                    return Util.HasProperty(ParameterObject, "RiskQuarter") ? string.Format("-{0}", ParameterObject.RiskQuarter) : "";
                default:
                    return "";
            }
        }

        public string GetFileName()
        {
            return string.Format("Export{0}{1}-{2}.{3}", GetTypeFileName(Type), GetFileNameSuffix(), Id, GetTypeFileExtension(Type));
        }

        public string GetPath()
        {
            return Path.Combine(GetDirectory(), GetFileName());
        }

        public string GetContentType()
        {
            return GetTypeContentType(Type);
        }

        public bool IsFileExists()
        {
            return File.Exists(GetPath());
        }

        public bool WriteHeader(bool writableHeader = true)
        {
            if (ParameterObject == null)
                return writableHeader;
            switch (Type)
            {
                case TypeRiDataSearch:
                case TypeClaimRegisterSearch:
                case TypeRiDataWarehouse:
                case TypeRiDataWarehouseHistory:
                    return (ParameterObject.WriteHeader == true);
                default:
                    return writableHeader;
            }
        }
    }
}
