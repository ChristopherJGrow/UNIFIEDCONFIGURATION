//--------------------------------------------------------------------
// © Copyright 1989-2017 Edgenet, Inc. - All rights reserved.
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
    /// <summary>
    /// Yet Another Monkey Patching class
    /// </summary>
    public static class DelegateExtension
    {
        /// <summary>
        /// Removes all subscribers for the delegate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="myDelegate"></param>
        /// <returns></returns>
        public static T RemoveAllEvents<T>(this T myDelegate) where T : class
        {
            return DelegateEx.RemoveAllEvents(myDelegate);
        }

        //public static T RunAsync<T>(this T myDelegate) where T : class
        //{
        //    return myDelegate;
        //}


    }
}
