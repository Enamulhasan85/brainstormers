/**
 * Authentication Tests - User Registration
 * 
 * Tests the user registration flow including:
 * - Successful registration
 * - Form validation
 * - Email confirmation requirement
 * - Duplicate user handling
 */

describe('User Authentication - Registration', () => {
  beforeEach(() => {
    cy.visit('/Identity/Account/Register')
  })

  it('should display the registration form', () => {
    cy.contains(/register/i).should('be.visible')
    cy.get('input[name="Input.Email"]').should('be.visible')
    cy.get('input[name="Input.Password"]').should('be.visible')
    cy.get('input[name="Input.ConfirmPassword"]').should('be.visible')
    cy.get('button[type="submit"]').should('be.visible')
  })

  it('should successfully register a new user', () => {
    const timestamp = Date.now()
    const email = `testuser${timestamp}@example.com`
    const password = 'Password123!'

    cy.get('input[name="Input.Email"]').type(email)
    cy.get('input[name="Input.Password"]').type(password)
    cy.get('input[name="Input.ConfirmPassword"]').type(password)
    cy.get('button[type="submit"]').click()

    // Should redirect to registration confirmation page
    cy.url().should('include', '/Identity/Account/RegisterConfirmation')
    cy.contains(/confirm.*email/i).should('be.visible')
  })

  it('should show validation error for invalid email format', () => {
    cy.get('input[name="Input.Email"]').type('notanemail')
    cy.get('input[name="Input.Password"]').type('Password123!')
    cy.get('input[name="Input.ConfirmPassword"]').type('Password123!')
    cy.get('button[type="submit"]').click()

    cy.get('input[name="Input.Email"]:invalid').should('exist')
  })

  it('should show validation error for weak password', () => {
    cy.get('input[name="Input.Email"]').type('test@example.com')
    cy.get('input[name="Input.Password"]').type('weak')
    cy.get('input[name="Input.ConfirmPassword"]').type('weak')
    cy.get('button[type="submit"]').click()

    // Should show password strength validation error
    cy.contains(/password.*must/i).should('be.visible')
  })

  it('should show validation error for password mismatch', () => {
    cy.get('input[name="Input.Email"]').type('test@example.com')
    cy.get('input[name="Input.Password"]').type('Password123!')
    cy.get('input[name="Input.ConfirmPassword"]').type('DifferentPassword123!')
    cy.get('button[type="submit"]').click()

    // Should show password mismatch error - exact message from the model
    cy.contains(/password and confirmation password do not match/i).should('be.visible')
  })

  it('should show error for duplicate email registration', () => {
    const email = Cypress.env('testUserEmail')

    cy.get('input[name="Input.Email"]').type(email)
    cy.get('input[name="Input.Password"]').type('Password123!')
    cy.get('input[name="Input.ConfirmPassword"]').type('Password123!')
    cy.get('button[type="submit"]').click()

    // Should show error that user already exists
    cy.contains(/already.*taken|user.*already.*exists/i).should('be.visible')
  })

  it('should show required field validation for empty fields', () => {
    cy.get('button[type="submit"]').click()

    // Check that validation messages appear for required fields
    // ASP.NET validation creates spans with text-danger class and validation messages
    cy.get('.text-danger').should('have.length.at.least', 2)
    
    // Or check for specific validation messages
    cy.contains(/email.*required|required/i).should('be.visible')
    cy.contains(/password.*required|required/i).should('be.visible')
  })

  it('should toggle password visibility', () => {
    // Password field should start as type="password"
    cy.get('input#password').should('have.attr', 'type', 'password')
    
    // Click the eye icon to toggle visibility
    cy.get('.toggle-password').first().click()
    cy.get('input#password').should('have.attr', 'type', 'text')
    
    // Click again to hide
    cy.get('.toggle-password').first().click()
    cy.get('input#password').should('have.attr', 'type', 'password')
  })
})
