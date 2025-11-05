using Microsoft.Extensions.Configuration;

namespace TaskForge.NUnitPlayTests.Configuration
{
    public class TestConfiguration
    {
        private static readonly Lazy<TestConfiguration> _instance = 
            new Lazy<TestConfiguration>(() => new TestConfiguration());

        public static TestConfiguration Instance => _instance.Value;

        private readonly IConfiguration _configuration;

        private TestConfiguration()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        #region Application Settings

        public string BaseUrl
        {
            get
            {
                var url = _configuration["BaseUrl"] ??
                          Environment.GetEnvironmentVariable("BASE_URL") ??
                          "https://localhost:7034";
                return url.TrimEnd('/');
            }
        }

        public string ApiUrl => 
            _configuration["ApiUrl"] ?? 
            Environment.GetEnvironmentVariable("API_URL") ?? 
            $"{BaseUrl}/api";

        #endregion

        #region Timeout Settings

        /// <summary>
        /// Default timeout for operations in milliseconds
        /// Environment Variable: TIMEOUT
        /// Default: 30000 (30 seconds)
        /// </summary>
        public int DefaultTimeout => 
            int.TryParse(_configuration["DefaultTimeout"] ?? Environment.GetEnvironmentVariable("TIMEOUT"), out var result) 
                ? result 
                : 30000;

        /// <summary>
        /// Navigation timeout in milliseconds
        /// Environment Variable: NAVIGATION_TIMEOUT
        /// Default: 30000 (30 seconds)
        /// </summary>
        public int NavigationTimeout => 
            int.TryParse(_configuration["NavigationTimeout"] ?? Environment.GetEnvironmentVariable("NAVIGATION_TIMEOUT"), out var result) 
                ? result 
                : 30000;

        #endregion

        #region Screenshot Settings

        /// <summary>
        /// Take screenshots on test failure
        /// Environment Variable: SCREENSHOT_ON_FAILURE=0 to disable
        /// Default: true
        /// </summary>
        public bool TakeScreenshotOnFailure => 
            !(_configuration.GetValue<bool>("DisableScreenshotOnFailure") || 
              Environment.GetEnvironmentVariable("SCREENSHOT_ON_FAILURE") == "0");

        /// <summary>
        /// Screenshot directory path
        /// Environment Variable: SCREENSHOT_DIR
        /// Default: Screenshots
        /// </summary>
        public string ScreenshotDirectory => 
            _configuration["ScreenshotDirectory"] ?? 
            Environment.GetEnvironmentVariable("SCREENSHOT_DIR") ?? 
            "Screenshots";

        #endregion

        #region Test User Credentials

        /// <summary>
        /// Test user email
        /// Environment Variable: TEST_USER_EMAIL
        /// Default: testuser@example.com
        /// </summary>
        public string TestUserEmail => 
            _configuration["TestUser:Email"] ?? 
            Environment.GetEnvironmentVariable("TEST_USER_EMAIL") ?? 
            "testuser@example.com";

        /// <summary>
        /// Test user password
        /// Environment Variable: TEST_USER_PASSWORD
        /// Default: Password123!
        /// </summary>
        public string TestUserPassword => 
            _configuration["TestUser:Password"] ?? 
            Environment.GetEnvironmentVariable("TEST_USER_PASSWORD") ?? 
            "Password123!";

        /// <summary>
        /// Test user username
        /// Environment Variable: TEST_USER_USERNAME
        /// Default: TestUser
        /// </summary>
        public string TestUserUsername => 
            _configuration["TestUser:Username"] ?? 
            Environment.GetEnvironmentVariable("TEST_USER_USERNAME") ?? 
            "TestUser";

        /// <summary>
        /// Admin user email
        /// Environment Variable: ADMIN_USER_EMAIL
        /// Default: admin@example.com
        /// </summary>
        public string AdminUserEmail => 
            _configuration["AdminUser:Email"] ?? 
            Environment.GetEnvironmentVariable("ADMIN_USER_EMAIL") ?? 
            "admin@example.com";

        /// <summary>
        /// Admin user password
        /// Environment Variable: ADMIN_USER_PASSWORD
        /// Default: AdminPassword123!
        /// </summary>
        public string AdminUserPassword => 
            _configuration["AdminUser:Password"] ?? 
            Environment.GetEnvironmentVariable("ADMIN_USER_PASSWORD") ?? 
            "AdminPassword123!";

        #endregion

        #region Debugging Settings

        /// <summary>
        /// Enable debug mode
        /// Environment Variable: PWDEBUG=1
        /// Default: false
        /// </summary>
        public bool DebugMode => 
            Environment.GetEnvironmentVariable("PWDEBUG") == "1";

        /// <summary>
        /// Enable trace recording
        /// Environment Variable: TRACE=1
        /// Default: false
        /// </summary>
        public bool RecordTrace => 
            _configuration.GetValue<bool>("RecordTrace") || 
            Environment.GetEnvironmentVariable("TRACE") == "1";

        /// <summary>
        /// Trace directory path
        /// Environment Variable: TRACE_DIR
        /// Default: Traces
        /// </summary>
        public string TraceDirectory => 
            _configuration["TraceDirectory"] ?? 
            Environment.GetEnvironmentVariable("TRACE_DIR") ?? 
            "Traces";

        #endregion

        #region Helper Methods

        /// <summary>
        /// Get a custom configuration value
        /// </summary>
        public string? GetValue(string key)
        {
            return _configuration[key] ?? Environment.GetEnvironmentVariable(key);
        }

        /// <summary>
        /// Get a custom configuration value with type conversion
        /// </summary>
        public T? GetValue<T>(string key, T? defaultValue = default)
        {
            return _configuration.GetValue<T>(key, defaultValue);
        }

        #endregion
    }
}
