﻿using System.Web;
using System.Web.Optimization;

namespace УправлениеПроектами
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                           "~/Scripts/jquery-1.*"));


            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap*"));

            bundles.Add(new StyleBundle("~/Content/css")
                    .Include("~/Content/site.css")  /* не перепутайте порядок */
                    .Include("~/Content/bootstrap*"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                "~/Scripts/CustomScripts/common.js"));

            bundles.Add(new ScriptBundle("~/bundles/reservation").Include(
                "~/Scripts/CustomScripts/reservation.js"));

            // основной скриптовый файл приложения
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/CustomScripts/app.js"));
            // ExtJS
            bundles.Add(new ScriptBundle("~/bundles/ExtJS").Include(
              "~/Scripts/ExtJS/ext-all-debug.js"));
            bundles.Add(new StyleBundle("~/Content/css/ExtJS").Include(
             "~/Scripts/ExtJS/resources/css/ext-all.css"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-1.*"));

            bundles.Add(new StyleBundle("~/Content/css/jqueryui").Include(
                "~/Content/jquery-ui-1*"));

            bundles.Add(new ScriptBundle("~/bundles/jquerydev").Include("~/development-bundle/jquery-1.*"));
            bundles.Add(new ScriptBundle("~/bundles/jquerydevui").Include("~/development-bundle/ui/jquery-ui*"));
            bundles.Add(new ScriptBundle("~/bundles/jquerydevuidate").Include("~/development-bundle/ui/jquery.ui.datepicker*"));
            bundles.Add(new StyleBundle("~/Content/css/jquerydevui").Include("~/development-bundle/themes/ui-lightness/jquery-ui*"));
        }
    }
}