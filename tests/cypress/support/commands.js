// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
// Cypress.Commands.add("login", (email, password) => { ... })
//
//
// -- This is a child command --
// Cypress.Commands.add("drag", { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add("dismiss", { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This will overwrite an existing command --
// Cypress.Commands.overwrite("visit", (originalFn, url, options) => { ... })

// Select nth option in select element
// https://stackoverflow.com/a/55577084/212978
Cypress.Commands.add(
    'selectNth',
    { prevSubject: 'element' },
    (subject, pos) => {
      cy.wrap(subject)
        .children('option')
        .eq(pos)
        .then(e => cy.wrap(subject).select(e.val()))
    }
  )
  
  // Select option in select element with partial text match
  Cypress.Commands.add(
    'selectContaining',
    { prevSubject: 'element' },
    (subject, text, options) => {
      cy.wrap(subject)
        .contains('option', text, options)
        .then(e => cy.wrap(subject).select(e.val()))
    }
  )
  
  // Enable file uploads
  // https://github.com/javieraviles/cypress-upload-file-post-form
  // Example:
  // const fileName = 'your_file_name.xlsx';
  // const fileType = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';
  // const fileInput = 'input[type=file]';
  // cy.upload_file(fileName, fileType, fileInput);
  Cypress.Commands.add('upload_file', (fileName, fileType = ' ', selector) => {
    cy.get(selector).then(subject => {
      cy.fixture(fileName, 'base64')
        .then(Cypress.Blob.base64StringToBlob)
        .then(blob => {
          const el = subject[0]
          const testFile = new File([blob], fileName, { type: fileType })
          const dataTransfer = new DataTransfer()
          dataTransfer.items.add(testFile)
          el.files = dataTransfer.files
        })
    })
  })
  