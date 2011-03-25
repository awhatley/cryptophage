using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;

using Microsoft.Win32;

namespace Cryptophage
{
    internal static class GpgPathFinder
    {
        private static readonly Dictionary<string, string> RegistryLocations = new Dictionary<string, string>
        {
            { "SOFTWARE\\GNU\\GnuPG", "Install Directory" },
            { "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\GnuPG", "InstallLocation" },
            { "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\GPG4Win", "InstallLocation" },
            
            { "SOFTWARE\\Wow6432Node\\GNU\\GnuPG", "Install Directory" },
            { "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\GnuPG", "InstallLocation" },
            { "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\GPG4Win", "InstallLocation" },
        };

        private static string _gpgPath;

        public static string GetGpgPath()
        {
            return _gpgPath ?? (_gpgPath = DiscoverPathToGpg());
        }

        private static string DiscoverPathToGpg()
        {
            const string gpgExecutable = "gpg.exe";
            const string gpg2Executable = "gpg2.exe";

            var installLocation = GetGpgInstallLocation();
            var gpgPath1 = Path.Combine(installLocation, gpgExecutable);
            var gpgPath2 = Path.Combine(installLocation, gpg2Executable);

            return File.Exists(gpgPath1)
                ? gpgPath1
                : File.Exists(gpgPath2)
                    ? gpgPath2
                    : null;
        }

        private static string GetGpgInstallLocation()
        {
            const string defaultLocation = @"C:\Program Files\GNU\GnuPG";
            using(var localMachine = Registry.LocalMachine)
            {
                return RegistryLocations
                    .Select(location => GetRegistryKeyValue(localMachine, location.Key, location.Value))
                    .Where(location => location != null)
                    .DefaultIfEmpty(defaultLocation)
                    .First();
            }
        }

        private static string GetRegistryKeyValue(RegistryKey root, string subKeyPath, string name)
        {
            string value = null;

            try
            {
                using(var subKey = root.OpenSubKey(subKeyPath))
                {
                    if(subKey != null)
                        value = subKey.GetValue(name) as string;
                }
            }

            catch(SecurityException) { }
            catch(UnauthorizedAccessException) { }

            return value;
        }
    }
}