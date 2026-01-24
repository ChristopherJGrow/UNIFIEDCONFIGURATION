




using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Config.Core.Extensions;

namespace Config.Core.Auth
{

    public interface ITokenService
    {
        //string              SecretKey { get; set; }

        bool TokenIsValid(string token, string secret = null);
        string TokenGenerate(ITokenConfig model, string secret = null);

        IEnumerable<Claim> TokenClaimsGet(string token, string secret = null);

        //SecurityKey SymmetricSecurityKeyGet(string secret = null);

        TokenValidationParameters TokenValidationParametersGet(string secret = null);
    }

    //[Export( ScopeOptions.Singleton, typeof( ITokenService ) )]
    public class CTokenMgr : ITokenService
    {
        protected CTokenMgr()
        {
        }

        static Lazy<CTokenMgr> __Instance = new Lazy<CTokenMgr>( () => new CTokenMgr() );
        public static CTokenMgr Instance => __Instance.Value;

        protected SecurityKey SymmetricSecurityKeyGet(string secret = null)
        {
            //var settings = Factory.Resolve<ISettingsProvider>();

            var secretConfigured = Environment.GetEnvironmentVariable("+UNIFIEDCONFIG_SECRET") ?? "Long Live YoYo Dyne don't let this be your secret              ";            

            secret ??= secretConfigured;

            if ( secret.IsNullOrEmpty() )
                throw new Exception( "Secret was null or empty" );

            // This will have problems because the key can't have any repeating characters in it
            // Day Month and Year could have repeats in it..
            //var mySecretKey = "A this is my custom Secret";
            //var mySecretKey  = $"A {key}"; // {DateTime.UtcNow.Month} {DateTime.UtcNow.Day} {DateTime.UtcNow.Year}";
            //var mySecretKey  = $"{key} 11 11 2022"; // {DateTime.UtcNow.Month} {DateTime.UtcNow.Day} {DateTime.UtcNow.Year}";
            //var mySecretKey  = $"{key}"; // {DateTime.UtcNow.Month} {DateTime.UtcNow.Day} {DateTime.UtcNow.Year}";

            
            // This is currently a problem because Kestral creates a single TokenValidationParamters object
            // As the day changes if not refreshed all tokens will fail after this.
            // once this is solved add this line back in
            //
            //secret = $"{secret} {DateTime.UtcNow.Month} {DateTime.UtcNow.Day} {DateTime.UtcNow.Year}";            

            // Much happier at encoding strings that Convert.FromBase64String
            //ASCIIEncoding ascii = new ASCIIEncoding();
            //var symmetricKey  = ascii.GetBytes( secret.ToCharArray() );

            var symmetricKey = Encoding.UTF8.GetBytes( secret.ToCharArray() );

            return new SymmetricSecurityKey( symmetricKey );
        }

        public TokenValidationParameters TokenValidationParametersGet(string secret = null)
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = this.SymmetricSecurityKeyGet( secret ),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        }

        public IEnumerable<Claim> TokenClaimsGet(string token, string secret = null)
        {
            if (token.IsNullOrEmpty())
                throw new ArgumentException( "Given token is null or empty" );

            TokenValidationParameters tokenValidationParameters = TokenValidationParametersGet(secret);
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token,tokenValidationParameters,out SecurityToken validatedToken);
                return tokenValid.Claims;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool TokenIsValid(string token, string secret = null)
        {
            if (token.IsNullOrEmpty())
                throw new ArgumentException( "Given token is null or empty" );

            TokenValidationParameters tokenValidationParameters = TokenValidationParametersGet(secret);

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                ClaimsPrincipal tokenValid = jwtSecurityTokenHandler.ValidateToken(token,tokenValidationParameters,out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string TokenGenerate(ITokenConfig model, string secret = null)
        {
            if (model == null || model.Claims == null || model.Claims.Length == 0)
                throw new ArgumentException( "Arguments to create token are not valid" );

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject=new ClaimsIdentity(model.Claims),
                //Expires = DateTime.UtcNow.AddMinutes( Convert.ToInt32( model.ExpireMinutes )  ),
                Expires = DateTime.UtcNow.AddMinutes( model.ExpireMinutes   ),
                SigningCredentials = new SigningCredentials(this.SymmetricSecurityKeyGet(secret),model.SecurityAlgorithm)
                // Consider setting Issuer and Audience when enabling their validation.
                // Issuer = model.Issuer,
                // Audience = model.Audience
            };
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            string retval = jwtSecurityTokenHandler.WriteToken(securityToken);

            return retval;
        }



    }
}

