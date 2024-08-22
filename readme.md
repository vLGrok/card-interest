# Interest Calculation Program

## Overview

This C# program calculates the total interest accrued over a year based on daily balances recorded in a CSV file. The interest is calculated using a fixed annual interest rate, with the balance for each day used to compute the daily interest. If no balance is recorded for a specific day, the balance from the most recent previous entry is used.

## Input Data

The program reads a CSV file named `card-data.csv` containing balance and date entries in the following format:

```
\"balance\",\"date\"
11573.13,1/1/2024
```

### Notes:
- Each line contains a balance followed by a date.
- If multiple entries exist for the same date, only the last entry is considered.
- The date format should be `MM/dd/yyyy`.

## Usage

1. **Prepare your CSV file:**
   - Ensure the file is named `card-data.csv` and is placed in the same directory as the executable.
   - The CSV should follow the format described above.

2. **Set the Interest Rate:**
   - The default annual interest rate is set to 20%. You can change this value in the code by updating the `interestRate` variable.

3. **Run the Program:**
   - The program will read the CSV file, calculate the daily interest, and output the total interest for the year.

4. **Output:**
   - The total interest for the year is displayed in the console in a currency format.

## Example

Suppose your `card-data.csv` file contains the following data:

```
11573.13,1/1/2024
12054.98,1/4/2024
```

Running the program will output the total interest accrued for the year 2024, calculated using the default 20% annual interest rate, which can be updated in the code.
