using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Models;

namespace Config.Core.Web
{
    
    /// <summary>
    /// Proxy for calling UnifiedConfigurationService endpoints from WPF.
    /// </summary>
    public class UnifiedConfigurationProxy
    {
        public string JWT { get; set; } = string.Empty; // here purely for debug purposes

        public string UserID { get; set; } = string.Empty;

        private readonly HttpClient _http;

      





        /// <param name="baseAddress">Base URL of the UnifiedConfigurationService (e.g., https://localhost:5091)</param>
        /// <param name="httpMessageHandler">Optional custom handler (for auth, certificates, etc.)</param>
        public UnifiedConfigurationProxy(string baseAddress, HttpMessageHandler? httpMessageHandler = null)
        {
            if (string.IsNullOrWhiteSpace(baseAddress)) throw new ArgumentException("Base address required", nameof(baseAddress));
            _http = httpMessageHandler is null ? new HttpClient() : new HttpClient(httpMessageHandler);
            _http.BaseAddress = new Uri(baseAddress.TrimEnd('/'));


        }


        public async Task<TokenDetailsResult?> GetTokenDetailsAsync(CancellationToken ct = default)
        {
            TokenDetailsResult? retval = null;
            var resp = await _http.GetAsync("auth/TokenDetails/", ct);

            resp.EnsureSuccessStatusCode();
            
            retval = await resp.Content
                .ReadFromJsonAsync<TokenDetailsResult>(cancellationToken: ct)
                .ConfigureAwait(false);
            
            return retval;
        }   

        /// <summary>
        /// Create a token for accessing the service
        /// </summary>
        /// <param name="environment">DEV, QA, UAT or PROD</param>
        /// <param name="application">core app name</param>
        /// <param name="module">apps module</param>
        /// <param name="version">version</param>
        /// <param name="id">User Id</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<bool> TokenCreateAsync(string environment, string application, string module, /*string BuildNumber,*/ string id, CancellationToken ct = default)
        {
            bool retval = false;
            string body = string.Empty;
            var request = new TokenRequest()
            {
                Environment = environment,
                Application=application,
                Module = module,
                //BuildNumber = BuildNumber,
                UserId = id
            };
            var resp = await _http.PostAsJsonAsync($"auth/TokenCreate/", request );
            if (resp.IsSuccessStatusCode)
            {
                var token = await resp.Content
                    .ReadFromJsonAsync<TokenResponse>(cancellationToken: ct)
                    .ConfigureAwait(false);

                body = await resp.Content.ReadAsStringAsync( ct ).ConfigureAwait( false );

                var jwt = token?.Token;

                if (!string.IsNullOrWhiteSpace( jwt ))
                {

                    this._http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue( "Bearer", jwt );
                    

                    this.JWT = jwt;
                    this.UserID = id;
                    
                    retval = token?.IsAdmin ?? false;
                }

            }
            else
            {
                var errorContent = await resp.Content.ReadAsStringAsync( ct ).ConfigureAwait( false );
                throw new HttpRequestException( $"Token creation failed with status code {resp.StatusCode}: {errorContent}" );
            }

            return retval;
        }

        // If you need to attach tokens/cookies, set DefaultRequestHeaders on the HttpClient externally or add helpers.

        public async Task<List<string>> GetUniqueSectionsAsync()
        {
            UniqueSectionsResult? result=null;
            
            // Controller base route: [Route("config")] => config
            var resp = await _http.GetAsync( "config/GetUniqueSections/");

            // Throw execption if not successful
            resp.EnsureSuccessStatusCode();

            result = await resp.Content.ReadFromJsonAsync<UniqueSectionsResult>();

            var retval = result?.Sections ?? new List<string>();
            return retval;
        }

        public async Task<SectionResult?> GetSectionSettingsAsync(string section, string buildNumber)
        {
            if (string.IsNullOrWhiteSpace(section)) throw new ArgumentException("section required", nameof(section));

            //var resp = await _http.GetAsync( $"config/GetSection/sections/{Uri.EscapeDataString( section )}" );
            var resp = await _http.GetAsync( $"config/GetSectionSettings?section={Uri.EscapeDataString( section )}&buildNumber={Uri.EscapeDataString(buildNumber)}" );

            resp.EnsureSuccessStatusCode();

            var retval = await resp.Content.ReadFromJsonAsync<SectionResult>();

            return retval;
        }

        public async Task<SectionResult?> GetSectionSettingsAllBuildsAsync(string section)
        {
            if (string.IsNullOrWhiteSpace( section )) throw new ArgumentException( "section required", nameof( section ) );

            //var resp = await _http.GetAsync( $"config/GetSection/sections/{Uri.EscapeDataString( section )}" );
            var resp = await _http.GetAsync( $"config/GetSectionSettingsAllBuilds?section={Uri.EscapeDataString( section )}" );

            resp.EnsureSuccessStatusCode();

            var retval = await resp.Content.ReadFromJsonAsync<SectionResult>();

            return retval;
        }

        public async Task<SettingGetResult> GetSettingAsync(string section, string variable)
        {
            if (string.IsNullOrWhiteSpace(section)) throw new ArgumentException("section required", nameof(section));
            if (string.IsNullOrWhiteSpace(variable)) throw new ArgumentException("variable required", nameof(variable));
            var resp = await _http.GetAsync($"config/GetSetting?section={Uri.EscapeDataString(section)}&variable={Uri.EscapeDataString(variable)}");

            resp.EnsureSuccessStatusCode();

            var retval = await resp.Content.ReadFromJsonAsync<SettingGetResult>();

            return retval;
        }

        public async Task<bool> SetUserSettingAsync(string module, string buildNumber, string section, string variable, string value,string userId)
        {
            var payload = new SettingSetRequest
            {
                Section = section,
                Variable = variable,
                Value = value,
                Module = module,
                BuildNumber = buildNumber,
                UserId = userId,
                IsDefault = false
                //UserId = userId 
            };
            var resp = await _http.PostAsJsonAsync($"config/SetUserSetting", payload);

            resp.EnsureSuccessStatusCode();

            var retval = await resp.Content.ReadFromJsonAsync<bool>();
            return retval;
        }

        public async Task<bool> SetDefaultSettingAsync(string module,string buildNumber, string section, string variable, string value)
        {
            var payload = new SettingSetRequest
            {
                Section = section,
                Variable = variable,
                Value = value,
                Module = module,
                BuildNumber = buildNumber,
                IsDefault = true
            };
            var resp = await _http.PostAsJsonAsync($"config/SetDefaultSetting/", payload);

            resp.EnsureSuccessStatusCode();

            var retval = await resp.Content.ReadFromJsonAsync<bool>();
            return retval;
        }

        public async Task<bool> DeleteUserSettingAsync(string module, string section, string variable,  string buildNumber, string userId)
        {
            var payload = new SettingSetRequest
            {
                Section = section,
                Variable = variable,                
                Module = module,
                BuildNumber = buildNumber,
                UserId = userId,
                IsDefault = false                
            };

            //var resp = await _http.DeleteAsync($"config/DeleteUserSetting?section={Uri.EscapeDataString(section)}&variable={Uri.EscapeDataString(variable)}&buildNumber={ Uri.EscapeDataString( buildNumber )}");
            var resp = await _http.PostAsJsonAsync($"config/DeleteUserSetting/" , payload );
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteDefaultSettingAsync(string module, string section, string variable,  string buildNumber)
        {
            var payload = new SettingSetRequest
            {
                Section = section,
                Variable = variable,
                Module = module,
                BuildNumber = buildNumber,
                IsDefault = true
                //UserId = userId 
            };

            //var resp = await _http.DeleteAsync($"config/DeleteDefaultSetting/sections/{Uri.EscapeDataString(section)}/variables/{Uri.EscapeDataString(variable)}");
            var resp = await _http.PostAsJsonAsync($"config/DeleteDefaultSetting/", payload);
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> IsDefaultAuthorizedAsync()
        {
            // Returns the claim value from controller
            var resp = await _http.PostAsJsonAsync($"auth/IsDefaultAuthorized/", 123456);

            resp.EnsureSuccessStatusCode();

            var retval = await resp.Content.ReadFromJsonAsync<bool>();

            return retval;
        }
    }
}
