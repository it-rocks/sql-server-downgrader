using System.Windows;
using System.Collections.ObjectModel;
using SqlServerDowngrader.Models;
using SqlServerDowngrader.Services;

namespace SqlServerDowngrader
{
    public partial class MainWindow : Window
    {
        private InstanceDiscoveryService _discoveryService;
        private SqlServiceManager _serviceManager;
        private DatabaseConnectionService _dbService;
        private ConversionEngine _conversionEngine;
        private ObservableCollection<string> _instances;
        private ObservableCollection<string> _databases;

        public MainWindow()
        {
            InitializeComponent();
            InitializeServices();
            LoadInstances();
        }

        private void InitializeServices()
        {
            _discoveryService = new InstanceDiscoveryService();
            _serviceManager = new SqlServiceManager();
            _dbService = new DatabaseConnectionService();
            _conversionEngine = new ConversionEngine();
            _instances = new ObservableCollection<string>();
            _databases = new ObservableCollection<string>();
        }

        private void LoadInstances()
        {
            try
            {
                LogMessage("Discovering SQL Server instances...");
                var instances = _discoveryService.DiscoverInstances();
                
                _instances.Clear();
                foreach (var instance in instances)
                {
                    _instances.Add(instance);
                }

                SourceInstanceCombo.ItemsSource = _instances;
                TargetInstanceCombo.ItemsSource = _instances;
                
                LogMessage($"Found {instances.Count} SQL Server instance(s)");
            }
            catch (Exception ex)
            {
                LogMessage($"Error discovering instances: {ex.Message}", true);
            }
        }

        private void RefreshInstances_Click(object sender, RoutedEventArgs e)
        {
            LoadInstances();
        }

        private void StartService_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(SourceInstanceCombo.Text))
                {
                    MessageBox.Show("Please select a source instance first.");
                    return;
                }

                LogMessage($"Starting SQL Server service for {SourceInstanceCombo.Text}...");
                _serviceManager.StartService(SourceInstanceCombo.Text);
                
                UpdateServiceStatus();
                LogMessage("Service started successfully");
            }
            catch (Exception ex)
            {
                LogMessage($"Error starting service: {ex.Message}", true);
            }
        }

        private void UpdateServiceStatus()
        {
            try
            {
                if (string.IsNullOrEmpty(SourceInstanceCombo.Text))
                    return;

                bool isRunning = _serviceManager.IsServiceRunning(SourceInstanceCombo.Text);
                SourceServiceStatus.Text = isRunning ? "✓ Running" : "✗ Stopped";
                SourceServiceStatus.Foreground = isRunning ? 
                    System.Windows.Media.Brushes.Green : System.Windows.Media.Brushes.Red;

                StartServiceBtn.IsEnabled = !isRunning;
            }
            catch (Exception ex)
            {
                LogMessage($"Error updating service status: {ex.Message}");
            }
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.SaveFileDialog
            {
                Filter = "SQL Scripts (*.sql)|*.sql",
                FileName = "migration_script.sql"
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ExportPathTextbox.Text = dialog.FileName;
            }
        }

        private void SaveLog_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.SaveFileDialog
            {
                Filter = "Log Files (*.log)|*.log|Text Files (*.txt)|*.txt",
                FileName = $"migration_log_{DateTime.Now:yyyyMMdd_HHmmss}.log"
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.File.WriteAllText(dialog.FileName, LogTextbox.Text);
                MessageBox.Show("Log saved successfully");
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            LogTextbox.Clear();
            ConversionProgress.Value = 0;
            StatusText.Text = "Ready";
            StatusText.Foreground = System.Windows.Media.Brushes.Green;
            LogMessage("Log cleared - ready for new conversion");
        }

        private async void StartConversion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrEmpty(SourceInstanceCombo.Text))
                {
                    MessageBox.Show("Please select a source instance");
                    return;
                }

                if (string.IsNullOrEmpty(SourceDatabaseCombo.Text))
                {
                    MessageBox.Show("Please select a source database");
                    return;
                }

                if (string.IsNullOrEmpty(TargetInstanceCombo.Text))
                {
                    MessageBox.Show("Please select a target instance");
                    return;
                }

                // Create conversion settings
                var settings = new ConversionSettings
                {
                    SourceInstance = SourceInstanceCombo.Text,
                    SourceDatabase = SourceDatabaseCombo.Text,
                    TargetInstance = TargetInstanceCombo.Text,
                    TargetDatabase = string.IsNullOrEmpty(TargetDatabaseCombo.Text) ? 
                        SourceDatabaseCombo.Text : TargetDatabaseCombo.Text,
                    StructureOnly = StructureOnlyRadio.IsChecked == true,
                    TestMode = TestModeCheckbox.IsChecked == true,
                    IncludeIndexes = IncludeIndexesCheckbox.IsChecked == true,
                    IncludeConstraints = IncludeConstraintsCheckbox.IsChecked == true,
                    IncludeTriggers = IncludeTriggersCheckbox.IsChecked == true,
                    IncludeStoredProcedures = IncludeSPCheckbox.IsChecked == true,
                    PreserveIdentity = PreserveIdentityCheckbox.IsChecked == true,
                    ExportScript = ExportScriptCheckbox.IsChecked == true,
                    ExportPath = ExportPathTextbox.Text,
                    OverwriteTarget = OverwriteCheckbox.IsChecked == true
                };

                LogMessage("Starting conversion process...");
                StatusText.Text = "Converting...";
                StatusText.Foreground = System.Windows.Media.Brushes.Orange;

                // Execute conversion
                await _conversionEngine.ExecuteConversionAsync(settings, 
                    (progress, message) =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            ConversionProgress.Value = progress;
                            LogMessage(message);
                        });
                    });

                StatusText.Text = "Conversion completed successfully!";
                StatusText.Foreground = System.Windows.Media.Brushes.Green;
                LogMessage("✓ Conversion completed successfully");
            }
            catch (Exception ex)
            {
                StatusText.Text = "Error during conversion";
                StatusText.Foreground = System.Windows.Media.Brushes.Red;
                LogMessage($"✗ Error: {ex.Message}", true);
            }
        }

        private void LogMessage(string message, bool isError = false)
        {
            Dispatcher.Invoke(() =>
            {
                string timestamp = DateTime.Now.ToString("HH:mm:ss");
                string logEntry = $"[{timestamp}] {message}";
                LogTextbox.AppendText(logEntry + Environment.NewLine);
                LogTextbox.ScrollToEnd();

                if (isError)
                {
                    FooterText.Text = $"Error: {message}";
                    FooterText.Foreground = System.Windows.Media.Brushes.Red;
                }
                else
                {
                    FooterText.Text = message;
                    FooterText.Foreground = System.Windows.Media.Brushes.Gray;
                }
            });
        }
    }
}