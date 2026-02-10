using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;


using Config.Core;
using Config.Core.Extensions;
//using Config.Core;
//using Config.Core.Extensions;

namespace Config.Core.MVVM//UnifiedConfiguration.PresentationModel
{
    public enum ViewModelResult
    {
        OK,
        Cancel
    }

    public class CViewModelBase : IViewModelBase
    {

        public virtual void Dispose()
        {
            this.Clear();
        }
        public virtual IViewModelBase Owner { get; set; }
        
        /// <summary>        
        /// Pop tasty Ubik into your toaster, made only from fresh fruit and healthful all-vegetable shortening. 
        /// Ubik makes breakfast a feast, puts zing into your thing! Safe when handled as directed.
        /// </summary>
        public IUserBaseInterfaceContext Ubic { get; set; }


        public virtual void Update(object obj)
        {

        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged( this, e );

        }

        /// <summary>
        /// Faster call to OnPropertyChanged method
        /// </summary>
        /// <param name="myValue"></param>
        //protected virtual void OnPropertyChanged(string myValue)
        //{
        //    this.OnPropertyChanged( new PropertyChangedEventArgs( myValue ) );
        //}

        /// <summary>
        /// Faster call to OnPropertyChanged method
        /// </summary>
        /// <param name="myValue"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string myValue = null)
        {
            this.OnPropertyChanged( new PropertyChangedEventArgs( myValue ) );
        }


        /// <summary>
        /// Automatic propertyName from CallerMemberName
        /// This allows you to change the property in the fChange while automatic propertyName from CallerMemberName
        /// Example:
        /// 
        /// set => this.PropertyChange( () => this.Text = value );
        /// 
        /// </summary>
        /// <param name="fChange"></param>
        /// <param name="myValue"></param>
        protected virtual void OnPropertyChanged(Action fChange, [CallerMemberName] string propertyName = null)
        {            
            fChange?.Invoke();
            this.OnPropertyChanged( new PropertyChangedEventArgs( propertyName ) );
        }

        /// <summary>
        /// automatic propertyName from CallerMemberName
        /// This allows you to change the property in the fChange only if fIsDifferent is true
        /// Example:
        /// 
        /// set => this.PropertyChange( ()=> this._Text != value,  () => this._Text = value );
        /// 
        /// </summary>
        /// <param name="fIsDifferent"></param>
        /// <param name="fChange"></param>
        /// <param name="myValue"></param>
        protected virtual bool OnPropertyChanged(Func<bool> fIsDifferent, Action fChange, [CallerMemberName] string propertyName = null)
        {
            bool retval = false;
            if (fIsDifferent!=null && fIsDifferent())
            {
                retval = true;
                fChange?.Invoke();
                this.OnPropertyChanged( new PropertyChangedEventArgs( propertyName ) );
            }
            else
            {
                // Optionally log or handle the case where the property change was skipped
                //CqDiag.always( $"Property {myValue} Change Skipped" );
            }
            return retval;
        }

        /// <summary>
        /// Clears the object and removes events hooked to our object
        /// </summary>
        public virtual void Clear()
        {
            this.PropertyChanged.RemoveAllEvents();
        }

    }
}
