//--------------------------------------------------------------------
// © Copyright 1989-2019 Syndigo, LLC. - All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Syndigo, LLC.  Reproduction, disclosure or use without specific 
// written authorization from Edgenet, LLC. is prohibited.
// For more information see: http://www.syndigo.com
//--------------------------------------------------------------------

using System;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
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
    public static class DictionaryExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty<TKey, TValue>(this System.Collections.Generic.Dictionary<TKey, TValue> dict)
        {
            return dict == null || dict.Count == 0;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotNullOrEmpty<TKey, TValue>(this System.Collections.Generic.Dictionary<TKey, TValue> dict)
        {
            return dict != null && dict.Count > 0;
        }


        public static bool Merge<TKey, TValue>(this System.Collections.Generic.Dictionary<TKey, TValue> dict, TKey key, TValue newValue)
        {
            bool bRemoved;
            dict.Merge(key, newValue, out bRemoved);

            return bRemoved;
        }

        public static bool Merge<TKey, TValue>(this System.Collections.Generic.Dictionary<TKey, TValue> dict, TKey key, TValue newValue, out bool bRemoved)
        {
            bRemoved = false;
            if (dict.ContainsKey(key))
            {
                dict.Remove(key);
                bRemoved = true;
            }
            dict.Add(key, newValue);

            return bRemoved;
        }



        public static bool RemoveSafe<TKey, TValue>(this System.Collections.Generic.Dictionary<TKey, TValue> dict, TKey key)
        {
            bool bRemoved;
            dict.RemoveSafe(key, out bRemoved);

            return bRemoved;
        }


        public static bool RemoveSafe<TKey, TValue>(this System.Collections.Generic.Dictionary<TKey, TValue> dict, TKey key, out bool bRemoved)
        {
            bRemoved = false;
            if (dict.ContainsKey(key))
            {
                dict.Remove(key);
                bRemoved = true;
            }

            return bRemoved;
        }

        public static bool MergeSafe<TKey, TValue>(this System.Collections.Generic.Dictionary<TKey, TValue> dict, TKey key, TValue val)
        {
            bool removed = false;

            if (dict.ContainsKey(key))
            {
                dict.Remove(key);
                removed = true;
            }

            dict.Add(key, val);

            return removed;
        }

        //public static bool TryAdd<TKey, TValue>(this System.Collections.Generic.Dictionary<TKey, TValue> dict, TKey key, TValue val)
        //{
        //    bool added = false;

        //    if (!dict.ContainsKey(key))
        //    {
        //        dict.Add(key, val);
        //        added = true;
        //    }

        //    return added;
        //}

        public static TValue TryGet<TKey, TValue>(this Dictionary<TKey, TValue> dict,
                                                    TKey key,
                                                    TValue def = default(TValue))
        {
            TValue val;
            if (!dict.TryGetValue(key, out val))
            {
                return def;
            }
            return val;
        }
        



        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> valueFactory)
        {
            TValue val;
            if (!dict.TryGetValue(key, out val))
            {
                val = valueFactory(key);
                dict.Add(key, val);
            }

            return val;
        }

        public static async Task<TValue> GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, Func<TKey, Task<TValue>> valueFactory)
        {
            TValue val;
            if (!dict.TryGetValue(key, out val))
            {
                val = await valueFactory(key);
                dict.Add(key, val);
            }

            return val;
        }

        public static bool TryRemove<TKey, TValue>(this System.Collections.Generic.Dictionary<TKey, TValue> dict, TKey key, out TValue val)
        {
            bool found = false;

            if (dict.ContainsKey(key))
            {
                val = dict[key];
                dict.Remove(key);
                found = true;
            }
            else
                val = default(TValue);
            return found;
        }

        public static TValue AddOrUpdate<TKey, TValue>(this System.Collections.Generic.Dictionary<TKey, TValue> dict,
                                                       TKey key,
                                                       TValue val,
                                                       Func<TKey,TValue, TValue> fUpdateValueFactory)
        {
            if (dict.ContainsKey(key))
            {
                var old = dict.TryGet(key);
                old = fUpdateValueFactory(key, old);
                dict.Remove(key);
                dict.Add(key,old );
                return old;
            }
            else
            {
                dict.Add(key, val);
                return val;
            }
        }

        public static TValue AddOrUpdate<TKey, TValue>(this System.Collections.Generic.Dictionary<TKey, TValue> dict,
                                                       TKey key,
                                                        Func<TKey, TValue> fCreateValueFactory,
                                                       Func<TKey, TValue,TValue> fUpdateValueFactory)
        {

            if (dict.ContainsKey(key))
            {
                var old = dict.TryGet(key);                
                old = fUpdateValueFactory(key, old);               
                dict.Remove(key);
                dict.Add(key,old );
                return old;
            }
            else
            {
                var created = fCreateValueFactory(key);
                dict.Add(key, created);
                return created;
            }
            
        }
    }

   

    public static class IDictionaryExtension
    {
        public static void Merge<TKey, TValue>(this System.Collections.Generic.IDictionary<TKey, TValue> dict, TKey key, TValue newValue)
        {
            bool bRemoved;
            dict.Merge(key, newValue, out bRemoved);
        }

        public static void Merge<TKey, TValue>(this System.Collections.Generic.IDictionary<TKey, TValue> dict, TKey key, TValue newValue, out bool bRemoved)
        {
            bRemoved = false;
            if (dict.ContainsKey(key))
            {
                dict.Remove(key);
                bRemoved = true;
            }
            dict.Add(key, newValue);
        }

        public static bool MergeSafe<TKey, TValue>(this System.Collections.Generic.IDictionary<TKey, TValue> dict, TKey key, TValue val)
        {
            bool bRemoved = false;
            if (dict.ContainsKey(key))
            {
                dict.Remove(key);
                bRemoved = true;
            }

            dict.Add(key, val);

            return bRemoved;
        }

        public static bool RemoveSafe<TKey, TValue>(this System.Collections.Generic.IDictionary<TKey, TValue> dict, TKey key)
        {
            bool bRemoved;
            dict.RemoveSafe(key, out bRemoved);

            return bRemoved;
        }


        public static bool RemoveSafe<TKey, TValue>(this System.Collections.Generic.IDictionary<TKey, TValue> dict, TKey key, out bool bRemoved)
        {
            bRemoved = false;
            if (dict.ContainsKey(key))
            {
                dict.Remove(key);
                bRemoved = true;
            }
            return bRemoved;
        }


       

        public static HashSet<TKey> AsKeySet<TKey,TValue>(this System.Collections.Generic.IDictionary<TKey, TValue> dict )
        {
            return new HashSet<TKey>(dict.Keys);

        }



        public static TValue TryGet<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue @default = default( TValue ))
        {
            TValue val;
            if ( !dict.TryGetValue( key, out val ) )
            {
                return @default;
            }

            return val;
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key,Func<TKey, TValue> valueFactory)
        {
            TValue val;
            if ( !dict.TryGetValue( key, out val ) )
            {
                val = valueFactory( key );
                dict.Add( key, val );
            }

            return val;
        }

        /// <summary>
        /// If your looking at this for a async overload reference...
        /// the difference here is the Func signature on the valueFactory which has a Task on the return which is Func<Tkey,Task<TValue>>
        /// usage: await dict.GetOrAdd(key,async (KEY)=> await azureThing.Get() );
        /// </summary>
        public static async Task<TValue> GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, Task<TValue>> valueFactory)
        {
            TValue val;
            if (!dict.TryGetValue(key, out val))
            {
                val = await valueFactory(key);
                dict.Add(key, val);
            }
            return val;
        }
        //public static async Task<TValue> GetOrAddParam<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, Task<TValue>> valueFactory, Func<TKey, Task<TValue>> getCallback)
        //{
        //    TValue val;
        //    if (!dict.TryGetValue(key, out val))
        //    {
        //        val = await valueFactory(key);
        //        dict.Add(key, val);
        //    }
        //    else
        //    {
        //        val = await getCallback(key);
        //    }
        //    return val;
        //}

        //public static async Task<TValue> GetOrAddAsync<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key,
        //    Func<TKey, Task<TValue>> valueFactory)
        //{
        //    TValue val;
        //    if ( !dict.TryGetValue( key, out val ) )
        //    {
        //        val = await valueFactory( key );
        //        dict.Add( key, val );
        //    }
        //    //else
        //    //{
        //    //    return val;
        //    //}
        //    return val;
        //}

        //public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue val)
        //{
        //    bool added = false;

        //    if (!dict.ContainsKey(key))
        //    {
        //        dict.Add(key, val);
        //        added = true;
        //    }

        //    return added;
        //}


    }
}
