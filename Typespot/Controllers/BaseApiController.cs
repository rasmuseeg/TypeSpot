using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Typespot.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading;
using System.Globalization;
using System.Web;


namespace Typespot.Controllers
{
    public class BaseApiController : ApiController
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        public ApplicationUserManager _userManager;
        public static IDictionary<string, PropertyValue> SiteConfig;

        public BaseApiController()
        {
        }

        public BaseApiController ( ApplicationUserManager userManager )
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
    }
}
