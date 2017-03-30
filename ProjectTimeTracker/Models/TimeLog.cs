using System;

namespace ProjectTimeTracker.Models
{
    

    public class TimeLog
    {
        public int TimeLogID { get; set; }
        public int ActivityID { get; set; }
        public int UsersID { get; set; }
        public DateTime StartTime{ get; set; }
        public DateTime EndTime { get; set; }

        public Activities Activities { get; set; }
        public Users Users { get; set; }
    }
}