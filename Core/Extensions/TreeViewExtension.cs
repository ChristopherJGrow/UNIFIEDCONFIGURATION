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
//    public static class TreeViewExtension
//    {
//        /// <summary>
//        /// Calls BeginUpdate and then EndUpdate inside the using block
//        /// </summary>
//        /// <param name="lv"></param>
//        /// <returns></returns>
//        public static Using SupressUpdating(this TreeView tv)
//        {
//            return new Using(() => tv.BeginUpdate(), () => tv.EndUpdate());
//        }
//    }

//    public static class TreeNodeCollectionExtension
//    {
//        /// <summary>
//        /// Attempts to find the given node, if not found it creates the node
//        /// </summary>
//        /// <param name="nodes"></param>
//        /// <param name="key"></param>
//        /// <param name="retNode"></param>
//        /// <returns>true if node is found false if created</returns>
//        public static bool RequestValue(this TreeNodeCollection nodes, string text, out TreeNode retNode)
//        {
//            bool retval = false;

//            if (nodes.ContainsKey(text))
//            {
//                retNode = nodes[text];
//                retNode.Text = text;
//                retval = true;
//            }
//            else
//                retNode = nodes.Add(text,text);


//            return retval;
//        }


//        /// <summary>
//        /// Attempts to find the given node, if not found it creates the node
//        /// </summary>
//        /// <param name="nodes"></param>
//        /// <param name="key">unique value in this collection</param>
//        /// <param name="text">displayed text</param>
//        /// <param name="retNode"></param>
//        /// <returns>true if node is found false if created</returns>
//        public static bool RequestValue(this TreeNodeCollection nodes, string key, string text, out TreeNode retNode)
//        {
//            bool retval = false;

//            if (nodes.ContainsKey(key))
//            {
//                retNode = nodes[key];
//                retNode.Text = text;
//                retval = true;
//            }
//            else
//                retNode = nodes.Add(key, text);


//            return retval;
//        }


//        /// <summary>
//        /// Attempts to find the given node, if not found it creates the node
//        /// </summary>
//        /// <param name="nodes"></param>
//        /// <param name="key"></param>
//        /// <param name="retNode"></param>
//        /// <returns>true if node is found false if created</returns>
//        public static TreeNode RequestValue(this TreeNodeCollection nodes, string text)
//        {
//            TreeNode retval;


//            nodes.RequestValue( text, text,out retval);

//            return retval;
//        }


//        /// <summary>
//        /// Attempts to find the given node, if not found it creates the node
//        /// </summary>
//        /// <param name="nodes"></param>
//        /// <param name="key">unique value in this collection</param>
//        /// <param name="text">displayed text</param>
//        /// <param name="retNode"></param>
//        /// <returns>requested node</returns>
//        public static TreeNode RequestValue(this TreeNodeCollection nodes, string key, string text)
//        {
//            TreeNode retval;


//            nodes.RequestValue(key, text, out retval);

//            return retval;

//        }

//    }
//}
