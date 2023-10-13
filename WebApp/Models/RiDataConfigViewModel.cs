using BusinessObject;
using BusinessObject.RiDatas;
using DataAccess.Entities;
using DataAccess.Entities.RiDatas;
using Services;
using Services.RiDatas;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class RiDataConfigViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Ceding Company")]
        public int CedantId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Treaty ID")]
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

        private FormCollection Form { get; set; }

        private IList<RiDataMappingBo> RiDataMappingBos { get; set; }

        private IList<RiDataComputationBo> RiDataComputationBos { get; set; }

        private IList<RiDataPreValidationBo> RiDataPreValidationBos { get; set; }

        private List<Type> ChildModules = new List<Type>
        {
            typeof(RiDataMappingBo),
            typeof(RiDataComputationBo),
            typeof(RiDataPreValidationBo),
        };

        public RiDataConfigViewModel()
        {
            Set();
        }

        public RiDataConfigViewModel(RiDataConfigBo riDataConfigBo)
        {
            Set(riDataConfigBo);
        }

        public void Set(RiDataConfigBo riDataConfigBo = null)
        {
            ModuleBo moduleBo = ModuleService.FindByController(ModuleBo.ModuleController.RiDataConfig.ToString());
            if (riDataConfigBo != null)
            {
                Id = riDataConfigBo.Id;
                CedantId = riDataConfigBo.CedantId;
                TreatyId = riDataConfigBo.TreatyId;
                Status = riDataConfigBo.Status;
                Code = riDataConfigBo.Code;
                Name = riDataConfigBo.Name;
                FileType = riDataConfigBo.FileType;
                FileTypeName = riDataConfigBo.FileTypeName;
                HasHeader = riDataConfigBo.RiDataFileConfig.HasHeader;
                HeaderRow = riDataConfigBo.RiDataFileConfig.HeaderRow;
                Delimiter = riDataConfigBo.RiDataFileConfig.Delimiter;
                DelimiterName = riDataConfigBo.RiDataFileConfig.DelimiterName;
                Worksheet = riDataConfigBo.RiDataFileConfig.Worksheet;
                RemoveQuote = riDataConfigBo.RiDataFileConfig.RemoveQuote;
                StartRow = riDataConfigBo.RiDataFileConfig.StartRow;
                EndRow = riDataConfigBo.RiDataFileConfig.EndRow;
                StartColumn = riDataConfigBo.RiDataFileConfig.StartColumn;
                EndColumn = riDataConfigBo.RiDataFileConfig.EndColumn;
                IsColumnToRowMapping = riDataConfigBo.RiDataFileConfig.IsColumnToRowMapping;
                NumberOfRowMapping = riDataConfigBo.RiDataFileConfig.NumberOfRowMapping;
                IsDataCorrection = riDataConfigBo.RiDataFileConfig.IsDataCorrection;
                CedantName = riDataConfigBo.CedantBo.Name;
                TreatyIdCode = riDataConfigBo.TreatyId != null ? riDataConfigBo.TreatyBo.TreatyIdCode : null;
                ModuleId = moduleBo.Id;
            }
            else
            {
                ModuleId = moduleBo.Id;
            }
        }

        public class RequiredIfPlainText : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var fileType = validationContext.ObjectType.GetProperty("FileType");
                var fileTypeValue = fileType.GetValue(validationContext.ObjectInstance, null);

                int? delimiter = value as int?;
                if ((int)fileTypeValue == RiDataConfigBo.FileTypePlainText && (delimiter == null || delimiter == 0))
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

        public static Expression<Func<RiDataConfig, RiDataConfigViewModel>> Expression()
        {
            return entity => new RiDataConfigViewModel
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

        public void SetItems()
        {
            foreach (Type objectType in ChildModules)
            {
                string objectTypeName = objectType.Name.Replace("Bo", "");

                MethodInfo isEmpty = objectType.GetMethod("IsEmpty", BindingFlags.Instance | BindingFlags.Public);
                MethodInfo isDefaultValuePickList = objectType.GetMethod("IsDefaultValuePickList", BindingFlags.Instance | BindingFlags.Public);
                MethodInfo isDefaultValueBenefit = objectType.GetMethod("IsDefaultValueBenefit", BindingFlags.Instance | BindingFlags.Public);
                MethodInfo isDefaultValueTreatyCode = objectType.GetMethod("IsDefaultValueTreatyCode", BindingFlags.Instance | BindingFlags.Public);
                MethodInfo getDefaultValueType = objectType.GetMethod("GetDefaultValueType", BindingFlags.Instance | BindingFlags.Public);

                if (objectType == typeof(RiDataComputationBo))
                {
                    List<RiDataComputationBo> riDataComputationBos = new List<RiDataComputationBo>();
                    for (int step = RiDataComputationBo.StepPreComputation1; step <= RiDataComputationBo.MaxStep; step++)
                    {
                        string objectStepTypeName = string.Format("{0}{1}", objectTypeName, step);
                        riDataComputationBos.AddRange((List<RiDataComputationBo>)GetItem(objectType, objectStepTypeName, isEmpty, isDefaultValuePickList, isDefaultValueBenefit, isDefaultValueTreatyCode, getDefaultValueType));
                    }
                    RiDataComputationBos = riDataComputationBos;
                }
                else if (objectType == typeof(RiDataPreValidationBo))
                {
                    List<RiDataPreValidationBo> riDataPreValidationBos = new List<RiDataPreValidationBo>();
                    for (int step = RiDataPreValidationBo.StepPreValidation; step <= RiDataPreValidationBo.MaxStep; step++)
                    {
                        string objectStepTypeName = string.Format("{0}{1}", objectTypeName, step);
                        riDataPreValidationBos.AddRange((List<RiDataPreValidationBo>)GetItem(objectType, objectStepTypeName, isEmpty, isDefaultValuePickList, isDefaultValueBenefit, isDefaultValueTreatyCode, getDefaultValueType));
                    }
                    RiDataPreValidationBos = riDataPreValidationBos;
                }
                else
                {
                    IList bos = GetItem(objectType, objectTypeName, isEmpty, isDefaultValuePickList, isDefaultValueBenefit, isDefaultValueTreatyCode, getDefaultValueType);
                    this.SetPropertyValue(string.Format("{0}Bos", objectTypeName), bos, BindingFlags.NonPublic | BindingFlags.Instance);
                }
            }
        }

        public IList GetItem(Type objectType, string objectTypeName, MethodInfo isEmpty, MethodInfo isDefaultValuePickList, MethodInfo isDefaultValueBenefit, MethodInfo isDefaultValueTreatyCode, MethodInfo getDefaultValueType)
        {
            Type genericListType = typeof(List<>);
            Type listBoType = genericListType.MakeGenericType(objectType);
            IList bos = (IList)Activator.CreateInstance(listBoType);

            int maxIndex = int.Parse(Form.Get(string.Format("{0}.MaxIndex", objectTypeName)));
            int index = 0;

            while (index <= maxIndex)
            {
                object bo = GetFormVariables(objectType, index, objectTypeName: objectTypeName);

                if (isEmpty != null && (bool)isEmpty.Invoke(bo, null))
                {
                    index++;
                    continue;
                }

                if (objectType == typeof(RiDataMappingBo))
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

                    if ((int)bo.GetPropertyValue("TransformFormula") == RiDataMappingBo.TransformFormulaInputTable)
                    {
                        bo.SetPropertyValue("RiDataMappingDetailBos", GetRiDataMappingDetail(index));
                    }
                }

                bos.Add(bo);
                index++;
            }
            return bos;
        }

        public List<RiDataMappingDetailBo> GetRiDataMappingDetail(int parentIndex)
        {
            List<RiDataMappingDetailBo> riDataMappingDetailBos = new List<RiDataMappingDetailBo>();

            int index = 0;
            string indexStr = Form.Get(string.Format("RiDataMappingDetail.MaxIndex[{0}]", parentIndex));
            int maxIndex = !string.IsNullOrEmpty(indexStr) ? int.Parse(indexStr) : -1;

            while (index <= maxIndex)
            {
                RiDataMappingDetailBo riDataMappingDetailBo = (RiDataMappingDetailBo)GetFormVariables(typeof(RiDataMappingDetailBo), index, parentIndex);

                riDataMappingDetailBos.Add(riDataMappingDetailBo);

                index++;
            }

            return riDataMappingDetailBos;
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
                    case "RiDataConfigId":
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
            if (bo.GetPropertyValue("StandardOutputId") != null)
            {
                StandardOutputBo standardOutputBo = StandardOutputService.Find((int?)bo.GetPropertyValue("StandardOutputId"));
                if (standardOutputBo != null)
                {
                    bo.SetPropertyValue("StandardOutputBo", standardOutputBo);
                    bo.SetPropertyValue("StandardOutputCode", standardOutputBo.Code);
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

        public Result ValidateChildItems(bool validateFormula = false)
        {
            Result result = new Result();

            foreach (Type objectType in ChildModules)
            {
                string objectTypeName = objectType.Name.Replace("Bo", "");
                IList bos = (IList)this.GetChildBos(objectTypeName);

                MethodInfo validate = objectType.GetMethod("Validate", BindingFlags.Instance | BindingFlags.Public);

                MethodInfo getStepName = objectType.GetMethod("GetStepName", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static);
                string displayName = (string)objectType.GetField("DisplayName").GetValue(null);

                bool valCondition = (objectType != typeof(RiDataMappingBo)) ? validateFormula : false;
                bool valFormula = (objectType == typeof(RiDataComputationBo)) ? validateFormula : false;

                Dictionary<string, int> moduleIndex = new Dictionary<string, int>();
                foreach (object bo in bos)
                {
                    List<string> errors = (List<string>)validate.Invoke(bo, null);

                    if (objectType == typeof(RiDataMappingBo))
                    {
                        errors.AddRange(ValidateDataMappings(bo));
                    }

                    if (valCondition)
                    {
                        bool enableOriginal = true;
                        if (objectType == typeof(RiDataComputationBo) && (int)bo.GetPropertyValue("Step") == RiDataComputationBo.StepPreComputation1)
                            enableOriginal = false;

                        var soe = new StandardOutputEval
                        {
                            Condition = (string)bo.GetPropertyValue("Condition"),
                            EnableOriginal = enableOriginal
                        };

                        soe.EvalCondition();
                        if (valFormula && (int)bo.GetPropertyValue("Mode") == RiDataComputationBo.ModeFormula)
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

        public string[] ValidateDataMappings(object bo)
        {
            RiDataMappingBo riDataMappingBo = (RiDataMappingBo)bo;
            List<string> errors = new List<string>();

            if (riDataMappingBo.TransformFormula != RiDataMappingBo.TransformFormulaFixedValue && Delimiter != RiDataConfigBo.DelimiterFixedLength)
            {
                if (!HasHeader && !int.TryParse(riDataMappingBo.RawColumnName, out _))
                {
                    errors.Add("Raw Column Name must be numeric when there is no header");
                }
                else if (string.IsNullOrEmpty(riDataMappingBo.RawColumnName))
                {
                    errors.Add(string.Format(MessageBag.Required, "Raw Column Name"));
                }
            }

            if (IsColumnToRowMapping && riDataMappingBo.Row == null)
                errors.Add(string.Format(MessageBag.Required, "Row"));

            if (Delimiter == RiDataConfigBo.DelimiterFixedLength && riDataMappingBo.Length == null)
                errors.Add(string.Format(MessageBag.Required, "Length"));

            if (riDataMappingBo.TransformFormula == RiDataMappingBo.TransformFormulaInputTable && Status == RiDataConfigBo.StatusPending && riDataMappingBo.RiDataMappingDetailBos.Count == 0)
            {
                errors.Add(MessageBag.InputTableEmpty);
            }

            List<string> rawValues = new List<string> { };
            int index = 1;
            foreach (RiDataMappingDetailBo riDataMappingDetailBo in riDataMappingBo.RiDataMappingDetailBos)
            {
                List<string> mappingDetailErrors = riDataMappingDetailBo.Validate();

                if (!string.IsNullOrEmpty(riDataMappingDetailBo.RawValue))
                {
                    if (rawValues.Count > 0 && rawValues.Contains(riDataMappingDetailBo.RawValue))
                    {
                        errors.Add(string.Format(MessageBag.DuplicateRawValue, riDataMappingDetailBo.RawValue));
                    }
                    rawValues.Add(riDataMappingDetailBo.RawValue);
                }

                foreach (string error in mappingDetailErrors)
                {
                    errors.Add(string.Format("{0} at #{1} in Input Table", error, index));
                }

                index++;
            }

            return errors.ToArray();
        }

        public void ProcessRawDataMappings(int authUserId, ref TrailObject trail)
        {
            List<int> savedMappingIds = new List<int> { };
            foreach (RiDataMappingBo bo in RiDataMappingBos)
            {
                RiDataMappingBo riDataMappingBo = bo;

                riDataMappingBo.RiDataConfigId = Id;
                riDataMappingBo.CreatedById = authUserId;
                riDataMappingBo.UpdatedById = authUserId;

                RiDataMappingService.Save(ref riDataMappingBo, ref trail);

                if (riDataMappingBo.TransformFormula == RiDataMappingBo.TransformFormulaInputTable)
                {
                    List<int> savedMappingDetailIds = new List<int> { };
                    foreach (RiDataMappingDetailBo mappingBo in riDataMappingBo.RiDataMappingDetailBos)
                    {
                        RiDataMappingDetailBo riDataMappingDetailBo = mappingBo;
                        riDataMappingDetailBo.RiDataMappingId = riDataMappingBo.Id;
                        riDataMappingDetailBo.CreatedById = authUserId;
                        riDataMappingDetailBo.UpdatedById = authUserId;

                        RiDataMappingDetailService.Save(ref riDataMappingDetailBo, ref trail);

                        savedMappingDetailIds.Add(riDataMappingDetailBo.Id);
                    }
                    RiDataMappingDetailService.DeleteByDataMappingIdExcept(riDataMappingBo.Id, savedMappingDetailIds, ref trail);
                }

                savedMappingIds.Add(riDataMappingBo.Id);
            }
            RiDataMappingService.DeleteByDataConfigIdExcept(Id, savedMappingIds, ref trail);
        }

        public void SaveChildItems(int authUserId, ref TrailObject trail)
        {
            Type riDataMappingType = typeof(RiDataMappingService);
            Assembly service = riDataMappingType.Assembly;
            foreach (Type objectType in ChildModules)
            {
                if (objectType == typeof(RiDataMappingBo))
                {
                    ProcessRawDataMappings(authUserId, ref trail);
                    continue;
                }

                Type test = typeof(RiDataComputationService);
                string objectTypeName = objectType.Name.Replace("Bo", "");
                string temp = string.Format("Services.RiDatas.{0}Service", objectTypeName);

                bool same = temp == test.FullName;
                //Assembly assembly = Assembly.LoadFile("");/
                Type objectServiceType = service.GetType(temp);
                if (objectServiceType == null)
                {
                    temp = string.Format("{0}Service", objectTypeName);
                    objectServiceType = Type.GetType(temp);
                }

                IList bos = (IList)this.GetChildBos(objectTypeName);
                List<int> savedIds = new List<int> { };

                MethodInfo save = objectServiceType.GetMethod("Save", new Type[] { objectType.MakeByRefType(), typeof(TrailObject).MakeByRefType() });
                MethodInfo delete = objectServiceType.GetMethod("DeleteByDataConfigIdExcept", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static);

                foreach (object bo in bos)
                {
                    bo.SetPropertyValue("RiDataConfigId", Id);
                    bo.SetPropertyValue("CreatedById", authUserId);
                    bo.SetPropertyValue("UpdatedById", authUserId);
                    save.Invoke(objectServiceType, new object[] { bo, trail });
                    savedIds.Add((int)bo.GetPropertyValue("Id"));
                }

                delete.Invoke(objectServiceType, new object[] { Id, savedIds, trail });
            }
        }

        public void ProcessStatusHistory(FormCollection form, int authUserId, ref TrailObject trail)
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

                string statusRemark = form.Get(string.Format("StatusRemark"));
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

        public IList<RiDataComputationBo> GetComputationBosWithStep(int step)
        {
            return RiDataComputationBos.Where(q => q.Step == step).ToList();
        }

        public IList<RiDataPreValidationBo> GetValidationBosWithStep(int step)
        {
            return RiDataPreValidationBos.Where(q => q.Step == step).ToList();
        }
    }
}