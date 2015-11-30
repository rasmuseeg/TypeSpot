using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Typespot.Models
{
    public class HomeViewModel
    {
        public int CenterId { get; set; }
        public int HarmonicGroupId { get; set; }
        public int SocialStyleId { get; set; }
        public int TonalityId { get; set; }

        public List<Center> Centers { get;set;}
        public List<HarmonicGroup> HarmonicGroups { get; set; }
        public List<SocialStyle> SocialStyles { get; set; }
        public List<Tonality> Tonalities { get; set; }

        public List<Personality> Personalities { get; set; }

        public List<Report> Reports { get;set; }
        public int TotalReports { get;set; }
        public int TotalToday { get; set; }
    }
}