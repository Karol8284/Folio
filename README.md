# 📚 Folio - Room Booking System

## 🎯 What is this?
A full-stack application for booking and managing rooms/spaces with user authentication and real-time availability.

## 🏗️ Tech Stack
- **Backend**: C# 11, ASP.NET Core 7, Entity Framework Core, SQL Server
- **Frontend**: HTML5, CSS3, Vanilla JavaScript
- **Auth**: JWT (JSON Web Tokens)
- **Architecture**: Clean Architecture, Repository Pattern

## 🎨 Project Structure
## 🏗️ Folio Tech Stack


### Folio.CORE/ 
  - Business logic & entities
### Folio.Infrastructure/
 - Database & external services
### Folio.Shared/
 - Shared models & DTOs
### Folio.API/
-  REST API endpoints

### Folio.WebAssembly/
 - Frontend ASP.NET Blazor webasembly
### Folio.Tests/
 - Unit & Integration tests

## ✨ Features
- ✅ User authentication (JWT)
- ✅ Room CRUD operations
- ✅ Booking management
- ✅ Real-time availability
- ✅ Responsive UI

## 🚀 Quick Start

### Prerequisites
- .NET 7 SDK
- SQL Server (or LocalDB)
- Modern web browser

### Installation

```bash
# Clone repo
git clone https://github.com/Karol8284/Folio
cd Folio

# Restore & build
dotnet restore
dotnet build

# Setup database
dotnet ef database update -p Folio.Infrastructure -s Folio.API

# Run API
cd Folio.API
dotnet run
```
API runs on: https://localhost:7001

Frontend
Open Folio.WebAssembly/index.html in your browser

## API Endpoints
| Method | Endpoint | Description |
| --- | ---- | ---- |

