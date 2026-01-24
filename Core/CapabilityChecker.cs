using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Config.Core
{


    /*

        CREATE TABLE Capability(
        CapabilityId INT IDENTITY PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL UNIQUE  -- e.g. 'Config.EditGlobalDefaults'
    );

    CREATE TABLE SecurityGroup(
        SecurityGroupId INT IDENTITY PRIMARY KEY,
        DomainGroupName NVARCHAR(200) NOT NULL UNIQUE  -- e.g. 'DOMAIN\\ConfigGlobalAdmins'
    );

    CREATE TABLE SecurityGroupCapability(
        SecurityGroupId INT NOT NULL,
        CapabilityId INT NOT NULL,
        PRIMARY KEY (SecurityGroupId, CapabilityId),
        FOREIGN KEY (SecurityGroupId) REFERENCES SecurityGroup(SecurityGroupId),
        FOREIGN KEY (CapabilityId) REFERENCES Capability(CapabilityId)
    );

    CREATE TABLE CapabilityScope (
    CapabilityScopeId INT IDENTITY PRIMARY KEY,
    CapabilityId INT NOT NULL,
    EnvironmentName NVARCHAR(50) NULL,  -- null = any
    ApplicationName NVARCHAR(100) NULL,
    ModuleName NVARCHAR(100) NULL
);
    */

    public static class CapabilityChecker
    {
        // Ideally cache this in memory
        private static readonly object _lock = new object();
        private static Dictionary<string, HashSet<string>> _groupToCapabilities;
        private static DateTime _lastLoaded;
        private static readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

        public static bool UserHasCapability(IPrincipal user, string capabilityName)
        {
            if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
                return false;

            return true;

            EnsureCacheLoaded();

            // Enumerate groups user belongs to
            var windowsIdentity = user.Identity as WindowsIdentity;
            if (windowsIdentity == null)
                return false;

            var groups = windowsIdentity.Groups;
            if (groups == null) return false;

            foreach (var sid in groups)
            {
                string groupName;
                try
                {
                    groupName = sid.Translate( typeof( NTAccount ) ).ToString(); // "DOMAIN\\GroupName"
                }
                catch
                {
                    continue;
                }

                if (_groupToCapabilities.TryGetValue( groupName, out var caps ) &&
                    caps.Contains( capabilityName ))
                {
                    return true;
                }
            }

            return false;
        }

        private static void EnsureCacheLoaded()
        {
            if (_groupToCapabilities != null && DateTime.UtcNow - _lastLoaded < _cacheDuration)
                return;

            lock (_lock)
            {
                if (_groupToCapabilities != null && DateTime.UtcNow - _lastLoaded < _cacheDuration)
                    return;

                // TODO: load from DB
                // _groupToCapabilities = LoadGroupCapabilitiesFromDatabase();
                // For example:
                // "DOMAIN\\ConfigGlobalAdmins" => {"Config.EditGlobalDefaults", "Config.EditAppDefaults"}
                // "DOMAIN\\ConfigSupport"      => {"Config.EditUserOverrides"}

                _lastLoaded = DateTime.UtcNow;
            }
        }
    }
}
