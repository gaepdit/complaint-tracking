import { login } from '../../support'

context('Staff complaint access', () => {
  beforeEach(() => {
    login(Cypress.env('staff-user'), Cypress.env('staff-pass'))
  })

  it('can open complaint from View Complaint ID form', () => {
    cy.visit('/')
    cy.get('#FindComplaint').type(Cypress.env('publicComplaintId') + '{enter}')
    cy.url().should(
      'eq',
      Cypress.config().baseUrl +
        '/Complaints/Details/' +
        Cypress.env('publicComplaintId')
    )
    cy.get('h1').should(
      'contain',
      'Complaint ID ' + Cypress.env('publicComplaintId')
    )
    cy.get('h1').should('not.contain', 'Public Copy')
    cy.get('h2').eq(0).should('contain', 'Status: Approved/Closed')
  })

  it('can access public complaint and open public copy', () => {
    cy.visit('Complaints/Details/' + Cypress.env('publicComplaintId'))
    cy.get('h1').should(
      'contain',
      'Complaint ID ' + Cypress.env('publicComplaintId')
    )
    cy.get('h1').should('not.contain', 'Public Copy')
    cy.get('h2').eq(0).should('contain', 'Status: Approved/Closed')

    cy.contains('View public page').click()
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
    cy.get('h2').eq(0).should('contain', 'Status: Approved/Closed')
  })

  it('can access non-public complaint and open public details', () => {
    // find open complaint
    cy.visit('Complaints')
    cy.get('#ComplaintStatus')
      .select('Under Investigation')
      .get('#submit')
      .click()
    cy.get('tbody a').eq(0).click()

    cy.url().should(
      'contain',
      Cypress.config().baseUrl + '/Complaints/Details/'
    )
    cy.get('h1').should('contain', 'Complaint ID')
    cy.get('h1').should('not.contain', 'Public Copy')
    cy.get('h2').eq(0).should('contain', 'Status: Under Investigation')

    cy.contains('View public details').click()
    cy.url().should(
      'contain',
      Cypress.config().baseUrl + '/Complaints/PublicDetails/'
    )
    cy.get('h1').should('contain', 'Public Details')
    cy.get('h2').eq(0).should('contain', 'Status: Under Investigation')
  })

  it('can not access deleted complaint', () => {
    cy.request({
      url: 'Complaints/Details/' + Cypress.env('deletedComplaintId'),
      failOnStatusCode: false,
    }).should((resp) => {
      expect(resp.status).to.eq(404)
    })
  })
})
