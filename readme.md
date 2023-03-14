# Reporting application

This application is generated using Visual Studio 2022 and developed using ASP.NET Core Web. This report application generates reports on product sales and distribution of product shipping destinations.

## Development and runtime environment

Before you can build this project, you must install and configure the following dependencies on your machine:

[Development]: We use Visual Studio 2022 to develop and build the project.
Please visit https://visualstudio.microsoft.com/downloads to download Visual Studio 2022 

[Runtime]: We use ASP.Net Core 6.0 Runtime to run this project.
Please visit https://dotnet.microsoft.com/download to download ASP.Net Core Runtime 6.0

## Database and migration script

This project uses mysql database and generates database tables in a code-first way.
Or use the Initial-data.sql initialization script or run the migration command under the PM control of the ProductWebApi project

## Configuration

Database connection, Email, Job timing expression and WebApi Url are in appsettings.json of the corresponding project

## Complie and Deploy

To compile and deploy this application, execute the following commands in order:

    cd ProductWebAPI
    dotnet publish -c Release -o Out/ProductWebAPI

    cd Out/ProductWebAPI/

    dotnet productwebapi.dll --urls="https://localhost:7101;http://localhost:7100"

    cd..
    cd..
    cd..
    cd ReportingApplication
    dotnet publish -c Release -o Out/ReportingApplication

    cd Out/ReportingApplication

    dotnet ReportingApplication.dll --urls="https://localhost:8101;http://localhost:8100"


## Multi-environment support

This application supports Development and Product environments. For the development environment, please add parameters at runtime, such as:

    dotnet productwebapi.dll --urls="https://localhost:7001;http://localhost:7000" --environment=Development
    dotnet ReportingApplication.dll --urls="https://localhost:8001;http://localhost:8000" --environment=Development


## Testing

Please use the dotnet test command to run all test cases in the test project, such as:

   dotnet test # ReportingApplication
