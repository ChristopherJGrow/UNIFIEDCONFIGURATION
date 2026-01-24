////--------------------------------------------------------------------
//// © Copyright 1989-2014 Edgenet, Inc. - All rights reserved.
//// This file contains confidential and proprietary trade secrets of
//// Edgenet, Inc.  Reproduction, disclosure or use without specific 
//// written authorization from Edgenet, Inc. is prohibited.
//// For more information see: http://www.edgenet.com
////--------------------------------------------------------------------

//using System;
//using System.Drawing;
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




//namespace Sage.Shared.COR.Main.Extensions
//{
//    public static class ListViewExtension
//    {
//        /// <summary>
//        /// calls BeginUpdate and then EndUpdate inside the using block
//        /// </summary>
//        /// <param name="lv"></param>
//        /// <returns></returns>
//        public static Using SupressUpdating(this ListView lv)
//        {
//            return new Using(() => lv.BeginUpdate(), () => lv.EndUpdate());
//        }
//    }
//}
