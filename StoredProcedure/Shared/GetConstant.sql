CREATE OR ALTER FUNCTION [dbo].[GetConstant] (@Name VARCHAR(MAX)) RETURNS VARCHAR(MAX)

AS

BEGIN
	RETURN 
		CASE (@Name)
			-- General
			WHEN 'AuthUserId' THEN '1'
			-- Pick List (Code)
			WHEN 'TransactionTypeCodeAlteration' THEN 'AL'
			WHEN 'PolicyStatusCodeTerminated' THEN 'T'
			WHEN 'PolicyStatusCodeReversal' THEN 'R'
			WHEN 'FundsAccountingTypeCodeIndividual' THEN 'INDIVIDUAL'
			WHEN 'FundsAccountingTypeCodeGroup' THEN 'GROUP'
			-- RI Data
			WHEN 'RiDataRecordTypeNew' THEN '1'
			WHEN 'RiDataRecordTypeAdj' THEN '2'
			WHEN 'ProcessWarehouseStatusPending' THEN '1'
			WHEN 'ProcessWarehouseStatusSuccess' THEN '2'
			WHEN 'ProcessWarehouseStatusFailed' THEN '3'
			-- Claim Register
			WHEN 'ClaimStatusApproved' THEN '13'
			-- Sanction
			WHEN 'SanctionBatchStatusSuccess' THEN '3'
			WHEN 'PreviousDecisionPending' THEN '1'
			WHEN 'PreviousDecisionWhitelist' THEN '2'
			WHEN 'PreviousDecisionExactMatch' THEN '3'
			WHEN 'RuleIdentity' THEN '1'
			WHEN 'RuleNameSymbolRemoval' THEN '2'
			WHEN 'RuleNameKeywordReplacement' THEN '3'
			WHEN 'RuleNameGroupKeyword' THEN '4'
			WHEN 'RuleNameGroupKeywordReplacement' THEN '5'
			-- Per Life
			WHEN 'AggregationDetailStatusPending' THEN '1'
			WHEN 'AggregationDetailStatusValidationSuccess' THEN '4'
			WHEN 'RetentionLimitTypeIndividual' THEN '1'
			WHEN 'RetentionLimitTypeGroup' THEN '2'
			-- Exception Type
			WHEN 'ExceptionTypeFieldValidation' THEN '1'
			WHEN 'ExceptionTypeBasicCheck' THEN '2'
			WHEN 'ExceptionTypeConflictCheck' THEN '3'
			WHEN 'ExceptionTypeDuplicationCheck' THEN '4'
			WHEN 'ExceptionTypeRetroBenefitCodeMapping' THEN '5'
			WHEN 'ExceptionTypeRetroBenefitRetentionLimit' THEN '6'
			-- Exception Error Type
			WHEN 'ExceptionErrorTypeMissingInfo' THEN '1'
			WHEN 'ExceptionErrorTypeAarNull' THEN '2'
			WHEN 'ExceptionErrorTypeNetPremiumNull' THEN '3'
			WHEN 'ExceptionErrorTypeAarLessThanZero' THEN '4'
			WHEN 'ExceptionErrorTypeNetPremiumLessThanZero' THEN '5'
			WHEN 'ExceptionErrorTypeMissingNetPremiumZeroAarLessThanMinRetentionAmount' THEN '6'
			WHEN 'ExceptionErrorTypeMissingNetPremiumZeroAarMoreOrEqualMinRetentionAmount' THEN '7'
			WHEN 'ExceptionErrorTypeGenderNullOrNotPermitted' THEN '8'
			WHEN 'ExceptionErrorTypeTerritoryCodeNullOrNotPermitted' THEN '9'
			WHEN 'ExceptionErrorTypeReinsEffDateNullOrGreaterThanSystem' THEN '10'
			WHEN 'ExceptionErrorTypeUwNullOrLessThanZero' THEN '11'
			WHEN 'ExceptionErrorTypeCedingTreatyCodeNull' THEN '12'
			WHEN 'ExceptionErrorTypeTreatyCodeNull' THEN '13'
			WHEN 'ExceptionErrorTypeSameLifeAssureConflictInGenderOrTerrritoryCode' THEN '14'
			WHEN 'ExceptionErrorTypeDuplicationRecord' THEN '15'
			WHEN 'ExceptionErrorTypeRetroBenefitCodeMapping' THEN '16'
			WHEN 'ExceptionErrorTypeRetroBenefitRetentionLimit' THEN '17'
			-- Flag Code
			WHEN 'FlagCodeBad' THEN '1'
			WHEN 'FlagCodeGood1' THEN '2'
			WHEN 'FlagCodeQ1' THEN '3'
			WHEN 'FlagCodeQ2d1' THEN '4'
			WHEN 'FlagCodeQ2d2' THEN '5'
			WHEN 'FlagCodeQ2d3' THEN '6'
			WHEN 'FlagCodeQ2d4' THEN '7'
			WHEN 'FlagCodeQ2dA' THEN '8'
			WHEN 'ProceedStatusProceed' THEN '1'
			WHEN 'ProceedStatusNotProceed' THEN '2'
			ELSE ''
		END  
END;