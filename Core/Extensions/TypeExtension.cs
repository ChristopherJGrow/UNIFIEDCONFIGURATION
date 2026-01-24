//--------------------------------------------------------------------
// © Copyright 1989-2014 Edgenet, Inc. - All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Edgenet, Inc.  Reproduction, disclosure or use without specific 
// written authorization from Edgenet, Inc. is prohibited.
// For more information see: http://www.edgenet.com
//--------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;


using System.Reflection;
using System.Threading;

using System.ComponentModel;

using System.Net;
using System.Text;

using System.Data;
using System.Xml;
using System.Linq;
using System.Runtime.CompilerServices;
//using Sage.Shared.COR.Main.Threading;

namespace Config.Core.Extensions
{
    public static class TypeExtension
    {
        /// <summary>
        /// Finds if the given attribute is in the type specified
        /// </summary>
        /// <param name="myType"></param>
        /// <param name="attribType"></param>
        /// <returns></returns>
        public static bool HasAttribute(this Type myType, Type attribType)
        {
            bool bRetval = false;

            foreach (Object myAttrib in myType.GetCustomAttributes(true))            
            {
                if (attribType.IsInstanceOfType(myAttrib))
                    bRetval = true;
            }

            return bRetval;
        }

        /// <summary>
        /// Finds if the given attribute is present anywhere in the inheritance
        /// </summary>
        /// <param name="myType"></param>
        /// <param name="attribType"></param>
        /// <param name="bRecursive"></param>
        /// <returns></returns>
        public static bool HasAttributeAnywhere(this Type myType, Type attribType)
        {
            bool bRetval = false;

            List<Type> inheritanceList = new List<Type>();

            Type aType = myType;

            while (aType != null)
            {
                inheritanceList.Add(aType);
                aType = aType.BaseType;
            }

            foreach (var curType in inheritanceList)
            {
                foreach (Object myAttrib in curType.GetCustomAttributes(true))
                {
                    if (attribType.IsInstanceOfType(myAttrib))
                    {
                        bRetval = true;
                        break;
                    }
                }
                if (bRetval == true)
                    break;
            }

            return bRetval;
        }


        //a thread-safe way to hold default instances created at run-time
        private static ConcurrentDictionary<Type, object> __typeDefaults = new ConcurrentDictionary<Type, object>();


        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static object GetDefaultValue(this Type type)
        {
            object retval = null;
            if (type.IsValueType)
            {
                //__typeDefaults.TryGetValue(type, out retval, () => Activator.CreateInstance(type));
                retval = __typeDefaults.GetOrAdd(type, (TYPE) => Activator.CreateInstance(TYPE));
            }
            return retval;
        }



    }

}
