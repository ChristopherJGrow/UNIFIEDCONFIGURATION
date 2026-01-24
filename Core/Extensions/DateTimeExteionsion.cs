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
    public static class DateTimeExteionsion
    {
        public static string ToStringSortable(this DateTime date)
        {
            var aDate = string.Format("{0:D4}/{1:D2}/{2:D2}", date.Year, date.Month, date.Day);
            var aTime = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", date.Hour, date.Minute, date.Second, date.Millisecond);

            return string.Format("{0}-{1}", aDate, aTime);
        }
    }

}
