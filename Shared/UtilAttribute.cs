using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared
{
    public class UtilAttribute
    {
        public static string GetTableName(Type t)
        {
            TableAttribute att = (TableAttribute)System.Attribute.GetCustomAttribute(t, typeof(TableAttribute));

            if (att == null)
            {
                return null;
            }
            else
            {
                return att.Name;
            }
        }
    }
}
