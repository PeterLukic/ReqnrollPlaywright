using Microsoft.Playwright;
using ReqnrollPlaywright.Utils;

namespace ReqnrollPlaywright.PageObjects
{
    public abstract class BasePage
    {
        protected readonly IPage _page;
        protected readonly int _timeout;

        protected BasePage(IPage page)
        {
            _page = page;
            _timeout = ConfigurationManager.GetTimeout();
        }

        protected async Task ClickAsync(string selector)
        {
            await _page.ClickAsync(selector, new PageClickOptions { Timeout = _timeout });
        }

        protected async Task FillAsync(string selector, string value)
        {
            await _page.FillAsync(selector, value, new PageFillOptions { Timeout = _timeout });
        }

        protected async Task<string> GetTextAsync(string selector)
        {
            return await _page.TextContentAsync(selector, new PageTextContentOptions { Timeout = _timeout }) ?? string.Empty;
        }

        protected async Task<bool> IsVisibleAsync(string selector)
        {
            return await _page.IsVisibleAsync(selector);
        }

        protected async Task WaitForSelectorAsync(string selector)
        {
            await _page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions { Timeout = _timeout });
        }

        protected async Task NavigateToAsync(string url)
        {
            await _page.GotoAsync(url, new PageGotoOptions { Timeout = _timeout });
        }
    }
}