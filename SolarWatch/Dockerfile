FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build
WORKDIR /app
COPY Backend/*.csproj ./
RUN dotnet restore
COPY Backend/ ./
RUN dotnet publish -c Release -o out
NEW
FROM node:18 AS frontend-build
WORKDIR /frontend
COPY Frontend/package*.json ./
RUN npm install
COPY Frontend/ .
RUN npm run build
NEW
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80

# Copy backend files
COPY --from=backend-build /app/out ./

# Copy frontend files
COPY --from=frontend-build /frontend/dist ./wwwroot

# Command to run the application
ENTRYPOINT ["dotnet", "SolarWatch.dll"]