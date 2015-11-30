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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Typespot.Controllers
{
  [Authorize]
  public class ReportsController : BaseController
  {
    // GET: Reports
    public async Task<ActionResult> Index()
    {
      // TODO: Display Triaderne ud på listen
      // Sortering på antal eller brugernavn (ascending/decending)

      //if( User.IsInRole( "Superusers" ) )
      //{
      //    IEnumerable<ReportsByUser> model = (
      //        from report in db.Reports.Include( r => r.Personality )
      //        group report by report.User into grps
      //        select new ReportsByUser
      //        {
      //            User = grps.FirstOrDefault().User,
      //            Reports = (
      //                from r in grps
      //                orderby r.CreateDate descending
      //                select r
      //            )
      //        }
      //    ).ToList();

      //    return View( model );
      //}

      //string userId = User.Identity.GetUserId();
      //var employeeReports = new List<ReportsByUser>(){
      //    new ReportsByUser(){
      //        User = UserManager.FindById(userId),
      //        Reports = (
      //            from report in db.Reports
      //            where report.UserId == userId
      //            orderby report.CreateDate descending
      //            select report
      //        )
      //    }
      //}.ToList();
      var result = await Task.FromResult(0);
      return View();
    }

    // GET: Reports/Details/5
    public async Task<ActionResult> Details(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Report report = await db.Reports.FindAsync(id);
      if (report == null)
      {
        return HttpNotFound();
      }
      return View(report);
    }

    // GET: Reports/Create
    public ActionResult Create()
    {
      ViewBag.PersonalityId = new SelectList(db.Personalities, "Id", "Name");
      return View();
    }

    // POST: Reports/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<ActionResult> Create([Bind(Include = "Id,Customer,Message,PersonalityId")] Report report)
    {
      if (ModelState.IsValid && User.Identity.IsAuthenticated)
      {
        report.UserId = User.Identity.GetUserId();
        report.CreateDate = DateTime.Now;
        db.Reports.Add(report);
        await db.SaveChangesAsync();
        return RedirectToAction("Index");
      }

      ViewBag.PersonalityId = new SelectList(db.Personalities, "Id", "Name", report.PersonalityId);
      return View(report);
    }

    // GET: Reports/Edit/5
    public async Task<ActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Report report = await db.Reports.FindAsync(id);
      if (report == null)
      {
        return HttpNotFound();
      }
      report.UpdateDate = DateTime.Now;
      ViewBag.PersonalityId = new SelectList(db.Personalities, "Id", "Name", report.PersonalityId);
      return View(report);
    }

    // POST: Reports/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    public async Task<ActionResult> Edit([Bind(Include = "Id,Customer,Message,PersonalityId,UpdateDate")] Report report)
    {
      if (ModelState.IsValid)
      {
        //The conversion of a datetime2 data type to a datetime data type resulted in an out-of-range value.
        report.UpdateDate = DateTime.Now;

        db.Entry(report).State = EntityState.Unchanged;
        db.Entry(report).Property(p => p.Customer).IsModified = true;
        db.Entry(report).Property(p => p.Message).IsModified = true;
        db.Entry(report).Property(p => p.PersonalityId).IsModified = true;
        db.Entry(report).Property(p => p.UpdateDate).IsModified = true;
        await db.SaveChangesAsync();

        return RedirectToAction("Index");
      }
      ViewBag.PersonalityId = new SelectList(db.Personalities, "Id", "Name", report.PersonalityId);
      return View(report);
    }

    // GET: Reports/Delete/5
    public async Task<ActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Report report = await db.Reports.FindAsync(id);
      if (report == null)
      {
        return HttpNotFound();
      }
      return View(report);
    }

    // POST: Reports/Delete/5
    [HttpPost, ActionName("Delete")]
    public async Task<ActionResult> DeleteConfirmed(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Report report = await db.Reports.FindAsync(id);
      if (report == null)
      {
        return HttpNotFound();
      }
      db.Reports.Remove(report);
      await db.SaveChangesAsync();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<ActionResult> Trash(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Report report = await db.Reports.FindAsync(id);
      if (report == null)
      {
        return HttpNotFound();
      }
      report.Trashed = true;
      db.Entry(report).State = EntityState.Modified;
      await db.SaveChangesAsync();

      return new HttpStatusCodeResult(HttpStatusCode.Accepted);
    }

    [HttpPost]
    public async Task<ActionResult> Restore(int? id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Report report = await db.Reports.FindAsync(id);
      if (report == null)
      {
        return HttpNotFound();
      }
      report.Trashed = false;
      db.Entry(report).State = EntityState.Modified;
      await db.SaveChangesAsync();

      return new HttpStatusCodeResult(HttpStatusCode.Accepted);
    }

    [HttpGet]
    public async Task<ActionResult> GetUsers()
    {
      // Does user have permission?
      string userId = "";
      if (!User.IsInRole("Superusers"))
      {
        userId = User.Identity.GetUserId();
      }

      var users = await (
          from user in db.Users
          where (
              from report in db.Reports
              where report.UserId == user.Id
              select report
          ).Any()
          && (string.IsNullOrEmpty(userId) || user.Id == userId)
          select user
      ).ToListAsync();

      var model = (
          from user in users
          select new
          {
            FullName = user.FullName,
            UserId = user.Id,
            GravatarHash = user.GravatarHash,
            Count = (
                  from report in db.Reports
                  where report.UserId == user.Id
                  select report
              ).Count()
          }
      ).ToList();

      return Json(model, JsonRequestBehavior.AllowGet);
    }

    [HttpPost]
    public async Task<ActionResult> GetReports(DateTime dateFrom, DateTime dateTo)
    {
      var reports = await (
          from report in db.Reports.Include(p => p.Personality).Include(p => p.User)
          orderby report.CreateDate descending
          where
          (report.CreateDate >= dateFrom
          && report.CreateDate <= dateTo)
          select report
      ).ToListAsync();

      return Json(reports);
    }

    [HttpPost]
    public async Task<ActionResult> GetPagedReports(int itemsPerPage, int currentPage, DateTime dateFrom, DateTime dateTo, Boolean includeTrashed, string userId)
    {
      // Does user have permission? 
      Boolean isSuperuser = User.IsInRole("Superusers");
      if (!isSuperuser)
      {
        userId = User.Id;
      }

      var totalReports = await (
          from report in db.Reports.Include(p => p.Personality).Include(p => p.User)
          orderby report.CreateDate descending
          where (report.CreateDate >= dateFrom && report.CreateDate <= dateTo)
          && (string.IsNullOrEmpty(userId) || report.UserId == userId)
          // Show trashed items if not super user
          && ((includeTrashed && isSuperuser) || !report.Trashed)
          select report
      ).ToListAsync();

      var reports = totalReports.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage).ToList();
      int Count = totalReports.Count();

      return Json(new { Reports = reports, Count = Count });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dateFrom">Range from</param>
    /// <param name="dateTo">Range to</param>
    /// <param name="seperator">Default is ','</param>
    /// <returns>Filename (24-12-2015 til 28-12-2015).csv</returns>
    public async Task<ActionResult> Export(DateTime dateFrom, DateTime dateTo, string seperator = ",")
    {
      var reports = await (
          from report in db.Reports.Include(p => p.User).Include(p => p.Personality)
          orderby report.CreateDate descending
          where
          (report.CreateDate >= dateFrom
          && report.CreateDate <= dateTo)
          && report.Trashed == false
          select report
      ).ToListAsync();

      // Convert reports to csv file
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      System.Reflection.PropertyInfo[] properties = typeof(Report).GetProperties();

      //
      // Create headers
      sb.Append("#" + seperator);
      sb.Append("Type" + seperator);
      sb.Append("Besked" + seperator);
      sb.Append("Kunde" + seperator);
      sb.Append("Navn" + seperator);
      sb.Append("Oprettet" + seperator);
      sb.Append("\n");

      //
      // Create content
      foreach (var report in reports)
      {
        sb.Append(report.Personality.Type + seperator);
        sb.Append(report.Personality.Name + seperator);
        sb.Append(report.Message + seperator);
        sb.Append(report.Customer + seperator);
        sb.Append(report.User.FullName + seperator);
        sb.Append(report.CreateDate.ToString("dd-MM-yyyy HH:mm") + seperator);
        sb.Append("\n");
      }

      //
      // File name
      // append from and to dates to the file name
      string fileName = string.Format("Spots ({0} til {1})", dateFrom.ToString("dd-MM-yyyy"), dateTo.ToString("dd-MM-yyyy"));

      Response.AddHeader("Content-disposition", "attachment;filename=" + fileName + ".csv");
      return Content(sb.ToString(), "text/csv", System.Text.Encoding.Unicode);
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
