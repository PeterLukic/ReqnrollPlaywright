Feature: OrangeHRM Login

As a user of OrangeHRM system
I want to be able to login to the application
So that I can access the HR management features

@login @smoke
Scenario: Successful login with valid credentials
	Given I navigate to the OrangeHRM login page
	When I enter valid username and password
	And I click the login button
	Then I should see the dashboard page

#@login @negative
#Scenario: Login with invalid credentials
	#Given I am on the OrangeHRM login page
	#When I enter invalid username "InvalidUser" and password "InvalidPass"
	#And I click the login button
	#Then I should see an error message


#@login @negative1
#Scenario: Login with empty credentials
	#Given I am on the OrangeHRM login page
	#When I leave username and password fields empty
	#And I click the login button
	#Then I should see validation errors for username and password fields


#@login @logintable
#Scenario Outline: Login with different credential combinations
	#Given I am on the OrangeHRM login page
	#When I enter username "<username>" and password "<password>"
	#And I click the login button
	#Then the login result should be "<result>"

	#Examples:
		#| username | password  | result  |
		#| Admin    | admin123  | success |
		#| admin    | adminAAA  | failure |  