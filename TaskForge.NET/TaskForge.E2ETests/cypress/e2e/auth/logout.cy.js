/**
 * Authentication Tests - Logout
 * 
 * Tests the user logout functionality
 */

describe('User Authentication - Logout', () => {
  beforeEach(() => {
    // Login before each test
    cy.loginAsTestUser()
    cy.visit('/Home/Index')
  })

  it('should successfully logout user', () => {
    // Navigate to logout
    cy.visit('/Identity/Account/Logout')
    
    // Confirm logout
    cy.get('button[type="submit"]').click()

    // Should be redirected to home/welcome page
    cy.url().should('not.include', '/Home/Index')
    
    // Authentication cookie should be cleared
    cy.getCookie('.AspNetCore.Identity.Application').should('not.exist')
  })

  it('should not access protected pages after logout', () => {
    cy.logout()

    // Try to access protected page
    cy.visit('/Project/Index')

    // Should be redirected to login page
    cy.url().should('include', '/Identity/Account/Login')
  })

  it('should clear user session after logout', () => {
    cy.logout()

    // Visit login page
    cy.visit('/Identity/Account/Login')

    // User should not be auto-logged in
    cy.url().should('include', '/Identity/Account/Login')
  })
})
