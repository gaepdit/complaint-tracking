import { login } from '../../support'

context('Staff search', () => {
  beforeEach(() => {
    login(Cypress.env('staff-user'), Cypress.env('staff-pass'))
  })

  it('can use complaints search form', () => {
    cy.visit('Complaints')
    cy.get('#submit').should('contain', 'Search').click()
    cy.url().should(
      'eq',
      Cypress.config().baseUrl + '/Complaints?submit=search#search-results'
    )
    cy.get('tbody').should('exist')
    cy.get('tbody a').eq(0).click()
    cy.url().should(
      'contain',
      Cypress.config().baseUrl + '/Complaints/Details/'
    )
    cy.get('h2').eq(0).should('contain', 'Status:')
  })

  it('can use complaint actions search form', () => {
    cy.visit('ComplaintActions')
    cy.get('#submit').should('contain', 'Search').click()
    cy.url().should(
      'eq',
      Cypress.config().baseUrl +
        '/ComplaintActions?submit=search#search-results'
    )
    cy.get('tbody').should('exist')
  })

  it('can download search results', () => {
    cy.request(
      'Complaints?DateReceivedFrom=6-13-2017&DateReceivedTo=6-13-2017&export=csv'
    ).then((response) => {
      expect(response.status).to.eq(200)
      expect(response.headers['content-type']).to.eq('text/csv')
    })
  })
})
