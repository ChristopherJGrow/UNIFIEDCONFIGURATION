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
using System.Runtime.CompilerServices;


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
    public static class ArrayExtension
    {
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void WipeDefault<T>(this T[] array) 
        {
            for(int cLoop=0;cLoop< array.Length; ++cLoop)            
            {
                array[cLoop] = default(T);
            }
        }

        /// <summary>
        /// Combines two arrays of the same type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrayFirst"></param>
        /// <param name="arraySecond"></param>
        /// <returns></returns>
        public static T[] Combine<T>(this T[] arrayFirst, T[] arraySecond)
        {
            T[] things = (T[])Array.CreateInstance(typeof(T), arrayFirst.Length + arraySecond.Length);

            arrayFirst.CopyTo(things, 0);

            arraySecond.CopyTo(things, arrayFirst.Length);

            return things;
        }

        /// <summary>
        /// Combines multiple arrays of the same types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrayFirst"></param>
        /// <param name="arrayOthers"></param>
        /// <returns></returns>
        public static T[] Combine<T>(this T[] arrayFirst, params T[][] arrayOthers)
        {
            int length = arrayFirst.Length;
            int offset = 0;
            int cLoop = 0;

            for (cLoop = 0; cLoop < arrayOthers.Length; cLoop++)
            {
                length += arrayOthers[cLoop].Length;
            }

            T[] things = (T[])Array.CreateInstance(typeof(T), length);

            arrayFirst.CopyTo(things, offset);
            offset += arrayFirst.Length;


            for (cLoop = 0; cLoop < arrayOthers.Length; cLoop++)
            {
                arrayOthers[cLoop].CopyTo(things, offset);
                offset += arrayOthers[cLoop].Length;
            }

            return things;
        }

      
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this Array array)
        {
            return array == null || array.Length == 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotNullOrEmpty(this Array array)
        {
            return !(array == null || array.Length == 0);
        }

        
    }
}
