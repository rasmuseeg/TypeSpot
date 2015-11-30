using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Typespot.Models;

namespace Typespot.Controllers
{
    [Authorize( Roles = "Superusers")]
    public class SocialStylesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SocialStyles
        public async Task<ActionResult> Index()
        {
            return View(await db.SocialStyles.ToListAsync());
        }

        // GET: SocialStyles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SocialStyle socialStyle = await db.SocialStyles.FindAsync(id);
            if (socialStyle == null)
            {
                return HttpNotFound();
            }
            return View(socialStyle);
        }

        // GET: SocialStyles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SocialStyles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Description,Url,File")] SocialStyle socialStyle)
        {
            if (ModelState.IsValid)
            {
                db.SocialStyles.Add(socialStyle);
                await db.SaveChangesAsync();
                socialStyle.UploadFile();
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(socialStyle);
        }

        // GET: SocialStyles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SocialStyle socialStyle = await db.SocialStyles.FindAsync(id);
            if (socialStyle == null)
            {
                return HttpNotFound();
            }
            return View(socialStyle);
        }

        // POST: SocialStyles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,Url,File")] SocialStyle socialStyle)
        {
            if (ModelState.IsValid)
            {
                socialStyle.UploadFile();
                db.Entry(socialStyle).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(socialStyle);
        }

        // GET: SocialStyles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SocialStyle socialStyle = await db.SocialStyles.FindAsync(id);
            if (socialStyle == null)
            {
                return HttpNotFound();
            }
            return View(socialStyle);
        }

        // POST: SocialStyles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SocialStyle socialStyle = await db.SocialStyles.FindAsync(id);
            db.SocialStyles.Remove(socialStyle);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
