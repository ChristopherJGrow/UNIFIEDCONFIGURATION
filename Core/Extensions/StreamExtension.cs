//--------------------------------------------------------------------
// © Copyright 1989-2017 Edgenet, Inc. - All rights reserved.
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
    public static class StreamExtension
    {
        /// <summary>
        /// Copyies the passed stream into the current stream
        /// </summary>
        /// <param name="streamOut"></param>
        /// <param name="streamIn"></param>
        /// <returns>bytes copied</returns>
        public static uint CopyFrom(this Stream streamOut, Stream streamIn)
        {
            return streamOut.CopyFrom(streamIn, 16384);
        }

        /// <summary>
        /// Copyies the passed stream into the current stream
        /// </summary>
        /// <param name="streamOut"></param>
        /// <param name="streamIn"></param>
        /// <param name="bufferSize"></param>
        /// <returns>bytes copied</returns>
        public static uint CopyFrom(this Stream streamOut, Stream streamIn, int bufferSize)
        {
            uint retval = 0;

            var buff = new byte[bufferSize];
            int size = buff.Length;
            int numBytes;
            while ((numBytes = streamIn.Read(buff, 0, size)) > 0)
            {
                streamOut.Write(buff, 0, numBytes);
                retval += (uint)numBytes;
            }

            return retval;
        }

        //http://www.java2s.com/Tutorial/CSharp/0300__File-Directory-Stream/StreamseekingSeekOriginCurrentSeekOriginBeginSeekOriginEnd.htm

        /// <summary>
        /// Impliments Seek for SeekOrigin.Begin and SeekOrigin.End without an offset
        /// SeekOrigin.Current will throw an exception
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static long Seek(this Stream stream, SeekOrigin origin)
        {
            long retval = 0;
            switch (origin)
            {
                case SeekOrigin.End:
                    //if (stream.Length>0)
                    retval = stream.Seek(0, SeekOrigin.End);
                    break;
                case SeekOrigin.Begin:
                    retval = stream.Seek(0, SeekOrigin.Begin);
                    break;

                case SeekOrigin.Current:
                    throw new Exception("Unable to Seek current without offset, try Seek(int offset, SeekOrigin from)");
                    break;
            }

            return retval;

        }

        /// <summary>
        /// Reads an entire file into memory and returns the data as bytes
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadAll(this Stream stream)
        {
            byte[] buffer;

            int length = (int)stream.Length;      // get file length
            buffer = new byte[length];            // create buffer
            int count;                            // actual number of bytes read
            int sum = 0;                          // total number of bytes read

            // read until Read method returns 0 (end of the stream has been reached)
            while ( (count = stream.Read(buffer, sum, length - sum)) > 0)
            {
                sum += count;  // sum is a buffer offset for next reading
            }
           
            return buffer;
        }
    }

}
