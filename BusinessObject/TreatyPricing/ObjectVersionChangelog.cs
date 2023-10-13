using BusinessObject.Identity;
using Newtonsoft.Json.Linq;
using Shared;
using Shared.Trails;
using Shared.Trails.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.TreatyPricing
{
    public class ObjectVersionChangelog
    {
        public int Version { get; set; }

        public string VersionStr { get; set; }

        public int ObjectVersionId { get; set; }

        public object VersionObject { get; set; }

        public string CreatedByName { get; set; }

        public string CreatedAtStr { get; set; }

        public IList<UserTrailBo> UserTrailBos { get; set; }

        public string BetweenVersionTrail { get; set; }

        // Workflow Details
        public TreatyPricingWorkflowObjectBo WorkflowObjectBo { get; set; }

        public ObjectVersionChangelog(object versionObject, UserTrailBo firstTrail = null, TreatyPricingWorkflowObjectBo workflowObjectBo = null)
        {
            VersionObject = versionObject;
            Version = int.Parse(versionObject.GetPropertyValue("Version").ToString());
            VersionStr = string.Format("{0}.0", Version);
            ObjectVersionId = int.Parse(versionObject.GetPropertyValue("Id").ToString());

            UserTrailBos = new List<UserTrailBo>();
            if (firstTrail != null)
            {
                UserTrailBos.Add(firstTrail);
                CreatedAtStr = firstTrail.CreatedAtStr;
                CreatedByName = firstTrail.CreatedByBo?.FullName;
            }

            WorkflowObjectBo = workflowObjectBo;
        }

        public void AddTrail(UserTrailBo trail)
        {
            UserTrailBos.Insert(0, trail);
        }

        public void FormatBetweenVersionTrail(object previousVersion)
        {
            var trail = new TrailObject();
            var trailObj = new JObject();

            foreach (var property in VersionObject.GetType().GetProperties())
            {
                string propertyName = property.Name;
                if (propertyName.EndsWith("Bo"))
                    continue;

                if (IsExclude(propertyName))
                    continue;

                var currValue = VersionObject.GetPropertyValue(property.Name);
                var prevValue = previousVersion.GetPropertyValue(property.Name);
                if (IsJson(propertyName))
                {
                    JArray currentChildren = JArray.Parse(currValue.ToString());
                    JArray previousChildren = JArray.Parse(prevValue.ToString());
                    if (currentChildren.Count == 0)
                    {
                        if (previousChildren.Count > 0)
                            trailObj.Add(propertyName, new JValue(string.Empty));

                        continue;
                    }

                    var trailChildren = FormatChildArray(propertyName, currentChildren, previousChildren);
                    

                    trailObj.Add(propertyName, trailChildren);
                }
                else
                {
                    if (currValue != null && !currValue.Equals(prevValue))
                    {
                        trailObj.Add(propertyName, new JValue(currValue));
                    }
                }
            }

            string objName = Util.ReplaceLastOccurrence(VersionObject.GetType().Name, "Bo", "s");
            trail.MergeUpdate(trailObj, objName, ObjectVersionId.ToString());
            BetweenVersionTrail = trail.ToString();
        }

        public JArray FormatChildArray(string name, JArray currentChildren, JArray previousChildren, Type type = null)
        {
            var trailChildren = new JArray();
            foreach (JObject child in currentChildren)
            {
                string similarProperty = GetSimilarProperty(name, type);

                string similarValue = TrailObject.FindPropertyStr(child, similarProperty);
                JObject previousChild = TrailObject.FindObjectByProperty(previousChildren, similarProperty, similarValue);
                if (previousChild == null)
                {
                    trailChildren.Add(child);
                    continue;
                }

                var trailChild = new JObject();
                foreach (var property in child)
                {
                    string propertyName = property.Key;

                    if (propertyName.EndsWith("Bo"))
                        continue;

                    JToken value = property.Value;
                    JToken prevValue = TrailObject.FindProperty(previousChild, propertyName);

                    if (value is JArray arr && prevValue is JArray prevArr)
                    {
                        Type childType = Type.GetType(string.Format("BusinessObject.TreatyPricing.{0}Bo", name));

                        trailChild.Add(propertyName, FormatChildArray(propertyName, arr, prevArr, childType));
                    }
                    else
                    {
                        string valueStr = value.ToObject<string>();
                        if (propertyName == similarProperty)
                        {
                            trailChild.Add(new JProperty(propertyName, value));
                            continue;
                        }

                        string previousChildPropertyValue = TrailObject.FindPropertyStr(previousChild, propertyName);
                        if (valueStr != previousChildPropertyValue)
                        {
                            trailChild.Add(new JProperty(propertyName, value));
                        }
                    }
                }

                if (trailChild.Count > 1)
                {
                    trailChildren.Add(trailChild);
                }
            }

            return trailChildren;
        }

        public bool IsJson(string propertyName)
        {
            return Attribute.IsDefined(VersionObject.GetType().GetProperty(propertyName), typeof(IsJsonPropertyAttribute));
        }

        public bool IsExclude(string propertyName)
        {
            return Attribute.IsDefined(VersionObject.GetType().GetProperty(propertyName), typeof(ExcludeTrailAttribute));
        }

        public string GetSimilarProperty(string propertyName, Type type = null)
        {
            if (type == null)
                type = VersionObject.GetType();

            var jsonAttribute = type.GetProperty(propertyName).GetCustomAttribute<IsJsonPropertyAttribute>();
            return jsonAttribute.SimilarProperty;
        }
    }
}
