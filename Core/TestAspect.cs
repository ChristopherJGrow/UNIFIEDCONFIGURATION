using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml.Linq;

using Config.Core.Extensions;

namespace Config.Core
{
    public static class TestAspect
    {
        static public async Task Run(Func<Task> func)
        {
            try
            {
                await func();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToStringFull());
            }
        }

        static public async Task<T> Run<T>(Func<Task<T>> func)
        {
            T retval = default(T);
            try
            {
                retval = await func();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToStringFull());
            }
            return retval;
        }

        static public void Run(Action func)
        {
            try
            {
                func();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception");
            }
        }
    }
}
