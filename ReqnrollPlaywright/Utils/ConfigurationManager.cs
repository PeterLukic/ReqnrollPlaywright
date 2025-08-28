using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ReqnrollPlaywright.Utils
{
    public class ConfigurationManager
    {
        private static IConfiguration? _configuration;
        private static readonly object _lock = new object();

        public static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    lock (_lock)
                    {
                        if (_configuration == null)
                        {
                            InitializeConfiguration();
                        }
                    }
                }
                return _configuration!;
            }
        }

        private static void InitializeConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("Utils/appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
        }

        public static string GetBaseUrl() => Configuration["TestSettings:BaseUrl"] ?? throw new InvalidOperationException("BaseUrl not configured");
        public static string GetUsername() => Configuration["TestSettings:Username"] ?? throw new InvalidOperationException("Username not configured");
        public static string GetPassword() => Configuration["TestSettings:Password"] ?? throw new InvalidOperationException("Password not configured");
        public static string GetBrowserType() => Configuration["TestSettings:BrowserType"] ?? "chromium";
        public static bool GetHeadless() => bool.Parse(Configuration["TestSettings:Headless"] ?? "false");
        public static int GetTimeout() => int.Parse(Configuration["TestSettings:Timeout"] ?? "30000");
        public static int GetParallelWorkers() => int.Parse(Configuration["TestSettings:ParallelWorkers"] ?? "1");
        public static int GetSlowMo() => int.Parse(Configuration["TestSettings:SlowMo"] ?? "0");
        public static bool GetVideoRecord() => bool.Parse(Configuration["TestSettings:VideoRecord"] ?? "false");
        public static bool GetScreenshotOnFailure() => bool.Parse(Configuration["TestSettings:ScreenshotOnFailure"] ?? "true");

        // Updated to get project root path for reports
        public static string GetReportPath()
        {
            var configPath = Configuration["ReportSettings:ReportPath"];
            if (!string.IsNullOrEmpty(configPath))
            {
                return Path.IsPathRooted(configPath) ? configPath : GetProjectRootPath(configPath);
            }
            return GetProjectRootPath("TestResults");
        }

        public static string GetReportName() => Configuration["ReportSettings:ReportName"] ?? "TestReport";
        public static bool GetGenerateHtmlReport() => bool.Parse(Configuration["ReportSettings:GenerateHtmlReport"] ?? "true");

        // Helper method to get project root path
        private static string GetProjectRootPath(string relativePath)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            // If running from bin/Debug/net8.0, go up to project root
            if (currentDirectory.Contains("bin"))
            {
                var projectRoot = Directory.GetParent(currentDirectory)?.Parent?.Parent?.FullName;
                if (projectRoot != null)
                {
                    return Path.Combine(projectRoot, relativePath);
                }
            }

            // Fallback to current directory + relative path
            return Path.Combine(currentDirectory, relativePath);
        }
    }
}