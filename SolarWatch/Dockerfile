# Use the .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# Set the working directory
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY /*.csproj ./
RUN dotnet restore

# Copy the entire project and build it
COPY / ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Set the environment variable for ASP.NET Core URLs
ENV ASPNETCORE_URLS=http://+:80

# Copy the published output from the build stage
COPY --from=build-env /app/out ./

# Command to run the application
ENTRYPOINT ["dotnet", "SolarWatch.dll"]