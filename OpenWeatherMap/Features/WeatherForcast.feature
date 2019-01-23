Feature: WeatherForcast
	In order to holiday in Sydney
	As a holiday Makey
	I want to know warmer days on specific days

@TestSuite1
Scenario Outline: A happy holiday maker
          Given I like to holiday in <City>
          And I only like to holiday on <WeekDay>
          When I look up the weather forecast
          Then I receive the weather forecast		  
          And the temperature is warmer than <Temperature> degrees on <WeekDay>
		  And the destination is <City>	  
		  


Examples: 
  
      | City   | WeekDay    | Temperature |
      | Sydney | Thursday   | 10          |
      
