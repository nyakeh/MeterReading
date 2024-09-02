# MeterReader Solution

## Overview

The MeterReader solution is designed to manage and process smart meter readings. 
It includes services for reading, validating, and storing electricity smart meter readings, as well as retrieving account and historical meter reading data. 
The solution is built using C# .NET Core Web API's with NUnit & Moq for unit testing.

## Current Capabilities
- Upload Meter Readings file: Validates the data, and stores valid readings in the repository. Invalid readings are recorded with reasons for failure.
- Retrieve all meter readings and all accounts.
- Retrieve meter reading histery per account.
- Unit tests on business logic.
- Bulk delete of meter readings from the repository, to enable re-testing using the same data files.


## API usage
- API's documented using Swagger when running application locally.
- I also icluded a Postman collection in the repository 'Ensek - Meter Reader.postman_collection.json'.

## Future improvements

- Improve validation: Support different date formats
- UI to display data
- File upload GUI
- Duplicate account meter read update doesnt detail row not used
- End-to-end tests on  the controller methods

## Project Structure

- **MeterReader.Models**: Contains the data models used throughout the application.
- **MeterReader.Data**: Defines the repository interfaces and their implementations.
- **MeterReader.Services**: Contains the business logic for processing meter readings.
- **MeterReader.Tests**: Unit tests for the services.

