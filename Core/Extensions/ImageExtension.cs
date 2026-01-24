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
//    public static class ImageExtension
//    {
//        //public static Image Resize(this Image imgPhoto, System.Windows.Forms.PictureBoxSizeMode sizeMode, int Width, int Height)
//        //{
//        //    return ImageEx.ResizeImage(sizeMode, imgPhoto, Width, Height);
//        //}


//        public static Stream ToStream(this Image image, ImageFormat format)
//        {
//            var stream = new System.IO.MemoryStream();
//            //image.Save(stream, formaw);

//            image.Save(stream, format);

//            stream.Position = 0;
//            return stream;
//        }

//        //private static ImageCodecInfo GetEncoderInfo(String mimeType)
//        //{
//        //    int j;
//        //    ImageCodecInfo[] encoders;
//        //    encoders = ImageCodecInfo.GetImageEncoders();
//        //    for (j = 0; j < encoders.Length; ++j)
//        //    {
//        //        if (encoders[j].MimeType == mimeType)
//        //            return encoders[j];

//        //        encoders[j].
//        //    }
//        //    return null;
//        //}

//    }

//    public static class ImageFormatExtension
//    {
//        public static string GetFileExtension(this ImageFormat fmt)
//        {
//            string retval = ".idk";

//            if (fmt.Guid == ImageFormat.Bmp.Guid)
//                retval = ".bmp";
//            else if (fmt.Guid == ImageFormat.Emf.Guid)
//                retval = ".emf";
//            else if (fmt.Guid == ImageFormat.Exif.Guid)
//                retval = ".exif";
//            else if (fmt.Guid == ImageFormat.Gif.Guid)
//                retval = ".gif";
//            else if (fmt.Guid == ImageFormat.Icon.Guid)
//                retval = ".ico";
//            else if (fmt.Guid == ImageFormat.Jpeg.Guid)
//                retval = ".jpg";
//            else if (fmt.Guid == ImageFormat.MemoryBmp.Guid)
//                retval = ".bmp";
//            else if (fmt.Guid == ImageFormat.Png.Guid)
//                retval = ".png";
//            else if (fmt.Guid == ImageFormat.Tiff.Guid)
//                retval = ".tip";
//            else if (fmt.Guid == ImageFormat.Wmf.Guid)
//                retval = ".wmf";
         
//            return retval;
//        }
//    }
//}
