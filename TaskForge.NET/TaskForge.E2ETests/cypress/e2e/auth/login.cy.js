/**
 * Authentication Tests - Login Functionality
 * 
 * Tests the user login flow including:
 * - Successful login with valid credentials
 * - Failed login with invalid credentials
 * - Form validation
 * - Session persistence
 */

describe('User Authentication - Login', () => {
  beforeEach(() => {
    // Visit login page before each test
    cy.visit('/Identity/Account/Login')
  })

  it('should display the login form', () => {
    cy.contains(/log in/i).should('be.visible')
    cy.get('input[name="Input.Email"]').should('be.visible')
    cy.get('input[name="Input.Password"]').should('be.visible')
    cy.get('button[type="submit"]').should('be.visible')
  })

  it('should successfully log in with valid credentials', () => {
    const email = Cypress.env('testUserEmail')
    const password = Cypress.env('testUserPassword')

    cy.get('input[name="Input.Email"]').type(email)
    cy.get('input[name="Input.Password"]').type(password)
    cy.get('button[type="submit"]').click()

    // Assert successful login - redirected to home page
    cy.url().should('not.include', '/Identity/Account/Login')
    cy.url().should('include', '/Home')
    
    // Verify user is authenticated
    cy.getCookie('.AspNetCore.Identity.Application').should('exist')
  })

  it('should show error with invalid credentials', () => {
    cy.get('input[name="Input.Email"]').type('invalid@example.com')
    cy.get('input[name="Input.Password"]').type('WrongPassword123!')
    cy.get('button[type="submit"]').click()

    // Should stay on login page and show error
    cy.url().should('include', '/Identity/Account/Login')
    cy.contains(/invalid login attempt/i).should('be.visible')
  })

  it('should show validation error for empty email', () => {
    cy.get('input[name="Input.Password"]').type('Password123!')
    cy.get('button[type="submit"]').click()

    // Should show required field validation
    cy.get('input[name="Input.Email"]:invalid').should('exist')
  })

  it('should show validation error for empty password', () => {
    cy.get('input[name="Input.Email"]').type('test@example.com')
    cy.get('button[type="submit"]').click()

    // Should show required field validation
    cy.get('input[name="Input.Password"]:invalid').should('exist')
  })

  it('should show validation error for invalid email format', () => {
    cy.get('input[name="Input.Email"]').type('notanemail')
    cy.get('input[name="Input.Password"]').type('Password123!')
    cy.get('button[type="submit"]').click()

    // Should show email format validation
    cy.get('input[name="Input.Email"]:invalid').should('exist')
  })

  it('should toggle "Remember me" checkbox', () => {
    cy.get('input[name="Input.RememberMe"]').check().should('be.checked')
    cy.get('input[name="Input.RememberMe"]').uncheck().should('not.be.checked')
  })

  it('should have a link to register page', () => {
    cy.contains(/register/i)
      .should('have.attr', 'href')
      .and('include', '/Identity/Account/Register')
  })

  it('should have a link to forgot password page', () => {
    cy.contains(/forgot.*password/i)
      .should('have.attr', 'href')
      .and('include', '/Identity/Account/ForgotPassword')
  })

  it('should persist session after page reload', () => {
    // Login
    cy.loginAsTestUser()
    cy.visit('/Home/Index')

    // Reload page
    cy.reload()

    // Should still be authenticated
    cy.url().should('include', '/Home')
    cy.getCookie('.AspNetCore.Identity.Application').should('exist')
  })

  it('should redirect to return URL after login', () => {
    const returnUrl = '/Project/Index'
    cy.visit(`/Identity/Account/Login?ReturnUrl=${encodeURIComponent(returnUrl)}`)

    cy.loginAsTestUser()

    // Should redirect to the return URL
    cy.url().should('include', returnUrl)
  })
})
