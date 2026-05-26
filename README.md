# 📚 Fullstack Upskilling Journey: .NET 9 + Angular 21+

> A comprehensive 8-week learning program transitioning from .NET 4.8/Angular 12 (2020) to modern fullstack development with latest frameworks, clean architecture, and security best practices.

## 🎯 Goal

Prepare for a senior fullstack role with hands-on expertise in:
- **Frontend**: Angular 21+ (Signals, Standalone Components, RxJS, NgRx)
- **Backend**: .NET 9 (ASP.NET Core, Clean Architecture, SOLID)
- **DevOps**: Docker, CI/CD, Azure deployment
- **Security**: Azure AD, MSAL, XSS prevention, CORS, token handling

**Timeline**: 2 months | **Effort**: 2 hours/day | **Approach**: Parallel frontend + backend learning

---

## 📋 Learning Plan Overview

### **Week 1-2: Fundamentals & Setup** 
- .NET 9 what's new vs 4.8
- Angular 21 new features (Signals, Standalone Components)
- Tool setup (Visual Studio, VS Code, Angular CLI)
- Azure SQL Database setup
- Git workflow refresh

### **Week 3: REST APIs & C# Fundamentals**
- REST API design best practices
- Entity Framework Core 8
- Dependency Injection in .NET
- Clean Architecture layers (API, Application, Domain, Infrastructure)

### **Week 4: Advanced C# & Design Patterns**
- SOLID principles deep dive
- Common design patterns (Factory, Strategy, Decorator, Observer)
- Async/await best practices
- Error handling & structured logging

### **Week 5: Security & Authentication**
- Authentication vs Authorization
- Azure AD / MSAL integration
- Bearer tokens & JWT basics
- CORS, CSRF, secure headers
- xUnit testing + Moq mocking

### **Week 6: RxJS & Reactive Programming (Angular)**
- Observables vs Promises
- Common RxJS operators
- Memory leak prevention (takeUntil, async pipe)
- HttpClient interceptors for auth
- Error handling in streams

### **Week 7: Signals & Standalone Components (Angular)**
- Signal() and Computed signals
- Effect() lifecycle
- Standalone component APIs
- OnPush change detection strategy
- Testing with Signals

### **Week 8: NgRx & State Management**
- Actions, Reducers, Effects
- Feature store architecture
- Entity adapters
- Vitest unit testing
- Store selectors + memoization

### **Week 9: Frontend Security & Advanced CSS**
- XSS prevention (DomSanitizer, CSP)
- Token secure storage
- SCSS preprocessor & theming
- Responsive design (mobile-first)
- Accessibility (A11y)

### **Week 10: DevOps & Deployment**
- Dockerfile for .NET API
- Dockerfile for Angular app
- Docker Compose orchestration
- Azure DevOps CI/CD pipelines
- Deployment to Azure App Service

### **Week 11-12: Practice & Interview Prep**
- Code review challenges
- Performance optimization
- Edge case handling
- System design discussions
- Application polishing & documentation

---

## 🚀 Quick Start

### Prerequisites
- ✅ Node.js 22 LTS (for Angular)
- ✅ .NET SDK 9.0+ (download from microsoft.com)
- ✅ Visual Studio 2024 or VS Code
- ✅ Git
- ✅ Azure subscription (free tier)
- ✅ SQL Server (local OR Azure SQL Database - recommended)

### Local Development Setup

#### 1. Clone & Install Dependencies

```bash
# Backend
cd Backend
dotnet restore

# Frontend
cd ../Frontend
npm install
```

#### 2. Database Setup (Azure SQL or Local)

> ⚠️ **Important:** Connection strings live in **User Secrets** (dev only) — never in `appsettings.json` (which is committed to git). `appsettings.json` keeps `DefaultConnection` empty by design.

**Option A: Azure SQL Database (Recommended)**

1. Create Azure SQL Database in Azure Portal (free tier OK).
2. Configure Azure SQL firewall to allow your client IP.
3. Set the connection string in User Secrets:
   ```bash
   cd Backend/src/TaskManager.API
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=tcp:YOUR-SERVER.database.windows.net,1433;Initial Catalog=TaskManagerDb;User ID=YOUR-USER;Password=YOUR-PASSWORD;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
   ```
   (`UserSecretsId` is already set in `TaskManager.API.csproj` — no need to run `dotnet user-secrets init`.)

**Option B: Local SQL Server Express**

1. Install SQL Server Express (free) from Microsoft.
2. Set the connection string in User Secrets:
   ```bash
   cd Backend/src/TaskManager.API
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=TaskManagerDb;Integrated Security=true;TrustServerCertificate=True;"
   ```

**Apply migrations** (creates tables in the chosen DB):
```bash
cd Backend
dotnet ef database update --project src/TaskManager.Infrastructure --startup-project src/TaskManager.API
```

#### 3. Run Locally

```bash
# Backend (Terminal 1)
cd Backend
dotnet watch run --project src/TaskManager.API

# Frontend (Terminal 2)
cd Frontend
npm start
```

**Backend API**: http://localhost:5250 (Swagger UI: http://localhost:5250/swagger)
**Frontend**: http://localhost:4200

#### 4. Docker Compose (All-in-one)

```bash
docker-compose up
# API: http://localhost:5000
# Frontend: http://localhost:4200
# Database: localhost:1433
```

---

## 📁 Project Structure

```
upskilling/
├── Backend/
│   ├── src/
│   │   ├── TaskManager.API/          # Controllers, Program.cs, appsettings
│   │   ├── TaskManager.Application/  # Services, DTOs, Interfaces
│   │   ├── TaskManager.Domain/       # Entities, Value Objects, Aggregates
│   │   └── TaskManager.Infrastructure/ # EF Core, Repositories, DbContext
│   ├── tests/
│   │   └── TaskManager.Tests/        # xUnit + Moq tests
│   ├── TaskManager.sln
│   ├── Dockerfile
│   └── .dockerignore
│
├── Frontend/
│   ├── src/
│   │   ├── app/
│   │   │   ├── core/               # Singleton services (auth, api)
│   │   │   ├── shared/             # Reusable components, pipes, directives
│   │   │   ├── features/           # Feature modules (tasks, projects)
│   │   │   └── store/              # NgRx store (actions, reducers, effects)
│   │   ├── assets/
│   │   ├── styles/                 # Global SCSS
│   │   └── main.ts
│   ├── public/
│   ├── package.json
│   ├── angular.json
│   ├── Dockerfile
│   └── .dockerignore
│
├── docker-compose.yml
├── .gitignore
└── README.md (this file)
```

---

## 🏗️ Architecture Explanation

### Backend: Clean Architecture Layers

```
         ┌─────────────────────┐
         │   TaskManager.API   │  ← Controllers, DI setup, middleware
         └──────────┬──────────┘
                    │
         ┌──────────▼──────────┐
         │   Application       │  ← Business logic, DTOs, interfaces
         └──────────┬──────────┘
                    │
      ┌─────────────┼─────────────┐
      │             │             │
   ┌──▼──┐    ┌──────▼──────┐ ┌──▼──────┐
   │Domain│    │Infrastructure│ │External│
   └──────┘    └──────────────┘ └────────┘
   (Entities)  (EF Core, Repos) (APIs)
```

**Why this structure?**
- **Separation of Concerns**: Each layer has single responsibility
- **Testability**: Core logic isolated from infrastructure
- **Maintainability**: Easy to locate & modify features
- **Flexibility**: Switch DB, API, authentication without touching business logic

### Frontend: Angular Architecture

```
         ┌──────────────────────┐
         │   Components/Pages   │  ← UI, user interaction
         └──────────┬───────────┘
                    │
      ┌─────────────┼─────────────┐
      │             │             │
   ┌──▼──┐    ┌──────▼──────┐ ┌──▼──────┐
   │Store │    │  Services  │ │  Guards │
   └──────┘    └────────────┘ └────────┘
   (NgRx)    (API, Auth)    (Route protection)
```

**Why this?**
- **Store (NgRx)**: Single source of truth, time-travel debugging
- **Services**: Reusable business logic, dependency injection
- **Guards**: Route protection, auth verification

---

## 🔑 Key Concepts You'll Learn

### C# / .NET
- ✅ Async/await patterns
- ✅ LINQ query optimization
- ✅ Dependency Injection container
- ✅ Middleware pipeline
- ✅ Entity Framework Core migrations
- ✅ SOLID principles in practice

### TypeScript / Angular
- ✅ Reactive programming (RxJS)
- ✅ Signals (Angular 17+)
- ✅ Change detection optimization
- ✅ State management (NgRx)
- ✅ Component composition
- ✅ Typed forms (Angular 14+)

### Security
- ✅ Authentication (Azure AD, MSAL)
- ✅ Authorization (role-based, policy-based)
- ✅ XSS prevention
- ✅ CSRF protection
- ✅ Secure token storage
- ✅ CORS configuration

### Testing
- **Backend**: xUnit, Moq (unit + integration)
- **Frontend**: Vitest, Testing Library (unit + component)

---

## 📅 Weekly Tasks

See [WEEKLY_TASKS.md](./WEEKLY_TASKS.md) for detailed tasks per week.

After completing this file, create GitHub Issues from the tasks:

```bash
# Example: Create issue for Week 1
gh issue create --title "Week 1: Fundamentals & Setup" --body "$(cat <<'EOF'
- [ ] Research .NET 9 new features
- [ ] Setup Visual Studio 2024 / VS Code
- [ ] Create Azure SQL Database
- [ ] Learn Angular Signals basics
EOF
)"
```

---

## 🧪 Testing Strategy

### Backend (xUnit + Moq)
```csharp
[Fact]
public async Task CreateProject_WithValidData_ReturnsOk()
{
    // Arrange
    var service = new ProjectService(_mockRepo.Object);
    var command = new CreateProjectCommand { Name = "Test" };

    // Act
    var result = await service.CreateAsync(command);

    // Assert
    Assert.NotNull(result);
}
```

### Frontend (Vitest)
```typescript
describe('TaskService', () => {
  it('should fetch tasks from API', async () => {
    const tasks = await service.getTasks();
    expect(tasks).toHaveLength(2);
  });
});
```

---

## 🔐 Security Checklist

By end of Week 5:
- [ ] API uses [Authorize] attributes
- [ ] CORS properly configured
- [ ] No secrets in appsettings.json (use User Secrets in dev)
- [ ] Structured logging in place
- [ ] Exception handling middleware
- [ ] Input validation on all endpoints

By end of Week 9 (Frontend):
- [ ] Auth tokens in sessionStorage (not localStorage)
- [ ] DomSanitizer used for user content
- [ ] Content Security Policy headers
- [ ] HTTPS enforced
- [ ] No sensitive data in localStorage

---

## 📊 Progress Tracking

Use GitHub Projects or Azure DevOps to track:
1. Create a **"Learning Progress"** board
2. Add columns: **To Do**, **In Progress**, **Code Review**, **Done**
3. Move tasks as you complete weeks

Example tasks:
```
- Week 1: Setup & Fundamentals
- Week 3: REST APIs & EF Core
- Week 6: RxJS Deep Dive
- etc.
```

---

## 🎓 Resources

You'll get detailed notes on each topic. Key areas:

### Backend Deep Dives
- Clean Architecture (Uncle Bob)
- SOLID Principles applied
- EF Core query patterns
- Async/await gotchas
- .NET 9 features

### Frontend Deep Dives
- RxJS marble diagrams
- Signals vs Observables
- NgRx entity normalizers
- Change detection strategies
- Accessibility practices

---

## ❓ FAQ

**Q: Should I focus on backend or frontend first?**  
A: Both in parallel (Week 3-8 alternates). This keeps both skills fresh.

**Q: Can I skip Docker/DevOps (Week 10)?**  
A: Not ideal. Docker is expected nowadays. It's only 1 week though.

**Q: What if I'm stuck on a topic?**  
A: Ask Claude (me). We'll go deeper with examples.

**Q: When should I start applying for roles?**  
A: Aim for middle of Week 8 with a completed, polished project.

**Q: Should I use .NET 9 or 8?**  
A: .NET 9 (latest LTS). Download from microsoft.com if not installed.

---

## 🚀 Next Steps

1. ✅ Read this README thoroughly
2. ⬜ Install .NET 9 SDK (if using .NET 8 now)
3. ⬜ Create GitHub repo & push this skeleton
4. ⬜ Setup Azure SQL Database
5. ⬜ Start Week 1 (reading materials from Claude)
6. ⬜ Create GitHub Issues from WEEKLY_TASKS.md

---

## 📝 Notes

- This is a **learning journey**, not a production project
- Code quality matters - refactor as you learn new patterns
- Tests are mandatory - write them as you code
- Ask questions - understanding > speed
- **Celebrate wins!** 🎉

---

**Happy learning! 💪**

*Last updated: May 26, 2024*  
*Duration: 8-12 weeks | Effort: 2h/day | Status: Ready to start*
