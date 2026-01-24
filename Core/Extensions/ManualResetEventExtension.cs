////--------------------------------------------------------------------
//// © Copyright 1989-2017 Edgenet, Inc. - All rights reserved.
//// This file contains confidential and proprietary trade secrets of
//// Edgenet, Inc.  Reproduction, disclosure or use without specific 
//// written authorization from Edgenet, Inc. is prohibited.
//// For more information see: http://www.edgenet.com
////--------------------------------------------------------------------

//using System;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Drawing.Imaging;
//using System.IO;
//using System.Diagnostics;
//using System.Threading.Tasks;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text.RegularExpressions;


//using System.Reflection;
//using System.Threading;

//using System.ComponentModel;

//using System.Net;
//using System.Text;

//using System.Data;
//using System.Xml;
//using System.Linq;



//namespace CadQuest.Core.Extensions
//{
//    public static class ManualResetEventExtension
//    {
//        /// <summary>
//        /// Waits eventDelayInMilliseconds and calls DoEvents every eventDelayInMilliseconds 
//        /// </summary>
//        /// <param name="eventDelayInMilliseconds"></param>
//        /// <returns></returns>
//        //public static bool WaitWithEvents(this System.Threading.ManualResetEvent mre, int eventDelayInMilliseconds)
//        //{
//        //    return mre.WaitWithEvents(Timeout.Infinite, eventDelayInMilliseconds);
//        //}

//        ///// <summary>
//        ///// Waits eventDelayInMilliseconds and calls DoEvents every eventDelayInMilliseconds until delayInMilliseconds is up
//        ///// </summary>
//        ///// <param name="waitInMilliseconds"></param>
//        ///// <param name="eventDelayInMilliseconds"></param>
//        ///// <returns></returns>
//        //public static bool WaitWithEvents(this System.Threading.ManualResetEvent mre, int waitInMilliseconds, int eventDelayInMilliseconds)
//        //{
//        //    int waitCount = waitInMilliseconds / 10;

//        //    DateTime dtBegin = DateTime.Now;

//        //    bool waitDone = false;
//        //    do
//        //    {
//        //        waitDone = mre.WaitOne(eventDelayInMilliseconds);
//        //        if (!waitDone)
//        //        {

//        //            TimeSpan ts = DateTime.Now - dtBegin;
//        //            if (waitInMilliseconds != Timeout.Infinite && ts.TotalMilliseconds > waitInMilliseconds)
//        //                break;
//        //            else
//        //                WpfHelp.DoEvents();
//        //        }
//        //    } while (!waitDone);


//        //    return waitDone;
//        }



//        /// <summary>
//        /// Waits to be signaled and polls provided func and exits if true
//        /// Returns the return value of func which is always called at least once
//        /// </summary>
//        /// <param name="resEv"></param>
//        /// <param name="intervalInMilliseconds"></param>
//        /// <param name="func"></param>
//        /// <returns></returns>
//        public static bool WaitAndTest(this System.Threading.ManualResetEvent resEv, int intervalInMilliseconds, Func<bool> func)
//        {
//            //bool retval = false;
//            //bool result = false;
//            //do
//            //{
//            //    result = resEv.WaitOne(intervalInMilliseconds);
//            //    if( !result )
//            //    {
//            //        retval = func();
//            //        if( retval )
//            //        {
//            //            result = true;
//            //            break;
//            //        }
//            //    }

//            //} while (!result);

//            //return retval;

//            return resEv.WaitAndTest(Timeout.Infinite, intervalInMilliseconds, func);
//        }





//        /// <summary>
//        /// Waits to be signaled and polls provided func and exits if true
//        /// Returns the return value of func which is always called at least once
//        /// </summary>
//        /// <param name="mre"></param>
//        /// <param name="totalWaitInMilliseconds"></param>
//        /// <param name="intervalInMilliseconds"></param>
//        /// <param name="func"></param>
//        /// <returns></returns>
//        public static bool WaitAndTest(this System.Threading.ManualResetEvent mre, int totalWaitInMilliseconds, int intervalInMilliseconds, Func<bool> func)
//        {
//            int waitCount = totalWaitInMilliseconds / 10;

//            DateTime dtBegin = DateTime.Now;

//            bool waitDone = func();

//            while (!waitDone)
//            {
//                if (mre.WaitOne(intervalInMilliseconds))
//                    break;

//                TimeSpan ts = DateTime.Now - dtBegin;
//                if (totalWaitInMilliseconds != Timeout.Infinite && ts.TotalMilliseconds > totalWaitInMilliseconds)
//                    break;
//                else
//                    waitDone = func();
//            }

//            return waitDone;
//        }
//    }
//}
