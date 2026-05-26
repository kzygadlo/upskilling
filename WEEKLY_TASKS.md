# 📅 Weekly Tasks & Checkpoints

This file contains all tasks for the 12-week upskilling program. **Copy-paste these into GitHub Issues** as you progress.

---

## Week 1-2: Fundamentals & Setup

### Tasks:
- [ ] Install .NET 9 SDK (if not already installed)
- [ ] Install Visual Studio 2024 Community (or use VS Code + C# extension)
- [ ] Create Azure SQL Database (free tier)
- [ ] Read: "What's new in .NET 9" (microsoft.com)
- [ ] Read: "Angular 17+ Signals" (angular.io documentation)
- [ ] Clone skeleton project & run locally (both backend + frontend)
- [ ] Git basic workflow refresh (commit, push, pull, branches)
- [ ] Setup VS Code extensions (C#, Angular, REST Client, Docker)

### Deliverable:
- [ ] Backend API runs on http://localhost:5000
- [ ] Frontend runs on http://localhost:4200
- [ ] Database connection works
- [ ] Git commits working properly

**Checkpoint**: Run `dotnet run` (backend) and `npm start` (frontend) successfully

---

## Week 3: REST APIs & C# Fundamentals

### Learn:
- [ ] REST API design principles (JSON:API, OpenAPI specs)
- [ ] HTTP status codes & best practices (2xx, 4xx, 5xx semantics)
- [ ] Entity Framework Core 8
  - [ ] DbContext setup & migrations
  - [ ] Relationships (1-N, M-N)
  - [ ] Query optimization (Include, Select, NoTracking)
- [ ] Dependency Injection in ASP.NET Core
  - [ ] Scoped vs Transient vs Singleton
  - [ ] Constructor injection
  - [ ] Service registration
- [ ] Clean Architecture layers & SOLID principles (intro)

### Code Tasks:
- [ ] Create Entity classes (Project, Task, User)
  ```csharp
  public class Project
  {
      public int Id { get; set; }
      public string Name { get; set; }
      public ICollection<TaskItem> Tasks { get; set; }
  }
  ```

- [ ] Create DbContext in Infrastructure layer
  ```csharp
  public class TaskManagerContext : DbContext
  {
      public DbSet<Project> Projects { get; set; }
      public DbSet<TaskItem> Tasks { get; set; }
  }
  ```

- [ ] Create first migrations
  ```bash
  dotnet ef migrations add InitialCreate
  dotnet ef database update
  ```

- [ ] Create ProjectController with basic endpoints
  ```csharp
  [ApiController]
  [Route("api/[controller]")]
  public class ProjectsController : ControllerBase
  {
      [HttpGet] public async Task<List<ProjectDto>> GetAll() => ...
      [HttpPost] public async Task<ProjectDto> Create(CreateProjectDto dto) => ...
  }
  ```

- [ ] Create Repository pattern for data access
- [ ] Create Service layer for business logic
- [ ] Add simple logging

### Tests:
- [ ] Unit test: ProjectService.CreateAsync() returns valid ProjectDto
- [ ] Unit test: ProjectService.GetAsync(id) throws NotFoundException for invalid id

**Checkpoint**: 
```
GET /api/projects → returns []
POST /api/projects → creates new project
```

---

## Week 4: Advanced C# & Design Patterns

### Learn:
- [ ] SOLID principles in detail
  - [ ] Single Responsibility Principle
  - [ ] Open/Closed Principle
  - [ ] Liskov Substitution
  - [ ] Interface Segregation
  - [ ] Dependency Inversion
- [ ] Design patterns:
  - [ ] Factory Pattern
  - [ ] Strategy Pattern
  - [ ] Decorator Pattern
  - [ ] Observer Pattern
- [ ] Async/await pitfalls (deadlocks, ConfigureAwait)
- [ ] Exception handling strategies
- [ ] Structured logging (Serilog or built-in ILogger)

### Code Tasks:
- [ ] Refactor ProjectService using Factory pattern
  ```csharp
  public interface IProjectFactory
  {
      Project CreateProject(string name);
  }
  ```

- [ ] Add fluent validation to DTOs
  ```csharp
  public class CreateProjectValidator : AbstractValidator<CreateProjectDto>
  {
      public CreateProjectValidator()
      {
          RuleFor(x => x.Name)
              .NotEmpty().WithMessage("Name is required")
              .MaximumLength(100);
      }
  }
  ```

- [ ] Add global exception handling middleware
  ```csharp
  app.UseExceptionHandler("/error");
  app.UseStatusCodePages();
  ```

- [ ] Add structured logging to all major operations
  ```csharp
  _logger.LogInformation("Creating project: {@ProjectName}", projectName);
  ```

- [ ] Implement custom exception classes
  ```csharp
  public class ProjectNotFoundException : Exception { }
  public class ValidationException : Exception { }
  ```

### Tests:
- [ ] Test invalid CreateProjectDto fails validation
- [ ] Test ProjectFactory creates valid projects
- [ ] Test exception handler returns proper status codes

**Checkpoint**: API returns proper error responses (400, 404, 500 with meaningful messages)

---

## Week 5: Security & Authentication

### Learn:
- [ ] Authentication vs Authorization concepts
- [ ] JWT (JSON Web Token) basics
- [ ] OAuth 2.0 fundamentals
- [ ] Azure AD & MSAL (Microsoft Authentication Library)
  - [ ] Register app in Azure AD
  - [ ] MSAL client library setup
  - [ ] Token validation
- [ ] CORS (Cross-Origin Resource Sharing) configuration
- [ ] CSRF protection
- [ ] Secure headers (Content-Security-Policy, X-Frame-Options, etc.)
- [ ] Password hashing best practices
- [ ] Rate limiting

### Code Tasks:
- [ ] Register API app in Azure AD
  - [ ] Get Client ID, Tenant ID, Scope
  - [ ] Configure API permissions

- [ ] Add JWT bearer authentication to API
  ```csharp
  services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"));
  
  app.UseAuthentication();
  app.UseAuthorization();
  ```

- [ ] Add [Authorize] to protected endpoints
  ```csharp
  [Authorize]
  [HttpPost("api/projects")]
  public async Task<ActionResult> CreateProject(CreateProjectDto dto) => ...
  ```

- [ ] Configure CORS for frontend
  ```csharp
  services.AddCors(options =>
  {
      options.AddPolicy("Angular", policy =>
          policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader());
  });
  ```

- [ ] Add secure headers middleware
  ```csharp
  app.Use(async (context, next) =>
  {
      context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
      context.Response.Headers.Add("X-Frame-Options", "DENY");
      await next();
  });
  ```

- [ ] Create UserService with authentication logic
- [ ] Add integration tests for auth flows

### Tests:
- [ ] Test unauthorized access returns 401
- [ ] Test [Authorize] attribute blocks unauthenticated requests
- [ ] Test valid token allows access
- [ ] Test expired token returns 401

**Checkpoint**:
```
POST /api/projects (no auth) → 401 Unauthorized
POST /api/projects (with token) → 201 Created
```

---

## Week 6: RxJS & Reactive Programming (Angular)

### Learn:
- [ ] Observable vs Promise
- [ ] Hot vs Cold observables
- [ ] Common RxJS operators:
  - [ ] map, filter, switchMap, mergeMap
  - [ ] debounceTime, throttleTime
  - [ ] tap, catchError, throwError
  - [ ] takeUntil
  - [ ] combineLatest, forkJoin
- [ ] Memory leak prevention (unsubscribe patterns)
- [ ] HttpClient + interceptors
- [ ] Error handling in streams
- [ ] Subject (BehaviorSubject, ReplaySubject)

### Code Tasks:
- [ ] Create ApiService using HttpClient
  ```typescript
  @Injectable({ providedIn: 'root' })
  export class ApiService {
    constructor(private http: HttpClient) {}
    
    getProjects(): Observable<Project[]> {
      return this.http.get<Project[]>('/api/projects');
    }
  }
  ```

- [ ] Create auth interceptor for token injection
  ```typescript
  @Injectable()
  export class AuthInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      const token = sessionStorage.getItem('token');
      if (token) {
        req = req.clone({
          setHeaders: { Authorization: `Bearer ${token}` }
        });
      }
      return next.handle(req);
    }
  }
  ```

- [ ] Create error handling interceptor
  ```typescript
  @Injectable()
  export class ErrorInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      return next.handle(req).pipe(
        catchError((error: HttpErrorResponse) => {
          console.error('API error:', error.message);
          return throwError(() => new Error('API Error'));
        })
      );
    }
  }
  ```

- [ ] Create ProjectService with RxJS operators
  ```typescript
  getProjects(): Observable<Project[]> {
    return this.api.getProjects().pipe(
      map(projects => projects.sort((a, b) => a.name.localeCompare(b.name))),
      catchError(err => {
        console.error('Failed to load projects', err);
        return of([]);
      })
    );
  }
  ```

- [ ] Create error handling service
  ```typescript
  handleError(error: HttpErrorResponse) {
    if (error.status === 401) {
      // Redirect to login
    } else if (error.status === 404) {
      // Show not found message
    }
  }
  ```

- [ ] Implement takeUntil for unsubscribe management
  ```typescript
  private destroy$ = new Subject<void>();
  
  ngOnInit() {
    this.projects$.pipe(
      takeUntil(this.destroy$)
    ).subscribe(...);
  }
  
  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
  ```

### Tests:
- [ ] Test ApiService.getProjects() returns projects
- [ ] Test error handling returns empty array on failure
- [ ] Test takeUntil unsubscribes properly

**Checkpoint**: 
```
API calls work with proper error handling
No subscription memory leaks
Auth tokens injected automatically
```

---

## Week 7: Signals & Standalone Components (Angular)

### Learn:
- [ ] Signal() API (Angular 17+)
  - [ ] signal() creation
  - [ ] computed() signals
  - [ ] effect() side effects
  - [ ] input() & output()
- [ ] Standalone components
  - [ ] @Component({ standalone: true })
  - [ ] Import directives/components directly
  - [ ] Standalone routing
- [ ] Change detection strategies (OnPush with Signals)
- [ ] Signal vs Observable trade-offs

### Code Tasks:
- [ ] Convert ProjectListComponent to standalone
  ```typescript
  @Component({
    selector: 'app-project-list',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './project-list.component.html'
  })
  export class ProjectListComponent {
    projects = signal<Project[]>([]);
    
    constructor(private projectService: ProjectService) {
      effect(() => {
        console.log('Projects changed:', this.projects());
      });
    }
  }
  ```

- [ ] Create computed signal for filtered projects
  ```typescript
  searchTerm = signal('');
  filteredProjects = computed(() => {
    const term = this.searchTerm().toLowerCase();
    return this.projects().filter(p => 
      p.name.toLowerCase().includes(term)
    );
  });
  ```

- [ ] Refactor components with OnPush strategy
  ```typescript
  @Component({
    selector: 'app-project-card',
    standalone: true,
    changeDetection: ChangeDetectionStrategy.OnPush
  })
  ```

- [ ] Create signal-based service
  ```typescript
  export class ProjectSignalService {
    projects = signal<Project[]>([]);
    isLoading = signal(false);
    
    loadProjects() {
      this.isLoading.set(true);
      this.projectService.getProjects().subscribe(data => {
        this.projects.set(data);
        this.isLoading.set(false);
      });
    }
  }
  ```

### Tests:
- [ ] Test computed signal filters correctly
- [ ] Test effect() runs on signal update
- [ ] Test OnPush change detection works

**Checkpoint**: 
```
Components are standalone
Signals used for state
No ChangeDetectionStrategy.Default
```

---

## Week 8: NgRx & State Management

### Learn:
- [ ] NgRx fundamentals:
  - [ ] Actions
  - [ ] Reducers
  - [ ] Effects (side effects)
  - [ ] Selectors
- [ ] Store architecture
  - [ ] Feature stores
  - [ ] Entity adapters
  - [ ] Lazy-loaded features
- [ ] Best practices (feature-first structure)
- [ ] Testing effects (marble testing)

### Code Tasks:
- [ ] Create NgRx actions
  ```typescript
  // projects.actions.ts
  export const loadProjects = createAction(
    '[Projects Page] Load Projects'
  );
  
  export const loadProjectsSuccess = createAction(
    '[Projects Effect] Load Projects Success',
    props<{ projects: Project[] }>()
  );
  
  export const loadProjectsFailure = createAction(
    '[Projects Effect] Load Projects Failure',
    props<{ error: string }>()
  );
  ```

- [ ] Create reducer
  ```typescript
  export const projectsFeatureKey = 'projects';
  
  export interface ProjectsState {
    entities: Project[];
    isLoading: boolean;
    error: string | null;
  }
  
  export const projectsReducer = createReducer(
    initialState,
    on(loadProjects, state => ({ ...state, isLoading: true })),
    on(loadProjectsSuccess, (state, { projects }) => ({
      ...state,
      entities: projects,
      isLoading: false
    })),
    on(loadProjectsFailure, (state, { error }) => ({
      ...state,
      error,
      isLoading: false
    }))
  );
  ```

- [ ] Create effects
  ```typescript
  @Injectable()
  export class ProjectsEffects {
    loadProjects$ = createEffect(() =>
      this.actions$.pipe(
        ofType(loadProjects),
        switchMap(() =>
          this.projectService.getProjects().pipe(
            map(projects => loadProjectsSuccess({ projects })),
            catchError(error => of(loadProjectsFailure({ error: error.message })))
          )
        )
      )
    );
    
    constructor(
      private actions$: Actions,
      private projectService: ProjectService
    ) {}
  }
  ```

- [ ] Create selectors
  ```typescript
  export const selectProjectsState = createFeatureSelector<ProjectsState>(
    projectsFeatureKey
  );
  
  export const selectAllProjects = createSelector(
    selectProjectsState,
    state => state.entities
  );
  
  export const selectIsLoading = createSelector(
    selectProjectsState,
    state => state.isLoading
  );
  ```

- [ ] Integrate into component
  ```typescript
  export class ProjectListComponent {
    projects$ = this.store.select(selectAllProjects);
    isLoading$ = this.store.select(selectIsLoading);
    
    constructor(private store: Store) {}
    
    ngOnInit() {
      this.store.dispatch(loadProjects());
    }
  }
  ```

### Tests:
- [ ] Unit test reducer handles actions correctly
- [ ] Test effects dispatch success action on API call
- [ ] Test selectors return correct state slice
- [ ] Test marble testing for effects

**Checkpoint**:
```
NgRx store dispatches actions
Effects call API correctly
Selectors memoized properly
Devtools show action flow
```

---

## Week 9: Frontend Security & Advanced CSS

### Learn:
- [ ] XSS (Cross-Site Scripting) prevention
  - [ ] DomSanitizer usage
  - [ ] Content Security Policy (CSP)
  - [ ] Sanitization vs Escaping
- [ ] Token secure storage (sessionStorage vs localStorage)
- [ ] SCSS preprocessor
  - [ ] Variables, mixins, nesting
  - [ ] Functions & operations
- [ ] CSS theming (light/dark mode)
- [ ] Responsive design (mobile-first)
- [ ] Accessibility (A11y)
  - [ ] ARIA labels
  - [ ] Semantic HTML
  - [ ] Keyboard navigation

### Code Tasks:
- [ ] Use DomSanitizer for user content
  ```typescript
  constructor(private sanitizer: DomSanitizer) {}
  
  getSafeHtml(html: string) {
    return this.sanitizer.sanitize(SecurityContext.HTML, html);
  }
  ```

- [ ] Move tokens to sessionStorage (not localStorage)
  ```typescript
  saveToken(token: string) {
    sessionStorage.setItem('auth_token', token);
  }
  
  getToken(): string | null {
    return sessionStorage.getItem('auth_token');
  }
  ```

- [ ] Create SCSS theming system
  ```scss
  // themes.scss
  $light-theme: (
    primary: #1976d2,
    secondary: #ff4081,
    background: #ffffff
  );
  
  $dark-theme: (
    primary: #64b5f6,
    secondary: #ff80ab,
    background: #121212
  );
  
  @mixin themed-color($property, $color-key) {
    #{$property}: map-get($light-theme, $color-key);
    
    [data-theme="dark"] & {
      #{$property}: map-get($dark-theme, $color-key);
    }
  }
  ```

- [ ] Implement light/dark mode toggle
  ```typescript
  toggleTheme() {
    const current = document.documentElement.getAttribute('data-theme');
    const next = current === 'dark' ? 'light' : 'dark';
    document.documentElement.setAttribute('data-theme', next);
    localStorage.setItem('theme', next);
  }
  ```

- [ ] Create responsive layout
  ```scss
  .container {
    padding: 1rem;
    
    @media (min-width: 768px) {
      padding: 2rem;
    }
    
    @media (min-width: 1024px) {
      padding: 3rem;
    }
  }
  ```

- [ ] Add accessibility attributes
  ```html
  <button aria-label="Create new project" 
          aria-describedby="create-help"
          [attr.disabled]="isDisabled || null">
    <i aria-hidden="true" class="icon-plus"></i>
    Add Project
  </button>
  <small id="create-help">Click to add a new project</small>
  ```

### Tests:
- [ ] Test DomSanitizer removes malicious scripts
- [ ] Test theme toggle persists
- [ ] Test ARIA labels present
- [ ] Test keyboard navigation works

**Checkpoint**:
```
No XSS vulnerabilities
Theme toggles properly
Fully accessible keyboard navigation
Mobile responsive layout
```

---

## Week 10: DevOps & Deployment

### Learn:
- [ ] Docker fundamentals
  - [ ] Dockerfile syntax
  - [ ] Image vs Container
  - [ ] Layers & caching
- [ ] Docker Compose (multi-container orchestration)
- [ ] Azure DevOps (CI/CD)
  - [ ] Pipelines (YAML)
  - [ ] Build steps
  - [ ] Deploy to App Service
- [ ] Kubernetes basics (nice-to-have)
  - [ ] Deployments, Services, ConfigMaps

### Code Tasks:
- [ ] Create Backend Dockerfile
  ```dockerfile
  # Build stage
  FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
  WORKDIR /src
  COPY ["src/TaskManager.API/TaskManager.API.csproj", "src/TaskManager.API/"]
  RUN dotnet restore "src/TaskManager.API/TaskManager.API.csproj"
  COPY . .
  RUN dotnet build "src/TaskManager.API/TaskManager.API.csproj" -c Release -o /app/build
  
  # Publish stage
  FROM build AS publish
  RUN dotnet publish "src/TaskManager.API/TaskManager.API.csproj" -c Release -o /app/publish
  
  # Runtime stage
  FROM mcr.microsoft.com/dotnet/aspnet:9.0
  WORKDIR /app
  COPY --from=publish /app/publish .
  EXPOSE 8080
  ENTRYPOINT ["dotnet", "TaskManager.API.dll"]
  ```

- [ ] Create Frontend Dockerfile
  ```dockerfile
  # Build stage
  FROM node:20-alpine AS build
  WORKDIR /app
  COPY package*.json ./
  RUN npm ci
  COPY . .
  RUN npm run build
  
  # Runtime stage
  FROM nginx:alpine
  COPY --from=build /app/dist /usr/share/nginx/html
  COPY nginx.conf /etc/nginx/conf.d/default.conf
  EXPOSE 80
  CMD ["nginx", "-g", "daemon off;"]
  ```

- [ ] Create docker-compose.yml (already in repo)
- [ ] Test docker-compose locally
  ```bash
  docker-compose up
  # Test: http://localhost:4200
  ```

- [ ] Create Azure DevOps pipeline (azure-pipelines.yml)
  ```yaml
  trigger:
    - main
  
  pool:
    vmImage: 'ubuntu-latest'
  
  steps:
  - task: UseDotNet@2
    inputs:
      packageType: 'sdk'
      version: '9.0.x'
  
  - task: DotNetCoreCLI@2
    inputs:
      command: 'build'
      projects: 'Backend/**/*.csproj'
  
  - task: DotNetCoreCLI@2
    inputs:
      command: 'test'
      projects: 'Backend/**/*.Tests.csproj'
  ```

- [ ] Deploy to Azure App Service
- [ ] Setup environment variables in Azure

### Tests:
- [ ] Docker image builds successfully
- [ ] docker-compose up works end-to-end
- [ ] Pipeline builds & tests pass
- [ ] Deployment successful to Azure

**Checkpoint**:
```
docker-compose up → all services running
Azure pipeline passes
App deployed to Azure App Service
```

---

## Week 11-12: Practice & Interview Prep

### Code Review Challenges:
- [ ] Review & refactor a provided C# API code sample
  - Find: exception-driven logic, tight coupling, N+1 queries
  - Fix & explain improvements

- [ ] Review & refactor Angular component code
  - Find: memory leaks, unnecessary change detection, poor reactivity
  - Fix & explain improvements

### Performance Optimization:
- [ ] Identify N+1 queries in EF Core code
  ```csharp
  // ❌ BAD: N+1 query problem
  var projects = await _context.Projects.ToListAsync();
  foreach (var project in projects)
  {
      var tasks = await _context.Tasks
          .Where(t => t.ProjectId == project.Id)
          .ToListAsync(); // N queries!
  }
  
  // ✅ GOOD: Single query with Include
  var projects = await _context.Projects
      .Include(p => p.Tasks)
      .ToListAsync(); // 1 query
  ```

- [ ] Optimize Angular change detection
  ```typescript
  // ❌ BAD: Default change detection
  @Component({
    selector: 'app-list',
    template: '<div>{{ items$ | async }}</div>'
  })
  
  // ✅ GOOD: OnPush strategy
  @Component({
    selector: 'app-list',
    template: '<div>{{ items$ | async }}</div>',
    changeDetection: ChangeDetectionStrategy.OnPush
  })
  ```

### Edge Cases & Error Scenarios:
- [ ] Handle race conditions in API calls
- [ ] Handle concurrent modifications
- [ ] Handle network failures gracefully
- [ ] Handle timeout scenarios

### System Design:
- [ ] Design scalable project management API
- [ ] Design caching strategy for projects
- [ ] Design real-time updates (SignalR)

### Application Polishing:
- [ ] Add comprehensive error messages
- [ ] Add loading indicators
- [ ] Add success notifications
- [ ] Add input validation feedback
- [ ] Add keyboard shortcuts
- [ ] Add help/tooltips

### Documentation:
- [ ] Write API documentation (OpenAPI/Swagger)
- [ ] Write deployment guide
- [ ] Write contribution guide
- [ ] Add code comments for complex logic

### Final Review:
- [ ] All tests passing (backend + frontend)
- [ ] Code coverage > 70%
- [ ] No console errors/warnings
- [ ] No accessibility issues
- [ ] Responsive on mobile/tablet/desktop
- [ ] Works on Chrome, Firefox, Safari, Edge
- [ ] Performance Lighthouse score > 80
- [ ] No security vulnerabilities (OWASP)

**Checkpoint**:
```
Portfolio-ready application
Interview-ready knowledge
Confident explaining architecture
Ready to discuss trade-offs
```

---

## How to Use This File

1. **Create GitHub Issues** for each week:
   ```bash
   gh issue create --title "Week 1: Fundamentals & Setup" --body "..."
   ```

2. **Track Progress**:
   - Check off items as you complete them
   - Create GitHub Project board (Kanban)
   - Move issues: To Do → In Progress → Code Review → Done

3. **If Stuck**:
   - Ask Claude for deeper explanations
   - Review related documentation
   - Write a focused test case

4. **Celebrate Wins** 🎉
   - Share progress on Twitter/LinkedIn
   - Update portfolio with learnings

---

**Good luck! You've got this!** 💪
