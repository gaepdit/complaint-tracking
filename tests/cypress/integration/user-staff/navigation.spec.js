import { login } from '../../support'

context('Staff navigation', () => {
  beforeEach(() => {
    login(Cypress.env('staff-user'), Cypress.env('staff-pass'))
  })

  it('shows dashboard if logged in', () => {
    cy.visit('/')
    cy.get('h1').should('contain', 'Dashboard')
  })
})
