//--------------------------------------------------------------------
// © Copyright 1989-2026 Syndigo, LLC. - All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Syndigo, LLC.  Reproduction, disclosure or use without specific 
// written authorization from Syndigo, LLC. is prohibited.
// For more information see: http://www.syndigo.com
//--------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using System.Linq;
using System.Security.Principal;


namespace Config.Core.Extensions
{
    public static class IdentityReferenceCollectionExtension
    {
        public static IEnumerable<T> OfType<T>(this IdentityReferenceCollection collection)
        {
            foreach (object item in collection)
            {
                if (item is T)
                    yield return (T) item;
            }
        }

    }
}
