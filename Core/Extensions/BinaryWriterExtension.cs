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
    public static class BinaryWriterExtension
    {
        //http://www.java2s.com/Tutorial/CSharp/0300__File-Directory-Stream/StreamseekingSeekOriginCurrentSeekOriginBeginSeekOriginEnd.htm

        /// <summary>
        /// Impliments Seek for SeekOrigin.Begin and SeekOrigin.End without an offset
        /// SeekOrigin.Current will throw an exception
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static long Seek(this BinaryWriter bw, SeekOrigin origin)
        {
            long retval = 0;

            switch (origin)
            {
                case SeekOrigin.End:
                    //if (bw.BaseStream.Length > 0)
                    retval = bw.Seek(0, SeekOrigin.End);
                    break;
                case SeekOrigin.Begin:
                    retval = bw.Seek(0, SeekOrigin.Begin);
                    break;

                case SeekOrigin.Current:
                    throw new Exception("Unable to Seek current without offset, try Seek(int offset, SeekOrigin from)");
                    break;
            }

            return retval;

        }

    }
}
