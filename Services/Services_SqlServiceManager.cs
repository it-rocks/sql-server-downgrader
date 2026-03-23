using System.ServiceProcess;

namespace SqlServerDowngrader.Services
{
    public class SqlServiceManager
    {
        public bool IsServiceRunning(string instanceName)
        {
            try
            {
                string serviceName = instanceName == "MSSQLSERVER" ? 
                    "MSSQLSERVER" : $"MSSQL${instanceName}";

                var service = ServiceController.GetServices()
                    .FirstOrDefault(s => s.ServiceName == serviceName);

                return service != null && service.Status == ServiceControllerStatus.Running;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking service: {ex.Message}");
                return false;
            }
        }

        public void StartService(string instanceName)
        {
            try
            {
                string serviceName = instanceName == "MSSQLSERVER" ? 
                    "MSSQLSERVER" : $"MSSQL${instanceName}";

                var service = ServiceController.GetServices()
                    .FirstOrDefault(s => s.ServiceName == serviceName);

                if (service != null && service.Status != ServiceControllerStatus.Running)
                {
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error starting service: {ex.Message}");
            }
        }

        public void StopService(string instanceName)
        {
            try
            {
                string serviceName = instanceName == "MSSQLSERVER" ? 
                    "MSSQLSERVER" : $"MSSQL${instanceName}";

                var service = ServiceController.GetServices()
                    .FirstOrDefault(s => s.ServiceName == serviceName);

                if (service != null && service.Status == ServiceControllerStatus.Running)
                {
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error stopping service: {ex.Message}");
            }
        }
    }
}