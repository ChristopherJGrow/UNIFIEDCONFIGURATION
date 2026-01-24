using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;


namespace Config.Core.Auth
{
    public interface ITokenConfig
    {
        #region Members
        
        string SecurityAlgorithm { get; set; }
        int ExpireMinutes { get; set; }
        Claim[] Claims { get; set; }
        #endregion
    }
   

    public class CTokenConfigJWT : ITokenConfig
    {

        public int ExpireMinutes { get; set; } = 60 * 24 * 2; // 2 days

        // No duplicate characters can go in this.. and there are some lenght minimums
        //
        //public string   SecretKey { get; set; } =   $"this is my custom Secret key for authnetication   Hapy          {DateTime.UtcNow.Month} {DateTime.UtcNow.Day} {DateTime.UtcNow.Year}";
        // does not work  "Happy Happy Joy Joy to the world to which                      ";
        //                "this is my custom Secret key for authnetication                ";
        // does not work .. "Happy Happy Joy Joy to the world to which";
        // works "IW9zaGVFcmV6UHJpdmF0ZUtleQ==";
        // works "this is my custom Secret key for authnetication                "; //"this is my custom Secret key for authnetication                "; //"IW9zaGVFcmV6UHJpdmF0ZUtleQ=="; //"HappyHappyJoyJoy"; // this secret key should be move to a configured value..
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;

        public Claim[] Claims { get; set; }
    }
}
