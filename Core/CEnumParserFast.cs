using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.CompilerServices;

using Config.Core.Extensions;

namespace Config.Core
{
    /// <summary>
    /// Fast Enum Parser that compiles a lambda that uses a switch to evaluate the string
    /// 80 percent faster than TryParse
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class CEnumParserFast<T>
    {
        static Func<string, object> CompiledParser;

        static CEnumParserFast()
        {            
            CompiledParser = GetParseEnumDelegate(typeof(T) );
        }

        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        public static T Parse(string val) 
        {            
            // Enums cant start with digits.
            // So if the first char is a digit its not a enum
            // and this is the only other option
            if (Char.IsDigit(val[0]))
                return (T) (object) val.ToInt(); // make this part of the compiled code
            else
                return (T) CompiledParser( val );            
        }

       


        static public Func<String, object> GetParseEnumDelegate(Type tEnum)
        {
            var eValue = Expression.Parameter(typeof(string), "value"); // (String value)
            var tReturn = typeof(Object);

            return
              Expression.Lambda<Func<String, Object>>(
                Expression.Block( tReturn,
                  Expression.Convert( // We need to box the result (tEnum -> Object)
                    Expression.Switch( tEnum, eValue,
                      Expression.Block( tEnum,
                        Expression.Throw( Expression.New( typeof( Exception ).GetConstructor( Type.EmptyTypes ) ) ),
                        Expression.Default( tEnum )
                      ),
                      null,
                      Enum.GetValues( tEnum ).Cast<Object>().Select( v => Expression.SwitchCase(
                           Expression.Constant( v ),
                           Expression.Constant( v.ToString() )
                         ) ).ToArray()
                    ), tReturn
                  )
                ), eValue
              ).Compile();
        }
    }
}
