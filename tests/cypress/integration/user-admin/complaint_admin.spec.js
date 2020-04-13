import { login } from '../../support'

context('Admin complaint administration', () => {
  beforeEach(() => {
    login(Cypress.env('admin-user'), Cypress.env('admin-pass'))
  })

  if (Cypress.env('writesEnabled'))
    it('can reopen/assign/submit/close complaint', () => {
      // find closed complaint
      cy.visit('Complaints')
      cy.get('#ComplaintStatus')
        .select('Approved/Closed')
        .get('#submit')
        .click()
      cy.get('tbody a')
        .eq(0)
        .click()

      // reopen complaint
      cy.contains('Reopen').click()
      cy.get('h1').should('contain', 'Reopen Complaint ID')
      cy.get('#Comment').type('Test comment')
      cy.get('form')
        .contains('form', 'Reopen')
        .submit()
      cy.get('.usa-alert-success').should(
        'contain.text',
        'The Complaint has been reopened.'
      )

      // assign to staff
      cy.contains('a', 'Assign', { matchCase: false }).click()
      cy.get('h1').should('contain', 'Assignment for Complaint ID')
      cy.server()
      cy.route('GET', '/api/**').as('apiRequest')
      cy.get('#CurrentOfficeId').selectNth(1)
      cy.wait('@apiRequest')
      cy.get('#CurrentOwnerId').selectNth(1)
      cy.get('#Comment').type('Test comment')
      cy.get('form')
        .contains('form', 'Assign')
        .submit()
      cy.get('.usa-alert').should('contain.text', 'assign')

      // submit for review
      cy.contains('Submit for review').click()
      cy.get('h1').should('contain', 'Request Review for Complaint ID')
      cy.get('#ReviewById').selectNth(1)
      cy.get('#Comment').type('Test comment')
      cy.get('form')
        .contains('form', 'Request review')
        .submit()
      cy.get('.usa-alert-success').should(
        'contain.text',
        'The Complaint has been submitted for review.'
      )

      // close complaint
      cy.contains('Close Complaint').click()
      cy.get('h1').should('contain', 'Approve and Close Complaint ID')
      cy.get('#Comment').type('Test comment')
      cy.get('form')
        .contains('form', 'Approve')
        .submit()
      cy.get('.usa-alert-success').should(
        'contain.text',
        'The Complaint has been approved/closed.'
      )
    })
})
