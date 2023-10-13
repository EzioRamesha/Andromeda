using BusinessObject;
using BusinessObject.Claims;
using DataAccess.Entities;
using DataAccess.Entities.Claims;
using Services;
using Services.Claims;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class ClaimDataConfigViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Ceding Company")]
        public int CedantId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Treaty")]
        public int? TreatyId { get; set; }

        [Display(Name = "Status")]
        public int Status { get; set; }

        [Required]
        [StringLength(64)]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The {0} field is required.")]
        [Display(Name = "File Type")]
        public int FileType { get; set; }

        [Display(Name = "Has Header")]
        public bool HasHeader { get; set; }

        [Display(Name = "Header Row")]
        public int? HeaderRow { get; set; }

        [Display(Name = "Worksheet")]
        public string Worksheet { get; set; }

        [RequiredIfPlainText]
        [Display(Name = "Delimiter")]
        public int? Delimiter { get; set; }

        [Display(Name = "Remove Quote")]
        public bool RemoveQuote { get; set; }

        [Display(Name = "Start Row")]
        public int? StartRow { get; set; }

        [Display(Name = "End Row")]
        public int? EndRow { get; set; }

        [Display(Name = "Start Column")]
        public int? StartColumn { get; set; }

        [Display(Name = "End Column")]
        public int? EndColumn { get; set; }

        [Required]
        [Display(Name = "Column to Row Mapping")]
        public bool IsColumnToRowMapping { get; set; }

        [Display(Name = "Number of Row Mapping")]
        public int? NumberOfRowMapping { get; set; }

        [Required]
        [Display(Name = "Data Correction")]
        public bool IsDataCorrection { get; set; }

        public virtual Cedant Cedant { get; set; }

        public virtual CedantBo CedantBo { get; set; }

        public virtual Treaty Treaty { get; set; }

        public virtual TreatyBo TreatyBo { get; set; }

        public virtual string CedantName { get; set; }

        public virtual string TreatyIdCode { get; set; }

        public virtual string FileTypeName { get; set; }

        public virtual string DelimiterName { get; set; }

        public int ModuleId { get; set; }

        protected FormCollection Form { get; set; }

        private IList<ClaimDataMappingBo> ClaimDataMappingBos { get; set; }
        private IList<ClaimDataComputationBo> ClaimDataComputationBos { get; set; }
        private IList<ClaimDataValidationBo> ClaimDataValidationBos { get; set; }

        private readonly List<Type> ChildModules = new List<Type>
        {
            typeof(ClaimDataMappingBo),
            typeof(ClaimDataComputationBo),
            typeof(ClaimDataValidationBo),
        };

        public ClaimDataConfigViewModel()
        {
            Set();
        }

        public ClaimDataConfigViewModel(ClaimDataConfigBo claimDataConfigBo)
        {
            Set(claimDataConfigBo);
        }

        public void Set(ClaimDataConfigBo claimDataConfigBo = null)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.ClaimDataConfig.ToString());
            ModuleId = moduleBo.Id;
            if (claimDataConfigBo != null)
            {
                Id = claimDataConfigBo.Id;
                CedantId = claimDataConfigBo.CedantBo != null ? claimDataConfigBo.CedantBo.Id : claimDataConfigBo.CedantId;
                TreatyId = claimDataConfigBo.TreatyBo != null ? claimDataConfigBo.TreatyBo.Id : claimDataConfigBo.TreatyId;
                Status = claimDataConfigBo.Status;
                Code = claimDataConfigBo.Code;
                Name = claimDataConfigBo.Name;
                FileType = claimDataConfigBo.FileType;
                FileTypeName = claimDataConfigBo.FileTypeName;
                HasHeader = claimDataConfigBo.ClaimDataFileConfig.HasHeader;
                HeaderRow = claimDataConfigBo.ClaimDataFileConfig.HeaderRow;
                Delimiter = claimDataConfigBo.ClaimDataFileConfig.Delimiter;
                DelimiterName = claimDataConfigBo.ClaimDataFileConfig.DelimiterName;
                Worksheet = claimDataConfigBo.ClaimDataFileConfig.Worksheet;
                RemoveQuote = claimDataConfigBo.ClaimDataFileConfig.RemoveQuote;
                StartRow = claimDataConfigBo.ClaimDataFileConfig.StartRow;
                EndRow = claimDataConfigBo.ClaimDataFileConfig.EndRow;
                StartColumn = claimDataConfigBo.ClaimDataFileConfig.StartColumn;
                EndColumn = claimDataConfigBo.ClaimDataFileConfig.EndColumn;
                IsColumnToRowMapping = claimDataConfigBo.ClaimDataFileConfig.IsColumnToRowMapping;
                NumberOfRowMapping = claimDataConfigBo.ClaimDataFileConfig.NumberOfRowMapping;
                IsDataCorrection = claimDataConfigBo.ClaimDataFileConfig.IsDataCorrection;
                CedantName = claimDataConfigBo.CedantBo?.Name;
                TreatyIdCode = claimDataConfigBo.TreatyBo?.TreatyIdCode;
            }
        }

        public void Get(ref ClaimDataConfigBo claimDataConfigBo)
        {
            claimDataConfigBo.Status = Status;
            claimDataConfigBo.CedantId = CedantId;
            claimDataConfigBo.TreatyId = TreatyId == 0 ? null : TreatyId;
            claimDataConfigBo.Code = Code?.Trim();
            claimDataConfigBo.Name = Name?.Trim();
            claimDataConfigBo.FileType = FileType;
            claimDataConfigBo.ClaimDataFileConfig.HasHeader = HasHeader;
            claimDataConfigBo.ClaimDataFileConfig.HeaderRow = HeaderRow;
            claimDataConfigBo.ClaimDataFileConfig.Worksheet = FileType == ClaimDataConfigBo.FileTypeExcel ? Worksheet : null;
            claimDataConfigBo.ClaimDataFileConfig.Delimiter = FileType == ClaimDataConfigBo.FileTypePlainText ? Delimiter : null;
            claimDataConfigBo.ClaimDataFileConfig.RemoveQuote = RemoveQuote;
            claimDataConfigBo.ClaimDataFileConfig.StartRow = StartRow;
            claimDataConfigBo.ClaimDataFileConfig.EndRow = EndRow;
            claimDataConfigBo.ClaimDataFileConfig.StartColumn = StartColumn;
            claimDataConfigBo.ClaimDataFileConfig.EndColumn = EndColumn;
            claimDataConfigBo.ClaimDataFileConfig.IsColumnToRowMapping = IsColumnToRowMapping;
            claimDataConfigBo.ClaimDataFileConfig.NumberOfRowMapping = NumberOfRowMapping;
            claimDataConfigBo.ClaimDataFileConfig.IsDataCorrection = IsDataCorrection;

            // Child Items
            claimDataConfigBo.ClaimDataMappingBos = ClaimDataMappingBos;
            claimDataConfigBo.ClaimDataComputationBos = ClaimDataComputationBos;
            claimDataConfigBo.ClaimDataValidationBos = ClaimDataValidationBos;
        }

        public class RequiredIfPlainText : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var fileType = validationContext.ObjectType.GetProperty("FileType");
                var fileTypeValue = fileType.GetValue(validationContext.ObjectInstance, null);

                int? delimiter = value as int?;
                if ((int)fileTypeValue == ClaimDataConfigBo.FileTypePlainText && (delimiter == null || delimiter == 0))
                {
                    return new ValidationResult("The Delimiter field is Required");
                }
                return null;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (HasHeader && HeaderRow != null && StartRow != null)
            {
                if (HeaderRow >= StartRow)
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.GreaterThan, "Start Row", "Header Row"),
                    new[] { nameof(StartRow) }));
                }
            }

            if (StartRow != null && EndRow != null)
            {
                if (StartRow > EndRow)
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.GreaterThan, "End Row", "Start Row"),
                    new[] { nameof(EndRow) }));
                }
            }

            if (StartColumn != null && EndColumn != null)
            {
                if (StartColumn > EndColumn)
                {
                    results.Add(new ValidationResult(
                    string.Format(MessageBag.GreaterThan, "End Column", "Start Column"),
                    new[] { nameof(EndColumn) }));
                }
            }

            return results;
        }

        public static Expression<Func<ClaimDataConfig, ClaimDataConfigViewModel>> Expression()
        {
            return entity => new ClaimDataConfigViewModel
            {
                Id = entity.Id,
                Status = entity.Status,
                Code = entity.Code,
                Name = entity.Name,
                FileType = entity.FileType,

                CedantId = entity.CedantId,
                Cedant = entity.Cedant,

                TreatyId = entity.TreatyId,
                Treaty = entity.Treaty,
            };
        }

        public void SetChildItems(ClaimDataConfigBo claimDataConfigBo = null)
        {
            foreach (Type objectType in ChildModules)
            {
                string objectTypeName = objectType.Name.Replace("Bo", "");
                string objectListName = string.Format("{0}Bos", objectTypeName);
                IList bos;

                if (claimDataConfigBo == null)
                {
                    if (objectType == typeof(ClaimDataComputationBo))
                    {
                        List<ClaimDataComputationBo> claimDataComputationBos = new List<ClaimDataComputationBo>();
                        for (int step = ClaimDataComputationBo.StepPreComputation; step <= ClaimDataComputationBo.MaxStep; step++)
                        {
                            string objectStepTypeName = string.Format("{0}{1}", objectTypeName, step);
                            claimDataComputationBos.AddRange((List<ClaimDataComputationBo>)GetChildItem(objectType, objectStepTypeName));
                        }
                        bos = claimDataComputationBos;
                    }
                    else if (objectType == typeof(ClaimDataValidationBo))
                    {
                        List<ClaimDataValidationBo> claimDataValidationBos = new List<ClaimDataValidationBo>();
                        for (int step = ClaimDataValidationBo.StepPreValidation; step <= ClaimDataValidationBo.MaxStep; step++)
                        {
                            string objectStepTypeName = string.Format("{0}{1}", objectTypeName, step);
                            claimDataValidationBos.AddRange((List<ClaimDataValidationBo>)GetChildItem(objectType, objectStepTypeName));
                        }
                        bos = claimDataValidationBos;
                    }
                    else
                    {
                        bos = GetChildItem(objectType, objectTypeName);
                        this.SetPropertyValue(string.Format("{0}Bos", objectTypeName), bos, BindingFlags.NonPublic | BindingFlags.Instance);
                    }
                }
                else
                {
                    bos = GetChildItem(objectType, claimDataConfigBo);
                }

                this.SetPropertyValue(objectListName, bos, BindingFlags.NonPublic | BindingFlags.Instance);
            }
        }

        // For Import only
        public IList GetChildItem(Type objectType, object parentBo)
        {
            string objectTypeName = objectType.Name.Replace("Bo", "");
            Type genericListType = typeof(List<>);
            Type listBoType = genericListType.MakeGenericType(objectType);
            IList bos = (IList)Activator.CreateInstance(listBoType);

            MethodInfo isDefaultValuePickList = objectType.GetMethod("IsDefaultValuePickList", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo isDefaultValueBenefit = objectType.GetMethod("IsDefaultValueBenefit", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo isDefaultValueTreatyCode = objectType.GetMethod("IsDefaultValueTreatyCode", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo getDefaultValueType = objectType.GetMethod("GetDefaultValueType", BindingFlags.Instance | BindingFlags.Public);

            IList objectBos = (IList)parentBo.GetPropertyValue(string.Format("{0}Bos", objectTypeName));
            if (objectBos == null)
            {
                throw new Exception("The configuration file has missing objects");
            }

            foreach (object objectBo in objectBos)
            {
                object bo = objectBo;
                
                if (bo.HasProperty("StandardClaimDataOutputId") && bo.GetPropertyValue("StandardClaimDataOutputCode") != null)
                {
                    string standardOutputCode = (string)bo.GetPropertyValue("StandardClaimDataOutputCode");
                    int standardOutputId = StandardClaimDataOutputBo.GetTypeByCode(standardOutputCode);
                    StandardClaimDataOutputBo standardOutputBo = StandardClaimDataOutputService.Find(standardOutputId);
                    if (standardOutputBo != null)
                    {
                        bo.SetPropertyValue("StandardClaimDataOutputId", standardOutputBo.Id);
                        bo.SetPropertyValue("StandardClaimDataOutputBo", standardOutputBo);
                    }
                }

                if (objectType == typeof(ClaimDataMappingDetailBo) && bo.GetPropertyValue("PickListDetailCode") != null)
                {
                    string pickListDetailCode = (string)bo.GetPropertyValue("PickListDetailCode");
                    int standardOutputId = (int)parentBo.GetPropertyValue("StandardClaimDataOutputId");

                    PickListDetailBo pickListDetailBo = PickListDetailService.FindByStandardClaimDataOutputIdCode(standardOutputId, pickListDetailCode);
                    if (pickListDetailBo != null)
                    {
                        bo.SetPropertyValue("PickListDetailId", pickListDetailBo.Id);
                        bo.SetPropertyValue("PickListDetailBo", pickListDetailBo);
                    }
                }

                if (objectType == typeof(ClaimDataMappingBo))
                {
                    int transformFormula = (int)bo.GetPropertyValue("TransformFormula");
                    getDefaultValueType.Invoke(bo, null);

                    if (transformFormula == ClaimDataMappingBo.TransformFormulaFixedValue)
                    {
                        if ((bool)isDefaultValuePickList.Invoke(bo, null))
                        {
                            int standardOutputId = (int)bo.GetPropertyValue("StandardClaimDataOutputId");
                            PickListDetailBo pickListDetailBo = PickListDetailService.FindByStandardClaimDataOutputIdCode(standardOutputId, (string)bo.GetPropertyValue("DefaultValue"));
                            bo.SetPropertyValue("DefaultObjectId", pickListDetailBo?.Id);
                        }
                        else if ((bool)isDefaultValueBenefit.Invoke(bo, null))
                        {
                            BenefitBo benefitBo = BenefitService.FindByCode((string)bo.GetPropertyValue("DefaultValue"));
                            bo.SetPropertyValue("DefaultObjectId", benefitBo?.Id);
                        }
                        else if ((bool)isDefaultValueTreatyCode.Invoke(bo, null))
                        {
                            TreatyCodeBo treatyCodeBo = TreatyCodeService.FindByCode((string)bo.GetPropertyValue("DefaultValue"));
                            bo.SetPropertyValue("DefaultObjectId", treatyCodeBo?.Id);
                        }
                    }
                    else if (transformFormula == ClaimDataMappingBo.TransformFormulaInputTable)
                    {
                        bo.SetPropertyValue("ClaimDataMappingDetailBos", GetChildItem(typeof(ClaimDataMappingDetailBo), bo));
                    }
                }

                bos.Add(bo);
            }

            return bos;
        }

        public IList GetChildItem(Type objectType, string objectTypeName, int? parentIndex = null)
        {
            Type genericListType = typeof(List<>);
            Type listBoType = genericListType.MakeGenericType(objectType);
            IList bos = (IList)Activator.CreateInstance(listBoType);

            int maxIndex = 0;
            if (parentIndex == null)
            {
                maxIndex = int.Parse(Form.Get(string.Format("{0}.MaxIndex", objectTypeName)));
            }
            else
            {
                maxIndex = int.Parse(Form.Get(string.Format("{0}.MaxIndex[{1}]", objectTypeName, parentIndex)));
            }
            int index = 0;

            MethodInfo isEmpty = objectType.GetMethod("IsEmpty", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo isDefaultValuePickList = objectType.GetMethod("IsDefaultValuePickList", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo isDefaultValueBenefit = objectType.GetMethod("IsDefaultValueBenefit", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo isDefaultValueTreatyCode = objectType.GetMethod("IsDefaultValueTreatyCode", BindingFlags.Instance | BindingFlags.Public);
            MethodInfo getDefaultValueType = objectType.GetMethod("GetDefaultValueType", BindingFlags.Instance | BindingFlags.Public);

            while (index <= maxIndex)
            {
                object bo = GetFormVariables(objectType, index, parentIndex, objectTypeName);

                if (isEmpty != null && (bool)isEmpty.Invoke(bo, null))
                {
                    index++;
                    continue;
                }

                if (objectType == typeof(ClaimDataMappingBo))
                {
                    getDefaultValueType.Invoke(bo, null);
                    if (bo.GetPropertyValue("DefaultObjectId") != null)
                    {
                        if ((bool)isDefaultValuePickList.Invoke(bo, null))
                        {
                            PickListDetailBo pickListDetailBo = PickListDetailService.Find((int)bo.GetPropertyValue("DefaultObjectId"));
                            bo.SetPropertyValue("DefaultValue", pickListDetailBo?.Code);
                        }
                        else if ((bool)isDefaultValueBenefit.Invoke(bo, null))
                        {
                            BenefitBo benefitBo = BenefitService.Find((int)bo.GetPropertyValue("DefaultObjectId"));
                            bo.SetPropertyValue("DefaultValue", benefitBo?.Code);
                        }
                        else if ((bool)isDefaultValueTreatyCode.Invoke(bo, null))
                        {
                            TreatyCodeBo treatyCodeBo = TreatyCodeService.Find((int)bo.GetPropertyValue("DefaultObjectId"));
                            bo.SetPropertyValue("DefaultValue", treatyCodeBo?.Code);
                        }
                    }

                    if ((int)bo.GetPropertyValue("TransformFormula") == ClaimDataMappingBo.TransformFormulaInputTable)
                    {
                        bo.SetPropertyValue("ClaimDataMappingDetailBos", GetChildItem(typeof(ClaimDataMappingDetailBo), "ClaimDataMappingDetail", index));
                    }
                }

                bos.Add(bo);
                index++;
            }
            return bos;
        }

        public object GetFormVariables(Type objectType, int index, int? parentIndex = null, string objectTypeName = null)
        {
            object bo = Activator.CreateInstance(objectType);
            if (string.IsNullOrEmpty(objectTypeName))
                objectTypeName = objectType.Name.Replace("Bo", "");

            foreach (var property in objectType.GetProperties())
            {
                string propertyName = property.Name;
                string formValue;

                switch (propertyName)
                {
                    case "ClaimDataConfigId":
                        bo.SetPropertyValue(propertyName, Id);
                        continue;
                    //case "SortIndex":
                    //    bo.SetPropertyValue(propertyName, index + 1);
                    //    continue;
                    case "Row":
                        if (!IsColumnToRowMapping)
                        {
                            bo.SetPropertyValue(propertyName, 0);
                            continue;
                        }
                        break;
                }

                if (parentIndex != null)
                {
                    formValue = Form.Get(string.Format("{0}.{1}[{2}][{3}]", objectTypeName, propertyName, parentIndex.Value, index));
                }
                else
                {
                    formValue = Form.Get(string.Format("{0}.{1}[{2}]", objectTypeName, propertyName, index));
                }

                if (string.IsNullOrEmpty(formValue))
                {
                    if (property.PropertyType == typeof(int))
                        bo.SetPropertyValue(propertyName, 0);

                    continue;
                }

                object value = formValue;
                if (property.PropertyType == typeof(int))
                {
                    value = int.Parse(formValue);
                }
                else if (property.PropertyType == typeof(int?))
                {
                    value = int.TryParse(formValue, out int formattedValue) ? (int?)formattedValue : null;
                }
                else if (property.PropertyType == typeof(bool))
                {
                    value = bool.Parse(formValue);
                }

                bo.SetPropertyValue(propertyName, value);
            }

            SetObjects(ref bo);
            return bo;
        }

        public void SetObjects(ref object bo)
        {
            if (bo.GetPropertyValue("StandardClaimDataOutputId") != null)
            {
                StandardClaimDataOutputBo standardOutputBo = StandardClaimDataOutputService.Find((int?)bo.GetPropertyValue("StandardClaimDataOutputId"));
                if (standardOutputBo != null)
                {
                    bo.SetPropertyValue("StandardClaimDataOutputBo", standardOutputBo);
                    bo.SetPropertyValue("StandardClaimDataOutputCode", standardOutputBo.Code);
                }
            }

            if (bo.GetPropertyValue("PickListDetailId") != null)
            {
                PickListDetailBo pickListDetailBo = PickListDetailService.Find((int?)bo.GetPropertyValue("PickListDetailId"));
                if (pickListDetailBo != null)
                {
                    bo.SetPropertyValue("PickListDetailBo", pickListDetailBo);
                    bo.SetPropertyValue("PickListDetailCode", pickListDetailBo.Code);
                }
            }
        }

        public Result ValidateChildItems(ClaimDataFileConfig claimDataFileConfig, bool validateFormula = false)
        {
            Result result = new Result();
            foreach (Type objectType in ChildModules)
            {
                string objectTypeName = objectType.Name.Replace("Bo", "");
                IList bos = (IList)GetChildBos(objectTypeName);

                if (bos == null)
                    continue;

                MethodInfo validate = objectType.GetMethod("Validate", BindingFlags.Instance | BindingFlags.Public);

                MethodInfo getStepName = objectType.GetMethod("GetStepName", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static);
                string displayName = (string)objectType.GetField("DisplayName").GetValue(null);

                bool valCondition = (objectType != typeof(ClaimDataMappingBo)) ? validateFormula : false;
                bool valFormula = (objectType == typeof(ClaimDataComputationBo)) ? validateFormula : false;

                Dictionary<string, int> moduleIndex = new Dictionary<string, int>();
                foreach (object bo in bos)
                {
                    List<string> errors = (List<string>)validate.Invoke(bo, new object[] { claimDataFileConfig });

                    if (valCondition)
                    {
                        int step = (int)bo.GetPropertyValue("Step");
                        bool enableRiData = false;
                        if (objectType == typeof(ClaimDataComputationBo) && step == ClaimDataComputationBo.StepPostComputation)
                        {
                            enableRiData = true;
                        }
                        else if (objectType == typeof(ClaimDataValidationBo) && step == ClaimDataValidationBo.StepPostValidation)
                        {
                            enableRiData = true;
                        }

                        var soe = new StandardClaimDataOutputEval
                        {
                            Condition = (string)bo.GetPropertyValue("Condition"),
                            Validate = true,
                            EnableRiData = enableRiData,
                        };

                        soe.EvalCondition();
                        if (valFormula)
                        {
                            soe.Formula = (string)bo.GetPropertyValue("CalculationFormula");
                            soe.EvalFormula();
                        }

                        errors.AddRange(soe.Errors);
                    }

                    string moduleName = displayName;
                    if (getStepName != null)
                    {
                        int key = (int)bo.GetPropertyValue("Step");
                        moduleName = (string)getStepName.Invoke(objectType, new object[] { key });
                    }

                    if (!moduleIndex.ContainsKey(moduleName))
                    {
                        moduleIndex[moduleName] = 0;
                    }
                    moduleIndex[moduleName]++;

                    foreach (string error in errors)
                    {
                        result.AddError(string.Format(MessageBag.ForModuleAtNo, error, moduleName, moduleIndex[moduleName]));
                    }
                }
            }
            return result;
        }

        public void SaveChildItems(int authUserId, ref TrailObject trail)
        {
            Type riDataMappingType = typeof(ClaimDataMappingService);
            Assembly service = riDataMappingType.Assembly;

            foreach (Type objectType in ChildModules)
            {
                string objectTypeName = objectType.Name.Replace("Bo", "");
                string temp = string.Format("Services.Claims.{0}Service", objectTypeName);
                Type objectServiceType = service.GetType(temp);

                IList bos = (IList)GetChildBos(objectTypeName);
                List<int> savedIds = new List<int> { };

                if (bos == null)
                    continue;

                MethodInfo save = objectServiceType.GetMethod("Save", new Type[] { objectType.MakeByRefType(), typeof(TrailObject).MakeByRefType() });
                MethodInfo delete = objectServiceType.GetMethod("DeleteByDataConfigIdExcept", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static);

                foreach (object bo in bos)
                {
                    bo.SetPropertyValue("ClaimDataConfigId", Id);
                    bo.SetPropertyValue("CreatedById", authUserId);
                    bo.SetPropertyValue("UpdatedById", authUserId);
                    save.Invoke(objectServiceType, new object[] { bo, trail });
                    savedIds.Add((int)bo.GetPropertyValue("Id"));

                    if (objectType == typeof(ClaimDataMappingBo) && (int)bo.GetPropertyValue("TransformFormula") == ClaimDataMappingBo.TransformFormulaInputTable)
                    {
                        int parentId = (int)bo.GetPropertyValue("Id");
                        List<int> savedChildIds = new List<int> { };
                        foreach (ClaimDataMappingDetailBo childBo in (List<ClaimDataMappingDetailBo>)bo.GetPropertyValue("ClaimDataMappingDetailBos"))
                        {
                            ClaimDataMappingDetailBo claimDataMappingDetailBo = childBo;
                            claimDataMappingDetailBo.ClaimDataMappingId = parentId;
                            claimDataMappingDetailBo.CreatedById = authUserId;
                            claimDataMappingDetailBo.UpdatedById = authUserId;
                            ClaimDataMappingDetailService.Save(ref claimDataMappingDetailBo, ref trail);
                            savedChildIds.Add(claimDataMappingDetailBo.Id);
                        }
                        ClaimDataMappingDetailService.DeleteByDataMappingIdExcept(parentId, savedChildIds, ref trail);
                    }
                }
                delete.Invoke(objectServiceType, new object[] { Id, savedIds, trail });
            }
        }

        public void ProcessStatusHistory(int authUserId, ref TrailObject trail)
        {
            bool isChangeStatus = false;
            bool isRequiredUpdate = false;
            StatusHistoryBo latestStatusHistoryBo = StatusHistoryService.FindLatestByModuleIdObjectId(ModuleId, Id);

            if (latestStatusHistoryBo == null || latestStatusHistoryBo.Status != Status)
            {
                isChangeStatus = true;
            }
            else if (latestStatusHistoryBo != null || latestStatusHistoryBo.Status == Status)
            {
                if (latestStatusHistoryBo.CreatedById != authUserId)
                    isChangeStatus = true;
                else
                    isRequiredUpdate = true;
            }

            if (isChangeStatus)
            {
                StatusHistoryBo statusHistoryBo = new StatusHistoryBo
                {
                    ModuleId = ModuleId,
                    ObjectId = Id,
                    Status = Status,
                    CreatedById = authUserId,
                    UpdatedById = authUserId,
                };
                StatusHistoryService.Save(ref statusHistoryBo, ref trail);

                string statusRemark = Form.Get(string.Format("StatusRemark"));
                if (!string.IsNullOrWhiteSpace(statusRemark))
                {
                    RemarkBo remarkBo = new RemarkBo
                    {
                        ModuleId = ModuleId,
                        ObjectId = Id,
                        Status = Status,
                        Content = statusRemark,
                        CreatedById = authUserId,
                        UpdatedById = authUserId,
                    };
                    RemarkService.Save(ref remarkBo, ref trail);
                }
            }

            // Only update datetime
            if (isRequiredUpdate)
            {
                StatusHistoryService.Update(ref latestStatusHistoryBo, ref trail);
            }
        }

        public FormCollection CurrentForm
        {
            get
            {
                return Form;
            }
            set
            {
                Form = value;
            }
        }
        
        public object GetChildBos(string name)
        {
            return this.GetPropertyValue(string.Format("{0}Bos", name), BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public IList<ClaimDataComputationBo> GetComputationBosWithStep(int step)
        {
            return ClaimDataComputationBos.Where(q => q.Step == step).ToList();
        }

        public IList<ClaimDataValidationBo> GetValidationBosWithStep(int step)
        {
            return ClaimDataValidationBos.Where(q => q.Step == step).ToList();
        }
    }
}