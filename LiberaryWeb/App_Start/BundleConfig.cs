using System.Web;
using System.Web.Optimization;

namespace LibraryWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862C:\Users\Hem\Dropbox\Learning Projects\LiberaryWeb\LiberaryWeb\App_Start\BundleConfig.cs
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                "~/Scripts/js/kendo.all.min.js",
                // "~/Scripts/kendo/kendo.timezones.min.js", // uncomment if using the Scheduler
                "~/Scripts/js/kendo.aspnetmvc.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                       "~/Scripts/toastr.js"));
            bundles.Add(new StyleBundle("~/Content/kendo/css").Include(
               "~/Content/styles/kendo.common-bootstrap.min.css",
               "~/Content/styles/kendo.bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                     "~/Content/toastr.css"));

            bundles.IgnoreList.Clear();

        }
    }
}
