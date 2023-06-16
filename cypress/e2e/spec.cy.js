describe('Cith pages', () => {
  beforeEach(() => {
    cy.visit('https://cith.nl/');
  });
  it('Check CV', () => {
    cy.contains('CV').click();
    // cy.get('span.searchbutton').click();
    cy.contains('Philip Thuijs').should('be.visible');
    cy.contains('OPLEIDING').should('be.visible');
    cy.contains('Experience').should('be.visible');
  });

  it('Check Games Page', () => {
    cy.contains('Games').click();
    cy.contains('FILES').should('be.visible');
  });

  it('Check Privacy Page', () => {
    cy.contains('Privacy').click();
    cy.contains('Privacy Policy').should('be.visible');
  });

  it('Check Bad Login', () => {
    cy.contains('Login').click();
    cy.get('#Input_Email').type('fake mail');
    cy.get('#Input_Password').type('fake pass{enter}');
    cy.contains('Invalid login attempt.').should('be.visible');
  });
});

describe('Cith API School', () => {
  let baseURL = "https://cith.nl/api/";

  beforeEach(() => {
    // Clears all data, this api is just for school.
    cy.request(baseURL + 'school/Reset')
  });


  it('Get list', () => {
    cy.request(baseURL + 'school/list').its('status').should('be.equal', 200);
  });

  it('Get school', () => {
    cy.request(baseURL + 'school').its('status').should('be.equal', 200);
  });

  it('POST school/5', () => {
    cy.request({
      method: 'POST', 
      url: baseURL + 'school/Create/5',
      failOnStatusCode: true
    });
  });
  
  it('POST school/5 error 409 on second post', () => {
    cy.request({
      method: 'POST', 
      url: baseURL + 'school/Create/5',
      failOnStatusCode: true
    });

    cy.request({
      method: 'POST', 
      url: baseURL + 'school/Create/5',
      failOnStatusCode: false
    }).then((response) => {
      expect(response.status).to.eq(409)
    });
  });


  it('PUT school/6 and GET same value', () => {
    cy.request({
      method: 'PUT', 
      url: baseURL + 'school/6/HelloWorld',
      failOnStatusCode: false
    });

    cy.request({
      method: 'GET', 
      url: baseURL + 'school/6',
      failOnStatusCode: true
    }).then((response) => {
      expect(response.body).to.eq('HelloWorld');
    });

    cy.request({
      method: 'PUT', 
      url: baseURL + 'school/6/Hello',
      failOnStatusCode: false
    });

    cy.request({
      method: 'GET', 
      url: baseURL + 'school/6',
      failOnStatusCode: true
    }).then((response) => {
      expect(response.body).to.eq('Hello');
    });
  });

  it('DELETE school', () => {
    cy.request({
      method: 'POST', 
      url: baseURL + 'school/Create/5'
    });

    cy.request({
      method: 'DELETE', 
      url: baseURL + 'school/5'
    });

    cy.request({
      method: 'DELETE', 
      url: baseURL + 'school/5',
      failOnStatusCode: false
    }).then((response) => {
      expect(response.status).to.eq(404)
    });
  });
});
/**
 * [{"method":"GET","route":"/api/School/List","action":"School.GetList"},
 * {"method":"GET","route":"/api/School","action":"School.Details"},
 * {"method":"GET","route":"/api/School/{id}","action":"School.OneDetail"},
 * {"method":"POST","route":"/api/School/Create/{id}","action":"School.Create"},
 * {"method":"PUT","route":"/api/School/{id}/{newValue}","action":"School.Edit"},
 * {"method":"DELETE","route":"/api/School/{id}","action":"School.Delete"}]
 */
