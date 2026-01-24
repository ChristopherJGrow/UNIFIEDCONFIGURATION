//--------------------------------------------------------------------
// © Copyright 1989-2025 Syndigo, LLC. - All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Syndigo, LLC.  Reproduction, disclosure or use without specific 
// written authorization from Syndigo, LLC. is prohibited.
// For more information see: http://www.syndigo.com
//--------------------------------------------------------------------

using Config.Core;
using Config.Core.Extensions;


using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Config.Core.Conversion
{
    /// <summary>
    /// Extended data conversion class
    /// </summary>
    /// 
    public class ConvertEx
    {

        //StringComparison.InvariantCultureIgnoreCase uses comparison rules based on english, but without any regional variations. 
        //  This is good for a neutral comparison that still takes into account some linguistic aspects.
        //  This is slower because of the extra rules applied for culture

        //StringComparison.OrdinalIgnoreCase compares the character codes without cultural aspects. 
        //  This is good for exact comparisons, like passwords, but not for sorting strings with unusual characters like é or ö. 
        //  This is also faster because there are no extra rules to apply before comparing.


        /// <summary>
        /// Very inclusive test of weather a string is boolean true or false
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public static bool ToBoolean(string sValue)
        {
            sValue = sValue.Trim();

            return sValue.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                    sValue.Equals("yes", StringComparison.OrdinalIgnoreCase) ||
                    sValue.Equals("t", StringComparison.OrdinalIgnoreCase) ||
                    sValue.Equals("y", StringComparison.OrdinalIgnoreCase) ||
                    sValue.Equals("1") ||
                    sValue.Equals("enabled", StringComparison.OrdinalIgnoreCase) ||
                    sValue.Equals("en", StringComparison.OrdinalIgnoreCase) ||
                    sValue.Equals("e", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsBoolean(string sValue)
        {

            sValue = sValue.Trim();

            return sValue.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                    sValue.Equals("false", StringComparison.OrdinalIgnoreCase) ||
                    sValue.Equals("yes", StringComparison.OrdinalIgnoreCase) ||
                    sValue.Equals("no", StringComparison.OrdinalIgnoreCase) ||
                    sValue.Equals("t", StringComparison.OrdinalIgnoreCase) ||
                    sValue.Equals("f", StringComparison.OrdinalIgnoreCase) ||
                    sValue.Equals("y", StringComparison.OrdinalIgnoreCase) ||
                    sValue.Equals("n", StringComparison.OrdinalIgnoreCase) ||
                    sValue.Equals("1") ||
                    sValue.Equals("0") ||
                    sValue.Equals("enabled", StringComparison.OrdinalIgnoreCase) ||
                    sValue.Equals("disabled", StringComparison.OrdinalIgnoreCase) ||
                    sValue.Equals("en", StringComparison.OrdinalIgnoreCase) ||
                    sValue.Equals("dis", StringComparison.OrdinalIgnoreCase) ||
                    sValue.Equals("e", StringComparison.OrdinalIgnoreCase) ||
                    sValue.Equals("d", StringComparison.OrdinalIgnoreCase);
        }

        public static T GetDefaultGeneric<T>()
        {
            return default;
        }

       

        private static CDynamicNumberToString g_Hex = new CDynamicNumberToString("0123456789ABCDEF");
        public static string ToHex(ulong uValue)
        {
            return g_Hex.Encode(uValue);
        }

        public static ulong FromHex(string sValue)
        {
            return g_Hex.Decode(sValue);
        }

        private static CDynamicNumberToString g_Binary = new CDynamicNumberToString("01");
        public static string ToBinary(ulong uValue)
        {
            return g_Binary.Encode(uValue);
        }

        /// <summary>
        /// Take a string represented binary representation and returns a number
        /// </summary>
        /// <param name="sValue">A string that looks like 00000011</param>
        /// <returns>a number that represent the binary string. Example 3</returns>
        public static ulong FromBinary(string sValue)
        {
            return g_Binary.Decode(sValue);
        }


        /// <summary>
        /// This version will not throw exceptions
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="thing"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T From<T>(object thing, T defVal)
        {
            try
            {
                return ConverterFast<T, object>.From(thing);
            }
            catch
            {
                return defVal;
            }
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static TTO From<TTO, TFROM>(TFROM from, TTO defTo)
        {
            try
            {
                //return ConverterFast<TTO, object>.From(from);
                return ConverterFast<TTO, TFROM>.From( from );
            }
            catch
            {
                return defTo;
            }

        }


   
    }

  
}
