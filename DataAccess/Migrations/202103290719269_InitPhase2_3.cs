namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitPhase2_3 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.RiDataCorrections", new[] { "CedantId" });
            DropIndex("dbo.RiDataCorrections", new[] { "TreatyCodeId" });
            CreateIndex("dbo.AnnuityFactorDetails", new[] { "PolicyTermRemain", "InsuredGenderCodePickListDetailId", "InsuredTobaccoUsePickListDetailId", "InsuredAttainedAge", "PolicyTerm", "AnnuityFactorId" }, name: "IX_AnnuityMapping");
            CreateIndex("dbo.AnnuityFactors", "ReinsEffDatePolStartDate");
            CreateIndex("dbo.AnnuityFactors", "ReinsEffDatePolEndDate");
            CreateIndex("dbo.AnnuityFactorMappings", new[] { "CedingPlanCode", "AnnuityFactorId" }, name: "IX_AnnuityFactorMapping");
            CreateIndex("dbo.FacMasterListingDetails", new[] { "PolicyNumber", "FacMasterListingId" }, name: "IX_FacMasterListingMapping");
            CreateIndex("dbo.Mfrs17CellMappingDetails", new[] { "CedingPlanCode", "BenefitCode", "TreatyCode", "Mfrs17CellMappingId" }, name: "IX_Mfrs17CellMapping");
            CreateIndex("dbo.Mfrs17CellMappingDetails", "TreatyCode");
            CreateIndex("dbo.RateDetails", new[] { "InsuredGenderCodePickListDetailId", "CedingTobaccoUsePickListDetailId", "CedingOccupationCodePickListDetailId", "AttainedAge", "IssueAge", "PolicyTerm", "PolicyTermRemain", "RateId" }, name: "IX_RateMapping");
            CreateIndex("dbo.RateTableDetails", new[] { "TreatyCode", "CedingPlanCode", "CedingTreatyCode", "CedingPlanCode2", "CedingBenefitTypeCode", "CedingBenefitRiskCode", "GroupPolicyNumber", "RateTableId" }, name: "IX_RateTableMapping");
            CreateIndex("dbo.RateTables", "ReinsEffDatePolStartDate");
            CreateIndex("dbo.RateTables", "ReinsEffDatePolEndDate");
            CreateIndex("dbo.RiData", new[] { "PolicyNumber", "CedingPlanCode", "MlreBenefitCode", "TreatyCode", "RiskPeriodMonth", "RiskPeriodYear", "RiderNumber" }, name: "IX_RiDataLookup");
            CreateIndex("dbo.RiData", "MlreBenefitCode");
            CreateIndex("dbo.RiData", "RiderNumber");
            CreateIndex("dbo.RiDataCorrections", new[] { "CedantId", "PolicyNumber", "InsuredRegisterNo", "TreatyCodeId" }, name: "IX_DataCorrection");
            CreateIndex("dbo.RiDataCorrections", "PolicyNumber");
            CreateIndex("dbo.RiDataCorrections", "InsuredRegisterNo");
            CreateIndex("dbo.TreatyBenefitCodeMappingDetails", new[] { "CedingPlanCode", "CedingBenefitTypeCode", "CedingBenefitRiskCode", "CedingTreatyCode", "CampaignCode", "TreatyBenefitCodeMappingId" }, name: "IX_ProductFeatureMapping");
        }
        
        public override void Down()
        {
            DropIndex("dbo.TreatyBenefitCodeMappingDetails", "IX_ProductFeatureMapping");
            DropIndex("dbo.RiDataCorrections", new[] { "InsuredRegisterNo" });
            DropIndex("dbo.RiDataCorrections", new[] { "PolicyNumber" });
            DropIndex("dbo.RiDataCorrections", "IX_DataCorrection");
            DropIndex("dbo.RiData", new[] { "RiderNumber" });
            DropIndex("dbo.RiData", new[] { "MlreBenefitCode" });
            DropIndex("dbo.RiData", "IX_RiDataLookup");
            DropIndex("dbo.RateTables", new[] { "ReinsEffDatePolEndDate" });
            DropIndex("dbo.RateTables", new[] { "ReinsEffDatePolStartDate" });
            DropIndex("dbo.RateTableDetails", "IX_RateTableMapping");
            DropIndex("dbo.RateDetails", "IX_RateMapping");
            DropIndex("dbo.Mfrs17CellMappingDetails", new[] { "TreatyCode" });
            DropIndex("dbo.Mfrs17CellMappingDetails", "IX_Mfrs17CellMapping");
            DropIndex("dbo.FacMasterListingDetails", "IX_FacMasterListingMapping");
            DropIndex("dbo.AnnuityFactorMappings", "IX_AnnuityFactorMapping");
            DropIndex("dbo.AnnuityFactors", new[] { "ReinsEffDatePolEndDate" });
            DropIndex("dbo.AnnuityFactors", new[] { "ReinsEffDatePolStartDate" });
            DropIndex("dbo.AnnuityFactorDetails", "IX_AnnuityMapping");
            CreateIndex("dbo.RiDataCorrections", "TreatyCodeId");
            CreateIndex("dbo.RiDataCorrections", "CedantId");
        }
    }
}
