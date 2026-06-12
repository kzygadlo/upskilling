# 3️⃣ ARCHITECTURE & SOLID - Q&A

---

## Q3.1: Clean Architecture - 4 layers, czym są?

**SHORT:**
**PATTERN**: Dependency points inward. Domain (center) = pure business. Application = use cases. Infrastructure (outer) = DB/API. API = delivery.

**FULL ANSWER:**

### The 4 Concentric Layers

```
        ┌─────────────────────────┐
        │   API LAYER             │
        │ Controllers, HTTP       │
        ├─────────────────────────┤
        │   APPLICATION LAYER     │
        │ Services, use cases     │
        ├─────────────────────────┤
        │   DOMAIN LAYER          │
        │ Entities, interfaces    │
        ├─────────────────────────┤
        │   INFRASTRUCTURE LAYER  │
        │ DbContext, repositories │
        └─────────────────────────┘

Dependency Rule: Can point inward, never outward
```

### Layer 1: Domain (Center)
```csharp
// Pure business logic, no frameworks
public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ProjectStatus Status { get; set; }
    
    public bool CanDelete() => Status != ProjectStatus.Active;
}

// Interfaces (abstract, owned by domain)
public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(int id);
    Task AddAsync(Project project);
    Task SaveChangesAsync();
}

// Business rules, domain objects, aggregates
// NO Framework dependencies (no DbContext, no ASP.NET)
```

### Layer 2: Application
```csharp
// Use cases, services, DTOs
public class CreateProjectUseCase
{
    private readonly IProjectRepository repository;
    
    public CreateProjectUseCase(IProjectRepository repository)
    {
        this.repository = repository;
    }
    
    public async Task<int> ExecuteAsync(CreateProjectDto dto)
    {
        // Validation
        if (string.IsNullOrEmpty(dto.Name))
            throw new ArgumentException("Name required");
        
        // Business logic
        var project = new Project { Name = dto.Name };
        await repository.AddAsync(project);
        await repository.SaveChangesAsync();
        
        return project.Id;
    }
}

// DTOs (for input/output)
public class CreateProjectDto
{
    public string Name { get; set; }
}

// NO HTTP, NO Database details
```

### Layer 3: Infrastructure
```csharp
// Implementation of domain interfaces
public class ProjectRepository : IProjectRepository
{
    private readonly TaskManagerDbContext db;
    
    public ProjectRepository(TaskManagerDbContext db)
    {
        this.db = db;
    }
    
    public async Task<Project?> GetByIdAsync(int id)
    {
        return await db.Projects.FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public async Task AddAsync(Project project)
    {
        db.Projects.Add(project);
    }
    
    public async Task SaveChangesAsync()
    {
        await db.SaveChangesAsync();
    }
}

// DbContext
public class TaskManagerDbContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
}

// Database-specific code
// Implements domain interfaces
```

### Layer 4: API (Delivery)
```csharp
[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly CreateProjectUseCase useCase;
    
    public ProjectsController(CreateProjectUseCase useCase)
    {
        this.useCase = useCase;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectDto dto)
    {
        try
        {
            var id = await useCase.ExecuteAsync(dto);
            return Created($"/api/projects/{id}", new { id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

// HTTP-specific code
// Calls application layer
// Transforms to/from HTTP
```

### Dependency Direction (Critical!)
```csharp
// ✓ Correct: pointing inward
API → Application → Domain ← Infrastructure
                      ↑
                 Implementations injected

// Domain doesn't know about:
// - DbContext
// - HTTP controllers
// - Any framework

// ✗ Wrong: pointing outward
API → Application → Infrastructure → Domain
// Domain depends on Database = WRONG
```

### File Structure
```
TaskManager.API/
├── Controllers/
│   └── ProjectsController.cs

TaskManager.Application/
├── UseCases/
│   ├── CreateProjectUseCase.cs
│   ├── GetProjectUseCase.cs
│   └── DeleteProjectUseCase.cs
├── DTOs/
│   ├── CreateProjectDto.cs
│   └── ProjectDto.cs
└── Validators/

TaskManager.Domain/
├── Entities/
│   └── Project.cs
└── Interfaces/
    └── IProjectRepository.cs

TaskManager.Infrastructure/
├── Data/
│   ├── TaskManagerDbContext.cs
│   └── Migrations/
└── Repositories/
    └── ProjectRepository.cs
```

---

## Q3.2: Dependency Rule - czym jest i dlaczego ważna?

**SHORT:**
**RULE**: Inner layers never depend on outer. Domain doesn't know about Database or HTTP. Dependencies injected from outside.

**FULL ANSWER:**

### Why Dependency Rule Matters
```csharp
// ❌ WRONG: Domain depends on Infrastructure
public class Project
{
    public int Id { get; set; }
    
    public void Save(SqlConnection connection)  // ❌ Domain knows SQL!
    {
        // SQL code in domain
    }
}

// ✓ CORRECT: Domain knows only business
public class Project
{
    public int Id { get; set; }
    
    public bool CanDelete() => Status != ProjectStatus.Active;  // Pure logic
}

// Infrastructure implements interface from Domain
public class ProjectRepository : IProjectRepository
{
    private readonly DbContext db;
    
    public async Task SaveAsync(Project project)  // Implementation detail
    {
        db.Projects.Add(project);
        await db.SaveChangesAsync();
    }
}
```

### Testability Benefit
```csharp
// With Dependency Rule:
public class CreateProjectUseCase
{
    private readonly IProjectRepository repository;
    
    public CreateProjectUseCase(IProjectRepository repository)
    {
        this.repository = repository;
    }
    
    public async Task<int> ExecuteAsync(CreateProjectDto dto)
    {
        var project = new Project { Name = dto.Name };
        await repository.AddAsync(project);
        await repository.SaveChangesAsync();
        return project.Id;
    }
}

// Test: Mock the repository (from Domain interface)
var mockRepo = new Mock<IProjectRepository>();
mockRepo.Setup(r => r.AddAsync(It.IsAny<Project>()))
    .Returns(Task.CompletedTask);

var useCase = new CreateProjectUseCase(mockRepo.Object);
await useCase.ExecuteAsync(new CreateProjectDto { Name = "Test" });

// ✓ Domain layer = testable without database
```

### Technology Swap Benefit
```csharp
// Without Dependency Rule:
public class ProjectRepository
{
    private readonly SqlConnection connection;  // ❌ Hard-coded
}
// To swap SQL → MongoDB: Rewrite entire repository

// With Dependency Rule:
public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(int id);
}

public class SqlProjectRepository : IProjectRepository { }
public class MongoProjectRepository : IProjectRepository { }
public class CosmosProjectRepository : IProjectRepository { }

// Swap in DI container:
services.AddScoped<IProjectRepository, MongoProjectRepository>();
// ✓ Domain + Application unchanged
```

### Inversion of Control (IoC)
```csharp
// Domain defines what it needs (interface)
public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(int id);
}

// Infrastructure provides implementation
public class ProjectRepository : IProjectRepository { }

// Application depends on interface (abstraction)
public class CreateProjectUseCase
{
    public CreateProjectUseCase(IProjectRepository repository) { }
    // Doesn't care if SQL, MongoDB, or test mock
}

// DI container wires it together
services.AddScoped<IProjectRepository, ProjectRepository>();
var app = services.BuildServiceProvider();
var useCase = app.GetRequiredService<CreateProjectUseCase>();
// ✓ IoC = control of dependencies reversed (outside in)
```

---

## Q3.3: Separation of Concerns - czym to jest?

**SHORT:**
**PRINCIPLE**: Each class/method has ONE responsibility. ProjectService handles projects, not emails/logging/payments. Easier to test, maintain, change.

**FULL ANSWER:**

### Violation Example
```csharp
// ❌ BAD: Multiple responsibilities
public class ProjectManager
{
    public void CreateProject(CreateProjectDto dto)
    {
        // Validation
        if (string.IsNullOrEmpty(dto.Name)) 
            throw new ArgumentException("Name required");
        
        // Business logic
        var project = new Project { Name = dto.Name };
        
        // Database
        _db.Projects.Add(project);
        _db.SaveChanges();
        
        // Email notification
        var email = new EmailService();
        email.Send($"Project '{project.Name}' created");
        
        // Logging
        Console.WriteLine($"[LOG] Project created: {project.Id}");
        
        // Metrics
        _metrics.IncrementCounter("projects.created");
    }
}
// One method does: validation, persistence, notifications, logging, metrics
// If email breaks, entire CreateProject breaks
// Hard to test (needs DB, email service, logging)
```

### Separated Concerns
```csharp
// ✓ GOOD: ProjectService = only projects
public class CreateProjectUseCase
{
    private readonly IProjectRepository repository;
    private readonly IProjectCreatedEventPublisher eventPublisher;
    
    public CreateProjectUseCase(IProjectRepository repository, 
                                IProjectCreatedEventPublisher eventPublisher)
    {
        this.repository = repository;
        this.eventPublisher = eventPublisher;
    }
    
    public async Task<int> ExecuteAsync(CreateProjectDto dto)
    {
        ValidateInput(dto);  // Validation
        var project = new Project { Name = dto.Name };
        await repository.AddAsync(project);  // Persistence
        await repository.SaveChangesAsync();
        await eventPublisher.PublishAsync(
            new ProjectCreatedEvent(project.Id));  // Event
        return project.Id;
    }
    
    private void ValidateInput(CreateProjectDto dto)
    {
        if (string.IsNullOrEmpty(dto.Name))
            throw new ArgumentException("Name required");
    }
}

// ✓ Validator = validation only
public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}

// ✓ EventHandler = responds to project created
public class SendProjectCreatedEmailHandler 
    : IEventHandler<ProjectCreatedEvent>
{
    private readonly IEmailService emailService;
    
    public async Task HandleAsync(ProjectCreatedEvent @event)
    {
        await emailService.SendAsync(
            $"Project created: {@event.ProjectId}");
    }
}

// ✓ Logging = via middleware/decorator
public class LoggingProjectRepositoryDecorator : IProjectRepository
{
    private readonly IProjectRepository inner;
    private readonly ILogger logger;
    
    public async Task AddAsync(Project project)
    {
        logger.LogInformation($"Adding project: {project.Name}");
        await inner.AddAsync(project);
        logger.LogInformation($"Project added: {project.Id}");
    }
}
```

### Benefits of Separation
```csharp
// ✓ Testability
var mockRepo = new Mock<IProjectRepository>();
var mockEventPublisher = new Mock<IProjectCreatedEventPublisher>();
var useCase = new CreateProjectUseCase(mockRepo.Object, mockEventPublisher.Object);
await useCase.ExecuteAsync(new CreateProjectDto { Name = "Test" });
// Easy to test (no database, no email)

// ✓ Maintainability
// If email breaks: fix SendProjectCreatedEmailHandler
// ProjectService unchanged

// ✓ Reusability
// ProjectService can be used by API, scheduled job, command-line tool
// Each has own email/logging/metrics handling

// ✓ Change Impact
// Add new event handler (e.g., update metrics): 
// - Create new handler
// - Wire in DI
// - ProjectService = unchanged
```

---

## Q3.4: Layered Architecture - pros/cons

**SHORT:**
**PROS**: Clear separation, testable, organized. **CONS**: Extra classes (boilerplate), over-engineering for simple features.

**FULL ANSWER:**

### Pros
1. **Clear Separation**
   - Each layer has clear responsibility
   - Easy to understand structure
   
2. **Testability**
   - Mock dependencies
   - Test layers independently
   
3. **Maintainability**
   - Changes isolated to layers
   - Easy to find code (by layer type)
   
4. **Reusability**
   - Services reusable across controllers
   - Repositories reusable across services
   
5. **Team Scalability**
   - Different teams work on different layers
   - Clear contracts (interfaces)

### Cons
1. **Boilerplate**
   ```csharp
   // Simple operation = lots of files
   CreateProjectDto → CreateProjectUseCase → ProjectService 
   → IProjectRepository → ProjectRepository → DbContext
   // 6+ classes for "create project"
   ```

2. **Indirection**
   - Many layers to follow during debugging
   - Complex call stack
   
3. **Over-engineering for Simple Apps**
   - CRUD with no business logic
   - Add complexity that's not needed
   
4. **Performance Overhead**
   - Mapping (DTO → Entity → DTO)
   - Multiple round-trips through layers

### When to Use
```csharp
// ✓ USE layered architecture
// - Large teams
// - Complex business logic
// - Long-lived application
// - Multiple consumers (API, jobs, commands)
// - Strict security requirements

// ❌ SKIP layered (too heavy)
// - Solo developer
// - Simple CRUD
// - MVP/startup
// - Short-lived application
// - Can refactor later
```

### Simplified Version (for small apps)
```csharp
// Still layered, but simpler
// Domain layer: Entities
// Application layer: Services (business logic)
// Infrastructure layer: DbContext
// API layer: Controllers (just HTTP)

// Skip intermediate DTOs for internal operations
// Use DTOs only at API boundary
```

---

## Q3.5-Q3.10: SOLID Principles

### Q3.5: SOLID - Overview

**SHORT:**
Five principles: Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion. Guide to maintainable OOP.

---

## Q3.6: Single Responsibility Principle

**SHORT:**
**RULE**: One class, one reason to change. ProjectService handles projects, not emails/logging. If business logic changes, only ProjectService updates.

**FULL ANSWER:**

```csharp
// ❌ Violates SRP
public class OrderProcessor
{
    public void Process(Order order)
    {
        // Validate order
        if (order.Items.Count == 0) throw new Exception("No items");
        
        // Calculate tax
        var tax = order.Total * 0.2m;
        
        // Save to database
        db.Orders.Add(order);
        db.SaveChanges();
        
        // Send email
        emailService.SendOrderConfirmation(order);
        
        // Log to file
        File.AppendAllText("orders.log", $"{DateTime.Now}: {order.Id}");
    }
}
// Reasons to change:
// 1. Validation logic changes
// 2. Tax calculation changes
// 3. Database schema changes
// 4. Email template changes
// 5. Logging format changes
// = TOO MANY reasons

// ✓ Follows SRP
public class OrderService  // Only order business logic
{
    public void Process(Order order)
    {
        var validator = new OrderValidator();
        validator.Validate(order);  // Delegate validation
        
        var taxCalculator = new TaxCalculator();
        order.Tax = taxCalculator.Calculate(order.Total);  // Delegate tax
        
        repository.SaveOrder(order);  // Persist via repository
    }
}

public class OrderValidator  // Only validation
{
    public void Validate(Order order)
    {
        if (order.Items.Count == 0) 
            throw new InvalidOperationException("No items");
    }
}

public class TaxCalculator  // Only tax logic
{
    public decimal Calculate(decimal amount) => amount * 0.2m;
}

public class OrderRepository  // Only data access
{
    public void SaveOrder(Order order)
    {
        db.Orders.Add(order);
        db.SaveChanges();
    }
}

public class OrderNotificationService  // Only notifications
{
    public async Task NotifyOrderCreatedAsync(Order order)
    {
        await emailService.SendOrderConfirmationAsync(order);
    }
}

// Each class has ONE reason to change
```

---

## Q3.7: Open/Closed Principle

**SHORT:**
**RULE**: Open for extension, closed for modification. Add features via inheritance/composition, not by modifying existing code.

**FULL ANSWER:**

```csharp
// ❌ Violates OCP
public class PaymentProcessor
{
    public void Process(Payment payment)
    {
        if (payment.Type == PaymentType.CreditCard)
        {
            // Process credit card
        }
        else if (payment.Type == PaymentType.PayPal)
        {
            // Process PayPal
        }
        else if (payment.Type == PaymentType.Bitcoin)
        {
            // Process Bitcoin
        }
    }
}
// To add new payment type: Modify PaymentProcessor (CLOSED for modification)

// ✓ Follows OCP
public interface IPaymentHandler
{
    void Process(Payment payment);
}

public class CreditCardPaymentHandler : IPaymentHandler
{
    public void Process(Payment payment)
    {
        // Credit card logic
    }
}

public class PayPalPaymentHandler : IPaymentHandler
{
    public void Process(Payment payment)
    {
        // PayPal logic
    }
}

public class BitcoinPaymentHandler : IPaymentHandler
{
    public void Process(Payment payment)
    {
        // Bitcoin logic
    }
}

public class PaymentProcessor
{
    private readonly IPaymentHandler handler;
    
    public PaymentProcessor(IPaymentHandler handler)
    {
        this.handler = handler;
    }
    
    public void Process(Payment payment)
    {
        handler.Process(payment);
    }
}

// To add new payment type: Create new handler
// PaymentProcessor = CLOSED for modification
// System = OPEN for extension (new handlers)
```

---

## Q3.8: Liskov Substitution Principle

**SHORT:**
**RULE**: Derived classes must be substitutable for base. If class A uses IPaymentHandler, all implementations work identically.

**FULL ANSWER:**

```csharp
// ❌ Violates LSP
public class Penguin : Bird
{
    public override void Fly()
    {
        throw new NotImplementedException("Penguins can't fly!");
    }
}

// Code expects: Bird.Fly() works
var bird = new Penguin();
bird.Fly();  // Throws! Breaks contract

// ✓ Follows LSP
public interface IFlying
{
    void Fly();
}

public class Sparrow : IFlying
{
    public void Fly() { }  // Implements
}

public class Penguin : Bird  // Doesn't implement IFlying
{
    public void Swim() { }
}

// Code that expects IFlying:
public class Trainer
{
    public void TeachToFly(IFlying bird)
    {
        bird.Fly();  // Always works (no false contracts)
    }
}
```

---

## Q3.9: Interface Segregation Principle

**SHORT:**
**RULE**: Many specific interfaces > one giant interface. Client implements only what it needs.

**FULL ANSWER:**

```csharp
// ❌ Violates ISP
public interface IRepository
{
    T GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
    void SaveChanges();
    void Backup();
    void Optimize();
    void Migrate();
}

public class MyService : IRepository
{
    // Forced to implement all 8 methods
    // But service only needs GetById and Add!
    public void Backup() { throw new NotImplementedException(); }
    public void Optimize() { throw new NotImplementedException(); }
    public void Migrate() { throw new NotImplementedException(); }
}

// ✓ Follows ISP
public interface IReadRepository<T>
{
    T GetById(int id);
}

public interface IWriteRepository<T>
{
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}

public interface IMaintenanceRepository
{
    void Backup();
    void Optimize();
    void Migrate();
}

public class MyService
{
    private readonly IReadRepository<Project> reader;
    private readonly IWriteRepository<Project> writer;
    
    public MyService(IReadRepository<Project> reader, 
                     IWriteRepository<Project> writer)
    {
        this.reader = reader;
        this.writer = writer;
    }
    // Implements only what it needs
}
```

---

## Q3.10: Dependency Inversion Principle

**SHORT:**
**RULE**: Depend on abstractions, not concretions. Inject IRepository, not SqlRepository directly.

**FULL ANSWER:**

```csharp
// ❌ Violates DIP
public class OrderService
{
    private readonly SqlRepository repository = new();  // Concrete dependency
    
    public void CreateOrder(Order order)
    {
        repository.Add(order);
    }
}
// Tightly coupled to SqlRepository
// Can't test without real SQL
// Can't swap to MongoDB

// ✓ Follows DIP
public interface IOrderRepository
{
    Task AddAsync(Order order);
}

public class OrderService
{
    private readonly IOrderRepository repository;
    
    public OrderService(IOrderRepository repository)  // Injected abstraction
    {
        this.repository = repository;
    }
    
    public async Task CreateOrderAsync(Order order)
    {
        await repository.AddAsync(order);
    }
}

// Implementation injected from outside
services.AddScoped<IOrderRepository, SqlRepository>();
// Can swap: services.AddScoped<IOrderRepository, MongoRepository>();
```

---

## Q3.11-Q3.16: Design Patterns (Brief)

### Q3.11: Repository Pattern
**WHAT**: Abstraction over data access. Service doesn't know if SQL/MongoDB/API.

### Q3.12: Factory Pattern
**WHAT**: Create objects without knowing concrete class. Factory.Create() instead of new SpecificClass().

### Q3.13: Strategy Pattern
**WHAT**: Swap algorithms at runtime. IValidationStrategy, IPaymentStrategy.

### Q3.14: Singleton Pattern
**WHAT**: One instance for entire app. Logger, Configuration, ConnectionPool.

### Q3.15: Observer Pattern
**WHAT**: Event-based communication. ProjectCreated event → multiple listeners.

### Q3.16: Adapter Pattern
**WHAT**: Translate between incompatible interfaces. Legacy API → modern interface.

---

## Q3.17-Q3.20: Application Design (Brief)

| Topic | Concept |
|-------|---------|
| Services Layer | Business logic, use cases, orchestration |
| DTOs vs Models | DTOs for API contract, Models for domain |
| Validation | Input validation + business rule validation |
| Error Handling | Consistent error responses, mapped status codes |

---

## 📌 SUMMARY - ARCHITECTURE & SOLID

| Principle | Rule |
|-----------|------|
| Clean Arch | 4 layers, dependency points inward |
| Dependency Rule | Inner never depends on outer |
| SRP | One reason to change |
| OCP | Open for extension, closed for modification |
| LSP | Derived classes substitute for base |
| ISP | Many specific interfaces, not one giant |
| DIP | Depend on abstractions, inject dependencies |

