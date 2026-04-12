using EventScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EventScheduler.Interfaces
{
    // Provides business logic for managing events
    public interface IEventService
    {
        // Schedules a new event
        Task ScheduleEventAsync(Event newEvent);

        // Cancels an existing event based on name and time
        Task CancelEventAsync(string name, DateTime dateTime);

        // Retrieves upcoming events sorted by date
        Task<IEnumerable<Event>> GetUpcomingEventsAsync();
    }
}