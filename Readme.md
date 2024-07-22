# Payslip Generator Coding Exercise
This is a coding exercise to generate a payslip given an employee's details. Given their annual salary
 and Super contribution rate, it calculates a month's:
- Pay period
- Gross Income
- Income Tax
- Net Income
- Super

It is hosted in an aspnetcore web application with a simple MVC view form to generate a payslip, or an api
 discoverable through the `/swagger` swagger ui

## Run Instructions
- `cd` into the `CodingExercise.Api` directory
- run the `dotnet run` command
- Navigate to either:
  - http://localhost:5179 for the web interface into the generator
  - http://localhost:5179/swagger for the swagger interface

## Assumptions
- Application is built as an aspnetcore application instead of a simple console app so that it can be scaled up
to have more features/be accessible over the web
- The application could be expanded to read employee details from a database. 
To do this, another project would be added e.g. `CodingExercise.Persistence`. Interfaces would be created in the 
`CodingExercise.Domain.Services` project, then implemented in the persistence project. They would then be injected
into the relevant domain service
- Because of leap years, I read in the current year, and use that to calculate the payslip's pay period. Alternatively
A field could be added to accept the current year through the form/api
- The main purpose of the exercise is to create a backend service, so the web ui is very basic
- I'm using a static list of tax brackets. These could be persisted to a database or a config provider so that when tax rates change, a
re-deploy wouldn't be needed. Possibly even having an associated applicable date range so that historically generated
payslips would still be valid
- Https redirection is disabled for easy running locally, but this should be turned on if actually deploying the app
