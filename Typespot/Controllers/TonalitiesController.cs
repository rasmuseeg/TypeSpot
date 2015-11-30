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
    [Authorize( Roles = "Superusers" )]
    public class TonalitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tonalities
        public async Task<ActionResult> Index()
        {
            return View(await db.Tonalities.ToListAsync());
        }

        // GET: Tonalities/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tonality tonality = await db.Tonalities.FindAsync(id);
            if (tonality == null)
            {
                return HttpNotFound();
            }
            return View(tonality);
        }

        // GET: Tonalities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tonalities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create ( [Bind( Include = "Id,Name,Description,Url,File" )] Tonality tonality )
        {
            if (ModelState.IsValid)
            {
                db.Tonalities.Add(tonality);
                await db.SaveChangesAsync();
                tonality.UploadFile();
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(tonality);
        }

        // GET: Tonalities/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tonality tonality = await db.Tonalities.FindAsync(id);
            if (tonality == null)
            {
                return HttpNotFound();
            }
            return View(tonality);
        }

        // POST: Tonalities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,Url,File")] Tonality tonality)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tonality).State = EntityState.Modified;
                tonality.UploadFile();
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tonality);
        }

        // GET: Tonalities/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tonality tonality = await db.Tonalities.FindAsync(id);
            if (tonality == null)
            {
                return HttpNotFound();
            }
            return View(tonality);
        }

        // POST: Tonalities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tonality tonality = await db.Tonalities.FindAsync(id);
            db.Tonalities.Remove(tonality);
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
