// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalendarWeekCalculator.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to calculate the ISO 8601 calendar week of a date.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace KWAnzeige;

/// <summary>
/// A class to calculate the ISO 8601 calendar week of a date.
/// </summary>
public static class CalendarWeekCalculator
{
    /// <summary>
    /// Gets the ISO8601 week of the year.
    /// </summary>
    /// <param name="time">The time to check.</param>
    /// <returns>The calendar week.</returns>
    public static int GetIso8601WeekOfYear(DateTime time)
    {
        var day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);

        // Monday to Wednesday are moved to the Thursday of the same week, because FirstFourDayWeek
        // alone only agrees with ISO 8601 from Thursday onwards.
        if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
        {
            time = time.AddDays(3);
        }

        return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }
}
