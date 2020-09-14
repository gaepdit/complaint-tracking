import { login } from '../../support'

context('Admin account access', () => {
  beforeEach(() => {
    login(Cypress.env('admin-user'), Cypress.env('admin-pass'))
  })

  it('can access account', () => {
    cy.visit('Account')
    cy.get('h1').should('contain', 'Your Account')
    cy.contains(Cypress.env('admin-user'))
  })

  if (Cypress.env('writesEnabled'))
    it('can edit account', () => {
      const badEmail = 'new@example.org'

      cy.visit('Account')
      cy.contains('Edit').click()
      cy.get('#Email').clear().type(badEmail)
      cy.get('#OfficeId').select('Air Protection Branch')
      cy.contains('Save').click()
      cy.get('.validation-summary-errors').should(
        'contain',
        'A valid DNR email address is required'
      )
      cy.get('#Email').clear().type(Cypress.env('admin-user'))
      cy.contains('Save').click()
      cy.get('.usa-alert-success').should(
        'contain.text',
        'Your profile was updated.'
      )
      cy.contains(Cypress.env('admin-user'))
    })

  it('can access other user', () => {
    cy.visit('Users')
    cy.get('h1').should('contain', 'CTS Users')

    cy.get('#Name').type(Cypress.env('staff-name'))
    cy.get('#submit').click()

    cy.contains(Cypress.env('staff-user'))
  })

  if (Cypress.env('writesEnabled'))
    it('can edit other user', () => {
      const badEmail = 'new@example.org'
      const goodEmail = Cypress.env('staff-user')

      cy.visit('Users')

      cy.get('#Name').type(Cypress.env('staff-name'))
      cy.get('#submit').click()

      cy.contains(Cypress.env('staff-user'))
        .parents('tr')
        .contains('Edit')
        .click()

      cy.get('#LastName').should('have.value', Cypress.env('staff-name'))

      cy.get('#Email')
        .should('have.value', Cypress.env('staff-user'))
        .clear()
        .type(badEmail)
      cy.contains('Save').click()
      cy.get('.field-validation-error').should(
        'contain',
        'A valid DNR email address is required'
      )
      cy.get('.validation-summary-errors').should(
        'contain',
        'A valid DNR email address is required'
      )

      cy.get('#Email').clear().type(goodEmail)
      cy.contains('Save').click()
      cy.get('.usa-alert-success').should(
        'contain.text',
        'The user profile was updated.'
      )
      cy.contains(goodEmail)
    })
})
