using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

class Program
{
    static void Main()
    {
        var entries = new Dictionary<DateTime, decimal>();

        string filePath = "card-data.csv"; // Path to your text file
        string[] lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length == 2)
            {
                decimal balance = decimal.Parse(parts[0].Trim('\"'), CultureInfo.InvariantCulture);
                DateTime date = DateTime.Parse(parts[1].Trim('\"'), CultureInfo.InvariantCulture);
                entries[date] = balance; // This will overwrite any existing entry for the date
            }
        }

        decimal interestRate = 0.20M; // Example interest rate (5%)
        decimal totalInterest = CalculateInterest(entries, interestRate);

        Console.WriteLine($"Total Interest for the year: {totalInterest:C}");
    }

    static decimal CalculateInterest(Dictionary<DateTime, decimal> entries, decimal interestRate)
    {
        decimal totalInterest = 0;
        decimal dailyRate = interestRate / 365;
        DateTime startDate = new DateTime(2023, 1, 1);
        DateTime endDate = new DateTime(2023, 12, 31);

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
