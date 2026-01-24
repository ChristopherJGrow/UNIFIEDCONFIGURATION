//--------------------------------------------------------------------
// © Copyright 1989-2025 Syndigo, LLC. - All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Syndigo, LLC.  Reproduction, disclosure or use without specific 
// written authorization from Syndigo, LLC. is prohibited.
// For more information see: http://www.syndigo.com
//--------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Config.Core.Conversion;

namespace Config.Core.Extensions
{
    static class IConfigurationProviderExtension
    {   
        /// <summary>
        /// Gets a value from the provider and converts it to the type specified by the default value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T Get<T>(this IConfigurationProvider self, string key, T defaultValue)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            self.TryGet(key, out var stringValue);            
            {
                return ConvertEx.From(stringValue, defaultValue);
            }
            return defaultValue;
        }
    }
}
