// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************

// -- Custom Commands for TaskForge.NET --

/**
 * Login command - Authenticates user and maintains session
 * @example cy.login('user@example.com', 'Password123!')
 */
Cypress.Commands.add('login', (email, password) => {
  cy.session(
    [email, password],
    () => {
      cy.visit('/Identity/Account/Login')
      cy.get('input[name="Input.Email"]').type(email)
      cy.get('input[name="Input.Password"]').type(password)
      cy.get('button[type="submit"]').click()
      
      // Wait for successful login
      cy.url().should('not.include', '/Identity/Account/Login')
      cy.url().should('include', '/Home')
    },
    {
      validate() {
        // Validate session is still valid
        cy.getCookie('.AspNetCore.Identity.Application').should('exist')
      },
    }
  )
})

/**
 * Login as default test user
 * @example cy.loginAsTestUser()
 */
Cypress.Commands.add('loginAsTestUser', () => {
  const email = Cypress.env('testUserEmail')
  const password = Cypress.env('testUserPassword')
  cy.login(email, password)
})

/**
 * Login as admin user
 * @example cy.loginAsAdmin()
 */
Cypress.Commands.add('loginAsAdmin', () => {
  const email = Cypress.env('adminEmail')
  const password = Cypress.env('adminPassword')
  cy.login(email, password)
})

/**
 * Logout command
 * @example cy.logout()
 */
Cypress.Commands.add('logout', () => {
  cy.visit('/Identity/Account/Logout')
  cy.get('button[type="submit"]').click()
  cy.url().should('include', '/')
})

/**
 * Create a new project
 * @example cy.createProject('My Project', 'Description', 1)
 */
Cypress.Commands.add('createProject', (title, description, status = 1) => {
  cy.visit('/Project/Create')
  cy.get('input[name="Title"]').type(title)
  
  if (description) {
    cy.get('textarea[name="Description"]').type(description)
  }
  
  cy.get('select[name="Status"]').select(status.toString())
  cy.get('input[name="StartDate"]').type(new Date().toISOString().split('T')[0])
  cy.get('button[type="submit"]').click()
  
  // Wait for redirect to project list
  cy.url().should('include', '/Project/Index')
})

/**
 * Create a new task in a project
 * @example cy.createTask(1, { title: 'New Task', priority: 2 })
 */
Cypress.Commands.add('createTask', (projectId, taskData) => {
  cy.visit(`/Project/Dashboard/${projectId}`)
  
  // Open create task modal (adjust selector based on your UI)
  cy.contains('button', /add task/i).click()
  
  cy.get('input[name="Title"]').type(taskData.title)
  
  if (taskData.description) {
    cy.get('textarea[name="Description"]').type(taskData.description)
  }
  
  if (taskData.priority !== undefined) {
    cy.get('select[name="Priority"]').select(taskData.priority.toString())
  }
  
  if (taskData.status !== undefined) {
    cy.get('select[name="Status"]').select(taskData.status.toString())
  }
  
  cy.get('button[type="submit"]').click()
  
  // Wait for modal to close
  cy.get('.modal').should('not.be.visible')
})

/**
 * Setup user profile
 * @example cy.setupUserProfile({ fullName: 'John Doe', phoneNumber: '+1234567890' })
 */
Cypress.Commands.add('setupUserProfile', (profileData) => {
  cy.visit('/UserProfile/Setup')
  
  if (profileData.fullName) {
    cy.get('input[name="FullName"]').clear().type(profileData.fullName)
  }
  
  if (profileData.phoneNumber) {
    cy.get('input[name="PhoneNumber"]').clear().type(profileData.phoneNumber)
  }
  
  if (profileData.professionalSummary) {
    cy.get('textarea[name="ProfessionalSummary"]').clear().type(profileData.professionalSummary)
  }
  
  if (profileData.location) {
    cy.get('input[name="Location"]').clear().type(profileData.location)
  }
  
  if (profileData.jobTitle) {
    cy.get('input[name="JobTitle"]').clear().type(profileData.jobTitle)
  }
  
  if (profileData.company) {
    cy.get('input[name="Company"]').clear().type(profileData.company)
  }
  
  cy.get('button[type="submit"]').click()
})

/**
 * Get anti-forgery token for API requests
 * @example cy.getAntiForgeryToken().then(token => { ... })
 */
Cypress.Commands.add('getAntiForgeryToken', () => {
  return cy.get('input[name="__RequestVerificationToken"]').invoke('val')
})

/**
 * Seed database with test data
 * @example cy.seedDatabase()
 */
Cypress.Commands.add('seedDatabase', () => {
  // This would call a custom task to seed the database
  cy.task('log', 'Seeding database with test data')
  // Implement actual seeding logic via API or direct DB connection
})

/**
 * Clean database after tests
 * @example cy.cleanDatabase()
 */
Cypress.Commands.add('cleanDatabase', () => {
  cy.task('log', 'Cleaning database')
  // Implement cleanup logic
})

/**
 * Wait for page to finish loading
 * @example cy.waitForPageLoad()
 */
Cypress.Commands.add('waitForPageLoad', () => {
  cy.window().its('document.readyState').should('equal', 'complete')
})

/**
 * Check if element is in viewport
 * @example cy.get('.element').isInViewport()
 */
Cypress.Commands.add('isInViewport', { prevSubject: true }, (subject) => {
  const rect = subject[0].getBoundingClientRect()
  
  expect(rect.top).to.be.at.least(0)
  expect(rect.left).to.be.at.least(0)
  expect(rect.bottom).to.be.at.most(Cypress.config('viewportHeight'))
  expect(rect.right).to.be.at.most(Cypress.config('viewportWidth'))
  
  return subject
})

// -- Overwrite existing commands --

/**
 * Overwrite visit command to add default options
 */
Cypress.Commands.overwrite('visit', (originalFn, url, options) => {
  return originalFn(url, {
    ...options,
    failOnStatusCode: false,
  })
})

/**
 * Overwrite type command to add default delay
 */
Cypress.Commands.overwrite('type', (originalFn, subject, text, options) => {
  return originalFn(subject, text, {
    delay: 10,
    ...options,
  })
})
