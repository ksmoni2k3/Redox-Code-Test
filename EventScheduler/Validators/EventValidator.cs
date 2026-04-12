using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventScheduler.Models;

namespace EventScheduler.Validators
{
    // Provides validation rules for Event objects
    public static class EventValidator
    {
        public static void Validate(Event e)
        {
            if (string.IsNullOrWhiteSpace(e.Name))
                throw new ArgumentException("Event name is required.");

            if (string.IsNullOrWhiteSpace(e.Location))
                throw new ArgumentException("Event location is required.");

            if (e.DateTime <= DateTime.Now)
                throw new ArgumentException("Event must be scheduled in the future.");

            if (e.Duration.HasValue && e.Duration <= TimeSpan.Zero)
                throw new ArgumentException("Duration must be greater than zero.");
        }
    }
}