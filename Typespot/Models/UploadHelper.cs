using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Typespot.Models
{
    public interface IUpload
    {
        HttpPostedFileBase File { get; set; }
        string Url { get; set; }
    }

    public static class UploadHelper<T> where T : IUpload
    {
        private static string uploadPath = "/Content/Uploads/";

        public static void UploadFile(T model)
        {
            if( model.File != null && model.File.ContentLength > 0 )
            {
                // Edit, delete first
                if( !string.IsNullOrEmpty( model.Url ) )
                {
                    DeleteFile(model);
                }

                // Save new image
                string ext = Path.GetExtension( model.File.FileName );
                string fileName = Guid.NewGuid().ToString() + ext;
                string savePath = HttpContext.Current.Server.MapPath( uploadPath + fileName );
                FileInfo info = new FileInfo( savePath );
                if( !info.Directory.Exists )
                {
                    info.Directory.Create();
                }
                model.File.SaveAs( savePath );
                model.Url = uploadPath + fileName;
            }
        }

        public static void DeleteFile (T model)
        {
            string fullPath = Path.GetFullPath( model.Url );
            FileInfo file = new FileInfo( fullPath );
            if( file.Exists )
            {
                file.Delete();
            }
        }
    }
}