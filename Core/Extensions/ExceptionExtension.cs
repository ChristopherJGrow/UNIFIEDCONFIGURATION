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
    public static class ExceptionExtension
    {
        //public static string ToStringAlt(this Exception oEx)
        //{
        //    return oEx.ToStringAlt(true);
        //}

        //public static string ToStringFull(this Exception oEx)
        //{
        //    return oEx.ToStringFull( true );
        //}

        public static string ToStringFull(this Exception oEx, bool bDisplayCallStack=true)
        {
            return oEx.ToStringFull(bDisplayCallStack);
           
        }

        /// <summary>
        /// The first printed exception will be where the error occured 
        /// Followed by each following catch throw
        /// </summary>
        /// <param name="oEx"></param>
        /// <param name="bDisplayCallStack"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        public static string ToStringFull(this Exception oEx, bool bDisplayCallStack, StringBuilder sb=null)
        {            
            StringBuilder retval =  sb==null ? new StringBuilder() : sb; // this makes sure we dont allocate a new builder for each recurse

            
            if ( oEx.InnerException != null && bDisplayCallStack )
            {
                retval.AppendFormat( oEx.InnerException.ToStringFull( bDisplayCallStack ,sb) );
                retval.Append( "\r\n\r\nFollowed By\r\n\r\n" );

            }

            retval.AppendLine( oEx.Message );     
            if (oEx.StackTrace!=null)       
                retval.Append( oEx.StackTrace.ToString() );

            return retval.ToString();
        }

        public static void TestNestedExcpetion()
        {
            try
            {
                try
                {
                    string str =null;

                    var size = str.Length;
                }
                catch ( Exception ex )
                {
                    throw new Exception( "Exception Message Inner", ex );
                }

            }
            catch ( Exception ex )
            {
                throw new Exception( "Exception Message From Outer", ex );
            }
        }
    }
}


