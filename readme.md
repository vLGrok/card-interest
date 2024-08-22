# Interest Calculation Program

## Overview

This C# program calculates the total interest accrued over a specified period based on daily balances recorded in a CSV file. The interest is calculated using a fixed annual interest rate, with the balance for each day used to compute the daily interest. If no balance is recorded for a specific day, the balance from the most recent previous entry is used.

## Input Data

The program reads a CSV file containing balance and date entries in the following format:

```
\"balance\",\"date\"
15249.31,\"1/1/2024\"
15979.31,\"1/1/2024\"
15059.31,\"1/5/2024\"
15559.41,\"1/9/2024\"
```

### Notes:
- Each line contains a balance followed by a date.
- If multiple entries exist for the same date, only the last entry is considered.
- The date format should be `M/d/yy`, which allows for dates with or without leading zeros, such as `1/1/24` or `01/01/24`.

## Usage

1. **Run the Program:**
   - The program is executed with the following command-line arguments:
     - **Interest Rate**: A decimal value representing the annual interest rate (e.g., `0.21` for 21%).
     - **File Path**: The path to the CSV file containing the balance data.
   - Example command:

     ```
     dotnet run 0.21 card-data.csv
     ```

2. **Date Range Input:**
   - After running the program, you will be prompted to input a start date and an end date in the `M/d/yy` format.
   - The program will calculate the interest over this specified date range.

3. **Output:**
   - The total interest for the specified period is displayed in the console in a currency format.

## Example

Suppose your `card-data.csv` file contains the following data:

```
\"15249.31\",\"1/1/24\"
\"15979.31\",\"1/1/24\"
\"15059.31\",\"1/5/24\"
\"15559.41\",\"1/9/24\"
```

Running the program with the following command:

```
dotnet run 0.21 card-data.csv
```

You will then be prompted to enter the start and end dates for the period over which you want to calculate interest.

For example, if you input `1/1/24` as the start date and `1/9/24` as the end date, the program will output the total interest accrued for that period using a 21% annual interest rate.

