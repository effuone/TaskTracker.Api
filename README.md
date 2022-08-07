# TaskTracker.Api

TaskTracker is ASP.NET Core WebAPI project. .NET6 version
## Installation

Project requires [.NET](https://dotnet.microsoft.com/en-us/) v6+ to run.

Install the dependencies and devDependencies and start the server.

```sh
git clone https://github.com/effuone/TaskTracker.Api.git
cd TaskTracker.Api
```
Add connection string to your database.

For Azure Data Studio or linux driven MSSQL
```sh
dotnet user-secrets set ConnectionStrings:TaskTrackerDb "Server=tcp:localhost;Database=TaskTrackerDb;User Id=[yourusename];Password=[yourpassword]"
```
For Windows SSMS (without authorization)
```sh
dotnet user-secrets set ConnectionStrings:TaskTrackerDb "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TaskTrackerDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
```

Revert migrations in databasse 
```sh
dotnet ef database update
```

Run project 
```sh
dotnet run
```
