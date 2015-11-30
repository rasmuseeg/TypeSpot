using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Typespot.Models
{
  public class PropertyValue
  {
    [Key]
    public Guid Id { get; set; }

    [Key]
    [Required]
    [MaxLength(20)]
    [Display(Name="Navn")]
    public string Name { get; set; }

    [Display(Name="Titel")]
    public string Title { get; set; }

    [Display(Name="Værdi")]
    public string Value { get; set; }

    [Display(Name="Type")]
    public string PropertyType { get; set; }

    [AllowHtml]
    [DataType(DataType.MultilineText)]
    [Display(Name="Beskrivelse")]
    public string Description { get; set; }

    //private string _RawValue;

    //public T Value {
    //    get {
    //      switch(PVType){
    //        case PVTypes.DateTime:
    //            return Convert.ToDateTime(_RawValue);
    //            break;
    //        case PVTypes.Double:
    //            return Convert.ToDouble(_RawValue);
    //            break;
    //        case PVTypes.Decimal:
    //            return Convert.ToDecimal(_RawValue);
    //            break;
    //        case PVTypes.Boolean:
    //            return Convert.ToBoolean(_RawValue);
    //            break;
    //        default:
    //           return _RawValue;
    //           break;
    //      }
    //    }
    //    set {
    //        _RawValue = value;
    //    }
    //}
  }

  //public enum PVTypes
  //{
  //    DateTime = "System.DateTime",
  //    Double = "System.Double",
  //    Boolean = "System.Boolean",
  //    Decimal = "System.Decimal",
  //    String = "System.String",
  //    Float = "System.Float"
  //}
}