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
            LocalDate date = zonedNow.Date;
            LocalTime time = zonedNow.TimeOfDay;
            LocalTime threeAM = new LocalTime(03, 00);
            if (time > threeAM) {
                date = date.PlusDays(1);
            }
            LocalDateTime next3AM = date + threeAM;
            ZonedDateTime zonedNext3AM = zonedNow.Zone.AtLeniently(next3AM);
            return zonedNext3AM - zonedNow;
        }

        public static bool Overlaps(this Interval self, Interval other) {
            return self.Contains(other.Start)
                || self.Contains(other.End)
                || other.Contains(self.Start)
                || other.Contains(self.End);
        }
    }
}
