


using Config.Core;
using Config.Core.Extensions; 
using Config.Core.Web;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.DependencyInjection;

using System.Security.Cryptography.X509Certificates;

using Yarp.ReverseProxy.Configuration;

//
// Thumbprints look like plain hex (e.g., A1B2C3...) but in the real world they often get pasted with:
// spaces( A1 B2 C3...)
// invisible Unicode characters (especially a Left-to-Right Mark that Windows sometimes includes when you copy from cert dialogs)
// mixed case
//
// Any of that can cause X509Store.Certificates.Find(FindByThumbprint, ...) to return zero results even though the cert is installed.
//
// THerefore we removes anything that’s not a hex digit (0-9, A-F) and uppercases it
//
string NormalizeThumbprint(string? thumbprint)
{
    if (string.IsNullOrWhiteSpace( thumbprint ))
        return string.Empty;

    // Keep only 0-9, a-f, A-F (strips spaces and weird invisible chars)
    var hexOnly = new string(thumbprint.Where(Uri.IsHexDigit).ToArray());

    return hexOnly.ToUpperInvariant();
}

X509Certificate2 LoadCertByThumbprint(string certThumbprint)
{
    using var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
    store.Open( OpenFlags.ReadOnly );

    // validOnly:false so you can diagnose chain/trust issues without “not found”
    var certs = store.Certificates.Find(X509FindType.FindByThumbprint, certThumbprint, validOnly: false);

    if (certs.Count == 0)
        throw new InvalidOperationException( $"Certificate {certThumbprint} not found in LocalMachine\\My." );

    var cert = certs[0];

    // This is the real test: can we open the private key?
    using var key = cert.GetRSAPrivateKey(); // or GetECDsaPrivateKey()
    if (!cert.HasPrivateKey || key is null)
        throw new InvalidOperationException( "Certificate found, but private key is not usable (permission/provider issue)." );

    return cert;
}


try
{

    Console.Title = "Reverse Proxy Service";

    //var config =  new ConfigurationBuilder()
    //   .SetBasePath( AppContext.BaseDirectory ) // where the exe is running
    //   .AddJsonFile( "appsettings.json", optional: false, reloadOnChange: true )
    //   .AddJsonFile( $"appsettings.{Environment.GetEnvironmentVariable( "DOTNET_ENVIRONMENT" ) ?? "Production"}.json",
    //                optional: true, reloadOnChange: true )
    //   .AddEnvironmentVariables()
    //   .Build();


    var builder = WebApplication.CreateBuilder(args);

    var certHash = NormalizeThumbprint(builder.Configuration.GetValue("Tls:CertThumbprint",string.Empty));

    if (certHash.IsNullOrEmpty())
    {
        throw new InvalidOperationException( "Cert thumbprint not set in appsettings.json" );
        
    }


    var cert = LoadCertByThumbprint(certHash);
    if (cert == null)
    {
        throw new InvalidOperationException( $"Cert thumbprint {certHash} not found " );
    }
    else
    {
        Console.WriteLine( $"Loaded certificate with thumbprint {cert.Thumbprint}" );
        Console.WriteLine( $"   FriendlyName: {cert.FriendlyName}" );
        Console.WriteLine( $"        Subject: {cert.Subject}" );
        Console.WriteLine( $"         Issuer: {cert.Issuer}" );
        Console.WriteLine( $"      NotBefore: {cert.NotBefore}" );
        Console.WriteLine( $"       NotAfter: {cert.NotAfter}" );
        Console.WriteLine( $"  HasPrivateKey: {cert.HasPrivateKey}" );
    }

    builder.WebHost.ConfigureKestrel( options =>
    {
        options.ListenAnyIP( 443, listen =>
        {
         

            listen.UseHttps( cert );
        } );
    } );


    // Load YARP routes/clusters from configuration (appsettings.json)
    builder.Services
        .AddReverseProxy()
        .LoadFromConfig( builder.Configuration.GetSection( "ReverseProxy" ) );

    // Optional but recommended (helps apps downstream know original scheme/client IP later)
    builder.Services.Configure<ForwardedHeadersOptions>( o =>
    {
        o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    } );

    var app = builder.Build();

    app.UseForwardedHeaders();

    // This is the actual proxy pipeline
    app.MapReverseProxy();

    app.Run();

}
catch(Exception ex)
{
    Console.WriteLine( $"Error: {ex.ToString()}");
    
}