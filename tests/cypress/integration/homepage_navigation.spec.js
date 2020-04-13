context("Homepage navigation", () => {
  it("can navigate to public page", () => {
    cy.visit("/")
    cy.contains("Public Inquiry Portal").click()
    cy.url().should("eq", Cypress.config().baseUrl + "/Public")
  })

  it("can navigate to Employee Access Portal", () => {
    cy.visit("/")
    cy.contains("Employee Access Portal").click()
    cy.url().should("eq", Cypress.config().baseUrl + "/Account/Login")
  })
})
