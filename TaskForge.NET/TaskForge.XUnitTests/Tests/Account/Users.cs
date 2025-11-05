using Microsoft.Playwright;
using Xunit;
using static Microsoft.Playwright.Assertions;

namespace TaskForge.XUnitTests.Tests.Account
{
    public class Users
    {
        [Fact]
        public async Task Test_ViewUsersList()
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

            // Login as Admin
            await page.GetByRole(AriaRole.Main).GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("syl.enamul@gmail.com");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("Admin@123");
            await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Welcome to TaskForge" })).ToBeVisibleAsync();

            await page.GetByRole(AriaRole.Link, new() { Name = "Users" }).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Heading)).ToContainTextAsync("User List");
        }

        [Fact]
        public async Task Test_CreateNewUser()
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

            // Login as Admin
            await page.GetByRole(AriaRole.Main).GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("syl.enamul@gmail.com");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("Admin@123");
            await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Welcome to TaskForge" })).ToBeVisibleAsync();

            await page.GetByRole(AriaRole.Link, new() { Name = "Users" }).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Navigation, new() { Name = "Main Navigation" })).ToBeVisibleAsync();

            await page.GetByRole(AriaRole.Link, new() { Name = "Create New User" }).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Navigation, new() { Name = "Main Navigation" })).ToBeVisibleAsync();

            // Generate random email and full name for unique test data
            var uniqueId = Guid.NewGuid().ToString().Substring(0, 8);
            var fullName = $"Test User {uniqueId}";
            var email = $"testuser{uniqueId}@gmail.com";

            await page.GetByRole(AriaRole.Textbox, new() { Name = "FullName" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "FullName" }).FillAsync(fullName);
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync(email);
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password", Exact = true }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password", Exact = true }).FillAsync("test@123");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "ConfirmPassword" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "ConfirmPassword" }).FillAsync("test@123");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "PhoneNumber" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "PhoneNumber" }).FillAsync("01234567891");
            await page.GetByLabel("Role").ClickAsync();
            await page.GetByLabel("Role").SelectOptionAsync(new[] { "User" });
            
            // Submit the form
            await page.GetByRole(AriaRole.Button, new() { Name = " Create" }).ClickAsync();

            // Verify redirection to User List
            await Expect(page.GetByRole(AriaRole.Heading)).ToContainTextAsync("User List");

            // Search for the newly created user by email
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Search" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Search" }).FillAsync(email);
            await page.GetByRole(AriaRole.Button, new() { Name = "Filter" }).ClickAsync();
            
            // Verify the user appears in the list with correct email and name
            await Expect(page.Locator("tbody")).ToContainTextAsync(email);
            await Expect(page.Locator("tbody")).ToContainTextAsync(fullName);

        }


        [Fact]
        public async Task Test_DeleteUser()
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

            // Login as Admin
            await page.GetByRole(AriaRole.Main).GetByRole(AriaRole.Link, new() { Name = "Login" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("syl.enamul@gmail.com");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("Admin@123");
            await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Heading, new() { Name = "Welcome to TaskForge" })).ToBeVisibleAsync();

            // Navigate to Users page
            await page.GetByRole(AriaRole.Link, new() { Name = "Users" }).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Heading)).ToContainTextAsync("User List");

            // Create a new user to delete
            await page.GetByRole(AriaRole.Link, new() { Name = "Create New User" }).ClickAsync();
            await Expect(page.GetByRole(AriaRole.Navigation, new() { Name = "Main Navigation" })).ToBeVisibleAsync();

            // Generate random email and full name for unique test data
            var uniqueId = Guid.NewGuid().ToString().Substring(0, 8);
            var fullName = $"Delete Test User {uniqueId}";
            var email = $"deletetest{uniqueId}@gmail.com";

            await page.GetByRole(AriaRole.Textbox, new() { Name = "FullName" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "FullName" }).FillAsync(fullName);
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync(email);
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password", Exact = true }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Password", Exact = true }).FillAsync("test@123");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "ConfirmPassword" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "ConfirmPassword" }).FillAsync("test@123");
            await page.GetByRole(AriaRole.Textbox, new() { Name = "PhoneNumber" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "PhoneNumber" }).FillAsync("01234567891");
            await page.GetByLabel("Role").ClickAsync();
            await page.GetByLabel("Role").SelectOptionAsync(new[] { "User" });
            
            // Submit the form
            await page.GetByRole(AriaRole.Button, new() { Name = " Create" }).ClickAsync();

            // Verify redirection to User List
            await Expect(page.GetByRole(AriaRole.Heading)).ToContainTextAsync("User List");

            // Search for the newly created user by email
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Search" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Search" }).FillAsync(email);
            await page.GetByRole(AriaRole.Button, new() { Name = "Filter" }).ClickAsync();
            
            // Verify the user appears in the list
            await Expect(page.Locator("tbody")).ToContainTextAsync(email);
            await Expect(page.Locator("tbody")).ToContainTextAsync(fullName);

            // Click on the Delete link for the created user
            await page.Locator("tbody").GetByRole(AriaRole.Link).First.ClickAsync();
            await Expect(page.GetByRole(AriaRole.Navigation, new() { Name = "Main Navigation" })).ToBeVisibleAsync();

            // Confirm deletion
            await page.GetByRole(AriaRole.Button, new() { Name = "Yes, Delete" }).ClickAsync();
            
            // Verify successful deletion message
            await Expect(page.GetByRole(AriaRole.Main)).ToContainTextAsync("Delete: User successfully deleted.");

            // Optional: Verify user no longer appears in the list
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Search" }).ClickAsync();
            await page.GetByRole(AriaRole.Textbox, new() { Name = "Search" }).FillAsync(email);
            await page.GetByRole(AriaRole.Button, new() { Name = "Filter" }).ClickAsync();
            
            // The user should not exist anymore
            await Expect(page.Locator("tbody")).Not.ToContainTextAsync(email);
        }
    }
}