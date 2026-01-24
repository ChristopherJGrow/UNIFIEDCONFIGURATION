//--------------------------------------------------------------------
// © Copyright 1989-2014 Edgenet, Inc. - All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Edgenet, Inc.  Reproduction, disclosure or use without specific 
// written authorization from Edgenet, Inc. is prohibited.
// For more information see: http://www.edgenet.com
//--------------------------------------------------------------------

using System;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using System.Reflection;
using System.Threading;

using System.ComponentModel;

using System.Net;
using System.Text;

using System.Data;
using System.Xml;
using System.Linq;



namespace Config.Core.Extensions
{

    public static class LinkedListNodeExtension
    {
        public static LinkedListNode<T> NextWithWrap<T>(this LinkedListNode<T> node)
        {            
            return node.Next ?? node.List.First;
        }

        public static LinkedListNode<T> PrevWithWrap<T>(this LinkedListNode<T> node)
        {
            return node.Previous ?? node.List.Last;
        }
    }
}
