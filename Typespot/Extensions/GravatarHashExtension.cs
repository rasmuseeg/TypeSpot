using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Security.Cryptography;
using System.Text;

namespace Typespot.Extensions
{
    public static class GravatarHashExtension
    {
        public static string MD5(string email){
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes( email.Trim() );
            byte[] hash = md5.ComputeHash( inputBytes );

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for( int i = 0; i < hash.Length; i++ )
            {
                sb.Append( hash[i].ToString( "X2" ) );
            }
            return sb.ToString().ToLower();
        }
    }
}