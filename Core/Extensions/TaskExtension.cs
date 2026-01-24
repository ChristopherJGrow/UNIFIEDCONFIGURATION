//--------------------------------------------------------------------
// © Copyright 1989-2019 Syndigo, LLC All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Edgenet, Inc.  Reproduction, disclosure or use without specific 
// written authorization from Syndigo, LLC is prohibited.
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
using System.Runtime.CompilerServices;


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
    public static class TaskExtension
    {

        /// <summary>
        /// Combines two arrays of the same type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrayFirst"></param>
        /// <param name="arraySecond"></param>
        /// <returns></returns>
        public static async void Wait(this Task[] tasks)
        {
            foreach (var task in tasks)
            {
                await task;
            }
        }

    


    }
}
