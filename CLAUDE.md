# TaskManager - Fullstack Upskilling Project

## 📚 Project Overview

This is a **2-month fullstack learning journey** upgrading from .NET 4.8/Angular 12 (2020) to modern stack:
- **Frontend**: Angular 21+ (Signals, Standalone Components, RxJS, NgRx)
- **Backend**: .NET 9 (ASP.NET Core, Clean Architecture, SOLID)
- **Database**: Azure SQL Server
- **DevOps**: Docker, Azure, CI/CD

**Goal**: Prepare for senior fullstack role with hands-on expertise in modern frameworks.

**Timeline**: 12 weeks | **Effort**: 2h/day

---

## 🏗️ Architecture

### Backend: Clean Architecture (4 layers)
```
TaskManager.API          ← REST endpoints, DI, middleware
  ↓
TaskManager.Application  ← Business logic, DTOs, services
  ↓
TaskManager.Domain       ← Entities, business rules
  ↓
TaskManager.Infrastructure ← EF Core, repositories, database
```

### Frontend: Modern Angular
- **Standalone Components** (no NgModules)
- **Signals** for state management (Angular 17+)
- **RxJS** for reactive programming
- **NgRx** for complex state (optional, Week 8)
- **Vitest** for testing

### Database
- **Azure SQL Database** (cloud, secure)
- **Entity Framework Core 9** for data access
- **User Secrets** for dev credentials (no secrets in repo!)

---

## 🔧 Tech Stack

### Backend
- **.NET 9.0** (LTS, fast, cross-platform)
- **ASP.NET Core** (minimal APIs, structured logging)
- **Entity Framework Core 8** (ORM, migrations)
- **SQL Server** (Azure SQL or local Express)
- **xUnit + Moq** (testing)

### Frontend
- **Angular 21+** (latest, reactive, performant)
- **TypeScript 5.9**
- **RxJS 7.8** (reactive streams)
- **SCSS** (CSS preprocessor)
- **Vitest** (unit testing)
- **Node.js 22 LTS**

### DevOps
- **Docker** (containerization)
- **Azure** (cloud, SQL DB, App Service)
- **Git** (version control)
- **.gitignore** (excludes: bin/, obj/, node_modules/)

---

## 📂 Project Structure

```
upskilling/
├── Backend/
│   ├── src/
│   │   ├── TaskManager.API/          ← Controllers, middleware, Program.cs
│   │   ├── TaskManager.Application/  ← Services, DTOs, validators
│   │   ├── TaskManager.Domain/       ← Entities (Project, TaskItem)
│   │   └── TaskManager.Infrastructure/ ← DbContext, repositories, migrations
│   ├── tests/
│   │   └── TaskManager.Tests/        ← xUnit tests
│   ├── TaskManager.sln
│   ├── Dockerfile
│   └── docker-compose.yml
│
├── Frontend/
│   ├── src/
│   │   ├── app/
│   │   │   ├── app.component.*       ← Root component
│   │   │   ├── app.config.ts         ← DI setup (providers)
│   │   │   ├── app.routes.ts         ← Routing
│   │   │   ├── core/                 ← Singleton services (API, auth)
│   │   │   ├── shared/               ← Reusable components
│   │   │   ├── features/             ← Feature modules (projects, tasks)
│   │   │   └── store/                ← NgRx state (optional)
│   │   ├── main.ts                   ← Bootstrap
│   │   └── styles.scss               ← Global styles
│   ├── package.json
│   ├── angular.json
│   ├── Dockerfile
│   └── nginx.conf
│
├── README.md                 ← Full learning plan + setup guide
├── WEEKLY_TASKS.md          ← 12-week task checklist
├── CLAUDE.md               ← This file
└── docker-compose.yml      ← Local development orchestration
```

---

## 🚀 Quick Start

### Prerequisites
- .NET 9.0 SDK
- Node.js 22 LTS
- Angular CLI 21+
- Git
- Azure subscription (free tier)

### Setup

```bash
# Backend
cd Backend
dotnet build TaskManager.sln
dotnet watch run --project src/TaskManager.API

# Frontend (new terminal)
cd Frontend
npm install
npm start

# Access
Backend API:  http://localhost:5000
Frontend:     http://localhost:4200
```

### Database

**User Secrets (dev only)**:
```bash
cd Backend/src/TaskManager.API
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=...password=...;"
```

**Azure SQL**:
- Connection string stored in User Secrets (NOT in appsettings.json)
- IP firewall rule configured in Azure Portal
- Migrations auto-applied via EF Core

---

## 📋 Learning Modules

### Week 1-2: Fundamentals & Setup ✅
- Project skeleton created
- Frontend: Angular 21 (standalone, routing)
- Backend: .NET 9 (clean architecture)
- Database: Azure SQL configured
- User Secrets for security

### Week 3: REST APIs & EF Core
- REST API design
- Entity Framework Core 8
- Dependency Injection
- Clean Architecture layers

### Week 4: Advanced C# & Design Patterns
- SOLID principles
- Design patterns (Factory, Strategy, etc)
- Async/await patterns
- Error handling & logging

### Week 5: Security & Authentication
- Azure AD / MSAL
- JWT tokens
- CORS configuration
- xUnit testing

### Week 6: RxJS & Reactive Programming
- Observables vs Promises
- RxJS operators (map, filter, switchMap, etc)
- Memory leak prevention (takeUntil)
- HttpClient interceptors

### Week 7: Signals & Standalone Components
- Angular Signals (state management)
- Computed signals
- Standalone APIs
- OnPush change detection

### Week 8: NgRx & State Management
- Actions, reducers, effects
- Entity adapters
- Selectors (memoized)
- Vitest testing

### Week 9: Frontend Security & CSS
- XSS prevention (DomSanitizer)
- Secure token storage (sessionStorage)
- SCSS theming
- Responsive design

### Week 10: DevOps & Deployment
- Docker (containerization)
- Azure DevOps (CI/CD)
- Kubernetes basics
- App Service deployment

### Week 11-12: Practice & Interview Prep
- Code review challenges
- Performance optimization
- System design discussions
- Portfolio polishing

---

## 🔐 Security Patterns

### User Secrets (Development)
```
Windows: C:\Users\{username}\AppData\Roaming\Microsoft\UserSecrets\{GUID}\secrets.json
macOS:   ~/.microsoft/usersecrets/{GUID}/secrets.json
Linux:   ~/.microsoft/usersecrets/{GUID}/secrets.json
```

NOT committed to git. Set in Visual Studio or CLI:
```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "..."
```

### Production (Azure Key Vault)
- Encrypted secrets in cloud
- Access via managed identity
- Audit logging
- Secret rotation

### Frontend Security
- Tokens in `sessionStorage` (not `localStorage`)
- DomSanitizer for user content
- Content Security Policy headers
- HTTPS enforced

---

## 🧪 Testing Strategy

### Backend (xUnit + Moq)
```csharp
[Fact]
public async Task CreateProject_WithValidData_ReturnsSuccess()
{
    // Arrange, Act, Assert
}
```

**Run**: `dotnet test Backend/tests/TaskManager.Tests/TaskManager.Tests.csproj`

### Frontend (Vitest)
```typescript
describe('ProjectService', () => {
  it('should fetch projects', async () => { });
});
```

**Run**: `npm run test` (Frontend)

---

## 📝 Code Conventions

### C# (.NET)
- **Naming**: PascalCase for classes/methods, camelCase for variables
- **Async**: Always use async/await, ConfigureAwait(false) for libraries
- **DI**: Constructor injection, interfaces for abstraction
- **Comments**: Only for "why", not "what" (code should be self-documenting)
- **SOLID**: Each class has one responsibility

### TypeScript (Angular)
- **Naming**: camelCase for variables/methods, PascalCase for types
- **Signals**: Prefer `signal()` + `computed()` for simple state
- **RxJS**: Use `takeUntil()` for unsubscribe management
- **Standalone**: All new components are standalone
- **Lint**: ESLint configured in `.eslintrc.json`

---

## 🔄 Git Workflow

### Branch Strategy
- `main`: Always deployable
- Feature branches: `feature/project-crud`, `feature/auth`
- Commits: Atomic (one change per commit)

### Commit Messages
```
[Type]: Brief description (under 50 chars)

Optional detailed explanation
- Bullet points for changes
- Why, not what

Co-Authored-By: Claude Haiku 4.5 <noreply@anthropic.com>
```

**Types**: `feat:`, `fix:`, `docs:`, `refactor:`, `test:`, `chore:`

### .gitignore
Excludes:
- `bin/`, `obj/` (build artifacts)
- `node_modules/`, `.angular/`, `dist/` (npm builds)
- `secrets.json` (user secrets)
- `.env`, `.env.local` (environment files)
- `appsettings.Development.json` (if contains secrets)

---

## 🛠️ Common Commands

### Backend
```bash
cd Backend

# Build
dotnet build TaskManager.sln

# Run with hot-reload
dotnet watch run --project src/TaskManager.API

# Run tests
dotnet test

# Database migrations
dotnet ef migrations add {MigrationName} --project src/TaskManager.Infrastructure --startup-project src/TaskManager.API
dotnet ef database update --project src/TaskManager.Infrastructure --startup-project src/TaskManager.API

# User Secrets
cd src/TaskManager.API
dotnet user-secrets init
dotnet user-secrets set "key" "value"
dotnet user-secrets list
```

### Frontend
```bash
cd Frontend

# Install dependencies
npm install

# Dev server with hot-reload
npm start

# Build for production
npm run build

# Run tests
npm run test

# Update Angular
ng update @angular/core @angular/cli
```

### Docker
```bash
# Build and run all services locally
docker-compose up

# Clean up
docker-compose down
docker system prune
```

---

## 📊 Current Status

### ✅ Completed
- [x] Project skeleton (Backend + Frontend)
- [x] .NET 9 upgrade (all .csproj)
- [x] Angular 21 upgrade (standalone, routing)
- [x] Database setup (Azure SQL)
- [x] User Secrets (local dev)
- [x] Entities & DbContext created
- [x] Initial migration applied
- [x] Docker setup (docker-compose.yml)
- [x] .gitignore configured

### ⬜ Next
- [ ] Week 1: Fundamentals review
- [ ] First API endpoint (Project CRUD)
- [ ] Unit tests (xUnit)
- [ ] Frontend components (Angular)
- [ ] Week 2-12: Full learning journey

### 📚 Learning Resources

**Backend (.NET 9)**
- Microsoft Docs: https://learn.microsoft.com/en-us/dotnet/
- EF Core: https://learn.microsoft.com/en-us/ef/core/
- Clean Architecture: "The Clean Architecture" by Uncle Bob

**Frontend (Angular 21)**
- Angular Docs: https://angular.io/docs
- RxJS: https://rxjs.dev/
- NgRx: https://ngrx.io/

**Security**
- OWASP Top 10: https://owasp.org/www-project-top-ten/
- Azure Security: https://learn.microsoft.com/en-us/azure/security/

---

## 🤝 Collaboration Notes

### ⚡ HOW TO WORK WITH CLAUDE (IMPORTANT!)

#### Rule 1: Small Steps (Incremental Execution)

**NEVER give 10-step plans!** User will hit issues on step 1-2.

**Instead: Small, testable increments**

❌ BAD:
```
Here's your plan:
1. Create API endpoint
2. Add validation
3. Write tests
4. Update frontend
5. Connect to database
... (10 steps total)
```

✅ GOOD:
```
Step 1: Create /api/projects endpoint (GET)
- Just read data, return JSON
- Test it: curl http://localhost:5000/api/projects

When done, tell me ✅ or ❌
Then we do Step 2.
```

**Why?**
- Issues happen early (firewall, port, permissions)
- User gets stuck and needs helper questions
- Small steps = fast feedback = learning
- Verify each step before moving forward

---

#### Rule 2: Ask Before Acting (Explicit Approvals)

**NEVER auto-execute without asking first!**

**Always follow this pattern:**

```
❌ DON'T:
[automatically edit files]
[run commands]
[make commits]

✅ DO:

1️⃣ SHOW PLAN:
"Here's what I'll do:
- Edit: Backend/src/TaskManager.API/Controllers/ProjectController.cs
  └─ Add GET /projects endpoint
- Modify: appsettings.json
  └─ Add logging level
- Create: Backend/tests/ProjectControllerTests.cs
  └─ Add unit tests"

2️⃣ ASK FOR APPROVAL:
"Ready to proceed? (yes/no)"

3️⃣ WAIT FOR RESPONSE:
User says: "yes" or "no" or "change X first"

4️⃣ EXECUTE:
Only then edit/create/run
```

---

#### Rule 3: Show Git Changes (Transparency)

**NEVER commit without showing what's being committed!**

**Always do this:**

```
📝 FILES TO CHANGE:
- src/TaskManager.API/Controllers/ProjectController.cs (new file)
  └─ Added: GET /api/projects endpoint
  
- tests/ProjectControllerTests.cs (new file)
  └─ Added: 5 unit tests

📊 COMMIT MESSAGE:
"Add Project CRUD endpoints - GET /api/projects

- Created ProjectController with GetProjects method
- Returns List<ProjectDto> from database
- Includes error handling and logging
- Added 5 unit tests with 100% coverage

Co-Authored-By: Claude Haiku 4.5 <noreply@anthropic.com>"

✅ Approve commit? (yes/no)"
```

**Why?**
- User sees exactly what's being changed
- User can catch mistakes before commit
- User understands git history
- User retains control

---

#### Rule 4: User Controls Actions

**These require EXPLICIT approval:**

1. **File edits** → Show diff first, ask confirmation
2. **File creation** → Show content, ask confirmation
3. **Git commits** → Show files + message, ask confirmation
4. **Bash/PowerShell commands** → Show command + description, ask confirmation
5. **Branch operations** → Show plan, ask confirmation
6. **Destructive ops** (clean, delete) → Show what's deleted, ask confirmation

**These are OK without asking:**
- Information gathering (git status, file reads)
- Explanations (no side effects)
- Plans/suggestions (no execution)
- Questions (clarification)

---

#### Rule 5: Describe Commands (Context for CLI)

**Always show WHAT and WHY for Bash/PowerShell:**

```
❌ DON'T:
"Run this: cd Backend && dotnet build"

✅ DO:
COMMAND: cd Backend && dotnet build

DESCRIPTION:
- Changes to Backend folder
- Builds entire solution (all projects)
- Checks for compilation errors
- Needed because: Files changed, verify it compiles

EXPECTED OUTPUT:
"Kompilacja powiodła się" (no errors)
```

**Why?**
- User understands what's happening
- User can catch wrong commands
- User can run manually if needed
- Clear debugging if it fails

---

#### Rule 6: Use UI Dialogs for Approvals

**Instead of "type yes/no", use clickable buttons:**

When asking for approval, use UI dialog with options:
- ✅ **Proceed** (execute action)
- ❌ **Cancel** (don't do it, stay in discussion)
- 💬 **Discuss First** (talk more before executing)

**Examples:**

```
Ready to create Project controller?

[Proceed] [Cancel] [Discuss First]
```

```
Ready to commit changes?

Files: 3 modified, 2 created
Message: "Add Project CRUD endpoints"

[Proceed] [Cancel] [Discuss First]
```

```
Ready to run: dotnet build?

[Proceed] [Cancel] [Discuss First]
```

**Why?**
- Faster than typing "yes"
- Less error-prone
- Clear visual confirmation
- Three options instead of binary yes/no

---

#### Rule 7: Sprawdzaj Prerequisites (Check Before Asking)

**NIGDY nie proś testować endpoint, jeśli serwis nie jest uruchomiony!**

**Błąd**: Poprosiłem użytkownika o `curl http://localhost:5000/swagger` bez upewnienia się, że backend jest uruchomiony.

**Wynik**: Użytkownik dostał błąd "Nie można połączyć się z serwerem"

**Lekcja**: Przed każdym testem, upewnij się że:
- Backend uruchomiony (`dotnet watch run`)
- Frontend uruchomiony (`npm start`)
- Baza danych dostępna (Azure SQL firewall OK, User Secrets loaded)

**Wzór**:
```
❌ DON'T: "Sprawdzić czy API działa: curl http://localhost:5000/swagger"
✅ DO: "Najpierw uruchom backend, potem testujemy"
```

### Claude Code Instructions
- **Use Clean Architecture**: Keep concerns separated
- **SOLID First**: Refactor toward SOLID as you learn
- **Test-First**: Write tests before or alongside code
- **Security-First**: Think about security at every layer
- **Comment Wisely**: Only "why", code explains "what"
- **No Over-Engineering**: Three instances = pattern, not premature abstraction
- **Small Steps First**: Plan big, execute tiny. Verify, then next step
- **Assume Failure**: Expect step 1 to have issues. Don't wait for step 10

### Async/Await
- Always use `async`/`await` (no blocking)
- Prefer `ConfigureAwait(false)` in libraries
- Avoid `Task.Result` / `.Wait()` (deadlock risk)

### RxJS Best Practices
- Use `takeUntil()` for subscriptions
- Use `shareReplay()` for cached data
- Unsubscribe in `ngOnDestroy`
- Prefer async pipe over manual subscriptions

---

## 🎯 Success Criteria

By end of Week 12, you should be able to:

✅ Build REST APIs with proper design patterns  
✅ Understand Clean Architecture & SOLID principles  
✅ Write secure, tested code (.NET + Angular)  
✅ Deploy to Azure (App Service, SQL DB, Docker)  
✅ Explain architectural decisions in interviews  
✅ Mentor others on modern fullstack practices  

---

## 📞 Questions?

If Claude needs clarification on:
1. **Architecture decisions** → Refer to Clean Architecture in docs
2. **Security concerns** → Refer to OWASP Top 10
3. **Testing strategy** → Refer to xUnit/Vitest docs
4. **Best practices** → Refer to Microsoft/Angular official docs

---

**Last Updated**: May 26, 2026  
**Status**: Week 1 Complete, Ready for Learning  
**Next**: Week 3 - REST APIs & EF Core
