////--------------------------------------------------------------------
//// © Copyright 1989-2015 Edgenet, LLC. - All rights reserved.
//// This file contains confidential and proprietary trade secrets of
//// Edgenet, LLC.  Reproduction, disclosure or use without specific 
//// written authorization from Edgenet, LLC. is prohibited.
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
//using System.Windows.Forms;

//using System.Reflection;
//using System.Threading;

//using System.ComponentModel;

//using System.Net;
//using System.Text;

//using System.Data;
//using System.Xml;
//using System.Linq;


//using CadQuest.Def;
//using CadQuest.Interfaces;
//using CadQuest.PInvoke;
//using CadQuest.Core;
//using CadQuest.Core.Threading;

//namespace CadQuest.Core.Extensions
//{
//    public static class DispatcherExtensions
//    {
//        /// <summary>
//        /// This allows lambdas to be passed to invoke directly without a cast or new delegate
//        /// </summary>
//        /// <param name="Control"></param>
//        /// <param name="Action"></param>
//        //public static Object Invoke(this System.Windows.Threading.Dispatcher myDispatcher, Action myAction)
//        //{
//        //    return myDispatcher.Invoke(myAction);
//        //}

//        //public static Object Invoke(this System.Windows.Threading.Dispatcher myDispatcher, System.Windows.Threading.DispatcherPriority dispatcherPriority, Action myAction)
//        //{
//        //    return myDispatcher.Invoke(myAction, dispatcherPriority);
//        //}

//        //public static TResult Invoke(this System.Windows.Threading.Dispatcher myDispatcher, Action myAction)
//        //{
//        //    return myDispatcher.Invoke(myAction);
//        //}

//        //public static TResult Invoke(this System.Windows.Threading.Dispatcher myDispatcher, System.Windows.Threading.DispatcherPriority dispatcherPriority, Action myAction)
//        //{
//        //    return myDispatcher.Invoke(myAction, dispatcherPriority);
//        //}

//        public static void Invoke(this System.Windows.Threading.Dispatcher myDispatcher, Action myAction)
//        {
//            myDispatcher.Invoke(myAction);
//        }

//        public static void Invoke(this System.Windows.Threading.Dispatcher myDispatcher, System.Windows.Threading.DispatcherPriority dispatcherPriority, Action myAction)
//        {
//            myDispatcher.Invoke(myAction, dispatcherPriority);
//        }

//        public static TResult Invoke<TResult>(this System.Windows.Threading.Dispatcher myDispatcher, Func<TResult> myAction)
//        {
//            return (TResult)myDispatcher.Invoke(myAction);
//        }

//        public static TResult Invoke<TResult>(this System.Windows.Threading.Dispatcher myDispatcher, System.Windows.Threading.DispatcherPriority dispatcherPriority, Func<TResult> myAction)
//        {
//            return (TResult)myDispatcher.Invoke(myAction, dispatcherPriority);
//        }

//        public static void BeginInvoke(this System.Windows.Threading.Dispatcher myDispatcher, Action myAction)
//        {
//            myDispatcher.BeginInvoke(myAction);
//        }

//        public static void BeginInvoke(this System.Windows.Threading.Dispatcher myDispatcher, System.Windows.Threading.DispatcherPriority dispatcherPriority, Action myAction)
//        {
//            myDispatcher.BeginInvoke(myAction, dispatcherPriority);
//        }

//        public static bool InvokeRequired(this System.Windows.Threading.Dispatcher myDispatcher)
//        {
//            return !myDispatcher.CheckAccess();
//        }

//        public static ISmartDispatcher SmartDispatcherCreate(this System.Windows.Threading.Dispatcher myDispatcher)
//        {
//            return SmartDispatcher.Create(myDispatcher);
//        }
//    }
//}
