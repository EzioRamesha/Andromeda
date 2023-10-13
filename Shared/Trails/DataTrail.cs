using Newtonsoft.Json.Linq;
using Shared.Trails.Attributes;
using System;
using System.Linq;

namespace Shared.Trails
{
    public class DataTrail
    {
        public object Ori { get; set; }
        public object New { get; set; }
        public int TrailType { get; set; } = (int)TrailObject.TrailTypes.Insert;
        public JObject Trail { get; set; }
        public string[] PrimaryKeys { get; set; }
        public string[] IgnoreFields { get; set; }
        public bool Valid { get; set; } = false;
        public bool Update { get; set; } = false;

        public DataTrail()
        {
        }

        public DataTrail(object oriObj, bool delete = false, bool trail = false, string[] primaryKeys = null, string[] ignoreFields = null)
        {
            Ori = oriObj;

            if (delete)
                TrailType = (int)TrailObject.TrailTypes.Delete;
            else
                TrailType = (int)TrailObject.TrailTypes.Insert;

            if (trail)
                TrailType = (int)TrailObject.TrailTypes.Trail;

            PrimaryKeys = primaryKeys;
            IgnoreFields = ignoreFields;

            Process();
        }

        public DataTrail(object oriObj, object newObj, string[] primaryKeys = null, string[] ignoreFields = null)
        {
            Ori = oriObj;
            New = newObj;

            TrailType = (int)TrailObject.TrailTypes.Update;

            PrimaryKeys = primaryKeys;
            IgnoreFields = ignoreFields;

            Process();
        }

        public void Process()
        {
            if (Ori == null)
                return;

            Valid = false;
            Update = false;

            Trail = new JObject();
            foreach (var property in Ori.GetType().GetProperties())
            {
                if (Excluded(property.Name))
                {
                    continue;
                }
                if (IgnoreFields != null && IgnoreFields.Contains(property.Name))
                {
                    continue;
                }

                var oriValue = GetOriValue(property.Name);
                var newValue = GetNewValue(property.Name);

                object value = null;
                bool add = false;
                bool primaryKey = false;

                switch (TrailType)
                {
                    case (int)TrailObject.TrailTypes.Trail:
                    case (int)TrailObject.TrailTypes.Insert:
                    case (int)TrailObject.TrailTypes.Delete:
                        value = oriValue;
                        add = true;
                        break;

                    case (int)TrailObject.TrailTypes.Update:
                        if ((oriValue != null && !oriValue.Equals(newValue)) || (newValue != null && !newValue.Equals(oriValue)))
                        {
                            value = newValue;
                            add = true;

                            Update = true;
                        }
                        break;
                }

                if (!add)
                {
                    if (property.Name == "Id" || (PrimaryKeys != null && PrimaryKeys.Contains(property.Name)))
                    {
                        value = oriValue;
                        add = true;
                        primaryKey = true;
                    }
                }

                if (add)
                {
                    if (Trail.ContainsKey(property.Name))
                    {
                        Trail[property.Name] = new JValue(value);

                        if (!primaryKey)
                            Valid = true;
                    }
                    else
                    {
                        Trail.Add(property.Name, new JValue(value));

                        if (!primaryKey)
                            Valid = true;
                    }
                }
            }
        }

        public bool Excluded(string propertyName)
        {
            return Attribute.IsDefined(Ori.GetType().GetProperty(propertyName), typeof(ExcludeTrailAttribute));
        }

        public object GetOriValue(string propertyName)
        {
            return Ori.GetType().GetProperty(propertyName).GetValue(Ori, null);
        }

        public object GetNewValue(string propertyName)
        {
            if (New == null)
                return null;
            return New.GetType().GetProperty(propertyName).GetValue(New, null);
        }

        public void Merge(ref TrailObject trail, string table, string id = null)
        {
            switch (TrailType)
            {
                case (int)TrailObject.TrailTypes.Trail:
                    if (Valid)
                        trail.MergeTrail(Trail, table, id);
                    break;

                case (int)TrailObject.TrailTypes.Insert:
                    if (Valid)
                        trail.MergeInsert(Trail, table, id);
                    break;

                case (int)TrailObject.TrailTypes.Delete:
                    if (Valid)
                        trail.MergeDelete(Trail, table, id);
                    break;

                case (int)TrailObject.TrailTypes.Update:
                    if (Valid)
                        trail.MergeUpdate(Trail, table, id);
                    break;
            }
        }

        public void MergeTrail(ref TrailObject trail, string table, string id = null)
        {
            if (Valid)
                trail.MergeTrail(Trail, table, id);
        }

        public void MergeInsert(ref TrailObject trail, string table, string id = null)
        {
            if (Valid)
                trail.MergeInsert(Trail, table, id);
        }

        public void MergeUpdate(ref TrailObject trail, string table, string id = null)
        {
            if (Valid)
                trail.MergeUpdate(Trail, table, id);
        }

        public void MergeDelete(ref TrailObject trail, string table, string id = null)
        {
            if (Valid)
                trail.MergeDelete(Trail, table, id);
        }
    }
}