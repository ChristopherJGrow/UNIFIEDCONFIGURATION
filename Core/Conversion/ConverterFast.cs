// File: ConversionDemo/Core/CConverterFast.cs
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Config.Core.Conversion
{
    /// <summary>
    /// Per-(TTO, TFROM) converter with fast paths.
    /// Uses ISpanParsable via the *string* TryParse overload (expression trees can't handle ref structs).
    /// Many of these THROW exceptions when conversion fail!
    /// </summary>
    public static class ConverterFast<TTO, TFROM>
    {
        private static readonly Func<object, object> __Converter = Build();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TTO From( TFROM value ) => (TTO) __Converter(value!);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Resolve()
        {
            _ = __Converter; // why: ensures the delegate is built & JITed before timing
        }

        public static Func<object, object> Build()
        {
            var to   = typeof(TTO);
            var from = typeof(TFROM);

            // Exact registry hit first.
            if (ConversionAgents.Converters.TryGetValue((to, from), out var fn))
                return fn;

            // Enum fast path (string -> enum).
            if (to.IsEnum && from == typeof(string))
                return o => CEnumParserFast<TTO>.Parse((string) o);
            else if (to.IsEnum && from == typeof(object))
                return o => CEnumParserFast<TTO>.Parse((string) o);

            // Generic string -> T using ISpanParsable<T>.TryParse(string, IFormatProvider, out T)
            else if (from == typeof(string) && ImplementsISpanParsable(to))
            {
                var parser = CompileSpanParsableParserString(to);     // Func<string, object>
                return o => parser((string) o);
            }

            // Boxed sources: tiny runtime-type dispatcher with memoization.
            if (from == typeof(object))
            {
                return BuildObjectDispatcher(to);
            }

            // Last resort.
            return o => Convert.ChangeType(o, to)!;
        }

        private static Func<object, object> BuildObjectDispatcher( Type to )
        {
            var cache = new Dictionary<Type, Func<object, object>>(8);

            return obj =>
            {
                if (obj is null) throw new ArgumentNullException(nameof(obj));
                var rt = obj.GetType();

                if (!cache.TryGetValue(rt, out var fn))
                {
                    if (!ConversionAgents.Converters.TryGetValue((to, rt), out fn))
                    {
                        if (rt == typeof(string) && ImplementsISpanParsable(to))
                        {
                            var parser = CompileSpanParsableParserString(to); // Func<string, object>
                            fn = s => parser((string) s);
                        }
                        else
                        {
                            fn = o => Convert.ChangeType(o, to)!;
                        }
                    }
                    cache[rt] = fn;
                }

                return fn(obj);
            };
        }

        // ---- Helpers ----

        private static bool ImplementsISpanParsable( Type to )
        {
            var isp = typeof(ISpanParsable<>).MakeGenericType(to);
            return isp.IsAssignableFrom(to);
        }

        // Builds a cached delegate:
        // (string s) => T.TryParse(s, InvariantCulture, out var v) ? (object)v : throw new FormatException(...)
        private static Func<string, object> CompileSpanParsableParserString( Type to )
        {
            var sParam   = Expression.Parameter(typeof(string), "s");
            var result   = Expression.Variable(to, "value");

            var tryParse = to.GetMethod(
                name: "TryParse",
                bindingAttr: System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static,
                binder: null,
                types: new[] { typeof(string), typeof(IFormatProvider), to.MakeByRefType() },
                modifiers: null);

            if (tryParse is null)
                throw new NotSupportedException($"Type {to} implements ISpanParsable but no TryParse(string, IFormatProvider, out {to.Name}) found.");

            var call = Expression.Call(
                instance: null,
                method: tryParse,
                arguments: new Expression[]
                {
                    sParam,
                    Expression.Constant(CultureInfo.InvariantCulture, typeof(IFormatProvider)),
                    result
                });

            var body = Expression.Block(
                typeof(object),
                new[] { result },
                Expression.Condition(
                    call,
                    Expression.Convert(result, typeof(object)),
                    Expression.Throw(
                        Expression.New(typeof(FormatException).GetConstructor(new[] { typeof(string) })!,
                            Expression.Constant($"Input string was not in a correct format for {to.Name}.")),
                        typeof(object))
                )
            );

            return Expression.Lambda<Func<string, object>>(body, sParam).Compile();
        }
    }
}
