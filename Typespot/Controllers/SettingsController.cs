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
    [Authorize(Roles="Superusers")]
    public class SettingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Settings
        public async Task<ActionResult> Index()
        {
            return View(await db.Settings.ToListAsync());
        }

        // GET: Settings/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PropertyValue propertyValue = await db.Settings.FindAsync(id);
            if (propertyValue == null)
            {
                return HttpNotFound();
            }
            return View(propertyValue);
        }

        // GET: Settings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Settings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Value,PropertyType")] PropertyValue propertyValue)
        {
            if (ModelState.IsValid)
            {
                propertyValue.Id = Guid.NewGuid();
                db.Settings.Add(propertyValue);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(propertyValue);
        }

        // GET: Settings/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PropertyValue propertyValue = await db.Settings.FindAsync(id);
            if (propertyValue == null)
            {
                return HttpNotFound();
            }
            return View(propertyValue);
        }

        // POST: Settings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Value,PropertyType")] PropertyValue propertyValue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(propertyValue).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(propertyValue);
        }

        // GET: Settings/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PropertyValue propertyValue = await db.Settings.FindAsync(id);
            if (propertyValue == null)
            {
                return HttpNotFound();
            }
            return View(propertyValue);
        }

        // POST: Settings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            PropertyValue propertyValue = await db.Settings.FindAsync(id);
            db.Settings.Remove(propertyValue);
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
