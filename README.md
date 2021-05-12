

<!-- ABOUT THE PROJECT -->
## About The Project
This is a CQRS reference application using ASP.NET Core MVC, MediatR, EF Core, SQL Server.
  <!--
  <br />
    <a target="_blank" href="https://www.srinadimpalli.com/2021/05/vertical-slice-architecture-using-net5-cqrs-mediatr-ef-core-c/"><strong>Explore the docs »</strong></a>
    -->
    
### Built With
* Visual Studio 2019
* ASP.NET Core 3.1
* [MediatR](https://www.nuget.org/packages/MediatR/)
* Entity Framework Core
* SQL Server

### Database Configuration 
You can create database using Database First or Code First approach.
### Database first approach.
* Copy the Sql database script from "CreateDB.txt" under the solution level and create a database named "CQRS" in your Sql Server instance and update the connection string with your sql server name.
### Code First
* Navigate to the AspNetCoreFactory.Domain directory and open the command prompt and follow the instruction listed in "ReverseEngineerDb.txt" file.

### Featurers:
* Flat Areas: Flat areas have no subfolders like [Models, Controllers, Views]. All files in a module reside in the same folder.
* Modified from https://github.com/OdeToCode/AddFeatureFolders
```
.AddRazorOptions(o =>
            {
                // Accommodates the '_Base' folder name

                o.ViewLocationFormats.Clear();
                o.ViewLocationFormats.Add(options.AreaPlaceholder + @"\{0}.cshtml");
                o.ViewLocationFormats.Add(options.RootFolderName + @"\_Base\{0}.cshtml");
                o.ViewLocationExpanders.Add(expander);
            });
```           
* MediatR: MediatR separates messages into queries and commands.
* Event Sourcing pattern: (partially implemented), look for Event table.Event class logging Event Source records.
* Record Name: Is a human-readable field that easily identifies a record.
* Lookups: Lookups are table-driven dropdown [List and Edit pages].
* Caching: in-memory data store, caching removes the need for table joins and simplifies data and increases performance.
* Roll-ups: Rolup performs re-calculation of the rollup fields in the database[example: TotalBookings]. It does this by issuing raw SQL to the database.    code path [..code/database/Rollup.cs]

### Usage
<p align="left">
      <img src="https://github.com/srinadimpalli/CQRS-using-ASP.NET-Core-MVC/blob/master/AspNetCoreFactory.CQRS.Core/wwwroot/images/cqrs.jpg" alt="usage">
</P
</br>
<p align="left">
      <img src="https://github.com/srinadimpalli/CQRS-using-ASP.NET-Core-MVC/blob/master/AspNetCoreFactory.CQRS.Core/wwwroot/images/Modules_VSlice.PNG" alt="modules">
</P

## Author

[Raju Nadimpalli](https://srinadimpalli.com) - raju@srinadimpalli.com

