# PriceAggregator

# Install

- Download and Install Microsoft .Net SDK 7.0

# After downloading the solution, enter inside directory "PriceAggregator.Api"

# To start application run the following command:

- dotnet run --environment production
  
# To get the price for 01/01/2023 23:00:00 from BTC/USD access the following link:

- https://localhost:8765/api/Prices?Year=2023&Month=1&Day=1&Hour=23&convertFrom=BTC&convertTo=USD

# To get the prices between 01/07/2022 00:00:00 and 01/12/2023 23:00:00 from BTC/USD access the following link: 

- https://localhost:8765/api/Prices/getAllPricesBetweenTwoDates?StartYear=2022&StartMonth=07&StartDay=01&StartHour=0&EndYear=2023&EndMonth=12&EndDay=01&EndHour=23&convertFrom=BTC&convertTo=USD

# To see the logs navigate to C:\logs directory.

# Supported currencies :
    -USD,
    -EUR,
    -BTC,
    -ETH
