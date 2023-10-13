using System.Collections.Generic;

namespace Shared
{
    public class MessageBag
    {
        public const string AccessDenied = "Sorry, you do not have permission to access this page";
        public const string AccessDeniedWithName = "Sorry, you do not have permission to {0} these details";
        public const string AccessDeniedWithActionName = "Sorry, you do not have permission to {0} this {1}";
        public const string UploadAccessDenied = "Sorry, you do not have permission to upload";
        public const string InvalidUrl = "Invalid URL";
        public const string FriendlyExceptionError = "Oops! An error has occurred. The error was logged, please contact Enrii Support immediately.";
        public const string EmailError = "Email service is currently unavailable. Please contact admin for further details.";
        public const string NewUserNotSent = "The user details was not sent to the new user's email address as the email service is currently unavailable.";
        public const string NewPasswordNotSent = "The new password was not sent to the user's email address as the email service is currently unavailable.";
        public const string InvalidEmail = "Email {0} is invalid";
        public const string RequestPasswordDenied = "You are not permitted to request for a password. Please contact admin for further details.";
        public const string FileNotExists = "The file doesn't exists: {0}";
        public const string FileNotSupport = "The file doesn't support: {0}";
        public const string DirectoryNotExists = "The directory doesn't exists: {0}";
        public const string AlreadyTaken = "{0} '{1}' is already taken.";
        public const string RecordFound = "Record found!";
        public const string NoRecordFound = "No record found!";
        public const string NoRecordFoundIn = "No record found in {0}!";
        public const string NoRecordFoundWithName = "No {0} record found!";
        public const string NoRecordFoundWithNameIn = "No {0} record found in {1}!";
        public const string Required = "{0} is required";
        public const string RequiredWithRow = "{0} is required at row #{1}";
        public const string IsEmpty = "{0} is empty";
        public const string ForModuleAtNo = "{0} for {1} at #{2}";
        public const string MaxLength = "The length of the input string({0}) is more than database max length({1})";
        public const string SubStringPosition = "The start position({0}) is more than or equal the length of the input string({1})";
        public const string InputTableEmpty = "Input table is empty";
        public const string DuplicateAccessGroup = "Another record with the same access group \"{0}\" already exists";
        public const string ConcurrentProcess = "Another record with the same status \"{0}\" already processing";
        public const string ValueIsNull = "Value is null";
        public const string IsNullWithName = "{0}'s is null";
        public const string NotExistsWithValue = "The {0} doesn't exists: {1}";
        public const string ObjectLockedBy = "User {0} is editing this record since {1}";
        public const string ObjectLockLostError = "You no longer have the control to edit this record";

        public const string CreateSuccessfully = "Record has been created successfully";
        public const string CreateSuccessfullyWithName = "{0} has been created successfully";
        public const string UpdateSuccessfully = "Record has been updated successfully";
        public const string UpdateSuccessfullyWithName = "{0} has been updated successfully";
        public const string DeleteSuccessfully = "Record has been deleted successfully";
        public const string DeleteSuccessfullyWithName = "{0} has been deleted successfully";

        public const string UserSuccessfullySuspended = "User has been suspended successfully";
        public const string UserSuccessfullyActivated = "User has been activated successfully";
        public const string UserNotFoundInAd = "User not found in Active Directory";
        public const string UsernameRequired = "Username is required to search for Active Directory users";
        public const string DuplicateUsername = "Username {0} is already taken.";
        public const string DuplicateEmail = "Email {0} is already taken.";
        public const string PasswordUsed = "This password has been used recently";
        public const string PasswordEmailedSuccesfully = "New password has been sent to the user's email address";
        public const string InvalidLogin = "Invalid login attempt.";
        public const string UserSuspended = "You are Suspended. Please contact admin for further details.";
        public const string UnableUpdateSelf = "You cannot edit your own account's details.";
        public const string UnableDeleteSelf = "You cannot delete your own account.";

        public const string PasswordLengthRequirement = "be {0} - {1} characters long";
        public const string PasswordContainRequirement = "contain at least {0}";

        public const string UnableInsert = "Unable to insert this record.";
        public const string UnableUpdate = "Unable to update this record.";
        public const string UnableDelete = "Unable to delete this record.";

        public const string NoInactiveUser = "No inactive user found";

        public const string UnableDeleteRecordInUsed = "This record is currently in used and could not be deleted";
        public const string UnableDeleteRecordInUsedWithName = "This {0} record is currently in used and could not be deleted";
        public const string UnableProcessRecordInUsed = "This record is currently in used and could not be processed";
        public const string UnableProcessPerLifeAggregation = "This record could not be processed as there are data in this record that is linked to Per Life Claims and/or Per Life SOA";
        public const string UnableDeletePerLifeAggregation = "This record could not be deleted as there are data in this record that is linked to Per Life Claims and/or Per Life SOA";

        public const string ObjectSubmitted = "{0} has been Submitted for approval!";
        public const string ObjectApproved = "{0} has been approved!";
        public const string ObjectRejected = "{0} has been rejected!";
        public const string ObjectDisabled = "{0} has been disabled!";
        public const string ObjectInUse = "{0} is currently in use & cannot be edited!";
        public const string DuplicateRawValue = "Duplicate Raw Value: {0}";

        public const string CedantDisabled = "Reminder: The Cedant Company \" {0} \" already been disabled";
        public const string BenefitDisabled = "Reminder: The MLRe Benefit \" {0} \" already been disabled";
        public const string TreatyCodeDisabled = "Reminder: The Treaty Code \" {0} \" already been disabled";

        public const string CedantStatusInactive = "The Cedant status is Inactive";
        public const string BenefitStatusInactive = "The Benefit status is Inactive";
        public const string BenefitStatusInactiveWithCode = "The Benefit status is Inactive: {0}";
        public const string TreatyCodeStatusInactive = "The Treaty Code status is Inactive";
        public const string TreatyCodeStatusInactiveWithCode = "The Treaty Code status is Inactive: {0}";
        public const string TreatyCodeNotBelongsToCedant = "The Treaty Code: {0} not belongs to Cedant Code: {1}";
        public const string ClaimCodeStatusInactive = "The Claim Code status is Inactive";
        public const string ClaimCodeStatusInactiveWithCode = "The Claim Code status is Inactive: {0}";
        public const string EventCodeStatusInactive = "The MLRe Event Code status is Inactive";
        public const string EventCodeStatusInactiveWithCode = "The MLRe Event Code status is Inactive: {0}";
        public const string RetroPartyStatusInactive = "The Retro Party status is Inactive";
        public const string RetroPartyStatusInactiveWithParty = "The Retro Party status is Inactive: {0}";
        public const string RetroBenefitCodeStatusInactive = "The Retro Benfit Code status is Inactive";
        public const string RetroBenefitCodeStatusInactiveWithCode = "The Retro Benfit Code status is Inactive: {0}";

        public const string BenefitCodeNotFound = "The MLRe Benefit Code is not found: {0}";
        public const string TreatyCodeNotFound = "The Treaty Code is not found: {0}";
        public const string EventCodeNotFound = "The MLRe Event Code is not found: {0}";
        public const string CedantNotFound = "The Cedant is not found: {0}";
        public const string ClaimCodeNotFound = "The Claim Code is not found: {0}";
        public const string RetroPartyNotFound = "The Retro Party is not found: {0}";

        public const string InvalidDateFormatWithValue = "Invalid Date Input: {0}";

        public const string EndDefaultDate = "The {0} Start Date Field must be ealier than {1}";
        public const string StartDateEarlier = "The {0} Start Date Field must be ealier than the {0} End Date Field";
        public const string EndDateLater = "The {0} End Date Field must be later than the {0} Start Date Field";
        public const string InvalidDateFormat = "Invalid Date Format";
        public const string InvalidDateFormatWithName = "Invalid {0} Format";
        public const string InvalidDateFormatWithNameWithRow = "Invalid {0} Format at row #{1}";

        public const string GreaterThan = "The {0} must be greater than the {1}";
        public const string GreaterThanWithRow = "The {0} must be greater than the {1} at row #{2}";
        public const string GreaterOrEqualTo = "The {0} must be greater or equal to The {1}";
        public const string GreaterOrEqualToWithRow = "The {0} must be greater or equal to The {1} at row #{2}";
        public const string LowerOrEqualThan = "The {0} must be lower or equal than The {1}";
        public const string LowerOrEqualToWithRow = "The {0} must be lower or equal to The {1} at row #{2}";

        // RI Data
        public const string RiDataSubmittedForPreProcessing = "RI Data has been Submitted for Pre-Processing!";
        public const string RiDataSubmittedForPostProcessing = "RI Data has been Submitted for Post-Processing!";
        public const string RiDataSubmittedForFinalise = "RI Data has been Submitted for finalise!";
        public const string RiDataSubmittedForProcessWarehouse = "RI Data has been Submitted for processing to warehouse!";
        public const string RiDataConfigStatusInactive = "The RI Data Config status is {0}. Please submit for Approval.";

        // Claim Data
        public const string ClaimDataSubmittedForProcessing = "Claim Data has been Submitted for processing!";
        public const string ClaimDataSubmittedForReportClaim = "Claim Data has been Submitted for report claim!";
        public const string ClaimDataConfigStatusInactive = "The Claim Data Config status is {0}. Please submit for Approval.";

        // No batch records
        public const string NoBatchPendingDelete = "No batch records pending to detele";
        public const string NoBatchPendingProcess = "No batch records pending to process";
        public const string NoBatchPendingFinalise = "No batch records pending to finalise";
        public const string NoBatchPendingCompile = "No batch records pending to compile to Warehouse";

        // File Config Error Message
        public const string DelimiterFixedLengthIsNull = "The DelimiterFixedLength is null, Property: {0}";
        public const string DelimiterFixedLengthIsZero = "The DelimiterFixedLength is zero, Property: {0}";
        public const string MultipleMappingRecordsFound = "Multiple records matched!";
        public const string MultipleMappingRecordsFoundWithName = "{0} multiple records matched!";
        public const string MappingDetailCombination = "  Id: {0}, DetailId: {1}, Combination: {2}";
        public const string TotalMappingDetailCombination = "  Total number of {0}: {1}";
        public const string ComputationConditionPassed = "The computation condition passed!";
        public const string ComputationConditionFailed = "The computation condition failed!";
        public const string ComputationResultNull = "The computation result value is null!";
        public const string NoComputationConfigured = "No computation was configured!";
        public const string NoComputationCondition = "No computation conditions were passed!";
        public const string NoPreValidationConfigured = "No pre-validation was configured!";
        public const string NoPostValidationConfigured = "No post-validation record was configured!";
        public const string UnableSetToDateTimeValue = "The data type is unable to set DateTime value";
        public const string UnableSetToStringValue = "The data type is unable to set String value";
        public const string UnableSetToIntValue = "The data type is unable to set Int value";
        public const string UnableSetToDoubleValue = "The data type is unable to set Double value";
        public const string UnableSetToBooleanValue = "The data type is unable to set Boolean value";
        public const string UnableSetValue = "The data type is not supported: {0}";
        public const string NoRecordMatchParams = "No record was matched by parameters!";
        public const string NoRecordMatchParamsWithName = "{0} no record was matched by parameters!";

        // ProcessRiData
        public const string ProcessRiDataBatchPreProcessing = "Pre-Processing RI Data Batch Success";
        public const string ProcessRiDataBatchPreSuccess = "Pre-Process RI Data Batch Success";
        public const string ProcessRiDataBatchPreFailed = "Pre-Process RI Data Batch Failed";
        public const string ProcessRiDataBatchPostProcessing = "Post-Processing RI Data Batch Success";
        public const string ProcessRiDataBatchPostSuccess = "Post-Process RI Data Batch Success";
        public const string ProcessRiDataBatchPostFailed = "Post-Process RI Data Batch Failed";
        public const string ProcessRiDataFileProcessing = "Processing RI Data File";
        public const string ProcessRiDataFileCompleted = "Process RI Data File Completed";
        public const string ProcessRiDataFileCompletedFailed = "Process RI Data File Completed - Failed";
        public const string ProcessRiDataFileCompletedExcluded = "Process RI Data File Completed - Excluded";

        // FinaliseRiDataBatch
        public const string FinalisingRiDataBatch = "Finalising RI Data Batch";
        public const string FinalisedRiDataBatch = "Finalised RI Data Batch";
        public const string FinalisedRiDataBatchFailed = "Finalised RI Data Batch Failed";

        // ProcessClaimDataBatch
        public const string ProcessClaimDataBatchProcessing = "Processing SOA Data Batch";
        public const string ProcessClaimDataBatchSuccess = "Process Claim Data Batch Success";
        public const string ProcessClaimDataBatchFailed = "Process Claim Data Batch Failed";
        public const string ProcessClaimDataFileProcessing = "Processing Claim Data File";
        public const string ProcessClaimDataFileCompleted = "Process Claim Data File Completed";
        public const string ProcessClaimDataFileCompletedFailed = "Process Claim Data File Completed - Failed";
        public const string ProcessClaimDataFileCompletedExcluded = "Process Claim Data File Completed - Excluded";

        // ProcessSoaDataBatch
        public const string ProcessSoaDataBatchProcessing = "Processing SOA Data Batch";
        public const string ProcessSoaDataBatchSuccess = "Process SOA Data Batch Success";
        public const string ProcessSoaDataBatchFailed = "Process SOA Data Batch Failed";
        public const string ProcessSoaDataFileProcessing = "Processing SOA Data File";
        public const string ProcessSoaDataFileCompleted = "Process SOA Data File Completed";
        public const string ProcessSoaDataFileCompletedFailed = "Process SOA Data File Completed - Failed";
        public const string ProcessSoaDataFileCompletedExcluded = "Process SOA Data File Completed - Excluded";
        public const string SummarySoaDataBatchDataUpdating = "SOA Data Batch Data Updating";
        public const string SummarySoaDataBatchDataUpdateCompleted = "SOA Data Batch Data Update Completed";

        // ReportingClaimDataBatch
        public const string ReportedClaimDataBatch = "Reported Claim Data Batch";
        public const string ReportingClaimDataBatchFailed = "Reporting Claim Data Batch Failed";

        // ProcessMfrs17Reporting
        public const string NoMfrs17reportingPendingProcess = "No MFRS17 Reporting pending to process";
        public const string NoMfrs17reportingPendingUpdate = "No MFRS17 Reporting pending update";
        public const string NoPremiumFrequencyFound = "No Premium Frequncy Found";
        public const string InvalidQuarterFormat = "Invalid Quarter Format: {0}";

        // ProcessClaimRegister
        public const string NoClaimRegisterPendingProcess = "No Claim Register records pending to process";
        public const string NoClaimRegisterPendingProvision = "No Claim Register records pending provision";
        public const string NoClaimRegisterPendingProvisionReprocess = "No Claim Register records pending provision reprocess";

        // ReferralClaim
        public const string NoReferralClaimPendingAssessment = "No Referral Claim records pending assessment";

        // ProcessWarehouseRiDataBatch
        public const string ProcessedWarehouseRiDataBatch = "Processed RI Data Batch to Warehouse";
        public const string ProcessWarehouseRiDataBatchFailed = "Process RI Data Batch to Warehouse Failed";

        // GenerateMfrs17Reporting
        public const string NoMfrs17reportingPendingGenerate = "No MFRS17 Reporting pending to generate";

        // ProcessCutOff
        public const string NoCutOffPendingToProcess = "No Cut Off record pending to process";
        public const string ProcessCannotRunDueToCutOff = "Process cannot be run as Cut Off process is running";

        // Phase 2 General
        public const string DuplicateRecordFound = "Duplicate Record Found";

        // Export
        public const string ExportSuccessfullySuspended = "Export has been suspended successfully";
        public const string ExportSuccessfullySubmitForProcess = "Export has been submit for processing successfully";
        public const string ExportSuccessfullyCancelled = "Export has been cancelled succesfully";

        // ProcessDirectRetro
        public const string NoDirectRetroPendingProcess = "No Direct Retro pending to process";

        // SanctionUpload
        public const string NoSanctionBatchPendingProcess = "No Sanction Batch pending to process";
        public const string ProcessSanctionBatchProcessing = "Processing Sanction Upload";
        public const string ProcessSanctionBatchSuccess = "Process Sanction Upload Success";
        public const string ProcessSanctionBatchFailed = "Process Sanction Upload Failed";

        // SanctionVerification
        public const string NotTimeToCreateAutoVerification = "Not Time to Create Auto Sanction Verification";
        public const string NoSanctionVerificationPendingProcess = "No Sanction Verification pending to process";

        // TreatyPricingRateTableGroup
        public const string NoTreatyPricingRateTableGroupPendingProcess = "No Treaty Pricing Rate Table Group pending to process";
        public const string ProcessTreatyPricingRateTableGroupProcessing = "Processing Treaty Pricing Rate Table Group";
        public const string ProcessTreatyPricingRateTableGroupSuccess = "Process Treaty Pricing Rate Table Group Success";
        public const string ProcessTreatyPricingRateTableGroupFailed = "Process Treaty Pricing Rate Table Group Failed";

        // PerLifeAggregation
        public const string NoPerLifeAggregationPendingProcess = "No Per Life Aggregation pending to process";
        public const string ProcessPerLifeAggregationProcessing = "Processing Per Life Aggregation";
        public const string ProcessPerLifeAggregationSuccess = "Process Per Life Aggregation Success";
        public const string ProcessPerLifeAggregationFailed = "Process Per Life Aggregation Failed";

        // PerLifeAggregationDetail
        public const string NoPerLifeAggregationDetailPendingValidation = "No Per Life Aggregation Detail pending validation";
        public const string ProcessPerLifeAggregationDetailValidating = "Processing Per Life Aggregation Detail Validation";
        public const string ProcessPerLifeAggregationDetailValidationSuccess = "Processing Per Life Aggregation Detail Success Validation";
        public const string ProcessPerLifeAggregationDetailValidationFailed = "Processing Per Life Aggregation Detail Failed Validation";

        public const string NoPerLifeAggregationDetailPendingProcess = "No Per Life Aggregation Detail pending to process";
        public const string ProcessPerLifeAggregationDetailProcessing = "Processing Per Life Aggregation Detail";
        public const string ProcessPerLifeAggregationDetailSuccess = "Process Per Life Aggregation Detail Success";
        public const string ProcessPerLifeAggregationDetailFailed = "Process Per Life Aggregation Detail Failed";

        public const string NoPerLifeAggregationDetailPendingAggregation = "No Per Life Aggregation Detail pending aggregation";
        public const string ProcessPerLifeAggregationDetailProcessingAggregation = "Processing Per Life Aggregation Detail";
        public const string ProcessPerLifeAggregationDetailAggregationSuccess = "Process Per Life Aggregation Detail Aggregation Success";
        public const string ProcessPerLifeAggregationDetailAggregationFailed = "Process Per Life Aggregation Detail Aggregation Failed";

        public const string StoredProcedureNotFound = "Stored Procedure not found";

        // FinanceProvisioning
        public const string NoFinanceProvisioningPendingProcess = "No Finance Provisioning pending to process";
        public const string ProcessFinanceProvisioningProcessing = "Processing Finance Provisioning";
        public const string ProcessFinanceProvisioningProcessingWithId = "Processing Finance Provisioning Id: {0}";
        public const string ProcessFinanceProvisioningSuccess = "Process Finance Provisioning Success";
        public const string ProcessFinanceProvisioningFailed = "Process Finance Provisioning Failed";

        // PerLifeClaims
        public const string NoPerLifeClaimPendingProcess = "No Per Life Claim pending to process";
        public const string ProcessPerLifeClaimProcessing = "Processing Per Life Claim";
        public const string ProcessPerLifeClaimSuccess = "Process Per Life Claim Success";
        public const string ProcessPerLifeClaimFailed = "Process Per Life Claim Failed";

        public const string NoPerLifeClaimDataPendingValidation = "No Per Life Claim Data pending validation";
        public const string ProcessPerLifeClaimDataValidating = "Processing Per Life Claim Data Validation";
        public const string ProcessPerLifeClaimDataValidationSuccess = "Processing Per Life Claim Data Success Validation";
        public const string ProcessPerLifeClaimDataValidationFailed = "Processing Per Life Claim Data Failed Validation";

        public const string NoPerLifeClaimRetroRecoveryPendingProcess = "No Per Life Claim Retro Recovery pending to process";
        public const string ProcessPerLifeClaimRetroRecoveryProcessing = "Processing Per Life Claim Retro Recovery";
        public const string ProcessPerLifeClaimRetroRecoverySuccess = "Process Per Life Claim Retro Recovery Success";
        public const string ProcessPerLifeClaimRetroRecoveryFailed = "Process Per Life Claim Retro Recovery Failed";

        // PerLifeSoa
        public const string NoPerLifeSoaPendingProcess = "No Per Life SOA pending to process";
        public const string ProcessPerLifeSoaProcessing = "Processing Per Life SOA";
        public const string ProcessPerLifeSoaSuccess = "Process Per Life SOA Success";
        public const string ProcessPerLifeSoaFailed = "Process Per Life SOA Failed";

        public List<string> Success { get; set; } = new List<string>();

        public List<string> Errors { get; set; } = new List<string>();

        public List<string> Warnings { get; set; } = new List<string>();

        public void AddSuccess(string success)
        {
            Success.Add(success);
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public void AddWarning(string warning)
        {
            Warnings.Add(warning);
        }

        public void AddTakenError(string key, string value)
        {
            Errors.Add(string.Format(AlreadyTaken, key, value));
        }

        public void AddUnableInsert()
        {
            Errors.Add(UnableInsert);
        }

        public void AddUnableUpdate()
        {
            Errors.Add(UnableUpdate);
        }

        public void AddUnableDelete()
        {
            Errors.Add(UnableDelete);
        }
    }
}
