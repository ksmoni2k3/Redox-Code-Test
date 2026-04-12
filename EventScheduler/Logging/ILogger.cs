using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventScheduler.Logging
{
    // Defines logging methods for application events
    public interface ILogger
    {
        void Info(string message);
        void Error(string message);
    }
}
