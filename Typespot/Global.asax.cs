using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using ClientDependency.Core;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Typespot.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Typespot
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        public ApplicationUserManager _userManager;

        /// <summary>
        /// Constructors
        /// </summary>
        public MvcApplication () { }
        public MvcApplication ( ApplicationUserManager userManager )
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? this.Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        protected void Application_Start ()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters( GlobalFilters.Filters );
            RouteConfig.RegisterRoutes( RouteTable.Routes );
            //BundleConfig.RegisterBundles( BundleTable.Bundles );
            BundleConfig.RegisterBundles();

            //AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Email;
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            //List<Claim> _claims = new List<Claim>();
            //_claims.AddRange(new List<Claim>
            //{
            //    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", _user.Email)),
            //    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", _user.Email)
            //});

        }

        protected void Application_PostAuthenticateRequest ( Object sender, EventArgs e )
        {
            if( HttpContext.Current.User.Identity.IsAuthenticated )
            {
                ApplicationUser user = UserManager.FindByName( HttpContext.Current.User.Identity.Name );

                CustomPrincipal newUser = new CustomPrincipal( user );
                //newUser.Identity = User.Identity;
                //Claim cPatient = new Claim(typeof(PatientPortalPrincipal).ToString(), );

                HttpContext.Current.User = newUser;
            }
        }

        //protected void Application_AuthenticateRequest ( Object sender, EventArgs e )
        //{
        //    if( Request.IsAuthenticated )
        //    {
        //        string username = HttpContext.Current.User.Identity.Name;
        //        var identity = new MyIdentity( username, true );
        //        var principal = new MyPrincipal( identity, identity.Roles );
        //        HttpContext.Current.User = principal;
        //    }
        //}
    }
}
