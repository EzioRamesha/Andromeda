using System.Collections.Generic;

namespace BusinessObject.Identity
{
    public class DepartmentBo
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int? HodUserId { get; set; }

        public int CreatedById { get; set; }

        public int? UpdatedById { get; set; }

        public const int DepartmentTreatyPricing = 1;
        public const int DepartmentClaim = 2;
        public const int DepartmentDataAnalyticsAdministration = 3;
        public const int DepartmentShared = 4;
        public const int DepartmentIT = 5;
        public const int DepartmentValuation = 6;
        public const int DepartmentRetrocession = 7;
        public const int DepartmentUnderwriting = 8;
        public const int DepartmentFinance = 9;
        public const int DepartmentComplianceRisk = 10;
        public const int DepartmentCEO = 11;
        public const int DepartmentBD = 12;
        public const int DepartmentHealth = 13;
        public const int DepartmentProductPricing = 14;
        public const int DepartmentTreatyGroupPricing = 15;

        public static IList<DepartmentBo> GetDepartments()
        {
            return new List<DepartmentBo>() {
                GetStaticDepartment(DepartmentTreatyPricing),
                GetStaticDepartment(DepartmentClaim),
                GetStaticDepartment(DepartmentDataAnalyticsAdministration),
                GetStaticDepartment(DepartmentShared),
                GetStaticDepartment(DepartmentIT),
                GetStaticDepartment(DepartmentValuation),
                GetStaticDepartment(DepartmentRetrocession),
                GetStaticDepartment(DepartmentUnderwriting),
                GetStaticDepartment(DepartmentFinance),
                GetStaticDepartment(DepartmentComplianceRisk),
                GetStaticDepartment(DepartmentCEO),
                GetStaticDepartment(DepartmentBD),
                GetStaticDepartment(DepartmentHealth),
                GetStaticDepartment(DepartmentProductPricing),
                GetStaticDepartment(DepartmentTreatyGroupPricing),
            };
        }

        public static string GetIconName(int key)
        {
            switch(key)
            {
                case 1:
                    return "fas fa-hand-holding-usd";
                case 2:
                    return "fas fa-file-invoice-dollar";
                case 3:
                    return "fas fa-chart-line";
                case 4:
                    return "fas fa-cogs";
                case 5:
                    return "fas fa-user-cog";
                case 6:
                    return "fas fa-list-alt";
                case 7:
                    return "fas fa-list-alt";
                case 8:
                    return "fas fa-list-alt";
                case 9:
                    return "fas fa-list-alt";
                case 10:
                    return "fas fa-list-alt";
                case 11:
                    return "fas fa-list-alt";
                case 12:
                    return "fas fa-list-alt";
                case 13:
                    return "fas fa-list-alt";
                default:
                    return "";
            }
        }

        public static DepartmentBo GetStaticDepartment(int key)
        {
            switch (key)
            {
                case DepartmentTreatyPricing:
                    return new DepartmentBo
                    {
                        Id = key,
                        Code = "TP",
                        Name = "Treaty & Pricing",
                    };
                case DepartmentClaim:
                    return new DepartmentBo
                    {
                        Id = key,
                        Code = "CLAIM",
                        Name = "Claim",
                    };
                case DepartmentDataAnalyticsAdministration:
                    return new DepartmentBo
                    {
                        Id = key,
                        Code = "DAA",
                        Name = "Data Analytics & Administration",
                    };
                case DepartmentShared:
                    return new DepartmentBo
                    {
                        Id = key,
                        Code = "SHARED",
                        Name = "Shared",
                    };
                case DepartmentIT:
                    return new DepartmentBo
                    {
                        Id = key,
                        Code = "IT",
                        Name = "IT",
                    };
                case DepartmentValuation:
                    return new DepartmentBo
                    {
                        Id = key,
                        Code = "VALUATION",
                        Name = "Valuation",
                    };
                case DepartmentRetrocession:
                    return new DepartmentBo
                    {
                        Id = key,
                        Code = "RETROCESSION",
                        Name = "Retrocession",
                    };
                case DepartmentUnderwriting:
                    return new DepartmentBo
                    {
                        Id = key,
                        Code = "UNDERWRITING",
                        Name = "Underwriting",
                    };
                case DepartmentFinance:
                    return new DepartmentBo
                    {
                        Id = key,
                        Code = "FINANCE",
                        Name = "Finance",
                    };
                case DepartmentComplianceRisk:
                    return new DepartmentBo
                    {
                        Id = key,
                        Code = "CR",
                        Name = "Compliance & Risk",
                    };
                case DepartmentCEO:
                    return new DepartmentBo
                    {
                        Id = key,
                        Code = "CEO",
                        Name = "CEO",
                    };
                case DepartmentBD:
                    return new DepartmentBo
                    {
                        Id = key,
                        Code = "BD",
                        Name = "Business Development",
                    };
                case DepartmentHealth:
                    return new DepartmentBo
                    {
                        Id = key,
                        Code = "HEALTH",
                        Name = "Health",
                    };
                case DepartmentProductPricing:
                    return new DepartmentBo
                    {
                        Id = key,
                        Code = "PP",
                        Name = "Product Pricing",
                    };
                case DepartmentTreatyGroupPricing:
                    return new DepartmentBo
                    {
                        Id = key,
                        Code = "TGP",
                        Name = "Treaty & Group Pricing",
                    };
                default:
                    return null;
            }
        }
    }
}
