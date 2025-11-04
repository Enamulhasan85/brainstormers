/**
 * Task Management Tests - Create Task
 * 
 * Tests task creation within a project
 */

describe('Task Management - Create Task', () => {
  let projectId

  before(() => {
    // Create a test project first
    cy.loginAsTestUser()
    
    const timestamp = Date.now()
    const projectTitle = `Task Test Project ${timestamp}`
    cy.createProject(projectTitle, 'Project for testing task creation', 1)
    
    // Extract project ID from URL (assuming redirect to project dashboard)
    cy.url().then(url => {
      // We'll navigate to the first project we find
      cy.visit('/Project/Index')
    })
  })

  beforeEach(() => {
    cy.loginAsTestUser()
    // Visit a project dashboard (you may need to adjust this)
    cy.visit('/Project/Index')
    cy.get('body').then($body => {
      if (!$body.text().includes('No projects')) {
        cy.contains(/view.*details|dashboard/i).first().click()
      }
    })
  })

  it('should display create task button', () => {
    cy.contains(/add.*task|create.*task/i).should('exist')
  })

  it('should open create task modal', () => {
    cy.contains(/add.*task|create.*task/i).click()
    
    // Modal should be visible
    cy.get('.modal').should('be.visible')
    cy.get('input[name="Title"]').should('be.visible')
  })

  it('should create a new task with basic information', () => {
    const timestamp = Date.now()
    const taskTitle = `Test Task ${timestamp}`
    
    cy.contains(/add.*task|create.*task/i).click()
    
    cy.get('input[name="Title"]').type(taskTitle)
    cy.get('textarea[name="Description"]').type('This is a test task')
    cy.get('select[name="Priority"]').select('2') // High priority
    cy.get('select[name="Status"]').select('0') // To Do
    
    cy.get('button[type="submit"]').click()
    
    // Modal should close
    cy.get('.modal').should('not.be.visible')
    
    // Task should appear in the board
    cy.contains(taskTitle).should('be.visible')
  })

  it('should show validation error for empty task title', () => {
    cy.contains(/add.*task|create.*task/i).click()
    
    cy.get('textarea[name="Description"]').type('Task without title')
    cy.get('button[type="submit"]').click()
    
    // Should show validation error
    cy.contains(/title.*required/i).should('be.visible')
  })

  it('should set task priority', () => {
    cy.contains(/add.*task|create.*task/i).click()
    
    cy.get('select[name="Priority"]').select('0') // Low
    cy.get('select[name="Priority"]').should('have.value', '0')
    
    cy.get('select[name="Priority"]').select('3') // Critical
    cy.get('select[name="Priority"]').should('have.value', '3')
  })

  it('should set task dates', () => {
    cy.contains(/add.*task|create.*task/i).click()
    
    const startDate = '2025-11-01T10:00'
    const dueDate = '2025-11-15T17:00'
    
    cy.get('input[name="StartDate"]').type(startDate)
    cy.get('input[name="DueDate"]').type(dueDate)
    
    cy.get('input[name="StartDate"]').should('have.value', startDate)
    cy.get('input[name="DueDate"]').should('have.value', dueDate)
  })

  it('should cancel task creation', () => {
    cy.contains(/add.*task|create.*task/i).click()
    
    cy.get('input[name="Title"]').type('Task to cancel')
    
    // Click cancel or close button
    cy.contains(/cancel|close/i).click()
    
    // Modal should close
    cy.get('.modal').should('not.be.visible')
    
    // Task should not appear
    cy.contains('Task to cancel').should('not.exist')
  })
})
