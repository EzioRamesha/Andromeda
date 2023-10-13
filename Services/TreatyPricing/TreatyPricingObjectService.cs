using BusinessObject.TreatyPricing;
using DataAccess.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace Services.TreatyPricing
{
    public class TreatyPricingObjectService
    {
        public static TreatyPricingObjectBo FormBo(int id, string text = null)
        {
            return new TreatyPricingObjectBo
            {
                Id = id,
                Text = text,
            };
        }

        public static IList<TreatyPricingObjectBo> FormBos(IList<int> ids = null, IList<string> texts = null)
        {
            if (ids == null)
                return null;
            IList<TreatyPricingObjectBo> bos = new List<TreatyPricingObjectBo>() { };
            for (int i = 0; i < ids.Count; i++)
            {
                bos.Add(FormBo(ids[i], texts[i]));
            }
            return bos;
        }

        public static IList<TreatyPricingObjectBo> GetObjects(int moduleId)
        {
            using (var db = new AppDbContext())
            {
                switch (moduleId)
                {
                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingAdvantageProgram:
                        return FormBos(
                            db.TreatyPricingAdvantagePrograms.OrderBy(q => q.Id).Select(q => q.Id).ToList(),
                            db.TreatyPricingAdvantagePrograms.OrderBy(q => q.Id).Select(q => q.Code + " - " + q.Name).ToList());

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingCampaign:
                        return FormBos(
                            db.TreatyPricingCampaigns.OrderBy(q => q.Id).Select(q => q.Id).ToList(),
                            db.TreatyPricingCampaigns.OrderBy(q => q.Id).Select(q => q.Code + " - " + q.Name).ToList());

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingClaimApprovalLimit:
                        return FormBos(
                            db.TreatyPricingClaimApprovalLimits.OrderBy(q => q.Id).Select(q => q.Id).ToList(),
                            db.TreatyPricingClaimApprovalLimits.OrderBy(q => q.Id).Select(q => q.Code + " - " + q.Name).ToList());

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingCustomOther:
                        return FormBos(
                            db.TreatyPricingCustomOthers.OrderBy(q => q.Id).Select(q => q.Id).ToList(),
                            db.TreatyPricingCustomOthers.OrderBy(q => q.Id).Select(q => q.Code + " - " + q.Name).ToList());

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingDefinitionAndExclusion:
                        return FormBos(
                            db.TreatyPricingDefinitionAndExclusions.OrderBy(q => q.Id).Select(q => q.Id).ToList(),
                            db.TreatyPricingDefinitionAndExclusions.OrderBy(q => q.Id).Select(q => q.Code + " - " + q.Name).ToList());

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingFinancialTable:
                        return FormBos(
                            db.TreatyPricingFinancialTables.OrderBy(q => q.Id).Select(q => q.Id).ToList(),
                            db.TreatyPricingFinancialTables.OrderBy(q => q.Id).Select(q => q.FinancialTableId + " - " + q.Name).ToList());

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingMedicalTable:
                        return FormBos(
                            db.TreatyPricingMedicalTables.OrderBy(q => q.Id).Select(q => q.Id).ToList(),
                            db.TreatyPricingMedicalTables.OrderBy(q => q.Id).Select(q => q.MedicalTableId + " - " + q.Name).ToList());

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingProduct:
                        return FormBos(
                            db.TreatyPricingProducts.OrderBy(q => q.Id).Select(q => q.Id).ToList(),
                            db.TreatyPricingProducts.OrderBy(q => q.Id).Select(q => q.Code + " - " + q.Name).ToList());

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingProfitCommission:
                        return FormBos(
                            db.TreatyPricingProfitCommissions.OrderBy(q => q.Id).Select(q => q.Id).ToList(),
                            db.TreatyPricingProfitCommissions.OrderBy(q => q.Id).Select(q => q.Code + " - " + q.Name).ToList());

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingRateTable:
                        return FormBos(
                            db.TreatyPricingRateTables.OrderBy(q => q.Id).Select(q => q.Id).ToList(),
                            db.TreatyPricingRateTables.OrderBy(q => q.Id).Select(q => q.Code + " - " + q.Name).ToList());

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingUwLimit:
                        return FormBos(
                            db.TreatyPricingUwLimits.OrderBy(q => q.Id).Select(q => q.Id).ToList(),
                            db.TreatyPricingUwLimits.OrderBy(q => q.Id).Select(q => q.LimitId + " - " + q.Name).ToList());

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingUwQuestionnaire:
                        return FormBos(
                            db.TreatyPricingUwQuestionnaires.OrderBy(q => q.Id).Select(q => q.Id).ToList(),
                            db.TreatyPricingUwQuestionnaires.OrderBy(q => q.Id).Select(q => q.Code + " - " + q.Name).ToList());

                    default:
                        return FormBos(null);
                }
            }
        }
        
        public static void GetObjectDisplayInfo(int moduleId, int objectId, ref string moduleName, ref string moduleCode, ref string objectTextId, ref string objectName)
        {
            moduleName = TreatyPricingObjectBo.GetTreatyPricingObjectModule(moduleId);
            moduleCode = TreatyPricingObjectBo.GetTreatyPricingObjectModuleCode(moduleId);

            using (var db = new AppDbContext())
            {
                switch (moduleId)
                {
                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingAdvantageProgram:
                        objectTextId = db.TreatyPricingAdvantagePrograms.Where(q => q.Id == objectId).Select(q => q.Code.ToString()).FirstOrDefault();
                        objectName = db.TreatyPricingAdvantagePrograms.Where(q => q.Id == objectId).Select(q => q.Name.ToString()).FirstOrDefault();
                        break;

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingCampaign:
                        objectTextId = db.TreatyPricingCampaigns.Where(q => q.Id == objectId).Select(q => q.Code.ToString()).FirstOrDefault();
                        objectName = db.TreatyPricingCampaigns.Where(q => q.Id == objectId).Select(q => q.Name.ToString()).FirstOrDefault();
                        break;

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingClaimApprovalLimit:
                        objectTextId = db.TreatyPricingClaimApprovalLimits.Where(q => q.Id == objectId).Select(q => q.Code.ToString()).FirstOrDefault();
                        objectName = db.TreatyPricingClaimApprovalLimits.Where(q => q.Id == objectId).Select(q => q.Name.ToString()).FirstOrDefault();
                        break;

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingCustomOther:
                        objectTextId = db.TreatyPricingCustomOthers.Where(q => q.Id == objectId).Select(q => q.Code.ToString()).FirstOrDefault();
                        objectName = db.TreatyPricingCustomOthers.Where(q => q.Id == objectId).Select(q => q.Name.ToString()).FirstOrDefault();
                        break;

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingDefinitionAndExclusion:
                        objectTextId = db.TreatyPricingDefinitionAndExclusions.Where(q => q.Id == objectId).Select(q => q.Code.ToString()).FirstOrDefault();
                        objectName = db.TreatyPricingDefinitionAndExclusions.Where(q => q.Id == objectId).Select(q => q.Name.ToString()).FirstOrDefault();
                        break;

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingFinancialTable:
                        objectTextId = db.TreatyPricingFinancialTables.Where(q => q.Id == objectId).Select(q => q.FinancialTableId.ToString()).FirstOrDefault();
                        objectName = db.TreatyPricingFinancialTables.Where(q => q.Id == objectId).Select(q => q.Name.ToString()).FirstOrDefault();
                        break;

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingMedicalTable:
                        objectTextId = db.TreatyPricingMedicalTables.Where(q => q.Id == objectId).Select(q => q.MedicalTableId.ToString()).FirstOrDefault();
                        objectName = db.TreatyPricingMedicalTables.Where(q => q.Id == objectId).Select(q => q.Name.ToString()).FirstOrDefault();
                        break;

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingProduct:
                        objectTextId = db.TreatyPricingProducts.Where(q => q.Id == objectId).Select(q => q.Code.ToString()).FirstOrDefault();
                        objectName = db.TreatyPricingProducts.Where(q => q.Id == objectId).Select(q => q.Name.ToString()).FirstOrDefault();
                        break;

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingProfitCommission:
                        objectTextId = db.TreatyPricingProfitCommissions.Where(q => q.Id == objectId).Select(q => q.Code.ToString()).FirstOrDefault();
                        objectName = db.TreatyPricingProfitCommissions.Where(q => q.Id == objectId).Select(q => q.Name.ToString()).FirstOrDefault();
                        break;

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingRateTable:
                        objectTextId = db.TreatyPricingRateTables.Where(q => q.Id == objectId).Select(q => q.Code.ToString()).FirstOrDefault();
                        objectName = db.TreatyPricingRateTables.Where(q => q.Id == objectId).Select(q => q.Name.ToString()).FirstOrDefault();
                        break;

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingUwLimit:
                        objectTextId = db.TreatyPricingUwLimits.Where(q => q.Id == objectId).Select(q => q.LimitId.ToString()).FirstOrDefault();
                        objectName = db.TreatyPricingUwLimits.Where(q => q.Id == objectId).Select(q => q.Name.ToString()).FirstOrDefault();
                        break;

                    case TreatyPricingObjectBo.ObjectModuleTreatyPricingUwQuestionnaire:
                        objectTextId = db.TreatyPricingUwQuestionnaires.Where(q => q.Id == objectId).Select(q => q.Code.ToString()).FirstOrDefault();
                        objectName = db.TreatyPricingUwQuestionnaires.Where(q => q.Id == objectId).Select(q => q.Name.ToString()).FirstOrDefault();
                        break;

                    default:
                        objectTextId = "";
                        objectName = "";
                        break;
                }
            }
        }
    }
}
