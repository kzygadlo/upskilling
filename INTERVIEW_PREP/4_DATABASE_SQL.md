# 4️⃣ DATABASE & SQL - Q&A

---

## Q4.1-Q4.6: SQL Basics (Brief Overview)

### Q4.1: SELECT, WHERE, GROUP BY basics
```sql
SELECT Name, COUNT(*) as TaskCount
FROM Projects
WHERE Status = 'Active'
GROUP BY Name
HAVING COUNT(*) > 5
ORDER BY TaskCount DESC;
```

### Q4.2: INNER JOIN vs LEFT JOIN
- **INNER**: Both tables have matching row
- **LEFT**: All from left table, matching from right (nulls if no match)

### Q4.3: Aggregate functions
- SUM(), COUNT(), AVG(), MIN(), MAX()

### Q4.4: Subqueries
```sql
SELECT * FROM Projects WHERE Id IN (SELECT ProjectId FROM Tasks);
```

### Q4.5: CTEs (Common Table Expressions)
```sql
WITH ActiveProjects AS (
    SELECT * FROM Projects WHERE Status = 'Active'
)
SELECT * FROM ActiveProjects;
```

### Q4.6: Window functions
- ROW_NUMBER(), RANK(), LEAD(), LAG()

---

## Q4.7-Q4.12: Database Design

### Q4.7: Primary Key
**CONCEPT**: Unique identifier for row. Ensures no duplicates. Creates clustered index.

```sql
CREATE TABLE Projects (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);
```

### Q4.8: Foreign Keys
**CONCEPT**: Referential integrity. TaskId must exist in Tasks table.

```sql
CREATE TABLE Tasks (
    Id INT PRIMARY KEY,
    ProjectId INT NOT NULL,
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id)
);
```

### Q4.9: Relationships
```
One-to-Many:
Project (1) ←→ (many) Tasks
  ├─ Projects.Id (PK)
  └─ Tasks.ProjectId (FK)

Many-to-Many:
Project (many) ←→ (many) User
  └─ ProjectUser table
      ├─ ProjectId (FK)
      └─ UserId (FK)

One-to-One:
User (1) ←→ (1) Profile
  ├─ Users.Id (PK)
  └─ Profiles.UserId (FK, UNIQUE)
```

### Q4.10: Normalization
**CONCEPT**: Eliminate redundancy. 3NF = good for OLTP.

```csharp
// ❌ Denormalized (redundant)
Projects:
  Id | Name | ManagerName | ManagerEmail | ManagerPhone

// ✓ Normalized (3NF)
Projects:
  Id | Name | ManagerId

Managers:
  Id | Name | Email | Phone
```

### Q4.11: Denormalization
**CONCEPT**: Add redundancy for performance. 1 query instead of 2 joins.

```csharp
// Denormalized (for reporting)
Projects:
  Id | Name | ManagerName | TaskCount | TotalBudget
  
// Duplicate data, but fast reads
// Trade: consistency vs performance
```

### Q4.12: Transactions
**CONCEPT**: ACID. All-or-nothing.

```csharp
BEGIN TRANSACTION
  UPDATE Accounts SET Balance = Balance - 100 WHERE Id = 1;
  UPDATE Accounts SET Balance = Balance + 100 WHERE Id = 2;
COMMIT TRANSACTION;
// Both succeed or both fail
```

---

## Q4.13-Q4.19: Performance & Optimization

### Q4.13: Indexes
**CONCEPT**: B-tree structure. Fast lookup. Trade: slower writes, more storage.

```sql
CREATE INDEX idx_ProjectId ON Tasks(ProjectId);
// Speeds up: WHERE ProjectId = 1
// Slows down: INSERT, UPDATE, DELETE
```

### Q4.14: Clustered vs Non-clustered
```
Clustered:
  - One per table
  - Determines physical row order
  - Contains entire row
  - Fastest

Non-clustered:
  - Many per table
  - Separate structure + lookup to row
  - Contains indexed columns + bookmark
  - Slower, but more flexibility
```

### Q4.15: Execution Plan
**CONCEPT**: Query optimizer's strategy. Seek (fast) vs Scan (slow).

```sql
-- Look for:
SELECT * FROM Projects WHERE Id = 1;
-- Index Seek ✓ (fast)

SELECT * FROM Projects WHERE Name LIKE '%Task%';
-- Table Scan ✗ (slow, no index on substring)
```

### Q4.16: Query Profiler
**CONCEPT**: Measure real performance. Time, I/O, CPU.

```sql
SET STATISTICS TIME ON;
SET STATISTICS IO ON;
SELECT * FROM Projects WHERE Status = 'Active';
-- Check: CPU time, I/O operations
```

### Q4.17: Statistics
**CONCEPT**: Row count distribution. Query planner uses this.

```sql
-- Stale statistics = bad query plan
UPDATE STATISTICS Projects;
-- After bulk insert/delete: update stats
```

### Q4.18: N+1 Problem
**CONCEPT**: 1 query for parent + N queries for children.

```csharp
// ❌ N+1
var projects = db.Projects.ToList();  // 1 query
foreach (var p in projects)
{
    var tasks = db.Tasks.Where(t => t.ProjectId == p.Id).ToList();  // +N queries
}

// ✓ Fix: Include (1 query with JOIN)
var projects = db.Projects.Include(p => p.Tasks).ToList();
```

### Q4.19: Slow Queries - Debug
```
1. Execution plan (seek vs scan?)
2. Profiler (where is time spent?)
3. Indexes (missing on WHERE/JOIN columns?)
4. Statistics (accurate row counts?)
5. Rewrite (simplify query, split, cache result)
```

---

## Q4.20-Q4.25: EF Core & Data Access

### Q4.20: DbContext
**CONCEPT**: Session with database. Change Tracker monitors objects.

```csharp
public class TaskManagerDbContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent API configuration
    }
}
```

### Q4.21: Migrations
**CONCEPT**: Version control for schema. C# code → SQL.

```bash
dotnet ef migrations add AddProjectTable
dotnet ef database update
// Rollback: dotnet ef database update PreviousMigration
```

### Q4.22: Change Tracking
**CONCEPT**: EF knows what changed, generates UPDATE.

```csharp
var project = db.Projects.Find(1);
project.Name = "NewName";
db.SaveChanges();
// EF generates: UPDATE Projects SET Name = 'NewName' WHERE Id = 1
```

### Q4.23: Lazy Loading, Eager Loading
```csharp
// Lazy Loading (multiple queries)
var project = db.Projects.Find(1);
var taskCount = project.Tasks.Count;  // Separate query

// Eager Loading (single query with JOIN)
var project = db.Projects
    .Include(p => p.Tasks)
    .FirstOrDefault(p => p.Id == 1);
var taskCount = project.Tasks.Count;  // No additional query

// Explicit Loading
var project = db.Projects.Find(1);
db.Entry(project).Collection(p => p.Tasks).Load();  // Force load
```

### Q4.24: Projections
**CONCEPT**: Select only needed fields. Faster, less memory.

```csharp
// ❌ Load entire entities
var projects = db.Projects
    .Where(p => p.Active)
    .ToList();  // 100 projects × 10 properties

// ✓ Project only needed
var projectNames = db.Projects
    .Where(p => p.Active)
    .Select(p => new { p.Id, p.Name })
    .ToList();  // Only 2 properties
```

### Q4.25: Dapper vs EF Core
```
EF Core:
  ✓ Automatic change tracking
  ✓ LINQ support
  ✓ Migrations
  ✗ Slower
  ✗ Overhead
  → For complex business logic + writes

Dapper:
  ✓ Fast (minimal overhead)
  ✓ Raw SQL control
  ✓ Light
  ✗ No change tracking
  ✗ No migrations
  → For performance-critical reads, complex queries
```

---

## 📌 SUMMARY - DATABASE & SQL

| Topic | Key Concept |
|-------|------------|
| SQL Basics | SELECT, JOIN, WHERE, GROUP BY, aggregate functions |
| Keys | PK = unique, FK = referential integrity |
| Relationships | 1:1, 1:N, N:N via junction table |
| Normalization | 3NF = good OLTP, eliminate redundancy |
| Indexes | B-tree, fast reads, slow writes, need maintenance |
| N+1 Problem | Include() to load related in 1 query |
| Change Tracking | EF knows what changed, generates UPDATE |
| Projections | Select() only needed fields, faster |
| Performance | Profile before optimizing, measure impact |

