using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Typespot.Models
{
    public class Report
    {
        public int Id { get; set; }

        [DisplayName("Kunde")]
        public string Customer { get; set; }

        [DisplayName("Besked")]
        public string Message { get; set; }

        [DisplayName("Bruger")]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set;}

        [DisplayName("Oprettet")]
        public DateTime CreateDate { get;set;}

        [DisplayName("Sidst redigéret")]
        public DateTime? UpdateDate { get; set; }

        [DefaultValue(false)]
        [Display(Name="Papirkurv")]
        public Boolean Trashed { get; set; }

        [Required]
        [DisplayName("Typenavn")]
        public int PersonalityId { get; set; }
        public virtual Personality Personality { get; set; }
    }

    public class ReportsByUser
    {
        public ApplicationUser User { get; set; }
        public IEnumerable<Report> Reports { get; set; }
    }

    public class PagedReports
    {
        public int TotalReport { get; set; }
        public List<Report> MyProperty { get; set; }
    }
}