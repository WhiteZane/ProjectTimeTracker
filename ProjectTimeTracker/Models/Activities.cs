using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTimeTracker.Models
{
   
    public class Activities
    {
        public int ActivityID { get; set; }
        public string ActivityName { get; set; }
        public int ActivityTypeID { get; set; }

        public ActivityTypes ActivityTypes { get; set; }

        public ICollection<TimeLog> TimeLog { get; set; }

    }
}


   

  
