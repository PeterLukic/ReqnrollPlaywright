using Allure.Net.Commons;
using Microsoft.Playwright;
using ReqnrollPlaywright.Drivers;

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

            // Add Allure labels
            AllureApi.SetTestName(_scenarioContext.ScenarioInfo.Title);
            AllureApi.SetOwner("Perica");
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            if (_scenarioContext.TestError != null)
            {
                // Capture and attach screenshot on failure
                var screenshotPath = await _page!.ScreenshotAsync(new PageScreenshotOptions { Path = "screenshot.png" });
                AllureApi.AddAttachment("Screenshot", "image/png", screenshotPath);
            }

            // Dispose browser driver
            if (_scenarioContext.TryGetValue("BrowserDriver", out BrowserDriver? browserDriver))
            {
                await browserDriver.DisposeAsync();
            }
        }
    }
}
