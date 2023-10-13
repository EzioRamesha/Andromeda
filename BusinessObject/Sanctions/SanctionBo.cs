using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Sanctions
{
    public class SanctionBo
    {
        public int Id { get; set; }

        public int SanctionBatchId { get; set; }

        public SanctionBatchBo SanctionBatchBo { get; set; }

        public string PublicationInformation { get; set; }

        public string Category { get; set; }

        public string Name { get; set; }

        public string RefNumber { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public IList<SanctionNameBo> SanctionNameBos { get; set; }
        public IList<SanctionAddressBo> SanctionAddressBos { get; set; }
        public IList<SanctionBirthDateBo> SanctionBirthDateBos { get; set; }
        public IList<SanctionCommentBo> SanctionCommentBos { get; set; }
        public IList<SanctionCountryBo> SanctionCountryBos { get; set; }
        public IList<SanctionIdentityBo> SanctionIdentityBos { get; set; }
        public IList<SanctionFormatNameBo> SanctionFormatNameBos { get; set; }

        public const int CategoryIndividual = 1;
        public const int CategoryEntity = 2;

        public const int CategoryMax = 2;

        public static string GetCategoryName(int key, bool toUpper = false)
        {
            string category;
            switch (key)
            {
                case CategoryIndividual:
                    category = "Individual";
                    break;
                case CategoryEntity:
                    category = "Entity";
                    break;
                default:
                    category = "";
                    break;
            }

            if (toUpper)
                category = category.ToUpper();

            return category;
        }
    }
}
