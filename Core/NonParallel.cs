//--------------------------------------------------------------------
// © Copyright 1989-2022 Syndigo, LLC. - All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Syndigo, LLC.  Reproduction, disclosure or use without specific 
// written authorization from Syndigo, LLC. is prohibited.
// For more information see: http://www.syndigo.com
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.Core
{
    /// <summary>
    /// Class compatible with Parallel that can be used to remove the parallel component with minimal changes to code
    /// </summary>
    public class NonParallel
    {

        public static void For(int begin,int end, Action<int> func)
        {
            for(int cLoop=begin;cLoop<end;++cLoop)
            {
                func(cLoop);
            }
        }

        public static void For(int begin, int end, Func<int,Task> func)
        {
            for (int cLoop = begin; cLoop < end; ++cLoop)
            {
                func(cLoop);
            }
        }

        public static void ForEach<T>(IEnumerable<T> list, Action<T> func)
        {
            foreach ( var thing in list )
            {
                func( thing );
            }
        }

        public static void ForEach<T>(IEnumerable<T> list, Func<T,Task> func)
        {
            foreach (var thing in list)
            {
                func(thing);
            }
        }

        public static void ForEach<T>(IEnumerable<T> list, object po, Action<T, CqNonParallelLoopState> func)
        {
            ForEach<T>( list, func );
        }

        public static void ForEach<T>(IEnumerable<T> list, Action<T, CqNonParallelLoopState> func)
        {
            var state = new CqNonParallelLoopState();



            foreach ( var thing in list )
            {


                func( thing, state );

                if ( state.ShouldExitCurrentIteration || state.IsStopped )
                    break;
                else
                    state.Inc();
            }
        }
    }

    /// <summary>
    /// Used by the NonParallel methods
    /// </summary>
    public class CqNonParallelLoopState
    {
        public CqNonParallelLoopState()
        {

        }

        public long _loopState = 0;
        public void Inc() { this._loopState++; }

        public bool IsStopped
        {
            get { return this.ShouldExitCurrentIteration; }
            set { this.ShouldExitCurrentIteration = value; }
        }

        public void Stop()
        {
            this.ShouldExitCurrentIteration = true;
        }
        public void Break()
        {
            this.LowestBreakIteration = new Nullable<long>( this._loopState );
            this.ShouldExitCurrentIteration = true;
        }


        volatile bool _ShouldExitCurrentIteration = false;

        public bool ShouldExitCurrentIteration { get { return this._ShouldExitCurrentIteration; } private set { this._ShouldExitCurrentIteration = value; } }

        public bool IsExceptional { get { return false; } }

        private long? _LowestBreakIteration = null;
        public long? LowestBreakIteration
        {
            get { return this._LowestBreakIteration; }
            set { this._LowestBreakIteration = value; }
        }


    }

  
}
