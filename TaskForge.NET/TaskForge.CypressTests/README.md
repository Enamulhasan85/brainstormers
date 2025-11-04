# TaskForge.NET E2E Tests

End-to-end tests for TaskForge.NET using Cypress.

## Prerequisites

- Node.js (v14 or higher)
- TaskForge.NET application running locally
- PostgreSQL database configured

## Installation

1. **Copy environment variables file:**
   ```powershell
   cp .env.example .env
   ```

2. **Update `.env` file** with your configuration:
   - Set `BASE_URL` to your application URL
   - Update test user credentials
   - Adjust timeouts if needed

3. **Install dependencies:**
   ```powershell
   npm install
   ```

4. **Verify Cypress installation:**
   ```powershell
   npx cypress verify
   ```

## Configuration

### Environment Variables

The project uses environment variables for configuration. See [`ENV_VARIABLES.md`](ENV_VARIABLES.md) for detailed documentation.

**Quick setup:**
1. Copy `.env.example` to `.env`
2. Update values in `.env` file
3. Environment variables are automatically loaded when running tests

**Key variables:**
- `BASE_URL` - Application URL (default: `http://localhost:5283`)
- `TEST_USER_EMAIL` - Test user email
- `TEST_USER_PASSWORD` - Test user password
- `ADMIN_EMAIL` - Admin user email
- `ADMIN_PASSWORD` - Admin user password

**Example `.env` file:**
```env
BASE_URL=http://localhost:5283
TEST_USER_EMAIL=testuser@example.com
TEST_USER_PASSWORD=Password123!
```

### Command Line Override

You can also override environment variables via command line:

```powershell
# Windows PowerShell
$env:BASE_URL="http://localhost:8080"; npm run cy:run

# Or use Cypress CLI
npx cypress run --env testUserEmail=myuser@test.com,testUserPassword=MyPass123!
```

## Running Tests

### Interactive Mode (Cypress Test Runner)
```powershell
npm run cy:open
```

### Headless Mode (Command Line)
```powershell
npm run cy:run
```

### Run Specific Browser
```powershell
npm run cy:run:chrome
npm run cy:run:firefox
npm run cy:run:edge
```

### Run Specific Test Suite
```powershell
npm run test:auth       # Authentication tests only
npm run test:projects   # Project tests only
npm run test:tasks      # Task tests only
```

### Run Specific Test File
```powershell
npx cypress run --spec "cypress/e2e/auth/login.cy.js"
```

### Run with Headed Mode (See browser)
```powershell
npm run test:headed
```

## Test Structure

```
cypress/
├── e2e/                  # Test files
│   ├── auth/            # Authentication tests
│   ├── projects/        # Project management tests
│   ├── tasks/           # Task management tests
│   ├── user-profile/    # User profile tests
│   └── admin/           # Admin functionality tests
├── fixtures/             # Test data (JSON files)
├── support/              # Custom commands and utilities
│   ├── commands.js      # Custom Cypress commands
│   └── e2e.js          # Global configuration
└── downloads/           # Files downloaded during tests
```

## Custom Commands

Custom commands are defined in `cypress/support/commands.js`:

- `cy.login(email, password)` - Login helper
- `cy.createProject(title, description)` - Create project helper
- `cy.createTask(projectId, taskData)` - Create task helper
- `cy.seedDatabase()` - Database seeding helper

## Best Practices

1. **Use data attributes**: Add `data-cy` attributes to elements for reliable selectors
2. **Independent tests**: Each test should be independent and not rely on previous tests
3. **Clean state**: Use `beforeEach` to set up test data and `afterEach` to clean up
4. **Meaningful assertions**: Use descriptive assertion messages
5. **Page Objects**: Consider using Page Object pattern for complex pages

## Debugging

### Debug Specific Test
```powershell
npx cypress open --browser chrome
```

Then select the test file you want to debug.

### Time Travel
Cypress automatically takes snapshots. Hover over commands in the Command Log to see what happened at each step.

### Screenshots & Videos
- Screenshots are taken automatically on test failures
- Videos are recorded for all test runs in headless mode
- Location: `cypress/screenshots/` and `cypress/videos/`

## CI/CD Integration

### GitHub Actions Example
```yaml
name: Cypress E2E Tests

on: [push, pull_request]

jobs:
  cypress-run:
    runs-on: windows-latest
    steps:
      - name: Checkout
        uses: actions/checkout@v3
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'
      
      - name: Start Application
        run: |
          cd TaskForge.WebUI
          dotnet run &
        
      - name: Cypress run
        uses: cypress-io/github-action@v6
        with:
          working-directory: TaskForge.E2ETests
          wait-on: 'https://localhost:7001'
```

## Troubleshooting

### Application Not Running
Make sure TaskForge.NET is running before executing tests:
```powershell
cd ..\TaskForge.WebUI
dotnet run
```

### Port Conflicts
If the default port (7001) is in use, update `baseUrl` in `cypress.config.js`

### Certificate Errors
For HTTPS issues with self-signed certificates, Cypress might need additional configuration.

## Resources

- [Cypress Documentation](https://docs.cypress.io/)
- [Cypress Best Practices](https://docs.cypress.io/guides/references/best-practices)
- [TaskForge.NET Cypress Testing Guide](../CYPRESS_TESTING_GUIDE.md)
