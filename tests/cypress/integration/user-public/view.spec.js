context('Public view', () => {
  it('can access public page', () => {
    cy.visit('Public')
    cy.url().should('eq', Cypress.config().baseUrl + '/Public')
    cy.get('h1').should('contain', 'Public Complaint Search')
  })

  it('can open complaint from View Complaint ID form', () => {
    cy.visit('Public')
    cy.get('input[type=search]').type(
      Cypress.env('publicComplaintId') + '{enter}'
    )
    cy.url().should(
      'eq',
      Cypress.config().baseUrl +
        '/Public/ComplaintDetails/' +
        Cypress.env('publicComplaintId')
    )
    cy.get('h1').should(
      'contain',
      'Complaint ID ' + Cypress.env('publicComplaintId') + ' — Public Copy'
    )
    cy.get('h2')
      .eq(0)
      .should('contain', 'Status: Approved/Closed')
  })

  it('can access public complaint', () => {
    cy.visit('Public/ComplaintDetails/' + Cypress.env('publicComplaintId'))
    cy.url().should(
      'eq',
      Cypress.config().baseUrl +
        '/Public/ComplaintDetails/' +
        Cypress.env('publicComplaintId')
    )
    cy.get('h1').should(
      'contain',
      'Complaint ID ' + Cypress.env('publicComplaintId') + ' — Public Copy'
    )
    cy.get('h2')
      .eq(0)
      .should('contain', 'Status: Approved/Closed')
  })

  it('can access public complaint from non-public URL', () => {
    cy.visit('Complaints/Details/'+Cypress.env("publicComplaintId"))
    cy.url().should(
      'eq',
      Cypress.config().baseUrl + '/Public/ComplaintDetails/'+Cypress.env("publicComplaintId")
    )
    cy.get('h1').should('contain', 'Complaint ID '+Cypress.env("publicComplaintId")+' — Public Copy')
  })

  it('can not access deleted complaint', () => {
    cy.visit('Complaints/Details/' + Cypress.env('deletedComplaintId'))
    cy.url().should(
      'eq',
      Cypress.config().baseUrl +
        '/Public/ComplaintDetails/' +
        Cypress.env('deletedComplaintId')
    )
    cy.get('h1').should(
      'contain',
      'Complaint ID ' + Cypress.env('deletedComplaintId')
    )
    cy.get('h1').should('not.contain', 'Public Copy')
    cy.get('h2').should('not.exist')
  })

  it('redirects to login page on attempt to access non-public page', () => {
    cy.visit('Complaints')
    cy.url().should(
      'eq',
      Cypress.config().baseUrl + '/Account/Login/?ReturnUrl=%2FComplaints'
    )
  })
})
