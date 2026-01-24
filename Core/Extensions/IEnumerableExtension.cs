//--------------------------------------------------------------------
// © Copyright 1989-2020 Syndigo, LLC. - All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Syndigo, LLC.  Reproduction, disclosure or use without specific 
// written authorization from Syndigo, LLC. is prohibited.
// For more information see: http://www.syndigo.com
//--------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using System.Linq;


namespace Config.Core.Extensions
{

    public static class IEnumerableExtension
    {
        public static string ToDelimited<T>(this IEnumerable<T> list, string delimiter = ",")
        {
            return list == null ? null : String.Join( delimiter, list );

        }

        ///// <summary>
        ///// Allows distinct by a comparer and decider.  After equality you can use a different field to decide which distinct item to take
        ///// </summary>
        ///// <typeparam name="TSource"></typeparam>
        ///// <param name="source"></param>
        ///// <param name="fAreEqual"></param>
        ///// <param name="fDecide"></param>
        ///// <returns></returns>
        //public static IEnumerable<TSource> DistinctBy<TSource>(this IOrderedEnumerable<TSource> source,
        //                                                                Func<TSource, TSource, bool> fAreEqual,
        //                                                                Func<TSource, TSource, TSource> fDecide)
        //{
        //    TSource retval = source.First() ; 

        //    foreach (var item in source)
        //    {

        //        if (fAreEqual( item, retval ))
        //        {
        //            retval = fDecide( retval, item );
        //            continue;
        //        }
        //        else
        //        {
        //            yield return retval;
        //        }

        //        retval = item;
        //    }

        //    yield return retval;

        //}

        ///// <summary>
        ///// Simple Distinct with an Equality comparer
        ///// </summary>
        ///// <typeparam name="TSource"></typeparam>
        ///// <param name="source"></param>
        ///// <param name="fAreEqual"></param>
        ///// <returns></returns>
        //public static IEnumerable<TSource> DistinctBy<TSource>(this IOrderedEnumerable<TSource> source,
        //                                                              Func<TSource, TSource, bool> fAreEqual)
        //{

        //    TSource retval = source.First();           

        //    foreach (var item in source)
        //    {

        //        if (!fAreEqual( item, retval ) )
        //        {                        
        //            yield return retval;
        //        }                                       

        //        retval = item;
        //    }

        //    yield return retval;

        //}

        ///// <summary>
        ///// Simple Distinct with an Equality comparer
        ///// </summary>
        ///// <typeparam name="TSource"></typeparam>
        ///// <param name="source"></param>
        ///// <param name="fAreEqual"></param>
        ///// <returns></returns>
        //public static IEnumerable<TSource> DistinctBy<TSource,TKey>(this IOrderedEnumerable<TSource> source,
        //                                                              Func<TSource, TKey> fKey)
        //{
        //    TSource retval = source.First();

        //    foreach (var item in source)
        //    {                
        //        if ( !fKey(item).Equals(fKey(retval)) )
        //        {
        //            yield return retval;
        //        }

        //        retval = item;
        //    }

        //    yield return retval;
        //}

        //public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IOrderedEnumerable<TSource> source,
        //                                                             Func<TSource, TKey> fKey,
        //                                                             IEqualityComparer<TKey> comparer)
        //{
        //    TSource retval = source.First();

        //    foreach (var item in source)
        //    {
        //        if (!comparer.Equals( fKey( item ),  fKey( retval ) ) )
        //        {
        //            yield return retval;
        //        }

        //        retval = item;
        //    }

        //    yield return retval;
        //}

        /// <summary>
        /// Select pairs from source possibly with an offset record between them.  Note the offset loops
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="funcA"></param>
        /// <param name="funcB"></param>
        /// <param name="offset">Gow many elements separate Item1 and Item2.. Item2 will wrap when Item1 hits the end of the Enumerable</param>
        /// <returns></returns>
        public static IEnumerable<(T1,T2)> SelectPairs<T1,T2, TSource>( this IEnumerable<TSource> source, Func<TSource,T1> funcA , Func<TSource,T2> funcB, int offset=0)
        {

            var iter = source.GetEnumerator();
            var iterSecond = source.GetEnumerator();

            for(int cLoop=0;cLoop<offset; ++cLoop)
                iterSecond.MoveNext();


            while (iter.MoveNext())
            {
                if (!iterSecond.MoveNext())
                {
                    iterSecond.Reset();
                    iterSecond.MoveNext();
                }

                yield return (funcA(iter.Current), funcB(iterSecond.Current));
               
            }            
        }

        /// <summary>
        /// Select offset pairs from source.. the offset wraps!  IE: offset 1 = (element[0], element[1])
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="offset">Gow many elements separate Item1 and Item2.. Item2 will wrap when Item1 hits the end of the Enumerable</param>
        /// <returns></returns>
        public static IEnumerable<(TSource, TSource)> SelectPairs< TSource>(this IEnumerable<TSource> source, int offset )
        {

            var iter = source.GetEnumerator();
            var iterSecond = source.GetEnumerator();

            for (int cLoop = 0; cLoop < offset; ++cLoop)
                iterSecond.MoveNext();


            while (iter.MoveNext())
            {
                if (!iterSecond.MoveNext())
                {
                    iterSecond.Reset();
                    iterSecond.MoveNext();
                }

                yield return ( iter.Current , iterSecond.Current );

            }
        }

        /// <summary>
        /// Select offset between Triplets from source.. the offset wraps!  IE: offset 1 = (element[0], element[1], element[2])
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="offset">Gow many elements separate Item1, Item2 and Item3.. Item2 and Item3 will wrap when Item1 hits the end of the Enumerable</param>
        /// <returns></returns>
        public static IEnumerable<(TSource, TSource,TSource)> SelectTriple<TSource>(this IEnumerable<TSource> source, int offset)
        {

            var iterFirst = source.GetEnumerator();
            var iterSecond = source.GetEnumerator();
            var iterThird= source.GetEnumerator();

            for (int cLoop = 0; cLoop < offset; ++cLoop)
                iterSecond.MoveNext();
            
            for (int cLoop = 0; cLoop < offset*2; ++cLoop)
                iterThird.MoveNext();


            while (iterFirst.MoveNext())
            {
                if (!iterSecond.MoveNext())
                {
                    iterSecond.Reset();
                    iterSecond.MoveNext();
                }

                if (!iterThird.MoveNext())
                {
                    iterThird.Reset();
                    iterThird.MoveNext();
                }

                yield return (iterFirst.Current, iterSecond.Current,iterThird.Current);
            }

            offset = 0;
        }

        /// <summary>
        /// Returns the first two elements in the enumerable or default<TSource>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns>first,second</returns>
        public static (TSource,TSource) FirstTwoOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            var iterator = source.GetEnumerator();

            TSource first  = iterator.MoveNext() ? iterator.Current : default(TSource);
            TSource second = iterator.MoveNext() ? iterator.Current : default(TSource);

            return (first, second);            
        }

        /// <summary>
        /// Returns the last two elements in the enuemrable or default<TSource>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns>second to last, last</returns>
        public static (TSource, TSource) LastTwoOrDefault<TSource>(this IEnumerable<TSource> source)
        {            
            var iterator = source.Reverse().GetEnumerator();
            
            TSource last =          iterator.MoveNext() ? iterator.Current : default(TSource);
            TSource penultimate =   iterator.MoveNext() ? iterator.Current : default(TSource);

            return ( last, penultimate);
        }


        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static TSource SecondOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            var iterator = source.GetEnumerator();

            //if (iterator.MoveNext() && iterator.MoveNext())
            //    return iterator.Current;
            //else
            //    return default( TSource );

            return iterator.MoveNext() && iterator.MoveNext() ? iterator.Current : default( TSource );
        }
        

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void ForEach<TValue>(this System.Collections.Generic.IEnumerable<TValue> list, Action<TValue> func)
        {
            foreach (var item in list)
            {
                func( item );
            }
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void ForEach<TValue>(this System.Collections.Generic.IEnumerable<TValue> list, Func<TValue,bool> func)
        {
            foreach (var item in list)
            {
                if (func( item ))
                    break;
            }
        }



        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void ForEach<TValue>(this System.Collections.Generic.IEnumerable<TValue> list, Action<TValue,int> func)
        {
            int cLoop=0;
            foreach (var item in list)
            {
                func( item , cLoop);
                ++cLoop;
            }
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void ForEach<TValue>(this System.Collections.Generic.IEnumerable<TValue> list, Action<TValue, int, int> func)
        {
            int cLoop=0;
            int cEnd =list.Count();
            foreach (var item in list)
            {
                func( item, cLoop, cEnd );
                ++cLoop;
            }
        }
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static void ForEach<TValue>(this System.Collections.Generic.IEnumerable<TValue> list, Func<TValue, int, int,bool> func)
        {
            int cLoop=0;
            int cEnd =list.Count();
            foreach (var item in list)
            {
                if (func( item, cLoop, cEnd ))
                    break;

                ++cLoop;
            }
        }

        /// <summary>
        /// given a list allows you to exclude things from another list with a custom comparer
        /// </summary>
        /// <typeparam name="TValue1"></typeparam>
        /// <typeparam name="TValue2"></typeparam>
        /// <param name="list"></param>
        /// <param name="exceptList"></param>
        /// <param name="fCompare">func that returns true when an item should be excluded</param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static IEnumerable<TValue1> Except<TValue1, TValue2>(this   System.Collections.Generic.IEnumerable<TValue1> list, 
                                                                            System.Collections.Generic.IEnumerable<TValue2> exceptList, 
                                                                            Func<TValue1, TValue2, bool> fCompare)
        {
            return list.Where( (ITEM) => !exceptList.Any( (EXCLUDE) => fCompare(ITEM, EXCLUDE ) ));            
        }
    }

}
