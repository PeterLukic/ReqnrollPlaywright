using Microsoft.Playwright;
using ReqnrollPlaywright.Drivers;
[assembly: Parallelize(Workers = 3, Scope = ExecutionScope.MethodLevel)]

namespace ReqnrollPlaywright.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly ScenarioContext _scenarioContext;
        private IPage? _page;

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public async Task BeforeScenario()
        {
            var browserDriver = new BrowserDriver();
            await browserDriver.InitializeAsync();
            _page = browserDriver.Page;
            _scenarioContext["BrowserDriver"] = browserDriver;
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            if (_scenarioContext.TestError != null)
            {
                // Capture and attach screenshot on failure
            
                var screenshotPath = await _page!.ScreenshotAsync(new PageScreenshotOptions { Path = "screenshot.png" });

            }


            // Dispose browser driver
            if (_scenarioContext.TryGetValue("BrowserDriver", out BrowserDriver? browserDriver))
            {
                if (browserDriver != null)
                {
                    await browserDriver.DisposeAsync();
                }
            }
        }
    }
}
