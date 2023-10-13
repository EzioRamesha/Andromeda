using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.TreatyPricing
{
    public class HipsCategoryDetailBo
    {
        public int Id { get; set; }

        public int HipsCategoryId { get; set; }

        public HipsCategoryBo HipsCategoryBo { get; set; }

        public string Subcategory { get; set; }

        public string Description { get; set; }

        public int ItemType { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const int ItemTypeAmount = 1;
        public const int ItemTypeText = 2;
        public const int ItemTypeTextAmount = 3;

        public const int ItemTypeMax = 3;

        public static string GetItemTypeName(int key)
        {
            switch(key)
            {
                case ItemTypeAmount:
                    return "Amount";
                case ItemTypeText:
                    return "Text";
                case ItemTypeTextAmount:
                    return "Text / Amount";
                default:
                    return "";
            }
        }
    }
}
