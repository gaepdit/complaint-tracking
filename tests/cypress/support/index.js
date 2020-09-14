// ***********************************************************
// This example support/index.js is processed and
// loaded automatically before your test files.
//
// This is a great place to put global configuration and
// behavior that modifies Cypress.
//
// You can change the location of this file or turn off
// automatically serving support files with the
// 'supportFile' configuration option.
//
// You can read more here:
// https://on.cypress.io/configuration
// ***********************************************************

// Import commands.js using ES2015 syntax:
import './commands'

// Alternatively you can use CommonJS syntax:
// require('./commands')

// https://glebbahmutov.com/blog/keep-passwords-secret-in-e2e-tests/
/**
 * Logs the user by making API call to POST /login.
 * Make sure "cypress.json" + CYPRESS_ environment variables
 * have username and password values set.
 */
export const login = (username, password) => {
  // it is ok for the username to be visible in the Command Log
  expect(username, 'username was set').to.be.a('string').and.not.be.empty
  // but the password value should not be shown
  if (typeof password !== 'string' || !password) {
    throw new Error('Missing password value')
  }

  cy.visit('Account/Login')
  cy.get('input[name=__RequestVerificationToken]').then((tokenInput) => {
    cy.request({
      method: 'Post',
      url: 'Account/Login',
      form: true,
      followRedirect: false,
      body: {
        Email: username,
        Password: password,
        RememberMe: false,
        __RequestVerificationToken: tokenInput.val(),
      },
    })
  })
  cy.getCookie('.AspNetCore.Identity.Application').should('exist')
}
