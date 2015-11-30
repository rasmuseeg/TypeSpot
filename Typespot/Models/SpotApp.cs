using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Typespot.Models
{
    public class SpotApp
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public ICollection<AppGroup> Groups { get; set; }
        public ICollection<AppLookup> Lookups { get; set; }
    }
    public class AppGroup
    {
        public Guid Id { get; set; }

        public string Picture { get; set; }
        public ICollection<GroupOption> Options { get; set; }
    }

    public class AppLookup
    {
        [Key]
        public Guid Id { get; set; }
        public Guid SpotAppId { get; set; }

        [ForeignKey("SpotAppId")]
        public virtual SpotApp SpotApp { get; set; }
        public ICollection<AppLookupOption> Options { get; set; }
    }

    public class AppLookupOption
    {
        public Guid Id { get; set; }
        public Guid SpotAppId { get; set; }
        public Guid GroupOptionId { get; set; }

        public virtual Guid SpotApp { get; set; }
        public virtual Guid GroupOption { get; set; }
    }

    public class GroupOption
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }

        public Guid AppGroupId { get; set; }
        public virtual AppGroup AppGroup { get; set; }
    }
}