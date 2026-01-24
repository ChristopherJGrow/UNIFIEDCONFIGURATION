using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.Core
{
    /// <summary>
    /// Code to host some XML escape unescape code..
    /// </summary>
    public static class XmlHelp
    {
        ///
        /// -cjg-
        /// I realize there is probably a faster way to do this..
        /// however this is resonably performant considering the time used.
        /// 


        /// <summary>
        /// Encodes characters & \" ' < and > 
        /// </summary>
        /// <param name="pStr"></param>
        /// <returns></returns>
        public static string Escape(string pStr)
        {
            StringBuilder sb = new StringBuilder(pStr);

            sb.Replace( "&", "&amp;" );
            sb.Replace( "\"", "&quot;" );
            sb.Replace( "'", "&apos;" );
            sb.Replace( "<", "&lt;" );
            sb.Replace( "<", "&gt;" );


            return sb.ToString();
        }

        /// <summary>
        /// Decodes &amp; &quot; &apos; &lt; and &gt;
        /// </summary>
        /// <param name="pStr"></param>
        /// <returns></returns>
        public static string UnEscape(string pStr)
        {

            StringBuilder sb = new StringBuilder(pStr);

            sb.Replace( "&amp;", "&" );
            sb.Replace( "&quot;", "\"" );
            sb.Replace( "&apos;", "'" );
            sb.Replace( "&lt;", "<" );
            sb.Replace( "&gt;", "<" );

            return sb.ToString();
        }
    }
}
