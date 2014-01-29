using System.Web;
using System.Web.Optimization;

namespace GestionScrumV3
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();

            // JS

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapjs").Include(
                        "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapval").Include(
                        "~/Scripts/bootstrap.validate.js"));

            bundles.Add(new ScriptBundle("~/bundles/fullcalendar").Include(
                       "~/Scripts/fullcalendar.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/datepickerjs").Include(
                        "~/Scripts/bootstrap-datepicker.js",
                        "~/Scripts/bootstrap-datepicker.fr.js"));

            bundles.Add(new ScriptBundle("~/bundles/sliderjs").Include(
                        "~/Scripts/bootstrap-slider.js"));

            // CSS

            bundles.Add(new StyleBundle("~/Content/bootstrapcss").Include(
                        "~/Content/bootstrap.min.css",
                        "~/Content/bootstrap-theme.min.css"));

            bundles.Add(new StyleBundle("~/Content/sitecss").Include(
                        "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/fullcalendarcss").Include(
                        "~/Content/fullcalendar.css", 
                        "~/Content/fullcalendar.print.css"));

            bundles.Add(new StyleBundle("~/Content/datepickercss").Include(
                        "~/Content/datepicker3.css"));

            bundles.Add(new StyleBundle("~/Content/splidercss").Include(
                        "~/Content/slider.css"));
        }
    }
}