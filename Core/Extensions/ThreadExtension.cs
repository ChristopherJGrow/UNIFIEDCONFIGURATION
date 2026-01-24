////--------------------------------------------------------------------
//// © Copyright 1989-2014 Edgenet, Inc. - All rights reserved.
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

//namespace Sage.Shared.COR.Main.Extensions
//{
//    public static class ThreadExtension
//    {
//        public static bool HasDispatcher(this System.Threading.Thread thread)
//        {
//            return DispatcherEx.CurrentDispatcherEnabled();
//        }

//        public static bool IsMainThread(this System.Threading.Thread thread)
//        {
//            // this is ugly but the only way without a ui element
//            var retval = System.Threading.Thread.CurrentThread.Name.Equals("User Interface Thread", StringComparison.InvariantCultureIgnoreCase);
//            return retval;
//        }

//    }
//}
