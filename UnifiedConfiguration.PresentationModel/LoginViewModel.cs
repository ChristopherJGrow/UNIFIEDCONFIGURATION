using Config.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnifiedConfiguration.PresentationModel
{
    public sealed class LoginViewModel
    {
        private readonly IAuthService _auth;

        public LoginViewModel(IAuthService auth) => _auth = auth;

        public async Task LoginCommandAsync()
        {
            var res = await _auth.LoginAsync();
            if (!res.Success)
            {
                // show message via your dialog service, set status text, etc
            }
            else
            {
                // store token, call your UCS service, etc
            }
        }
    }
}
