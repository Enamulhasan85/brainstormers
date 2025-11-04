# Environment Variables Configuration

This document explains how to configure environment variables for the Cypress E2E tests.

## Quick Setup

1. Copy the example environment file:
   ```powershell
   cp .env.example .env
   ```

2. Update the `.env` file with your actual values

3. Install dependencies (includes dotenv):
   ```powershell
   npm install
   ```

4. Run tests (environment variables will be loaded automatically):
   ```powershell
   npm run cy:open
   ```

## Environment Variables Reference

### Application URLs

| Variable | Description | Default | Example |
|----------|-------------|---------|---------|
| `BASE_URL` | Base URL of the application | `http://localhost:5283` | `https://app.example.com` |
| `API_URL` | API endpoint URL | `http://localhost:5283` | `https://api.example.com` |

### User Credentials

| Variable | Description | Default |
|----------|-------------|---------|
| `TEST_USER_EMAIL` | Test user email | `testuser@example.com` |
| `TEST_USER_PASSWORD` | Test user password | `Password123!` |
| `ADMIN_EMAIL` | Admin user email | `admin@taskforge.com` |
| `ADMIN_PASSWORD` | Admin user password | `Admin123!` |
| `OPERATOR_EMAIL` | Operator user email | `operator@taskforge.com` |
| `OPERATOR_PASSWORD` | Operator user password | `Operator123!` |

### Test Configuration

| Variable | Description | Default | Notes |
|----------|-------------|---------|-------|
| `VIEWPORT_WIDTH` | Browser viewport width | `1280` | In pixels |
| `VIEWPORT_HEIGHT` | Browser viewport height | `720` | In pixels |
| `DEFAULT_COMMAND_TIMEOUT` | Default command timeout | `10000` | In milliseconds |
| `REQUEST_TIMEOUT` | HTTP request timeout | `10000` | In milliseconds |
| `RESPONSE_TIMEOUT` | HTTP response timeout | `10000` | In milliseconds |
| `PAGE_LOAD_TIMEOUT` | Page load timeout | `60000` | In milliseconds |

### Feature Flags

| Variable | Description | Default | Values |
|----------|-------------|---------|--------|
| `ENABLE_EMAIL_TESTS` | Enable email testing | `false` | `true` or `false` |
| `ENABLE_VISUAL_TESTS` | Enable visual regression testing | `false` | `true` or `false` |
| `ENABLE_VIDEO_RECORDING` | Record videos of test runs | `true` | `true` or `false` |
| `ENABLE_SCREENSHOTS` | Take screenshots on failure | `true` | `true` or `false` |

### Retry Configuration

| Variable | Description | Default | Notes |
|----------|-------------|---------|-------|
| `RETRY_RUN_MODE` | Number of retries in headless mode | `2` | 0-3 recommended |
| `RETRY_OPEN_MODE` | Number of retries in interactive mode | `0` | Usually 0 for debugging |

## Usage Examples

### Example 1: Local Development
```env
BASE_URL=http://localhost:5283
TEST_USER_EMAIL=dev@localhost.com
TEST_USER_PASSWORD=DevPass123!
```

### Example 2: Staging Environment
```env
BASE_URL=https://staging.taskforge.com
API_URL=https://staging.taskforge.com/api
TEST_USER_EMAIL=staging_test@example.com
TEST_USER_PASSWORD=StagingPass123!
ENABLE_VIDEO_RECORDING=false
```

### Example 3: CI/CD Pipeline
```env
BASE_URL=https://ci.taskforge.com
TEST_USER_EMAIL=ci_test@example.com
TEST_USER_PASSWORD=CIPass123!
RETRY_RUN_MODE=3
ENABLE_VIDEO_RECORDING=true
ENABLE_SCREENSHOTS=true
```

## Overriding Environment Variables

### Method 1: Command Line (Temporary Override)
```powershell
# Windows PowerShell
$env:BASE_URL="http://localhost:8080"; npm run cy:run

# Linux/Mac
BASE_URL=http://localhost:8080 npm run cy:run
```

### Method 2: Cypress CLI
```powershell
npx cypress run --env testUserEmail=custom@email.com,testUserPassword=CustomPass123!
```

### Method 3: cypress.config.js Override
You can also override specific values directly in `cypress.config.js` if needed.

## Security Best Practices

1. **Never commit `.env` file** to version control
   - ✅ `.env` is already in `.gitignore`
   - ✅ Use `.env.example` for documentation

2. **Use strong passwords** for test accounts
   - Minimum 8 characters
   - Mix of letters, numbers, and symbols

3. **Rotate credentials** regularly
   - Update test user passwords periodically
   - Use different credentials per environment

4. **Restrict test user permissions**
   - Test users should have minimal required permissions
   - Don't use production credentials for testing

5. **Use environment-specific credentials**
   - Different `.env` files for dev, staging, production testing
   - Never use production credentials in automated tests

## CI/CD Integration

### GitHub Actions
```yaml
- name: Run Cypress tests
  env:
    BASE_URL: ${{ secrets.BASE_URL }}
    TEST_USER_EMAIL: ${{ secrets.TEST_USER_EMAIL }}
    TEST_USER_PASSWORD: ${{ secrets.TEST_USER_PASSWORD }}
  run: npm run cy:run
```

### Azure DevOps
```yaml
- script: npm run cy:run
  env:
    BASE_URL: $(BASE_URL)
    TEST_USER_EMAIL: $(TEST_USER_EMAIL)
    TEST_USER_PASSWORD: $(TEST_USER_PASSWORD)
  displayName: 'Run Cypress Tests'
```

## Troubleshooting

### Problem: Environment variables not loading
**Solution**: 
1. Ensure `.env` file exists in the root of `TaskForge.E2ETests`
2. Verify `dotenv` package is installed: `npm install`
3. Check file encoding (should be UTF-8)

### Problem: Tests using wrong URL
**Solution**: 
1. Check `BASE_URL` in `.env` file
2. Verify no trailing slash in URL
3. Ensure application is running on specified port

### Problem: Authentication failures
**Solution**: 
1. Verify credentials in `.env` match actual user accounts
2. Ensure test users exist in the database
3. Check password complexity requirements

### Problem: Boolean values not working
**Solution**: 
- Use string values: `'true'` or `'false'` (with quotes)
- Not: `true` or `false` (without quotes)

## Default Values

If an environment variable is not set, the following defaults will be used:
- Base URL: `http://localhost:5283`
- Viewport: 1280x720
- Timeouts: 10 seconds (except page load: 60 seconds)
- Retries: 2 in headless, 0 in interactive
- Video: Enabled
- Screenshots: Enabled

## Accessing Environment Variables in Tests

Inside your test files, access environment variables like this:

```javascript
describe('Example Test', () => {
  it('should use environment variables', () => {
    const baseUrl = Cypress.config('baseUrl')
    const testUser = Cypress.env('testUserEmail')
    const apiUrl = Cypress.env('apiUrl')
    
    cy.log(`Testing against: ${baseUrl}`)
    cy.log(`Using test user: ${testUser}`)
  })
})
```

## Further Reading

- [Cypress Environment Variables Documentation](https://docs.cypress.io/guides/guides/environment-variables)
- [dotenv Package Documentation](https://www.npmjs.com/package/dotenv)
- [Cypress Configuration Documentation](https://docs.cypress.io/guides/references/configuration)
