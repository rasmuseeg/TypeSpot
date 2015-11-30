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
    [Authorize( Roles= "Superusers" )]
    public class PersonalitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public async Task<ActionResult> GetSelectList ()
        {
            var model = await (
                from item in db.Personalities
                select new {
                    Id = item.Id,
                    Name = item.Name,
                    Type = item.Type
                }
            ).ToListAsync();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // GET: Personalities
        public async Task<ActionResult> Index()
        {
            var personalities = db.Personalities.Include(p => p.Center).Include(p => p.HarmonicGroup).Include(p => p.SocialStyle).Include(p => p.Tonality);
            return View(await personalities.ToListAsync());
        }

        // GET: Personalities/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personality personality = await db.Personalities.FindAsync(id);
            if (personality == null)
            {
                return HttpNotFound();
            }
            return View(personality);
        }

        // GET: Personalities/Create
        public ActionResult Create()
        {
            ViewBag.CenterId = new SelectList(db.Centers, "Id", "Name");
            ViewBag.HarmonicGroupId = new SelectList(db.HarmonicGroups, "Id", "Name");
            ViewBag.SocialStyleId = new SelectList(db.SocialStyles, "Id", "Name");
            ViewBag.TonalityId = new SelectList(db.Tonalities, "Id", "Name");
            return View();
        }

        // POST: Personalities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Type,Name,Description,CenterId,TonalityId,HarmonicGroupId,SocialStyleId")] Personality personality)
        {
            if (ModelState.IsValid)
            {
                db.Personalities.Add(personality);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CenterId = new SelectList(db.Centers, "Id", "Name", personality.CenterId);
            ViewBag.HarmonicGroupId = new SelectList(db.HarmonicGroups, "Id", "Name", personality.HarmonicGroupId);
            ViewBag.SocialStyleId = new SelectList(db.SocialStyles, "Id", "Name", personality.SocialStyleId);
            ViewBag.TonalityId = new SelectList(db.Tonalities, "Id", "Name", personality.TonalityId);
            return View(personality);
        }

        // GET: Personalities/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personality personality = await db.Personalities.FindAsync(id);
            if (personality == null)
            {
                return HttpNotFound();
            }
            ViewBag.CenterId = new SelectList(db.Centers, "Id", "Name", personality.CenterId);
            ViewBag.HarmonicGroupId = new SelectList(db.HarmonicGroups, "Id", "Name", personality.HarmonicGroupId);
            ViewBag.SocialStyleId = new SelectList(db.SocialStyles, "Id", "Name", personality.SocialStyleId);
            ViewBag.TonalityId = new SelectList(db.Tonalities, "Id", "Name", personality.TonalityId);
            return View(personality);
        }

        // POST: Personalities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit ( [Bind( Include = "Id,Type,Name,Description,CenterId,TonalityId,HarmonicGroupId,SocialStyleId" )] Personality personality )
        {
            if (ModelState.IsValid)
            {
                db.Entry(personality).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CenterId = new SelectList(db.Centers, "Id", "Name", personality.CenterId);
            ViewBag.HarmonicGroupId = new SelectList(db.HarmonicGroups, "Id", "Name", personality.HarmonicGroupId);
            ViewBag.SocialStyleId = new SelectList(db.SocialStyles, "Id", "Name", personality.SocialStyleId);
            ViewBag.TonalityId = new SelectList(db.Tonalities, "Id", "Name", personality.TonalityId);
            return View(personality);
        }

        // GET: Personalities/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personality personality = await db.Personalities.FindAsync(id);
            if (personality == null)
            {
                return HttpNotFound();
            }
            return View(personality);
        }

        // POST: Personalities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Personality personality = await db.Personalities.FindAsync(id);
            db.Personalities.Remove(personality);
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
