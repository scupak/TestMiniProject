Feature: CreateBooking


Scenario: Startdate in the past
	Given I have entered a start date 1 days before today into the datetime pickers
	And I have entered an end date 1 days after today into the datetime pickers
	When The method runs
	Then an argument exception should be thrown

Scenario: Enddate is before stardate
	Given I have entered a start date 5 days after today into the datetime pickers
	And I have entered an end date 4 days after today into the datetime pickers
	When The method runs
	Then an argument exception should be thrown
	
Scenario: Booking before occupied range
	Given I have entered a start date 1 days after today into the datetime pickers
	And I have entered an end date 9 days after today into the datetime pickers
	And the occupied startdate is 10 days after today and the enddate is 20 days after today.
	When The method runs
	Then the booking should be created

Scenario: Booking in occupied range before first day
	Given I have entered a start date 1 days after today into the datetime pickers
	And I have entered an end date 10 days after today into the datetime pickers
	And the occupied startdate is 10 days after today and the enddate is 20 days after today.
	When The method runs
	Then the booking should not be created

Scenario: Booking in occupied range before last day
	Given I have entered a start date 1 days after today into the datetime pickers
	And I have entered an end date 20 days after today into the datetime pickers
	And the occupied startdate is 10 days after today and the enddate is 20 days after today.
	When The method runs
	Then the booking should not be created	
	
Scenario: Booking in occupied range after 
	Given I have entered a start date 20 days after today into the datetime pickers
	And I have entered an end date 21 days after today into the datetime pickers
	And the occupied startdate is 10 days after today and the enddate is 20 days after today.
	When The method runs
	Then the booking should not be created
	
Scenario: Booking after occupied range 
	Given I have entered a start date 21 days after today into the datetime pickers
	And I have entered the maximum end date value into the datetime pickers
	And the occupied startdate is 10 days after today and the enddate is 20 days after today.
	When The method runs
	Then the booking should be created	