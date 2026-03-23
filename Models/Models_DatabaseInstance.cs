namespace SqlServerDowngrader.Models
{
    public class DatabaseInstance
    {
        public string InstanceName { get; set; }
        public string ServerName { get; set; }
        public int Version { get; set; }
        public string VersionName { get; set; }
        public bool IsLocalInstance { get; set; }
        public bool IsServiceRunning { get; set; }
        public List<string> Databases { get; set; } = new List<string>();
        public DateTime LastRefreshed { get; set; }
    }
}