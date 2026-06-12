# 5️⃣ TESTING & DEVOPS - Q&A

---

## Q5.1-Q5.5: Testing Fundamentals

### Q5.1: Unit Tests
**CONCEPT**: Test single method/class in isolation. Fast, many.

```csharp
[Fact]
public void Increment_WithValidValue_ReturnsIncremented()
{
    // Arrange
    var counter = new Counter();
    
    // Act
    counter.Increment();
    
    // Assert
    Assert.Equal(1, counter.Value);
}
```

### Q5.2: Integration Tests
**CONCEPT**: Test multiple components together. Real DB, real HTTP. Slower, fewer.

```csharp
[Fact]
public async Task CreateProject_WithValidData_SavesToDatabase()
{
    // Arrange: Create real DB context
    var dbContext = new TaskManagerDbContext();
    var service = new CreateProjectUseCase(new ProjectRepository(dbContext));
    
    // Act
    var projectId = await service.ExecuteAsync(
        new CreateProjectDto { Name = "Test" });
    
    // Assert
    var saved = await dbContext.Projects.FindAsync(projectId);
    Assert.NotNull(saved);
}
```

### Q5.3: E2E Tests
**CONCEPT**: Full user journey. Browser + API. Slowest, fewest.

```csharp
[Fact]
public async Task UserCanCreateAndViewProject()
{
    // Open browser
    await page.GotoAsync("http://localhost:3000/projects");
    
    // Fill form
    await page.FillAsync("[name=name]", "TestProject");
    
    // Submit
    await page.ClickAsync("button[type=submit]");
    
    // Assert
    var title = await page.GetByText("TestProject");
    Assert.NotNull(title);
}
```

### Q5.4: Test Pyramid
```
        /\
       /  \        E2E (few, slow)
      /────\
     /      \      Integration (some, medium)
    /────────\
   /          \    Unit (many, fast)
  /____________\
```

**Distribution:**
- Unit: 70-80%
- Integration: 15-20%
- E2E: 5-10%

### Q5.5: AAA Pattern
```csharp
[Fact]
public void TestExample()
{
    // Arrange: Setup
    var input = new CreateProjectDto { Name = "Test" };
    var service = new CreateProjectUseCase(mockRepository);
    
    // Act: Execute
    var result = service.Execute(input);
    
    // Assert: Verify
    Assert.NotNull(result);
    mockRepository.Verify(r => r.Add(It.IsAny<Project>()), Times.Once);
}
```

---

## Q5.6-Q5.10: Testing Tools & Techniques

### Q5.6: xUnit
**FRAMEWORK**: .NET testing framework. [Fact] for single, [Theory] for multiple.

```csharp
[Fact]
public void SingleTest() { }

[Theory]
[InlineData(2, 4)]
[InlineData(3, 9)]
public void MultipleTests(int input, int expected)
{
    Assert.Equal(expected, input * input);
}
```

### Q5.7: Moq
**LIBRARY**: Mock objects for testing.

```csharp
var mockRepo = new Mock<IProjectRepository>();
mockRepo
    .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
    .ReturnsAsync(new Project { Id = 1 });

mockRepo.Verify(r => r.GetByIdAsync(1), Times.Once);
```

### Q5.8: Mocking
**CONCEPT**: Fake objects for dependencies.

```csharp
// ❌ Without mocking: Test fails if DB is down
public void TestWithoutMock()
{
    var repo = new ProjectRepository(realDbContext);
    var service = new CreateProjectUseCase(repo);
    service.Execute(new CreateProjectDto { Name = "Test" });
}

// ✓ With mocking: Test isolated
public void TestWithMock()
{
    var mockRepo = new Mock<IProjectRepository>();
    var service = new CreateProjectUseCase(mockRepo.Object);
    service.Execute(new CreateProjectDto { Name = "Test" });
    mockRepo.Verify(r => r.AddAsync(It.IsAny<Project>()));
}
```

### Q5.9: Test Coverage
**METRIC**: Percentage of code executed by tests.

```
80% coverage = good
90%+ = excellent
100% = overkill (edge cases, warnings still possible)
```

Tools: Coverlet, ReportGenerator

### Q5.10: TDD
**METHODOLOGY**: Write test first, then code.

```
1. Write failing test
2. Write minimal code to pass
3. Refactor
4. Repeat
```

Benefits:
- Tests are requirements
- Code is testable by design
- Better coverage

---

## Q5.11-Q5.18: DevOps & Cloud

### Q5.11: Docker
**CONCEPT**: Containerization. App + dependencies in image.

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0
COPY bin/Release/net9.0 /app
ENTRYPOINT ["dotnet", "TaskManager.API.dll"]
```

Benefits:
- Consistency (local = production)
- Isolation (don't conflict)
- Scalability (easy to replicate)

### Q5.12: Dockerfile
**CONCEPT**: Recipe for building image.

```dockerfile
# Multi-stage build (optimize size)
FROM mcr.microsoft.com/dotnet/sdk:9.0 as build
WORKDIR /src
COPY . .
RUN dotnet build

FROM mcr.microsoft.com/dotnet/aspnet:9.0
COPY --from=build /src/bin/Release/net9.0 /app
ENTRYPOINT ["dotnet", "TaskManager.API.dll"]
```

### Q5.13: Docker Compose
**CONCEPT**: Run multiple containers.

```yaml
version: '3.8'
services:
  api:
    build: ./Backend
    ports:
      - "5000:80"
    depends_on:
      - db
  db:
    image: mcr.microsoft.com/mssql/server:2019
    environment:
      SA_PASSWORD: YourPassword!
```

### Q5.14: Kubernetes Basics (AKS)
**CONCEPT**: Orchestrate containers. Deploy, scale, monitor.

```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: api
spec:
  replicas: 3
  template:
    spec:
      containers:
      - name: api
        image: myacr.azurecr.io/taskmanager:latest
        ports:
        - containerPort: 80
```

Key concepts:
- Pod: smallest deployable unit
- Deployment: manage replicas
- Service: network access
- Namespace: logical isolation

### Q5.15: CI/CD
**CONCEPT**: Continuous Integration + Deployment.

**CI:**
- Code pushed → tests run → feedback in 5min
- Catch bugs early

**CD:**
- Tests pass → auto-deploy to staging/prod
- Zero downtime deployment

### Q5.16: GitHub Actions / Azure Pipelines
**TOOLS**: Define CI/CD workflow.

```yaml
# GitHub Actions (.github/workflows/test.yml)
name: Tests
on: [push]
jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      - uses: actions/setup-dotnet@v1
      - run: dotnet test
```

```yaml
# Azure Pipelines (azure-pipelines.yml)
trigger:
  - main
pool:
  vmImage: 'ubuntu-latest'
steps:
  - task: DotNetCoreCLI@2
    inputs:
      command: 'test'
```

### Q5.17: Azure Services
**CONCEPT**: Cloud platform.

- **App Service**: Host web apps (PaaS)
- **Azure SQL**: Managed database
- **Key Vault**: Secrets management
- **Functions**: Serverless compute
- **Container Registry**: Private image registry
- **DevOps**: CI/CD pipelines

### Q5.18: Infrastructure as Code (IaC)
**CONCEPT**: Define infrastructure in code.

```hcl
# Terraform
resource "azurerm_app_service" "api" {
  name                = "taskmanager-api"
  resource_group_name = azurerm_resource_group.rg.name
  app_service_plan_id = azurerm_app_service_plan.plan.id
}
```

Benefits:
- Version controlled
- Repeatable
- Auditable
- Automated

---

## Q5.19-Q5.20: Monitoring & Logging

### Q5.19: Logging Best Practices
```csharp
// Structured logging (JSON)
logger.LogInformation("Project created: {@Project}", project);
// Output: {"timestamp":"...", "message":"Project created", "Project": {...}}

// Log levels
logger.LogDebug("Debug info");      // Development
logger.LogInformation("Info");      // Normal operation
logger.LogWarning("Warning");       // Something unusual
logger.LogError("Error", exception);// Error occurred
logger.LogCritical("Critical");     // System failing

// Don't log
// - Passwords
// - Credit cards
// - API keys (use sanitization)
```

### Q5.20: Monitoring
**CONCEPT**: Observe system health.

```csharp
// Metrics (numbers)
- Request count
- Response time (p50, p95, p99)
- Error rate
- CPU/Memory usage
- Database query time

// Alerts
if (ErrorRate > 5%) → Alert
if (ResponseTime.P99 > 5s) → Alert
if (Memory > 80%) → Alert

// Dashboards
- Real-time metrics
- Historical trends
- Anomaly detection
```

Tools: Prometheus, Grafana, Application Insights, New Relic, Datadog

---

## 📌 SUMMARY - TESTING & DEVOPS

| Topic | Key Concept |
|-------|------------|
| Unit Tests | Single method, isolated, many, fast |
| Integration Tests | Multiple components, real DB, fewer |
| E2E Tests | Full user journey, slowest, fewest |
| Test Pyramid | 70% unit, 15% integration, 5% E2E |
| Mocking | Fake dependencies for isolated tests |
| xUnit | .NET testing framework |
| Docker | Container image, consistency, isolation |
| K8s (AKS) | Orchestrate containers, deploy, scale |
| CI/CD | Automate test + deploy on every push |
| IaC | Infrastructure in code, version controlled |
| Monitoring | Metrics, logs, alerts, dashboards |
| Logging | Structured, log levels, sanitization |

