using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.Core
{
    /// <summary>
    /// Class to support execution of Actions at the beggining and end of a using block
    /// This is the simples support class for a using block there is
    /// </summary>
    public class Using : IDisposable
    {
        protected Action _start;
        protected Action _end;


        protected Using()
        {
        }

        /// <summary>
        /// Creates a using object with action for beginning of block and end of block
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Using(Action start, Action end)
        {
            this._start = start;
            this._end = end;
            this._start();

        }

        /// <summary>
        /// Creates a using object with an action for the end of the block only.. 
        /// no code executed at begining of block!
        /// </summary>
        /// <param name="end"></param>
        public Using(Action end)
        {
            this._end = end;

        }


        public void Dispose()
        {
            if ( this._end != null )
                this._end();

        }

    }
}
