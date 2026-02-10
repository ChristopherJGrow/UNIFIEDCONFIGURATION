using Config.Core.Extensions;
using Config.Core.WPF;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.Core.WPF
{

    public abstract class AsyncCmdBase : CmdBase
    {
        private bool _isExecuting;

        /// Optional: expose to bindings (spinner, etc.)
        public bool IsExecuting
        {
            get => _isExecuting;
            private set
            {
                if (_isExecuting != value)
                {
                    _isExecuting = value;
                    OnPropertyChanged( nameof( IsExecuting ) );
                }
            }
        }

        /// <summary>
        /// If true, command disables itself while running (prevents double-click reentrancy).
        /// </summary>
        protected virtual bool DisableWhileExecuting => true;
        
        public override void Execute(object obj)
        {
            try
            {               
                this.ExecuteSafe( obj );
            }
            catch (Exception ex)
            {
                this.LastError = ex;
                Console.WriteLine( $"CmdBase.Execute Exception: {ex.ToStringFull()} " );
            }
        }

        /// <summary>
        /// If false, re-entrancy allowed (rarely what you want).
        /// </summary>
        protected virtual bool AllowReentrancy => false;

        public sealed override void ExecuteSafe(object obj)
        {
            // CmdBase already checked IsEnabled before calling us.
            // But we also gate on in-flight execution.
            if (!AllowReentrancy && IsExecuting)
                return;

            // Fire-and-forget *safely* (we observe exceptions).
            _ = ExecuteInternalAsync( obj );
        }

        private async Task ExecuteInternalAsync(object obj)
        {
            bool previousEnabled = IsEnabled;

            try
            {
                IsExecuting = true;

                if (DisableWhileExecuting)
                {
                    IsEnabled = false;
                    // if your UI listens to CanExecuteChanged, you can also raise it:
                    OnCanExecuteChanged( EventArgs.Empty );
                }

                // Let WPF pump input/rendering before the real work begins.
                await Task.Yield();

                await ExecuteSafeAsync( obj );
            }
            catch (Exception ex)
            {
                // Keep your existing pattern consistent.
                LastError = ex;
                Console.WriteLine( $"AsyncCmdBase.Execute exception: {ex}" );
                // optionally: call an overridable hook
                OnAsyncException( ex, obj );                
            }
            finally
            {
                if (DisableWhileExecuting)
                {
                    IsEnabled = previousEnabled;
                    OnCanExecuteChanged( EventArgs.Empty );
                }

                IsExecuting = false;
            }
        }

        protected virtual void OnAsyncException(Exception ex, object obj) { }

        /// Implement this in derived commands
        protected abstract Task ExecuteSafeAsync(object obj);
    }

}