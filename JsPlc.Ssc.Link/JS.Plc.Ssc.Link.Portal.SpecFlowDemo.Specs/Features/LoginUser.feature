Feature: LoginUser

Scenario: Logging in with valid credentials
	Given I am at the Home page
	Then I click the login button
	Then I should be at the login page
	Then I fill in the following form
	| field | value |
	| Username | steven.farkas@jstest3.onmicrosoft.com |
	| Password | Password1* |
	Then I click the login button
	Then I should be at the home page
	Given I am loged in i should see the menu
