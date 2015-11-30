using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Typespot.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading;
using System.Globalization;

namespace Typespot.Controllers
{
    public abstract class BaseController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        public ApplicationUserManager _userManager;
        public static IDictionary<string, PropertyValue> SiteConfig;

        public BaseController()
        {
            SiteConfig = db.Settings.ToDictionary(p=>p.Name);
        }

        public BaseController ( ApplicationUserManager userManager )
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        protected virtual new CustomPrincipal User
        {
            get { return HttpContext.User as CustomPrincipal; }
        }

        protected override void OnActionExecuted ( ActionExecutedContext filterContext )
        {
            //if( currentUser != null ) //user your user object
            //{
            //    Thread.CurrentThread.CurrentCulture = new CultureInfo( currentUser.Culture );
            //    Thread.CurrentThread.CurrentUICulture = new CultureInfo( currentUser.Culture );
            //}
            //else
            //{
                Thread.CurrentThread.CurrentCulture = new CultureInfo( "da-DK" );
                Thread.CurrentThread.CurrentUICulture = new CultureInfo( "da-DK" );
            //}
            ViewBag.SiteConfig = SiteConfig;
        }
    }
}