using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTimeTrackerAuth.Models
{
    public class TimeLog
    {
        public int TimeLogID { get; set; }
        public string Username { get; set; }
        public int ActivityID { get; set; }
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        [Display(Name = "End Time")]
        public DateTime? EndTime { get; set; }

        public Activity Activities { get; set; }
    }
}
