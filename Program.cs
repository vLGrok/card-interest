using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        if (!TryParseArguments(args, out decimal interestRate, out string filePath))
            return;

        if (!TryGetDateRange(out DateTime startDate, out DateTime endDate))
            return;

        if (!TryProcessFile(filePath, out Dictionary<DateTime, decimal> entries))
            return;

        decimal totalInterest = CalculateInterest(entries, interestRate, startDate, endDate);
        Console.WriteLine($"Total Interest for the period {startDate.ToShortDateString()} to {endDate.ToShortDateString()}: {totalInterest:C}");
    }

    static bool TryParseArguments(string[] args, out decimal interestRate, out string filePath)
    {
        interestRate = 0;
        filePath = string.Empty;

        if (args.Length < 2)
        {
            Console.WriteLine("Usage: Program <interest_rate> <file_path>");
            return false;
        }

        if (!decimal.TryParse(args[0], NumberStyles.Float, CultureInfo.InvariantCulture, out interestRate) || interestRate <= 0)
        {
            Console.WriteLine("Error: Invalid interest rate. Please provide a positive decimal value (e.g., 0.21 for 21%).");
            return false;
        }

        filePath = args[1];
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: The file '{filePath}' does not exist.");
            return false;
        }

        return true;
    }

    static bool TryGetDateRange(out DateTime startDate, out DateTime endDate)
    {
        startDate = DateTime.MinValue;
        endDate = DateTime.MinValue;

        try
        {
            startDate = GetValidatedDate("Please enter the start date (M/d/yy): ");
            endDate = GetValidatedDate("Please enter the end date (M/d/yy): ");

            if (startDate > endDate)
            {
                Console.WriteLine("Error: Start date must be before or equal to the end date.");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }

        return true;
    }

    static DateTime GetValidatedDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            try
            {
                return ValidateDateString(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    static bool TryProcessFile(string filePath, out Dictionary<DateTime, decimal> entries)
    {
        entries = new Dictionary<DateTime, decimal>();

        try
        {
            string[] lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                // Split the line using the tab character as the delimiter
                var parts = line.Split('\t');
                if (parts.Length == 2)
                {
                    string balanceString = parts[0].Trim();
                    string dateString = parts[1].Trim();

                    // Remove any commas in the balance string and parse as currency
                    balanceString = balanceString.Replace(",", "");
                    decimal balance = ParseCurrency(balanceString);
                    DateTime date = DateTime.Parse(dateString, CultureInfo.InvariantCulture);

                    entries[date] = balance;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing file: {ex.Message}");
            return false;
        }

        return true;
    }

    static decimal ParseCurrency(string currencyString)
    {
        // Remove leading currency symbols like $ and other spaces
        currencyString = currencyString.Replace("$", "").Trim();

        if (decimal.TryParse(currencyString, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal result))
        {
            return result;
        }
        else
        {
            throw new FormatException($"Invalid currency format: {currencyString}");
        }
    }

    static DateTime ValidateDateString(string? dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString))
        {
            throw new ArgumentException("Date string cannot be null or empty.");
        }

        if (!DateTime.TryParseExact(dateString, "M/d/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
        {
            throw new FormatException($"Invalid date format. Expected format is M/d/yy. Input was: {dateString}");
        }

        return parsedDate;
    }

    static decimal CalculateInterest(Dictionary<DateTime, decimal> entries, decimal interestRate, DateTime startDate, DateTime endDate)
    {
        decimal totalInterest = 0;
        decimal dailyRate = interestRate / 365;

        decimal lastBalance = entries.Count > 0 ? entries.First().Value : 0; // Start with the first entry's balance or 0 if none
        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        {
            if (entries.TryGetValue(date, out decimal currentBalance))
            {
                lastBalance = currentBalance;
            }

            totalInterest += lastBalance * dailyRate;
        }

        return totalInterest;
    }
}
