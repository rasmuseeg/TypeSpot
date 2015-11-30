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
    public class HarmonicGroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HarmonicGroups
        public async Task<ActionResult> Index()
        {
            return View(await db.HarmonicGroups.ToListAsync());
        }

        // GET: HarmonicGroups/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HarmonicGroup harmonicGroup = await db.HarmonicGroups.FindAsync(id);
            if (harmonicGroup == null)
            {
                return HttpNotFound();
            }
            return View(harmonicGroup);
        }

        // GET: HarmonicGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HarmonicGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create ( [Bind( Include = "Id,Name,Description,Url,File" )] HarmonicGroup harmonicGroup )
        {
            if (ModelState.IsValid)
            {
                db.HarmonicGroups.Add(harmonicGroup);
                await db.SaveChangesAsync();
                harmonicGroup.UploadFile();
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(harmonicGroup);
        }

        // GET: HarmonicGroups/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HarmonicGroup harmonicGroup = await db.HarmonicGroups.FindAsync(id);
            if (harmonicGroup == null)
            {
                return HttpNotFound();
            }
            return View(harmonicGroup);
        }

        // POST: HarmonicGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit ( [Bind( Include = "Id,Name,Description,Url,File" )] HarmonicGroup harmonicGroup )
        {
            if (ModelState.IsValid)
            {
                harmonicGroup.UploadFile();
                db.Entry(harmonicGroup).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(harmonicGroup);
        }

        // GET: HarmonicGroups/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HarmonicGroup harmonicGroup = await db.HarmonicGroups.FindAsync(id);
            if (harmonicGroup == null)
            {
                return HttpNotFound();
            }
            return View(harmonicGroup);
        }

        // POST: HarmonicGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HarmonicGroup harmonicGroup = await db.HarmonicGroups.FindAsync(id);
            db.HarmonicGroups.Remove(harmonicGroup);
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
