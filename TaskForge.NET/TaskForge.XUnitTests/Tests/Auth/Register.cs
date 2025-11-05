using Microsoft.Playwright;
using Xunit;
using static Microsoft.Playwright.Assertions;

namespace TaskForge.XUnitTests.Tests.Auth
{
    public class Register
    {
        [Fact]
        public async Task Test1UserRegistration()
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

            await page.GetByRole(AriaRole.Link, new() { Name = "Register" }).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Navigation, new() { Name = "Main Navigation" })).ToBeVisibleAsync();

            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync($"test1{Guid.NewGuid()}@gmail.com");
            await page.Locator("#password").ClickAsync();
            await page.Locator("#password").FillAsync("test1@123");
            await page.Locator("#confirmPassword").ClickAsync();
            await page.Locator("#confirmPassword").FillAsync("test1@123");
            
            // Submit the form by clicking the Register button
            await page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
            
            // Wait for the success message
            await Expect(page.GetByRole(AriaRole.Paragraph)).ToContainTextAsync("Please check your email to confirm your account.");
        }

    }
}