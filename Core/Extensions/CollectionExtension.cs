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
    public static class CollectionExtension
    {
        /// <summary>
        /// Creates a string from the collection using a comma as the delimiter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>

        public static string ToDelimitedString<T>(this ICollection<T> collection)
        {
            return collection.ToDelimitedString(',');
        }

        /// <summary>
        /// Creates a string from the collection using the given delimiter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string ToDelimitedString<T>(this ICollection<T> collection, char delimiter)
        {
            int size = 0;
            foreach (var thing in collection)
            {
                size += thing.ToString().Length + 1;
            }

            StringBuilder sbr = new StringBuilder(size);
            bool bNeedDelimiter = false;
            foreach (var thing in collection)
            {
                if (bNeedDelimiter)
                    sbr.Append(delimiter);
                else
                    bNeedDelimiter = true;

                sbr.Append(thing.ToString());

            }
            return sbr.ToString();
        }

    }

   
}
