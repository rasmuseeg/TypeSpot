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
using Microsoft.AspNet.Identity.EntityFramework;

namespace Typespot.Controllers
{
  [Authorize(Roles = "Superusers,Employees")]
  public class UsersController : BaseController
  {
    // GET: Users
    public async Task<ActionResult> Index()
    {
      //IEnumerable<ApplicationUser> model = await UserManager.Users.ToListAsync();

      return View();
    }

    // GET: Users
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
      IEnumerable<UserViewModel> model = (
          from user in await UserManager.Users.ToListAsync()
          select new UserViewModel()
          {
            Id = user.Id,
            ImageUrl = string.IsNullOrEmpty(user.ImageUrl) ? "//gravatar.com/avatar/" + user.GravatarHash + "?s=48" : user.ImageUrl + "?width=48&height=48",
            FullName = user.FullName,
            Deleted = false,
            Active = (!user.LockoutEnabled && user.EmailConfirmed),
            Email = user.Email,
            PhoneNumer = user.PhoneNumber,
            ReportsCount = (
                  from report in db.Reports
                  where report.UserId == user.Id
                  select report
              ).Count(),
            LastEntry = (
                  from report in db.Reports
                  where report.UserId == user.Id && report.Trashed == false
                  orderby report.CreateDate descending
                  select report
              ).Select(d => (DateTime?)d.CreateDate)
              .DefaultIfEmpty()
              .OrderByDescending(p => p.Value)
              .FirstOrDefault()
          }
      );

      return Json(model.ToDictionary(p => p.Id), JsonRequestBehavior.AllowGet);
    }

    // GET: Users/Details/5
    public async Task<ActionResult> Details(string id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ApplicationUser applicationUser = await UserManager.FindByIdAsync(id);
      var reports = await (
              from report in db.Reports.Include(p => p.Personality).Include(p => p.User)
              where report.UserId == id && !report.Trashed
              select report
          ).ToListAsync();

      if (applicationUser == null)
      {
        return HttpNotFound();
      }

      UserIndexViewModel model = new UserIndexViewModel()
      {
        Reports = (
              from report in reports
              where report.UserId == id && !report.Trashed
              let dt = report.CreateDate
              group report by report.CreateDate.Date into g
              orderby g.Key descending
              select new ReportsByDay
              {
                Date = g.Key,
                Reports = g.OrderByDescending(p => p.CreateDate).ToList()
              }
          ).ToList(),
        TotalReports = reports.Count(),
        User = applicationUser
      };

      return View(model);
    }

    // GET: Users/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: Users/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(RegisterViewModel model)
    {
      if (model.GeneratePassword || ModelState.IsValid)
      {
        Boolean exist = await db.Users.AnyAsync(p => p.Email == model.Email);
        if (!exist)
        {
          var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.FullName };

          if (String.IsNullOrEmpty(model.Password))
          {
            model.Password = await UserManager.GenerateRandomPasswordAsync();
          }

          var result = await UserManager.CreateAsync(user, model.Password);
          if (result.Succeeded)
          {
            //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            // Send an email with this link
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

            // First user to register should be Superuser!
            if (UserManager.Users.Count() == 0)
            {
              await UserManager.AddToRoleAsync(user.Id, "Superusers");
            }

            await UserManager.AddToRoleAsync(user.Id, "Employees");

            ViewBag.Invited = true;

            return RedirectToAction("Index", "Users");
          }

          AddErrors(result);
        }
        else
        {
          // Logic for if user exist
          ModelState.AddModelError("", "Der er allerede en bruger med denne email i vores system.");
        }
      }

      // If we got this far, something failed, redisplay form
      return View(model);
    }

    // GET: Users/Edit/5
    [Authorize(Roles = "Employees,Superusers")]
    public async Task<ActionResult> Edit(string id)
    {
      string currentUserId = User.Identity.GetUserId();

      if (!User.IsInRole("Superusers") && currentUserId != id)
      {
        return RedirectToAction("Index");
      }
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ApplicationUser applicationUser = await UserManager.FindByIdAsync(id);

      if (applicationUser == null)
      {
        return HttpNotFound();
      }

      ViewBag.PersonalityId = new SelectList(await db.Personalities.ToListAsync(), "Id", "Name");
      ViewBag.Roles = await db.Roles.ToListAsync();

      return View(applicationUser);
    }

    // POST: Users/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Employees,Superusers")]
    public async Task<ActionResult> Edit([Bind(Include = "Id,FullName,PersonalityId,UserName,Email,EmailConfirmed,PhoneNumber,LockoutEnabled,ImageUrl,File")] ApplicationUser model, ICollection<string> roles)
    {
      if (ModelState.IsValid)
      {
        // Upload new profile picture
        //var helper = UploadHelper<model>
        model.UploadFile();

        db.Entry(model).State = EntityState.Unchanged;
        db.Entry(model).Property(p => p.FullName).IsModified = true;
        db.Entry(model).Property(p => p.PersonalityId).IsModified = true;
        db.Entry(model).Property(p => p.Email).IsModified = true;
        db.Entry(model).Property(p => p.UserName).IsModified = true;
        db.Entry(model).Property(p => p.EmailConfirmed).IsModified = true;
        db.Entry(model).Property(p => p.PhoneNumber).IsModified = true;
        db.Entry(model).Property(p => p.LockoutEnabled).IsModified = true;
        db.Entry(model).Property(p => p.ImageUrl).IsModified = true;

        await db.SaveChangesAsync();

        // Update roles if superuser was editing
        if (User.IsInRole("Superusers"))
        {
          await UpdateRoles(model.Id, roles);
        }

        return RedirectToAction("Details", new { id = model.Id });
      }
      return View(model);
    }

    // GET: Users/Delete/5
    public async Task<ActionResult> Delete(string id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      ApplicationUser applicationUser = await UserManager.FindByIdAsync(id);
      if (applicationUser == null)
      {
        return HttpNotFound();
      }

      string avatar = string.IsNullOrEmpty(applicationUser.ImageUrl) ? "//gravatar.com/avatar/" + applicationUser.GravatarHash + "?s=48" : applicationUser.ImageUrl + "?width=48&height=48";
      var model = new UserViewModel()
      {
        Id = applicationUser.Id,
        Email = applicationUser.Email,
        FullName = applicationUser.FullName,
        ImageUrl = avatar,
        PhoneNumer = applicationUser.PhoneNumber,
        ReportsCount = db.Reports.Where(p => p.UserId == applicationUser.Id).Count(),
        LastEntry = db.Reports.Where(p => p.UserId == applicationUser.Id)
          .OrderByDescending(p => p.CreateDate)
          .Select(d => (DateTime?)d.CreateDate)
          .DefaultIfEmpty().FirstOrDefault(),
      };

      return View(model);
    }

    // POST: Users/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(string id)
    {
      ApplicationUser applicationUser = await UserManager.FindByIdAsync(id);

      // Delete Roles
      string[] roles = applicationUser.Roles.Select(p => p.RoleId).ToArray();
      await UserManager.RemoveFromRolesAsync(applicationUser.Id, roles);

      // Delete Reports
      var reports = db.Reports.Where(p => p.UserId == applicationUser.Id);
      db.Reports.RemoveRange(reports);
      await db.SaveChangesAsync();

      await UserManager.DeleteAsync(applicationUser);
      //await db.SaveChangesAsync();
      return RedirectToAction("Index");
    }

    public async Task<ActionResult> AddToRole(string userId, string role)
    {
      if (userId == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      await UserManager.AddToRoleAsync(userId, role);
      await db.SaveChangesAsync();

      return RedirectToAction("Index");
    }

    public async Task<ActionResult> RemoveFromRole(string userId, string role)
    {
      if (userId == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      await UserManager.RemoveFromRoleAsync(userId, role);
      await db.SaveChangesAsync();

      return RedirectToAction("Index");
    }

    public async Task<ActionResult> UpdateRoles(string userId, ICollection<string> roles)
    {
      var user = await UserManager.FindByIdAsync(userId);
      IList<string> userRoles = UserManager.GetRoles(user.Id);
      var identityRoles = await db.Roles.ToListAsync();

      //TODO: Prevent user from deleting self.
      //TODO: Prevent from removing angel user from superusers.
      foreach (IdentityRole identityRole in identityRoles)
      {
        Boolean isInRole = userRoles.Contains(identityRole.Name);
        // Hvis x er i role og brugeren ikke rollen
        // ADD
        if (!isInRole && roles.Any(p => p == identityRole.Name))
        {
          await UserManager.AddToRoleAsync(user.Id, identityRole.Name);
        }
        // Hvis x ikke er i roles, og brugeren er i rolle
        // DELETE
        else if (isInRole && !roles.Any(p => p == identityRole.Name))
        {
          await UserManager.RemoveFromRoleAsync(user.Id, identityRole.Name);
        }
        else
        {
          // Hvis x er i rolle og burgeren er i rolle
          // KEEP
        }
      }

      return new HttpStatusCodeResult(HttpStatusCode.Accepted);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        db.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Helpers
    private void AddErrors(IdentityResult result)
    {
      foreach (var error in result.Errors)
      {
        ModelState.AddModelError("", error);
      }
    }
    #endregion
  }
}
