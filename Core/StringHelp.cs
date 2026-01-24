//--------------------------------------------------------------------
// © Copyright 1989-2022 Syndigo, LLC. - All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Syndigo, LLC.  Reproduction, disclosure or use without specific 
// written authorization from Syndigo, LLC. is prohibited.
// For more information see: http://www.syndigo.com
//--------------------------------------------------------------------


using System;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

//using Sage.Shared.COR.Domain;
//using Sage.Shared.COR.Main.Log;


namespace Config.Core
{
    /*
    Regular Expressions in C#
    By Prasad H.

    The following example shows the usage of Regular Expressions in C#. The program basically has all the Validation Programs using Regular Expression. 
    */
    public class StringHelp
    {
        static char g_defaultSeparator = ',';
        

        


        #region Section Support Code

        static public bool hasSection(string str, int iSectionNumber)
        {
            return StringHelp.Section(str, iSectionNumber).Length > 0;
        }

        static public bool hasSection(string str, int iSectionNumber, char separator)
        {
            return StringHelp.Section(str, iSectionNumber, separator).Length > 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public int SectionCount(string str)
        {
            return StringHelp.SectionCount(str, StringHelp.g_defaultSeparator);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public int SectionCount(string str, char separator)
        {
            var sepArray = new char[] { separator };
            int iFoundAt = 0;
            int cLoop = 1;

            while ((iFoundAt = str.IndexOfAny(sepArray, iFoundAt)) != -1)
            {
                iFoundAt++; // advance past the separator
                cLoop++;
            }

            return cLoop;


            //int iFoundAt = 0;
            //int cLoop = 1;

            //while( ( iFoundAt = str.IndexOfAny(new char[] { separator }, iFoundAt) ) != -1 )
            //{
            //    iFoundAt++; // advance past the separator
            //    cLoop++;
            //}

            //return cLoop;

            //int retval = 0;
            ////int cLoop = 0;
            ////for( cLoop = 0; cLoop < str.Length; cLoop++ )
            ////{
            ////    if( str[cLoop] == separator )
            ////        retval++;
            ////}

            //foreach( var myChar in str )
            //{
            //    if( myChar == separator )
            //        retval++;
            //}

            //return retval;
        }

        /// <summary>
        /// Find a csv section that starts with name= and return its value.  If not present then return null.
        /// </summary>
        /// <param name="str">the csv to search</param>
        /// <param name="name">the name to search for</param>
        /// <returns></returns>
        static public string findNamedSection(string str, string name)
        {
            return findNamedSection(str, name, ',', '=');
        }

        /// <summary>
        /// Basic function to return a named section, where you can pass in the separators to use.  Normally these are , and =
        /// </summary>
        /// <param name="str">the string to search</param>
        /// <param name="name">the named section to search for</param>
        /// <param name="sectionSeparator">Normally a comma</param>
        /// <param name="nameValueSeparator">Normally an equals</param>
        /// <returns></returns>
        static public string findNamedSection(string str, string name, char sectionSeparator, char nameValueSeparator)
        {
            int count = StringHelp.SectionCount(str);
            for (int i = 0; i < count; ++i)
            {
                string nameValuePair = StringHelp.Section(str, i, sectionSeparator);
                string n = StringHelp.Section(nameValuePair, 0, nameValueSeparator);
                if (name == n)
                {
                    // use sectionremainder so the value can have an equals sign in it
                    return StringHelp.SectionRemainder(nameValuePair, 1, nameValueSeparator);
                }
            }

            return null;
        }

        static public int[] IndexOfChars(string str, char chToLocate)
        {
            System.Collections.Generic.List<int> retval = new System.Collections.Generic.List<int>();

            int cLoop = 0;
            int cEnd = str.Length;
            for (cLoop = 0; cLoop < cEnd; cLoop++)
            {
                if (str[cLoop] == chToLocate)
                    retval.Add(cLoop);
            }
            return retval.ToArray();
        }

        static public int[] IndexesOfAll(string str, char[] separators)
        {
            List<int> indexes = new List<int>();

            for(int cLoop=0;cLoop<str.Length;++cLoop)
            {
                foreach (var item in separators)
                {
                    if (item == str[cLoop])
                    {
                        indexes.Add(cLoop);
                    }
                }
                   
            }
            return indexes.ToArray();

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public string Section(string str, int iSectionNumber)
        {
            return StringHelp.Section(str, iSectionNumber, StringHelp.g_defaultSeparator);
        }
        /// <summary>
        /// takes a string separated by some character and return a descrete section of it.
        /// Example: string temp = StringHelp.Section("hello,there,dude",2,',');
        /// should return a string containing "there"
        /// </summary>
        /// <param name="str">Input string you want the section for</param>
        /// <param name="iSectionNumber">which section of the string do you want?</param>
        /// <param name="separator">separator character</param>
        /// <returns>section of the string you requested</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public string Section(string str, int iSectionNumber, char separator)
        {
            string retval = "";
            int cLoop = iSectionNumber;
            int iFoundEnd = 0;
            int iFoundAt = 0;

            if (str == null)
                return ("");

            // Allows you get a section from the back side of a string
            // so with 5 sections -2 gets you the 3rd section
            if (cLoop < 0)
            {
                cLoop = StringHelp.SectionCount(str, separator) + cLoop;
            }

            while (cLoop > 0)
            {

                iFoundAt = str.IndexOfAny(new char[] { separator }, iFoundAt);
                if (iFoundAt != -1)
                {
                    iFoundAt++; // advance past the separator
                    cLoop--;
                }
                else
                    break;
            }

            if (iFoundAt != -1)
            {
                iFoundEnd = str.IndexOfAny(new char[] { separator }, iFoundAt);
                if (iFoundEnd == -1)
                    retval = str.Substring(iFoundAt);
                else
                    retval = str.Substring(iFoundAt, iFoundEnd - iFoundAt);
            }
            else
                retval = "";


            return retval.Trim();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public string SectionRemainder(string str, int iSectionNumber)
        {
            return StringHelp.SectionRemainder(str, iSectionNumber, StringHelp.g_defaultSeparator);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public string SectionRemainder(string str, int iSectionNumber, char separator)
        {
            string retval = "";
            int cLoop = iSectionNumber;
            //int iFoundEnd=0;
            int iFoundAt = 0;

            while (cLoop > 0)
            {

                iFoundAt = str.IndexOfAny(new char[] { separator }, iFoundAt);
                if (iFoundAt != -1)
                {
                    iFoundAt++; // advance past the separator
                    cLoop--;
                }
                else
                    break;
            }

            if (iFoundAt != -1)
            {
                retval = str.Substring(iFoundAt);
            }
            else
                retval = "";


            return retval;
        }


        /// <summary>
        /// Trims from the right side of the string by iSectionNumber sections
        /// </summary>
        /// <param name="str"></param>
        /// <param name="iSectionNumber"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        static public string SectionTrimRight(string str, int cSectionsToTrim, char separator)
        {
            int[] slashpos = StringHelp.IndexOfChars(str, '/');

            int index = slashpos.Length - cSectionsToTrim;

            string retval;

            if (index < 0 || index >= slashpos.Length)
                retval = null;
            else
                retval = str.Substring(0, slashpos[index]);

            return retval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="cSectionNumber"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        static public string SectionTrimLeft(string str, int cSectionsToTrim, char separator)
        {
            int[] slashpos = StringHelp.IndexOfChars(str, '/');

            int index = cSectionsToTrim - 1;

            string retval;
            if (index < 0 || index >= slashpos.Length)
                retval = null;
            else
                retval = str.Substring(slashpos[index] + 1);

            return retval;
        }


        #endregion

        /// <summary>
        /// Grabs the right N characters from a string
        /// </summary>
        /// <param name="str">input string</param>
        /// <param name="number">number of characters to take from right</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public string Right(string str, int number)
        {
            int pos = str.Length - number;

            string retval;

            if (pos >= 0)
                retval = str.Substring(pos);
            else
                retval = "";

            return retval;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static public string Left(string str, int length)
        {
            string retval;

            if (length > 0 && length < str.Length)
                retval = str.Substring(0, length);
            else if (length > 0)
                retval = str;
            else
                retval = "";

            return retval;

        }





        /// <summary>
        /// Compares str2 to the right N characters of string 1.  
        /// </summary>
        /// <param name="str1">input string to compare the right side of</param>
        /// <param name="str2">input string that will be compared</param>
        /// <returns></returns>
        static public int RightCompare(string str1, string str2)
        {
            return string.Compare(StringHelp.Right(str1, str2.Length), str2);
        }

        static public int RightCompare(string str1, string str2, StringComparison sc)
        {
            return string.Compare(StringHelp.Right(str1, str2.Length), str2, sc);
        }




        static public bool IsNaturalNumber(String strNumber)
        {
            //Regex objNotNaturalPattern = new Regex("[^0-9]");
            //Regex objNaturalPattern = new Regex("0*[1-9][0-9]*");

            //return !objNotNaturalPattern.IsMatch(strNumber) &&
            //    objNaturalPattern.IsMatch(strNumber);


            int iTemp = 0;
            bool bRetval = int.TryParse(strNumber, out iTemp);

            return bRetval;

        }

        // Function to test for Positive Integers with zero inclusive

        static public bool IsWholeNumber(string strNumber)
        {
            Regex objNotWholePattern = new Regex("[^0-9]");

            return !objNotWholePattern.IsMatch(strNumber);
        }

        // Function to Test for Integers both Positive & Negative

        static public bool IsInteger(string strNumber)
        {
            Regex objNotIntPattern = new Regex("[^0-9-]");
            Regex objIntPattern = new Regex("^-[0-9]+$|^[0-9]+$");

            return !objNotIntPattern.IsMatch(strNumber) &&
                objIntPattern.IsMatch(strNumber);
        }

        // Function to Test for Positive Number both Integer & Real

        static public bool IsPositiveNumber(string strNumber)
        {
            Regex objNotPositivePattern = new Regex("[^0-9.]");
            Regex objPositivePattern = new Regex("^[.][0-9]+$|[0-9]*[.]*[0-9]+$");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");

            return !objNotPositivePattern.IsMatch(strNumber) &&
                objPositivePattern.IsMatch(strNumber) &&
                !objTwoDotPattern.IsMatch(strNumber);
        }

        // Function to test whether the string is valid number or not
        static public bool IsNumber(string strNumber)
        {
            if (strNumber.Trim().Equals("-"))
                return false;

            Regex objNotNumberPattern = new Regex("[^0-9.-]");
            Regex objTwoDotPattern = new Regex("[0-9]*[.][0-9]*[.][0-9]*");
            Regex objTwoMinusPattern = new Regex("[0-9]*[-][0-9]*[-][0-9]*");
            String strValidRealPattern = "^([-]|[.]|[-.]|[0-9])[0-9]*[.]*[0-9]+$";
            String strValidIntegerPattern = "^([-]|[0-9])[0-9]*$";
            Regex objNumberPattern = new Regex("(" + strValidRealPattern + ")|(" + strValidIntegerPattern + ")");

            return !objNotNumberPattern.IsMatch(strNumber) &&
                !objTwoDotPattern.IsMatch(strNumber) &&
                !objTwoMinusPattern.IsMatch(strNumber) &&
                objNumberPattern.IsMatch(strNumber);
        }

        // Function To test for Alphabets.

        static public bool IsAlpha(string strToCheck)
        {
            Regex objAlphaPattern = new Regex("[^a-zA-Z]");

            return !objAlphaPattern.IsMatch(strToCheck);
        }

        // Function to Check for AlphaNumeric.

        static public bool IsAlphaNumeric(string strToCheck)
        {
            Regex objAlphaNumericPattern = new Regex("[^a-zA-Z0-9]");

            return !objAlphaNumericPattern.IsMatch(strToCheck);
        }


        static public string ReplaceIgnoreCase(string original, string pattern, string replacement)
        {
            int count,
                position0,
                position1;

            count = position0 = position1 = 0;

            string upperString = original.ToUpper();
            string upperPattern = pattern.ToUpper();

            int inc = (original.Length / pattern.Length) *
                      (replacement.Length - pattern.Length);

            char[] chars = new char[original.Length + Math.Max(0, inc)];

            while ((position1 = upperString.IndexOf(upperPattern,
                                              position0)) != -1)
            {
                for (int i = position0; i < position1; ++i)
                    chars[count++] = original[i];
                for (int i = 0; i < replacement.Length; ++i)
                    chars[count++] = replacement[i];
                position0 = position1 + pattern.Length;
            }

            if (position0 == 0) return original;

            for (int i = position0; i < original.Length; ++i)
                chars[count++] = original[i];

            return new string(chars, 0, count);
        }

        /// <summary>
        /// This is a replacement method for string.Copy that doesn't actually copy.  Please remove this if testable.
        /// </summary>
        /// <param name="origional"></param>
        /// <returns></returns>
        static public string CopyPlacebo(string origional)
        {
            return origional;

            //return string.Copy(origional);
        }


    }

}
