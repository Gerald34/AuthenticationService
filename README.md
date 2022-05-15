# AuthenticationService
Authentication service for admin users

## Installation

Download and Install the [Microsoft .NET Core 6](https://dotnet.microsoft.com/en-us/download)



Clone project [GitHub](https://github.com/Gerald34/AuthenticationService.git) and run the following command to install dependencies.

```bash
dotnet restore
```

Edit the appsettings.json file and add your local database credentials.

Used database: [SqlServer](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
```json
{
  "ConnectionStrings": {
    "UserDatabase": "Server=xxxxx;Database=xxxxx;User Id=xxxxx; Password=xxxxx;"
  }
}
```
  
Run Migration and update database context
```bash
dotnet ef migrations add Initial
```

Build project
```bash
dotnet watch run
```
## Author
Gerald Mathabela

Founder: [Labworx Studios](https://www.labworxstudios.com)
