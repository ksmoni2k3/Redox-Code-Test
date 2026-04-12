using EventScheduler.Interfaces;
using EventScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventScheduler.Logging;
using EventScheduler.Validators;

namespace EventScheduler.Services
{
    // Handles event-related business logic such as scheduling, cancellation, and retrieving upcoming events
    public class EventService : IEventService
    {
        private readonly IEventRepository _repository;
        private readonly ILogger _logger;

        public EventService(IEventRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // To Schedule a new event 
        public async Task ScheduleEventAsync(Event newEvent)
        {
            try
            {
                // To Validate input data
                EventValidator.Validate(newEvent);

                var events = (await _repository.GetAllAsync()).ToList();

                // To Check overlapping events (supports optional duration)
                bool isConflict = events.Any(e =>
                {
                    var existingStart = e.DateTime;
                    var existingEnd = e.Duration.HasValue
                        ? e.DateTime.Add(e.Duration.Value)
                        : e.DateTime;

                    var newStart = newEvent.DateTime;
                    var newEnd = newEvent.Duration.HasValue
                        ? newEvent.DateTime.Add(newEvent.Duration.Value)
                        : newEvent.DateTime;

                    return newStart < existingEnd && existingStart < newEnd;
                });

                if (isConflict)
                {
                    throw new InvalidOperationException("Event time conflict detected.");
                }

                events.Add(newEvent);
                await _repository.SaveAllAsync(events);

                _logger.Info($"Event '{newEvent.Name}' scheduled.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
        }

        // To Cancel an event based on name and date/time
        public async Task CancelEventAsync(string name, DateTime dateTime)
        {
            var events = (await _repository.GetAllAsync()).ToList();

            var eventToRemove = events.FirstOrDefault(e =>
                e.Name == name && e.DateTime == dateTime);

            if (eventToRemove != null)
            {
                events.Remove(eventToRemove);
                await _repository.SaveAllAsync(events);
                _logger.Info($"Event '{name}' cancelled.");
            }
        }

        // To Retrieve upcoming events sorted by date/time
        public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
        {
            var events = await _repository.GetAllAsync();

            return events
                .Where(e => e.DateTime > DateTime.Now)
                .OrderBy(e => e.DateTime);
        }
    }
}