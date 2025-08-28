using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReqnrollPlaywright.Drivers;
using ReqnrollPlaywright.PageObjects;
using ReqnrollPlaywright.Utils;
using Reqnroll;

namespace ReqnrollPlaywright.StepDefinitions
{
    [Binding]
    public class OrangeHRMLoginStepDefinitions
    {
        private readonly BrowserDriver _browserDriver;
        private readonly LoginPage _loginPage;
        private readonly ScenarioContext _scenarioContext;

        public OrangeHRMLoginStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _browserDriver = new BrowserDriver();
            _browserDriver.InitializeAsync().GetAwaiter().GetResult();
            _loginPage = new LoginPage(_browserDriver.Page);
        }

        // ---------------- GIVEN ----------------

        [Given(@"I navigate to the OrangeHRM login page")]
        [Given(@"I am on the OrangeHRM login page")]
        public async Task GivenIAmOnTheOrangeHRMLoginPage()
        {
            await _loginPage.NavigateToLoginPageAsync();
            var isDisplayed = await _loginPage.IsLoginPageDisplayedAsync();
            Assert.IsTrue(isDisplayed, "Login page is not displayed");
        }

        // ---------------- WHEN ----------------

        [When(@"I enter valid username and password")]
        public async Task WhenIEnterValidUsernameAndPassword()
        {
            await _loginPage.LoginWithDefaultCredentialsAsync();
        }

        [When(@"I enter invalid username ""([^""]*)"" and password ""([^""]*)""")]
        public async Task WhenIEnterInvalidUsernameAndPassword(string username, string password)
        {
            await _loginPage.LoginAsync(username, password);
        }

        [When(@"I leave username and password fields empty")]
        public async Task WhenILeaveUsernameAndPasswordFieldsEmpty()
        {
            await _loginPage.LoginAsync("", "");
        }

        [When(@"I enter username ""([^""]*)"" and password ""([^""]*)""")]
        public async Task WhenIEnterUsernameAndPassword(string username, string password)
        {
            await _loginPage.LoginAsync(username, password);
        }

        [When(@"I click the login button")]
        public async Task WhenIClickTheLoginButton()
        {
            await _loginPage.ClickLoginButtonAsync();
        }

        // ---------------- THEN ----------------

        [Then(@"I should see the dashboard page")]
        public async Task ThenIShouldSeeTheDashboardPage()
        {
            var isLoginSuccessful = await _loginPage.IsLoginSuccessfulAsync();
            Assert.IsTrue(isLoginSuccessful, "Dashboard page not displayed after login");
        }

        [Then(@"I should see an error message")]
        public async Task ThenIShouldSeeAnErrorMessage()
        {
            var isErrorVisible = await _loginPage.IsErrorMessageDisplayedAsync();
            Assert.IsTrue(isErrorVisible, "Error message not displayed");
        }

        [Then(@"I should see validation errors")]
        public async Task ThenIShouldSeeValidationErrors()
        {
            var isErrorVisible = await _loginPage.IsErrorMessageDisplayedAsync();
            Assert.IsTrue(isErrorVisible, "Validation errors not displayed for empty credentials");
        }

        [Then(@"the login result should be ""([^""]*)""")]
        public async Task ThenTheLoginResultShouldBe(string expectedResult)
        {
            if (expectedResult.Equals("success", StringComparison.OrdinalIgnoreCase))
            {
                var isLoginSuccessful = await _loginPage.IsLoginSuccessfulAsync();
                Assert.IsTrue(isLoginSuccessful, "Login was expected to succeed but failed");
            }
            else
            {
                var isErrorVisible = await _loginPage.IsErrorMessageDisplayedAsync();
                Assert.IsTrue(isErrorVisible, "Login was expected to fail but succeeded");
            }
        }

 
    }
}
