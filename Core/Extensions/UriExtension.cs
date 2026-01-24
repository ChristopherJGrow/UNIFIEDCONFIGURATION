//--------------------------------------------------------------------
// © Copyright 1989-2014 Edgenet, Inc. - All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Edgenet, Inc.  Reproduction, disclosure or use without specific 
// written authorization from Edgenet, Inc. is prohibited.
// For more information see: http://www.edgenet.com
//--------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
//using System.Windows.Forms;

using System.Web;



using System.ComponentModel;

using System.Net;
using System.Text;

using System.Data;
using System.Xml;
using System.Linq;







namespace Config.Core.Extensions
{
    public static class UriExtension
    {

        /// <summary>
        /// Replaces one of the paramters in a Uri
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static Uri QueryChangeParam(this Uri uri, string key, string val)
        {
           
            var qs = HttpUtility.ParseQueryString(uri.Query);

            
            qs[key.Trim()] = val.Trim();
           
            var uriBuilder = new UriBuilder(uri);
            uriBuilder.Query = qs.ToString();
            var retval = uriBuilder.Uri;

            return retval;

        }
    }
}
