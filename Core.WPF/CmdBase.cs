using Config.Core;
using Config.Core.MVVM;
using Config.Core.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.Core.WPF
{
    public abstract class CmdBase : CViewModelBase, ICommandViewModel
    {

        public virtual event EventHandler CanExecuteChanged;
        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            this.CanExecuteChanged?.Invoke( this, e );
        }

        string _Text=string.Empty;
        public virtual string Text
        {
            get => this._Text; 
            set => this.OnPropertyChanged( ()=> this._Text = value );                
        }

        string _GestureText=string.Empty;
        public virtual string GestureText
        {
            get { return this._GestureText; }
            set
            {
                this._GestureText = value;
                this.OnPropertyChanged();
            }
        }

        string _Key=string.Empty;
        public virtual string Key
        {
            get { return this._Key; }
            set
            {
                this._Key = value;
                this.OnPropertyChanged( nameof( this.Key ) );
            }
        }
        string _KeyModifier=string.Empty;
        public virtual string KeyModifier
        {
            get { return this._KeyModifier; }
            set
            {
                this._KeyModifier = value;
                this.OnPropertyChanged();
            }
        }


        // As CanExecute is bascially broke this shouldn't be overloaded either
        // Use IsEnabled 
        public virtual bool CanExecute(object obj)
        {
            return true;
        }

        

        // We dont want this being overloaded so it is not virtual on purpose.
        // Overload ExecuteSafe
        public virtual void Execute(object obj)
        {
            try
            {
                // This is all because CanExecute is flat broke but not a big deal
                //
                if (this.IsEnabled)
                {
                    var prevEnabled = this.IsEnabled;
                    using (_ = new Using( () => this.IsEnabled = false, () => this.IsEnabled = prevEnabled ))
                    {
                        this.ExecuteSafe( obj );
                    }
                }

            }
            catch (Exception ex)
            {
                this.LastError = ex;

                Console.WriteLine( $"CmdBase.Execute Exception: {ex.ToStringFull()} " );
                //if (obj is IErrorPresenter)
                //{
                //    var pres = obj as IErrorPresenter;
                //    pres.ErrorShow( ex.Message, ex );
                //}
            }
        }

        public Exception LastError { get; protected set; } = null;

        /// <summary>
        /// Executes code that is inside a Try Catch with error reporting
        /// </summary>
        /// <param name="obj"></param>
        public abstract void ExecuteSafe(object obj);

        bool _IsEnabled = true;
        public virtual bool IsEnabled
        {
            get { return this._IsEnabled; }
            set
            {
                if (this._IsEnabled != value)
                {
                    this._IsEnabled = value;
                    this.OnPropertyChanged( nameof( this.IsEnabled ) );
                }
            }
        }

        bool _IsVisible =true;
        public virtual bool IsVisible
        {
            get { return this._IsVisible; }
            set
            {
                if (this._IsVisible != value)
                {
                    this._IsVisible = value;
                    this.OnPropertyChanged();
                }
            }
        }

    }
}
