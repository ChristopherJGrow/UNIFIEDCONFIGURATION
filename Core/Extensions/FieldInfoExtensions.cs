//--------------------------------------------------------------------
// © Copyright 1989-2017 Edgenet, Inc. - All rights reserved.
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
    public static class FieldInfoExtensions
    {

        /// <summary>
        /// True if the given attribute is applied to the field
        /// </summary>
        /// <param name="field"></param>
        /// <param name="myType"></param>
        /// <returns></returns>
        public static bool HasAttribute(this FieldInfo field, Type myType)
        {
            bool bRetval = false;
            foreach (var myAttrib in field.GetCustomAttributes(true))
            {
                if (myType.IsInstanceOfType(myAttrib))
                {
                    bRetval = true;
                    break;
                }
            }
            return bRetval;
        }

        public static Attribute GetAttribute(this FieldInfo field, Type myType)
        {
            Attribute retval = null;
            foreach (var myAttrib in field.GetCustomAttributes(true))
            {
                if (myType.IsInstanceOfType(myAttrib) && typeof(Attribute).IsAssignableFrom(myType))
                {
                    retval = (Attribute)myAttrib;
                    break;
                }
            }
            return retval;
        }


        /// <summary>
        /// True if the given attrubtue is applied to the field itself or is in the field type's inheritance chain
        /// </summary>
        /// <param name="field"></param>
        /// <param name="myType"></param>
        /// <returns></returns>
        public static bool HasAttributeAnywhere(this FieldInfo field, Type myType)
        {
            bool bRetval = false;
            // Make sur the type of the field doesn't have the CqDoNotSerializeAttribute
            foreach (var myAttrib in field.GetCustomAttributes(true))
            {
                if (myType.IsInstanceOfType(myAttrib))
                {
                    bRetval = true;
                    break;
                }
            }
            if (!bRetval)
                bRetval = field.FieldType.HasAttributeAnywhere(myType);

            return bRetval;
        }

    }
}
