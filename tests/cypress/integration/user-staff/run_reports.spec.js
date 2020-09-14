import { login } from '../../support'

context('Staff reports', () => {
  beforeEach(() => {
    login(Cypress.env('staff-user'), Cypress.env('staff-pass'))
  })

  it('can run a report', () => {
    cy.visit('Reports')
    cy.get('h1').eq(0).should('contain', 'Status Reports')

    cy.visit('Reports/DaysToClosureByOffice')
    cy.get('h1').should('contain', 'Report: Days To Closure By Office')
    cy.get('form+table tbody tr')
      .should('have.length.greaterThan', 0)
      .eq(0)
      .find('a')
      .click()
    cy.get('h1').should('contain', 'Report: Days To Closure By Staff')
  })
})
