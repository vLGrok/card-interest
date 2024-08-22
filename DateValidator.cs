using System;
using System.Globalization;

public static class DateValidator
{
    public static DateTime ValidateDateString(string dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString))
        {
            throw new ArgumentException("Date string cannot be null or empty.");
        }

        if (!DateTime.TryParseExact(dateString, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
        {
            throw new FormatException($"Invalid date format. Expected format is MM/dd/yyyy. Input was: {dateString}");
        }

        return parsedDate;
    }
}
