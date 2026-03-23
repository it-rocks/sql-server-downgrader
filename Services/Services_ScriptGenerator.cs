using SqlServerDowngrader.Models;

namespace SqlServerDowngrader.Services
{
    public class ScriptGenerator
    {
        public string GenerateScript(ConversionSettings settings)
        {
            var script = new System.Text.StringBuilder();

            script.AppendLine("-- SQL Server Database Migration Script");
            script.AppendLine($"-- Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            script.AppendLine($"-- Source: {settings.SourceInstance}.{settings.SourceDatabase}");
            script.AppendLine($"-- Target: {settings.TargetInstance}.{settings.TargetDatabase}");
            script.AppendLine();

            if (settings.OverwriteTarget)
            {
                script.AppendLine($"-- Drop existing database if it exists");
                script.AppendLine($"IF EXISTS (SELECT 1 FROM sys.databases WHERE name = '{settings.TargetDatabase}')");
                script.AppendLine($"    DROP DATABASE [{settings.TargetDatabase}];");
                script.AppendLine();
            }

            script.AppendLine($"-- Create target database");
            script.AppendLine($"CREATE DATABASE [{settings.TargetDatabase}];");
            script.AppendLine($"GO");
            script.AppendLine();

            script.AppendLine($"USE [{settings.TargetDatabase}];");
            script.AppendLine($"GO");
            script.AppendLine();

            if (!settings.StructureOnly)
            {
                script.AppendLine("-- Note: Data migration should be executed separately");
            }

            return script.ToString();
        }
    }
}