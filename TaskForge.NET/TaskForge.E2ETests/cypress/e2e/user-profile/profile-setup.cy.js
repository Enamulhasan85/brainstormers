/**
 * User Profile Tests - Profile Setup
 * 
 * Tests user profile setup and editing functionality
 */

describe('User Profile - Setup', () => {
  beforeEach(() => {
    cy.loginAsTestUser()
    cy.visit('/UserProfile/Setup')
  })

  it('should display the profile setup form', () => {
    cy.contains(/profile|setup/i).should('be.visible')
    cy.get('input[name="FullName"]').should('be.visible')
    cy.get('input[name="PhoneNumber"]').should('be.visible')
    cy.get('textarea[name="ProfessionalSummary"]').should('be.visible')
  })

  it('should successfully setup user profile', () => {
    cy.fixture('user-profiles').then((profiles) => {
      const profile = profiles.sampleProfiles[0]

      cy.get('input[name="FullName"]').clear().type(profile.fullName)
      cy.get('input[name="PhoneNumber"]').clear().type(profile.phoneNumber)
      cy.get('textarea[name="ProfessionalSummary"]').clear().type(profile.professionalSummary)
      cy.get('input[name="Location"]').clear().type(profile.location)
      cy.get('input[name="JobTitle"]').clear().type(profile.jobTitle)
      cy.get('input[name="Company"]').clear().type(profile.company)
      cy.get('input[name="LinkedInProfile"]').clear().type(profile.linkedInProfile)
      cy.get('input[name="WebsiteUrl"]').clear().type(profile.websiteUrl)

      cy.get('button[type="submit"]').click()

      // Should show success message or redirect
      cy.url().should('not.include', '/UserProfile/Setup')
    })
  })

  it('should validate phone number format', () => {
    cy.get('input[name="PhoneNumber"]').clear().type('invalid-phone')
    cy.get('button[type="submit"]').click()

    // Should show validation error
    cy.contains(/phone.*invalid|valid.*phone/i).should('be.visible')
  })

  it('should validate URL format for LinkedIn', () => {
    cy.get('input[name="LinkedInProfile"]').clear().type('not-a-url')
    cy.get('button[type="submit"]').click()

    // Should show validation error
    cy.contains(/url.*invalid|valid.*url/i).should('be.visible')
  })

  it('should validate URL format for website', () => {
    cy.get('input[name="WebsiteUrl"]').clear().type('not-a-url')
    cy.get('button[type="submit"]').click()

    // Should show validation error
    cy.contains(/url.*invalid|valid.*url/i).should('be.visible')
  })

  it('should allow optional fields to be empty', () => {
    cy.get('input[name="FullName"]').clear().type('Test User')
    
    // Leave optional fields empty
    cy.get('input[name="LinkedInProfile"]').clear()
    cy.get('input[name="WebsiteUrl"]').clear()
    cy.get('input[name="Company"]').clear()

    cy.get('button[type="submit"]').click()

    // Should still save successfully
    cy.url().should('not.include', '/UserProfile/Setup')
  })
})
