using BusinessObject;
using DataAccess.Entities;
using Services;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Models
{
    public class DiscountTableViewModel
    {
        public int Id { get; set; }

        [Required, DisplayName("Ceding Company")]
        public int CedantId { get; set; }

        public Cedant Cedant { get; set; }

        public CedantBo CedantBo { get; set; }

        public FormCollection Form { get; set; }

        public IList<RiDiscountBo> RiDiscountBos { get; set; }

        public IList<LargeDiscountBo> LargeDiscountBos { get; set; }

        public IList<GroupDiscountBo> GroupDiscountBos { get; set; }

        private readonly List<Type> ChildModules = new List<Type>
        {
            typeof(RiDiscountBo),
            typeof(LargeDiscountBo),
            typeof(GroupDiscountBo),
        };

        public DiscountTableViewModel() { }

        public DiscountTableViewModel(DiscountTableBo discountTableBo)
        {
            Set(discountTableBo);
        }

        public void Set(DiscountTableBo bo)
        {
            if (bo != null)
            {
                Id = bo.Id;
                CedantId = bo.CedantId;
                CedantBo = bo.CedantBo;
            }
        }

        public void Get(ref DiscountTableBo bo)
        {
            bo.CedantId = CedantId;
            bo.CedantBo = CedantBo;

            // Child Items
            bo.RiDiscountBos = RiDiscountBos;
            bo.LargeDiscountBos = LargeDiscountBos;
            bo.GroupDiscountBos = GroupDiscountBos;
        }

        public DiscountTableBo FormBo(int createdById, int updatedById)
        {
            return new DiscountTableBo
            {
                CedantId = CedantId,
                CedantBo = CedantService.Find(CedantId),
                CreatedById = createdById,
                UpdatedById = updatedById,
            };
        }

        public static Expression<Func<DiscountTable, DiscountTableViewModel>> Expression()
        {
            return entity => new DiscountTableViewModel
            {
                Id = entity.Id,
                CedantId = entity.CedantId,
                Cedant = entity.Cedant,
            };
        }

        public void SetChildItems(DiscountTableBo discountTableBo = null)
        {
            foreach (Type objectType in ChildModules)
            {
                string objectTypeName = objectType.Name.Replace("Bo", "");
                string objectListName = string.Format("{0}Bos", objectTypeName);
                IList bos = null;

                if (discountTableBo == null)
                {
                    string maxIndexStr = Form.Get(string.Format("{0}.MaxIndex", objectTypeName));

                    if (string.IsNullOrEmpty(maxIndexStr))
                        continue;

                    int maxIndex = int.Parse(maxIndexStr);
                    bos = GetChildItem(objectType, maxIndex);
                }

                this.SetPropertyValue(objectListName, bos);
            }
        }

        public IList GetChildItem(Type objectType, int maxIndex)
        {
            Type genericListType = typeof(List<>);
            Type listBoType = genericListType.MakeGenericType(objectType);
            IList bos = (IList)Activator.CreateInstance(listBoType);

            MethodInfo isEmpty = objectType.GetMethod("IsEmpty", BindingFlags.Instance | BindingFlags.Public);

            int index = 0;
            while (index <= maxIndex)
            {
                object bo = GetFormVariables(objectType, index);

                if (isEmpty != null && (bool)isEmpty.Invoke(bo, null))
                {
                    index++;
                    continue;
                }

                bos.Add(bo);
                index++;
            }
            return bos;
        }

        public object GetFormVariables(Type objectType, int index)
        {
            object bo = Activator.CreateInstance(objectType);
            string objectTypeName = objectType.Name.Replace("Bo", "");

            foreach (var property in objectType.GetProperties())
            {
                string propertyName = property.Name;
                string formValue;

                if (propertyName == "DiscountTableId")
                {
                    bo.SetPropertyValue(propertyName, Id);
                    continue;
                }

                formValue = Form.Get(string.Format("{0}.{1}[{2}]", objectTypeName, propertyName, index));

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

                bo.SetPropertyValue(propertyName, value);
            }

            SetObjects(ref bo);
            return bo;
        }

        public void SetObjects(ref object bo)
        {
            if (bo.GetPropertyValue("DiscountCode") != null)
            {
                var code = bo.GetPropertyValue("DiscountCode").ToString()?.Trim().ToLower();
                bo.SetPropertyValue("DiscountCodeToLower", code);
            }

            if (bo.GetPropertyValue("EffectiveStartDateStr") != null)
            {
                DateTime? dt = Util.GetParseDateTime(bo.GetPropertyValue("EffectiveStartDateStr").ToString());
                if (dt.HasValue)
                {
                    bo.SetPropertyValue("EffectiveStartDate", dt);
                }
            }

            if (bo.GetPropertyValue("EffectiveEndDateStr") != null)
            {
                DateTime? dt = Util.GetParseDateTime(bo.GetPropertyValue("EffectiveEndDateStr").ToString());
                if (dt.HasValue)
                {
                    bo.SetPropertyValue("EffectiveEndDate", dt);
                }
            }

            if (bo.GetPropertyValue("DurationFromStr") != null)
            {
                double? d = Util.StringToDouble(bo.GetPropertyValue("DurationFromStr").ToString());
                if (d.HasValue)
                {
                    bo.SetPropertyValue("DurationFrom", d);
                }
            }

            if (bo.GetPropertyValue("DurationToStr") != null)
            {
                double? d = Util.StringToDouble(bo.GetPropertyValue("DurationToStr").ToString());
                if (d.HasValue)
                {
                    bo.SetPropertyValue("DurationTo", d);
                }
            }

            if (bo.GetPropertyValue("AarFromStr") != null)
            {
                double? d = Util.StringToDouble(bo.GetPropertyValue("AarFromStr").ToString());
                if (d.HasValue)
                {
                    bo.SetPropertyValue("AarFrom", d);
                }
            }

            if (bo.GetPropertyValue("AarToStr") != null)
            {
                double? d = Util.StringToDouble(bo.GetPropertyValue("AarToStr").ToString());
                if (d.HasValue)
                {
                    bo.SetPropertyValue("AarTo", d);
                }
            }

            if (bo.GetPropertyValue("GroupSizeFromStr") != null)
            {
                int? i = Util.GetParseInt(bo.GetPropertyValue("GroupSizeFromStr").ToString());
                if (i.HasValue)
                {
                    bo.SetPropertyValue("GroupSizeFrom", i);
                }
            }

            if (bo.GetPropertyValue("GroupSizeToStr") != null)
            {
                int? i = Util.GetParseInt(bo.GetPropertyValue("GroupSizeToStr").ToString());
                if (i.HasValue)
                {
                    bo.SetPropertyValue("GroupSizeTo", i);
                }
            }

            if (bo.GetPropertyValue("DiscountStr") != null)
            {
                double? d = Util.StringToDouble(bo.GetPropertyValue("DiscountStr").ToString());
                if (d.HasValue)
                {
                    bo.SetPropertyValue("Discount", d);
                }
            }
        }

        public Result ValidateChildItems()
        {
            Result result = new Result();
            foreach (Type objectType in ChildModules)
            {
                string objectTypeName = objectType.Name.Replace("Bo", "");
                IList bos = (IList)this.GetPropertyValue(string.Format("{0}Bos", objectTypeName));

                if (bos == null)
                    continue;

                MethodInfo validate = objectType.GetMethod("Validate", BindingFlags.Instance | BindingFlags.Public);

                string displayName = (string)objectType.GetField("DisplayName").GetValue(null);

                int index = 1;
                foreach (object bo in bos)
                {
                    List<string> errors = (List<string>)validate.Invoke(bo, new object[] { });

                    foreach (string error in errors)
                    {
                        result.AddError(string.Format(MessageBag.ForModuleAtNo, error, displayName, index));
                    }

                    index++;
                }
            }
            return result;
        }

        public Result ValidateDuplicate()
        {
            Result result = new Result();
            foreach (Type objectType in ChildModules)
            {
                string objectTypeName = objectType.Name.Replace("Bo", "");
                IList bos = (IList)this.GetPropertyValue(string.Format("{0}Bos", objectTypeName));

                if (bos == null)
                    continue;

                MethodInfo validateDuplicate = objectType.GetMethod("ValidateDuplicate", BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);

                string displayName = (string)objectType.GetField("DisplayName").GetValue(null);

                Type temp = bos.GetType();
                List<string> errors = (List<string>)validateDuplicate.Invoke(objectType, new object[] { bos });

                foreach (string error in errors)
                {
                    result.AddError(string.Format(error, displayName));
                }
            }
            return result;
        }

        public void SaveChildItems(int authUserId, ref TrailObject trail)
        {
            Type discountTableType = typeof(DiscountTableService);
            Assembly service = discountTableType.Assembly;

            foreach (Type objectType in ChildModules)
            {
                string objectTypeName = objectType.Name.Replace("Bo", "");
                string temp = string.Format("Services.{0}Service", objectTypeName);
                Type objectServiceType = service.GetType(temp);

                IList bos = (IList)this.GetPropertyValue(string.Format("{0}Bos", objectTypeName));
                List<int> savedIds = new List<int> { };

                if (bos == null)
                    continue;

                MethodInfo save = objectServiceType.GetMethod("Save", new Type[] { objectType.MakeByRefType(), typeof(TrailObject).MakeByRefType() });
                MethodInfo delete = objectServiceType.GetMethod("DeleteByDiscountTableIdExcept", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static);

                foreach (object bo in bos)
                {
                    bo.SetPropertyValue("DiscountTableId", Id);
                    bo.SetPropertyValue("CreatedById", authUserId);
                    bo.SetPropertyValue("UpdatedById", authUserId);
                    save.Invoke(objectServiceType, new object[] { bo, trail });
                    savedIds.Add((int)bo.GetPropertyValue("Id"));
                }
                delete.Invoke(objectServiceType, new object[] { Id, savedIds, trail });
            }
        }
    }
}