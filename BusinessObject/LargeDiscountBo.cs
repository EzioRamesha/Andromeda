using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class LargeDiscountBo
    {
        public int Id { get; set; }

        public int DiscountTableId { get; set; }

        public DiscountTableBo DiscountTableBo { get; set; }

        public string DiscountCode { get; set; }

        public string DiscountCodeToLower { get; set; }

        public DateTime? EffectiveStartDate { get; set; }

        public string EffectiveStartDateStr { get; set; }

        public DateTime? EffectiveEndDate { get; set; }

        public string EffectiveEndDateStr { get; set; }

        public double AarFrom { get; set; }

        public string AarFromStr { get; set; }

        public double AarTo { get; set; }

        public string AarToStr { get; set; }

        public double Discount { get; set; }

        public string DiscountStr { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const string DisplayName = "Large SA Discount";

        public const int TypeId = 1;
        public const int TypeDiscountCode = 2;
        public const int TypeEffectiveStartDate = 3;
        public const int TypeEffectiveEndDate = 4;
        public const int TypeAarFrom = 5;
        public const int TypeAarTo = 6;
        public const int TypeDiscount = 7;

        public List<string> Validate()
        {
            List<string> errors = new List<string> { };
            DateTime? dateFrom = null;
            DateTime? dateTo = null;
            double? aarFrom = null;
            double? aarTo = null;

            bool dateError = false;

            if (string.IsNullOrEmpty(DiscountCode))
                errors.Add(string.Format(MessageBag.Required, "Discount Code"));

            if (!string.IsNullOrEmpty(EffectiveStartDateStr) && !Util.TryParseDateTime(EffectiveStartDateStr, out _, out _))
            {
                errors.Add(string.Format(MessageBag.InvalidDateFormatWithName, "Effective Start Date"));
                dateError = true;
            }
            else
            {
                dateFrom = Util.GetParseDateTime(EffectiveStartDateStr);
            }

            if (!string.IsNullOrEmpty(EffectiveEndDateStr) && !Util.TryParseDateTime(EffectiveEndDateStr, out _, out _))
            {
                errors.Add(string.Format(MessageBag.InvalidDateFormatWithName, "Effective End Date"));
                dateError = true;
            }
            else
            {
                dateTo = Util.GetParseDateTime(EffectiveEndDateStr);
            }

            if (!dateError)
            {
                if (dateFrom.HasValue && !dateTo.HasValue)
                {
                    errors.Add(string.Format(MessageBag.Required, "Effective End Date"));
                }
                else if (!dateFrom.HasValue && dateTo.HasValue)
                {
                    errors.Add(string.Format(MessageBag.Required, "Effective Start Date"));
                }
                else if (dateFrom.HasValue && dateTo.HasValue && dateTo < dateFrom)
                {
                    errors.Add(string.Format(MessageBag.GreaterOrEqualTo, "Effective End Date", "Effective Start Date"));
                }
            }

            if (string.IsNullOrEmpty(AarFromStr))
            {
                errors.Add(string.Format(MessageBag.Required, "ORI Sum Assured From"));
            }
            else if (!Util.IsValidDouble(AarFromStr, out _, out _))
            {
                errors.Add("ORI Sum Assured From format is not valid");
            }
            else
            {
                aarFrom = Util.StringToDouble(AarFromStr);
            }

            if (string.IsNullOrEmpty(AarToStr))
            {
                errors.Add(string.Format(MessageBag.Required, "ORI Sum Assured To"));
            }
            else if (!Util.IsValidDouble(AarToStr, out _, out _))
            {
                errors.Add("ORI Sum Assured To format is not valid");
            }
            else
            {
                aarTo = Util.StringToDouble(AarToStr);
            }

            if (aarFrom.HasValue && aarTo.HasValue && aarTo < aarFrom)
            {
                errors.Add(string.Format(MessageBag.GreaterOrEqualTo, "ORI Sum Assured To", "ORI Sum Assured From"));
            }

            if (string.IsNullOrEmpty(DiscountStr))
            {
                errors.Add(string.Format(MessageBag.Required, "Discount"));
            }
            else if (!Util.IsValidDouble(DiscountStr, out _, out _))
            {
                errors.Add("Discount format is not valid");
            }

            return errors;
        }

        public static List<string> ValidateDuplicate(IList<LargeDiscountBo> bos)
        {
            List<string> errors = new List<string> { };
            List<LargeDiscountBo> largeDiscountBos = new List<LargeDiscountBo> { };
            int count = 1;

            foreach (var bo in bos)
            {
                if (count == 1)
                {
                    largeDiscountBos.Add(new LargeDiscountBo
                    {
                        DiscountCodeToLower = bo.DiscountCodeToLower,
                        EffectiveStartDate = bo.EffectiveStartDate,
                        EffectiveEndDate = bo.EffectiveEndDate,
                        AarFrom = bo.AarFrom,
                        AarTo = bo.AarTo,
                    });
                }
                else
                {
                    var query = largeDiscountBos
                        .Where(q => q.DiscountCodeToLower == bo.DiscountCodeToLower)
                        .Where(q =>
                            (
                                q.AarFrom <= bo.AarFrom
                                && q.AarTo >= bo.AarFrom
                                ||
                                q.AarFrom <= bo.AarTo
                                && q.AarTo >= bo.AarTo
                            ));

                    if (bo.EffectiveStartDate.HasValue && bo.EffectiveEndDate.HasValue)
                    {
                        query = query
                            .Where(q =>
                            (
                                q.EffectiveStartDate <= bo.EffectiveStartDate
                                && q.EffectiveEndDate >= bo.EffectiveStartDate
                                ||
                                q.EffectiveStartDate <= bo.EffectiveEndDate
                                && q.EffectiveEndDate >= bo.EffectiveEndDate
                            )
                            || (!q.EffectiveStartDate.HasValue && !q.EffectiveEndDate.HasValue)
                        );
                    }

                    if (query.Any())
                    {
                        int idx = bos.IndexOf(bo) + 1;
                        errors.Add("Duplicate Discount: {0} at # Row " + idx);
                    }

                    largeDiscountBos.Add(new LargeDiscountBo
                    {
                        DiscountCodeToLower = bo.DiscountCodeToLower,
                        EffectiveStartDate = bo.EffectiveStartDate,
                        EffectiveEndDate = bo.EffectiveEndDate,
                        AarFrom = bo.AarFrom,
                        AarTo = bo.AarTo,
                    });
                }
                count++;
            }

            return errors;
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(DiscountCode) &&
                string.IsNullOrEmpty(EffectiveStartDateStr) &&
                string.IsNullOrEmpty(EffectiveEndDateStr) &&
                string.IsNullOrEmpty(AarFromStr) &&
                string.IsNullOrEmpty(AarToStr) &&
                string.IsNullOrEmpty(DiscountStr);
        }

        public override string ToString()
        {
            return DiscountCode;
        }

        public static List<Column> GetColumns()
        {
            List<Column> Cols = new List<Column>
            {
                new Column
                {
                    Header = "ID",
                    ColIndex = TypeId,
                    Property = "Id",
                },
                new Column
                {
                    Header = "Discount Code",
                    ColIndex = TypeDiscountCode,
                    Property = "DiscountCode",
                },
                new Column
                {
                    Header = "Effective Start Date",
                    ColIndex = TypeEffectiveStartDate,
                    Property = "EffectiveStartDate",
                },
                new Column
                {
                    Header = "Effective End Date",
                    ColIndex = TypeEffectiveEndDate,
                    Property = "EffectiveEndDate",
                },
                new Column
                {
                    Header = "ORI Sum Assured From",
                    ColIndex = TypeAarFrom,
                    Property = "AarFrom",
                },
                new Column
                {
                    Header = "ORI Sum Assured To",
                    ColIndex = TypeAarTo,
                    Property = "AarTo",
                },
                new Column
                {
                    Header = "Discount",
                    ColIndex = TypeDiscount,
                    Property = "Discount",
                },
            };

            return Cols;
        }
    }
}
