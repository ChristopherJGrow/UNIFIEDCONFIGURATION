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
using System.Runtime.CompilerServices;


namespace Config.Core.Extensions
{
   


    public static class ListExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return list == null || list.Count == 0;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotNullOrEmpty<T>(this List<T> list)
        {
            return list != null && list.Count > 0;
        }

    }

    public static class DecimalExtension
    {
        public static string ToCurrencyString(this decimal val)
        {
            return $"{val:C}";
        }
    }

    public static class DoubleExtension
    {
        public static string ToCurrencyString(this double val)
        {
            return val.ToString("C");
        }

        public static Int32 ToInt(this double val)
        {
            return (Int32) val;
        }

        public static Int64 ToLong(this double val)
        {
            return (Int64) val;
        }

        public static bool ToBool(this double val)
        {
            return val >= 1.0f;
        }

        public static float IntAdd(this double val, float other)
        {
            return (float) ( (long) val + (long) other );
        }
        public static float IntSubtract(this double val, float other)
        {
            return (float) ( (long) val - (long) other );
        }

        /// <summary>
        /// Calculates percentages without fear of div zero or other issues
        /// value must be less than long.MAX / 100
        /// </summary>
        /// <param name="myvalue">value which is a percentage of something</param>
        /// <param name="total">total</param>
        /// <returns></returns>
        /// 
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static double PercentOf(this double myvalue, double total)
        {
            return total > 0 ? ( myvalue * 100 ) / total : 100;
        }
    }
    public static class FloatExtension
    {
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Int32 ToInt(this float val)
        {
            return (Int32) val;
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Int64 ToLong(this float val)
        {
            return (Int64) val;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool ToBool(this float val)
        {
            return val >=1.0f;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float IntAdd(this float val,float other )
        {
            return (float) ((long) val + (long) other);
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float IntSubtract(this float val, float other)
        {
            return (float) ( (long) val - (long) other );
        }

        /// <summary>
        /// Calculates percentages without fear of div zero or other issues
        /// value must be less than long.MAX / 100
        /// </summary>
        /// <param name="myvalue">value which is a percentage of something</param>
        /// <param name="total">total</param>
        /// <returns></returns>
        /// 
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float PercentOf(this float myvalue, float total)
        {
            return total > 0 ? ( myvalue * 100 ) / total : 100;
        }


    }
    public static class Int16Extension
    {
        //[MethodImpl( MethodImplOptions.AggressiveInlining )]
        //public static CQLENGTH ToLENGTH(this Int16 myVal)
        //{
        //    return new CQLENGTH(myVal);
        //}


        /// <summary>
        /// Converts to boolean using old C++ rules
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ToBool(this Int16 val)
        {
            //return val == 0 ? false : true;
            return val != 0;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static double ToDouble(this Int16 val)
        {
            return (double) val;
        }


    }

    public static class UInt16Extension
    {
        //[MethodImpl( MethodImplOptions.AggressiveInlining )]
        //public static CQLENGTH ToLENGTH(this UInt16 myVal)
        //{
        //    return new CQLENGTH(myVal);
        //}
        /// <summary>
        /// Converts to boolean using old C++ rules
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool ToBool(this UInt16 val)
        {
            return val != 0 ;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static double ToDouble(this UInt16 val)
        {
            return (double) val;
        }

    }

    public static class Int32Extension
    {
        //[MethodImpl( MethodImplOptions.AggressiveInlining )]
        //public static CQLENGTH ToLENGTH(this Int32 myVal)
        //{
        //    return new CQLENGTH(myVal);
        //}

        /// <summary>
        /// Converts to boolean using old C++ rules
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool ToBool(this Int32 val)
        {
            return val == 0 ? false : true;
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static double ToDouble(this Int32 val)
        {
            return (double) val;
        }


        /// <summary>
        /// Calculates percentages without fear of div zero or other issues
        /// value must be less than long.MAX / 100
        /// </summary>
        /// <param name="myvalue">value which is a percentage of something</param>
        /// <param name="total">total</param>
        /// <returns></returns>
        /// 
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static long PercentOf(this long myvalue, long total)
        {
            return total > 0 ? (myvalue * 100) / total : 100;            
        }
    }

    public static class UInt32Extension
    {
        //[MethodImpl( MethodImplOptions.AggressiveInlining )]
        //public static CQLENGTH ToLENGTH(this UInt32 myVal)
        //{
        //    return new CQLENGTH(myVal);
        //}

        /// <summary>
        /// Converts to boolean using old C++ rules
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool ToBool(this UInt32 val)
        {
            return val == 0 ? false : true;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static double ToDouble(this UInt32 val)
        {
            return (double) val;
        }

        /// <summary>
        /// Calculates percentages without fear of div zero or other issues
        /// value must be less than long.MAX / 100
        /// </summary>
        /// <param name="myvalue">value which is a percentage of something</param>
        /// <param name="total">total</param>
        /// <returns></returns>
        /// 
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static ulong PercentOf(this ulong myvalue, ulong total)
        {
            return total > 0 ? ( myvalue * 100 ) / total : 100;
        }
    }

    public static class Int64Extension
    {
        //[MethodImpl( MethodImplOptions.AggressiveInlining )]
        //public static CQLENGTH ToLENGTH(this Int64 myVal)
        //{
        //    return new CQLENGTH(myVal);
        //}

        /// <summary>
        /// Converts to boolean using old C++ rules
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool ToBool(this Int64 val)
        {
            return val == 0 ? false : true;
        }

        /// <summary>
        /// Calculates percentages without fear of div zero or other issues
        /// value must be less than long.MAX / 100
        /// </summary>
        /// <param name="myvalue">value which is a percentage of something</param>
        /// <param name="total">total</param>
        /// <returns></returns>
        /// 
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Int64 PercentOf(this Int64 myvalue, Int64 total)
        {
            return total > 0 ? ( myvalue * 100 ) / total : 100;
        }

    }

    public static class UInt64Extension
    {
        //[MethodImpl( MethodImplOptions.AggressiveInlining )]
        //public static CQLENGTH ToLENGTH(this UInt64 myVal)
        //{
        //    return new CQLENGTH( (long) myVal);
        //}

        /// <summary>
        /// Converts to boolean using old C++ rules
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        /// 
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool ToBool(this UInt64 val)
        {
            //return val == 0 ? false : true;

            return val != 0 ;
        }

        /// <summary>
        /// Calculates percentages without fear of div zero or other issues
        /// value must be less than long.MAX / 100
        /// </summary>
        /// <param name="myvalue">value which is a percentage of something</param>
        /// <param name="total">total</param>
        /// <returns></returns>
        /// 
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static UInt64 PercentOf(this UInt64 myvalue, UInt64 total)
        {
            return total > 0 ? ( myvalue * 100 ) / total : 100;
        }
    }

    /// <summary>
    /// Contains the supported ways to ToString a bool
    /// </summary>
    public enum BoolStringVariation
    {
        PassFail,
        YesNo,
        TrueFalse
    }

    public static class BoolExtension
    {
        /// <summary>
        /// Converts to int using old C++ rules
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static int ToInt(this bool val)
        {
            return val == true ? 1 : 0;
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static short ToShort(this bool val)
        {
            return val == true ? (short)1 : (short)0;
        }

        /// <summary>
        /// Used to represent bools in different ways.. 
        /// </summary>
        /// <param name="val"></param>
        /// <param name="variation"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static string ToString(this bool val, BoolStringVariation variation)
        {
            string retval;
            switch (variation)
            {
                case BoolStringVariation.PassFail:
                    retval = val == true ? "Pass" : "Fail";
                    break;
                case BoolStringVariation.YesNo:
                    retval = val == true ? "Yes" : "No";
                    break;
                default:
                    retval = val.ToString(); // true & false
                    break;
            }
            return retval;

        }
    }

    

    


 

  

   

    

}
