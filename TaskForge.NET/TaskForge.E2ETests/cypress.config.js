require('dotenv').config()
const { defineConfig } = require('cypress')

module.exports = defineConfig({
  projectId: 'shp1im',
  e2e: {
    // Base URL for your TaskForge.NET application
    baseUrl: process.env.BASE_URL || 'http://localhost:5283',
    
    // Viewport configuration
    viewportWidth: parseInt(process.env.VIEWPORT_WIDTH) || 1280,
    viewportHeight: parseInt(process.env.VIEWPORT_HEIGHT) || 720,
    
    // Video and screenshot settings
    video: process.env.ENABLE_VIDEO_RECORDING !== 'false',
    videoCompression: 32,
    screenshotOnRunFailure: process.env.ENABLE_SCREENSHOTS !== 'false',
    
    // Test execution settings
    defaultCommandTimeout: parseInt(process.env.DEFAULT_COMMAND_TIMEOUT) || 10000,
    requestTimeout: parseInt(process.env.REQUEST_TIMEOUT) || 10000,
    responseTimeout: parseInt(process.env.RESPONSE_TIMEOUT) || 10000,
    pageLoadTimeout: parseInt(process.env.PAGE_LOAD_TIMEOUT) || 60000,
    
    // Retry settings for flaky tests
    retries: {
      runMode: parseInt(process.env.RETRY_RUN_MODE) || 2,
      openMode: parseInt(process.env.RETRY_OPEN_MODE) || 0,
    },
    
    // Test file patterns
    specPattern: 'cypress/e2e/**/*.cy.{js,jsx,ts,tsx}',
    
    // Ignore test files in node_modules
    excludeSpecPattern: ['**/node_modules/**', '**/examples/**'],
    
    setupNodeEvents(on, config) {
      // implement node event listeners here
      
      // Example: Task to seed database
      on('task', {
        log(message) {
          console.log(message)
          return null
        },
        
        // Add custom tasks here
        // Example: database seeding, clearing data, etc.
      })
      
      return config
    },
    
    // Environment variables
    env: {
      // Default test user credentials
      testUserEmail: process.env.TEST_USER_EMAIL || 'testuser@example.com',
      testUserPassword: process.env.TEST_USER_PASSWORD || 'Password123!',
      
      // Admin credentials
      adminEmail: process.env.ADMIN_EMAIL || 'admin@taskforge.com',
      adminPassword: process.env.ADMIN_PASSWORD || 'Admin123!',
      
      // Operator credentials
      operatorEmail: process.env.OPERATOR_EMAIL || 'operator@taskforge.com',
      operatorPassword: process.env.OPERATOR_PASSWORD || 'Operator123!',
      
      // API endpoints
      apiUrl: process.env.API_URL || 'http://localhost:5283',
      
      // Feature flags
      enableEmailTests: process.env.ENABLE_EMAIL_TESTS === 'true',
      enableVisualTests: process.env.ENABLE_VISUAL_TESTS === 'true',
    },
  },
  
  // Component testing configuration (optional)
  component: {
    devServer: {
      framework: 'react',
      bundler: 'vite',
    },
  },
})
