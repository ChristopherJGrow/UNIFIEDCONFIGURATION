using Config.Core.Extensions;
using Config.Core.MVVM;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedConfiguration.PresentationModel
{
    public class GridItemViewModel : CViewModelBase
    {
        public GridItemViewModel() : base()
        {
            //string sid = WindowsIdentity.GetCurrent().User?.Value!;

            //// Yes its a global.. i'm so darn sorry..
            //// I'm sure there is some stupid binding that could have gotte me out of this mess.
            ////
            //if (UnifiedConfigurationViewModel.__NewItemTypeSelection==NewItemType.Override)
            //    this._UserId = sid;
            //else
            //    this._UserId = string.Empty;
        }

        public virtual new UnifiedConfigurationViewModel Owner
        {
            get => (UnifiedConfigurationViewModel) base.Owner;
            set => base.Owner = value;  
        }

        
        public GridItemViewModel Self => this;

        string _Module = string.Empty;
        public string Module
        {
            get => _Module;
            set => this.OnPropertyChanged( () => { this._Module = value; this.IsDirty = true; } );
        }

        string _Section = string.Empty;
        public string Section
        {
            get => _Section;
            set => this.OnPropertyChanged( () => { this._Section = value; this.IsDirty = true; } );
        }
        string _Variable = string.Empty;
        public string Variable
        {
            get => _Variable;
            set => this.OnPropertyChanged( () => { this._Variable = value; this.IsDirty = true; } );
        }
        string _Value = string.Empty;
        public string Value
        {
            get => _Value;
            set => this.OnPropertyChanged( () => { this._Value = value; this.IsDirty = true; } );
        }
        string _UserId = string.Empty;
        public string UserId
        {
            get => _UserId;
            set => this.OnPropertyChanged( () => { this._UserId = value; this.IsDirty = true; } );
        }

        string _UserPretty = string.Empty;
        public string UserPretty
        {
            get => this._UserPretty;
            set => this.OnPropertyChanged( () => this._UserPretty = value );
        }

        public bool IsDefault
        {
            get => this.UserId.IsNullOrEmpty();
        }   

        string _EffectiveBuildNumber = string.Empty;
        public string EffectiveBuildNumber
        {
            get => _EffectiveBuildNumber;
            set => this.OnPropertyChanged( () => { this._EffectiveBuildNumber = value; this.IsDirty = true; } );
        }

        string _BuildNumber = string.Empty;
        public string BuildNumber
        {
            get => _BuildNumber;
            set => this.OnPropertyChanged( () => { this._BuildNumber = value; this.IsDirty = true; } );
        }

        bool _IsDeleted  = false;
        public bool IsDeleted
        {
            get => this._IsDeleted;
            set 
            {
                this.OnPropertyChanged( () => this._IsDeleted = value );
                this.IsDirty = true;
            }
        }

        public bool IsDeletable
        {
            get => this.IsUserOverride || this.Owner.IsAppAdmin;
        }

        public bool IsUserOverride
        {
            get => this.UserId.IsNotNullOrEmpty();
        }

        bool _IsDirty = true;
        public bool IsDirty
        {
            get => _IsDirty;
            set
            {
                this.OnPropertyChanged( () => _IsDirty = value );
                this.OnPropertyChanged( nameof( this.IsDirtyString ) );
            }
        }       
        public string IsDirtyString
        {
            get 
            {
                string retval = string.Empty;
                if (this._IsDeleted)
                    retval = "\u2620";         // a skull
                else if (this._IsDirty)
                    retval = "\u2665";            //"♥"

                return retval;
            }
        }

        bool _IsReadOnly = false;
        public bool IsReadOnly
        {
            get => _IsReadOnly;
            set => this.OnPropertyChanged( () => _IsReadOnly = value );
        }

    }
}
