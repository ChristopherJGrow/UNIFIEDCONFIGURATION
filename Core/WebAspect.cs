


using System.Collections.Generic;

using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Diagnostics;


using Config.Core.Extensions;



namespace Config.Core
{

    /// <summary>
    /// Defines a context to run controllers in
    /// </summary>
    public static class WebAspect
    {
        static public List<Task> tasks = new List<Task>();
        static object tasksLock = new object();

        static public void AddTask(Task task)
        {
            lock(tasksLock)
            {
                tasks.Add(task);
            }
        }

        

        static public async Task Run(Func<Task> func)
        {
            //var logDef = Factory.Resolve<ILoggerDeferred>();
            try
            {
                await func();
            }
            catch (Exception e)
            {
                //CqDiag.error(e);
                //logDef.Push();
                await WaitForStuff();
            }
            finally
            {
                //logDef.Push();

                await WaitForStuff();
                
            }
        }

        



        static public  Task WaitForStuff()
        {
            return Task.CompletedTask;//CallBackFunc();
        }

        static public Task<T> Run<T>(Func<Task<T>> func)
        {
            //var logDef = Factory.Resolve<ILoggerDeferred>();
            Task<T> retval = default(Task<T>);
            //T retval = default(T);
            try
            {
                retval = func();


            }
            catch (Exception e)
            {
                //CqDiag.error( e );
                //logDef.Push();
                //await WaitForStuff();
                //Task.CompletedTask.Wait();
            }
            finally
            {

                //logDef.Push();
                //await WaitForStuff();
            }
            return retval;
        }
        //static public Task<T> RunAsync<T>( Func<Task<T>> func)
        //{
        //    var logDef = Factory.Resolve<ILoggerDeferred>();
        //    Task<T> retval = default(Task<T>);
        //    try
        //    {
        //        retval = func();
        //    }
        //    catch (Exception e)
        //    {
        //        CqDiag.error(CI.Get(), e);
        //        logDef.Push();
        //        //await WaitForStuff();
        //    }
        //    finally
        //    {

        //        logDef.Push();
        //        //await WaitForStuff();
        //    }
        //    return retval;
        //}

        static public void Run(Action func)
        {
            //var logDef = Factory.Resolve<ILoggerDeferred>();
            try
            {
                func();
            }
            catch (Exception e)
            {                
                //CqDiag.error( e);

                //logDef.Push();
                Task.Run(() =>
                {
                    return WaitForStuff();

                }).Wait();


            }
            finally
            {
                //logDef.Push();
                Task.Run(() =>
                {
                   return WaitForStuff();

                }).Wait();
            }        
        }

        static public T Run<T>(Func<T> func)
        {
            T retval = default(T);
            //var logDef = Factory.Resolve<ILoggerDeferred>();
            try
            {
                retval = func();
            }
            catch (Exception e)
            {
              

                //logDef.Push();
                Task.Run(() =>
                {
                    return WaitForStuff();

                }).Wait();

            }
            finally
            {
                //logDef.Push();
                Task.Run(() =>
                {
                    return WaitForStuff();

                }).Wait();
            }
            return retval;
        }

        static public T Run<T>(T def, Func<T> func )
        {
            T retval = def;
            //var logDef = Factory.Resolve<ILoggerDeferred>();
            try
            {
                retval = func();
            }
            catch (Exception e)
            {
               

                //logDef.Push();
                Task.Run( () =>
                {
                    return WaitForStuff();

                } ).Wait();

            }
            finally
            {
                //logDef.Push();
                Task.Run( () =>
                {
                    return WaitForStuff();

                } ).Wait();
            }
            return retval;
        }
    }
}