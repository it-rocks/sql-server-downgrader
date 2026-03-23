using System.Management;
using Microsoft.Win32;

namespace SqlServerDowngrader.Services
{
    public class InstanceDiscoveryService
    {
        public List<string> DiscoverInstances()
        {
            var instances = new List<string>();

            // Check local registry for SQL Server instances
            try
            {
                var localKey = Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Microsoft\Microsoft SQL Server");

                if (localKey != null)
                {
                    var instanceNames = localKey.GetValueNames()
                        .Where(n => n == "InstalledInstances")
                        .ToList();

                    if (instanceNames.Count > 0)
                    {
                        var installedInstances = localKey.GetValue("InstalledInstances") as string[];
                        if (installedInstances != null)
                        {
                            foreach (var instance in installedInstances)
                            {
                                string serverName = instance == "MSSQLSERVER" ? 
                                    Environment.MachineName : 
                                    $"{Environment.MachineName}\\{instance}";
                                instances.Add(serverName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading local instances: {ex.Message}");
            }

            // Discover network instances using SQL Server Browser
            try
            {
                var searcher = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_Service WHERE Name LIKE 'MSSQL%'");

                foreach (ManagementObject obj in searcher.Get())
                {
                    var displayName = obj["DisplayName"]?.ToString();
                    if (!string.IsNullOrEmpty(displayName) && !instances.Contains(displayName))
                    {
                        instances.Add(displayName);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error discovering network instances: {ex.Message}");
            }

            return instances.Distinct().ToList();
        }

        public int GetSqlServerVersion(string instanceName)
        {
            try
            {
                var key = Registry.LocalMachine.OpenSubKey(
                    @"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL");

                if (key != null)
                {
                    var instancePath = key.GetValue(instanceName)?.ToString();
                    if (!string.IsNullOrEmpty(instancePath))
                    {
                        var versionKey = Registry.LocalMachine.OpenSubKey(
                            $@"SOFTWARE\Microsoft\Microsoft SQL Server\{instancePath}\Setup");

                        if (versionKey != null)
                        {
                            var version = versionKey.GetValue("Version")?.ToString();
                            if (!string.IsNullOrEmpty(version))
                            {
                                return int.Parse(version.Split('.')[0]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting version: {ex.Message}");
            }

            return 0;
        }
    }
}