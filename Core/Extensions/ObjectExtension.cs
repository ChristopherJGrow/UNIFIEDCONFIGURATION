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
    public static class ObjectExtension
    {
        public static bool IsNull(this System.Object thing)
        {
            return thing == null;
        }
        public static bool IsNotNull(this System.Object thing)
        {
            return !(thing == null);
        }

        /// <summary>
        /// Copies all public properties from source to our objects public properties
        /// This is a very shallow copy and does nothing to traverse sub objects  
        /// Does NOT consider private properties or private/public fields      
        /// </summary>        
        /// <param name="source">Object to copy public properties from</param>        
        //public static void CopyPropertiesFrom<TTAR, TSRC>(this TTAR targetObject, TSRC source)
        //     where TTAR : class
        //    where TSRC : class
        //{
        //    targetObject.CopyPropertiesFrom(source, false);
        //}

        ///// <summary>
        ///// Copies all public properties from source to our objects public properties
        ///// This is a very shallow copy and does nothing to traverse sub objects  
        ///// Does NOT consider private properties or private/public fields      
        ///// </summary>        
        ///// <param name="source">Object to copy public properties from</param>        
        ///// <param name="bUseActualType">if true gets member list from enstantiated type rather than declared type</param>
        //public static void CopyPropertiesFrom<TTAR, TSRC>(this TTAR targetObject, TSRC source, bool bUseActualTypes)
        //    where TTAR : class
        //    where TSRC : class
        //{
        //    TypeMapper.CopyLikeProperties( source, targetObject);

        //    //var targetType = typeof(TTAR);
        //    //var sourceType = typeof(TSRC);

        //    //if (bUseActualTypes)
        //    //{
        //    //    targetType = targetObject.GetType();
        //    //    sourceType = source.GetType();
        //    //}



        //    //PropertyInfo[] allProporties = sourceType.GetProperties();
        //    //PropertyInfo targetProperty;

        //    //foreach (PropertyInfo fromProp in allProporties)
        //    //{
        //    //    targetProperty = targetType.GetProperty(fromProp.Name);

        //    //    if (!fromProp.CanRead) continue;
        //    //    if (targetProperty == null) continue;
        //    //    if (!targetProperty.CanWrite) continue;
        //    //    if (!targetProperty.PropertyType.IsAssignableFrom(fromProp.PropertyType)) continue;


        //    //    targetProperty.SetValue(targetObject, fromProp.GetValue(source, null), null);

        //    //    //CqDiag.always("Set Property {0} to {1}",fromProp.Name,fromProp.GetValue(source,null) ?? "NULL");
        //    //}
        //}

    }
}
