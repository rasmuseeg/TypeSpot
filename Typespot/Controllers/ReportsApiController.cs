using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Typespot.Models;

namespace Typespot.Controllers
{
    public class ReportsApiController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ReportsApi
        public IQueryable<Report> GetPagedReports()
        {
            return db.Reports;
        }

        // GET: api/ReportsApi/5
        [ResponseType(typeof(Report))]
        public async Task<IHttpActionResult> GetReport(int id)
        {
            Report report = await db.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            return Ok(report);
        }

        // PUT: api/ReportsApi/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutReport(int id, Report report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != report.Id)
            {
                return BadRequest();
            }

            db.Entry(report).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ReportsApi
        [ResponseType(typeof(Report))]
        public async Task<IHttpActionResult> PostReport(Report report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Reports.Add(report);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = report.Id }, report);
        }

        // DELETE: api/ReportsApi/5
        [ResponseType( typeof( Report ) )]
        public async Task<IHttpActionResult> Trash ( int id )
        {
            Report report = await db.Reports.FindAsync( id );
            if( report == null )
            {
                return NotFound();
            }
            report.Trashed = true;

            db.Entry(report).State = EntityState.Unchanged;
            db.Entry(report).Property(p=>p.Trashed).IsModified=true;
            await db.SaveChangesAsync();

            return Ok( report );
        }

        // DELETE: api/ReportsApi/5
        [ResponseType( typeof( Report ) )]
        public async Task<IHttpActionResult> Restore ( int id )
        {
            Report report = await db.Reports.FindAsync( id );
            if( report == null )
            {
                return NotFound();
            }
            report.Trashed = false;

            db.Entry( report ).State = EntityState.Unchanged;
            db.Entry( report ).Property( p => p.Trashed ).IsModified = true;
            await db.SaveChangesAsync();

            return Ok( report );
        }

        // DELETE: api/ReportsApi/5
        [ResponseType(typeof(Report))]
        public async Task<IHttpActionResult> DeleteReport(int id)
        {
            Report report = await db.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            db.Reports.Remove(report);
            await db.SaveChangesAsync();

            return Ok(report);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReportExists(int id)
        {
            return db.Reports.Count(e => e.Id == id) > 0;
        }
    }
}