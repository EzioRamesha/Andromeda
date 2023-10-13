using PagedList.Mvc;
using System.ComponentModel;
using System.Web;

namespace Shared
{
    public class Html
    {
        public static object ViewBag { get; private set; }

        public static string Tag(string content)
        {
            return Tag(content);
        }

        public static string Tag(string content, string tag = "div", object htmlAttributes = null)
        {
            string strHtmlAttributes = Attributes(htmlAttributes);
            if (!string.IsNullOrEmpty(strHtmlAttributes))
            {
                return string.Format("<{0} {1}>{2}</{3}>", tag, strHtmlAttributes, content, tag);
            }
            return string.Format("<{0}>{1}</{2}>", tag, content, tag);
        }

        public static string Attributes(object htmlAttributes = null)
        {
            string strHtmlAttributes = "";
            if (htmlAttributes != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
                {
                    strHtmlAttributes += string.Format("{0}=\"{1}\" ", property.Name.Replace('_', '-'), property.GetValue(htmlAttributes));
                }
            }
            return strHtmlAttributes;
        }

        public static HtmlString IsChecked(bool isChecked)
        {
            if (isChecked)
                return new HtmlString(Tag("", "i", new { @class = "fas fa-check-circle text-success" }));

            return new HtmlString(Tag("", "i", new { @class = "fas fa-times-circle text-danger" }));
        }

        public static string GetSortAsc(string field)
        {
            return string.Format("{0}Asc", field);
        }

        public static string GetSortDsc(string field)
        {
            return string.Format("{0}Dsc", field);
        }

        public static HtmlString Sorter(dynamic viewBag, string name, string field, string url)
        {
            string sortIcon = Tag("", "i", new { @class = "fas fa-sort fa-lg", style = "opacity:0.8;color:#01509F;" });
            string sortIconUp = Tag("", "i", new { @class = "fas fa-sort-up fa-lg", style = "opacity:0.8;color:#01509F;" });
            string sortIconDown = Tag("", "i", new { @class = "fas fa-sort-down fa-lg", style = "opacity:0.8;color:#01509F;" });

            string asc = GetSortAsc(field);
            string dsc = GetSortDsc(field);

            string content = name + " ";
            if (viewBag.SortOrder == asc)
            {
                content += sortIconUp;
            }
            else if (viewBag.SortOrder == dsc)
            {
                content += sortIconDown;
            }
            else
            {
                content += sortIcon;
            }

            return new HtmlString(Tag(content, "a", new { href = url }));
        }

        public static HtmlString SorterTab1(dynamic viewBag, string name, string field, string url)
        {
            string sortIcon = Tag("", "i", new { @class = "fas fa-sort fa-lg", style = "opacity:0.8;color:#01509F;" });
            string sortIconUp = Tag("", "i", new { @class = "fas fa-sort-up fa-lg", style = "opacity:0.8;color:#01509F;" });
            string sortIconDown = Tag("", "i", new { @class = "fas fa-sort-down fa-lg", style = "opacity:0.8;color:#01509F;" });

            string asc = GetSortAsc(field);
            string dsc = GetSortDsc(field);

            string content = name + " ";
            if (viewBag.SortOrderTab1 == asc)
            {
                content += sortIconUp;
            }
            else if (viewBag.SortOrderTab1 == dsc)
            {
                content += sortIconDown;
            }
            else
            {
                content += sortIcon;
            }

            return new HtmlString(Tag(content, "a", new { href = url }));
        }

        public static PagedListRenderOptions GetPagedListRenderOptions()
        {
            return new PagedListRenderOptions { LinkToPreviousPageFormat = "Prev", LinkToNextPageFormat = "Next" };
        }
    }
}
