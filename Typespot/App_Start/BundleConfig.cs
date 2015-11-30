using ClientDependency.Core;
using System.Web;
using System.Web.Optimization;

namespace Typespot
{
  public class BundleConfig
  {

    public static void RegisterBundles()
    {
      BundleManager.CreateCssBundle("Bootstrap",
                new CssFile("~/Content/css/site.css")
                 );

      BundleManager.CreateJsBundle("jquery", 1,
          new JavascriptFile("~/Scripts/lib/jquery/jquery-1.10.2.js"),
          new JavascriptFile("~/Scripts/lib/jquery/jquery.validate.js"),
          new JavascriptFile("~/Scripts/lib/jquery/jquery.validate.unobtrusive.bootstrap.js"),
          new JavascriptFile("~/Scripts/lib/jquery/jquery.hotkeys.js")
       );

      BundleManager.CreateJsBundle("bootstrap", 3,
          new JavascriptFile("~/Scripts/lib/bootstrap/modernizr-2.6.2.js"),
          new JavascriptFile("~/Scripts/lib/bootstrap/bootstrap.min.js"),
          new JavascriptFile("~/Scripts/lib/bootstrap/respond.min.js"),
          new JavascriptFile("~/Scripts/lib/moment/moment-with-locales.min.js")
          );

      BundleManager.CreateJsBundle("angular", 2,
          new JavascriptFile("~/Scripts/lib/anuglar/angular.js"),
          new JavascriptFile("~/Scripts/lib/anuglar/angular-animate.js"),
          new JavascriptFile("~/Scripts/lib/anuglar/angular-sanitize.js")
          );

      BundleManager.CreateJsBundle("core", 4,
          new JavascriptFile("~/Scripts/lib/angular-ui/ui-bootstrap.js"),
          new JavascriptFile("~/Scripts/application.js")
      );
    }
  }
}
