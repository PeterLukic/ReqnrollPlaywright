using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReqnrollPlaywright.Drivers;
using ReqnrollPlaywright.Utils;
using Reqnroll;

namespace ReqnrollPlaywright.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly ScenarioContext _scenarioContext;

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public async Task BeforeScenario()
        {
            // Only create browser if it doesn't exist in ScenarioContext
            if (!_scenarioContext.TryGetValue("BrowserDriver", out BrowserDriver? browserDriver) || browserDriver == null)
            {
                browserDriver = new BrowserDriver();
                await browserDriver.InitializeAsync();
                _scenarioContext["BrowserDriver"] = browserDriver;
            }
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            if (_scenarioContext.TryGetValue("BrowserDriver", out BrowserDriver? browserDriver) && browserDriver != null)
            {
                // Take screenshot if scenario failed
                if (_scenarioContext.TestError != null)
                {
                    var screenshotPath = await browserDriver.TakeScreenshotAsync(_scenarioContext.ScenarioInfo.Title);
                    TestHelper.LogError($"Scenario failed: {_scenarioContext.ScenarioInfo.Title}. Screenshot saved at {screenshotPath}", _scenarioContext.TestError);
                }
                else
                {
                    TestHelper.LogInfo($"Scenario passed: {_scenarioContext.ScenarioInfo.Title}");
                }

                await browserDriver.DisposeAsync();
            }
        }
    }
}
