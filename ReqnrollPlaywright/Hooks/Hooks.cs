using Microsoft.VisualStudio.TestTools.UnitTesting;

using Reqnroll;
using ReqnrollPlaywright.Drivers;
using ReqnrollPlaywright.Utils;


namespace ReqnrollPlaywright.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly ScenarioContext _scenarioContext;
        private BrowserDriver? _browserDriver;

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public async Task BeforeScenario()
        {
            _browserDriver = new BrowserDriver();
            await _browserDriver.InitializeAsync();

            // Store driver in ScenarioContext so step definitions can reuse it
            _scenarioContext["BrowserDriver"] = _browserDriver;
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            if (_scenarioContext.TryGetValue("BrowserDriver", out BrowserDriver? browserDriver) && browserDriver != null)
            {
                // Optionally take screenshot if test failed
                if (_scenarioContext.TestError != null)
                {
                    var screenshotPath = await browserDriver.TakeScreenshotAsync(_scenarioContext.ScenarioInfo.Title);
                    TestHelper.LogError($"Scenario failed: {_scenarioContext.ScenarioInfo.Title}. Screenshot saved at {screenshotPath}", _scenarioContext.TestError);
                }

                else
                {
                    TestHelper.LogInfo($"Scenario passed: {_scenarioContext.ScenarioInfo.Title}");
                }

                browserDriver.Dispose();
            }
        }
    }
}
