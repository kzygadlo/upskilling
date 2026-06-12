# 2️⃣ WEB API DESIGN - Q&A

---

## Q2.1: HTTP basics - request/response structure

**SHORT:**
**PROTOCOL**: HTTP = Request-Response. Request: method + path + headers + body. Response: status code + headers + body. Stateless.

**FULL ANSWER:**

### HTTP Request Structure
```
GET /api/projects/1 HTTP/1.1
Host: api.example.com
Authorization: Bearer token123
Content-Type: application/json

(optional body for POST/PUT)
```

**Components:**
- **Method**: GET, POST, PUT, DELETE, PATCH, HEAD, OPTIONS
- **Path**: /api/projects/1 (resource location)
- **Headers**: Metadata (Content-Type, Authorization, Accept-Language)
- **Body**: Data (JSON, XML) for POST/PUT/PATCH
- **Version**: HTTP/1.1 (or HTTP/2)

### HTTP Response Structure
```
HTTP/1.1 200 OK
Content-Type: application/json
Content-Length: 56

{
  "id": 1,
  "name": "TaskManager",
  "status": "active"
}
```

**Components:**
- **Status Code**: 200, 404, 500, etc.
- **Headers**: Metadata (Content-Type, caching info)
- **Body**: Response data (JSON, HTML)

### Stateless
```csharp
// Each request is independent
GET /api/projects/1  // Server doesn't remember this request
GET /api/projects/2  // No context from previous request

// Authentication via headers, not session state
Authorization: Bearer token123  // Token sent every request
```

---

## Q2.2: HTTP methods - GET, POST, PUT, DELETE, PATCH

**SHORT:**
**SEMANTICS**: GET = read (safe). POST = create (not idempotent). PUT = replace (idempotent). DELETE = remove (idempotent). PATCH = partial update.

**FULL ANSWER:**

### GET - Read Data
```csharp
GET /api/projects/1

// Characteristics:
// - Safe (doesn't change server state)
// - Idempotent (same result every time)
// - Cacheable
// - No body in request

// Response:
200 OK
{
  "id": 1,
  "name": "TaskManager"
}
```

### POST - Create Resource
```csharp
POST /api/projects

Request:
{
  "name": "NewProject"
}

// Characteristics:
// - NOT safe (changes server state)
// - NOT idempotent (multiple calls create multiple resources)
// - Not cacheable

// Response:
201 Created
Location: /api/projects/1
{
  "id": 1,
  "name": "NewProject"
}
```

### PUT - Replace Entire Resource
```csharp
PUT /api/projects/1

Request:
{
  "id": 1,
  "name": "UpdatedName",
  "description": "Updated description"
}

// Characteristics:
// - NOT safe (changes server state)
// - Idempotent (same result every time)
// - Replace entire resource (not partial)

// Response:
200 OK or 204 No Content
```

### DELETE - Remove Resource
```csharp
DELETE /api/projects/1

// Characteristics:
// - NOT safe
// - Idempotent (deleting twice = same result: gone)
// - Cacheable (might be cached as 404)

// Response:
204 No Content
```

### PATCH - Partial Update
```csharp
PATCH /api/projects/1

Request:
{
  "name": "OnlyChangeThis"
}

// Characteristics:
// - NOT safe
// - NOT idempotent (depends on current state)
// - Partial update (only specified fields)

// Response:
200 OK or 204 No Content
```

### Summary Table
| Method | Safe | Idempotent | Cacheable | Use Case |
|--------|------|-----------|-----------|----------|
| GET | Yes | Yes | Yes | Read |
| POST | No | No | No | Create |
| PUT | No | Yes | No | Replace |
| DELETE | No | Yes | Yes | Remove |
| PATCH | No | No | No | Partial update |

### Idempotent Definition
```
Idempotent = same result when called multiple times

PUT /projects/1 { name: "A" }  // Result: name = A
PUT /projects/1 { name: "A" }  // Result: name = A (same)
PUT /projects/1 { name: "A" }  // Result: name = A (same)

POST /projects { name: "New" }  // Result: ID 1
POST /projects { name: "New" }  // Result: ID 2 (different!)
POST /projects { name: "New" }  // Result: ID 3 (different!)
```

---

## Q2.3: HTTP status codes - 2xx, 4xx, 5xx meanings

**SHORT:**
**SEMANTICS**: 2xx = success. 4xx = client error (bad request). 5xx = server error (bug/down). Status code = protocol, not business logic.

**FULL ANSWER:**

### 2xx Success Codes
```
200 OK
  - Standard success response
  - GET: returned data
  - POST/PUT: operation completed
  
201 Created
  - Resource created successfully
  - Location header: where to find it
  - POST creates new resource

204 No Content
  - Success, but no body to return
  - DELETE, PUT often use this
  - Client expects empty response

200 vs 204:
  200 = success + return data
  204 = success + nothing to return
```

### 3xx Redirect Codes
```
301 Moved Permanently
  - Resource moved to new URL
  - Client should update bookmarks
  - GET /old → Redirect to /new

304 Not Modified
  - Client has cached version
  - Sent if-modified-since header
  - Server: not modified, use cache
```

### 4xx Client Error Codes
```
400 Bad Request
  - Malformed syntax
  - Missing required fields
  - JSON parsing error
  
401 Unauthorized
  - Missing authentication
  - Invalid token
  - Solution: login/renew token
  
403 Forbidden
  - Authenticated, but no permission
  - User is logged in, but can't access resource
  - Solution: request different resource or ask admin
  
404 Not Found
  - Resource doesn't exist
  - GET /projects/999 (no project 999)
  
409 Conflict
  - Duplicate creation attempt
  - Optimistic locking conflict
  - Example: user already exists with email
  
422 Unprocessable Entity
  - Valid syntax, but business logic fails
  - Email format invalid (syntax OK, business rule fails)
  - Age is negative (syntax OK, rule fails)
```

### 5xx Server Error Codes
```
500 Internal Server Error
  - Unhandled exception in code
  - Database connection failed
  - Third-party API unreachable
  
503 Service Unavailable
  - Server temporarily down
  - Maintenance
  - Too many requests (overload)
```

### When to Use Which
```csharp
public IActionResult CreateProject(CreateProjectDto dto)
{
    // 400: missing data or malformed
    if (dto == null || string.IsNullOrEmpty(dto.Name))
        return BadRequest("Name required");
    
    // 409: duplicate
    if (ProjectExists(dto.Name))
        return Conflict("Project already exists");
    
    // 422: business rule failed
    if (dto.Name.Length > 100)
        return UnprocessableEntity("Name too long");
    
    var project = new Project { Name = dto.Name };
    SaveProject(project);
    
    // 201: created
    return CreatedAtAction(nameof(GetProject), 
        new { id = project.Id }, project);
}
```

---

## Q2.4: Kiedy używać 400 vs 422 vs 404?

**SHORT:**
400 = malformed/missing data. 422 = valid data but business rule fails. 404 = resource not found.

**FULL ANSWER:**

### 400 Bad Request
```csharp
// ✓ Malformed JSON
POST /api/projects
{ "name": "Project, }  // Invalid JSON

// ✓ Missing required field
{ "description": "..." }  // Missing "name"

// ✓ Wrong type
{ "id": "not-a-number" }  // Expected number

public IActionResult Create(CreateProjectDto dto)
{
    if (string.IsNullOrEmpty(dto.Name))
        return BadRequest("Name is required");  // 400
}
```

### 422 Unprocessable Entity
```csharp
// ✓ Valid JSON structure, but business rule violated
{ "name": "", "age": -5 }  // Valid syntax, invalid business rules

// ✓ Duplicate key
{ "email": "existing@example.com" }  // Email already exists

// ✓ Out of range
{ "score": 1000 }  // Valid number, but max is 100

public IActionResult Create(CreateProjectDto dto)
{
    // Syntax is valid, but business rules fail
    if (dto.Name.Length > 100)
        return UnprocessableEntity("Name must be <= 100 chars");  // 422
    
    if (ProjectExists(dto.Name))
        return Conflict("Project already exists");  // 409 (subset of 422)
}
```

### 404 Not Found
```csharp
GET /api/projects/999  // Project 999 doesn't exist

public IActionResult GetProject(int id)
{
    var project = db.Projects.FirstOrDefault(p => p.Id == id);
    if (project == null)
        return NotFound();  // 404
}
```

### Comparison Table
| Code | Meaning | Example |
|------|---------|---------|
| 400 | Syntax/structure error | Missing field, malformed JSON |
| 401 | Not authenticated | No token, token expired |
| 403 | Not authorized | Authenticated, but no permission |
| 404 | Resource not found | GET /projects/999 |
| 409 | Conflict | Duplicate key |
| 422 | Business rule violated | Email format invalid, age negative |
| 500 | Server error | Exception, bug |

---

## Q2.5: REST principles - czym są?

**SHORT:**
**STYLE**: Resource-based URLs. HTTP methods for operations. Stateless. Representation exchanged. Links for discoverability.

**FULL ANSWER:**

### 1. Client-Server Architecture
```
Client (UI, mobile) ←→ Network ←→ Server (API)

Separation allows independent evolution.
```

### 2. Resource-Based URLs
```csharp
// ✓ REST: Resources + methods
GET    /api/projects          // Get all
GET    /api/projects/1        // Get one
POST   /api/projects          // Create
PUT    /api/projects/1        // Update
DELETE /api/projects/1        // Delete

// ❌ Not REST: Actions in URL
GET    /api/getProjects       // Wrong
POST   /api/createProject     // Wrong
GET    /api/deleteProject?id=1 // Wrong
```

### 3. Stateless
```csharp
// Each request contains all needed info
GET /api/projects/1
Authorization: Bearer token123  // No session state needed
Accept-Language: en-US

// Server doesn't store request context
// Next request is independent
```

### 4. Representation of Resources
```csharp
// Resource: Project with ID 1
// Representation: JSON format
GET /api/projects/1

{
  "id": 1,
  "name": "TaskManager",
  "description": "...",
  "createdAt": "2024-01-15T10:00:00Z"
}

// Same resource could be XML, HTML, etc.
// Content-Type header specifies format
```

### 5. HATEOAS (Hypermedia As The Engine Of Application State)
```csharp
// Response includes links to related resources
GET /api/projects/1

{
  "id": 1,
  "name": "TaskManager",
  "_links": {
    "self": { "href": "/api/projects/1" },
    "all-projects": { "href": "/api/projects" },
    "tasks": { "href": "/api/projects/1/tasks" }
  }
}

// Client discovers related operations via links
// Not just hardcoding /api/projects/1/tasks
```

### 6. Caching
```csharp
// Responses should indicate cacheability
HTTP/1.1 200 OK
Cache-Control: max-age=3600  // Cache for 1 hour
ETag: "abc123"

// Client/proxy can cache safe responses (GET)
```

### 7. Uniform Interface
```csharp
// Standard HTTP methods + status codes
// Consistent URL structure
// JSON representations
```

---

## Q2.6: Routing - jak projektować URL structure?

**SHORT:**
**STRATEGY**: Hierarchical, resource-based. `/api/v1/projects/{id}/tasks/{taskId}`. Noun-based (not verb-based).

**FULL ANSWER:**

### Hierarchical URLs
```csharp
// Collection
GET /api/projects

// Single resource
GET /api/projects/1

// Sub-resources
GET /api/projects/1/tasks          // All tasks in project
GET /api/projects/1/tasks/5        // Specific task

// Filter parameters
GET /api/projects?status=active    // Query string
GET /api/projects?skip=10&take=20  // Pagination
GET /api/tasks?assignedTo=john     // Filter

// Sorting
GET /api/projects?orderBy=name&direction=desc
```

### Versioning
```csharp
// URL versioning
GET /api/v1/projects
GET /api/v2/projects  // Different schema

// Header versioning
GET /api/projects
Accept-Version: 1.0

// Query versioning
GET /api/projects?api-version=2.0
```

### Naming Conventions
```csharp
// ✓ Plural nouns (resources)
/api/projects
/api/users
/api/tasks

// ✗ Singular
/api/project
/api/user

// ✓ Kebab-case (optional, consistent)
/api/user-preferences
/api/project-settings

// ✓ Include resource ID in path
/api/projects/1
/api/projects/1/tasks/5

// ✗ Action verbs (not REST)
/api/getProject
/api/createProject
/api/deleteProject?id=1
```

### Complex Relationships
```csharp
// One-to-many
/api/projects/1/tasks
/api/projects/1/tasks/5
/api/users/john/projects

// Many-to-many (belongs to multiple)
/api/projects/1/contributors
/api/users/john/projects

// Alternative: Use IDs only
/api/tasks?projectId=1
/api/tasks?assigneeId=john
```

### Example: Complete Structure
```
/api/v1
├── /projects
│   ├── GET              → List all
│   ├── POST             → Create
│   ├── /{id}
│   │   ├── GET          → Get one
│   │   ├── PUT          → Update
│   │   ├── DELETE       → Delete
│   │   └── /tasks
│   │       ├── GET      → List tasks in project
│   │       ├── POST     → Create task
│   │       └── /{taskId}
│   │           ├── GET  → Get task
│   │           ├── PUT  → Update task
│   │           └── DELETE → Delete task
└── /tasks
    ├── GET              → List all (with filters)
    └── /{id}
        ├── GET
        ├── PUT
        └── DELETE
```

---

## Q2.7: DTOs - Data Transfer Objects, po co się ich używa?

**SHORT:**
**PATTERN**: Separate API schema from domain model. DTO = what API exposes. Entity = internal domain logic. Different fields, different validation.

**FULL ANSWER:**

### Why Separate DTOs
```csharp
// Domain entity (internal, complex)
public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string InternalNotes { get; set; }  // Secret!
    public List<TaskItem> Tasks { get; set; }
    public decimal Budget { get; set; }
    public decimal CostToDate { get; set; }
    public User Manager { get; set; }
}

// DTO for API (external, limited)
public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int TaskCount { get; set; }  // Computed
    public string ManagerName { get; set; }  // Simplified
    // InternalNotes NOT exposed
}

// Benefits:
// - Hide sensitive data (InternalNotes)
// - Different field names (Manager → ManagerName)
// - Computed properties (TaskCount)
// - Control versioning independently
```

### Create vs Update DTOs
```csharp
// Input: Create new project
public class CreateProjectDto
{
    public string Name { get; set; }
    // No Id (server generates)
    // No CreatedAt (server sets)
}

// Input: Update existing
public class UpdateProjectDto
{
    public string Name { get; set; }
    // No Id (comes from URL)
    // No CreatedAt (immutable)
}

// Output: Return to client
public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

### Mapping Example
```csharp
public async Task<ProjectDto> GetProjectAsync(int id)
{
    var entity = await db.Projects.FirstOrDefaultAsync(p => p.Id == id);
    
    return new ProjectDto
    {
        Id = entity.Id,
        Name = entity.Name,
        TaskCount = entity.Tasks.Count,
        ManagerName = entity.Manager?.Name
    };
}

// Or with AutoMapper
var dto = mapper.Map<ProjectDto>(entity);
```

### Fluent Validation on DTOs
```csharp
public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
{
    public CreateProjectDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name required")
            .MaximumLength(100).WithMessage("Max 100 chars");
    }
}
```

---

## Q2.8: Serialization/Deserialization - czym to jest?

**SHORT:**
**PROCESS**: Serialization = C# object → JSON string. Deserialization = JSON string → C# object. System.Text.Json in .NET 9.

**FULL ANSWER:**

### Serialization (Object → JSON)
```csharp
public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
}

var project = new Project 
{ 
    Id = 1, 
    Name = "TaskManager",
    CreatedAt = new DateTime(2024, 1, 15)
};

// Serialize to JSON string
var json = JsonSerializer.Serialize(project);

// Result:
// {"id":1,"name":"TaskManager","createdAt":"2024-01-15T00:00:00"}
```

### Deserialization (JSON → Object)
```csharp
var json = """{"id":1,"name":"TaskManager","createdAt":"2024-01-15T00:00:00"}""";

// Deserialize to C# object
var project = JsonSerializer.Deserialize<Project>(json);

// Result: project.Id = 1, project.Name = "TaskManager"
```

### In ASP.NET Core (Automatic)
```csharp
[HttpPost]
public async Task<IActionResult> Create(CreateProjectDto dto)
{
    // ASP.NET Core automatically deserializes JSON body to dto
    // No manual JsonSerializer needed
    
    return Ok(new ProjectDto { ... });
    // ASP.NET Core automatically serializes response to JSON
}
```

### Customization
```csharp
public class Project
{
    [JsonPropertyName("projectId")]  // JSON name differs
    public int Id { get; set; }
    
    [JsonIgnore]  // Don't serialize
    public string InternalNotes { get; set; }
}

// Result:
// {"projectId":1,"name":"TaskManager"}  (no InternalNotes)
```

### Performance Considerations
```csharp
// ✓ Fast: native JSON
var json = JsonSerializer.Serialize(project);

// System.Text.Json = default (.NET 9+)
// Utf8JsonWriter = low-allocation streaming

// ❌ Avoid: manual string concatenation
var json = $"{{\\"id\\":{project.Id},\\"name\\":\\"{project.Name}\\"}}"  // Bad!
```

---

## Q2.9: Error handling w API - jak to robić?

**SHORT:**
**PATTERN**: Global exception handler. Map exceptions to status codes + error response. Consistent error format (ProblemDetails).

**FULL ANSWER:**

### Global Exception Handling
```csharp
// Middleware catches all exceptions
public class ExceptionMiddleware
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private static async Task HandleExceptionAsync(
        HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        
        switch (exception)
        {
            case ArgumentNullException:
                response.StatusCode = 400;
                break;
            case KeyNotFoundException:
                response.StatusCode = 404;
                break;
            default:
                response.StatusCode = 500;
                break;
        }
        
        await response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = response.StatusCode,
            Title = "An error occurred",
            Detail = exception.Message
        });
    }
}

// In Program.cs
app.UseMiddleware<ExceptionMiddleware>();
```

### ProblemDetails (Standard Format)
```csharp
// RFC 7807 Standard error format

{
  "type": "https://api.example.com/errors/validation-failed",
  "title": "Validation Failed",
  "status": 422,
  "detail": "One or more validation errors occurred.",
  "instance": "/api/projects",
  "errors": {
    "name": ["Name is required", "Name must be <= 100 chars"],
    "email": ["Invalid email format"]
  }
}
```

### Controller-Level Handling
```csharp
[ApiController]
public class ProjectsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectDto dto)
    {
        try
        {
            var project = new Project { Name = dto.Name };
            await db.Projects.AddAsync(project);
            await db.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetProject), 
                new { id = project.Id }, project);
        }
        catch (DbUpdateException ex)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Duplicate Project",
                Detail = "Project with this name already exists"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ProblemDetails
            {
                Title = "Internal Server Error"
            });
        }
    }
}
```

### Input Validation Errors
```csharp
// Automatic with model validation
[HttpPost]
public IActionResult Create([FromBody] CreateProjectDto dto)
{
    if (!ModelState.IsValid)
    {
        return BadRequest(ModelState);
        // Or map to ProblemDetails
    }
    
    // Process
}

// With FluentValidation
var validator = new CreateProjectDtoValidator();
var result = validator.Validate(dto);

if (!result.IsValid)
{
    return BadRequest(new ProblemDetails
    {
        Title = "Validation Failed",
        Extensions = new Dictionary<string, object?>
        {
            { "errors", result.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(x => x.Key, x => x.Select(e => e.ErrorMessage).ToArray())
            }
        }
    });
}
```

---

## Q2.10: Controllers - architektura, routing, atrybuty

**SHORT:**
**PATTERN**: Class per resource. Routes with attributes. DI in constructor. Actions return IActionResult.

**FULL ANSWER:**

### Basic Controller
```csharp
[ApiController]
[Route("api/[controller]")]  // /api/projects
public class ProjectsController : ControllerBase
{
    private readonly IProjectService service;
    
    // DI
    public ProjectsController(IProjectService service)
    {
        this.service = service;
    }
    
    // Actions
    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetAll()
    {
        var projects = await service.GetAllAsync();
        return Ok(projects);  // 200 OK
    }
}
```

### Routing Attributes
```csharp
[ApiController]
[Route("api/v1/[controller]")]  // Versioning
public class ProjectsController : ControllerBase
{
    // GET /api/v1/projects
    [HttpGet]
    public async Task<IActionResult> GetAll() { }
    
    // GET /api/v1/projects/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id) { }
    
    // GET /api/v1/projects/1/tasks
    [HttpGet("{id}/tasks")]
    public async Task<IActionResult> GetTasks(int id) { }
    
    // POST /api/v1/projects
    [HttpPost]
    public async Task<IActionResult> Create(CreateProjectDto dto) { }
    
    // PUT /api/v1/projects/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProjectDto dto) { }
    
    // DELETE /api/v1/projects/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) { }
}
```

### Action Return Types
```csharp
// IActionResult (explicit)
public IActionResult GetProject(int id)
{
    var project = db.Projects.Find(id);
    if (project == null)
        return NotFound();
    return Ok(project);
}

// ActionResult<T> (typed)
public ActionResult<ProjectDto> GetProject(int id)
{
    var project = db.Projects.Find(id);
    if (project == null)
        return NotFound();
    return project;  // Implicitly wrapped in Ok
}

// Task<ActionResult<T>> (async)
public async Task<ActionResult<ProjectDto>> GetProjectAsync(int id)
{
    var project = await db.Projects.FindAsync(id);
    return project ?? NotFound();
}
```

### Model Binding
```csharp
[HttpPost]
public IActionResult Create(
    [FromBody] CreateProjectDto dto,      // JSON body
    [FromRoute] int id,                   // URL route
    [FromQuery] string? filter,           // Query string
    [FromHeader] string authorization)   // HTTP header
{
    // All bound automatically
}

// [FromBody] is default for POST/PUT/PATCH
[HttpPost]
public IActionResult Create(CreateProjectDto dto)  // From body implicitly
{
}
```

### Validation
```csharp
[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    [HttpPost]
    public IActionResult Create(CreateProjectDto dto)
    {
        // ModelState.IsValid set automatically
        if (!ModelState.IsValid)
            return BadRequest(ModelState);  // 400
        
        // Valid, process
    }
}

// Or with ApiBehaviorOptions (auto validation)
services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = false;  // Default
    });
```

---

## 📌 SUMMARY - WEB API

| Topic | Key Concept |
|-------|------------|
| HTTP Methods | GET/POST/PUT/DELETE/PATCH + semantics |
| Status Codes | 2xx success, 4xx client error, 5xx server |
| REST | Resource-based URLs, stateless, HATEOAS |
| DTOs | Separate API from domain model |
| Serialization | JSON ↔ C# objects, automatic in ASP.NET |
| Validation | Input validation, FluentValidation |
| Error Handling | Global middleware, ProblemDetails standard |
| Controllers | Routes, model binding, return types |

