using Microsoft.Playwright;
using System.Text;

namespace ReqnrollPlaywright.Utils
{
    public static class TestHelper
    {
        public static async Task<string> TakeScreenshotAsync(IPage page, string testName)
        {
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var screenshotPath = Path.Combine(ConfigurationManager.GetReportPath(), "Screenshots", $"{testName}_{timestamp}.png");
            
            Directory.CreateDirectory(Path.GetDirectoryName(screenshotPath)!);
            
            await page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = screenshotPath,
                FullPage = true
            });
            
            return screenshotPath;
        }

        public static void LogInfo(string message)
        {
            Console.WriteLine($"[INFO] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        }

        public static void LogError(string message, Exception? exception = null)
        {
            Console.WriteLine($"[ERROR] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            if (exception != null)
            {
                Console.WriteLine($"Exception: {exception.Message}");
                Console.WriteLine($"StackTrace: {exception.StackTrace}");
            }
        }

        public static string GenerateRandomString(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new StringBuilder(length);
            
            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }
            
            return result.ToString();
        }

        public static async Task WaitForElementAsync(IPage page, string selector, int timeoutMs = 30000)
        {
            await page.WaitForSelectorAsync(selector, new PageWaitForSelectorOptions
            {
                Timeout = timeoutMs
            });
        }

        public static async Task WaitForPageLoadAsync(IPage page)
        {
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }
    }
}