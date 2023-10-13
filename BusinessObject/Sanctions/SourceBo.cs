using System;
using System.Collections.Generic;

namespace BusinessObject.Sanctions
{
    public class SourceBo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int TypeUN = 1;
        public const int TypeOFAC = 2;

        public static List<SourceBo> GetSources()
        {
            return new List<SourceBo>() {
                GetStaticSource(TypeUN),
                GetStaticSource(TypeOFAC),
            };
        }

        public static SourceBo GetStaticSource(int key)
        {
            switch (key)
            {
                case TypeUN:
                    return new SourceBo
                    {
                        Id = key,
                        Name = "UN",
                    };
                case TypeOFAC:
                    return new SourceBo
                    {
                        Id = key,
                        Name = "OFAC",
                    };
                default:
                    return null;
            }
        }
    }
}
