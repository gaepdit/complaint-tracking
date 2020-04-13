import { login } from '../../support'

context('Staff reports', () => {
  beforeEach(() => {
    login(Cypress.env('staff-user'), Cypress.env('staff-pass'))
  })

  it('can list and search users', () => {
    cy.visit('Users')
    cy.get('h1').should('have.text', 'CTS Users')
    cy.get('form+table tbody tr').should('have.length.greaterThan', 0)

    cy.get('#Name').type(Cypress.env('admin-name'))
    cy.get('#submit').click()

    cy.get('form+table tbody tr')
      .should('have.length.greaterThan', 0)
      .eq(0)
      .contains('a', 'View')
      .click()
    cy.get('h1').should('contain', 'User Profile:')
  })
})
