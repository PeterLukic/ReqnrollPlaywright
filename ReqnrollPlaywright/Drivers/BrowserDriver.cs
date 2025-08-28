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
            try
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
                
                // Set default timeout
                _page.SetDefaultTimeout(ConfigurationManager.GetTimeout());
                
                TestHelper.LogInfo($"Browser initialized successfully: {browserType}");
            }
            catch (Exception ex)
            {
                TestHelper.LogError("Failed to initialize browser", ex);
                throw;
            }
        }

        public async Task<string> TakeScreenshotAsync(string testName)
        {
            if (_page == null) throw new InvalidOperationException("Page not initialized");
            return await TestHelper.TakeScreenshotAsync(_page, testName);
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
