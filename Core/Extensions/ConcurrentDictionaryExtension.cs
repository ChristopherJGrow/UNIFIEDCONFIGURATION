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
    public static class ConcurrentDictionaryExtension
    {
        public static void Merge<TKey, TValue>(this System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue> dict,
                                                TKey key,
                                                TValue newValue)
        {
            dict.AddOrUpdate(key, newValue, (KEY, OLD) => newValue);

        }

        public static bool MergeSafe<TKey, TValue>(this System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue> dict,
                                                    TKey key,
                                                    TValue val)
        {
            var result = dict.AddOrUpdate(key, val, (KEY, OLD) => val);

            return result != null;
        }

        public static TValue AddOrUpdate<TKey, TValue>(this System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue> dict,
                                                    TKey key,
                                                    TValue val)
        {
            return dict.AddOrUpdate(key, val, (KEY, VAL) => val);
        }

        public static TValue TryGet<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict,
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

        public static bool TryRemoveEx<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict,
                                                TKey key)
        {
            TValue removed;
            return dict.TryRemove( key, out removed );
        }

        public static bool TryRemove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict,
                                                        TKey key)
        {
            TValue removed;
            return dict.TryRemove(key, out removed);
        }

        public static bool RemoveSafe<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key)
        {
            TValue removed;
            return dict.TryRemove(key, out removed);
        }


        public static bool RemoveSafe<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key, out bool bRemoved)
        {
            bRemoved = false;
            TValue removed;
            bRemoved = dict.TryRemove(key, out removed);
            return bRemoved;
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict)
        {
            return dict.ToDictionary(KVP => KVP.Key, KVP => KVP.Value);
        }



       

    }
}
