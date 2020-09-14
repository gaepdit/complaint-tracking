context('Authentication', () => {
  beforeEach(() => {
    cy.visit('Account/Login')
  })

  it('fails if login does not include token', () => {
    cy.request({
      method: 'Post',
      url: 'Account/Login',
      failOnStatusCode: false,
      body: {
        Email: Cypress.env('staff-user'),
        Password: Cypress.env('staff-pass'),
        RememberMe: false,
      },
    }).should((resp) => {
      expect(resp.status).to.eq(400)
    })
    cy.getCookie('.AspNetCore.Identity.Application').should('not.exist')
  })

  it('sets identity cookie on successful login', () => {
    cy.get('input[name=__RequestVerificationToken]').then((tokenInput) => {
      cy.request({
        method: 'Post',
        url: 'Account/Login',
        form: true,
        followRedirect: false,
        body: {
          Email: Cypress.env('staff-user'),
          Password: Cypress.env('staff-pass'),
          RememberMe: false,
          __RequestVerificationToken: tokenInput.val(),
        },
      }).should((resp) => {
        expect(resp.status).to.eq(302)
      })
      cy.getCookie('.AspNetCore.Identity.Application').should('exist')
    })
  })

  it('can log in and log out', () => {
    cy.get('h1').should('contain', 'Agency Login')

    cy.get('#Email').type(Cypress.env('staff-user'))
    cy.get('#Password').type(Cypress.env('staff-pass'), { log: false })
    cy.contains('Log in').click()
    cy.url().should('eq', Cypress.config().baseUrl + '/')
    cy.get('h1').should('contain', 'Dashboard')
    cy.get('.usa-alert-info').should(
      'contain.text',
      'You have been logged in. Welcome back!'
    )
    cy.getCookie('.AspNetCore.Identity.Application').should('exist')

    cy.contains('Admin').click()
    cy.contains('Sign out').click()
    cy.url().should('eq', Cypress.config().baseUrl + '/')
    cy.get('h1').should('contain', 'Georgia EPD Complaint Tracking System')
    cy.getCookie('.AspNetCore.Identity.Application').should('not.exist')
  })

  it('does not log in if password is incorrect', () => {
    cy.get('#Email').type(Cypress.env('staff-user'))
    cy.get('#Password').type('123')
    cy.contains('Log in').click()
    cy.url().should('eq', Cypress.config().baseUrl + '/Account/Login')
    cy.get('h1').should('contain', 'Agency Login')
    cy.getCookie('.AspNetCore.Identity.Application').should('not.exist')

    cy.get('.usa-alert-error')
      .contains('Invalid login attempt.')
      .should('exist')
    cy.get('.usa-alert-error').get('.usa-button').click()
    cy.get('.usa-alert-error')
      .contains('Invalid login attempt.')
      .should('not.exist')
  })

  it('does not log in if user does not exist', () => {
    cy.get('#Email').type('no-one@example.net')
    cy.get('#Password').type('123')
    cy.contains('Log in').click()
    cy.url().should('eq', Cypress.config().baseUrl + '/Account/Login')
    cy.get('h1').should('contain', 'Agency Login')
    cy.getCookie('.AspNetCore.Identity.Application').should('not.exist')
    cy.get('.usa-alert-error')
      .contains('Invalid login attempt.')
      .should('exist')
  })
})
