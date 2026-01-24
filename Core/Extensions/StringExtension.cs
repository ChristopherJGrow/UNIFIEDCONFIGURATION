//--------------------------------------------------------------------
// © Copyright 1989-2022 Syndigo, LLC. - All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Syndigo, LLC.  Reproduction, disclosure or use without specific 
// written authorization from Syndigo, LLC. is prohibited.
// For more information see: http://www.syndigo.com
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
using System.Runtime.CompilerServices;
using Config.Core.Conversion;



namespace Config.Core.Extensions
{
	/// <summary>
	/// Yet Another Monkey Patching class
	/// </summary>
	public static class StringExtension
	{

		private static Regex __IsGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);



		public static string ToInternString(this string pStr)
		{

			return string.Intern(pStr);
		}

        /// <summary>
        /// Checks string contains a guid
        /// </summary>
        /// <param name="pStr"></param>
        /// <returns></returns>
        /// 
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool IsGuid(this string pStr)
		{            
			bool retval = __IsGuid.IsMatch(pStr);
			return retval;
		}


        /// <summary>
        /// Adds the ability to use a StringComparison to Contains.  Uses IndexOf to determine if val is in the string
        /// </summary>
        /// <param name="org"></param>
        /// <param name="contains"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static bool Contains(this string org, string val, StringComparison comparisonType)
        {
            return org.IndexOf(val, comparisonType) != -1;
        }

        /// <summary>
        /// If string is a guid then returns a guid othersize an empty gu_id
        /// </summary>
        /// <param name="pStr"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Guid ToGuid(this string pStr)
		{
			return new Guid(pStr);
			//if (pStr.IsGuid())
			//    return new Guid(pStr);
			//else
			//    return Guid.Empty;
		}

		/// <summary>
		/// Converts string to a memory stream to use with things that need stream objects
		/// </summary>
		/// <param name="pStr"></param>
		/// <returns></returns>
		public static Stream ToStream(this string pStr)
		{
			var stream = new MemoryStream();


			// Convert the text into a byte array so that 
			// it can be loaded into the memory stream.
			byte[] bytes = Encoding.UTF8.GetBytes(pStr);

			// Write the XAML bytes into a memory stream.
			stream.Write(bytes, 0, bytes.Length);

			// Reset the stream's current position back 
			// to the beginning so that when it is read 
			// from, the read begins at the correct place.
			stream.Position = 0;

			return stream;
		}

		// This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
		// This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
		// 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
		private const string initVector = "tu89geji340t89u2";

		// This constant is used to determine the keysize of the encryption algorithm.
		private const int keysize = 256;

        //public static string Encrypt(this string plainText, string passPhrase = "HappyHappyJoyJoy")
        //{


        //	var crypto = CqCrypto.singleInstance;

        //	var retval = crypto.Encrypt(plainText, passPhrase);

        //	return retval;
        //}

        //public static string Decrypt(this string cipherText, string passPhrase="HappyHappyJoyJoy")
        //{
        //	var crypto = CqCrypto.singleInstance;

        //	var retval = crypto.Decrypt(cipherText, passPhrase);

        //	return retval;
        //}



        //public static string Encrypt(this string source)
        //{
        //    string retval = "";

        //    using (var memstrm = new MemoryStream() )
        //    {

        //        var crypto = CqCrypto.getSingleInstance();



        //        using (var cs = crypto.encodeStreamCreate(memstrm))
        //        {

        //            var swEncrypt = new StreamWriter(cs);





        //            swEncrypt.Write(source);

        //            //cs.Write(targetBy);



        //            //cs.Flush();
        //            //cs.Close();
        //        }

        //        byte[] targetBytes = memstrm.ToArray();


        //        retval = Convert.ToBase64String(targetBytes);




        //    }

        //    return retval;
        //}

        //public static string Decrypt(this string source)
        //{
        //    string retval = "";

        //    byte[] sourceBytes = Convert.FromBase64String(source);

        //    using( var memstrm = new MemoryStream(sourceBytes) )
        //    {
        //        memstrm.Seek(SeekOrigin.Begin);


        //        var crypto = CqCrypto.getSingleInstance();

        //        using (var ds = crypto.decodeStreamCreate(memstrm))
        //        {
        //            //var sr = new StreamReader(ds);

        //            var plainTextBytes = new byte[sourceBytes.Length*2];
        //            var decryptedByteCount = ds.Read(plainTextBytes, 0, sourceBytes.Length);

        //            ds.Close();

        //            memstrm.Close();

        //            retval = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);


        //            //var sr = new StreamReader(ds);


        //            //sr.BaseStream.Seek(SeekOrigin.Begin);

        //            //ds.Read(sourceBytes, 0, sourceBytes.Length);

        //            //var sr = new StreamReader(ds);

        //            //ds.Flush();

        //            //ds.Close();


        //            //byte[] targetBytes = memstrm.ToArray();

        //            //retval = sr.ReadToEnd();

        //            //sr.Close();

        //            //retval = sr.ReadToEnd();

        //            //sr.Close();

        //            //List<byte> readBytes = new List<byte>();

        //            //for(var readVal = ds.ReadByte() ; readVal!=-1; readVal = ds.ReadByte() )
        //            //{
        //            //    readBytes.Add((byte)readVal);
        //            //}


        //            //swEncrypt.Write(source);

        //            //cs.Flush();
        //            //cs.Close();
        //        }

        //        //byte[] targetBytes = memstrm.ToArray();


        //        //retval = Convert.ToBase64String(targetBytes);




        //    }


        //    return retval;

        //}
        public static string Enquote(this string UnquotedString)
        {
            if (UnquotedString[0] != '"')
            {
                UnquotedString = "\"" + UnquotedString;
            }

            if (UnquotedString[UnquotedString.Length - 1] != '"')
            {
                UnquotedString = UnquotedString + "\"";
            }

            return UnquotedString;
        }

        public static string Dequote(this string EnquotedString)
        {
            if (EnquotedString != null && EnquotedString.Length > 1)
            {
                if (EnquotedString[0] == '"')
                {
                    EnquotedString = EnquotedString.Substring(1);
                }

                if (EnquotedString[EnquotedString.Length - 1] == '"')
                {
                    EnquotedString = EnquotedString.Substring(0, EnquotedString.Length - 1);
                }
            }

            return EnquotedString;
        }


        /// <summary>
        /// Ensures the string ends with a specified character
        /// </summary>
        /// <param name="pStr"></param>
        /// <param name="ending"></param>
        /// <returns></returns>
        public static string EnsureEnding( this string pStr, char ending)
        {
            if (pStr[pStr.Length - 1] != ending)
                pStr += ending;

            return pStr; ;
        }

        /// <summary>
        /// Reverses the characters in a string.  NOT Linq Reverse
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Reverse(this string str)
        {
            return new string( str.ToArray().Reverse().ToArray() );
        }

        /// <summary>
        /// Converts Markup (Xml, Xaml, Html) bad characters to their encoded versions
        /// quotechar=&quot; '=&apos; <=&lt; >=&gt; &=&amp;
        /// </summary>
        /// <param name="pStr"></param>
        /// <returns></returns>
        public static string MarkupEncode(this string pStr)
		{
			string pRetStr = pStr;

			if (pStr.IsNotNullOrEmpty())
			{
				var sb = new StringBuilder(pStr, pStr.Length + 100);

				sb.Replace("\"", "&quot;");
				sb.Replace("'", "&apos;");
				sb.Replace("<", "&lt;");
				sb.Replace(">", "&gt;");
				sb.Replace("&", "&amp;");

				pRetStr = sb.ToString();
			}


			return pRetStr;
		}

        /// <summary>
        /// Splits and perform operation on each element
        /// </summary>
        /// <param name="str"></param>
        /// <param name="chSplit"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public string[] Split(this string str, char chSplit, Func<string, string> op)
		{
			string[] retval = str.Split(chSplit);

			int cLoop;
			for (cLoop = 0; cLoop < retval.Length; cLoop++)
			{
				retval[cLoop] = op(retval[cLoop]);
			}

			return retval;
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public string FormatWith(this string format, params object[] args)
		{
			return string.Format(format, args);
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public int[] IndexOfChars(this string str, char chToLocate)
		{
			return StringHelp.IndexOfChars(str, chToLocate);
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public bool IsNullOrEmpty(this string str)
		{
			return String.IsNullOrEmpty(str);
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public bool IsNotNullOrEmpty(this string str)
		{
			return !String.IsNullOrEmpty(str);
		}

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public bool IsNullOrWhiteSpace(this string str)
        {            
            return String.IsNullOrWhiteSpace(str);
        }

        #region Section

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public bool hasSection(this string str, int iSectionNumber)
		{
			return StringHelp.Section(str, iSectionNumber).Length > 0;
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public bool hasSection(this string str, int iSectionNumber, char separator)
		{
			return StringHelp.Section(str, iSectionNumber, separator).Length > 0;
		}

        /// <summary>
        /// Removes n sections from the right side of the string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="cSectionNumber">One based number of sections to remove</param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public string SectionTrimRight(this string str, int iSectionNumber)
		{
			return StringHelp.SectionTrimRight(str, iSectionNumber, ',');
		}

        /// <summary>
        /// Removes n sections from the right side of the string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="cSectionNumber">One based number of sections to remove</param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public string SectionTrimRight(this string str, int cSectionNumber, char separator)
		{
			return StringHelp.SectionTrimRight(str, cSectionNumber, separator);
		}

        /// <summary>
        /// Removes n sections from the left side of the string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="cSectionNumber">One based number of sections to remove</param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public string SectionTrimLeft(this string str, int cSectionNumber)
		{
			return StringHelp.SectionTrimLeft(str, cSectionNumber, ',');
		}

        /// <summary>
        /// Removes n sections from the left side of the string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="cSectionNumber">One based number of sections to remove</param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public string SectionTrimLeft(this string str, int iSectionNumber, char separator)
		{
			return StringHelp.SectionTrimLeft(str, iSectionNumber, separator);
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public int SectionCount(this string str)
		{
			return StringHelp.SectionCount(str);
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public int SectionCount(this string str, char separator)
		{
			return StringHelp.SectionCount(str, separator);
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static string Section(this string str, int iSectionNumber)
		{
			return StringHelp.Section(str, iSectionNumber);
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static string Section(this string str, int iSectionNumber, char separator)
		{
			return StringHelp.Section(str, iSectionNumber, separator);
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public string SectionRemainder(this string str, int iSectionNumber)
		{
			return StringHelp.SectionRemainder(str, iSectionNumber);
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public string SectionRemainder(this string str, int iSectionNumber, char separator)
		{
			return StringHelp.SectionRemainder(str, iSectionNumber, separator);
		}
       

        /// <summary>
        /// Looks from the right for instance of trimChar and grabs everything to the left of it.
        /// Example "a.b.c.d".LeftOfLast('.',1)
        /// Result "a.b"
        /// </summary>
        /// <param name="str"></param>
        /// <param name="trimChar">character to trim</param>
        /// <param name="iSkipCount">number of characters to skip</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public string LeftOfLast(this string str, char trimChar, int iSkipCount=0)
	    {
            string retval = str;
            int foundCount = 0;
            for (int cLoop = str.Length - 1; cLoop > -1; --cLoop)
            {               
                if (trimChar== str[cLoop])
                {
                    ++foundCount;

                    if (foundCount == iSkipCount + 1)
                    {
                        retval = str.Left(cLoop);
                        break;
                    }
                }                             
            }

            return retval ;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public string RightOfLast(this string str, char trimChar, int iSkipCount = 0)
        {
            string retval = str;
            int foundCount = 0;
            for (int cLoop = str.Length - 1; cLoop > -1; --cLoop)
            {
                if (trimChar == str[cLoop])
                {
                    ++foundCount;

                    if (foundCount == iSkipCount + 1)
                    {
                        if (str.Length > 1)
                            retval = str.Substring(cLoop + 1);
                        else
                            retval = "";
                        break;
                    }
                }
            }

            return retval;
        }

        #endregion


        static public int[] IndexesOfAll(this string str, char separator)
        {
            return StringHelp.IndexesOfAll(str, new char[] { separator });
        }

        static public int[] IndexesOfAll(this string str, char[] separators)
	    {
	        return StringHelp.IndexesOfAll(str,separators);
	    }


        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public string Left(this string myStr, int length)
		{
			return StringHelp.Left(myStr, length);
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public string Right(this string myStr, int length)
		{
			return StringHelp.Right(myStr, length);
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public int RightCompare(this string str1, string str2)
		{
			return StringHelp.RightCompare(str1, str2);
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public int RightCompare(this string str1, string str2, StringComparison sc)
		{
			return StringHelp.RightCompare(str1, str2, sc);
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public bool IsNaturalNumber(this string strNumber)
		{
			return StringHelp.IsNaturalNumber(strNumber);
		}

        // Function to test for Positive Integers with zero inclusive
        [MethodImpl( MethodImplOptions.AggressiveInlining )]

        static public bool IsWholeNumber(this string strNumber)
		{
			return StringHelp.IsWholeNumber(strNumber);
		}

        // Function to Test for Integers both Positive & Negative
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public bool IsInteger(this string strNumber)
		{
			return StringHelp.IsInteger(strNumber);
		}

        // Function to Test for Positive Number both Integer & Real
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public bool IsPositiveNumber(this string strNumber)
		{
			return StringHelp.IsPositiveNumber(strNumber);
		}

        // Function to test whether the string is valid number or not
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public bool IsNumber(this string strNumber)
		{
			return StringHelp.IsNumber(strNumber);
		}

        // Function To test for Alphabets.
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public bool IsAlpha(this string strToCheck)
		{
			return StringHelp.IsAlpha(strToCheck);
		}

        // Function to Check for AlphaNumeric.
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public bool IsAlphaNumeric(this string strToCheck)
		{
			return StringHelp.IsAlpha(strToCheck);
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public bool ToBool(this string strToConvert)
		{
			return ConvertEx.ToBoolean(strToConvert);
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public bool IsBool(this string strToConvert)
		{
			return ConvertEx.IsBoolean(strToConvert);
		}


        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public T ToEnum<T>(this string strToConvert, T defaultVal)
		{
			var retval = default(T);

			try
			{
                if ( strToConvert.IsNotNullOrEmpty() )
                    retval = CEnumParserFast<T>.Parse( strToConvert );
                    //retval = (T)Enum.Parse(typeof(T), strToConvert, true);
                else
                    retval = defaultVal;
			}
			catch (Exception)
			{
				retval = defaultVal;
			}


			return retval;

		}

		/// <summary>
		/// Given an array of chars replaces all them with newThing
		/// </summary>
		/// <param name="strToConvert"></param>
		/// <param name="oldChars"></param>
		/// <param name="newChar"></param>
		/// <returns></returns>
		public static string Replace(this string strToConvert, char[] oldChars, char newChar)
		{
			string retval = strToConvert;

			if (strToConvert.IsNotNullOrEmpty())
			{
				var sb = new StringBuilder(strToConvert, strToConvert.Length + 100);

				foreach (char myVal in oldChars)
				{
					sb.Replace(myVal, newChar);
				}
				retval = sb.ToString();
			}
			return retval;
		}

		/// <summary>
		/// Removes passed character from the string
		/// </summary>
		/// <param name="strToConvert"></param>
		/// <param name="removeChar"></param>
		/// <returns></returns>
		public static string RemoveChar(this string strToConvert, char removeChar)
		{
			StringBuilder retval = new StringBuilder(strToConvert);

			int cLoop;
			for (cLoop = 0; cLoop < retval.Length; cLoop++)
			{
				if (retval[cLoop] == removeChar)
					retval.Remove(cLoop--, 1);      // dec happens after call, precidence

			};


			return retval.ToString();
		}

		/// <summary>
		/// Removes passed characters from the sring
		/// </summary>
		/// <param name="strToConvert"></param>
		/// <param name="removeChars"></param>
		/// <returns></returns>
		public static string RemoveChar(this string strToConvert, char[] removeChars)
		{
			StringBuilder retval = new StringBuilder(strToConvert);

			int cLoopOuter;
			int cLoop;

			for (cLoopOuter = 0; cLoopOuter < removeChars.Length; cLoopOuter++)
			{

				for (cLoop = 0; cLoop < retval.Length; cLoop++)
				{
					if (retval[cLoop] == removeChars[cLoopOuter])
						retval.Remove(cLoop--, 1);    // dec happens after call, precidence

				};
			}

			return retval.ToString();
		}

        /// <summary>
        /// To Bool conversion and if string is null or empty returns defaultVal
        /// </summary>
        /// <param name="strToConvert"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
         [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public bool ToBool(this string strToConvert, bool defaultVal)
		{
			if (strToConvert.IsNullOrEmpty())
				return defaultVal;
			else
				return ConvertEx.ToBoolean(strToConvert);
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public Decimal ToDecimal(this string strToConvert)
		{
			Decimal retval = new Decimal();
			Decimal.TryParse(strToConvert, out retval);

			return retval;
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static Decimal ToDecimal(this string strToConvert, Decimal defaultVal)
		{
			Decimal retval ;

		    return Decimal.TryParse(strToConvert, out retval) ? retval : new Decimal();
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public int ToInt(this string strToConvert)
		{
			//return Convert.ToInt32(strToConvert);
			int retval=0;

			Int32.TryParse(strToConvert, out retval);

			return retval;
		}

        /// <summary>
        /// To Int conversion and if string is null or empty returns defaultVal
        /// </summary>
        /// <param name="strToConvert"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public int ToInt(this string strToConvert, int defaultVal)
		{			
			int retval ;

		    return Int32.TryParse(strToConvert, out retval) ? retval : defaultVal;
		}

        // moved to EN.ODP.COM.Main

  //      [MethodImpl( MethodImplOptions.AggressiveInlining )]
  //      public static CQLENGTH ToCQLENGTH(this string strToConvert)
		//{
		//	return CQLENGTH.Parse(strToConvert);
		//}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static uint ToUInt(this string strToConvert)
		{			

			uint retval = 0;

			UInt32.TryParse(strToConvert, out retval);

			return retval;
		}


        /// <summary>
        /// To UInt conversion and if string is null or empty returns defaultVal
        /// </summary>
        /// <param name="strToConvert"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static uint ToUInt(this string strToConvert, uint defaultVal)
		{
			uint retval ;

		    return UInt32.TryParse(strToConvert, out retval) ? retval : defaultVal;
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static long ToLong(this string strToConvert)
		{
			//return Convert.ToInt64(strToConvert);

			long retval = 0;

			Int64.TryParse(strToConvert, out retval);

			return retval;

		}


        /// <summary>
        /// To Long conversion and if string is null or empty returns defaultVal
        /// </summary>
        /// <param name="strToConvert"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public long ToLong(this string strToConvert, long defaultVal)
		{
			long retval ;

		    return long.TryParse(strToConvert, out retval) ? retval : defaultVal;
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public ulong ToULong(this string strToConvert)
		{
			ulong retval = 0;

			ulong.TryParse(strToConvert, out retval);

			return retval;
		}


        /// <summary>
        /// To ULong conversion and if string is null or empty returns defaultVal
        /// </summary>
        /// <param name="strToConvert"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public ulong ToULong(this string strToConvert, ulong defaultVal)
		{
			ulong retval ;

		    return ulong.TryParse(strToConvert, out retval) ? retval : defaultVal;
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float ToFloat(this string strToConvert)
        {            

            float retval = 0.0f;

            float.TryParse( strToConvert, out retval );

            return retval;
        }


        /// <summary>
        /// To Float conversion and if string is null or empty returns defaultVal
        /// </summary>
        /// <param name="strToConvert"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static float ToFloat(this string strToConvert, float defaultVal)
		{
			float retval ;		

		    return float.TryParse(strToConvert, out retval) ? retval : defaultVal;
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static double ToDouble(this string strToConvert)
		{
			//return Convert.ToDouble(strToConvert);

			double retval = 0.0;

			double.TryParse(strToConvert, out retval);

			return retval;
		}


        /// <summary>
        /// To Double conversion and if string is null or empty returns defaultVal
        /// </summary>
        /// <param name="strToConvert"></param>
        /// <param name="defaultVal"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public double ToDouble(this string strToConvert, double defaultVal)
		{
			double retval ;

		    return double.TryParse(strToConvert, out retval) ? retval : defaultVal;
		}


        /// <summary>
        /// Returns a date representation of a string.
        /// If the string isn't a date the returned value will be DateTime.MinValue
        /// </summary>
        /// <param name="strToConvert"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public DateTime ToDateTime(this string strToConvert)
		{
			DateTime retval = DateTime.MinValue;

			DateTime.TryParse(strToConvert, out retval);
								
			return retval;
		}


        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public byte[] ToBytesUTF8(this string strToConvert)
		{
			return new System.Text.UTF8Encoding().GetBytes(strToConvert);
		}
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public byte[] ToBytesUTF8(this string strToConvert, bool encoderShouldEmitUTF8Identifier)
		{
			return new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier).GetBytes(strToConvert);
		}
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public byte[] ToBytesUTF8(this string strToConvert, bool encoderShouldEmitUTF8Identifier, bool throwOnInvalidBytes)
		{
			return new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes).GetBytes(strToConvert);
		}

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public byte[] ToBytesASCII(this string strToConvert)
		{
			return new System.Text.ASCIIEncoding().GetBytes(strToConvert);
		}
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public string ReplaceIgnoreCase(this string original, string pattern, string replacement)
		{
			return StringHelp.ReplaceIgnoreCase(original, pattern, replacement);
		}

        /// <summary>
        /// Encodes characters & \" ' < and > 
        /// </summary>
        /// <param name="strToConvert"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public string XmlEscape(this string strToConvert)
		{
			return XmlHelp.Escape(strToConvert);
		}

        /// <summary>
        /// Decodes &amp; &quot; &apos; &lt; and &gt;
        /// </summary>
        /// <param name="strToConvert"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public string XmlUnEscape(this string strToConvert)
		{
			return XmlHelp.UnEscape(strToConvert);
		}



        /// <summary>
        /// Emails are so common now that it makes sense to have this in here..
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public bool IsValidEmail(this string str)
		{
			bool retval = false;

			if (str != null)
			{
				// http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx/

				string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
								+ @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
								+ @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

				Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);


				retval = regex.IsMatch(str);
			}


			return retval;
		}


        /// <summary>
        /// Padds and truncates a string to the requested width in characters
        /// </summary>
        /// <param name="text"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static string MakeWidth(this string text, int width)
		{

			string format = "{0,-" + width + "}";

			string arg = text.Length > width ? text.Substring(0, width) : text;

			return string.Format(format, arg);

		}


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Repeat(this string input, int count)
        {
            if (!string.IsNullOrEmpty(input))
            {
                StringBuilder builder = new StringBuilder(input.Length * count);

                for (int i = 0; i < count; i++) builder.Append(input);

                return builder.ToString();
            }

            return string.Empty;
        }

        public static string RepeatedCharsRemove(this string input, int maxRepeat=0)
        {
            if (input.Length <= 1) return input;

            
            StringBuilder sb = new StringBuilder(input.Length);

            int     repeat = 0;

            Char[]  chars = input.ToCharArray();
            Char    charLast = chars[0];

            sb.Append( charLast );

            for (int cLoop = 1; cLoop < input.Length; cLoop++)
            {
                if (chars[cLoop] == charLast && repeat >= maxRepeat)
                {
                    //sb.Append( chars[cLoop] );
                    ++repeat;
                }
                else
                {
                    sb.Append( chars[cLoop] );
                    repeat = 0;
                    charLast = chars[cLoop];
                }
            }
            return sb.ToString().ToInternString();
        }

    }
}
