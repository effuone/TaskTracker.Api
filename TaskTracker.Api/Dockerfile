FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["TaskTracker.Api/TaskTracker.Data.csproj", "TaskTracker.Data/"]
COPY ["TaskTracker.Api/TaskTracker.Domain.csproj", "TaskTracker.Domain/"]
COPY ["TaskTracker.Api/TaskTracker.Api.csproj", "TaskTracker.Api/"]
RUN dotnet restore "TaskTracker.Api/TaskTracker.Api.csproj"
COPY . .
WORKDIR "/src/TaskTracker.Api"
RUN dotnet build "TaskTracker.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskTracker.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskTracker.Api.dll"]