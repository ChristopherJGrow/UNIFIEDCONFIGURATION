//--------------------------------------------------------------------
// © Copyright 1989-2014 Edgenet, Inc. - All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Edgenet, Inc.  Reproduction, disclosure or use without specific 
// written authorization from Edgenet, Inc. is prohibited.
// For more information see: http://www.edgenet.com
//--------------------------------------------------------------------

using System;
using System.Drawing;
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
    public static class CultureInfoExtensions
    {

        /// <summary>
        /// Compares culture language
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="other">Culture to compare against</param>
        /// <returns></returns>
        public static bool IsSameLanguage(this System.Globalization.CultureInfo culture, System.Globalization.CultureInfo other)
        {
            bool bRetval = culture.Name.Left(2).Equals(other.Name.Left(2));

            return bRetval;
        }

        /// <summary>
        /// Compares culture language 
        /// For culture info and culture name
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="other">Name of the other culture</param>
        /// <returns></returns>
        public static bool IsSameLanguage(this System.Globalization.CultureInfo culture, string other)
        {
            bool bRetval = culture.Name.Left(2).Equals(other.Left(2));

            return bRetval;
        }


    }
}
