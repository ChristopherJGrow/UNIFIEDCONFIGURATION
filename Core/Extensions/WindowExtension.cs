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
////using System.Windows.Forms;

//using System.Windows.Controls;
//using System.Windows;

//using System.Reflection;
//using System.Threading;

//using System.ComponentModel;

//using System.Net;
//using System.Text;

//using System.Data;
//using System.Xml;
//using System.Linq;

//using System.Windows.Interop;

//using CadQuest.Def;
//using CadQuest.Interfaces;
//using CadQuest.PInvoke;
//using CadQuest.Core;
//using CadQuest.Core.Threading;




//namespace Sage.Shared.COR.Main.Extensions
//{

//    public static class WindowExtension
//    {
//        public static void SendToBack(this System.Windows.Window win)
//        {
//            var hWnd = new WindowInteropHelper(win).Handle;

//            CadQuest.PInvoke.CWIN32.USER32.SetWindowPos(hWnd, CWIN32.USER32.HWND_BOTTOM, 0, 0, 0, 0, CWIN32.USER32.SWP_NOSIZE | CWIN32.USER32.SWP_NOMOVE);

//        }

//        public static void BringToFront(this Window win)
//        {
//            win.WindowState = System.Windows.WindowState.Normal;
//            win.Activate();
//            win.Topmost = true;
//            win.Topmost = false;
//            win.Focus();

//        }     
     
//    }

    
//}