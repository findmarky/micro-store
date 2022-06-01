#Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "Inventory.API.csproj" --disable-parallel
RUN dotnet publish "Inventory.API.csproj" -c release -o /app --no-restore

#Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./

EXPOSE 7002

ENTRYPOINT ["dotnet", "Inventory.API.dll"]