using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Config.Core
{
    public static class SidHelp
    {
        public static string? SidToAccountName(string sidString)
        {
            if (string.IsNullOrWhiteSpace( sidString ))
                return null;

            try
            {
                var sid = new SecurityIdentifier(sidString);
                var account = (NTAccount)sid.Translate(typeof(NTAccount));
                // e.g. "MYDOMAIN\\jdoe"
                return account.Value;
            }
            catch (IdentityNotMappedException)
            {
                // SID is valid but can’t be resolved (no matching account)
                return null;
            }
            catch (Exception)
            {
                // Invalid SID format or other issue
                return null;
            }
        }


        // Basic “S-1-5-21-…” style SID pattern
        private static readonly Regex SidRegex = new(
        @"^S-\d-\d+(-\d+){1,14}$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        public static bool IsSid(string? value)
        {
            if (string.IsNullOrWhiteSpace( value ))
                return false;

            // Cheap shape/format check first
            if (!SidRegex.IsMatch( value ))
                return false;

            // Let Windows do the real validation
            try
            {
                _ = new SecurityIdentifier( value );
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (SystemException)
            {
                // Covers other invalid-SID cases
                return false;
            }
        }
    }
}
