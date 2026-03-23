namespace SqlServerDowngrader.Services
{
    public class ErrorHandler
    {
        public Exception HandleError(Exception ex)
        {
            var errorMessage = new System.Text.StringBuilder();
            errorMessage.AppendLine($"Error: {ex.Message}");
            errorMessage.AppendLine();
            errorMessage.AppendLine("Possible Solutions:");

            if (ex.Message.Contains("connection"))
            {
                errorMessage.AppendLine("1. Verify SQL Server instance name");
                errorMessage.AppendLine("2. Ensure SQL Server service is running");
                errorMessage.AppendLine("3. Check firewall settings");
                errorMessage.AppendLine("4. Verify user has necessary permissions");
            }
            else if (ex.Message.Contains("permission"))
            {
                errorMessage.AppendLine("1. Run application as Administrator");
                errorMessage.AppendLine("2. Check database user permissions");
                errorMessage.AppendLine("3. Verify SQL Server login credentials");
            }
            else if (ex.Message.Contains("version"))
            {
                errorMessage.AppendLine("1. Verify target SQL Server version is lower than source");
                errorMessage.AppendLine("2. Check for incompatible features");
                errorMessage.AppendLine("3. Review compatibility warnings");
            }

            return new Exception(errorMessage.ToString(), ex);
        }
    }
}