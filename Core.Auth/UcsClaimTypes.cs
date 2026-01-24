using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config.Core.Auth
{
    public static class UcsClaimTypes
    {
        public const string Enviornment =   "http://schemas.syndigo.com/ws/2025/11/identity/claims/Enviornment";
        public const string Application =   "http://schemas.syndigo.com/ws/2025/11/identity/claims/Application";
        public const string Module =        "http://schemas.syndigo.com/ws/2025/11/identity/claims/Module";
        //public const string BuildNumber =   "http://schemas.syndigo.com/ws/2025/11/identity/claims/BuildNumber";
        public const string UserId =        "http://schemas.syndigo.com/ws/2025/11/identity/claims/UserId";
        public const string IsDefaultAuth=  "http://schemas.syndigo.com/ws/2025/11/identity/claims/IsDefaultAuth";
    }
}
