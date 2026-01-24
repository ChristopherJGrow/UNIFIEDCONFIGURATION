using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.Core.WPF
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObservableCollectionEx<T> : System.Collections.ObjectModel.ObservableCollection<T>
    {
        public new void Clear()
        {
            this.CheckReentrancy();
            this.Items.Clear();
            this.OnCollectionChanged( new System.Collections.Specialized.NotifyCollectionChangedEventArgs( System.Collections.Specialized.NotifyCollectionChangedAction.Reset ) );
        }

        /// <summary>
        /// Blindingly fast way to add data to an observable collection
        /// </summary>
        /// <param name="stuff"></param>
        public void Add(IEnumerable<T> stuff)
        {
            this.CheckReentrancy();
            foreach (var item in stuff)
                this.Items.Add( item );

            this.OnCollectionChanged( new System.Collections.Specialized.NotifyCollectionChangedEventArgs( System.Collections.Specialized.NotifyCollectionChangedAction.Reset ) );
        }
    }
}


