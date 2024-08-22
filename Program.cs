using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Verify command line arguments
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: Program <interest_rate> <file_path>");
            return;
        }

        // Parse and verify interest rate
        if (!decimal.TryParse(args[0], NumberStyles.Float, CultureInfo.InvariantCulture, out decimal interestRate) || interestRate <= 0)
        {
            Console.WriteLine("Error: Invalid interest rate. Please provide a positive decimal value (e.g., 0.21 for 21%).");
            return;
        }

        // Verify file exists
        string filePath = args[1];
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: The file '{filePath}' does not exist.");
            return;
        }

        // Ask the user for the start and end dates
        DateTime startDate, endDate;
        
        while (true)
        {
            Console.Write("Please enter the start date (MM/dd/yyyy): ");
            string? startDateInput = Console.ReadLine();
            try
            {
                startDate = ValidateDateString(startDateInput);
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        while (true)
        {
            Console.Write("Please enter the end date (MM/dd/yyyy): ");
            string? endDateInput = Console.ReadLine();
            try
            {
                endDate = ValidateDateString(endDateInput);

                // Ensure start date is before or equal to end date
                if (startDate > endDate)
                {
                    Console.WriteLine("Error: Start date must be before or equal to the end date.");
                    continue;
                }
                
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Read and process the file
        var entries = new Dictionary<DateTime, decimal>();
        string[] lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length == 2)
            {
                decimal balance = decimal.Parse(parts[0].Trim('\"'), CultureInfo.InvariantCulture); // Parse balance first
                DateTime date = DateTime.Parse(parts[1].Trim('\"'), CultureInfo.InvariantCulture); // Parse date second
                entries[date] = balance; // This will overwrite any existing entry for the date
            }
        }

        // Calculate and display the total interest
        decimal totalInterest = CalculateInterest(entries, interestRate, startDate, endDate);
        Console.WriteLine($"Total Interest for the period {startDate.ToShortDateString()} to {endDate.ToShortDateString()}: {totalInterest:C}");
    }

    static DateTime ValidateDateString(string? dateString)
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
