
﻿using EventScheduler.Interfaces;
using EventScheduler.Models;
using LinqQuery.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;


// Build configuration from appsettings.json
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appSettings.json", optional: false)
    .Build();

// Setup Dependency Injection container
var services = new ServiceCollection()
    .AddApplicationServices(configuration);

// Build service provider (DI container)
var provider = services.BuildServiceProvider();

// Resolve required services
var numberService = provider.GetRequiredService<INumberService>();
var eventService = provider.GetRequiredService<IEventService>();


while (true)
{
    ShowMainMenu();

    var input = Console.ReadLine();

    switch (input)
    {
        case "1":
            RunLinq(numberService);
            break;

        case "2":
            await RunEventScheduler(eventService);
            break;

        case "3":
            Console.WriteLine("Exiting application...");
            return;

        default:
            Console.WriteLine("Invalid option. Try again.");
            break;
    }
}

#region MENU METHODS

// Displays the main menu options to the user.
static void ShowMainMenu()
{
    Console.WriteLine("\n=========================");
    Console.WriteLine("REDOX CODE TEST EXERCISES");
    Console.WriteLine("=========================");
    Console.WriteLine("1. LINQ Operations");
    Console.WriteLine("2. Event Scheduler");
    Console.WriteLine("3. Exit");
    Console.Write("Select option: ");
}


// Handles LINQ-related operations: 
static void RunLinq(INumberService service)
{
    Console.Write("\nEnter number limit: ");

    // Validate input
    if (!int.TryParse(Console.ReadLine(), out int n))
    {
        Console.WriteLine("Invalid input. Using default 100.");
        n = 100;
    }

    // Prevent invalid range (negative or zero)
    if (n <= 0)
    {
        Console.WriteLine("Number must be greater than 0. Using default 100.");
        n = 100;
    }

    // Generate numbers from 1 to N
    var numbers = service.GenerateNumbers(n);

    // Display even numbers using LINQ
    Console.WriteLine("\nEven Numbers:");
    Console.WriteLine(string.Join(", ", service.GetEvenNumbers(numbers)));

    // Display numbers divisible by 3 or 5 but not both
    Console.WriteLine("\nDivisible by 3 or 5 (but not both):");
    Console.WriteLine(string.Join(", ", service.GetThreeOrFiveDividends(numbers)));
}


// Displays Event Scheduler menu and routes to appropriate operation
static async Task RunEventScheduler(IEventService service)
{
    //  Added loop to keep user inside Event Scheduler
    while (true)
    {
        Console.WriteLine("\nEVENT SCHEDULER");
        Console.WriteLine("----------------");
        Console.WriteLine("1. Schedule Event");
        Console.WriteLine("2. Cancel Event");
        Console.WriteLine("3. View Upcoming Events");
        Console.WriteLine("4. Back to Main Menu"); 
        Console.Write("Select option: ");

        var option = Console.ReadLine();

        switch (option)
        {
            case "1":
                await ScheduleEvent(service);
                break;

            case "2":
                await CancelEvent(service);
                break;

            case "3":
                await ViewEvents(service);
                break;

            case "4":
                return;

            default:
                Console.WriteLine("Invalid option.");
                break;
        }

        // Optional UX improvement (pause before showing menu again)
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}

// Collects user input and schedules a new event. Includes validation and optional duration support.
static async Task ScheduleEvent(IEventService service)
{
    var e = new Event();

    Console.Write("\nEnter Event Name: ");
    e.Name = Console.ReadLine() ?? "";

    Console.Write("Enter Event location: ");
    e.Location = Console.ReadLine() ?? "";

    Console.Write("Enter date and time (yyyy-MM-dd HH:mm): ");
    var dateInput = Console.ReadLine();

    // Validate date input
    if (!DateTime.TryParse(dateInput, out DateTime dateTime))
    {
        Console.WriteLine("Invalid date format");
        return;
    }

    e.DateTime = dateTime;

    // Optional duration input (enhancement for better scheduling)
    Console.Write("Enter Event duration in minutes (optional, press Enter to skip): ");
    var durationInput = Console.ReadLine();

    if (!string.IsNullOrWhiteSpace(durationInput) &&
        int.TryParse(durationInput, out int minutes))
    {
        e.Duration = TimeSpan.FromMinutes(minutes);
    }

    try
    {
        await service.ScheduleEventAsync(e);
        Console.WriteLine("Event scheduled successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}


// Allows user to cancel an event by selecting from a list 
static async Task CancelEvent(IEventService service)
{
    var events = (await service.GetUpcomingEventsAsync()).ToList();

    if (!events.Any())
    {
        Console.WriteLine("No upcoming events to cancel.");
        return;
    }

    Console.WriteLine("\nSelect an event to cancel:");

    // Display events with index
    for (int i = 0; i < events.Count; i++)
    {
        Console.WriteLine(
            $"{i + 1}. {events[i].Name} | {events[i].Location} | {events[i].DateTime}");
    }

    Console.Write("\nEnter event number: ");

    // Validate selection
    if (!int.TryParse(Console.ReadLine(), out int index) ||
        index < 1 || index > events.Count)
    {
        Console.WriteLine("Invalid selection.");
        return;
    }

    var selectedEvent = events[index - 1];

    // Cancel selected event
    await service.CancelEventAsync(
        selectedEvent.Name,
        selectedEvent.DateTime);

    Console.WriteLine("Event cancelled successfully.");
}


// Displays all upcoming events sorted by date
static async Task ViewEvents(IEventService service)
{
    var events = await service.GetUpcomingEventsAsync();

    Console.WriteLine("\nUpcoming Events:");

    if (events.Any())
    {
        foreach (var e in events)
        {
            Console.WriteLine($"{e.Name} | {e.Location} | {e.DateTime}");
        }
    }
    else
    {
        Console.WriteLine("No Upcoming Events");
    }
}

#endregion