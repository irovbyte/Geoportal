FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Geoportal.Api.csproj", "./"]
RUN dotnet restore "Geoportal.Api.csproj"
COPY . .
RUN dotnet publish "Geoportal.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Geoportal.Api.dll"]