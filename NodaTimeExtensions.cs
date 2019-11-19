using System;
using System.Collections.Generic;
using System.Text;
using NodaTime;

namespace Zenworks.Utils {
    public static class NodaTimeExtensions {

        public static ZonedDateTime ThisTimeLastWeek(this ZonedDateTime now) {
            Period oneWeek = Period.FromWeeks(1);
            return new ZonedDateTime(now.LocalDateTime - oneWeek, now.Zone, now.Offset);
        }

        public static ZonedDateTime EarlyThisMorning(this ZonedDateTime now) {
            LocalDateTime localNow = now.LocalDateTime;
            LocalTime threeAM = new LocalTime(3, 0);
            LocalDateTime earlyMorning = localNow.TimeOfDay > threeAM
                ? localNow.Date + threeAM
                : localNow.Date - Period.FromDays(1) + threeAM;
            return new ZonedDateTime(earlyMorning, now.Zone, now.Offset);
        }

        public static Duration TimeUntilTomorrow(this ZonedClock clock) {
            ZonedDateTime zonedNow = clock.GetCurrentZonedDateTime();
            LocalDateTime thisTimeTomorrow = zonedNow.LocalDateTime.PlusDays(1);
            LocalDateTime earlyTomorrow = thisTimeTomorrow.Date + new LocalTime(3, 0);
            ZonedDateTime zonedEarlyTomorrow = zonedNow.Zone.AtLeniently(earlyTomorrow);
            return zonedEarlyTomorrow - zonedNow;
        }

        public static bool Overlaps(this Interval self, Interval other) {
            return self.Contains(other.Start)
                || self.Contains(other.End)
                || other.Contains(self.Start)
                || other.Contains(self.End);
        }
    }
}
