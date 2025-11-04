/**
 * Project Management Tests - Create Project
 * 
 * Tests project creation functionality
 */

describe('Project Management - Create Project', () => {
  beforeEach(() => {
    cy.loginAsTestUser()
    cy.visit('/Project/Create')
  })

  it('should display the create project form', () => {
    cy.contains(/create.*project/i).should('be.visible')
    cy.get('input[name="Title"]').should('be.visible')
    cy.get('textarea[name="Description"]').should('be.visible')
    cy.get('select[name="Status"]').should('be.visible')
    cy.get('input[name="StartDate"]').should('be.visible')
    cy.get('button[type="submit"]').should('be.visible')
  })

  it('should successfully create a new project', () => {
    const timestamp = Date.now()
    const projectTitle = `Test Project ${timestamp}`
    const projectDescription = 'This is a test project created by Cypress'

    cy.get('input[name="Title"]').type(projectTitle)
    cy.get('textarea[name="Description"]').type(projectDescription)
    cy.get('select[name="Status"]').select('1') // In Progress
    cy.get('input[name="StartDate"]').type('2025-11-01')
    cy.get('button[type="submit"]').click()

    // Should redirect to project list
    cy.url().should('include', '/Project/Index')
    
    // Verify project appears in list
    cy.contains(projectTitle).should('be.visible')
  })

  it('should show validation error for empty title', () => {
    cy.get('textarea[name="Description"]').type('Description without title')
    cy.get('button[type="submit"]').click()

    // Should show required field validation
    cy.contains(/title.*required/i).should('be.visible')
  })

  it('should create project with minimal required fields', () => {
    const timestamp = Date.now()
    const projectTitle = `Minimal Project ${timestamp}`

    cy.get('input[name="Title"]').type(projectTitle)
    cy.get('button[type="submit"]').click()

    // Should successfully create project
    cy.url().should('include', '/Project/Index')
  })

  it('should select different project statuses', () => {
    cy.get('select[name="Status"]').select('0').should('have.value', '0') // Not Started
    cy.get('select[name="Status"]').select('1').should('have.value', '1') // In Progress
    cy.get('select[name="Status"]').select('2').should('have.value', '2') // On Hold
    cy.get('select[name="Status"]').select('3').should('have.value', '3') // Completed
    cy.get('select[name="Status"]').select('4').should('have.value', '4') // Cancelled
  })

  it('should set start date', () => {
    const today = new Date().toISOString().split('T')[0]
    
    cy.get('input[name="StartDate"]').type(today)
    cy.get('input[name="StartDate"]').should('have.value', today)
  })

  it('should set end date', () => {
    const futureDate = '2025-12-31'
    
    cy.get('input[name="EndDate"]').type(futureDate)
    cy.get('input[name="EndDate"]').should('have.value', futureDate)
  })

  it('should navigate back to project list', () => {
    cy.contains(/back to list|cancel/i).click()
    cy.url().should('include', '/Project/Index')
  })
})
