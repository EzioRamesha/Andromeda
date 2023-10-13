using Shared;
using Shared.ProcessFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class GroupDiscountBo
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

        public int GroupSizeFrom { get; set; }

        public string GroupSizeFromStr { get; set; }

        public int GroupSizeTo { get; set; }

        public string GroupSizeToStr { get; set; }

        public double Discount { get; set; }

        public string DiscountStr { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public const string DisplayName = "Group Size Discount";

        public const int TypeId = 1;
        public const int TypeDiscountCode = 2;
        public const int TypeEffectiveStartDate = 3;
        public const int TypeEffectiveEndDate = 4;
        public const int TypeGroupSizeFrom = 5;
        public const int TypeGroupSizeTo = 6;
        public const int TypeDiscount = 7;

        public List<string> Validate()
        {
            List<string> errors = new List<string> { };
            DateTime? dateFrom = null;
            DateTime? dateTo = null;
            int? groupSizeFrom = null;
            int? groupSizeTo = null;

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

            if (string.IsNullOrEmpty(GroupSizeFromStr))
                errors.Add(string.Format(MessageBag.Required, "Group Size From"));
            else
                groupSizeFrom = Util.GetParseInt(GroupSizeFromStr);

            if (string.IsNullOrEmpty(GroupSizeToStr))
                errors.Add(string.Format(MessageBag.Required, "Group Size To"));
            else
                groupSizeTo = Util.GetParseInt(GroupSizeToStr);

            if (groupSizeFrom.HasValue && groupSizeTo.HasValue && groupSizeTo < groupSizeFrom)
            {
                errors.Add(string.Format(MessageBag.GreaterOrEqualTo, "Group Size To", "Group Size From"));
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

        public static List<string> ValidateDuplicate(IList<GroupDiscountBo> bos)
        {
            List<string> errors = new List<string> { };
            List<GroupDiscountBo> groupDiscountBos = new List<GroupDiscountBo> { };
            int count = 1;

            foreach (var bo in bos)
            {
                if (count == 1)
                {
                    groupDiscountBos.Add(new GroupDiscountBo
                    {
                        DiscountCodeToLower = bo.DiscountCodeToLower,
                        EffectiveStartDate = bo.EffectiveStartDate,
                        EffectiveEndDate = bo.EffectiveEndDate,
                        GroupSizeFrom = bo.GroupSizeFrom,
                        GroupSizeTo = bo.GroupSizeTo,
                    });
                }
                else
                {
                    var query = groupDiscountBos
                        .Where(q => q.DiscountCodeToLower == bo.DiscountCodeToLower)
                        .Where(q =>
                            (
                                q.GroupSizeFrom <= bo.GroupSizeFrom
                                && q.GroupSizeTo >= bo.GroupSizeFrom
                                ||
                                q.GroupSizeFrom <= bo.GroupSizeTo
                                && q.GroupSizeTo >= bo.GroupSizeTo
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

                    groupDiscountBos.Add(new GroupDiscountBo
                    {
                        DiscountCodeToLower = bo.DiscountCodeToLower,
                        EffectiveStartDate = bo.EffectiveStartDate,
                        EffectiveEndDate = bo.EffectiveEndDate,
                        GroupSizeFrom = bo.GroupSizeFrom,
                        GroupSizeTo = bo.GroupSizeTo,
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
                string.IsNullOrEmpty(GroupSizeFromStr) &&
                string.IsNullOrEmpty(GroupSizeToStr) &&
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
                    Header = "Group Size From",
                    ColIndex = TypeGroupSizeFrom,
                    Property = "GroupSizeFrom",
                },
                new Column
                {
                    Header = "Group Size To",
                    ColIndex = TypeGroupSizeTo,
                    Property = "GroupSizeTo",
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
