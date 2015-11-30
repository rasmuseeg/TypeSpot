using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Typespot.Models
{
    public class Setting
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }
}