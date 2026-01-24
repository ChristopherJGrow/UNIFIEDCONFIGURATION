//--------------------------------------------------------------------
// © Copyright 1989-2017 Edgenet, Inc. - All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Edgenet, Inc.  Reproduction, disclosure or use without specific 
// written authorization from Edgenet, Inc. is prohibited.
// For more information see: http://www.edgenet.com
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace Config.Core
{
    public class DelegateEx
    {

        //public static Delegate RemoveAll(Delegate myDelegate)
        //{
        //    return Delegate.RemoveAll(myDelegate, myDelegate);
        //}

        //public static EventHandler RemoveAllEvents(Delegate myDelegate)
        //{
        //    return Delegate.RemoveAll(myDelegate, myDelegate) as EventHandler;
        //}

        public static T RemoveAllEvents<T>(T myDelegate) where T : class
        {
            T retval = null;
            Delegate result;
            Delegate myDel = myDelegate as Delegate;

            if ( myDelegate != null )
            {
                if ( myDel != null )
                {
                    result = Delegate.RemoveAll( myDel, myDel );



                    retval = result as T;
                }
                else
                    throw new Exception( "Removing all Events" );// CqResIdAlt.IDS_ERR_REMOVEALLEVENTS );
            }

            return retval;
        }


        public static EventHandler NullEvent
        {
            get { return (EventHandler) null; }
        }

    }
}
