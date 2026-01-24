//--------------------------------------------------------------------
// © Copyright 1989-2025 Syndigo, LLC. - All rights reserved.
// This file contains confidential and proprietary trade secrets of
// Syndigo, LLC.  Reproduction, disclosure or use without specific 
// written authorization from Syndigo, LLC. is prohibited.
// For more information see: http://www.syndigo.com
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Config.Core //UnifiedConfiguration.PresentationModel
{




    public interface IUIContextable
    {
        IUserBaseInterfaceContext Ubic { get; set; }
    }


    public interface IViewModelBase : INotifyPropertyChanged, IUIContextable, IDisposable
    {
        IViewModelBase Owner { get; set; }


        void Update(object obj = null);


    }

    public interface IUserInterfaceClipboardable
    {
        void ClipboardCopy(string prefix, string thing);
        bool ClipboardHasData(string prefix);
        string ClipboardPaste(string prefix);
    }
    public interface IUserInterfaceMessagable
    {
        (bool, string) InputBox(string message, string defResponse);
        void MessageBox(string message, string title);

    }
    public interface IUserInterfaceErrorable
    {
        void Error(string message);
    }

    public interface IUserInterfaceBusyable
    {
        /// <summary>
        /// Anything you put in here the UI can hook and make look busy otherwiese it just runs
        /// </summary>
        void Busy(Action func);
    }

    public interface IUserInterfaceDispachable
    {
        void DispachIfNeeded(Action func);
    }

    public interface IUserBaseInterfaceContext : IUserInterfaceClipboardable, IUserInterfaceMessagable, IUserInterfaceErrorable, IUserInterfaceBusyable, IUserInterfaceDispachable
    {



    }

    /// <summary>
    /// Interface for something that can present or show an error message
    /// </summary>
    public interface IErrorPresenter
    {
        void ErrorShow(string msg, Exception ex = null);
    }


    public interface IWindowViewModel
    {
        /// <summary>
        /// Code executed in here should give an indication the UI is busy
        /// </summary>
        //Action<Action> BusyFunc { get; set; }
    }

    public interface IDialogViewModel : IWindowViewModel
    {
        /// <summary>
        /// True if the dialog was meant to be canceled by the user 
        /// </summary>
        bool IsCanceled { get; set; }
    }

    public interface ICommandViewModel : ICommand, IViewModelBase
    {
        string Text { get; set; }
        bool IsEnabled { get; set; }
        bool IsVisible { get; set; }

        //void Update(object vm=null);
    }

    public interface ICommandWithCallback : ICommandViewModel
    {
        /// <summary>
        /// Used to run the UI code that isn't available in the View Model or other places
        /// </summary>
        Action<object> Callback { get; set; }
    }

    public interface IAuthService
    {
        Task<AuthResult> LoginAsync();
        Task LogoutAsync();
        bool IsAuthenticated { get; }
    }
    public sealed class AuthResult
    {
        public bool Success { get; init; }
        public string? AccessToken { get; init; }
        public string? IdToken { get; init; }
        public string? Error { get; init; }
    }


}
