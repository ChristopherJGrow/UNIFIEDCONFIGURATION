//--------------------------------------------------------------------
// © Copyright 1989-2025 Syndigo, LLC. - All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Syndigo, LLC.  Reproduction, disclosure or use without specific 
// written authorization from Syndigo, LLC. is prohibited.
// For more information see: http://www.syndigo.com
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
using Config.Core.Conversion;



namespace Config.Core.Extensions
{
    public static class DatarowExtensions
    {
        /// <summary>
        /// Returns the given field value  
        /// If the database is db null the default value is returned
        /// </summary>
        /// <param name="row">the row</param>
        /// <param name="fieldname">the fieldname you want</param>
        /// <param name="myDefault">default in case the field is dbnull (defines return type)</param>
        /// <returns></returns>
        public static T GetField<T>(this System.Data.DataRow row, string fieldname, T myDefault)
        {
            T retval = myDefault;

            if (row[fieldname] != DBNull.Value)
                retval = ConvertEx.From(row[fieldname],myDefault);

            return retval;
        }



        /// <summary>
        /// Returns the given field value  
        /// If the database is db null the default value is returned
        /// </summary>
        /// <param name="row">the row</param>
        /// <param name="fieldname">the fieldname you want</param>
        /// <param name="myDefault">default in case the field is dbnull (defines return type)</param>
        /// <returns></returns>
        public static T GetField<T>(this System.Data.DataRow row, int iFieldNum, T myDefault)
        {
            T retval = myDefault;

            if (row[iFieldNum] != DBNull.Value)
                retval = ConvertEx.From(row[iFieldNum],myDefault);

            return retval;
        }


        /// <summary>
        /// Returns the given field value  
        /// If the database is db null the default value is returned
        /// </summary>
        /// <param name="row">the row</param>
        /// <param name="fieldname">the fieldname you want</param>
        /// <param name="myDefault">default in case the field is dbnull (defines return type)</param>
        /// <returns></returns>
        public static T GetField<T>(this System.Data.DataRow row, System.Data.DataColumn dc, T myDefault)
        {
            T retval = myDefault;


            if (dc != null && row[dc] != DBNull.Value)
                retval = ConvertEx.From(row[dc],myDefault);


            return retval;
        }


    }

}
