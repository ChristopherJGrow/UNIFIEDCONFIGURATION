using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;



namespace Models
{
    //public record TokenResponse([property: JsonPropertyName( "token" )] string Token);
    public class TokenResponse
    {
        public TokenResponse()
        {
        }
        public TokenResponse( string token )
        {
            Token = token;
        }   
        [JsonPropertyName( "token" )]
        public string Token { get; set; } = "";
        public bool IsAdmin { get; set; }
    }

    public class TokenRequest
    {
                
        public string Environment  { get; set; } = "";
        public string Application  { get; set; } = "";
        public string Module       { get; set; } = "";
        public string BuildNumber  { get; set; } = "";
        public string UserId          { get; set; } = null; // Either the user we want settings for or null and then we use the web request
    }

    public class TokenDetailsResult : TokenRequest
    {
        public DateTime ExpirationUtc { get; set; } = DateTime.UtcNow;
    }
}
