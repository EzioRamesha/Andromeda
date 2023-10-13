using Shared;
using System;
using System.Web.Optimization;

namespace WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(
                new ScriptBundle("~/bundles/bootstrap").
                Include(
                    //"~/Scripts/bootstrap.js",
                    "~/Scripts/bootstrap.bundle.min.js",
                    "~/Scripts/pace.min.js",
                    "~/Scripts/perfect-scrollbar.min.js",
                    "~/Scripts/coreui.min.js",
                    "~/Scripts/Charts.js",
                    "~/Scripts/bootstrap-datepicker.js",
                    "~/Scripts/moment.min.js",
                    "~/Scripts/utils.js",
                    "~/Scripts/jquery-ui.min.js",
                    "~/Scripts/bootstrap-tokenfield.min.js",
                    "~/Scripts/bootstrap-select.min.js"
                )
            );

            bool vueDebug = Boolean.Parse(Util.GetConfig("VueDebug"));
            bundles.Add(
                new ScriptBundle("~/bundles/vue").
                Include(
                    vueDebug ? "~/Scripts/vue/vue.js" : "~/Scripts/vue/vue.min.js"
                )
            );

            bundles.Add(
                new ScriptBundle("~/bundles/fullcalendar").
                Include(
                    "~/Scripts/fullcalendar/main.min.js",
                    "~/Scripts/fullcalendar/locales-all.min.js"
                )
            );

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/PagedList.css",
                      "~/Content/coreui.min.css",
                      "~/Content/bootstrap-datepicker.css",                      
                      "~/Content/jquery-ui.min.css",
                      "~/Content/bootstrap-tokenfield.min.css",
                      "~/Content/bootstrap-select.min.css",
                      "~/Content/fullcalendar/main.min.css",
                      "~/Content/site.css"
                    ));
        }
    }
}
