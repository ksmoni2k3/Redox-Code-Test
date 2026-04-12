using EventScheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventScheduler.Interfaces
{
   // Interface which defines methods for accessing and persisting events
    public interface IEventRepository
    {
        // Retrieves all stored events
        Task<IEnumerable<Event>> GetAllAsync();

        // Saves the given list of events
        Task SaveAllAsync(IEnumerable<Event> events);
    }
}
