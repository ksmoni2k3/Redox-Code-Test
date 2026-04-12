# Redox Code Test

**Make a fork of this repository and complete the following exercises in c#. Once completed, submit a pull request with your solutions.**

## Exercise 1: LINQ Query
1. Create a list of integers from 1 to 100. 
2. Use LINQ to find all even numbers in the list and print them.
3. Use a loop to find all of the numbers in the original list that are divisible by 3 or 5, but not 3 and 5. 
   The result should be:

>   3,  5,  6,  9, 10, 12, 18, 20, 21, 24, 25, 27, 33, 35, 36, 39, 40, 42, 48, 50, 51, 54, 55, 57, 63, 65, 66, 69, 70, 72, 78, 80, 81, 84, 85, 87, 93, 95, 96, 99


## Exercise 2: Event Scheduler
1. Create a class named 'Event' with properties 'Name', 'Location', 'DateTime'.
2. Create a class `EventScheduler` with a list of `Event`. Add methods to `ScheduleEvent`, `CancelEvent`, and `GetUpcomingEvents`.
3. Implement a feature to prevent double-booking, where two events are scheduled for the exact same time.
4. Save the Events to persistent storage. Any persistant storage can be used.
