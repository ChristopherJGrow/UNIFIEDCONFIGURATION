//--------------------------------------------------------------------
// © Copyright 1989-2015 Edgenet, LLC. - All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Edgenet, LLC.  Reproduction, disclosure or use without specific 
// written authorization from Edgenet, LLC. is prohibited.
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

using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Config.Core.Conversion;




namespace Config.Core.Extensions
{
    public static class XmlWriterExtension
    {
        public static Using WriteDocument(this XmlWriter pXmlWriter)
        {
            return new Using(() => pXmlWriter.WriteStartDocument(), () => pXmlWriter.WriteEndDocument());
        }


        public static Using WriteElements(this XmlWriter pXmlWriter, string element)
        {
            return new Using(() => pXmlWriter.WriteStartElement(element), () => pXmlWriter.WriteEndElement());
        }

        public static Using WriteElements(this XmlWriter pXmlWriter, string element, string nameSpace)
        {
            return new Using(() => pXmlWriter.WriteStartElement(element, nameSpace), () => pXmlWriter.WriteEndElement());
        }

        public static Using WriteElements(this XmlWriter pXmlWriter, string prefix, string element, string nameSpace)
        {
            return new Using(() => pXmlWriter.WriteStartElement(prefix, element, nameSpace), () => pXmlWriter.WriteEndElement());
        }

        public static void WriteElementsWithValue(this XmlWriter pXmlWriter, string value, string element)
        {
            pXmlWriter.WriteStartElement(element);
            pXmlWriter.WriteValue(value);
            pXmlWriter.WriteEndElement();
        }

    }


    public static class XmlDocumentExtension
    {
        public static string GetXml(this XmlDocument doc)
        {            
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            doc.WriteTo(xw);

            return sw.ToString();
        }
    }

    //public static class XmlReaderExtension
    //{

    //    public static void SkipToEndofNode(this XmlReader xr)
    //    {
    //        var name = xr.LocalName;

    //        while (!(xr.NodeType == XmlNodeType.EndElement && xr.LocalName == name) && xr.Read() == true) ;
    //    }

    //}

    public static class XElementExtension
    {
        /// <summary>
        /// Provides the element value if it exists or the default
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="name"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public static T ElementValue<T>(this XElement x, string name, T defValue)
        {
            T retval = defValue;
            var element = x.Element(name);
            if (element != null)
                retval = ConvertEx.From<T>(element.Value,defValue);

            return retval;
        }
    }
}
