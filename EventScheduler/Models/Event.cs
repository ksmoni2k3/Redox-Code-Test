using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventScheduler.Models
{
    public class Event
    {
        //Name, location, Start datetime of the event
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }

       // Duration of the event (Enhancement to avoid conflicts practically which is optional as per given requirement)
       public TimeSpan? Duration { get; set; }
    }

}
