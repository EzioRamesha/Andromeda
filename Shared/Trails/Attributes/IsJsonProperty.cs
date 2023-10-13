using System;

namespace Shared.Trails.Attributes
{
    public class IsJsonPropertyAttribute : Attribute
    {
        public string SimilarProperty { get; set; }

        public IsJsonPropertyAttribute(string similarProperty = "Id")
        {
            SimilarProperty = similarProperty;
        }
    }
}
