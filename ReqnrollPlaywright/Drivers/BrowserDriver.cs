using Microsoft.Playwright;
using ReqnrollPlaywright.Utils;

namespace ReqnrollPlaywright.Drivers
{
    public class BrowserDriver : IAsyncDisposable
    {
        private IPlaywright? _playwright;
        private IBrowser? _browser;
        private IBrowserContext? _context;
        private IPage? _page;
        private bool _disposed = false;

        public IPage Page => _page ?? throw new InvalidOperationException("Browser not initialized");
        public IBrowserContext Context => _context ?? throw new InvalidOperationException("Browser context not initialized");

        public async Task InitializeAsync()
        {
            TestHelper.LogInfo("Initializing browser driver");

            _playwright = await Playwright.CreateAsync();
            var browserType = ConfigurationManager.GetBrowserType().ToLower();
            var headless = ConfigurationManager.GetHeadless();
            var slowMo = ConfigurationManager.GetSlowMo();

            var launchOptions = new BrowserTypeLaunchOptions
            {
                Headless = headless,
                SlowMo = slowMo,
                Args = new[] { "--start-maximized" }
            };

            _browser = browserType switch
            {
                "firefox" => await _playwright.Firefox.LaunchAsync(launchOptions),
                "webkit" => await _playwright.Webkit.LaunchAsync(launchOptions),
                _ => await _playwright.Chromium.LaunchAsync(launchOptions)
            };

            var contextOptions = new BrowserNewContextOptions
            {
                ViewportSize = ViewportSize.NoViewport,
                RecordVideoDir = ConfigurationManager.GetVideoRecord() ? ConfigurationManager.GetReportPath() + "/Videos" : null
            };

            _context = await _browser.NewContextAsync(contextOptions);
            _page = await _context.NewPageAsync();
            _page.SetDefaultTimeout(ConfigurationManager.GetTimeout());

            TestHelper.LogInfo($"Browser initialized successfully: {browserType}");
        }

        public async Task<string> TakeScreenshotAsync(string testName)
        {
            if (_page == null) throw new InvalidOperationException("Page not initialized");

            // Save screenshots in Reports/Screenshots folder
            string screenshotsDir = Path.Combine(ConfigurationManager.GetReportPath(), "Screenshots");
            Directory.CreateDirectory(screenshotsDir);

            string screenshotPath = Path.Combine(screenshotsDir, $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            await _page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath });
            return screenshotPath;
        }

        public async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                try
                {
                    TestHelper.LogInfo("Disposing browser driver");
                    if (_page != null) await _page.CloseAsync();
                    if (_context != null) await _context.CloseAsync();
                    if (_browser != null) await _browser.CloseAsync();
                    _playwright?.Dispose();
                }
                catch (Exception ex)
                {
                    TestHelper.LogError("Error disposing browser driver", ex);
                }
                finally
                {
                    _disposed = true;
                }
            }
        }
    }
}
