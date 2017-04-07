using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTimeTrackerAuth.Models
{
    public class Activity
    {
        [Key]
        [Display(Name = "Activity ID")]
        public int ActivityID { get; set; }

        public string Username { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Activity Description cannot be longer than 50 characters")]
        [Display(Name = "Activity")]
        public string ActivityName { get; set; }

        [Display(Name = "Activity Type")]
        public int ActivityTypeID { get; set; }

        [Display(Name = "Activity Type")]
        public ActivityType ActivityTypes { get; set; }

        public ICollection<TimeLog> TimeLogs { get; set; }
    }
}
