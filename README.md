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

### Authentication
| Method | Endpoint | Description |
| --- | ---- | ---- |
| POST  | /api/auth/register  |  Create new account |
| POST  | /api/auth/login     |  Login user |
| GET   | /api/auth/profile   |  Get user profile (requires JWT) |

### Book

### USER


## 🧪 Testing
```bash
# Run unit tests
dotnet test Folio.Tests
```
## 📝 What I Learned
- JWT authentication & authorization
- Clean Architecture patterns
- EF Core + database migrations
- Vanilla JavaScript HTTP requests
- REST API design principles

## ⚠️ Known Issues
[ ] WebAssembly frontend not fully implemented
[ ] Email verification pending
[ ] Rate limiting on API endpoints

##🔮 Future Improvements
[ ] Implement 2FA
[ ] Add real-time notifications (SignalR)
[ ] ASP.NET MAUI Blazor hyberid APP and webassembly + Viue.js

## 📧 Contact
- Email: karol@example.com
- GitHub: github.com/Karol8284
- LinkedIn: linkedin.com/in/karol8284


## 📄 License
MIT License - feel free to use for learning
```bash
# Usuń typo folder
rm -rf Folio.Infrastucture

# Usuń duplicate sln
rm fOLIO.slnx

# Commit
git add .
git commit -m "fix: Remove duplicate folders and files"
git push
```
