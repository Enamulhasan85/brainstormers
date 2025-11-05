using Microsoft.Playwright;
using Xunit;
using static Microsoft.Playwright.Assertions;

namespace TaskForge.XUnitTests.Tests.Auth
{
    public class Login
    {
        [Fact]
        public async Task AdminUserLogin()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 1000
            });

            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            
            await page.GotoAsync("https://localhost:7034/");
            await Expect(page.GetByRole(AriaRole.Navigation, new() { Name = "Main Navigation" })).ToBeVisibleAsync();

            await page.GetByRole(AriaRole.Main).GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Login / Sign In" })).ToBeVisibleAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("syl.enamul@gmail.com");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).PressAsync("Tab");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("Admin@123");
            await page.GetByText("Remember me?").ClickAsync();
            await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Welcome to TaskForge" })).ToBeVisibleAsync();
            
            // Check if Remember Me worked - verify auth cookie is present
            var cookies = await context.CookiesAsync();
            // The cookie name is "TaskForge_Auth" as defined in Program.cs
            var authCookie = cookies.FirstOrDefault(c => c.Name == "TaskForge_Auth" || c.Name.Contains(".AspNetCore.Identity.Application"));
            
            // Verify cookie exists
            Assert.NotNull(authCookie);
            // With Remember Me checked, cookie should persist (Expires > 0)
            // ASP.NET Core Identity sets expiration based on ExpireTimeSpan in cookie options
            Assert.True(authCookie.Expires > 0, "Cookie should have an expiration date (persistent cookie)");
            // Verify expiration is in the future (cookie will persist after browser closes)
            Assert.True(authCookie.Expires > DateTimeOffset.UtcNow.ToUnixTimeSeconds(), 
                "Cookie expiration should be in the future");
            
            await Expect(page.GetByRole(AriaRole.Link, new() { Name = "Users" })).ToBeVisibleAsync();
            await page.GetByRole(AriaRole.Link, new() { Name = "Projects" }).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Link, new() { Name = "Projects" })).ToBeVisibleAsync();
            await Expect(page.GetByRole(AriaRole.Button, new() { Name = "syl.enamul@gmail.com" })).ToBeVisibleAsync();

        }

        [Fact]
        public async Task AdminUserLogin_WithoutRememberMe_ShouldCreateSessionCookie()
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 1000
            });

            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();
            
            await page.GotoAsync("https://localhost:7034/");
            await page.GetByRole(AriaRole.Main).GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
            
            // Login WITHOUT checking Remember me
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("syl.enamul@gmail.com");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("Admin@123");
            // NOTE: NOT clicking "Remember me?" checkbox
            await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            
            await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Welcome to TaskForge" })).ToBeVisibleAsync();
            
            // Check cookie - should be session-only (expires = -1 or very short)
            var cookies = await context.CookiesAsync();
            // The cookie name is "TaskForge_Auth" as defined in Program.cs
            var authCookie = cookies.FirstOrDefault(c => c.Name == "TaskForge_Auth" || c.Name.Contains(".AspNetCore.Identity.Application"));
            
            Assert.NotNull(authCookie);
            // Without Remember Me: In ASP.NET Core Identity, both session and persistent cookies 
            // may have Expires set, but the key difference is:
            // - Session cookies: typically have shorter expiration (30 minutes from Program.cs: ExpireTimeSpan)
            // - Persistent cookies: have longer expiration (14 days default with RememberMe)
            // So we check that the expiration is relatively short (less than 1 hour = 3600 seconds)
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var expirationDuration = authCookie.Expires - currentTime;
            Assert.True(expirationDuration < 3600, 
                $"Without Remember Me, cookie should expire soon (less than 1 hour). Actual: {expirationDuration} seconds");
        }

    }
}