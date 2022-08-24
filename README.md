# TaskTracker.Api

TaskTracker is ASP.NET Core WebAPI project. .NET6 version

## Features

- Ability to create / view / edit / delete information about projects
- Ability to create / view / edit / delete task information
- Ability to add and remove tasks from a project (one project can contain several tasks)
- Ability to view all tasks in the project

## Installation

Project requires [.NET](https://dotnet.microsoft.com/en-us/) v6+ to run.

Install the dependencies and start the server using Docker

```sh
git clone https://github.com/effuone/TaskTracker.Api.git
cd TaskTracker
touch .env
```
To run this project, you will need to add the following environment variables to your .env file

`DB_USER=SA`

`DB_PASSWORD=[YourPassword]`

`DB_PORT=1433`

`DB_DATABASE=TaskTrackerDb`

Run project 
```sh
docker-compose up
```