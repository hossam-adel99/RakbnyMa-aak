# 🚖 Rakbny Ma'ak  

**Rakbny Ma'ak** is the **backend part** of an **ITI graduation project** — a modern ride-sharing system built with **ASP.NET Core Web API**.  
It connects **drivers** and **passengers**, providing **secure authentication, trip management, real-time chat, live tracking, and background services**.  

---

## ✨ Features  
- 🔑 **Authentication & Authorization** (Identity + JWT)  
- 📧 **Email Confirmation** for new users  
- 🚗 **Trip Management** (create, book, update, cancel trips)  
- 💬 **Real-time Chat** using SignalR  
- 📍 **Live Trip Tracking** with driver location updates  
- ⚙️ **Seed Data** for initial setup (roles, users, etc.)  
- ⏳ **Background Jobs** with Hangfire (trip reminders, cleanup tasks)  
- 🔒 Designed with **security, scalability, and performance** in mind  

---

## 🛠 Tech Stack  
- **Backend:** ASP.NET Core Web API, C#  
- **Database:** SQL Server, Entity Framework Core (Migrations + Seed Data)  
- **Authentication & Security:** Identity, JWT Authentication, Email Confirmation  
- **Real-time Communication:** SignalR  
- **Background Jobs:** Hangfire  
- **Architecture:** Repository Pattern, Unit of Work, CQRS, Clean Architecture  

---

## 🚀 Getting Started  

### ✅ Prerequisites  
- .NET 7 SDK or later  
- SQL Server  
- Visual Studio / VS Code  

### ⚡ Installation  
1. Clone the repository:  
   ```bash
   git clone https://github.com/hossam-adel99/RakbnyMa-aak.git
   cd Rakbny-Maak
   ```  

2. Update the **connection string** in `appsettings.json`.  

3. Apply migrations & seed data:  
   ```bash
   dotnet ef database update
   ```  

4. Run the application:  
   ```bash
   dotnet run
   ```  

---

## 📚 API Documentation  
The project includes **Swagger UI** for interactive API testing.  
Once running, navigate to:  

👉 [Swagger Documentation](https://rakbny-api.runasp.net/swagger/index.html)  

---

## ⚙️ Background Jobs (Hangfire)  
- 🧹 Automatic cleanup of expired trips  
- 📩 Sending trip reminders via email  
- 🔔 Background notifications  

---

## 👨‍💻 Contributors  
- **Hossam Adel** – .NET Backend Developer  
