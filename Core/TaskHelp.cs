//--------------------------------------------------------------------
// © Copyright 1989-2022 Syndigo, LLC. - All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Syndigo, LLC.  Reproduction, disclosure or use without specific 
// written authorization from Syndigo, LLC. is prohibited.
// For more information see: http://www.syndigo.com
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.Threading.Tasks;


namespace Config.Core
{
    public static class TaskHelp
    {
       
        //static Random __rand = new Random();
        //public async static Task<int> GetAsyncTestResult()
        //{
        //    System.Threading.Thread.Sleep(__rand.Next(300));
        //    return __rand.Next(500);
        //}

        /// <summary>
        /// Runs an async Func from a non async method.. Make sure you declare func as async ()=> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func">Make sure you declare this as async ()=> </param>
        /// <returns></returns>
        public static T Run<T>(Func<Task<T>> func)
        {
            T retval;
            var task = Task.Run<T>(func); 

            task.ConfigureAwait(false);
            retval = task.GetAwaiter().GetResult();
            retval = task.Result;

            return retval;
        }

        /// <summary>
        /// Runs an async Func from a non async method.. Make sure you declare func as async ()=> 
        /// </summary>        
        /// <param name="func">Make sure you declare this as async ()=> </param>        
        public static void Run(Func<Task> func)
        {
            var task = Task.Run(func); 

            task.ConfigureAwait(false);
            task.GetAwaiter().GetResult();
            return ;
        }
    }
}
