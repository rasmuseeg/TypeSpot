using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Typespot.Models
{
    [DisplayName( "Personality" )]
    public class Personality
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Type nummer")]
        public int Type { get; set; }

        [DisplayName("Navn")]
        public string Name { get; set; }
        [AllowHtml]
        [DataType( "wysiwyg" )]
        [DisplayName("Beskrivelse")]
        public string Description { get; set; }

        [DisplayName( "Center" )]
        public int CenterId { get; set; }
        [DisplayName( "Tonalitet" )]
        public int TonalityId { get; set; }
        [DisplayName( "Harmonisk gruppe, 1. forsvar" )]
        public int HarmonicGroupId { get; set; }
        [DisplayName( "Social stil, 2. forsvar" )]
        public int SocialStyleId { get; set; }

        [ForeignKey( "CenterId" )]
        public virtual Center Center { get; set; }
        [ForeignKey( "TonalityId" )]
        public virtual Tonality Tonality { get; set; }
        [ForeignKey( "HarmonicGroupId" )]
        public virtual HarmonicGroup HarmonicGroup { get; set; }
        [ForeignKey( "SocialStyleId" )]
        public virtual SocialStyle SocialStyle { get; set; }
    }

    public interface IStrategy
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string Url { get; set; }
        HttpPostedFileBase File { get; set; }
    }

    /// <summary>
    /// Center
    /// </summary>
    ///
    [DisplayName( "Center" )]
    public class Center : IStrategy
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [AllowHtml]
        [DataType( "wysiwyg" )]
        public string Description { get; set; }

        [UIHint("Image")]
        [DisplayName( "Image" )]
        public string Url { get; set; }
        [NotMapped]
        [DataType( "Image" )]
        public HttpPostedFileBase File { get; set; }
    }

    /// <summary>
    /// Tonalitet
    /// </summary>
    /// 
    [DisplayName("Tonalitet")]
    public class Tonality : IStrategy
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Navn")]
        public string Name { get; set; }

        [AllowHtml]
        [DataType( "wysiwyg" )]
        [DisplayName("Beskrivelse")]
        public string Description { get; set; }

        [UIHint( "Image" )]
        [DisplayName( "Image" )]
        public string Url { get; set; }

        [NotMapped]
        [DataType( "Image" )]
        public HttpPostedFileBase File { get; set; }
    }

    /// <summary>
    /// Harmonic group 1st Defense
    /// </summary>
    /// 
    [DisplayName( "Harmonic Group" )]
    public class HarmonicGroup : IStrategy
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Navn")]
        [Required]
        public string Name { get; set; }

        [AllowHtml]
        [DataType( "wysiwyg" )]
        [DisplayName("Beskrivelse")]
        public string Description { get; set; }

        [UIHint( "Image" )] // Display template
        [DisplayName( "Billede" )]
        public string Url { get; set; }

        [NotMapped]
        [DataType( "Image" )]
        public HttpPostedFileBase File { get; set; }
    }

    /// <summary>
    /// Social Style, 2nd defense
    /// </summary>
    /// 
    [DisplayName( "Social Style" )]
    public class SocialStyle : IStrategy
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [DisplayName("Navn")]
        public string Name { get; set; }

        [AllowHtml]
        [DataType( "wysiwyg" )]
        [DisplayName("Beskrivelse")]
        public string Description { get; set; }

        [UIHint( "Image" )]
        [DisplayName( "Billede" )]
        public string Url { get; set; }

        [NotMapped]
        [DataType( "Image" )]
        public HttpPostedFileBase File { get; set; }
    }


    public static class IStrategyExtensions
    {
        private static string uploadPath = "/Content/Uploads/";

        public static void UploadFile(this IStrategy model)
        {
            if( model.File != null && model.File.ContentLength > 0 )
            {
                // Edit, delete first
                if( !string.IsNullOrEmpty( model.Url ) )
                {
                    model.DeleteFile();
                }

                // Save new image
                string ext = Path.GetExtension( model.File.FileName );
                string fileName = Guid.NewGuid().ToString() + ext;
                string savePath = HttpContext.Current.Server.MapPath( uploadPath + fileName);
                FileInfo info = new FileInfo(savePath);
                if( !info.Directory.Exists )
                {
                    info.Directory.Create();
                }
                model.File.SaveAs( savePath );
                model.Url = uploadPath + fileName;
            }
        }

        public static void DeleteFile ( this IStrategy model )
        {
            string fullPath = Path.GetFullPath(model.Url);
            FileInfo file = new FileInfo(fullPath);
            if( file.Exists )
            {
                file.Delete();
            }
        }
    }
}