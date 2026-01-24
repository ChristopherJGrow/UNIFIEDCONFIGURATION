using Config.Core.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.Core
{
    /// <summary>
    /// Quickly decodes enums to strings
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumHelp<T> where T : Enum
    {
        static SortedList<T, string> __EnumsIntToText = new SortedList<T, string>(Enum.GetValues( typeof(T) ).Length);

        static object __loaderLock = new object();

        static EnumHelp()
        {
            lock (__loaderLock)
            {
                string myEnumString;
                var enumType = typeof(T);

                var allEnums = Enum.GetValues( enumType ).Cast<T>();
                foreach (var myVal in allEnums)
                {
                    // yes this takes awhile but is done only once                   
                    myEnumString = Enum.GetName( enumType, myVal );

                    // We do a try add because we could get multiple entries 
                    // when you have two enum's with the same value
                    //
                    __EnumsIntToText.TryAdd( myVal, myEnumString );
                }

            }

        }

        /// <summary>
        /// quickly returns the string value of an Enum 
        /// </summary>
        /// <param name="val">Enum value</param>
        /// <returns></returns>
        //[MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static string StringFrom(T val)
        {
            return __EnumsIntToText.TryGet( val );
        }

        /// <summary>
        /// Returns all the possible stringed for an enum
        /// </summary>
        /// <returns></returns>
        //[MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static IEnumerable<string> Strings()
        {
            return __EnumsIntToText.Values;
        }

        public static IEnumerable<T> Enums()
        {
            return __EnumsIntToText.Keys;
        }
    }
}
