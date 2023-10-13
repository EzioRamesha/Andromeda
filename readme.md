## .Net Framework 4.8 (Entity Frame - CodeFirst)

## Prerequisites

* Windows 7 or above.
* [Microsoft SQL Server 2019 Developer Version](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
* [Visual Studio 2019 Community Version](https://visualstudio.microsoft.com/vs/)
    * Install components via Visual Studio Installer (Modify)
        * ASP.NET and web development
        * Data storage and processing
        * .NET desktop development
        * Universal Windows Platform development
        * .NET Core cross-platform development
* [.Net Framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)
* [Entity Framework 6.0 (Code-First)](https://www.entityframeworktutorial.net/code-first/setup-entity-framework-code-first-environment.aspx)

## Configuration

1. Copy `WebApp/Web.Sample.config` and paste as `WebApp/Web.config`
2. Config the database connection details at `<connectionStrings>` tag
3. Copy `WebApp/Views/Web.Sample.config` and paste as `WebApp/Views/Web.config`
4. Copy `ConsoleApp/App.Sample.config` and paste as `ConsoleApp/App.config`

## Initial Database
1. Open `Tools` > `NuGet Package Manager` > `Package Manager Console`
2. Execute command `update-database` to run migrations
