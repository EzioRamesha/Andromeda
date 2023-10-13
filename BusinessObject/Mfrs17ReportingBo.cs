using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Mfrs17ReportingBo
    {
        public int Id { get; set; }

        public string Quarter { get; set; }
        public QuarterObject QuarterObject { get; set; }

        public int Status { get; set; }

        public int TotalRecord { get; set; }

        public int? GenerateType { get; set; }

        public bool? GenerateModifiedOnly { get; set; }

        public double? GeneratePercentage { get; set; }

        public int CutOffId { get; set; }

        public CutOffBo CutOffBo { get; set; }

        public string Errors { get; set; }

        public bool? IsResume { get; set; } = false;

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const string DateQuarterFormatJs = "yyyy MM";

        public const string RegexPattern = @"^[0-9]{4}\s[Q]{1}[1-4]{1}$";

        public const int TypeLatestDataEndDate = 100001; // DO NOT CRASH with STANDARD OUTPUT ID

        public List<int> Mfrs17ReportingColumns = new List<int>
        {
            StandardOutputBo.TypeTreatyCode,
            StandardOutputBo.TypeReinsBasisCode,
            StandardOutputBo.TypeFundsAccountingTypeCode,
            StandardOutputBo.TypePremiumFrequencyCode,
            StandardOutputBo.TypeReportPeriodMonth,
            StandardOutputBo.TypeReportPeriodYear,
            StandardOutputBo.TypeRiskPeriodMonth,
            StandardOutputBo.TypeRiskPeriodYear,
            StandardOutputBo.TypeTransactionTypeCode,
            StandardOutputBo.TypePolicyNumber,
            StandardOutputBo.TypeReinsEffDatePol,
            StandardOutputBo.TypeCedingPlanCode,
            StandardOutputBo.TypeCedingBenefitTypeCode,
            StandardOutputBo.TypeCedingBenefitRiskCode,
            StandardOutputBo.TypeMlreBenefitCode,
            StandardOutputBo.TypeOriSumAssured,
            StandardOutputBo.TypeCurrSumAssured,
            StandardOutputBo.TypeAmountCededB4MlreShare,
            StandardOutputBo.TypeAar,
            StandardOutputBo.TypeAarSpecial1,
            StandardOutputBo.TypeAarSpecial2,
            StandardOutputBo.TypeAarSpecial3,
            StandardOutputBo.TypeInsuredGenderCode,
            StandardOutputBo.TypeInsuredTobaccoUse,
            StandardOutputBo.TypeInsuredDateOfBirth,
            StandardOutputBo.TypeInsuredOccupationCode,
            StandardOutputBo.TypeInsuredAttainedAge,
            StandardOutputBo.TypeInsuredGenderCode2nd,
            StandardOutputBo.TypeInsuredTobaccoUse2nd,
            StandardOutputBo.TypeInsuredDateOfBirth2nd,
            StandardOutputBo.TypeInsuredAttainedAge2nd,
            StandardOutputBo.TypeReinsuranceIssueAge,
            StandardOutputBo.TypeReinsuranceIssueAge2nd,
            StandardOutputBo.TypePolicyTerm,
            StandardOutputBo.TypePolicyExpiryDate,
            StandardOutputBo.TypeDurationYear,
            StandardOutputBo.TypeDurationDay,
            StandardOutputBo.TypeDurationMonth,
            StandardOutputBo.TypePremiumCalType,
            StandardOutputBo.TypeRateTable,
            StandardOutputBo.TypeAgeRatedUp,
            StandardOutputBo.TypeLoadingType,
            StandardOutputBo.TypeUnderwriterRating,
            StandardOutputBo.TypeUnderwriterRatingUnit,
            StandardOutputBo.TypeUnderwriterRatingTerm,
            StandardOutputBo.TypeUnderwriterRating2,
            StandardOutputBo.TypeUnderwriterRatingUnit2,
            StandardOutputBo.TypeUnderwriterRatingTerm2,
            StandardOutputBo.TypeUnderwriterRating3,
            StandardOutputBo.TypeUnderwriterRatingUnit3,
            StandardOutputBo.TypeUnderwriterRatingTerm3,
            StandardOutputBo.TypeFlatExtraAmount,
            StandardOutputBo.TypeFlatExtraUnit,
            StandardOutputBo.TypeFlatExtraTerm,
            StandardOutputBo.TypeFlatExtraAmount2,
            StandardOutputBo.TypeFlatExtraTerm2,
            StandardOutputBo.TypeStandardPremium,
            StandardOutputBo.TypeSubstandardPremium,
            StandardOutputBo.TypeFlatExtraPremium,
            StandardOutputBo.TypeGrossPremium,
            StandardOutputBo.TypeStandardDiscount,
            StandardOutputBo.TypeSubstandardDiscount,
            StandardOutputBo.TypeVitalityDiscount,
            StandardOutputBo.TypeTotalDiscount,
            StandardOutputBo.TypeNetPremium,
            StandardOutputBo.TypeRiCovPeriod,
            StandardOutputBo.TypeAdjBeginDate,
            StandardOutputBo.TypeAdjEndDate,
            StandardOutputBo.TypePolicyStatusCode,
            StandardOutputBo.TypePolicyStandardPremium,
            StandardOutputBo.TypePolicySubstandardPremium,
            StandardOutputBo.TypeLineOfBusiness,
            StandardOutputBo.TypeLoanInterestRate,
            StandardOutputBo.TypeDefermentPeriod,
            StandardOutputBo.TypeCampaignCode,
            StandardOutputBo.TypeTerritoryOfIssueCode,
            StandardOutputBo.TypeCedingTreatyCode,
            StandardOutputBo.TypeCedingPlanCodeOld,
            StandardOutputBo.TypeCedantReinsurerCode,
            StandardOutputBo.TypeAmountCededB4MlreShare2,
            StandardOutputBo.TypeGroupPolicyNumber,
            StandardOutputBo.TypeGroupPolicyName,
            StandardOutputBo.TypeNoOfEmployee,
            StandardOutputBo.TypePolicyTotalLive,
            StandardOutputBo.TypeGroupSubsidiaryName,
            StandardOutputBo.TypeGroupSubsidiaryNo,
            StandardOutputBo.TypeCedingPlanCode2,
            StandardOutputBo.TypeDependantIndicator,
            StandardOutputBo.TypeGhsRoomBoard,
            StandardOutputBo.TypeSpecialIndicator1,
            StandardOutputBo.TypeSpecialIndicator2,
            StandardOutputBo.TypeSpecialIndicator3,
            StandardOutputBo.TypeIndicatorJointLife,
            StandardOutputBo.TypeGstIndicator,
            StandardOutputBo.TypeMfrs17BasicRider,
            StandardOutputBo.TypeMfrs17CellName,
            StandardOutputBo.TypeMfrs17TreatyCode,
            TypeLatestDataEndDate,
            StandardOutputBo.TypeMfrs17AnnualCohort,
        };

        public static int GetMfrs17ReportingColumnRevisedLength(int key)
        {
            switch (key)
            {
                case StandardOutputBo.TypePolicyTerm:
                    return 11;
                case StandardOutputBo.TypeDurationDay:
                case StandardOutputBo.TypeAgeRatedUp:
                case StandardOutputBo.TypeLoadingType:
                case StandardOutputBo.TypeAdjEndDate:
                case TypeLatestDataEndDate:
                    return 12;
                case StandardOutputBo.TypeDurationYear:
                case StandardOutputBo.TypeRiCovPeriod:
                case StandardOutputBo.TypeCampaignCode:
                case StandardOutputBo.TypeGstIndicator:
                    return 13;
                case StandardOutputBo.TypeDurationMonth:
                case StandardOutputBo.TypeAdjBeginDate:
                case StandardOutputBo.TypeNoOfEmployee:
                case StandardOutputBo.TypeGhsRoomBoard:
                    return 14;
                case StandardOutputBo.TypeOriSumAssured:
                case StandardOutputBo.TypeAar:
                case StandardOutputBo.TypeAarSpecial1:
                case StandardOutputBo.TypeAarSpecial2:
                case StandardOutputBo.TypeAarSpecial3:
                case StandardOutputBo.TypeFlatExtraUnit:
                case StandardOutputBo.TypeFlatExtraTerm:
                case StandardOutputBo.TypeGrossPremium:
                case StandardOutputBo.TypeTotalDiscount:
                case StandardOutputBo.TypeNetPremium:
                    return 15;
                case StandardOutputBo.TypeRiskPeriodYear:
                case StandardOutputBo.TypeCurrSumAssured:
                case StandardOutputBo.TypePremiumCalType:
                case StandardOutputBo.TypeStandardPremium:
                case StandardOutputBo.TypeLineOfBusiness:
                case StandardOutputBo.TypeDefermentPeriod:
                    return 16;
                case StandardOutputBo.TypeMlreBenefitCode:
                case StandardOutputBo.TypeRiskPeriodMonth:
                case StandardOutputBo.TypeFlatExtraAmount:
                case StandardOutputBo.TypeFlatExtraTerm2:
                case StandardOutputBo.TypeStandardDiscount:
                case StandardOutputBo.TypeVitalityDiscount:
                case StandardOutputBo.TypePolicyTotalLive:
                    return 17;
                case StandardOutputBo.TypeReportPeriodYear:
                case StandardOutputBo.TypeReinsEffDatePol:
                case StandardOutputBo.TypePolicyExpiryDate:
                case StandardOutputBo.TypeUnderwriterRating:
                case StandardOutputBo.TypeFlatExtraPremium:
                case StandardOutputBo.TypeLoanInterestRate:
                case StandardOutputBo.TypeMfrs17BasicRider:
                    return 18;
                case StandardOutputBo.TypeReportPeriodMonth:
                case StandardOutputBo.TypeInsuredGenderCode:
                case StandardOutputBo.TypeInsuredTobaccoUse:
                case StandardOutputBo.TypeFlatExtraAmount2:
                case StandardOutputBo.TypeSubstandardPremium:
                case StandardOutputBo.TypeDependantIndicator:
                case StandardOutputBo.TypeSpecialIndicator1:
                case StandardOutputBo.TypeSpecialIndicator2:
                case StandardOutputBo.TypeSpecialIndicator3:
                    return 19;
                case StandardOutputBo.TypeInsuredAttainedAge:
                case StandardOutputBo.TypeUnderwriterRating2:
                case StandardOutputBo.TypeUnderwriterRating3:
                case StandardOutputBo.TypeSubstandardDiscount:
                case StandardOutputBo.TypePolicyStatusCode:
                case StandardOutputBo.TypeIndicatorJointLife:
                case StandardOutputBo.TypeMfrs17AnnualCohort:
                    return 20;
                case StandardOutputBo.TypeTransactionTypeCode:
                case StandardOutputBo.TypeInsuredDateOfBirth:
                case StandardOutputBo.TypeReinsuranceIssueAge:
                case StandardOutputBo.TypeCedantReinsurerCode:
                    return 21;
                case StandardOutputBo.TypePremiumFrequencyCode:
                    return 22;
                case StandardOutputBo.TypeInsuredOccupationCode:
                case StandardOutputBo.TypeInsuredGenderCode2nd:
                case StandardOutputBo.TypeInsuredTobaccoUse2nd:
                case StandardOutputBo.TypeUnderwriterRatingUnit:
                case StandardOutputBo.TypeUnderwriterRatingTerm:
                case StandardOutputBo.TypePolicyStandardPremium:
                case StandardOutputBo.TypeTerritoryOfIssueCode:
                    return 23;
                case StandardOutputBo.TypeInsuredAttainedAge2nd:
                    return 24;
                case StandardOutputBo.TypeInsuredDateOfBirth2nd:
                case StandardOutputBo.TypeReinsuranceIssueAge2nd:
                case StandardOutputBo.TypeUnderwriterRatingUnit2:
                case StandardOutputBo.TypeUnderwriterRatingTerm2:
                case StandardOutputBo.TypeUnderwriterRatingUnit3:
                case StandardOutputBo.TypeUnderwriterRatingTerm3:
                case StandardOutputBo.TypeMfrs17TreatyCode:
                    return 25;
                case StandardOutputBo.TypeAmountCededB4MlreShare:
                case StandardOutputBo.TypePolicySubstandardPremium:
                    return 26;
                case StandardOutputBo.TypeReinsBasisCode:
                case StandardOutputBo.TypeFundsAccountingTypeCode:
                case StandardOutputBo.TypeCedingPlanCode:
                case StandardOutputBo.TypeCedingBenefitTypeCode:
                case StandardOutputBo.TypeCedingTreatyCode:
                case StandardOutputBo.TypeCedingPlanCodeOld:
                case StandardOutputBo.TypeAmountCededB4MlreShare2:
                case StandardOutputBo.TypeGroupPolicyNumber:
                case StandardOutputBo.TypeGroupSubsidiaryNo:
                case StandardOutputBo.TypeCedingPlanCode2:
                    return 30;
                case StandardOutputBo.TypeTreatyCode:
                    return 35;
                case StandardOutputBo.TypeCedingBenefitRiskCode:
                case StandardOutputBo.TypeMfrs17CellName:
                    return 50;
                case StandardOutputBo.TypeRateTable:
                    return 90;
                case StandardOutputBo.TypeGroupPolicyName:
                case StandardOutputBo.TypeGroupSubsidiaryName:
                    return 128;
                case StandardOutputBo.TypePolicyNumber:
                    return 150;
                default:
                    return 10;
            }
        }

        public static string GetPropertyNameByType(int key)
        {
            switch (key)
            {
                case TypeLatestDataEndDate:
                    return "LatestDataEndDate";
            }
            return "";
        }

        public static string GetMfrs17ReportingColumnName(int key)
        {
            switch (key)
            {
                case TypeLatestDataEndDate:
                    return "INFORCE_DATE";
                default:
                    return "";
            }
        }

        public static string GetMfrs17ReportingColumnAlign(int key)
        {
            switch (key)
            {
                case StandardOutputBo.TypeTreatyCode:
                case StandardOutputBo.TypeReinsBasisCode:
                case StandardOutputBo.TypeFundsAccountingTypeCode:
                case StandardOutputBo.TypePremiumFrequencyCode:
                case StandardOutputBo.TypeTransactionTypeCode:
                case StandardOutputBo.TypePolicyNumber:
                case StandardOutputBo.TypeCedingPlanCode:
                case StandardOutputBo.TypeCedingBenefitTypeCode:
                case StandardOutputBo.TypeCedingBenefitRiskCode:
                case StandardOutputBo.TypeMlreBenefitCode:
                case StandardOutputBo.TypeInsuredGenderCode:
                case StandardOutputBo.TypeInsuredTobaccoUse:
                case StandardOutputBo.TypeInsuredOccupationCode:
                case StandardOutputBo.TypeInsuredGenderCode2nd:
                case StandardOutputBo.TypeInsuredTobaccoUse2nd:
                case StandardOutputBo.TypePremiumCalType:
                case StandardOutputBo.TypeRateTable:
                case StandardOutputBo.TypeLoadingType:
                case StandardOutputBo.TypePolicyStatusCode:
                case StandardOutputBo.TypeLineOfBusiness:
                case StandardOutputBo.TypeCampaignCode:
                case StandardOutputBo.TypeTerritoryOfIssueCode:
                case StandardOutputBo.TypeCedingTreatyCode:
                case StandardOutputBo.TypeCedingPlanCodeOld:
                case StandardOutputBo.TypeCedantReinsurerCode:
                case StandardOutputBo.TypeGroupPolicyNumber:
                case StandardOutputBo.TypeGroupPolicyName:
                case StandardOutputBo.TypeGroupSubsidiaryName:
                case StandardOutputBo.TypeGroupSubsidiaryNo:
                case StandardOutputBo.TypeCedingPlanCode2:
                case StandardOutputBo.TypeDependantIndicator:
                case StandardOutputBo.TypeIndicatorJointLife:
                case StandardOutputBo.TypeGstIndicator:
                case StandardOutputBo.TypeMfrs17BasicRider:
                case StandardOutputBo.TypeMfrs17CellName:
                case StandardOutputBo.TypeMfrs17TreatyCode:
                    return "R";
                default:
                    return "L";
            }
        }

        public const int StatusPending = 1;
        public const int StatusSubmitForProcessing = 2;
        public const int StatusProcessing = 3;
        public const int StatusSuccess = 4;
        public const int StatusFailed = 5;
        public const int StatusFinalised = 6;
        public const int StatusPendingGenerate = 7;
        public const int StatusGenerating = 8;
        public const int StatusPendingUpdate = 9;
        public const int StatusUpdating = 10;
        public const int StatusFailedOnGenerate = 11;

        public const int StatusMax = 11;

        public const int GenerateTypeSingle = 1;
        public const int GenerateTypeMultiple = 2;

        public const int GenerateTypeMax = 2;

        public static string GetStatusName(int key)
        {
            switch (key)
            {
                case StatusPending:
                    return "Pending";
                case StatusSubmitForProcessing:
                    return "Submit For Processing";
                case StatusProcessing:
                    return "Processing";
                case StatusSuccess:
                    return "Success";
                case StatusFailed:
                    return "Failed";
                case StatusFinalised:
                    return "Finalised";
                case StatusPendingGenerate:
                    return "Pending Generate";
                case StatusGenerating:
                    return "Generating";
                case StatusPendingUpdate:
                    return "Pending Update";
                case StatusUpdating:
                    return "Updating";
                case StatusFailedOnGenerate:
                    return "Failed - Generate";
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
                case StatusSubmitForProcessing:
                    return "status-submitprocess-badge";
                case StatusProcessing:
                    return "status-processing-badge";
                case StatusSuccess:
                    return "status-success-badge";
                case StatusFailed:
                    return "status-fail-badge";
                case StatusFinalised:
                    return "status-finalize-badge";
                case StatusPendingGenerate:
                    return "status-submitprocess-badge";
                case StatusGenerating:
                    return "status-processing-badge";
                case StatusPendingUpdate:
                    return "status-pending-badge";
                case StatusUpdating:
                    return "status-processing-badge";
                case StatusFailedOnGenerate:
                    return "status-fail-badge";
                default:
                    return "";
            }
        }

        public static string GetGenerateTypeName(int key)
        {
            switch (key)
            {
                case GenerateTypeSingle:
                    return "Single";
                case GenerateTypeMultiple:
                    return "Multiple";
                default:
                    return "";
            }
        }

        public void GetQuarterObject()
        {
            QuarterObject = new QuarterObject(Quarter);
        }
    }

    public class RiskQuarterDate
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime DataStartDate { get; set; }

        public DateTime DataEndDate { get; set; }

        public string RiskQuarter { get; set; }

        public const int TypeMonthly = 1;
        public const int TypeQuarterly = 2;
        public const int TypeSemiAnnual = 3;
        public const int TypeAnnual = 4;

        public static RiskQuarterDate GetRiskQuarterDate(DateTime date, int type)
        {
            RiskQuarterDate riskQuarterDate = new RiskQuarterDate { };
            switch (type)
            {
                case TypeMonthly:
                    var startDate = new DateTime(date.Year, date.Month, 1);
                    var endDate = date;
                    var dataStartDate = startDate;
                    var dataEndDate = date;
                    var quarter = new QuarterObject(startDate.Month, startDate.Year);

                    riskQuarterDate = new RiskQuarterDate
                    {
                        StartDate = startDate,
                        EndDate = endDate,
                        DataStartDate = dataStartDate,
                        DataEndDate = dataEndDate,
                        RiskQuarter = quarter.Quarter,
                    };
                    return riskQuarterDate;

                case TypeQuarterly:
                    startDate = new DateTime(date.AddMonths(-2).Year, date.AddMonths(-2).Month, 1);
                    endDate = date;
                    dataStartDate = startDate;
                    dataEndDate = date;
                    quarter = new QuarterObject(startDate.Month, startDate.Year);

                    riskQuarterDate = new RiskQuarterDate
                    {
                        StartDate = startDate,
                        EndDate = endDate,
                        DataStartDate = dataStartDate,
                        DataEndDate = dataEndDate,
                        RiskQuarter = quarter.Quarter,
                    };
                    return riskQuarterDate;

                default:
                    return riskQuarterDate;
            }
        }

        public static List<RiskQuarterDate> GetRiskQuarterDates(DateTime date, int type)
        {
            List<RiskQuarterDate> riskQuarterDates = new List<RiskQuarterDate> { };
            switch (type)
            {
                case TypeSemiAnnual:
                    var startDate = new DateTime(date.AddMonths(-5).Year, date.AddMonths(-5).Month, 1);
                    var endDate = new DateTime(date.AddMonths(-3).Year, date.AddMonths(-3).Month, 1);
                    var dataStartDate = startDate;
                    var dataEndDate = date;
                    var quarter = new QuarterObject(startDate.Month, startDate.Year);

                    riskQuarterDates.Add(new RiskQuarterDate
                    {
                        StartDate = startDate,
                        EndDate = endDate,
                        DataStartDate = dataStartDate,
                        DataEndDate = dataEndDate,
                        RiskQuarter = quarter.Quarter,
                    });

                    startDate = new DateTime(date.AddMonths(-2).Year, date.AddMonths(-2).Month, 1);
                    endDate = date;
                    quarter = new QuarterObject(startDate.Month, startDate.Year);
                    riskQuarterDates.Add(new RiskQuarterDate
                    {
                        StartDate = startDate,
                        EndDate = endDate,
                        DataStartDate = dataStartDate,
                        DataEndDate = dataEndDate,
                        RiskQuarter = quarter.Quarter,
                    });
                    return riskQuarterDates;

                case TypeAnnual:
                    startDate = new DateTime(date.AddMonths(-11).Year, date.AddMonths(-11).Month, 1);
                    endDate = new DateTime(date.AddMonths(-9).Year, date.AddMonths(-9).Month, 1);
                    dataStartDate = startDate;
                    dataEndDate = date;
                    quarter = new QuarterObject(startDate.Month, startDate.Year);

                    riskQuarterDates.Add(new RiskQuarterDate
                    {
                        StartDate = startDate,
                        EndDate = endDate,
                        DataStartDate = dataStartDate,
                        DataEndDate = dataEndDate,
                        RiskQuarter = quarter.Quarter,
                    });

                    startDate = new DateTime(date.AddMonths(-8).Year, date.AddMonths(-8).Month, 1);
                    endDate = new DateTime(date.AddMonths(-6).Year, date.AddMonths(-6).Month, 1);
                    quarter = new QuarterObject(startDate.Month, startDate.Year);
                    riskQuarterDates.Add(new RiskQuarterDate
                    {
                        StartDate = startDate,
                        EndDate = endDate,
                        DataStartDate = dataStartDate,
                        DataEndDate = dataEndDate,
                        RiskQuarter = quarter.Quarter,
                    });

                    startDate = new DateTime(date.AddMonths(-5).Year, date.AddMonths(-5).Month, 1);
                    endDate = new DateTime(date.AddMonths(-3).Year, date.AddMonths(-3).Month, 1);
                    quarter = new QuarterObject(startDate.Month, startDate.Year);
                    riskQuarterDates.Add(new RiskQuarterDate
                    {
                        StartDate = startDate,
                        EndDate = endDate,
                        DataStartDate = dataStartDate,
                        DataEndDate = dataEndDate,
                        RiskQuarter = quarter.Quarter,
                    });

                    startDate = new DateTime(date.AddMonths(-2).Year, date.AddMonths(-2).Month, 1);
                    endDate = date;
                    quarter = new QuarterObject(startDate.Month, startDate.Year);
                    riskQuarterDates.Add(new RiskQuarterDate
                    {
                        StartDate = startDate,
                        EndDate = endDate,
                        DataStartDate = dataStartDate,
                        DataEndDate = dataEndDate,
                        RiskQuarter = quarter.Quarter,
                    });
                    return riskQuarterDates;

                default:
                    return riskQuarterDates;
            }
        }

        public static RiskQuarterDate GetRiskQuarterDate(string riskQuarter, string premiumFrequencyCodePickListDetailCode)
        {
            string[] quarter = riskQuarter.Split(' ').Select(p => p.Trim()).ToArray();
            int startMonth = 0;
            int endMonth = 0;
            switch(quarter[1])
            {
                case "Q1":
                    startMonth = 1;
                    endMonth = 3;
                    break;
                case "Q2":
                    startMonth = 4;
                    endMonth = 6;
                    break;
                case "Q3":
                    startMonth = 7;
                    endMonth = 9;
                    break;
                case "Q4":
                    startMonth = 10;
                    endMonth = 12;
                    break;
            }
            int year = int.Parse(quarter[0]);
            int lastDay = DateTime.DaysInMonth(year, endMonth);

            if (premiumFrequencyCodePickListDetailCode == "M")
                startMonth = endMonth;

            DateTime startDate = new DateTime(year, startMonth, 1);
            DateTime endDate = new DateTime(year, endMonth, lastDay);

            return new RiskQuarterDate
            {
                StartDate = startDate,
                EndDate = endDate,
                RiskQuarter = riskQuarter,
            };
        }

    }

    public class Mfrs17ReportingFile
    {
        public string FileName { get; set; }

        public string SubFolder { get; set; }

        public string CreatedAtStr { get; set; }

        public string ModifiedAtStr { get; set; }
    }
}
