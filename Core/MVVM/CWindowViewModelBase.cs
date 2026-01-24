
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

using System.Windows;


using Config.Core;
using Config.Core.Extensions;


namespace Config.Core.MVVM //UnifiedConfiguration.PresentationModel
{

    /// <summary>
    /// Extends CqViewModelBaseWithOkCancel with additional app specific properties like WindowIcon
    /// </summary>
    public class CWindowViewModelBase : CViewModelBase, IDisposable
    {
        public CWindowViewModelBase()
        {

        }

        public override void Dispose()
        {
            base.Dispose();

            //this.WindowIcon = null;
            this.WindowTitle = string.Empty;

        }


        //private ImageSource _WindowIcon;
        //public virtual ImageSource WindowIcon
        //{
        //    get { return _WindowIcon; }
        //    set { this.OnPropertyChanged( _ => _WindowIcon = value ); }
        //}
        private byte[] _WindowIcon=Array.Empty<byte>();
        public virtual byte[] WindowIcon
        {
            get { return _WindowIcon; }
            set { this.OnPropertyChanged( () => this._WindowIcon = value ); }
        }



        private string _WindowTitle=string.Empty;
        public virtual string WindowTitle
        {
            get { return _WindowTitle; }
            set { this.OnPropertyChanged( () => this._WindowTitle = value ); }
        }


    }
}
