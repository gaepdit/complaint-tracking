import { login } from '../../support'

context('Staff account access', () => {
  beforeEach(() => {
    login(Cypress.env('staff-user'), Cypress.env('staff-pass'))
  })

  it('can access account', () => {
    cy.visit('Account')
    cy.get('h1').should('contain', 'Your Account')
    cy.contains(Cypress.env('staff-user'))
  })

  if (Cypress.env('writesEnabled'))
    it('can edit account', () => {
      const badEmail = 'new@example.org'

      cy.visit('Account')
      cy.contains('Edit').click()
      cy.get('#Email').clear().type(badEmail)
      cy.contains('Save').click()
      cy.get('.validation-summary-errors').should(
        'contain',
        'A valid DNR email address is required'
      )
      cy.get('#Email').clear().type(Cypress.env('staff-user'))
      cy.contains('Save').click()
      cy.get('.usa-alert-success').should(
        'contain.text',
        'Your profile was updated.'
      )
      cy.contains(Cypress.env('staff-user'))
    })

  it('can search for and access other user', () => {
    cy.visit('Users')
    cy.get('h1').should('contain', 'CTS Users')

    cy.get('#Name').type(Cypress.env('admin-name'))
    cy.get('#submit').click()

    cy.contains(Cypress.env('admin-user'))
      .parents('tr')
      .contains('View')
      .click()

    cy.contains(Cypress.env('admin-user'))
    cy.contains(Cypress.env('admin-name'))
  })

  it('is unable to edit other user', () => {
    cy.visit('Users')
    cy.get('h1').should('contain', 'CTS Users')

    cy.get('#Name').type(Cypress.env('admin-name'))
    cy.get('#submit').click()

    cy.contains(Cypress.env('admin-user'))
      .parents('tr')
      .contains('Edit')
      .should('not.exist')

    cy.contains(Cypress.env('admin-user'))
      .parents('tr')
      .contains('View')
      .click()
      .url()
      .then(($viewUrl) => {
        cy.visit($viewUrl.replace('Details', 'Edit'))
      })
      .contains('Access Denied')
      .url()
      .should('include', 'Account/AccessDenied')
  })
})
