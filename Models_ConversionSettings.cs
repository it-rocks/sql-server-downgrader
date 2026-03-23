namespace SqlServerDowngrader.Models
{
    public class ConversionSettings
    {
        public string SourceInstance { get; set; }
        public string SourceDatabase { get; set; }
        public string TargetInstance { get; set; }
        public string TargetDatabase { get; set; }
        public bool StructureOnly { get; set; }
        public bool TestMode { get; set; }
        public bool IncludeIndexes { get; set; }
        public bool IncludeConstraints { get; set; }
        public bool IncludeTriggers { get; set; }
        public bool IncludeStoredProcedures { get; set; }
        public bool PreserveIdentity { get; set; }
        public bool ExportScript { get; set; }
        public string ExportPath { get; set; }
        public bool OverwriteTarget { get; set; }
    }
}