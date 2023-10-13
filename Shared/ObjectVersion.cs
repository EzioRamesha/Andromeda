using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public abstract class ObjectVersion
    {
        public int EditableVersion { get; set; }

        public int CurrentVersion { get; set; }

        public int CurrentVersionObjectId { get; set; }

        public object CurrentVersionObject { get; set; }

        public IList<object> VersionObjects { get; set; }

        public ObjectVersion()
        {
            EditableVersion = 0;
            CurrentVersion = 0;
        }

        public List<int> GetVersions()
        {
            return VersionObjects.Select(q => int.Parse(q.GetPropertyValue("Version").ToString())).ToList();
        }

        public List<int> GetVersionIds()
        {
            return VersionObjects.Select(q => int.Parse(q.GetPropertyValue("Id").ToString())).ToList();
        }

        public object FindVersionObject(int id)
        {
            return VersionObjects.Where(q => int.Parse(q.GetPropertyValue("Id").ToString()) == id).FirstOrDefault();
        }

        public object FindVersionObjectByVersion(int version)
        {
            return VersionObjects.Where(q => int.Parse(q.GetPropertyValue("Version").ToString()) == version).FirstOrDefault();
        }

        public void AddVersionObject(object bo)
        {
            object versionObj = bo.GetPropertyValue("Version");
            if (versionObj == null)
                return;

            int version = int.Parse(versionObj.ToString());
            int index = VersionObjects.IndexOf(VersionObjects.Where(q => int.Parse(q.GetPropertyValue("Version").ToString()) == version).FirstOrDefault());
            if (index >= 0)
            {
                VersionObjects[index] = bo;
                if (CurrentVersion == version)
                {
                    SetCurrentVersionObject(bo);
                }
            }
            else
            {
                VersionObjects.Add(bo);

                CurrentVersion = version;
                SetCurrentVersionObject(bo);
                EditableVersion = version;
            }
        }

        public void SetCurrentVersionObject(int id)
        {
            object bo = FindVersionObject(id); 
            int version = int.Parse(bo.GetPropertyValue("Version").ToString());

            CurrentVersion = version;
            SetCurrentVersionObject(bo);
        }

        public void SetCurrentVersionObject(object bo)
        {
            int id = 0;
            object idObj = bo.GetPropertyValue("Id");
            if (idObj != null)
            {
                int.TryParse(idObj.ToString(), out id);
            }

            CurrentVersionObjectId = id;
            CurrentVersionObject = bo;
        }

        public void SetVersionObjects(IEnumerable<object> bos)
        {
            VersionObjects = new List<object>();
            if (bos == null)
                return;

            foreach (var bo in bos)
            {
                AddVersionObject(bo);
            }
        }
    }
}
