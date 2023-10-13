using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessObject
{
    public class RiDiscountBo
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

        public double? DurationFrom { get; set; }

        public string DurationFromStr { get; set; }

        public double? DurationTo { get; set; }

        public string DurationToStr { get; set; }

        public double Discount { get; set; }

        public string DiscountStr { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const string DisplayName = "RI Discount Rate";

        public const int TypeId = 1;
        public const int TypeDiscountCode = 2;
        public const int TypeEffectiveStartDate = 3;
        public const int TypeEffectiveEndDate = 4;
        public const int TypeDurationFrom = 5;
        public const int TypeDurationTo = 6;
        public const int TypeDiscount = 7;

        public List<string> Validate()
        {
            List<string> errors = new List<string> { };
            DateTime? dateFrom = null;
            DateTime? dateTo = null;

            if (string.IsNullOrEmpty(DiscountCode))
                errors.Add(string.Format(MessageBag.Required, "Discount Code"));

            bool dateError = false;

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

            bool durationError = false;

            double? durationFrom = Util.StringToDouble(DurationFromStr);
            double? durationTo = Util.StringToDouble(DurationToStr);

            if (!string.IsNullOrEmpty(DurationFromStr) && !durationFrom.HasValue)
            {
                errors.Add("Duration From format is not valid");
                durationError = true;
            }

            if (!string.IsNullOrEmpty(DurationToStr) && !durationTo.HasValue)
            {
                errors.Add("Duration To format is not valid");
                durationError = true;
            }

            if (!durationError)
            {
                if (durationFrom.HasValue && !durationTo.HasValue)
                {
                    errors.Add(string.Format(MessageBag.Required, "Duration To"));
                }
                else if (!durationFrom.HasValue && durationTo.HasValue)
                {
                    errors.Add(string.Format(MessageBag.Required, "Duration From"));
                }
                else if (durationFrom.HasValue && durationTo.HasValue && durationTo < durationFrom)
                {
                    errors.Add(string.Format(MessageBag.GreaterOrEqualTo, "Duration To", "Duration From"));
                }
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

        public static List<string> ValidateDuplicate(IList<RiDiscountBo> bos)
        {
            List<string> errors = new List<string> { };
            List<RiDiscountBo> riDiscountBos = new List<RiDiscountBo> { };
            int count = 1;

            foreach (var bo in bos)
            {
                if (count == 1)
                {
                    riDiscountBos.Add(new RiDiscountBo
                    {
                        DiscountCodeToLower = bo.DiscountCodeToLower,
                        EffectiveStartDate = bo.EffectiveStartDate,
                        EffectiveEndDate = bo.EffectiveEndDate,
                        DurationFrom = bo.DurationFrom,
                        DurationTo = bo.DurationTo,
                    });
                }
                else
                {
                    var query = riDiscountBos.Where(q => q.DiscountCodeToLower == bo.DiscountCodeToLower);

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

                    if (bo.DurationFrom.HasValue && bo.DurationTo.HasValue)
                    {
                        query = query
                            .Where(q =>
                            (
                                q.DurationFrom <= bo.DurationFrom
                                && q.DurationTo >= bo.DurationFrom
                                ||
                                q.DurationFrom <= bo.DurationTo
                                && q.DurationTo >= bo.DurationTo
                            )
                            || (!q.DurationFrom.HasValue && !q.DurationTo.HasValue)
                        );
                    }

                    if (query.Any())
                    {
                        int idx = bos.IndexOf(bo) + 1;
                        errors.Add("Duplicate Discount: {0} at # Row " + idx);
                    }

                    riDiscountBos.Add(new RiDiscountBo
                    {
                        DiscountCodeToLower = bo.DiscountCodeToLower,
                        EffectiveStartDate = bo.EffectiveStartDate,
                        EffectiveEndDate = bo.EffectiveEndDate,
                        DurationFrom = bo.DurationFrom,
                        DurationTo = bo.DurationTo,
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
                string.IsNullOrEmpty(DurationFromStr) &&
                string.IsNullOrEmpty(DurationToStr) &&
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
                    Header = "Duration From",
                    ColIndex = TypeDurationFrom,
                    Property = "DurationFrom",
                },
                new Column
                {
                    Header = "Duration To",
                    ColIndex = TypeDurationTo,
                    Property = "DurationTo",
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
