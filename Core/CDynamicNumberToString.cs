//--------------------------------------------------------------------
// © Copyright 1989-2017 Edgenet, LLC. - All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Edgenet, LLC.  Reproduction, disclosure or use without specific 
// written authorization from Edgenet, LLC. is prohibited.
// For more information see: http://www.edgenet.com
//--------------------------------------------------------------------


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Config.Core.Conversion;


namespace Config.Core
{
    /// <summary>
    /// 
    /// Dynamically converts ulongs to string with calculate able base
    /// 
    /// This means you provide this class with a string 
    /// which defines the character set of the output
    /// 
    /// For base 10 you would use "0123456789"
    /// For hex you would use "0123456789ABCDEF"
    /// For binary you would use "01"
    /// 
    /// </summary>
    /// 
    public class CDynamicNumberToString //: ISelfTestable
    {

        private int m_MaximumChars = 0;
        private int m_Base = 0;
        private string m_Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private char[] m_CharsChars = null;
        private Hashtable m_CharsRev = null;
        private ulong[] m_BaseMultipliers = null;
        private ulong[] m_BaseMaxPerCol = null;

        /// <summary>
        /// If this class is used to generate non-obvious coding this magic number will help with making the order of a low order series 
        /// less obvious.. it is an obvuscation helper.. it is not necessary when doing obvious number formats like hex or binary
        /// where the transformation is well know.
        /// 
        /// for example a low order series or {0,1,2,3} might look like this.. {AAAA,AAAB,AAAC,AAAD}
        /// 
        /// The magic number can be used to shift leading zeros to another range where their meening may not be as significant.
        /// </summary>
        /// 
        private ulong m_MagicNumber = 0;


        public CDynamicNumberToString()
        {
            this.preCalculateAndIndex();
        }

        public CDynamicNumberToString(string baseChars)
        {
            this.m_Chars = baseChars;
            this.preCalculateAndIndex();
        }

        public CDynamicNumberToString(string baseChars, ulong magicNumber)
        {
            this.m_Chars = baseChars;
            this.m_MagicNumber = magicNumber;
            this.preCalculateAndIndex();
        }

        #region Properties

        /// <summary>
        /// The maximum number of characters we will Encode to based on ulong and this.m_base
        /// </summary>	
        public int MaximumChars
        {
            get { return this.m_MaximumChars; }

        }

        /// <summary>
        /// To set the base use the Chars property and base will be recalculated
        /// </summary>	
        public int Base
        {

            get { return this.m_Base; }
        }

        /// <summary>
        /// The valid characters used for the number base
        /// </summary>	
        public string Chars
        {
            get
            {
                return this.m_Chars;
            }
            set
            {
                this.m_Chars = value;
                this.preCalculateAndIndex();
            }

        }

        #endregion

        #region Misc

        public ulong MaxValueAtColumn(int col)
        {
            if (col == 0) col++;
            if (col > this.m_MaximumChars) col = this.m_MaximumChars;

            return this.m_BaseMaxPerCol[col - 1];
        }

        /// <summary>
        /// Calculates to power of each column for the base number scheme
        /// </summary>
        private void preCalculateAndIndex()
        {


            // Make it fast to find a character by index
            this.m_CharsChars = this.m_Chars.ToCharArray();

            // Make it fast to find the index of a character
            this.m_CharsRev = new Hashtable();
            int myCount;
            for (myCount = 0; myCount < this.m_CharsChars.Length; myCount++)
                this.m_CharsRev.Add(this.m_CharsChars[myCount], (ulong)myCount);


            // Determine the base value depending on the number of characters..
            // IE.. 
            // "0123456789ABCDEF"						
            // would be base 16 because there are 16 characters
            //
            // "0123456789"								
            // would be base 10
            //
            // "0123455689ABCDEFGHIJKLMNOPQRSTUVWXYZ"	
            // would be base 36
            this.m_Base = this.m_Chars.Length;

            //
            // Considering the base we are using 
            // what is the maximum number of characters that could be generated
            // during an Encode
            //
            double maxValue = 0;
            for (myCount = 1; maxValue < (double)ulong.MaxValue; myCount++)
            {
                //
                // accuracy isn't important here.. 
                // additionally we are examining the next value so it may exceed the maximum ulong
                // therefore Math.Pow will tell us when we've hit the maximum column count without
                // being so precise
                //
                maxValue = Math.Pow(this.m_Base, myCount) - 1;
            }

            this.m_MaximumChars = myCount - 1;

            this.m_BaseMultipliers = new ulong[this.m_MaximumChars];
            this.m_BaseMaxPerCol = new ulong[this.m_MaximumChars];

            //ulong uMax = ulong.MaxValue;
            //
            // Pre-calculate the multipliers for the numeric base
            //			
            for (myCount = 0; myCount < this.m_MaximumChars; myCount++)
            {
                ulong uValue = this.Pow((ulong)this.m_Base, (ulong)myCount); // here we need to be dead nutz on
                //if (uValue==0) 
                //	uValue=ulong.MaxValue;

                this.m_BaseMultipliers[myCount] = uValue;
            }

            // Calculate the maximum value at each column
            //
            for (myCount = 0; myCount < this.m_MaximumChars; myCount++)
            {
                // this may overflow on the last calculation
                // the code below the loop will correct for this
                this.m_BaseMaxPerCol[myCount] = (this.m_BaseMultipliers[myCount] * (ulong)this.m_Base) - 1;

            }

            //	I kinda question this.. but its a quick solution..  cjg
            //
            //	This will correct for an overflow on the last value
            //
            this.m_BaseMaxPerCol[this.m_MaximumChars - 1] = ulong.MaxValue;

        }

        //
        // We use this because it doesn't loose resolution when numbers get big!
        //
        public ulong Pow(ulong x, ulong y)
        {
            ulong uRetval = 1;
            
            for (ulong cLoop = 0; cLoop < y; cLoop++)
            {
                uRetval *= x;
            }

            return uRetval;
        }

        public ulong Pow(ulong y)
        {
            ulong x = (ulong)this.m_Base;
            ulong uRetval = 1;

            for (ulong cLoop = 0; cLoop < y; cLoop++)
            {
                uRetval *= x;
            }

            return uRetval;
        }


        #endregion

        #region TestCode

        public static bool TestSingleValue(CDynamicNumberToString myBase, ulong uValue)
        {
            bool bRetval = false;
            string tempEncode;
            ulong tempDecode;

            tempEncode = myBase.Encode(uValue, 5);
            tempDecode = myBase.Decode(tempEncode);

            if (tempDecode != uValue)
            {
                //CWIN32.Beep(2600,100);
                
                System.Diagnostics.Debugger.Break();
                bRetval = true;
            }

            //if (myCount%uSlice==0 || tempDecode!=myCount)
            

            return bRetval;
        }

        public static bool TestChars(string myChars, ulong magicNumber)
        {
            

            ulong myCount;
            ulong uSlice = 9;
            bool bRetval = false;
            DateTime dt, dtDone;
            TimeSpan ts;
            CDynamicNumberToString altBase = new CDynamicNumberToString(myChars, magicNumber);

            dt = DateTime.Now;



            // Test a range of values..
            for (myCount = (ulong)0xFFFF; myCount > uSlice; myCount -= uSlice)
                CDynamicNumberToString.TestSingleValue(altBase, myCount);

            // Test boundary conditions..
            CDynamicNumberToString.TestSingleValue(altBase, ulong.MinValue);
            CDynamicNumberToString.TestSingleValue(altBase, ulong.MinValue + 1);

            CDynamicNumberToString.TestSingleValue(altBase, (ulong)0xFF - 1);
            CDynamicNumberToString.TestSingleValue(altBase, (ulong)0xFF);
            CDynamicNumberToString.TestSingleValue(altBase, (ulong)0xFF + 1);

            CDynamicNumberToString.TestSingleValue(altBase, (ulong)0xFFFF - 1);
            CDynamicNumberToString.TestSingleValue(altBase, (ulong)0xFFFF);
            CDynamicNumberToString.TestSingleValue(altBase, (ulong)0xFFFF + 1);

            CDynamicNumberToString.TestSingleValue(altBase, (ulong)0xFFFFFFFF - 1);
            CDynamicNumberToString.TestSingleValue(altBase, (ulong)0xFFFFFFFF);
            CDynamicNumberToString.TestSingleValue(altBase, (ulong)0xFFFFFFFF + 1);

            CDynamicNumberToString.TestSingleValue(altBase, ulong.MaxValue - 1);
            CDynamicNumberToString.TestSingleValue(altBase, ulong.MaxValue);

            dtDone = DateTime.Now;
            ts = dtDone - dt;
            //CqDiag.debug(CI.Get(),"Total RunTime was {0} for base {1}", ts.ToString(), altBase.Base);
            return bRetval;
        }

        

        /// <summary>
        /// Self test code to make sure this class works correctly
        /// </summary>
        public bool Test()
        {
            lock(this.GetType())
            {


                CDynamicNumberToString.TestChars("ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789", 0);

                CDynamicNumberToString.TestChars("ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789", 1234);

                CDynamicNumberToString.TestChars("0123456789ABCDEF", 0);

                CDynamicNumberToString.TestChars("01", 0);


            }


            return true;

        }

        #endregion

        #region Encode


        virtual public string Encode(Enum eVal)
        {
            return this.Encode(eVal, 0);
        }

        virtual public string Encode(uint uintValue)
        {
            return this.Encode(uintValue, 0);
        }


        virtual public string Encode(int intValue)
        {
            return this.Encode(intValue, 0);
        }

        virtual public string Encode(ulong intValue)
        {
            return this.Encode(intValue, 0);
        }



        virtual public string Encode(Enum eVal, int minChars)
        {
            
            ulong ulValue = (ulong)System.Convert.ToUInt32(eVal);
            return this.Encode(ulValue, minChars);
        }

        virtual public string Encode(uint uintValue, int minChars)
        {
            return this.Encode((ulong)uintValue, minChars);
        }


        virtual public string Encode(int intValue, int minChars)
        {
            return this.Encode((ulong)intValue, minChars);
        }

        virtual public string Encode(ulong intValue, int minChars)
        {
            intValue += this.m_MagicNumber;

            string strRet = "";


            int intPlaces,
                    intPlaceOffset;
            ulong intRemain,
                    intMultiples;

            // Find Number of places
            intPlaces = 1;
            //while (!((Math.Pow(this.m_Base, intPlaces)) > intValue))
            while (!(this.m_BaseMaxPerCol[intPlaces - 1] >= intValue))
            {
                intPlaces += 1;
            }

            intRemain = intValue;
            for (; intPlaces != 0; --intPlaces)
            {
                intPlaceOffset = intPlaces - 1;

                ulong myMultiplier = this.m_BaseMultipliers[intPlaceOffset];

                //intMultiples = Convert.ToInt32(Math.Floor(Convert.ToDouble(Decimal.Divide(intRemain, Convert.ToDecimal(Math.Pow(this.m_Base, intPlaces))))));  

                intMultiples = intRemain / myMultiplier;

                //intRemain -= Convert.ToInt32(((Math.Pow(this.m_Base,intPlaces)) * intMultiples));
                intRemain -= myMultiplier * intMultiples;

                strRet += this.m_CharsChars[intMultiples];


            }

            while (strRet.Length < minChars)
            {
                strRet = m_CharsChars[0] + strRet;
            }

            return strRet;
        }

        #endregion

        #region Decode

        virtual public ulong Decode(string strValue)
        {
            ulong retval = 0;
            int intPlaces = 0;
            ulong myMultiplier = 0;
            ulong colValue = 0;

            if (strValue.Length > this.m_MaximumChars)
            {
                throw new Exception( "Too many characters in decode value" ); //new CqException(CqResIdAlt.ERR_CORE_TOO_MANY_CHARACTERS_IN_DECODED_VALUE);
            }

            // Make it easier to look at the incoming strings individual characters;
            char[] strValueChars = strValue.ToCharArray();

            //
            //	If the encode string size is less than eight 
            //	then we will use indexOf instead of the lookup hashtable
            //
            if (this.m_Chars.Length <= 8)
            {
                for (intPlaces = 0; intPlaces < strValue.Length; intPlaces++)
                {
                    myMultiplier = this.m_BaseMultipliers[strValue.Length - 1 - intPlaces];

                    colValue = (ulong)this.m_Chars.IndexOf(strValueChars[intPlaces]);

                    retval += colValue * myMultiplier;
                }


            }
            else
            {
                for (intPlaces = 0; intPlaces < strValue.Length; intPlaces++)
                {
                    myMultiplier = this.m_BaseMultipliers[strValue.Length - 1 - intPlaces];

                    char myChar = strValueChars[intPlaces];

                    if (this.m_CharsRev.Contains(myChar))
                    {
                        colValue = (ulong)this.m_CharsRev[myChar];
                        retval += colValue * myMultiplier;
                    }


                }
            }

            retval -= this.m_MagicNumber;

            return retval;
        }

        #endregion

    }



    /// <summary>
    /// 
    /// Converts ulongs to string with a base defined by the string of characters in the constructor
    /// Radix is implied by the characters provided to the constructor
    /// 
    /// This means you provide this class with a string 
    /// which defines the character set of the output
    /// 
    /// For base 10 you would use "0123456789"
    /// For hex you would use     "0123456789ABCDEF"
    /// For binary you would use  "01"
    /// 
    /// </summary>
    /// 
    public class NumberEncoder
    {
        private int _MaximumChars = 0;
        private int _Base = 0;
        private string _Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"; // default base 36
        private char[] _CharsChars = null;

        private System.Collections.Concurrent.ConcurrentDictionary<char,ulong> m_CharsRev = null;

        //private Dictionary<char, ulong> m_CharsRevAlt = null;

        private ulong[] m_BaseMultipliers = null;
        private ulong[] m_BaseMaxPerCol = null;

        /// <summary>
        /// If this class is used to generate non-obvious coding the number shift will help with making the order of a low order series 
        /// less obvious.. it is an obfuscation helper.. it is not necessary when doing obvious number formats like hex or binary
        /// where the transformation is well know.
        /// 
        /// for example a low order series or {0,1,2,3} might look like this.. {AAAA,AAAB,AAAC,AAAD}
        /// 
        /// The NumberShift can be used to shift leading zeros to another range where their meening may not be as significant.
        /// its simply adds number shif on encode and subtracts number shit on decode
        /// primarily a helper for crypto stuff
        /// </summary>
        /// 
        private ulong _NumberShift = 0;


        public NumberEncoder()
        {
            this.preCalculateAndIndex();
        }

        public NumberEncoder( string baseChars )
        {
            this._Chars = baseChars;
            this.preCalculateAndIndex();
        }

        public NumberEncoder( string baseChars, ulong shift )
        {
            this._Chars = baseChars;
            this._NumberShift = shift;
            this.preCalculateAndIndex();
        }

        #region Properties

        /// <summary>
        /// The maximum number of characters we will Encode to based on ulong and this.m_base
        /// </summary>	
        public int MaximumChars
        {
            get { return this._MaximumChars; }

        }

        /// <summary>
        /// To set the base use the Chars property and base will be recalculated
        /// </summary>	
        public int Base
        {

            get { return this._Base; }
        }

        /// <summary>
        /// The valid characters used for the number base
        /// </summary>	
        public string Chars
        {
            get
            {
                return this._Chars;
            }
            set
            {
                this._Chars = value;
                this.preCalculateAndIndex();
            }

        }

        #endregion

        #region Misc

        public ulong MaxValueAtColumn( int col )
        {
            if ( col == 0 )
                col++;
            if ( col > this._MaximumChars )
                col = this._MaximumChars;

            return this.m_BaseMaxPerCol[col - 1];
        }

        /// <summary>
        /// Calculates to power of each column for the base number scheme
        /// </summary>
        private void preCalculateAndIndex()
        {


            // Make it fast to find a character by index
            this._CharsChars = this._Chars.ToCharArray();

            // Make it fast to find the index of a character

            int myCount;

            this.m_CharsRev = new System.Collections.Concurrent.ConcurrentDictionary<char, ulong>();
            //this.m_CharsRevAlt = new Dictionary<char, ulong>();
            for ( myCount = 0; myCount < this._CharsChars.Length; myCount++ )
            {
                //this.m_CharsRevAlt.Add(this.m_CharsChars[myCount], (ulong) myCount);
                this.m_CharsRev.TryAdd(this._CharsChars[myCount], (ulong) myCount);
            }





            // Determine the base value depending on the number of characters..
            // IE.. 
            // "0123456789ABCDEF"						
            // would be base 16 because there are 16 characters
            //
            // "0123456789"								
            // would be base 10
            //
            // "0123455689ABCDEFGHIJKLMNOPQRSTUVWXYZ"	
            // would be base 36
            this._Base = this._Chars.Length;

            //
            // Considering the base we are using 
            // what is the maximum number of characters that could be generated
            // during an Encode
            //
            double maxValue = 0;
            for ( myCount = 1; maxValue < (double) ulong.MaxValue; myCount++ )
            {
                //
                // accuracy isn't important here.. 
                // additionally we are examining the next value so it may exceed the maximum ulong
                // therefore Math.Pow will tell us when we've hit the maximum column count without
                // being so precise
                //
                maxValue = Math.Pow(this._Base, myCount) - 1;
            }

            this._MaximumChars = myCount - 1;

            this.m_BaseMultipliers = new ulong[this._MaximumChars];
            this.m_BaseMaxPerCol = new ulong[this._MaximumChars];

            //
            // Pre-calculate the multipliers for the numeric base
            //			
            for ( myCount = 0; myCount < this._MaximumChars; myCount++ )
            {
                ulong uValue = Pow((ulong) this._Base, (ulong) myCount); // here we need to be dead nutz on

                this.m_BaseMultipliers[myCount] = uValue;
            }

            // Calculate the maximum value at each column
            //
            for ( myCount = 0; myCount < this._MaximumChars; myCount++ )
            {
                // this may overflow on the last calculation
                // the code below the loop will correct for this
                this.m_BaseMaxPerCol[myCount] = ( this.m_BaseMultipliers[myCount] * (ulong) this._Base ) - 1;

            }

            //	I kinda question this.. but its a quick solution..  cjg
            //
            //	This will correct for an overflow on the last value
            //
            this.m_BaseMaxPerCol[this._MaximumChars - 1] = ulong.MaxValue;

        }

        //
        // We use this because it doesn't loose resolution when numbers get big!
        //
        private static ulong Pow( ulong x, ulong y )
        {
            ulong uRetval = 1;

            ulong cLoop;
            for ( cLoop = 0; cLoop < y; cLoop++ )
            {
                uRetval *= x;
            }

            return uRetval;
        }

        //public ulong Pow( ulong y )
        //{
        //    ulong x = (ulong) this.m_Base;
        //    ulong uRetval = 1;

        //    ulong cLoop;
        //    for ( cLoop = 0; cLoop < y; cLoop++ )
        //    {
        //        uRetval *= x;
        //    }

        //    return uRetval;
        //}


        #endregion



        #region Encode


        virtual public string Encode( Enum eVal )
        {
            return this.Encode(eVal, 0);
        }

        virtual public string Encode( uint uintValue )
        {
            return this.Encode(uintValue, 0);
        }


        virtual public string Encode( int intValue )
        {
            return this.Encode(intValue, 0);
        }

        virtual public string Encode( ulong intValue )
        {
            return this.Encode(intValue, 0);
        }



        virtual public string Encode( Enum eVal, int minChars )
        {
            ulong ulValue = (ulong) System.Convert.ToUInt32(eVal);
            return this.Encode(ulValue, minChars);
        }

        virtual public string Encode( uint uintValue, int minChars )
        {
            return this.Encode((ulong) uintValue, minChars);
        }


        virtual public string Encode( int intValue, int minChars )
        {
            return this.Encode((ulong) intValue, minChars);
        }

        virtual public string Encode( ulong intValue, int minChars )
        {
            intValue += this._NumberShift;

            string strRet = "";


            int intPlaces,
                    intPlaceOffset;
            ulong intRemain,
                    intMultiples;

            ulong myMultiplier;

            // Find Number of places
            intPlaces = 1;

            while ( !( this.m_BaseMaxPerCol[intPlaces - 1] >= intValue ) )
            {
                intPlaces += 1;
            }

            intRemain = intValue;
            for ( ; intPlaces != 0; --intPlaces )
            {
                intPlaceOffset = intPlaces - 1;

                myMultiplier = this.m_BaseMultipliers[intPlaceOffset];

                intMultiples = intRemain / myMultiplier;

                intRemain -= myMultiplier * intMultiples;

                strRet += this._CharsChars[intMultiples];


            }

            while ( strRet.Length < minChars )
            {
                strRet = _CharsChars[0] + strRet;
            }

            return strRet;
        }

        #endregion

        #region Decode


        virtual public ulong Decode( string strValue )
        {
            ulong retval = 0;
            int intPlaces = 0;
            ulong myMultiplier = 0;
            ulong colValue = 0;
            char myChar;

            if ( strValue.Length > this._MaximumChars )
            {
                throw new Exception("Too many characters in decoded value");
            }

            // Make it easier to look at the incoming strings individual characters;
            char[] strValueChars = strValue.ToCharArray();

            //
            //	If the encode string size is less than eight 
            //	then we will use indexOf instead of the lookup hashtable
            //
            if ( this._Chars.Length <= 8 )
            {
                for ( intPlaces = 0; intPlaces < strValue.Length; intPlaces++ )
                {
                    myMultiplier = this.m_BaseMultipliers[strValue.Length - 1 - intPlaces];

                    colValue = (ulong) this._Chars.IndexOf(strValueChars[intPlaces]);

                    retval += colValue * myMultiplier;
                }


            }
            else
            {
                for ( intPlaces = 0; intPlaces < strValue.Length; intPlaces++ )
                {
                    myMultiplier = this.m_BaseMultipliers[strValue.Length - 1 - intPlaces];

                    myChar = strValueChars[intPlaces];

                    if ( this.m_CharsRev.TryGetValue(myChar, out colValue) )
                    {
                        //colValue = this.m_CharsRev[myChar]; // the performance here is so minor that it's just not worth it.
                        retval += colValue * myMultiplier;
                    }
                }

            }

            retval -= this._NumberShift;

            return retval;
        }

        #endregion

    }

    

}
