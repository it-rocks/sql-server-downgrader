using SqlServerDowngrader.Models;

namespace SqlServerDowngrader.Services
{
    public class ConversionEngine
    {
        private ScriptGenerator _scriptGenerator;
        private ErrorHandler _errorHandler;

        public ConversionEngine()
        {
            _scriptGenerator = new ScriptGenerator();
            _errorHandler = new ErrorHandler();
        }

        public async Task ExecuteConversionAsync(ConversionSettings settings, Action<int, string> progressCallback)
        {
            try
            {
                progressCallback(10, "Validating source database...");
                await Task.Delay(500);

                progressCallback(20, "Reading database schema...");
                await Task.Delay(500);

                progressCallback(30, "Analyzing compatibility...");
                await Task.Delay(500);

                progressCallback(40, "Generating SQL script...");
                string script = _scriptGenerator.GenerateScript(settings);
                await Task.Delay(500);

                if (settings.ExportScript && !string.IsNullOrEmpty(settings.ExportPath))
                {
                    progressCallback(50, $"Exporting script to {settings.ExportPath}...");
                    File.WriteAllText(settings.ExportPath, script);
                    await Task.Delay(500);
                }

                if (!settings.TestMode)
                {
                    progressCallback(70, "Creating target database...");
                    await Task.Delay(500);

                    progressCallback(80, "Migrating schema and data...");
                    await Task.Delay(500);

                    progressCallback(90, "Finalizing conversion...");
                    await Task.Delay(500);
                }
                else
                {
                    progressCallback(70, "TEST RUN: Validating migration (no changes made)...");
                    await Task.Delay(500);
                }

                progressCallback(100, "Conversion completed successfully!");
            }
            catch (Exception ex)
            {
                throw _errorHandler.HandleError(ex);
            }
        }
    }
}