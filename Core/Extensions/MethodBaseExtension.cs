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


using System.Reflection;
using System.Threading;

using System.ComponentModel;

using System.Net;
using System.Text;

using System.Data;
using System.Xml;
using System.Linq;



namespace Config.Core.Extensions
{
    public static class MethodBaseExtension
    {
        /// <summary>
        /// Given the name of a method base returns the property name without the get_ or set_ 
        /// If called on a method will return MethodBase.Name
        /// </summary>
        /// <param name="methodBase"></param>
        /// <returns></returns>
        public static string ToPropertyName(this MethodBase methodBase)
        {

            var retval = methodBase.Name;

            // this compare is faster than the other options
            //
            if (string.CompareOrdinal(retval, 0, "set_", 0, 4) == 0 || string.CompareOrdinal(retval, 0, "get_", 0, 4) == 0)
                retval = retval.Substring(4);

            return retval;
        }
    }
}
