using System;
using DealCloud.Common.Enums;

namespace DealCloud.Common.Entities
{
    public class Frequency
    {
        public TimeZoneInfo ClientTimeZone { get; private set; }

        public DateTime UtcRunTime { get; private set; }

        public DateTime ClientRunTime { get; private set; }

        public RecurrenceType Recurrence { get; private set; }

        public DateTime GetNearestUtcRunTime()
        {
            var result = TimeZoneInfo.ConvertTime(GetNearestClientRunTime(), ClientTimeZone, TimeZoneInfo.Utc);

            return result;
        }

        public DateTime GetNearestClientRunTime()
        {
            var clientNow = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, ClientTimeZone);
            var result = ClientRunTime;

            while (result < clientNow)
            {
                switch (Recurrence)
                {
                    case RecurrenceType.Instant:
                        result = clientNow;
                        break;
                    case RecurrenceType.Daily:
                        result = result.AddDays(1);
                        break;
                    case RecurrenceType.Weekly:
                        result = result.AddDays(7);
                        break;
                    case RecurrenceType.Monthly:
                        result = result.AddMonths(1);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Recurrence), Recurrence, $"{nameof(Recurrence)} was out of range.");
                }
            }

            return result;
        }

        public FrequencyComponents GetClientFrequencyComponents()
        {
            var result = new FrequencyComponents {Recurrence = Recurrence};

            switch (Recurrence)
            {
                case RecurrenceType.Instant:
                    break;
                case RecurrenceType.Daily:
                    result.Time = ClientRunTime.TimeOfDay;
                    break;
                case RecurrenceType.Weekly:
                    result.Time = ClientRunTime.TimeOfDay;
                    result.WeekDay = ClientRunTime.DayOfWeek;
                    break;
                case RecurrenceType.Monthly:
                    result.Time = ClientRunTime.TimeOfDay;
                    result.Day = ClientRunTime.Day;
                    result.IsLastDayOfMonth = result.Day >= 28;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Recurrence), Recurrence, $"{nameof(Recurrence)} was out of range.");
            }

            return result;
        }

        public override int GetHashCode()
        {
            var result = 17;

            unchecked
            {
                // ReSharper disable NonReadonlyMemberInGetHashCode
                result = result * 37 + (int) Recurrence;
                result = result * 37 + UtcRunTime.GetHashCode();
                result = result * 37 + ClientRunTime.GetHashCode();
                // ReSharper restore NonReadonlyMemberInGetHashCode
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return obj is Frequency frequency &&
                   Recurrence == frequency.Recurrence &&
                   UtcRunTime == frequency.UtcRunTime &&
                   ClientRunTime == frequency.ClientRunTime;
        }

        public override string ToString()
        {
            return GetNearestUtcRunTime().ToString("f");
        }

        private Frequency() { }

        public static Frequency FromUtcDateTime(DateTime utcRunTime, RecurrenceType recurrence, TimeZoneInfo clientTimeZone)
        {
            if (clientTimeZone == null) throw new ArgumentNullException(nameof(clientTimeZone));

            var clientRunTime = TimeZoneInfo.ConvertTime(utcRunTime, TimeZoneInfo.Utc, clientTimeZone);
            var result = new Frequency
            {
                ClientTimeZone = clientTimeZone,
                UtcRunTime = new DateTime(utcRunTime.Year, utcRunTime.Month, utcRunTime.Day, utcRunTime.Hour, utcRunTime.Minute, 0, DateTimeKind.Utc),
                ClientRunTime = new DateTime(clientRunTime.Year, clientRunTime.Month, clientRunTime.Day, clientRunTime.Hour, clientRunTime.Minute, 0, DateTimeKind.Unspecified),
                Recurrence = recurrence
            };

            return result;
        }

        public static Frequency FromClientFrequencyComponents(FrequencyComponents clientComponents, TimeZoneInfo clientTimeZone)
        {
            if (clientComponents == null) throw new ArgumentNullException(nameof(clientComponents));
            if (clientTimeZone == null) throw new ArgumentNullException(nameof(clientTimeZone));

            var clientNow = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, clientTimeZone);
            var nearestClientRunTime = clientNow;
            var minutes = TimeSpan.FromMinutes((clientNow.Hour * 60) + clientNow.Minute);

            // NOTE: logic should break with InvalidOperationException because of invalid object state if any
            // ReSharper disable PossibleInvalidOperationException
            if (clientComponents.Recurrence == RecurrenceType.Daily && clientComponents.Time.Value <= minutes)
            {
                nearestClientRunTime = new DateTime(clientNow.Year, clientNow.Month, clientNow.Day).Add(TimeSpan.FromHours(24) + clientComponents.Time.Value);
            }
            else if (clientComponents.Recurrence == RecurrenceType.Weekly && (clientNow.DayOfWeek != clientComponents.WeekDay.Value || clientComponents.Time.Value <= minutes))
            {
                var daysToAdd = ((int)clientComponents.WeekDay.Value - (int) clientNow.DayOfWeek + 7) % 7;

                var nearestWeekDay = clientNow.AddDays(daysToAdd == 0 ? 7 : daysToAdd);

                nearestClientRunTime = new DateTime(nearestWeekDay.Year, nearestWeekDay.Month, nearestWeekDay.Day).Add(clientComponents.Time.Value);
            }
            else if (clientComponents.Recurrence == RecurrenceType.Monthly && (clientNow.Day != clientComponents.Day.Value || clientComponents.Time.Value <= minutes))
            {
                var daysLeftInCurrentMonth = DateTime.DaysInMonth(clientNow.Year, clientNow.Month) - clientNow.Day;
                var daysToAdd = clientNow.Day < clientComponents.Day.Value
                    ? clientComponents.IsLastDayOfMonth.HasValue && clientComponents.IsLastDayOfMonth.Value
                        ? daysLeftInCurrentMonth
                        : clientComponents.Day.Value - clientNow.Day
                    : clientComponents.IsLastDayOfMonth.HasValue && clientComponents.IsLastDayOfMonth.Value
                        ? daysLeftInCurrentMonth + DateTime.DaysInMonth(clientNow.Year, clientNow.Month + 1)
                        : daysLeftInCurrentMonth + clientComponents.Day.Value;

                var nearestMonthDay = clientNow.AddDays(daysToAdd);

                nearestClientRunTime = new DateTime(nearestMonthDay.Year, nearestMonthDay.Month, nearestMonthDay.Day).Add(clientComponents.Time.Value);
            }
            else if (clientComponents.Recurrence != RecurrenceType.Instant)
            {
                nearestClientRunTime = new DateTime(clientNow.Year, clientNow.Month, clientNow.Day).Add(clientComponents.Time.Value);
            }
            // ReSharper restore PossibleInvalidOperationException

            var result = FromUtcDateTime(TimeZoneInfo.ConvertTime(nearestClientRunTime, clientTimeZone, TimeZoneInfo.Utc), clientComponents.Recurrence, clientTimeZone);

            return result;
        }
    }

    public class FrequencyComponents
    {
        private static readonly TimeSpan _maxTimeSpanValue = TimeSpan.FromMinutes((24 * 60) - 1); // NOTE: max minutes is 23.59

        private TimeSpan? _time;

        private int? _day;

        /// <summary>
        ///     Get or sets <see cref="RecurrenceType"/> for the current instance.
        /// </summary>
        public RecurrenceType Recurrence { get; set; }

        /// <summary>
        ///     Gets or sets time of the day to spawn for instances which <see cref="Recurrence"/> is 
        ///     <see cref="RecurrenceType.Daily"/>, <see cref="RecurrenceType.Weekly"/> or <see cref="RecurrenceType.Monthly"/>.
        ///     Value can not be greater than 23:59:00.
        /// </summary>
        public TimeSpan? Time
        {
            get { return _time; }
            set
            {
                if (value.HasValue && value.Value > _maxTimeSpanValue)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(Time)} value can not be more than {_maxTimeSpanValue}.");
                }

                _time = value;
            }
        }

        /// <summary>
        ///     Gets or sets day of week to spawn for instances which <see cref="Recurrence"/> is <see cref="RecurrenceType.Weekly"/>.
        /// </summary>
        public DayOfWeek? WeekDay { get; set; }

        /// <summary>
        ///     Gets or sets day of week to spawn for instances which <see cref="Recurrence"/> is <see cref="RecurrenceType.Monthly"/>.
        ///     Value can not be greater than 31
        /// </summary>
        public int? Day
        {
            get { return _day; }
            set
            {
                if (value.HasValue && value.Value > 31)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(Day)} value can not be more than 31.");
                }

                _day = value;
            }
        }

        /// <summary>
        ///     Gets or sets value indicating need to spawn at last day of each month
        ///     for instances which <see cref="Recurrence"/> is <see cref="RecurrenceType.Monthly"/>.
        /// </summary>
        public bool? IsLastDayOfMonth { get; set; }
    }
}