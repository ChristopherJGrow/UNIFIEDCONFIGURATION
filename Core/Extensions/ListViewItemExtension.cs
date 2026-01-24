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

//namespace CadQuest.Core.Extensions
//{
//    public static class ListViewItemExtension
//    {
//        /// <summary>
//        /// Returns true if the MouseEventArgs indicate a click on the image
//        /// </summary>
//        /// <param name="lvItem"></param>
//        /// <param name="e"></param>
//        /// <returns></returns>
//        public static bool IsClickOnImage(this ListViewItem lvItem, MouseEventArgs e)
//        {
//            int xPos = lvItem.Position.X;
//            // Check if we are clicking on the image area of the list
//            bool bRetval = (e.Location.X > xPos && e.Location.X < xPos + lvItem.ImageList.ImageSize.Width);

//            return bRetval;
//        }


//    }
//}
