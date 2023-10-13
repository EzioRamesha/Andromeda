using BusinessObject;
using BusinessObject.Claims;
using BusinessObject.Identity;
using BusinessObject.RiDatas;
using DataAccess.Entities.Identity;
using DataAccess.Entities.RiDatas;
using DataAccess.EntityFramework;
using Services;
using Services.Claims;
using Services.Identity;
using Services.RiDatas;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands.RawFiles.ClaimRegister
{
    class CreateDummyData : Command
    {
        public ModuleBo ClaimRegisterModuleBo { get; set; }

        public bool ClaimRegister { get; set; } = false;
        public bool RiDataWarehouse { get; set; } = false;
        public bool ClaimData { get; set; } = false;
        public int? ClaimDataBatchId { get; set; }
        public int? ClaimDataConfigId { get; set; }

        public CreateDummyData()
        {
            Title = "CreateDummyData";
            Description = "To create dummy data";
            Options = new string[] {
                "--r|claimRegister : Create Dummy Claim Register",
                "--w|riDataWarehouse : Create Dummy RI Data Warehouse",
                "--c|claimData : Create Dummy Claim Data",
                "--claimDataBatchId= : Default Claim Data Batch Id",
                "--claimDataConfigId= : Default Claim Data Config Id",
            };
            Hide = true;
        }

        public override void Initial()
        {
            base.Initial();

            ClaimRegister = IsOption("claimRegister");
            RiDataWarehouse = IsOption("riDataWarehouse");
            ClaimData = IsOption("claimData");
            ClaimDataBatchId = OptionIntegerNullable("claimDataBatchId");
            ClaimDataConfigId = OptionIntegerNullable("claimDataConfigId");
        }

        public override void Run()
        {
            PrintStarting();

            ClaimRegisterModuleBo = ModuleService.FindByController(ModuleBo.ModuleController.ClaimRegister.ToString());

            // Claim Registers
            if (ClaimRegister)
            {
                if (ValidateClaimDataConfigId())
                {
                    CreateDummy1();
                    CreateTestDuplicate();
                    CreateTestError1();
                    CreateTestError2();
                    CreateTestError3();
                    CreateTestError4();
                } 
            }

            // Ri Data Warehouse
            if (RiDataWarehouse)
            {
                CreateWarehouse1();
                CreateWarehouse2();
            }

            // Claim Data
            if (ClaimData)
            {
                if (ValidateClaimDataBatchId())
                {
                    CreateClaimData1();
                    CreateClaimData2();
                }
            }

            PrintProcessCounts();
            PrintEnding();
        }

        public bool ValidateClaimDataBatchId()
        {
            if (ClaimDataBatchId == null)
            {
                PrintMessage("Claim Data Batch Id is required");
                return false;
            }
           
            if (!ClaimDataBatchService.IsExists(ClaimDataBatchId.Value))
            {
                PrintMessage("Claim Data Batch Id not found");
                return false;
            }

            return true;
        }

        public bool ValidateClaimDataConfigId()
        {
            if (ClaimDataConfigId == null)
            {
                PrintMessage("Claim Data Config Id is required");
                return false;
            }

            if (!ClaimDataConfigService.IsExists(ClaimDataConfigId.Value))
            {
                PrintMessage("Claim Data Config Id not found");
                return false;
            }

            return true;
        }

        public void PrintProcessCounts()
        {
            PrintMessageOnly("Dummy Data Created: ");
            foreach (var pair in ProcessCount)
            {
                PrintMessageOnly(string.Format("{0}: {1}", pair.Key, pair.Value));
            }
        }

        public ClaimDataBo ClaimDataBase()
        {
            return new ClaimDataBo()
            {
                ClaimDataBatchId = ClaimDataBatchId.Value,
                PreComputationStatus = ClaimDataBo.PreComputationStatusPending,
                PreValidationStatus = ClaimDataBo.PreValidationStatusPending,
                TempA1 = 10000,
                MlreRetainAmount = 5000,
                DateOfEvent = DateTime.Parse("2020-10-01"),
                CreatedById = User.DefaultSuperUserId,
            };
        }
        
        public ClaimRegisterBo ClaimRegisterBase()
        {
            return new ClaimRegisterBo()
            {
                ClaimStatus = ClaimRegisterBo.StatusReported,
                ClaimDataConfigId = ClaimDataConfigId,
                TempA1 = 10000,
                MlreRetainAmount = 5000,
                DateOfEvent = DateTime.Parse("2020-10-01"),
                CreatedById = User.DefaultSuperUserId,
            };
        }

        public RiDataWarehouseBo RiDataWarehouseBase()
        {
            return new RiDataWarehouseBo()
            {
                CreatedById = User.DefaultSuperUserId,
            };
        }

        public CutOffBo CutOffBase()
        {
            return new CutOffBo()
            {
                CreatedById = User.DefaultSuperUserId,
            };
        }

        public void CreateClaimData1()
        {
            var bo = ClaimDataBase();
            bo.MlreEventCode = "MECODE";
            bo.ClaimCode = "CCODE";
            bo.PolicyNumber = "UL20030036857";
            bo.TreatyCode = "HLA-05";
            bo.InsuredName = "HLA insured test";
            bo.InsuredGenderCode = "M";
            bo.Layer1SumRein = 13025.74;
            SaveClaimData(bo);
        }
        
        public void CreateClaimData2()
        {
            var bo = ClaimDataBase();
            bo.MlreEventCode = "MCODE";
            bo.ClaimCode = "CCODE2";
            bo.PolicyNumber = "UL20030036857";
            bo.TreatyCode = "HLA-05";
            bo.InsuredName = "HLA insured test 2";
            bo.InsuredGenderCode = "F";
            bo.Layer1SumRein = 77.34;
            bo.PreValidationStatus = ClaimDataBo.PreValidationStatusFailed;
            bo.PreComputationStatus = ClaimDataBo.PreComputationStatusSuccess;
            SaveClaimData(bo);
        }

        public void CreateWarehouse1()
        {
            var bo = RiDataWarehouseBase();
            bo.TreatyCode = "HLA-05";
            bo.CedingPlanCode = "CPCODE";
            bo.CedingBenefitTypeCode = "DTH";
            bo.CedingBenefitRiskCode = "CBRCODE";
            bo.PolicyNumber = "UL20030036857";
            bo.EndingPolicyStatus = RiDataWarehouseBo.PolicyStatusInforce;
            bo.Aar = 100.10;
            bo.NetPremium = 57.19;
            bo.PremiumFrequencyCode = "M";
            bo.RiskPeriodMonth = 2;
            bo.RiskPeriodYear = 2019;
            bo.LastUpdatedDate = DateTime.Parse("2020-10-01");
            bo.RiskPeriodStartDate = DateTime.Parse("2019-08-27");
            bo.RiskPeriodEndDate = DateTime.Parse("2020-08-27");
            SaveRiDataWarehouse(bo);
        }
        
        public void CreateWarehouse2()
        {
            var bo = RiDataWarehouseBase();
            bo.TreatyCode = "HLA-05";
            bo.CedingPlanCode = "CPCODE1";
            bo.CedingBenefitTypeCode = "DTH";
            bo.CedingBenefitRiskCode = "CBRCODE2";
            bo.PolicyNumber = "UL20030036857";
            bo.EndingPolicyStatus = RiDataWarehouseBo.PolicyStatusTerminated;
            bo.Aar = 12000.00;
            bo.NetPremium = 756.88;
            bo.PremiumFrequencyCode = "A";
            bo.RiskPeriodMonth = 12;
            bo.RiskPeriodYear = 2020;
            bo.LastUpdatedDate = DateTime.Parse("2020-10-09");
            bo.RiskPeriodStartDate = DateTime.Parse("2019-12-25");
            bo.RiskPeriodEndDate = DateTime.Parse("2020-12-10");
            SaveRiDataWarehouse(bo);
        }
        
        public void CreateWarehouse3()
        {
            var bo = RiDataWarehouseBase();
            bo.TreatyCode = "HLA-05";
            bo.CedingPlanCode = "CPCODE";
            bo.CedingBenefitTypeCode = "DTH";
            bo.CedingBenefitRiskCode = "CBRCODE";
            bo.PolicyNumber = "UL20030036857";
            bo.EndingPolicyStatus = RiDataWarehouseBo.PolicyStatusTerminated;
            bo.Aar = 12000.00;
            bo.NetPremium = 756.88;
            bo.PremiumFrequencyCode = "A";
            bo.RiskPeriodMonth = 12;
            bo.RiskPeriodYear = 2020;
            bo.LastUpdatedDate = DateTime.Parse("2020-10-09");
            bo.RiskPeriodStartDate = DateTime.Parse("2019-12-25");
            bo.RiskPeriodEndDate = DateTime.Parse("2020-12-10");
            SaveRiDataWarehouse(bo);
        }

        public void CreateDummy1()
        {
            var bo = ClaimRegisterBase();
            bo.CedingCompany = "HLA";
            bo.CedingEventCode = "CECODE";
            bo.CedingClaimType = "CTYPE";
            bo.FundsAccountingTypeCode = "GROUP";
            bo.PolicyNumber = "UL20030036857";
            bo.TreatyCode = "HLA-05";
            bo.InsuredName = "HLA insured test";
            bo.CedingPlanCode = "UCIR001QCM";
            SaveClaimRegister(bo);
        }
        
        public void CreateTestDuplicate()
        {
            var bo = ClaimRegisterBase();
            bo.CedingCompany = "HLA";
            bo.CedingEventCode = "CECODE";
            bo.CedingClaimType = "CTYPE";
            bo.FundsAccountingTypeCode = "INDIVIDUAL";
            bo.PolicyNumber = "UL20030036857";
            bo.TreatyCode = "HLA-05";
            bo.InsuredName = "HLA insured test";
            bo.CedingPlanCode = "UCIR001QCM";
            SaveClaimRegister(bo);
        }
        
        public void CreateTestError1()
        {
            // Validate Error in Event Claim Code
            var bo = ClaimRegisterBase();
            bo.FundsAccountingTypeCode = "INDIVIDUAL";
            bo.PolicyNumber = "UL20030036857";
            bo.TreatyCode = "HLA-05";
            bo.InsuredName = "HLA insured test";
            bo.CedingPlanCode = "UCIR001QCM";
            SaveClaimRegister(bo);
        }
        
        public void CreateTestError2()
        {
            // Not found in Event Claim Code
            var bo = ClaimRegisterBase();
            bo.CedingCompany = "HLA";
            bo.CedingEventCode = "CECODE";
            bo.CedingClaimType = "CTYPE3";
            bo.FundsAccountingTypeCode = "INDIVIDUAL";
            bo.PolicyNumber = "UL20030036857";
            bo.TreatyCode = "HLA-05";
            bo.InsuredName = "HLA insured test";
            bo.CedingPlanCode = "UCIR001QCM";
            SaveClaimRegister(bo);
        }
        
        public void CreateTestError3()
        {
            // Validate Error in Ri Data Mapping
            var bo = ClaimRegisterBase();
            bo.CedingCompany = "HLA";
            bo.CedingEventCode = "CECODE";
            bo.CedingClaimType = "CTYPE2";
            SaveClaimRegister(bo);
        }

        public void CreateTestError4()
        {
            // Not found in Ri Data Mapping
            var bo = ClaimRegisterBase();
            bo.CedingCompany = "HLA";
            bo.CedingEventCode = "CECODE";
            bo.CedingClaimType = "CTYPE";
            bo.FundsAccountingTypeCode = "INDIVIDUAL";
            bo.PolicyNumber = "UL20030036860";
            bo.TreatyCode = "HLA-05";
            bo.InsuredName = "HLA insured error";
            bo.CedingPlanCode = "UCIR001QCM";
            SaveClaimRegister(bo);
        }

        public void CreateTestError5()
        {
            // Validate Error in Claim Code Mapping
            var bo = ClaimRegisterBase();
            bo.CedingCompany = "HLA";
            bo.CedingEventCode = "CECODE";
            bo.CedingClaimType = "CTYPE";
            bo.FundsAccountingTypeCode = "INDIVIDUAL";
            bo.PolicyNumber = "UL20030036857";
            bo.TreatyCode = "HLA-05";
            bo.InsuredName = "HLA insured test";
            bo.CedingPlanCode = "UCIR001QCM";
            SaveClaimRegister(bo);
        }

        public void SaveClaimRegister(ClaimRegisterBo bo)
        {
            TrailObject trail = new TrailObject();
            Result result = ClaimRegisterService.Create(ref bo, ref trail);

            StatusHistoryBo statusBo = new StatusHistoryBo
            {
                ModuleId = ClaimRegisterModuleBo.Id,
                ObjectId = bo.Id,
                Status = bo.ClaimStatus,
                CreatedById = User.DefaultSuperUserId,
                UpdatedById = User.DefaultSuperUserId,
            };
            StatusHistoryService.Create(ref statusBo, ref trail);

            UserTrailBo userTrailBo = new UserTrailBo(
                bo.Id,
                "Create new Claim Register",
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            SetProcessCount("Claim Register");
        }
        
        public void SaveRiDataWarehouse(RiDataWarehouseBo bo)
        {
            TrailObject trail = new TrailObject();
            Result result = RiDataWarehouseService.Create(ref bo, ref trail);

            UserTrailBo userTrailBo = new UserTrailBo(
                bo.Id,
                "Create new RI Data Warehouse",
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            SetProcessCount("RI Data Warehouse");
        }
        
        public void SaveClaimData(ClaimDataBo bo)
        {
            TrailObject trail = new TrailObject();
            Result result = ClaimDataService.Create(ref bo, ref trail);

            UserTrailBo userTrailBo = new UserTrailBo(
                bo.Id,
                "Create new Claim Data",
                result,
                trail,
                User.DefaultSuperUserId
            );
            UserTrailService.Create(ref userTrailBo);

            SetProcessCount("Claim Data");
        }
    }
}
