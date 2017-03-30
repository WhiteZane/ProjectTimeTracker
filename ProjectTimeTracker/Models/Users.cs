using System;
using System.Collections.Generic;

namespace ProjectTimeTracker.Models
{
    public class Users
    {
        public int ID { get; set; }
        public string FullName { get; set; }

        public ICollection<TimeLog> TimeLog { get; set; }
    }
}
