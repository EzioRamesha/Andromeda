using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Shared.Trails
{
    public class TrailObject
    {
        public enum TrailTypes
        {
            Trail,
            Insert,
            Update,
            Delete,
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JObject Trail { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JObject Insert { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JObject Update { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public JObject Delete { get; set; }

        public bool IsValidId(JObject trail, ref string id)
        {
            if (id == null)
            {
                id = (string)trail.GetValue("Id");
                if (id == null)
                {
                    throw new Exception("Please enter id");
                }
            }
            return true;
        }

        public bool IsValid()
        {
            return !(Trail == null && Insert == null && Update == null && Delete == null);
        }

        public void Merge(JObject trail, string table, int trailType = (int)TrailTypes.Trail, string id = null)
        {
            IsValidId(trail, ref id);

            JObject tableTrail = new JObject();
            JObject objectTrail = new JObject();

            switch (trailType)
            {
                case (int)TrailTypes.Trail:
                    if (Trail == null)
                        Trail = new JObject();

                    if (Trail.ContainsKey(table))
                    {
                        tableTrail.Merge(Trail.GetValue(table));
                    }
                    break;

                case (int)TrailTypes.Insert:
                    if (Insert == null)
                        Insert = new JObject();

                    if (Insert.ContainsKey(table))
                    {
                        tableTrail.Merge(Insert.GetValue(table));
                    }
                    break;

                case (int)TrailTypes.Update:
                    if (Update == null)
                        Update = new JObject();

                    if (Update.ContainsKey(table))
                    {
                        tableTrail.Merge(Update.GetValue(table));
                    }
                    break;

                case (int)TrailTypes.Delete:
                    if (Delete == null)
                        Delete = new JObject();

                    if (Delete.ContainsKey(table))
                    {
                        tableTrail.Merge(Delete.GetValue(table));
                    }
                    break;
            }

            if (tableTrail.ContainsKey(id))
            {
                objectTrail.Merge(tableTrail.GetValue(id));
            }
            objectTrail.Merge(trail);

            tableTrail[id] = objectTrail;
            switch (trailType)
            {
                case (int)TrailTypes.Trail:
                    Trail[table] = tableTrail;
                    break;

                case (int)TrailTypes.Insert:
                    Insert[table] = tableTrail;
                    break;

                case (int)TrailTypes.Update:
                    Update[table] = tableTrail;
                    break;

                case (int)TrailTypes.Delete:
                    Delete[table] = tableTrail;
                    break;
            }
        }

        public void MergeTrail(JObject trail, string table, string id = null)
        {
            Merge(trail, table, (int)TrailTypes.Trail, id);
        }

        public void MergeInsert(JObject trail, string table, string id = null)
        {
            Merge(trail, table, (int)TrailTypes.Insert, id);
        }

        public void MergeUpdate(JObject trail, string table, string id = null)
        {
            Merge(trail, table, (int)TrailTypes.Update, id);
        }

        public void MergeDelete(JObject trail, string table, string id = null)
        {
            Merge(trail, table, (int)TrailTypes.Delete, id);
        }

        public void Test()
        {
            /*
            Insert = new JObject(
                new JObject(
                    new JProperty("AccessGroup", new JObject(
                            new JProperty("testid1", new JObject(
                                    new JProperty("Id", "testid1"),
                                    new JProperty("Code", "codename")
                                )
                            ),
                            new JProperty("testid2", new JObject(
                                    new JProperty("Id", "testid2"),
                                    new JProperty("Code", "policebefore"),
                                    new JProperty("Name", "nameBefore")
                                )
                            )
                        )
                    )
                )
            );

            var ag2 = new JObject(
                new JProperty("Id", "testid2"),
                new JProperty("Name", "nameAfter")
            );

            MergeInsert(ag2, "AccessGroup");

            Console.WriteLine(Insert);
            */

            Update = new JObject(
                new JObject(
                    new JProperty("AccessGroup", new JObject(
                            new JProperty("testid1", new JObject(
                                    new JProperty("Id", "testid1"),
                                    new JProperty("Code", "codename")
                                )
                            ),
                            new JProperty("testid2", new JObject(
                                    new JProperty("Id", "testid2"),
                                    new JProperty("Code", "policebefore"),
                                    new JProperty("Name", "nameBefore")
                                )
                            )
                        )
                    )
                )
            );

            var ag2 = new JObject(
                new JProperty("Id", "testid2"),
                new JProperty("Name", "nameAfter")
            );

            MergeUpdate(ag2, "AccessGroup");

            Console.WriteLine(Update);
        }

        public Formatting GetFormatting()
        {
            string formatStr = Util.GetConfig("TrailFormat");
            if (bool.TryParse(formatStr, out bool format))
            {
                if (format)
                    return Formatting.Indented;
            }
            return Formatting.None;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, GetFormatting());
        }

        public string ToStringIgnoreNull()
        {
            return JsonConvert.SerializeObject(this,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = GetFormatting(),
                }
            );
        }

        public static JObject FindTable(JObject container, string table)
        {
            if (container == null)
                return null;
            foreach (var property in container)
            {
                if (property.Key == table)
                    return property.Value.ToObject<JObject>();
            }

            return null;
        }

        public static JProperty FindFirstTable(JObject container, string table)
        {
            JObject obj = FindTable(container, table);
            if (obj == null)
                return null;

            return obj.First.ToObject<JProperty>();
        }

        public static JToken FindProperty(JToken container, string name)
        {
            if (container is JObject obj)
            {
                foreach (var property in obj)
                {
                    if (property.Key == name)
                        return property.Value;
                }
            }

            return null;
        }

        public static string FindPropertyStr(JToken container, string name)
        {
            return FindProperty(container, name)?.ToObject<string>();
        }

        public static JObject FindObjectByProperty(JArray container, string name, object value)
        {
            foreach (JObject obj in container) {
                if (FindPropertyStr(obj, name) == value.ToString())
                    return obj;
            }

            return null;
        }

        public static JProperty FindFirstProperty(JToken container, string name)
        {
            if (container is JObject obj)
            {
                foreach (var pair in obj)
                {
                    return FindFirstProperty(pair.Value, name);
                }
            }
            else if (container is JProperty prop)
            {
                if (prop.Value is JObject obj2)
                {
                    return FindFirstProperty(obj2, name);
                }
                else if (prop.Name == name)
                    return prop;
            }

            return null;
        }
    }
}