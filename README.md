# SolarWatch Project

SolarWatch is a full-stack web application designed to provide sunrise and sunset data for specific cities. The project includes a **C# ASP.NET Core backend**, **React.js frontend**, and is fully containerized using **Docker**. The backend uses **Entity Framework** with a database for data persistence.

## Features

- 🌅 Retrieve sunrise and sunset times for cities.
- 🌍 Manage city data (CRUD operations).
- 🛠 Authentication system with JWT support.
- 💻 Responsive React.js frontend for a seamless user experience.
- 🐳 Containerized with Docker for easy deployment.

---

## Tech Stack

### Backend

- **C#** with **ASP.NET Core** for the REST API.
- **Entity Framework Core** for database operations.
- **MSSQL**.
- **Authentication** using **JWT Tokens**.
- **Unit and Integration Tests** with NUnit and Moq.

### Frontend

- **React.js** with modern JavaScript (ES6+).
- **Axios** for API requests.
- Responsive design using **CSS**

### Tools

- **Docker** for containerization.
- **GitHub** for version control and repository hosting.

---

## Prerequisites

Before running the application, ensure the following are installed:

- [Docker](https://www.docker.com/)
- [Node.js](https://nodejs.org/)
- [.NET SDK](https://dotnet.microsoft.com/)

---

## Setup

### Clone the Repository

- git clone https://github.com/prvics/solarwatch-cc-project
- cd solarwatch-cc-project

### Backend

- cd backend

Restore dependencies and run migrations:

- dotnet restore
- dotnet ef database update

Run the API:

- dotnet run

### Frontend

- cd frontend

Install dependencies:

- npm i

Start the React application:

- npm start

---

### To run fully Dockerized

## Run

- cd Solarwatch

Run and Build the Containers:

- docker-compose up --build

- http://localhost:5173

## Stop

- cd Solarwatch

Stop the Containers:

- docker-compose down

## TroubleShoot

- cd Solarwatch

If something doesn’t work:

- Check logs: Use docker-compose logs to see what’s happening in the containers.

- Restart with clean build: Run:

docker-compose down -v
docker-compose up --build

---

### API Endpoints

## Authentication
-	POST /api/Auth/Register - Register a new user
-	POST /api/Auth/Login - Login and retrieve a token

- POST /api/Auth/Register - Register a new user
- POST /api/Auth/Login - Login and retrieve a token

## City Management

- GET /api/SolarWatch/cities - Get all cities
- GET /api/SolarWatch/cities/{id} - Get a city by ID
- POST /api/SolarWatch/cities - Create a new city
- PUT /api/SolarWatch/cities/{id} - Update an existing city
- DELETE /api/SolarWatch/cities/{id} - Delete a city

## Sunrise and Sunset Management

- GET /api/SolarWatch/sunrise-sunsets/{id} - Get sunrise and sunset data by ID
- POST /api/SolarWatch/sunrise-sunsets - Create new sunrise and sunset data
- PUT /api/SolarWatch/sunrise-sunsets/{id} - Update sunrise and sunset data
- DELETE /api/SolarWatch/sunrise-sunsets/{id} - Delete sunrise and sunset data
