using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Typespot.Models
{
  public class UserViewModel
  {
    public string Id { get; set; }
    [Required]
    public string FullName { get; set; }
    [Required]
    public string Email { get; set; }
    public Boolean Deleted { get; set; }
    public Boolean Active { get; set; }
    public string ImageUrl { get; set; }
    public DateTime? LastEntry { get; set; }

    public int ReportsCount { get; set; }

    public string PhoneNumer { get; set; }
  }

  public class UserIndexViewModel
  {
    public ICollection<ReportsByDay> Reports { get; set; }
    public int TotalReports { get; set; }
    public Models.ApplicationUser User { get; set; }
  }

  public class ReportsByDay
  {
    public DateTime Date { get; set; }
    public List<Report> Reports { get; set; }
  }
}