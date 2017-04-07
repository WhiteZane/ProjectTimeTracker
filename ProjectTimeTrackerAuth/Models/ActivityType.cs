using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTimeTrackerAuth.Models
{
    public class ActivityType
    {
        [Key]
        public int ActivityTypeID { get; set; }

        public string Username { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Activity Description cannot be longer than 50 characters")]
        [Display(Name = "Activity Type")]
        public string ActTypeDescrip { get; set; }

        [Display(Name = "Production")]
        public bool ActTypeProd { get; set; }

        public ICollection<Activity> Activities { get; set; }
    }
}
