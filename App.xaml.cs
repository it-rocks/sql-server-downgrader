using System.Windows;

namespace SqlServerDowngrader
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Global exception handling
            AppDomain.CurrentDomain.UnhandledException += (s, ex) =>
            {
                MessageBox.Show($"Application Error: {ex.ExceptionObject}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            };
            
            Application.Current.DispatcherUnhandledException += (s, ex) =>
            {
                MessageBox.Show($"Dispatcher Error: {ex.Exception.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                ex.Handled = true;
            };
        }
    }
}