//--------------------------------------------------------------------
// © Copyright 1989-2017 Edgenet, Inc. - All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Edgenet, Inc.  Reproduction, disclosure or use without specific 
// written authorization from Edgenet, Inc. is prohibited.
// For more information see: http://www.edgenet.com
//--------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

using System.Runtime.CompilerServices;

using System.Threading;

using System.Text;

using System.Linq;

//using Sage.Shared.COR.Main.Threading;


namespace Config.Core.Extensions
{
    public static class AsyncEnumerableExtension
    {
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> items, CancellationToken cancellationToken = default)
        {
            var results = new List<T>();
            await foreach (var item in items.WithCancellation( cancellationToken ).ConfigureAwait( false ))
            {
                results.Add( item );
            }
            return results;
        }

    }

    /// <summary>
    /// Class to manipulate bits in Flags based enums
    /// </summary>
    public static class EnumerationExtensions
    {

        /// <summary>
        /// Returns true if ALL the passed variables bits are NOT set in the variable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DoesNotHave<T>(this System.Enum type, T value)
        {

            return !type.Has(value);
        }

        /// <summary>
        /// Returns true if ALL the passed variables bits are set in the variable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Has<T>(this System.Enum type, T value)
        {
            try
            {
                return (((int)(object)type & (int)(object)value) == (int)(object)value);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if any of the bit flags in value are set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasAny<T>(this System.Enum type, T value)
        {
            try
            {
                var temp = ((int)(object)type & (int)(object)value) ;
                return temp > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if the specified variable is equal to the passed value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Is<T>(this System.Enum type, T value)
        {
            try
            {
                return (int)(object)type == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Returns the passed value with the specified bits turned on
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Add<T>(this System.Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type | (int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Could not append value from enumerated type '{0}'.", typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// Returns the passed value with the specified bits removed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Remove<T>(this System.Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type & ~(int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Could not remove value from enumerated type '{0}'.", typeof(T).Name), ex);
            }
        }

        static ConcurrentDictionary<Type, ConcurrentDictionary<int, string>> __FastEnumData = new ConcurrentDictionary<Type, ConcurrentDictionary<int, string>>();

        /// <summary>
        /// Fast code to get an Enum's string quckly 
        /// for 32bit enums only!
        /// </summary>
        /// <param name="myEnum"></param>
        /// <returns></returns>
        public static string ToStringFast<T>(this T myEnum) where T: System.Enum
        {
            return EnumHelp<T>.StringFrom(myEnum);          
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this System.Enum type)
        {
            var retval = (int)Convert.ChangeType(type, typeof(int));
            return retval;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ToLong(this System.Enum type)
        {
            var retval = (long)Convert.ChangeType(type, typeof(long));
            return retval;
        }

    }

}
