# .NET Engineering Skills — 4-Day Interview Prep
## BCM dotnet Guild Internal Interview

**Timeline:** 4 days intensive  
**Focus:** Level 1 → Level 2 (foundational + production-ready)  
**Goal:** Interview-ready across 9 competency areas  
**Status:** Pre-interview cramming (not deep mastery)

---

## Strategy

Each day has:
- **What to learn** (specific topics)
- **Depth** (Level 1 = understand, Level 2 = apply)
- **Practice questions** (interview prep)
- **Time allocation** (2h/day = 8h total)

### Key Principle
- **Level 1 is mandatory** for all 9 areas (foundational)
- **Level 2 for critical areas** (C#, ASP.NET, Architecture, Testing)
- **Level 3 = awareness only** (mention, don't deep-dive)
- **Practice interview answers** for each area

---

# DAY 1: C#, .NET Platform & Runtime + Foundations

## Topics (2 hours)

### 1. C# Core Concepts (45 min)
**Level 1 — What you MUST know:**

#### Value vs Reference Types
```csharp
// VALUE TYPE (stack)
int x = 5;
int y = x;      // Copy by value
y = 10;
Console.WriteLine(x); // Still 5

// REFERENCE TYPE (heap)
class Project { public int Id; }
var p1 = new Project { Id = 1 };
var p2 = p1;    // Reference, not copy
p2.Id = 2;
Console.WriteLine(p1.Id); // Now 2!

// NULLABLE TYPES (C# 8+)
int? nullable = null;        // Can be null
if (nullable.HasValue) { }
string? text = null;         // Reference type nullable
```

**Interview question:**
> "Explain value vs reference types in C#. How does it affect memory and performance?"

**Answer structure:**
- Value types (struct, int, bool) → stack memory → fast, copied
- Reference types (class, string) → heap memory → slower, referenced
- Nullable reference types prevent null reference exceptions
- GC manages heap, stack is automatic

#### Collections & LINQ
```csharp
// Basic collections (LEVEL 1)
List<Project> projects = new();
Dictionary<int, Project> projectsById = new();
HashSet<string> names = new();

// LINQ (LEVEL 1)
var activeProjects = projects
    .Where(p => p.Active)
    .OrderBy(p => p.CreatedAt)
    .ToList();

// LINQ to SQL (Level 2 bridge)
var fromDb = dbContext.Projects
    .Where(p => p.Active)
    .Select(p => new { p.Id, p.Name })
    .ToListAsync();
```

**Interview question:**
> "What's the difference between `List<T>.Where()` and `DbContext.Projects.Where()`?"

**Answer:**
- List<T>.Where() = LINQ to Objects (in-memory)
- DbContext.Where() = LINQ to Entities (SQL translation)
- First executes in C#, second executes in database

#### Exception Handling
```csharp
try
{
    await _service.CreateProjectAsync(dto);
}
catch (ArgumentNullException ex)
{
    // Specific exception
    _logger.LogError(ex, "Invalid data");
}
catch (Exception ex)
{
    // Generic fallback
    _logger.LogError(ex, "Unexpected error");
}
finally
{
    // Cleanup
}
```

### 2. .NET Platform & Runtime (45 min)
**Level 1 — Conceptual understanding:**

#### What is .NET?
```
.NET = Platform (Runtime + BCL + Compiler)

Components:
├─ CLR (Common Language Runtime)
│  └─ Executes your code
├─ JIT Compiler
│  └─ C# → IL (Intermediate Language) → Machine code
├─ GC (Garbage Collector)
│  └─ Frees unused heap memory
└─ BCL (Base Class Library)
   └─ Ready-made classes (System.*, etc)
```

**Interview question:**
> "What happens when you run `dotnet run`?"

**Answer:**
1. Compiler translates C# → IL (Intermediate Language)
2. JIT compiler translates IL → machine code (at runtime)
3. CLR executes machine code
4. GC cleans up unused objects periodically
5. Program terminates

#### Async/Await (Conceptual)
```csharp
// DON'T think: "This spawns a thread"
await Task.Delay(5000);

// DO think: "This releases the thread for other work"
// After 5s, thread resumes from this point

// Compiler turns this into STATE MACHINE
// You don't need to understand internals, just:
// - await = "wait for this, but free the thread"
// - async = "this method has await inside"
// - Task<T> = "I return a value later"
```

**Interview question:**
> "Why is async/await important in web APIs?"

**Answer:**
- 1 thread can handle many concurrent requests
- Thread doesn't block on I/O (DB, HTTP)
- Better scalability (10k+ requests on single thread pool)
- Not multi-threading, but concurrency

#### Garbage Collection (Awareness)
```csharp
// You don't manage memory (unlike C++)
var project = new Project(); // Allocates on heap
// ... use project ...
// When no more references → GC cleans it up automatically

// AVOID:
var list = new List<Project>();
for (int i = 0; i < 1_000_000; i++)
{
    list.Add(new Project()); // 1 million allocations
}
// ❌ GC pressure! Bad practice.

// BETTER:
List<Project> projects = await _repository.GetAllAsync(); // Single allocation
```

**Interview question:**
> "What's the difference between `managed code` and `unmanaged code` in .NET?"

**Answer:**
- Managed code = .NET, GC handles memory (safe, automatic)
- Unmanaged code = C/C++, you manage memory (fast, dangerous)
- .NET has unsafe keyword to use pointers, but rare

### 3. Modern C# Features (30 min)
**Level 1 — Awareness:**

```csharp
// RECORDS (C# 9+) - Immutable by default
public record ProjectDto(int Id, string Name);
var dto = new ProjectDto(1, "TaskManager");
// Records have built-in equality, ToString, etc

// INIT-ONLY PROPERTIES (C# 9+)
public class Project
{
    public int Id { get; init; } // Can set once, then read-only
}
var p = new Project { Id = 1 };
p.Id = 2; // ❌ Error!

// NULLABLE REFERENCE TYPES (C# 8+)
#nullable enable
string? nullableName = null;  // ✅ Can be null
string nonNullName = "Text";  // ❌ Cannot be null (compiler warning)

// PATTERN MATCHING (C# 7+)
var message = project switch
{
    null => "No project",
    { Name: "" } => "Empty name",
    { Status: ProjectStatus.Active } => "Active",
    _ => "Other"
};
```

## Practice Interview Questions (answer these)

1. **"What's the difference between `string` and `StringBuilder`?"**
   - string = immutable (each change creates new object)
   - StringBuilder = mutable (efficient for many concatenations)

2. **"Explain boxing and unboxing."**
   - Boxing = value type → object (heap allocation, copy)
   - Unboxing = object → value type (copy back)
   - Performance cost: avoid generic collections with value types

3. **"What's the CLR and why does it matter?"**
   - Common Language Runtime = the engine executing your code
   - Handles memory, threading, security
   - Allows language interoperability (C#, VB.NET, F# all run on CLR)

4. **"How does async/await differ from threading?"**
   - Async/await = 1 thread, many concurrent operations (I/O)
   - Threading = N threads, N CPU cores
   - Async is more efficient for I/O-bound work

---

# DAY 2: ASP.NET Core & Web APIs

## Topics (2 hours)

### 1. HTTP Fundamentals (30 min)
**Level 1 — Mandatory:**

```
HTTP Request Structure:
┌─────────────────────┐
│ GET /api/projects   │ ← METHOD + PATH
├─────────────────────┤
│ Host: api.example   │
│ Authorization: ...  │ ← HEADERS
│ Content-Type: ...   │
├─────────────────────┤
│ (empty for GET)     │ ← BODY (optional)
└─────────────────────┘

HTTP Response Structure:
┌──────────────┐
│ 200 OK       │ ← STATUS CODE
├──────────────┤
│ Content-Type │ ← HEADERS
├──────────────┤
│ { "id": 1 }  │ ← BODY (JSON)
└──────────────┘
```

#### Common Status Codes
```
2xx = Success
├─ 200 OK - Successful GET/POST/PUT
├─ 201 Created - Resource created (POST)
├─ 204 No Content - Success, no body (DELETE)

3xx = Redirect
├─ 301 Moved Permanently
├─ 304 Not Modified

4xx = Client error
├─ 400 Bad Request - Invalid data
├─ 401 Unauthorized - Missing auth
├─ 403 Forbidden - Auth but no permission
├─ 404 Not Found
├─ 409 Conflict - Duplicate

5xx = Server error
├─ 500 Internal Server Error
├─ 503 Service Unavailable
```

**Interview question:**
> "When should you return 400 vs 422 vs 404?"

**Answer:**
- 400 = Malformed request (syntax error)
- 422 = Validation failed (e.g., email format invalid)
- 404 = Resource not found

### 2. ASP.NET Core API Design (45 min)
**Level 1-2:**

#### Controllers
```csharp
[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _service;
    
    public ProjectsController(IProjectService service)
    {
        _service = service; // DI
    }
    
    // GET /api/projects
    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetAll()
    {
        var projects = await _service.GetAllAsync();
        return Ok(projects); // 200 OK
    }
    
    // GET /api/projects/1
    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDto>> GetById(int id)
    {
        var project = await _service.GetByIdAsync(id);
        if (project == null)
            return NotFound(); // 404
        return Ok(project);
    }
    
    // POST /api/projects
    [HttpPost]
    public async Task<ActionResult<ProjectDto>> Create(CreateProjectDto dto)
    {
        var project = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), 
            new { id = project.Id }, project); // 201
    }
    
    // PUT /api/projects/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProjectDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent(); // 204
    }
    
    // DELETE /api/projects/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent(); // 204
    }
}
```

#### Routing
```csharp
// Attribute routing (modern)
[Route("api/projects")]
public IActionResult GetProjects() { }

[Route("api/projects/{id}")]
public IActionResult GetProject(int id) { }

// Convention routing (older)
// public IActionResult Index() // → GET /projects/index
// public IActionResult Details(int id) // → GET /projects/details/1
```

#### Serialization (JSON)
```csharp
// By default, ASP.NET Core serializes objects to JSON
public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

// GET /api/projects/1 returns:
// { "id": 1, "name": "TaskManager" }

// Customization:
public class ProjectDto
{
    [JsonPropertyName("project_id")] // JSON name
    public int Id { get; set; }
    
    [JsonIgnore] // Don't serialize
    public string InternalValue { get; set; }
}
```

**Interview question:**
> "Design a REST API endpoint for creating a new project. What HTTP method? Status code? Request/response body?"

**Answer:**
```
POST /api/projects
Request:
{
  "name": "My Project",
  "description": "..."
}

Response (201 Created):
{
  "id": 1,
  "name": "My Project",
  "description": "...",
  "createdAt": "2024-01-15T10:00:00Z"
}
```

### 3. Dependency Injection (30 min)
**Level 1-2:**

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Register dependencies (DI Container)
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddDbContext<TaskManagerDbContext>();
builder.Services.AddControllers();

var app = builder.Build();
app.MapControllers();
app.Run();

// Now controllers get dependencies automatically:
public class ProjectsController
{
    // Constructor injection
    public ProjectsController(IProjectService service) 
    {
        // service is created by DI container
    }
}
```

#### Lifetimes
```csharp
// Transient = new instance every time
builder.Services.AddTransient<IService, Service>();

// Scoped = new instance per HTTP request
builder.Services.AddScoped<IService, Service>();

// Singleton = one instance for entire application
builder.Services.AddSingleton<IService, Service>();
```

**When to use:**
- **Transient:** Stateless utilities, helpers
- **Scoped:** Services with HTTP request context (repositories, EF DbContext)
- **Singleton:** Caching, configuration, connection pools

### 4. Middleware Pipeline (15 min)
**Level 1 — Conceptual:**

```
HTTP Request → Middleware 1 → Middleware 2 → ... → Controller → Response

Example:
Request enters
  ↓
Authentication middleware (validates token)
  ↓
Logging middleware (logs request)
  ↓
CORS middleware (validates origin)
  ↓
Error handling middleware
  ↓
Routing → Controller
  ↓
Response exits (reverse order)
```

```csharp
// Program.cs
var app = builder.Build();

// Middleware order MATTERS!
app.UseHttpsRedirection();    // Redirect to HTTPS
app.UseCors("AllowAll");      // CORS policy
app.UseAuthentication();      // Validate JWT token
app.UseAuthorization();       // Check permissions
app.MapControllers();         // Route to controllers

app.Run();
```

## Practice Interview Questions

1. **"What's the difference between HTTP POST and PUT?"**
   - POST = create new resource (idempotent: no, can create duplicates)
   - PUT = update existing resource (idempotent: yes, same result)

2. **"Explain dependency injection in ASP.NET Core."**
   - DI = provide dependencies via constructor (not new'ing inside)
   - Benefits: testable, loose coupling, flexible
   - Registered in Program.cs with lifetime (transient/scoped/singleton)

3. **"What's the purpose of DTOs?"**
   - DTO = Data Transfer Object (separate from domain entities)
   - API returns ProjectDto (not Project entity directly)
   - Reasons: security, flexibility, API contract independence

4. **"How does ASP.NET Core handle authentication?"**
   - Middleware intercepts request
   - Validates JWT token (or session)
   - Sets User principal for authorization

---

# DAY 3: Architecture & Design + Data Access

## Topics (2 hours)

### 1. Clean Architecture Recap (30 min)
**Level 1-2:**

```
The 4 Layers (MANDATORY KNOWLEDGE):

┌────────────────────────────────┐
│ 1. API LAYER                   │
│ Controllers, HTTP routing      │
│ Status: 200, 404, 500          │
├────────────────────────────────┤
│ 2. APPLICATION LAYER           │
│ Services, business logic       │
│ Validation, DTOs               │
├────────────────────────────────┤
│ 3. DOMAIN LAYER                │
│ Entities, pure business rules  │
│ Interfaces (IRepository)       │
├────────────────────────────────┤
│ 4. INFRASTRUCTURE LAYER        │
│ DbContext, repositories        │
│ Database, external services    │
└────────────────────────────────┘

Dependency Rule:
├─ Outer → Inner ✅ (API depends on Application)
├─ Inner → Outer ❌ (Domain never depends on API)
└─ Inversion: Interfaces in Domain, implementations in Infrastructure
```

**Interview question:**
> "Why should Domain layer not depend on Infrastructure?"

**Answer:**
- Domain is pure business logic (reusable)
- If Domain imports EF Core, it's coupled to specific database
- Solution: Domain defines IRepository interface, Infrastructure implements it
- Allows testing Domain without database

### 2. SOLID Principles (15 min)
**Level 1 — Awareness:**

```
S = Single Responsibility Principle
  └─ Each class has ONE reason to change
  └─ ProjectService = manage projects (not repositories, not logging)

O = Open/Closed Principle
  └─ Open for extension, closed for modification
  └─ Add features via inheritance/composition, not changing existing code

L = Liskov Substitution Principle
  └─ Derived classes must be substitutable for base
  └─ If IProjectRepository, all implementations work the same way

I = Interface Segregation Principle
  └─ Many specific interfaces > one giant interface
  └─ ISaveProject, IDeleteProject (not IProjectRepository with 100 methods)

D = Dependency Inversion Principle
  └─ Depend on abstractions, not concretions
  └─ Use IProjectRepository, not ProjectRepository directly
```

**Interview question:**
> "Give an example of violating Single Responsibility Principle."

**Answer:**
```csharp
// ❌ BAD: ProjectService does everything
public class ProjectService
{
    public void CreateProject() { /* ... */ }
    public void SendEmail() { /* ... */ }
    public void LogToFile() { /* ... */ }
    public void CalculateTaxes() { /* ... */ }
}

// ✅ GOOD: Separate concerns
public class ProjectService { public void CreateProject() { } }
public class EmailService { public void SendEmail() { } }
public class LoggingService { public void LogToFile() { } }
```

### 3. Design Patterns (Quick Reference) (15 min)
**Level 1 — Awareness:**

```
Repository Pattern (CRITICAL)
  └─ Abstraction over data access
  └─ Interface: IProjectRepository
  └─ Implementation: ProjectRepository (with EF Core)

Factory Pattern
  └─ Create objects without specifying classes
  └─ Example: ProjectFactory.Create(dto)

Strategy Pattern
  └─ Swap algorithms at runtime
  └─ Example: IValidationStrategy

Singleton Pattern
  └─ One instance for entire app
  └─ Example: logging, configuration

Observer Pattern
  └─ Event-driven communication
  └─ Example: Project created → notify listeners
```

**Interview question:**
> "What's the Repository Pattern and why use it?"

**Answer:**
- Abstraction over database access
- Controller/Service doesn't know about EF Core or SQL
- Easy to swap implementations (e.g., Dapper instead of EF)
- Easy to test (mock repository)

### 4. Data Access & EF Core (30 min)
**Level 1-2:**

#### DbContext & Entities
```csharp
public class TaskManagerDbContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer("connection_string");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent API configuration
        modelBuilder.Entity<Project>()
            .HasKey(p => p.Id);
        
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne(t => t.Project);
    }
}

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<TaskItem> Tasks { get; set; } = new();
}

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; }
}
```

#### Queries
```csharp
// Simple CRUD
var project = await dbContext.Projects.FindAsync(1);

// Filtering
var active = await dbContext.Projects
    .Where(p => p.Active)
    .ToListAsync();

// Relationships (Include)
var projectWithTasks = await dbContext.Projects
    .Include(p => p.Tasks)
    .FirstOrDefaultAsync(p => p.Id == 1);

// Projection (Select)
var dto = await dbContext.Projects
    .Where(p => p.Active)
    .Select(p => new ProjectDto 
    { 
        Id = p.Id, 
        Name = p.Name 
    })
    .ToListAsync();

// Sorting & pagination
var page = await dbContext.Projects
    .OrderBy(p => p.CreatedAt)
    .Skip((pageNum - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
```

#### Migrations
```bash
# Create migration
dotnet ef migrations add AddProjectTable --project Infrastructure

# Apply migration
dotnet ef database update

# Rollback
dotnet ef database update <PreviousMigrationName>
```

**Interview question:**
> "What's the difference between `FindAsync()` and `FirstOrDefaultAsync()`?"

**Answer:**
- FindAsync(id) = searches by primary key (fast, cached)
- FirstOrDefaultAsync(predicate) = searches with condition (can be any property)

#### Transactions
```csharp
using (var transaction = await dbContext.Database.BeginTransactionAsync())
{
    try
    {
        await dbContext.Projects.AddAsync(project);
        await dbContext.SaveChangesAsync();
        
        await dbContext.Tasks.AddAsync(task);
        await dbContext.SaveChangesAsync();
        
        await transaction.CommitAsync();
    }
    catch
    {
        await transaction.RollbackAsync();
        throw;
    }
}
```

**Interview question:**
> "Why use transactions? Give a real example."

**Answer:**
- Ensures atomicity: either all changes succeed or all fail
- Example: Transfer money between accounts
  - Debit account A
  - Credit account B
  - Both must succeed, or both fail (no partial transfer)

#### Performance Considerations
```csharp
// ❌ N+1 Problem
var projects = await dbContext.Projects.ToListAsync();
foreach (var p in projects) // Loop!
{
    var tasks = await dbContext.Tasks
        .Where(t => t.ProjectId == p.Id)
        .ToListAsync(); // Separate query for EACH project!
}

// ✅ Solution: Include
var projects = await dbContext.Projects
    .Include(p => p.Tasks) // Single query with JOIN
    .ToListAsync();
```

**Interview question:**
> "What's the N+1 problem and how do you fix it?"

**Answer:**
- N+1 = 1 query for parent + N queries for children (N+1 total)
- Fix: Use Include() for relationships (single JOIN)
- Or: Use projection (Select)

## Practice Interview Questions

1. **"Design the architecture for TaskManager backend. Which layers? How do they interact?"**

2. **"Explain the Repository Pattern. What are the benefits?"**

3. **"What's the difference between Scoped and Transient lifetimes?"**

4. **"How do you handle relationships in EF Core (one-to-many, many-to-many)?"**

5. **"What's change tracking in EF Core? Why does it matter?"**

---

# DAY 4: Testing, Security & Practical Interview Simulation

## Topics (2 hours)

### 1. Testing Fundamentals (45 min)
**Level 1-2:**

#### Unit Testing with xUnit
```csharp
public class ProjectServiceTests
{
    private readonly ProjectService _service;
    private readonly Mock<IProjectRepository> _repositoryMock;
    
    public ProjectServiceTests()
    {
        _repositoryMock = new Mock<IProjectRepository>();
        _service = new ProjectService(_repositoryMock.Object);
    }
    
    [Fact]
    public async Task CreateProject_WithValidName_ReturnsProjectDto()
    {
        // Arrange
        var dto = new CreateProjectDto { Name = "Test" };
        var project = new Project { Id = 1, Name = "Test" };
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Project>()))
            .Returns(Task.CompletedTask);
        
        // Act
        var result = await _service.CreateAsync(dto);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.Name);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Once);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task CreateProject_WithInvalidName_ThrowsException(string name)
    {
        // Arrange
        var dto = new CreateProjectDto { Name = name };
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _service.CreateAsync(dto));
    }
}
```

#### Test Pyramid
```
        /\
       /  \         E2E Tests (few, slow, valuable)
      /────\
     /      \       Integration Tests (medium, moderate)
    /────────\
   /          \     Unit Tests (many, fast, cheap)
  /____________\
```

**Interview question:**
> "Why write unit tests? What's the ideal ratio of unit/integration/E2E tests?"

**Answer:**
- Unit tests = fast, isolated, many (70-80%)
- Integration tests = slower, real DB (15-20%)
- E2E tests = UI testing, rare (5%)
- Unit tests catch bugs early, reduce regression risk

#### Mocking
```csharp
// Mock = fake object for testing
var repositoryMock = new Mock<IProjectRepository>();

// Setup return value
repositoryMock
    .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
    .ReturnsAsync(new Project { Id = 1, Name = "Test" });

// Verify it was called
repositoryMock.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Once);
```

**Why mock?**
- Don't want to test database in unit tests
- Want to isolate the service logic
- Want deterministic tests (no random data)

### 2. Security Basics (30 min)
**Level 1-2:**

#### Authentication vs Authorization
```csharp
// AUTHENTICATION = "Who are you?"
// Verify credentials (username/password, JWT, OAuth)

[Authorize] // Requires authentication
public async Task<IActionResult> GetMyProjects()
{
    var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
    return Ok(await _service.GetByUserAsync(userId));
}

// AUTHORIZATION = "What can you do?"
// Verify permissions

[Authorize(Roles = "Admin")] // Only admins
public async Task<IActionResult> DeleteProject(int id)
{
    await _service.DeleteAsync(id);
    return NoContent();
}
```

#### JWT Tokens
```csharp
// In Program.cs
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

// Creating JWT
public string CreateToken(User user)
{
    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.Role, user.Role)
    };
    
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    
    var token = new JwtSecurityToken(
        issuer: _config["Jwt:Issuer"],
        audience: _config["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddHours(1),
        signingCredentials: creds
    );
    
    return new JwtSecurityTokenHandler().WriteToken(token);
}
```

#### Input Validation
```csharp
// ❌ BAD: No validation
public async Task<IActionResult> CreateProject(CreateProjectDto dto)
{
    var project = new Project { Name = dto.Name };
    await _service.CreateAsync(project);
    return Ok();
}

// ✅ GOOD: Validate input
[HttpPost]
public async Task<IActionResult> CreateProject(CreateProjectDto dto)
{
    if (string.IsNullOrWhiteSpace(dto.Name))
        return BadRequest("Name is required");
    
    if (dto.Name.Length > 100)
        return BadRequest("Name too long");
    
    var project = new Project { Name = dto.Name };
    await _service.CreateAsync(project);
    return Ok();
}

// ✅ BETTER: Use FluentValidation
public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name required")
            .MaximumLength(100).WithMessage("Max 100 chars");
    }
}
```

#### HTTPS & CORS
```csharp
// In Program.cs
app.UseHttpsRedirection(); // Force HTTPS

var corsPolicy = "AllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, builder =>
    {
        builder
            .WithOrigins("https://example.com") // Only this origin
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

app.UseCors(corsPolicy);
```

**Interview question:**
> "Why use HTTPS? Why enforce CORS?"

**Answer:**
- HTTPS = encrypted communication (prevents man-in-the-middle)
- CORS = whitelist allowed origins (prevents malicious JS from other sites)

#### SQL Injection Protection
```csharp
// ❌ VULNERABLE: String concatenation
var sql = $"SELECT * FROM Projects WHERE Name = '{userInput}'";
// If userInput = "'; DROP TABLE Projects; --"
// Executes: SELECT * FROM Projects WHERE Name = ''; DROP TABLE Projects; --'

// ✅ SAFE: Parameterized queries (EF Core)
var projects = await dbContext.Projects
    .FromSqlInterpolated($"SELECT * FROM Projects WHERE Name = {userInput}")
    .ToListAsync();
// EF Core sanitizes userInput automatically

// ✅ SAFE: Parameterized with Dapper
var projects = await connection.QueryAsync<Project>(
    "SELECT * FROM Projects WHERE Name = @Name",
    new { Name = userInput }
);
```

### 3. Azure & DevOps Basics (15 min)
**Level 1 — Awareness:**

```
Docker = Containerization
├─ Package app + dependencies in image
├─ Run anywhere (local, cloud, on-prem)

Azure = Cloud Platform
├─ App Service = host web apps
├─ Azure SQL = managed database
├─ Azure Key Vault = secrets management
├─ Azure Functions = serverless compute

CI/CD = Automation
├─ Every commit → run tests
├─ If tests pass → deploy to staging
├─ Manual approval → deploy to production
```

**Interview question:**
> "Why containerize your application?"

**Answer:**
- Consistency: same environment local + production
- Isolation: dependencies don't conflict
- Scalability: easy to run multiple containers

### 4. Practical Interview Simulation (30 min)

#### Scenario 1: Design Review
> "You're asked to design a REST API for a project management system. You need to support: creating projects, managing tasks within projects, and assigning tasks to users. Design the API endpoints, database schema, and architecture."

**Expected Answer (Interview quality):**

```
API Endpoints:
├─ POST /api/projects (create project)
├─ GET /api/projects (list projects)
├─ GET /api/projects/{id} (get project with tasks)
├─ PUT /api/projects/{id} (update project)
├─ DELETE /api/projects/{id} (delete project)
│
├─ POST /api/projects/{projectId}/tasks (create task)
├─ GET /api/projects/{projectId}/tasks (list tasks)
├─ PUT /api/tasks/{taskId} (update task)
├─ DELETE /api/tasks/{taskId} (delete task)
│
└─ PUT /api/tasks/{taskId}/assign/{userId} (assign task)

Database Schema:
┌─────────────┐
│ Projects    │
├─────────────┤
│ Id (PK)     │
│ Name        │
│ CreatedAt   │
│ UserId (FK) │
└─────────────┘

┌─────────────────┐
│ Tasks           │
├─────────────────┤
│ Id (PK)         │
│ Title           │
│ ProjectId (FK)  │
│ AssigneeId (FK) │
│ Status          │
│ CreatedAt       │
└─────────────────┘

Architecture:
API Layer:
  ├─ ProjectsController
  └─ TasksController

Application Layer:
  ├─ ProjectService
  └─ TaskService

Domain Layer:
  ├─ Project (entity)
  ├─ Task (entity)
  ├─ IProjectRepository
  └─ ITaskRepository

Infrastructure Layer:
  ├─ TaskManagerDbContext
  ├─ ProjectRepository
  └─ TaskRepository
```

#### Scenario 2: Bug Fixing
> "You receive a bug report: 'When I create a project, sometimes tasks from other projects show up in mine.' What could be the cause? How would you debug it?"

**Expected Answer:**

```
Potential causes:
1. N+1 Query Problem
   └─ Load project, then query tasks without filtering properly
   
2. Missing WHERE clause
   └─ SELECT * FROM Tasks (no ProjectId filter)
   
3. Caching issue
   └─ Old data cached, not invalidated on create
   
4. Concurrency issue
   └─ Race condition in Task assignment

Debug steps:
1. Enable SQL logging (see actual queries)
   options.LogTo(Console.WriteLine);
   
2. Check repository query
   return await dbContext.Tasks
       .Where(t => t.ProjectId == projectId) // Must filter!
       .ToListAsync();
   
3. Check for caching
   return await dbContext.Tasks
       .AsNoTracking() // Don't cache
       .Where(t => t.ProjectId == projectId)
       .ToListAsync();
   
4. Write test to reproduce
   public async Task GetTasks_ForProject1_DoesntReturnProject2Tasks()
   {
       // Arrange: Create projects and tasks
       // Act: Get tasks for Project 1
       // Assert: Verify only Project 1 tasks returned
   }
```

#### Scenario 3: Performance Question
> "Your API endpoint `GET /api/projects/{id}` is slow. It returns project with 1000 tasks. How would you optimize?"

**Expected Answer:**

```
Issues:
1. Loading 1000 tasks every time (memory + bandwidth)
2. Serializing 1000 tasks (JSON size)
3. No pagination

Solutions:
1. Pagination (get first 50 tasks)
   GET /api/projects/1/tasks?page=1&pageSize=50

2. Lazy loading (don't load tasks in initial request)
   GET /api/projects/1
   └─ Returns project without tasks
   
   GET /api/projects/1/tasks
   └─ Separate request for tasks

3. Projection (only get needed fields)
   SELECT Id, Title (not full Task object)

4. Caching (cache popular projects)
   IDistributedCache.GetAsync(cacheKey)

5. Indexing (database level)
   CREATE INDEX idx_projectId ON Tasks(ProjectId)

Code example:
[HttpGet("{id}")]
public async Task<IActionResult> GetProject(int id, [FromQuery] int page = 1)
{
    var project = await dbContext.Projects
        .FirstOrDefaultAsync(p => p.Id == id);
    
    var taskPage = await dbContext.Tasks
        .Where(t => t.ProjectId == id)
        .OrderBy(t => t.CreatedAt)
        .Skip((page - 1) * 50)
        .Take(50)
        .ToListAsync();
    
    return Ok(new { project, tasks = taskPage });
}
```

#### Scenario 4: Security Question
> "How would you secure the project endpoints so users can only access their own projects?"

**Expected Answer:**

```
1. Authentication
   └─ User must provide JWT token
   
2. Extract user ID from token
   └─ var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
   
3. Verify ownership
   └─ Check project belongs to authenticated user
   
4. Return Forbidden if not owner

Code:
[HttpGet("{id}")]
[Authorize]
public async Task<IActionResult> GetProject(int id)
{
    var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
    
    var project = await dbContext.Projects
        .FirstOrDefaultAsync(p => p.Id == id && p.UserId == int.Parse(userId));
    
    if (project == null)
        return Forbid(); // or NotFound() to not reveal existence
    
    return Ok(project);
}

Database:
Projects table should have UserId column
└─ Enforce: project.UserId == authenticated user
```

---

## Interview Checklist

Before interview, you should be comfortable answering:

### C# & .NET
- [ ] Value vs reference types
- [ ] Async/await vs threading
- [ ] LINQ basics
- [ ] Exception handling
- [ ] Collections (List, Dictionary, HashSet)

### ASP.NET Core
- [ ] REST API design (CRUD endpoints)
- [ ] HTTP methods & status codes
- [ ] Controllers & routing
- [ ] Dependency injection setup
- [ ] Middleware pipeline

### Architecture
- [ ] 4-layer Clean Architecture
- [ ] SOLID principles (at least understand, not memorize)
- [ ] Repository pattern
- [ ] Why separate concerns

### Data Access
- [ ] Entity Framework basics (DbContext, migrations)
- [ ] Relationships (one-to-many, etc)
- [ ] N+1 problem & Include()
- [ ] Transactions
- [ ] Migrations

### Testing
- [ ] Unit tests with xUnit
- [ ] Mocking with Moq
- [ ] Arrange-Act-Assert pattern
- [ ] Why test

### Security
- [ ] Authentication vs authorization
- [ ] JWT tokens
- [ ] Input validation
- [ ] SQL injection prevention
- [ ] HTTPS & CORS

### Design Scenarios
- [ ] Design API for a feature
- [ ] Debug a production issue
- [ ] Optimize slow endpoint
- [ ] Secure user data

---

## Quick Reference — Common Interview Questions

| Question | Answer |
|----------|--------|
| What's difference between List and IEnumerable? | List = concrete, IEnumerable = abstraction. IEnumerable is more flexible. |
| Why use interfaces? | Decoupling, testability, abstraction. |
| How do you handle errors in API? | Try-catch in service, return appropriate status code (400, 404, 500). |
| What's the difference between PUT and PATCH? | PUT = replace entire resource, PATCH = partial update. |
| How do you handle database migrations? | EF migrations (Add-Migration, Update-Database). |
| What's the CAP theorem? | Consistency, Availability, Partition tolerance. You can guarantee 2 of 3. |
| How do you scale a backend? | Caching, database optimization, async, load balancing. |
| What's a monolith vs microservices? | Monolith = one large app. Microservices = many small services (more complex). |

---

## Last Minute (1 hour before interview)

**DO:**
- [ ] Review SOLID principles
- [ ] Review 4-layer architecture
- [ ] Review REST API design
- [ ] Review one design scenario
- [ ] Review one debugging scenario

**DON'T:**
- [ ] Learn new topics
- [ ] Get stressed about Level 3 material
- [ ] Memorize code examples

**Mindset:**
- Interviewer wants to see thinking, not perfection
- "I'm not sure, but here's how I'd approach it" is good
- Ask clarifying questions
- Think out loud

---

**Good luck! 🚀**
