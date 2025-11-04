/**
 * Project Management Tests - Project List
 * 
 * Tests project listing, filtering, and search functionality
 */

describe('Project Management - Project List', () => {
  beforeEach(() => {
    cy.loginAsTestUser()
    cy.visit('/Project/Index')
  })

  it('should display the project list page', () => {
    cy.contains(/project/i).should('be.visible')
    cy.url().should('include', '/Project/Index')
  })

  it('should display "Create New Project" button', () => {
    cy.contains(/create.*project/i).should('be.visible')
  })

  it('should navigate to create project page', () => {
    cy.contains(/create.*project/i).click()
    cy.url().should('include', '/Project/Create')
  })

  it('should display projects in card or list format', () => {
    // Check if any projects are displayed
    // This test assumes at least one project exists
    cy.get('body').then($body => {
      if ($body.text().includes('No projects')) {
        cy.log('No projects available')
      } else {
        cy.contains(/project/i).should('exist')
      }
    })
  })

  it('should filter projects by status', () => {
    // Test filtering by "In Progress"
    cy.get('select[name="filter.Status"]').select('1') // In Progress
    cy.get('button[type="submit"]').click()
    
    cy.url().should('include', 'Status=1')
  })

  it('should search projects', () => {
    // Test search functionality
    cy.get('input[name="filter.SearchTerm"]').type('Test Project')
    cy.get('button[type="submit"]').click()
    
    cy.url().should('include', 'SearchTerm')
  })

  it('should navigate to project dashboard', () => {
    // Click on first project (if exists)
    cy.get('body').then($body => {
      if (!$body.text().includes('No projects')) {
        cy.contains(/view.*details|dashboard/i).first().click()
        cy.url().should('include', '/Project/Dashboard/')
      } else {
        cy.log('No projects to view')
      }
    })
  })

  it('should display pagination controls if many projects', () => {
    // Check for pagination
    cy.get('body').then($body => {
      if ($body.text().includes('Previous') || $body.text().includes('Next')) {
        cy.contains(/next|previous/i).should('be.visible')
      } else {
        cy.log('Pagination not needed')
      }
    })
  })
})
