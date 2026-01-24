using Config.Core;
using Config.Core.Extensions;
using Config.Core.MVVM;
using Config.Core.Web;
using Config.Core.WPF;

using Microsoft.Extensions.Configuration;

using Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


using UnifiedConfiguration.PresentationModel.Cmd;

namespace UnifiedConfiguration.PresentationModel
{
    public enum NewItemType
    {
        Default,
        Override
    }

    public enum EnviormentType
    {
        DEV,
        QA,
        UAT,
        PROD
    }

    public class UnifiedConfigurationViewModel : CWindowViewModelBase
    {

        //Lazy<UnifiedConfigurationProxy> _Proxy = new Lazy<UnifiedConfigurationProxy>( ()=> new UnifiedConfigurationProxy("http://localhost:5191") );
        //Lazy<UnifiedConfigurationProxy> _Proxy = new Lazy<UnifiedConfigurationProxy>( ()=> new UnifiedConfigurationProxy("https://localhost:7214") );
        //Lazy<UnifiedConfigurationProxy> _Proxy = new Lazy<UnifiedConfigurationProxy>( ()=> new UnifiedConfigurationProxy("https://unifiedconfig.syndigo.com") );
        UnifiedConfigurationProxy Proxy { get; set; } = null;

        public UnifiedConfigurationViewModel()
        {
            
            var config = new ConfigurationBuilder()
                                .SetBasePath(AppContext.BaseDirectory)
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                .AddEnvironmentVariables()
                                .Build();
            
            var url = config.GetValue( "UnifiedConfigUrl", "" );

            this.Proxy = new UnifiedConfigurationProxy( url );

            //this.Proxy = new UnifiedConfigurationProxy( "https://unifiedconfig.syndigo.com" );
            //this.Proxy = new UnifiedConfigurationProxy( "https://localhost:7214" );
            //this.Proxy = new UnifiedConfigurationProxy( "http://localhost:5191" );
            //this.Proxy = new UnifiedConfigurationProxy( url );

            this.EnvironmentSource.Add( EnviormentType.DEV );
            this.EnvironmentSource.Add( EnviormentType.QA );
            this.EnvironmentSource.Add( EnviormentType.UAT );
            this.EnvironmentSource.Add( EnviormentType.PROD );
            
            string sid = WindowsIdentity.GetCurrent().User?.Value!;
            this._UserId = sid;
            //this.Environment = "DEV";
            this._Application = "IMS";

            this._Module = "PrintAgent";
            
            //this._BuildNumber = "600";

            this.CmdRowAdd = new CmdRowAdd() { Callback = this.RowAddAugment }; // This is data bound to DataGridBehaviors which will call back to augment new rows

            //this.IsAppAdmin = Task.Run<bool>( async () => await this.Proxy.IsDefaultAuthorizedAsync() ).Result;
            //NewItemInitializedCommand = new RelayCommand<object>( OnNewItemInitialized );

        }
        

        


        public UnifiedConfigurationViewModel Self => this;



        //---------------------------------

        NewItemType _NewItemTypeSelection=NewItemType.Override;
        public NewItemType NewItemTypeSelection
        {
            get => _NewItemTypeSelection;
            set => this.OnPropertyChanged( ()=>  _NewItemTypeSelection = value );
        }


        ObservableCollectionEx<NewItemType> _NewItemTypeSource = new ObservableCollectionEx<NewItemType>();
        public ObservableCollectionEx<NewItemType> NewItemTypeSource
        {
            get => _NewItemTypeSource;
            set => this.OnPropertyChanged( () => this._NewItemTypeSource = value );
        }

        //---------------------------------

        EnviormentType _EnvironmentSelection=EnviormentType.DEV;
        public EnviormentType EnvironmentSelection
        {
            get => this._EnvironmentSelection;
            set => this.OnPropertyChanged( () => { this._EnvironmentSelection = value; this.ItemsLoadFromRestToCache(); } );
        }


        ObservableCollectionEx<EnviormentType> _EnvironmentSource = new ObservableCollectionEx<EnviormentType>();
        public ObservableCollectionEx<EnviormentType> EnvironmentSource
        {
            get => _EnvironmentSource;
            set => this.OnPropertyChanged( () => this._EnvironmentSource = value );
        }

        //---------------------------------

        public NewItemType ItemSelectedType
        {
            get => this.ItemSelected.IsDefault ? NewItemType.Default : NewItemType.Override;
            set => this.OnPropertyChanged( () =>
            {
                if (this.ItemSelected != null)
                {
                    if (value == NewItemType.Default)
                    {
                        this.ItemSelected.UserId = string.Empty;
                    }
                    else
                    {
                        string sid = WindowsIdentity.GetCurrent().User?.Value!;
                        this.ItemSelected.UserId = sid;
                    }
                }
            } );
        }

        GridItemViewModel _ItemSelected=null;
        public GridItemViewModel ItemSelected
        {
            get => _ItemSelected;
            set => this.OnPropertyChanged( () => this._ItemSelected = value );
        }

        ObservableCollectionEx<GridItemViewModel> _ItemSource = new ObservableCollectionEx<GridItemViewModel>();
        public ObservableCollectionEx<GridItemViewModel> ItemSource
        {
            get => _ItemSource;
            set => this.OnPropertyChanged( () => this._ItemSource = value );
        }

        //---------------------------------

        bool _IsAppAdmin=false;
        public bool IsAppAdmin
        {
            get => _IsAppAdmin;
            set => this.OnPropertyChanged( () => _IsAppAdmin = value );
        }

        //string _Environment = string.Empty;
        //public string Environment
        //{
        //    get => this._Environment;
        //    set => this.OnPropertyChanged( () => this._Environment = value );

        //}
        string _Application = string.Empty;
        public string Application
        {
            get => this._Application;
            set => this.OnPropertyChanged( () => {this._Application = value; _ =  this.ItemsLoadFromRestToCache(); }   );
        }
        string _Module = string.Empty;
        public string Module
        {
            get => this._Module;
            set => this.OnPropertyChanged( () => { this._Module = value; _ =  this.ItemsLoadFromRestToCache(); } );
        }


        //public int BuildNumberActual => this._BuildNumber?.ToInt() ?? 0;

        public int BuildNumberActual => this._BuildNumber ?? 0;

        //string _BuildNumber = string.Empty;
        int? _BuildNumber = 1;
        public int? BuildNumber
        {
            get => this._BuildNumber;
            set
            {

                if (this.OnPropertyChanged( () => this._BuildNumber != value, () => this._BuildNumber = value ))
                {

                    this.OnPropertyChanged( nameof( this.IsUnfiltered ) );
                    this.OnPropertyChanged( nameof( this.IsUnfilteredInverse ) );

                    _ = this.ItemsLoadFromRestToCache();
                }
            }
        }

        public bool IsUnfilteredInverse => !this.IsUnfiltered;

        bool _IsUnfiltered = true;
        public bool IsUnfiltered
        {
            //get => this.BuildNumberActual <=0 ;
            get => this._IsUnfiltered;
            set 
            {
                if (this.OnPropertyChanged( () => this._IsUnfiltered != value, () => this._IsUnfiltered = value ))
                //this.OnPropertyChanged( () => this._IsAllUsersAllVersions = value );
                {                 

                    if (value)
                        this._BuildNumber = 0; // string.Empty;
                    //else
                    //    this._BuildNumber = 1; // "1";

                    this.OnPropertyChanged( nameof( this.IsUnfilteredInverse ) );
                    this.OnPropertyChanged( nameof( BuildNumber ) );
                    _ = this.ItemsLoadFromRestToCache();
                }
                
            }
        }
 






        string _UserId = string.Empty;
        public string UserId
        {
            get => this._UserId;
            set => this.OnPropertyChanged( () => this._UserId = value );
        }


        string _FilterVariable = string.Empty;
        public string FilterVariable
        {   get => this._FilterVariable;
            set
            {
                this.OnPropertyChanged( () => this._FilterVariable = value );
                this.ItemsLoadFromCache();
            }
        }

        string _FilterValue = string.Empty;
        public string FilterValue
        {
            get => this._FilterValue;
            set
            {
                this.OnPropertyChanged( () => this._FilterValue = value );
                this.ItemsLoadFromCache();
            }
        }


        public async Task Setup()
        {
            await this.ItemsLoadFromRestToCache();

            if (this.IsAppAdmin)
            {
                this.NewItemTypeSource.Add( NewItemType.Default ); /// only if you have access to this
                this.NewItemTypeSource.Add( NewItemType.Override );
            }
            else
                this.NewItemTypeSource.Add( NewItemType.Override );

            this.NewItemTypeSelection = NewItemType.Override;
        }

        List<GridItemViewModel> _ItemSourceCache = new List<GridItemViewModel>();

        public async Task ItemsLoadFromRestToCache()
        {
            
            this._ItemSourceCache.Clear();

            
            // Aloways create a new token because it holds the context with what Module, and Application your using 
            //
            this.IsAppAdmin = await this.Proxy.TokenCreateAsync( this.EnvironmentSelection.ToStringFast(), this.Application, this.Module,/* this.BuildNumber,*/ this.UserId );

            //this.IsAppAdmin = await this.Proxy.IsDefaultAuthorizedAsync();

            //
            // Need to test setting a default
            //
            var list = await this.Proxy.GetUniqueSectionsAsync();

            foreach (var section in list)
            {
                // Get all settings for section
                //var sectionResults = (this.BuildNumber.IsNullOrEmpty() || this.BuildNumber == "0") ? await this.Proxy.GetSectionSettingsAllBuildsAsync( section ) :
                var sectionResults = this.IsUnfiltered  ? await this.Proxy.GetSectionSettingsAllBuildsAsync( section ) :
                                                                    await this.Proxy.GetSectionSettingsAsync( section, this.BuildNumber?.ToString() ?? "0" );

                if (sectionResults == null)
                    continue;

                var givms = sectionResults.Settings.Select( (ITEM)=>
                {
                    return new GridItemViewModel()
                    {
                        Owner = this,
                        Module = ITEM.EffectiveModule,
                        Section = section,
                        Variable = ITEM.Variable,
                        Value = ITEM.Value,
                        UserId = ITEM.OverridingUserId,
                        UserPretty = this.DecodeUserIdToName(ITEM.OverridingUserId),
                        EffectiveBuildNumber = ITEM.EffectiveBuildNumber,
                        BuildNumber = ITEM.EffectiveBuildNumber,
                        IsDirty = false,
                    };
                });

                this._ItemSourceCache.AddRange( givms );
            }


            this.ItemsLoadFromCache();
        }

        public void  ItemsLoadFromCache()
        {          

            this.ItemSource.Clear();

            foreach( var item in this._ItemSourceCache)
            {
                if ( this.FilterVariable.IsNotNullOrEmpty() )
                {
                    if ( item.Variable.IndexOf( this.FilterVariable, StringComparison.OrdinalIgnoreCase ) < 0 )
                        continue;
                }
                if ( this.FilterValue.IsNotNullOrEmpty() )
                {
                    if ( item.Value.IndexOf( this.FilterValue, StringComparison.OrdinalIgnoreCase ) < 0 )
                        continue;
                }
                this.ItemSource.Add( item );
            }

        }

        public string DecodeUserIdToName(string userid)
        {
            if (SidHelp.IsSid( userid ))
            {
                return SidHelp.SidToAccountName( userid );
            }
            return string.Empty;
        }

        public void SelectedRowDelete()
        {
            

            if (this.ItemSelected != null && this.ItemSelected.IsDeletable)
            {
                this.ItemSelected.IsDeleted = !this.ItemSelected.IsDeleted;

                //var temp = this.ItemSelected;

                //this.ItemSource.Remove( this.ItemSelected );
                //this.ItemSelected = null;

                //Task.Run( async () =>
                //{
                //    if (temp.IsUserOverride)
                //        await this.Proxy.DeleteUserSettingAsync( temp.Section, temp.Variable ,temp.EffectiveBuildNumber);
                //    else
                //        await this.Proxy.DeleteDefaultSettingAsync( temp.Section, temp.Variable ,temp.EffectiveBuildNumber);

                //} ).GetAwaiter().GetResult();   
            }
            return;
        }

        public async Task<bool> SaveChangesAsync()
        {
            bool reloadNeeded = false;
            foreach (var item in this.ItemSource)
            {
                if (item.IsDirty)
                {
                    if (item.IsDeleted)
                    {
                        reloadNeeded = true;
                        if (item.IsUserOverride)
                        {
                            await this.Proxy.DeleteUserSettingAsync( item.Module, item.Section, item.Variable, item.EffectiveBuildNumber,item.Module );
                        }
                        else
                        {
                            await this.Proxy.DeleteDefaultSettingAsync( item.Module, item.Section, item.Variable, item.EffectiveBuildNumber );
                        }
                    }
                    else
                    {
                        if (item.IsUserOverride)
                        {
                            // New User Override
                            await this.Proxy.SetUserSettingAsync( item.Module, item.BuildNumber, item.Section, item.Variable, item.Value, item.UserId );
                        }
                        else if (this.IsAppAdmin)
                        {
                            await this.Proxy.SetDefaultSettingAsync( item.Module, item.BuildNumber, item.Section, item.Variable, item.Value );
                        }
                    }
                        
                    item.IsDirty = false;
                }
            }
            return reloadNeeded;
            //return Task.CompletedTask;
        }

        private void RowAddAugment(object obj)
        {            
            if (obj is GridItemViewModel row)
            {                
                switch (this.NewItemTypeSelection)
                {
                    case NewItemType.Default:
                        row.UserId = string.Empty;
                        break;
                    case NewItemType.Override:
                        string sid = WindowsIdentity.GetCurrent().User?.Value!;
                        row.UserId = sid;
                        break;
                }
                row.BuildNumber = "0";
            }
        }

        public void Update()
        {
            this.CmdRemoteSave.Update();

        }


        
        ICommandWithCallback _CmdRowAdd;
        public ICommandWithCallback CmdRowAdd
        {
            get => this._CmdRowAdd;
            set => this.OnPropertyChanged( () => this._CmdRowAdd = value );
        }

        ICommandViewModel _CmdRemoteSave = new CmdRemoteSave();
        public ICommandViewModel CmdRemoteSave => this._CmdRemoteSave;

        ICommandWithCallback _CmdDeleteRow = new CmdGridDelete();
        public ICommandWithCallback CmdDeleteRow => this._CmdDeleteRow;


        ICommandWithCallback _CmdGridCut = new CmdGridCut();
        public ICommandWithCallback CmdGridCut => this._CmdGridCut;

        ICommandWithCallback _CmdGridCopy = new CmdGridCopy();
        public ICommandWithCallback CmdGridCopy => this._CmdGridCopy;

        ICommandWithCallback _CmdGridPaste = new CmdGridPaste();
        public ICommandWithCallback CmdGridPaste => this._CmdGridPaste;

    }

    

    public static class IdentityLookup
    {
        //public static string? TryGetUpnFromSid(string sidValue)
        //{
        //    try
        //    {
        //        var sid = new SecurityIdentifier(sidValue);

        //        // Default domain context (uses the machine's domain)
        //        using var ctx = new PrincipalContext(ContextType.Domain);

        //        // Find the user by SID
        //        var user = UserPrincipal.FindByIdentity(ctx, IdentityType.Sid, sid.Value);

        //        return user?.UserPrincipalName; // e.g. user@company.com
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public static (string? Domain, string? User, string? DomainAndUser) TryGetDomainUserFromSid(string sidString)
        {
            try
            {
                var sid = new SecurityIdentifier(sidString);
                var account = (NTAccount)sid.Translate(typeof(NTAccount)); // e.g. "CONTOSO\\cgrow"

                var domainAndUser = account.Value;
                var parts = domainAndUser.Split('\\', 2);

                var domain = parts.Length == 2 ? parts[0] : null;
                var user   = parts.Length == 2 ? parts[1] : domainAndUser;

                return (domain, user, domainAndUser);
            }
            catch
            {
                // Not found / can't contact DC / SID orphaned / etc.
                return (null, null, null);
            }
        }

    }




}
