using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Typespot.Models
{
  // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
  public class ApplicationUser : IdentityUser
  {
    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    {
      // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
      var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
      // Add custom user claims here
      return userIdentity;
    }

    public ClaimsIdentity GenerateUserIdentity(UserManager<ApplicationUser> manager)
    {
      // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
      var userIdentity = manager.CreateIdentity(this, DefaultAuthenticationTypes.ApplicationCookie);
      // Add custom user claims here
      return userIdentity;
    }

    [DisplayName("Fulde navn")]
    public string FullName { get; set; }

    public Nullable<int> PersonalityId { get; set; }
    [ForeignKey("PersonalityId")]
    public Personality Personality { get; set; }

    [NotMapped]
    public string GravatarHash
    {
      get
      {
        // TODO:: Should save to database, and only create once.
        // step 1, calculate MD5 hash from input
        MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(this.Email.Trim());
        byte[] hash = md5.ComputeHash(inputBytes);

        // step 2, convert byte array to hex string
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
          sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString().ToLower();
      }
    }

    [NotMapped]
    [DataType("Image")]
    public HttpPostedFileBase File { get; set; }

    [DataType("Image")]
    [Display(Name = "Billede", Description = "Anbefalet 150x150. Maks 2mb")]
    public string ImageUrl { get; set; }

    private static string uploadPath = "/Content/Uploads/";

    public void UploadFile()
    {
      if (this.File != null && this.File.ContentLength > 0)
      {
        // Edit, delete first
        if (!string.IsNullOrEmpty(this.ImageUrl))
        {
          DeleteFile();
        }

        // Save new image
        string ext = Path.GetExtension(this.File.FileName);
        string fileName = this.Id + ext;
        string savePath = HttpContext.Current.Server.MapPath(uploadPath + fileName);
        FileInfo info = new FileInfo(savePath);
        if (!info.Directory.Exists)
        {
          info.Directory.Create();
        }
        this.File.SaveAs(savePath);
        this.ImageUrl = uploadPath + fileName;
      }
    }

    public void DeleteFile()
    {
      string fullPath = Path.GetFullPath(this.ImageUrl);
      FileInfo file = new FileInfo(fullPath);

      if (file.Exists)
      {
        file.Delete();
      }
    }
  }

  interface ICustomPrincipal : IPrincipal
  {
    string Id { get; set; }
    string FullName { get; set; }
    string Avatar { get; set; }
    string Gravatar { get; set; }
  }

  //public class CustomIdentity : I

  public class CustomPrincipal : ICustomPrincipal, IPrincipal
  {
    public ApplicationDbContext db = new ApplicationDbContext();
    private ApplicationUserManager _userManager;
    /// <summary>
    /// Constructors
    /// </summary>
    public CustomPrincipal() { }
    public CustomPrincipal(ApplicationUserManager userManager)
    {
      UserManager = userManager;
    }

    public ApplicationUserManager UserManager
    {
      get
      {
        return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
      }
      private set
      {
        _userManager = value;
      }
    }

    public CustomPrincipal(ApplicationUser user)
    {
      this.Identity = user.GenerateUserIdentity((ApplicationUserManager)UserManager);
      this.FullName = user.FullName;
      this.Avatar = user.ImageUrl;
      this.Id = user.Id;
      this.Gravatar = "//gravatar.com/avatar/" + user.GravatarHash;
      this.Roles = UserManager.GetRoles(user.Id);
    }

    /// <summary>
    /// Properties
    /// </summary>
    /// 
    public IIdentity Identity { get; private set; }
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Avatar { get; set; }
    public string Gravatar { get; set; }

    private IList<string> Roles { get; set; }
    public Boolean IsInRole(string role)
    {
      return this.Roles.Contains(role);
    }
  }

  public class CustomPrincipalSerializeModel
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Avatar { get; set; }
  }

  public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
  {
    public ApplicationDbContext()
        : base("DefaultConnection", throwIfV1Schema: false)
    {
    }

    public static ApplicationDbContext Create()
    {
      return new ApplicationDbContext();
    }

    public DbSet<Center> Centers { get; set; }
    public DbSet<Tonality> Tonalities { get; set; }
    public DbSet<HarmonicGroup> HarmonicGroups { get; set; }
    public DbSet<SocialStyle> SocialStyles { get; set; }
    public DbSet<Personality> Personalities { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<PropertyValue> Settings { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Identity tables
      modelBuilder.Entity<ApplicationUser>().ToTable("Users");
      modelBuilder.Entity<IdentityRole>().ToTable("Roles");
      modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
      modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
      modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");

      // Custom tabels
      modelBuilder.Entity<PropertyValue>().HasKey(r => new { r.Id });

    }

    //public System.Data.Entity.DbSet<Typespot.Models.ApplicationUser> ApplicationUsers { get; set; }
  }
}