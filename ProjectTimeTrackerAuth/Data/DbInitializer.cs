using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTimeTrackerAuth.Data {
    public class DbInitializer {

        public static void Initialize(TimerContext context) {
            context.Database.EnsureCreated();

            // Look for any activity types.
            //if (context.ActivityTypes.Any()) {
                return;   // DB has been seeded
            //}

        }
    }
}