import { login } from '../../support'

context('Staff complaint editing', () => {
  beforeEach(() => {
    login(Cypress.env('staff-user'), Cypress.env('staff-pass'))
  })

  if (Cypress.env('writesEnabled'))
    it('can add and edit complaint', () => {
      const complaintText = 'Test complaint entry'
      const complaintActionText = 'Test complaint action entry'

      // create complaint
      cy.visit('Complaints/Create')
      cy.contains('button', 'Complaint').click()
      cy.get('#PrimaryConcernId').selectNth(1)
      cy.contains('button', 'Assignment').click()

      cy.get('#CurrentOwnerId').selectContaining(Cypress.env('staff-name'))

      cy.contains('button', 'Save Complaint').click()
      cy.get('h1').should('contain', 'Complaint ID')

      // edit complaint
      cy.contains('Edit details').click()
      cy.contains('button', 'Complaint').click()
      cy.get('#ComplaintNature').type(complaintText)
      cy.contains('button', 'Save Complaint').click()
      cy.get('.usa-alert-success').should(
        'contain.text',
        'The Complaint was updated.'
      )
      cy.contains(complaintText)

      // add action
      cy.contains('Add/edit actions').click()
      cy.get('#ActionTypeId')
        .selectNth(1)
        .get('#Comments')
        .type(complaintActionText)
      cy.contains('button', 'Add Action').click()
      cy.contains(complaintActionText)
      cy.contains('Back to Complaint Details').click()

      // attach file
      const fileName = 'ga-gov.png'
      const fileType = 'image/png'
      const fileInput = 'input[type=file]'
      cy.upload_file(fileName, fileType, fileInput)
      cy.get('form.gaepd-form-fileupload').submit()
      // cy.contains("Upload selected files").click()
      cy.get('tr').contains(fileName)

      // delete file
      cy.get('tr')
        .contains('tr', fileName)
        .contains('Delete')
        .click()
      cy.get('h1').should('contain', 'Delete Attachment from Complaint ID')
      cy.contains('button', 'Delete Attachment').click()
      cy.get('tr')
        .contains(fileName)
        .should('not.exist')
    })
})
