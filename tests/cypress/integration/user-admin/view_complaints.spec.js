import { login } from '../../support'

context('Admin complaint access', () => {
  beforeEach(() => {
    login(Cypress.env('admin-user'), Cypress.env('admin-pass'))
  })

  it('can access deleted complaint', () => {
    // find deleted complaint
    cy.visit('Complaints')
    cy.get('#DeleteStatus')
      .select('Deleted')
      .get('#submit')
      .click()
    cy.get('tbody a')
      .eq(0)
      .click()
    cy.get('h1').should('contain', 'Complaint ID')
    cy.url().should(
      'contain',
      Cypress.config().baseUrl + '/Complaints/Details/'
    )
    cy.get('h2')
      .eq(0)
      .should('contain', 'Complaint Has Been Deleted')
  })
})
