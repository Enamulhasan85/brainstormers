// ***********************************************************
// This example support/e2e.js is processed and
// loaded automatically before your test files.
//
// This is a great place to put global configuration and
// behavior that modifies Cypress.
//
// You can change the location of this file or turn off
// automatically serving support files with the
// 'supportFile' configuration option.
//
// You can read more here:
// https://on.cypress.io/configuration
// ***********************************************************

// Import commands.js using ES2015 syntax:
import './commands'

// Alternatively you can use CommonJS syntax:
// require('./commands')

// -- Global Hooks --

// Run before each test
beforeEach(() => {
  // Clear cookies and local storage before each test
  // cy.clearCookies()
  // cy.clearLocalStorage()
  
  // Handle uncaught exceptions (ASP.NET Core sometimes throws benign errors)
  cy.on('uncaught:exception', (err, runnable) => {
    // Returning false here prevents Cypress from failing the test
    // You can add specific error messages to ignore if needed
    
    // Example: Ignore specific errors
    if (err.message.includes('ResizeObserver loop')) {
      return false
    }
    
    // Let other errors fail the test
    return true
  })
})

// Run after each test
afterEach(() => {
  // Take screenshot on failure (already done by Cypress, but you can customize)
  // Log test completion
})

// -- Global Configuration --

// Set default timeout for commands
Cypress.config('defaultCommandTimeout', 10000)

// Set viewport size
Cypress.config('viewportWidth', 1280)
Cypress.config('viewportHeight', 720)

// -- Custom Event Listeners --

// Log all console errors from the application
Cypress.on('window:before:load', (win) => {
  cy.spy(win.console, 'error')
  cy.spy(win.console, 'warn')
})

// -- Network Stubbing & Interception --

// Example: Intercept all API calls for logging
// Cypress.on('window:before:load', (win) => {
//   cy.intercept('**', (req) => {
//     console.log(`Request: ${req.method} ${req.url}`)
//   })
// })
