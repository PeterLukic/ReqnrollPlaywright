using Microsoft.Playwright;
using ReqnrollPlaywright.Utils;

namespace ReqnrollPlaywright.PageObjects
{
    public class LoginPage : BasePage
    {
        // Selectors based on the provided HTML structure
        private readonly string _usernameInput = "input[name='username']";
        private readonly string _passwordInput = "input[name='password']";
        private readonly string _loginButton = "button[type='submit']";
        private readonly string _loginForm = "form";
        private readonly string _errorMessage = ".oxd-alert-content";
        private readonly string _forgotPasswordLink = "a[href*='forgot-password']";
        private readonly string _pageTitle = "h5";

        public LoginPage(IPage page) : base(page)
        {
        }

        public async Task NavigateToLoginPageAsync()
        {
            var baseUrl = ConfigurationManager.GetBaseUrl();
            TestHelper.LogInfo($"Navigating to login page: {baseUrl}");
            await NavigateToAsync(baseUrl);
            await TestHelper.WaitForPageLoadAsync(_page);
        }

        public async Task<bool> IsLoginPageDisplayedAsync()
        {
            try
            {
                await WaitForSelectorAsync(_loginForm);
                return await IsVisibleAsync(_usernameInput) && await IsVisibleAsync(_passwordInput);
            }
            catch
            {
                return false;
            }
        }

        public async Task EnterUsernameAsync(string username)
        {
            TestHelper.LogInfo($"Entering username: {username}");
            await WaitForSelectorAsync(_usernameInput);
            await FillAsync(_usernameInput, username);
        }

        public async Task EnterPasswordAsync(string password)
        {
            TestHelper.LogInfo("Entering password");
            await WaitForSelectorAsync(_passwordInput);
            await FillAsync(_passwordInput, password);
        }

        public async Task ClickLoginButtonAsync()
        {
            TestHelper.LogInfo("Clicking login button");
            await ClickAsync(_loginButton);
            await TestHelper.WaitForPageLoadAsync(_page);
        }

        public async Task LoginAsync(string username, string password)
        {
            await EnterUsernameAsync(username);
            await EnterPasswordAsync(password);
            //await ClickLoginButtonAsync();
        }

        public async Task LoginWithDefaultCredentialsAsync()
        {
            var username = ConfigurationManager.GetUsername();
            var password = ConfigurationManager.GetPassword();
            await LoginAsync(username, password);
        }

        public async Task<bool> IsErrorMessageDisplayedAsync()
        {
            try
            {
                return await IsVisibleAsync(_errorMessage);
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> GetErrorMessageAsync()
        {
            if (await IsErrorMessageDisplayedAsync())
            {
                return await GetTextAsync(_errorMessage);
            }
            return string.Empty;
        }

        public async Task<bool> IsLoginSuccessfulAsync()
        {
            // After successful login, the URL should change and login form should not be visible
            await Task.Delay(2000); // Wait for navigation
            var currentUrl = _page.Url;
            return !currentUrl.Contains("auth/login") && !await IsVisibleAsync(_loginForm);
        }

        public async Task ClickForgotPasswordAsync()
        {
            await ClickAsync(_forgotPasswordLink);
        }
    }
}