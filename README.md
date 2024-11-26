# FoodRegistrationTool

Home Exam - ITPE3200 Web Applications.

## About

The Food registration Tool is a web application that allows food producers to register their products, stating product name, image, price, description and nutritional data in a centralized system. The application is based on Asp.Net Core Mvc with Entity Framework databse management. It supports creating, editing and deleting of both producers and products, and requires the user to be logged in to perform said actions. Registering users and updating of user data are also features eneabled in the application.

## Features

- Register and manage food products and its data and nutritional values.
- Associate products with producers.
- Responsive design using Bootstrap.

## Project Structure

- FoodRegistrationTool: Main Asp.Net Core MVC application.
- DAL: Data Access Layer containing ProductDbContext and repositories.
- Areas/Identity: Contains the Identity setup for user authentication.
- Logs: Contains the log files from application runtime.
- FoodRegistrationTool.Tests: Unit tests for the application.

## Running Application

1. Clone repository.
2. Start application from **FoodRegistrationTool** directory:
   `dotnet run`.

## Running Tests

1. Clone repository.
2. Start testing from **FoodRegistrationTool.Tests** directory:
   `dotnet test`.

# Miscellaneous

Etter å ha lagt til nye klasser under Controller og Models-mappene, utfør følgende kommandoer i terminalen for å unngå feilmeldinger under build og for å holde migrations og DB oppdatert:

`dotnet ef migrations add FoodRegistryDbExpanded + tall`

`dotnet ef database update`
