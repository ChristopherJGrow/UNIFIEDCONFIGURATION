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



//namespace Sage.Shared.COR.Main.Extensions
//{
//    public static class UIElementExtension
//    {
//        public static void UpdateCoerced(this System.Windows.UIElement element)
//        {
//            element.InvalidateVisual();
//            WpfHelp.DoEvents();

            
//            //control.Dispatcher.CurrentDispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new ThreadStart(() => { }));
//        }

//        public static bool InvokeRequired(this System.Windows.UIElement element)
//        {
//            return !element.Dispatcher.CheckAccess();
//        }


//    }
//}
