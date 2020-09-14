context('Public search form', () => {
  it('can use default search form', () => {
    cy.visit('Public')
    cy.get('#submit').should('contain', 'Search').click()
    cy.url().should(
      'eq',
      Cypress.config().baseUrl + '/Public?submit=search#search-results'
    )
    cy.get('tbody').should('exist')
    cy.get('tbody a').eq(0).click()
    cy.url().should(
      'contain',
      Cypress.config().baseUrl + '/Public/ComplaintDetails/'
    )
  })

  it('can search with options', () => {
    cy.visit('Public')
    cy.get('#TypeId').select('Sewage spill')
    cy.get('#CountyId').select('Fulton').should('exist')
    cy.get('#StateId').select('Georgia').should('exist')

    cy.get('#submit').should('contain', 'Search').click()
    cy.url().should('contain', 'submit=search#search-results')

    cy.get('tbody').should('exist')
    cy.get('tbody a').eq(0).click()
    cy.url().should(
      'contain',
      Cypress.config().baseUrl + '/Public/ComplaintDetails/'
    )
  })
})
