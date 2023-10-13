using BusinessObject.TreatyPricing;
using Newtonsoft.Json;
using Services.TreatyPricing;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class TreatyPricingProductBenefitController : BaseController
    {
        public static List<string> Validate(string benefit)
        {
            List<string> errors = new List<string>();
            List<TreatyPricingProductBenefitBo> bos = JsonConvert.DeserializeObject<List<TreatyPricingProductBenefitBo>>(benefit);

            foreach (var benefitBo in bos)
            {
                var bo = benefitBo;
                var benefitCode = string.Format("{0} - {1}", bo.BenefitCode, bo.Name);
                foreach (string propertyName in TreatyPricingProductBenefitBo.GetRequiredProperties())
                {
                    var value = bo.GetPropertyValue(propertyName);
                    if (value == null || string.IsNullOrEmpty(value.ToString()))
                    {
                        string displayName = bo.GetAttributeFrom<DisplayNameAttribute>(propertyName).DisplayName;
                        errors.Add(string.Format("{0} is required for benefit {1}", displayName, benefitCode));
                    }
                }

                foreach (string propertyName in TreatyPricingProductBenefitBo.GetMaxLengthStringProperties())
                {
                    var value = bo.GetPropertyValue(propertyName);
                    if (value != null && !string.IsNullOrEmpty(value.ToString()))
                    {
                        string displayName = bo.GetAttributeFrom<DisplayNameAttribute>(propertyName).DisplayName;

                        var maxLengthAttr = bo.GetAttributeFrom<MaxLengthAttribute>(propertyName);
                        int length = value.ToString().Length;
                        if (maxLengthAttr != null && length > 0 && length > maxLengthAttr.Length)
                        {
                            errors.Add(string.Format(MessageBag.MaxLength + " for benefit {2}", displayName, maxLengthAttr.Length, benefitCode));
                        }
                    }
                }

                if (bos.Where(q => q.Code == benefitCode).Count() > 1)
                    errors.Add(string.Format("Duplicate benefit and marketing name combination found for benefit {0}", benefitCode));
            }

            errors = errors.Distinct().ToList();

            return errors;
        }

        public ActionResult DownloadPricingFile(int id)
        {
            var benefit = TreatyPricingProductBenefitService.Find(id);
            if (benefit == null)
                return null;

            string path = benefit.GetLocalPath();
            if (System.IO.File.Exists(path))
            {
                return File(
                    System.IO.File.ReadAllBytes(benefit.GetLocalPath()),
                    System.Net.Mime.MediaTypeNames.Application.Octet,
                    benefit.PricingUploadFileName
                );
            }
            return null;
        }

        public static void Save(int versionId, string benefit, int authUserId, ref TrailObject trail, bool resetId = false)
        {
            if (string.IsNullOrEmpty(benefit))
                return;

            List<int> ids = new List<int>();

            List<TreatyPricingProductBenefitBo> bos = JsonConvert.DeserializeObject<List<TreatyPricingProductBenefitBo>>(benefit);
            foreach (var benefitBo in bos)
            {
                var bo = benefitBo;
                if (resetId)
                    bo.Id = 0;

                bo.TreatyPricingProductVersionId = versionId;
                bo.UpdatedById = authUserId;

                if (bo.Id == 0)
                    bo.CreatedById = authUserId;

                bo.SetSelectValues();

                bool isFileUploaded = false;
                string oldFilePath = null;
                string oriFilePath = null;
                var dbBenefit = TreatyPricingProductBenefitService.Find(bo.Id);
                if (!string.IsNullOrEmpty(bo.PricingUploadFileName))
                {
                    if (bo.Id == 0)
                    {
                        isFileUploaded = true;
                        oriFilePath = bo.GetLocalPath();
                        if (System.IO.File.Exists(oriFilePath))
                        {
                            bo.PricingUploadHashFileName = Hash.HashFileName(bo.PricingUploadFileName);
                        }
                        else
                            oriFilePath = null;
                    }
                    else
                    {
                        if (dbBenefit.PricingUploadHashFileName != bo.PricingUploadHashFileName)
                        {
                            isFileUploaded = true;
                            oldFilePath = dbBenefit.GetLocalPath();
                        }
                    }
                }
                else if (dbBenefit != null && !string.IsNullOrEmpty(dbBenefit.PricingUploadHashFileName))
                {
                    oldFilePath = dbBenefit.GetLocalPath();
                }

                Result result = TreatyPricingProductBenefitService.Save(ref bo, ref trail);
                if (result.Valid)
                {
                    ids.Add(bo.Id);

                    if (isFileUploaded)
                    {
                        string path = bo.GetLocalPath();
                        if (!string.IsNullOrEmpty(oriFilePath) && System.IO.File.Exists(oriFilePath)) 
                        {
                            Util.MakeDir(path);
                            System.IO.File.Copy(oriFilePath, path);
                        }
                        else
                        {
                            string tempPath = bo.GetTempPath("Uploads");
                            if (System.IO.File.Exists(tempPath))
                            {
                                Util.MakeDir(path);
                                System.IO.File.Move(tempPath, path);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(oldFilePath) && System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }

                    List<int> childIds = new List<int>();
                    foreach (var childBo in bo.TreatyPricingProductBenefitDirectRetroBos)
                    {
                        TreatyPricingProductBenefitDirectRetroBo directRetroBo = childBo;
                        if (resetId)
                            directRetroBo.Id = 0;

                        directRetroBo.TreatyPricingProductBenefitId = bo.Id;
                        directRetroBo.UpdatedById = authUserId;

                        if (directRetroBo.Id == 0)
                            directRetroBo.CreatedById = authUserId;

                        TreatyPricingProductBenefitDirectRetroService.Save(ref directRetroBo, ref trail);

                        childIds.Add(directRetroBo.Id);
                    }
                    TreatyPricingProductBenefitDirectRetroService.DeleteByBenefitExcept(benefitBo.Id, ref trail, childIds);

                }
            }
            TreatyPricingProductBenefitService.DeleteByVersionExcept(versionId, ids, ref trail);
        }
    }
}