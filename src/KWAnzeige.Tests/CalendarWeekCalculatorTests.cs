// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalendarWeekCalculatorTests.cs" company="Hämmer Electronics">
//   Copyright (c) All rights reserved.
// </copyright>
// <summary>
//   A class to test the <see cref="CalendarWeekCalculator" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace KWAnzeige.Tests;

/// <summary>
/// A class to test the <see cref="CalendarWeekCalculator"/> class.
/// </summary>
[TestClass]
public class CalendarWeekCalculatorTests
{
    /// <summary>
    /// The first day of the range that is checked day by day.
    /// </summary>
    private static readonly DateTime RangeStart = new(1900, 1, 1);

    /// <summary>
    /// The last day of the range that is checked day by day.
    /// </summary>
    private static readonly DateTime RangeEnd = new(2100, 12, 31);

    /// <summary>
    /// Checks the documented edge cases of the ISO 8601 week date, the ones where the calendar week does not
    /// belong to the year the date is in. The expected values are the examples of the ISO 8601 week date
    /// definition, they are not taken from the implementation.
    /// </summary>
    /// <param name="year">The year of the date to check.</param>
    /// <param name="month">The month of the date to check.</param>
    /// <param name="day">The day of the date to check.</param>
    /// <param name="expectedWeek">The expected calendar week.</param>
    [TestMethod]
    [DataRow(1977, 1, 1, 53)]
    [DataRow(1977, 1, 2, 53)]
    [DataRow(1977, 12, 31, 52)]
    [DataRow(1978, 1, 1, 52)]
    [DataRow(1978, 1, 2, 1)]
    [DataRow(1978, 12, 31, 52)]
    [DataRow(1979, 1, 1, 1)]
    [DataRow(1979, 12, 30, 52)]
    [DataRow(1979, 12, 31, 1)]
    [DataRow(1980, 1, 1, 1)]
    [DataRow(1980, 12, 28, 52)]
    [DataRow(1980, 12, 29, 1)]
    [DataRow(1981, 1, 1, 1)]
    [DataRow(1981, 12, 31, 53)]
    [DataRow(1982, 1, 1, 53)]
    [DataRow(1982, 1, 3, 53)]
    [DataRow(2020, 12, 31, 53)]
    [DataRow(2021, 1, 1, 53)]
    [DataRow(2021, 1, 4, 1)]
    [DataRow(2026, 1, 1, 1)]
    public void GetIso8601WeekOfYearReturnsTheDocumentedWeekForTheEdgeCases(int year, int month, int day, int expectedWeek)
    {
        var week = CalendarWeekCalculator.GetIso8601WeekOfYear(new DateTime(year, month, day));

        Assert.AreEqual(expectedWeek, week);
    }

    /// <summary>
    /// Checks whether every day of one week returns the same calendar week, from the Monday to the Sunday. That is
    /// the property the shifting of Monday to Wednesday in the implementation exists for.
    /// </summary>
    [TestMethod]
    public void GetIso8601WeekOfYearReturnsTheSameWeekForEveryDayOfAWeek()
    {
        var monday = new DateTime(2026, 8, 10);

        for (var offset = 0; offset < 7; offset++)
        {
            var date = monday.AddDays(offset);
            var week = CalendarWeekCalculator.GetIso8601WeekOfYear(date);

            Assert.AreEqual(33, week, $"The week of {date:yyyy-MM-dd} ({date.DayOfWeek}) is wrong.");
        }
    }

    /// <summary>
    /// Checks the implementation against <see cref="ISOWeek.GetWeekOfYear"/> of the framework for every single day
    /// of two centuries. The hand written calculation predates that method, this test is what keeps the two from
    /// drifting apart.
    /// </summary>
    [TestMethod]
    public void GetIso8601WeekOfYearMatchesTheCalendarWeekOfTheFramework()
    {
        for (var date = RangeStart; date <= RangeEnd; date = date.AddDays(1))
        {
            var week = CalendarWeekCalculator.GetIso8601WeekOfYear(date);

            Assert.AreEqual(ISOWeek.GetWeekOfYear(date), week, $"The week of {date:yyyy-MM-dd} is wrong.");
        }
    }

    /// <summary>
    /// Checks whether the returned week always stays inside the range ISO 8601 allows, which is what the text box
    /// of the main form shows unfiltered.
    /// </summary>
    [TestMethod]
    public void GetIso8601WeekOfYearNeverLeavesTheRangeOfOneToFiftyThree()
    {
        for (var date = RangeStart; date <= RangeEnd; date = date.AddDays(1))
        {
            var week = CalendarWeekCalculator.GetIso8601WeekOfYear(date);

            Assert.IsTrue(week >= 1 && week <= 53, $"The week of {date:yyyy-MM-dd} is {week}.");
        }
    }

    /// <summary>
    /// Checks whether the time of day is ignored. The main form passes <see cref="DateTime.Now"/> every second, so
    /// the result has to depend on the date alone.
    /// </summary>
    [TestMethod]
    public void GetIso8601WeekOfYearIgnoresTheTimeOfDay()
    {
        var midnight = new DateTime(2026, 8, 13, 0, 0, 0);
        var beforeMidnight = new DateTime(2026, 8, 13, 23, 59, 59);

        Assert.AreEqual(CalendarWeekCalculator.GetIso8601WeekOfYear(midnight), CalendarWeekCalculator.GetIso8601WeekOfYear(beforeMidnight));
    }

    /// <summary>
    /// Checks whether the current culture is irrelevant. The implementation asks
    /// <see cref="CultureInfo.InvariantCulture"/> for its calendar on purpose, a culture with a non Gregorian
    /// default calendar such as ar-SA must not change the result.
    /// </summary>
    [TestMethod]
    public void GetIso8601WeekOfYearIgnoresTheCurrentCulture()
    {
        var date = new DateTime(2026, 8, 13);
        var currentCulture = CultureInfo.CurrentCulture;

        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("ar-SA");
            var week = CalendarWeekCalculator.GetIso8601WeekOfYear(date);

            Assert.AreEqual(33, week);
        }
        finally
        {
            CultureInfo.CurrentCulture = currentCulture;
        }
    }
}
