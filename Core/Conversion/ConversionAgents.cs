//--------------------------------------------------------------------
// © Copyright 1989-2025 Syndigo, LLC. - All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Syndigo, LLC.  Reproduction, disclosure or use without specific 
// written authorization from Syndigo, LLC. is prohibited.
// For more information see: http://www.syndigo.com
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

using Config.Core.Extensions;


namespace Config.Core.Conversion
{
    /// <summary>
    /// Registry of hand-tuned converters keyed by (TargetType, SourceType).. always right to left.
    /// </summary>
    public static class ConversionAgents
    {
        // Why: exact (To, From) keys let CConverterFast avoid per-call switching.
        public static readonly Dictionary<(Type To, Type From), Func<object, object>> Converters =
            new Dictionary<(Type, Type), Func<object, object>>
            {
                //{ (typeof(int), typeof(object)), s =>
                //    {
                //        ReadOnlySpan<char> span = (s.ToString()).AsSpan().Trim();
                //        if (span.Length>0 && span[0]=='+') span = span.Slice(1);
                //        if (int.TryParse(span, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v)) return v;
                //        return Convert.ChangeType((string)s, typeof(int))!; // fallback
                //    }
                //},
                //{ (typeof(int), typeof(object)), s => int.Parse( s.ToString(), NumberStyles.Integer| NumberStyles.AllowLeadingSign|NumberStyles.AllowThousands, CultureInfo.CurrentCulture ) },

                // ===== string -> numerics/bool =====
                //{ (typeof(int), typeof(string)), s =>
                //    {
                //        ReadOnlySpan<char> span = ((string)s).AsSpan().Trim();
                //        if (span.Length>0 && span[0]=='+') span = span.Slice(1);
                //        if (int.TryParse(span, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v)) return v;
                //        return Convert.ChangeType((string)s, typeof(int))!; // fallback
                //    }
                //},
                //{ (typeof(int), typeof(string)), s => FastInt.Parse((string)s) },
                //{ (typeof(int), typeof(string)), s => int.Parse((string)s) },
                //{ (typeof(int), typeof(string)), s => int.Parse((string) s, NumberStyles.Integer| NumberStyles.AllowLeadingSign|NumberStyles.AllowThousands, CultureInfo.CurrentCulture  )  },
                //{ (typeof(int), typeof(string)), s => int.Parse((string) s, NumberStyles.Integer| NumberStyles.AllowLeadingSign, CultureInfo.CurrentCulture  )  },
                //{ (typeof(long), typeof(string)), s =>
                //    {
                //        ReadOnlySpan<char> span = ((string)s).AsSpan().Trim();
                //        if (span.Length>0 && span[0]=='+') span = span.Slice(1);
                //        if (long.TryParse(span, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v)) return v;
                //        return Convert.ChangeType((string)s, typeof(long))!;
                //    }
                //},
                //{ (typeof(long), typeof(object)),  s => long.Parse( s.ToString(),NumberStyles.Integer| NumberStyles.AllowLeadingSign|NumberStyles.AllowThousands, CultureInfo.CurrentCulture   ) },
                //{ (typeof(long), typeof(string)),  s => long.Parse((string)s,NumberStyles.Integer| NumberStyles.AllowLeadingSign|NumberStyles.AllowThousands, CultureInfo.CurrentCulture   ) },
                { (typeof(float), typeof(string)), s => (float) float.Parse((string)s, NumberStyles.Float|NumberStyles.AllowThousands, CultureInfo.CurrentCulture) }  ,
                { (typeof(double), typeof(string)), s => (double) double.Parse((string)s, NumberStyles.Float|NumberStyles.AllowThousands, CultureInfo.InvariantCulture) },
                { (typeof(bool), typeof(object)), s => ToBoolean(s.ToString()) },
                { (typeof(bool), typeof(string)), s => ToBoolean((string)s) },

                // ===== object -> string =====
                { (typeof(string), typeof(object)), o => o.ToString()! },

                // ===== Guid/Uri =====
                { (typeof(Guid), typeof(string)), s => new Guid((string)s) },
                { (typeof(Guid), typeof(object)), o => new Guid(o.ToString()!) }, // permissive fallback
                { (typeof(Uri), typeof(string)), s => new Uri(((string)s).Trim('{','}')) },
                { (typeof(Uri), typeof(object)), o => new Uri(o.ToString()!.Trim('{','}')) },

                // ===== exact numeric widening/narrowing (checked) =====
                // int <- *
                { (typeof(int), typeof(int)), o => (int)o },
                { (typeof(int), typeof(short)), o => (int)(short)o },
                { (typeof(int), typeof(sbyte)), o => (int)(sbyte)o },
                { (typeof(int), typeof(byte)), o => (int)(byte)o },
                { (typeof(int), typeof(ushort)), o => (int)(ushort)o },
                { (typeof(int), typeof(uint)), o => checked((int)(uint)o) },
                { (typeof(int), typeof(long)), o => checked((int)(long)o) },
                { (typeof(int), typeof(ulong)), o => checked((int)(ulong)o) },
                { (typeof(int), typeof(float)), o => checked((int)(float)o) },
                { (typeof(int), typeof(double)), o => checked((int)(double)o) },
                { (typeof(int), typeof(decimal)), o => checked((int)(decimal)o) },
                { (typeof(int), typeof(bool)), o => ((bool)o) ? 1 : 0 },

                // long <- *
                { (typeof(long), typeof(long)), o => (long)o },
                { (typeof(long), typeof(int)), o => (long)(int)o },
                { (typeof(long), typeof(short)), o => (long)(short)o },
                { (typeof(long), typeof(sbyte)), o => (long)(sbyte)o },
                { (typeof(long), typeof(byte)), o => (long)(byte)o },
                { (typeof(long), typeof(ushort)), o => (long)(ushort)o },
                { (typeof(long), typeof(uint)), o => (long)(uint)o },
                { (typeof(long), typeof(ulong)), o => checked((long)(ulong)o) },
                { (typeof(long), typeof(float)), o => checked((long)(float)o) },
                { (typeof(long), typeof(double)), o => checked((long)(double)o) },
                { (typeof(long), typeof(decimal)), o => checked((long)(decimal)o) },
                { (typeof(long), typeof(bool)), o => ((bool)o) ? 1L : 0L },

                // double <- *
                { (typeof(double), typeof(double)), o => (double)o },
                { (typeof(double), typeof(float)), o => (double)(float)o },
                { (typeof(double), typeof(decimal)), o => (double)(decimal)o },
                { (typeof(double), typeof(int)), o => (double)(int)o },
                { (typeof(double), typeof(long)), o => (double)(long)o },
                { (typeof(double), typeof(uint)), o => (double)(uint)o },
                { (typeof(double), typeof(ulong)), o => (double)(ulong)o },
                { (typeof(double), typeof(short)), o => (double)(short)o },
                { (typeof(double), typeof(byte)), o => (double)(byte)o },
                { (typeof(double), typeof(sbyte)), o => (double)(sbyte)o },
                { (typeof(double), typeof(ushort)), o => (double)(ushort)o },
                { (typeof(double), typeof(bool)), o => ((bool)o) ? 1d : 0d },

                // float <- *
                { (typeof(float), typeof(float)), o => (float)o },
                { (typeof(float), typeof(double)), o => (float)(double)o },
                { (typeof(float), typeof(decimal)), o => (float)(decimal)o },
                { (typeof(float), typeof(int)), o => (float)(int)o },
                { (typeof(float), typeof(long)), o => (float)(long)o },
                { (typeof(float), typeof(uint)), o => (float)(uint)o },
                { (typeof(float), typeof(ulong)), o => (float)(ulong)o },
                { (typeof(float), typeof(short)), o => (float)(short)o },
                { (typeof(float), typeof(byte)), o => (float)(byte)o },
                { (typeof(float), typeof(sbyte)), o => (float)(sbyte)o },
                { (typeof(float), typeof(ushort)), o => (float)(ushort)o },
                { (typeof(float), typeof(bool)), o => ((bool)o) ? 1f : 0f },

                // ushort/uint/ulong via string handled above; explicit numeric casts can be added similarly if needed.
            };

        /// <summary>
        /// Very inclusive test of weather a string is boolean true or false
        /// </summary>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public static bool ToBoolean( string sValue )
        {
            if (sValue.IsNullOrEmpty())
                throw new ArgumentNullException();

            sValue = sValue.Trim();

            return sValue.Equals("true", StringComparison.CurrentCultureIgnoreCase) ||
                    sValue.Equals("yes", StringComparison.CurrentCultureIgnoreCase) ||
                    sValue.Equals("t", StringComparison.CurrentCultureIgnoreCase) ||
                    sValue.Equals("y", StringComparison.CurrentCultureIgnoreCase) ||
                    sValue.Equals("1") ||
                    sValue.Equals("enabled", StringComparison.CurrentCultureIgnoreCase) ||
                    sValue.Equals("en", StringComparison.CurrentCultureIgnoreCase) ||
                    sValue.Equals("e", StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool IsBoolean( string sValue )
        {
            if (sValue.IsNullOrEmpty())
                throw new NullReferenceException();

            sValue = sValue.Trim();

            return sValue.Equals("true", StringComparison.CurrentCultureIgnoreCase) ||
                    sValue.Equals("false", StringComparison.CurrentCultureIgnoreCase) ||
                    sValue.Equals("yes", StringComparison.CurrentCultureIgnoreCase) ||
                    sValue.Equals("no", StringComparison.CurrentCultureIgnoreCase) ||
                    sValue.Equals("t", StringComparison.CurrentCultureIgnoreCase) ||
                    sValue.Equals("f", StringComparison.CurrentCultureIgnoreCase) ||
                    sValue.Equals("y", StringComparison.CurrentCultureIgnoreCase) ||
                    sValue.Equals("n", StringComparison.CurrentCultureIgnoreCase) ||
                    sValue.Equals("1") ||
                    sValue.Equals("0") ||
                    sValue.Equals("enabled", StringComparison.CurrentCultureIgnoreCase) ||
                    sValue.Equals("disabled", StringComparison.CurrentCultureIgnoreCase) ||
                    sValue.Equals("en", StringComparison.CurrentCultureIgnoreCase) ||
                    sValue.Equals("dis", StringComparison.CurrentCultureIgnoreCase) ||
                    sValue.Equals("e", StringComparison.CurrentCultureIgnoreCase) ||
                    sValue.Equals("d", StringComparison.CurrentCultureIgnoreCase);
        }
    }



    
}