# SQL Server Database Downgrader

A professional Windows desktop application for migrating SQL Server databases to lower versions with comprehensive safety features and detailed logging.

## Features

✅ **Instance Discovery**
- Automatic detection of local SQL Server instances
- Network instance scanning
- Service status monitoring

✅ **Service Management**
- Check SQL Server service status
- Start stopped services with one click
- Automatic status refresh

✅ **Database Selection**
- Browse all databases on selected instance
- Select source and target databases
- Auto-create target database option

✅ **Conversion Modes**
- **Structure Only**: Migrate schema without data
- **Structure + Data**: Complete database migration

✅ **Safety Features**
- Test Run (dry-run) mode without actual changes
- Version compatibility checking
- Backup recommendations
- Detailed error reporting

✅ **Advanced Options**
- Include/Exclude Indexes
- Include/Exclude Constraints
- Include/Exclude Triggers
- Include/Exclude Stored Procedures
- Preserve Identity Values
- Export migration script

✅ **Logging & Monitoring**
- Real-time conversion progress
- Detailed activity logging
- Save logs to file
- Timestamp on all messages

✅ **Supported Versions**
- SQL Server 2012 (v11)
- SQL Server 2014 (v12)
- SQL Server 2016 (v13)
- SQL Server 2017 (v14)
- SQL Server 2019 (v15)
- SQL Server 2022 (v16)
- SQL Server 2025 (v17)

## Installation

### Prerequisites
- Windows 7 or later
- .NET Framework 4.8+
- SQL Server 2012 or higher
- Administrator rights

### Setup
1. Download the latest release
2. Extract files to desired location
3. Run `SqlServerDowngrader.exe`

## Usage

### Basic Workflow

1. **Select Source Database**
   - Click "🔄 Refresh" to discover instances
   - Select source instance and database
   - Monitor service status

2. **Select Target**
   - Choose target instance
   - Specify target database (auto-created if empty)

3. **Configure Options**
   - Choose conversion mode
   - Enable Test Run if desired
   - Select advanced options

4. **Execute**
   - Review settings
   - Click "▶ Start Conversion"
   - Monitor progress and logs

### Test Run
- Enable "Test Run" mode before production migration
- Validates all conversion steps
- No changes made to target database

## Building from Source

### Requirements
- Visual Studio 2019 or 2022
- .NET Framework 4.8+ SDK
- Git

### Build Steps
```bash
git clone https://github.com/it-rocks/sql-server-downgrader.git
cd sql-server-downgrader
dotnet build
dotnet publish
```

## Project Structure

```
sql-server-downgrader/
├── App.xaml                              # Application resources
├── App.xaml.cs                           # Application startup
├── MainWindow.xaml                       # Main UI
├── MainWindow.xaml.cs                    # UI logic
├── Models/
│   ├── DatabaseInstance.cs
│   └── ConversionSettings.cs
├── Services/
│   ├── InstanceDiscoveryService.cs
│   ├── SqlServiceManager.cs
│   ├── DatabaseConnectionService.cs
│   ├── ConversionEngine.cs
│   ├── ScriptGenerator.cs
│   └── ErrorHandler.cs
└── SqlServerDowngrader.csproj
```

## Troubleshooting

### Issue: Cannot find SQL Server instances
**Solution:**
- Ensure SQL Server Browser service is running
- Check Windows Firewall settings
- Verify network connectivity

### Issue: Permission denied errors
**Solution:**
- Run application as Administrator
- Check SQL Server user permissions
- Verify database login credentials

### Issue: Conversion fails
**Solution:**
- Enable Test Run to validate settings
- Review detailed error messages in log
- Check SQL Server error logs
- Ensure target version is lower than source

## Error Messages and Solutions

The application provides intelligent error detection with automated solution suggestions:

- **Connection Errors**: Instance name, service status, firewall, permissions
- **Permission Errors**: Administrator rights, user permissions, credentials
- **Version Errors**: Target version compatibility, feature compatibility

## Logging

All activities are logged with timestamps:
- Save logs: Use "💾 Save Log" button
- Log format: Plain text
- Includes: Errors, warnings, progress updates

## Security Considerations

- Always create backups before migration
- Test on non-production environments first
- Review generated SQL scripts before execution
- Monitor SQL Server logs during migration
- Use Windows Authentication when possible

## Limitations

- Downgrade only (cannot upgrade versions)
- Some advanced features may not be compatible with lower versions
- Large database migrations may take extended time
- Network instances require SQL Browser service

## License

MIT License - See LICENSE file

## Support

For issues, suggestions, or contributions:
1. Visit: https://github.com/it-rocks/sql-server-downgrader
2. Create an issue with detailed information
3. Include error logs and system information

## Contributing

Contributions welcome! Please:
1. Fork the repository
2. Create feature branch
3. Submit pull request with description

## Changelog

### v1.0.0 (Initial Release)
- Instance discovery and management
- Database selection and migration
- Test run capabilities
- Error handling with solutions
- Logging and export features