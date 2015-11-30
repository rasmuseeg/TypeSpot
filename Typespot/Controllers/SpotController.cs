using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Typespot.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Typespot.Controllers
{
    [Authorize]
    public class SpotController : BaseController
    {
        public async Task<ActionResult> Index ()
        {
            var model = new HomeViewModel();
            model.Centers = await db.Centers.ToListAsync();
            model.Tonalities = await db.Tonalities.ToListAsync();
            model.HarmonicGroups = await db.HarmonicGroups.ToListAsync();
            model.SocialStyles = await db.SocialStyles.ToListAsync();
            model.Personalities = await db.Personalities.ToListAsync();
            
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> GetAll ()
        {
            var model = new HomeViewModel();
            model.Centers = db.Centers.ToList();
            model.Tonalities = db.Tonalities.ToList();
            model.HarmonicGroups = db.HarmonicGroups.ToList();
            model.SocialStyles = db.SocialStyles.ToList();
            model.Personalities = db.Personalities.ToList();

            string userId = User.Identity.GetUserId();

            var userReports = await (
                    from report in db.Reports
                    where report.UserId == userId 
                    &&    report.Trashed == false
                    orderby report.CreateDate descending
                    select report
                ).ToListAsync();

            model.Reports = userReports.Take(10).ToList();
            model.TotalReports = userReports.Count();
            model.TotalToday = userReports.Where(p=> DateTime.Today.Date <= p.CreateDate.Date).Count();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Index (Report model)
        {
            if(ModelState.IsValid){
                db.Reports.Add(model);
                await db.SaveChangesAsync();
            }

            ViewBag.CenterId = new SelectList( db.Centers, "Id", "Name" );
            ViewBag.HarmonicGroupId = new SelectList( db.HarmonicGroups, "Id", "Name" );
            ViewBag.SocialStyleId = new SelectList( db.SocialStyles, "Id", "Name" );
            ViewBag.TonalityId = new SelectList( db.Tonalities, "Id", "Name" );
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Create ( [Bind( Include = "Id,Customer,Message,PersonalityId" )] Report report )
        {

            if( ModelState.IsValid && User.Identity.IsAuthenticated )
            {
                var currentUser = await UserManager.FindByIdAsync( User.Identity.GetUserId() );

                report.UserId = User.Identity.GetUserId();
                report.CreateDate = DateTime.Now;
                db.Reports.Add( report );
                await db.SaveChangesAsync();

                return RedirectToAction( "Index" );
            }

            ViewBag.PersonalityId = new SelectList( db.Personalities, "Id", "Name", report.PersonalityId );
            return View( report );
        }

        
    }
}