Feature: CreateBooking
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario Outline: Add two numbers
	Given I have entered <firstNumber> into the calculator
	And I have also entered <secondNumber> into the calculator
	When I press add

	Examples:
	| firstNumber | secondNumber | sum |
	| 50          | 70           | 120 |
	| 30          | 20           |  50 |
