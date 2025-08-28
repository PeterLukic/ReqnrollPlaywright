Feature: OrangeHRM Login feature
  As a user of OrangeHRM system
  I want to be able to login to the application
  So that I can access the HR management features
  
  @login @negative
  Scenario: Login with invalid credentials
	Given I am on the OrangeHRM login page
	When I enter invalid username "InvalidUser" and password "InvalidPass"
	And I click the login button
	Then I should see an error message