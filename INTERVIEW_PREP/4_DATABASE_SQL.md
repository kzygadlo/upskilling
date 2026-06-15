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
- **WINDOW QUERY definicja**: Operuje na zbiorze wierszy bez GROUP BY, zachowując poszczególne wiersze

```sql
-- ROW_NUMBER: numerowanie wierszy
SELECT 
    Id, ProjectId, Title,
    ROW_NUMBER() OVER (PARTITION BY ProjectId ORDER BY CreatedDate) as TaskNumber
FROM Tasks;

-- COUNT OVER: agregat bez GROUP BY
SELECT 
    Id, ProjectId, Title,
    COUNT(*) OVER (PARTITION BY ProjectId) as ProjectTaskCount
FROM Tasks;

-- LEAD/LAG: poprzednia/następna wartość
SELECT 
    Id, Title, CreatedDate,
    LAG(CreatedDate) OVER (ORDER BY CreatedDate) as PreviousTaskDate,
    LEAD(CreatedDate) OVER (ORDER BY CreatedDate) as NextTaskDate
FROM Tasks;

-- RANK vs DENSE_RANK
SELECT 
    Id, Points,
    RANK() OVER (ORDER BY Points DESC) as Rank,              -- 1, 2, 2, 4
    DENSE_RANK() OVER (ORDER BY Points DESC) as DenseRank    -- 1, 2, 2, 3
FROM Users;
```

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

### Q4.12: Transactions & ACID
**CONCEPT**: ACID. All-or-nothing.

```csharp
BEGIN TRANSACTION
  UPDATE Accounts SET Balance = Balance - 100 WHERE Id = 1;
  UPDATE Accounts SET Balance = Balance + 100 WHERE Id = 2;
COMMIT TRANSACTION;
// Both succeed or both fail
```

**ACID Principles**:
- **A - Atomicity** (Atomowość): Transakcja to wszystko albo nic. Brak częściowych zmian.
- **C - Consistency** (Spójność): Dane pozostają w spójnym stanie. Reguły biznesowe zawsze zachowane.
- **I - Isolation** (Izolacja): Równoczesne transakcje się nie zakłócają.
- **D - Durability** (Trwałość): Co się zacommituje, nigdy się nie zgubi (nawet po wypadzie).

---

## 🔄 Concurrency Control: Locks, Isolation Levels, Optimistic/Pessimistic

### Q4.12a: Rodzaje Lockow (Locking)

```sql
-- SHARED LOCK (S)
-- Wiele transakcji może czytać równocześnie, żaden update
SELECT * FROM Projects WITH (NOLOCK);

-- EXCLUSIVE LOCK (X)
-- Tylko jedna transakcja, żaden inny access
UPDATE Projects SET Name = 'New' WHERE Id = 1;

-- INTENT LOCK (IS, IX)
-- "Będę chcieć X lub S lock na tej tabeli/paginie"
-- Zapobiega DROP TABLE gdy komuś potrzebny update

-- DEADLOCK
-- T1: locks Projects, czeka na Tasks
-- T2: locks Tasks, czeka na Projects
-- Database wyrzuca jeden i rollback
```

---

### Q4.12b: Isolation Levels

```sql
-- 1. READ UNCOMMITTED (najniższy)
-- Odczytuje niezacommitowane dane (dirty read)
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
-- ✗ Ryzyko: T1 zmienia X na 100, T2 czyta 100, T1 rollback → X jest 50!
-- ✓ Użycie: Raportowanie przybliżone, gdzie dokładność nie ważna

-- 2. READ COMMITTED (domyślne w SQL Server)
-- Czyta tylko zacommitowane dane
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
-- ✓ Bezpieczne od dirty read
-- ✗ Non-repeatable read: T1 czyta X=50, T2 zmienia X=100, T1 znów czyta X=100

-- 3. REPEATABLE READ
-- Jeśli T1 czyta X, nikt nie może zmienić X
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;
-- ✓ Powtórzony SELECT daje te same dane
-- ✗ Phantom read: SELECT ... WHERE Age > 30 zwraca 10, nowy wiersz dodany, powtórzenie = 11

-- 4. SERIALIZABLE (najwyższy)
-- Transakcje się nie mieszają, jak gdyby działały sekwencyjnie
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
-- ✓ Całkowita izolacja
-- ✗ Wolno, deadlock ryzyko, table locks

-- Kiedy użyć?
READ UNCOMMITTED:  Raportowanie, brudne dane OK
READ COMMITTED:    Większość aplikacji (domyślne)
REPEATABLE READ:   Finansowe, gdzie data consistency krytyczna
SERIALIZABLE:      Ultra-ważne, rzadko (perf issue)
```

---

### Q4.12c: Optimistic vs Pessimistic Locking

```csharp
// PESSIMISTIC (lock early)
// "Założę najgorsze, zaraz ktoś zmieni"
using (var transaction = db.Database.BeginTransaction())
{
    // SQL: SELECT ... WITH (XLOCK) -- Exclusive lock
    var project = db.Projects.FromSqlInterpolated(
        $"SELECT * FROM Projects WITH (XLOCK) WHERE Id = {id}"
    ).FirstOrDefault();
    
    project.Name = "Updated";
    db.SaveChanges();
    transaction.Commit();
    // Lock: od SELECT do COMMIT
    // ✓ Bezpieczny od conflicts
    // ✗ Slow, deadlock risk, locks konkurentów
}

// OPTIMISTIC (trust, check at end)
// "Założę, że nikt nie zmieni, sprawdzę na koniec"
public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    [Timestamp]  // ← SQL: ROWVERSION
    public byte[] RowVersion { get; set; }
}

try
{
    var project = db.Projects.Find(id);
    project.Name = "Updated";
    db.SaveChanges();  // DbUpdateConcurrencyException jeśli ktoś zmienił
}
catch (DbUpdateConcurrencyException)
{
    // Ktoś zmienił zanim my zacommitowaliśmy
    // Retry, merge, lub powiadom user
}

// Kiedy użyć?
PESSIMISTIC:  Bankowe (transfer), mało konkurencji, critical
OPTIMISTIC:   Web app, wiele konkurencji, mniej conflicts
```

---

## 🗄️ Database Types: SQL vs NoSQL vs NewSQL

### Q4.12d: Rodzaje Baz i Kiedy Się Ich Używa

```
SQL (Relational):
├─ Przykład: SQL Server, PostgreSQL, MySQL
├─ Model: Tabele z constraints (PK, FK, ACID)
├─ Query: SQL (SELECT, JOIN)
├─ Skalowanie: Vertical (szybszy serwer)
├─ ✓ ACID transakcje, normalizacja, relacje
├─ ✓ Structured data (forma zdefiniowana)
├─ ✗ Trudne rozproszone systemy
├─ ✗ Schemaless (zmiana schematu = ALTER TABLE)
└─ Użycie: ERP, CRM, bankowe, większość OLTP

NoSQL (Non-relational):
├─ Document (MongoDB): JSON-like dokumenty, flexible schema
├─ Key-Value (Redis): { "key" → "value" }, ultra-fast cache
├─ Wide Column (Cassandra): Big Data, Time Series
├─ Graph (Neo4j): Nodes + edges, sieci społeczne
├─ ✓ Horizontal skalowanie (wiele maszyn)
├─ ✓ Flexible schema (bez ALTER TABLE)
├─ ✓ Duże ilości danych, real-time
├─ ✗ Brak transakcji (eventual consistency)
├─ ✗ Brak JOINów (denormalizacja)
└─ Użycie: Social networks, Real-time analytics, Big Data, cache

NewSQL (Hybrid):
├─ Przykład: CockroachDB, Google Spanner
├─ Model: SQL + Horizontal scaling
├─ ✓ ACID + distributed
├─ ✗ Nowsze, nie zawsze stable
└─ Użycie: Global apps, correctness-critical

Twój projekt: SQL (TaskManager - structured, ACID ważny)
Cache layer: Redis (sesje, frequently accessed)
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
| Window Functions | ROW_NUMBER, RANK, LEAD, LAG - operuje bez GROUP BY |
| Keys | PK = unique, FK = referential integrity (musi być UNIQUE) |
| Relationships | 1:1 (FK + UNIQUE), 1:N (FK), N:N (junction table) |
| ACID | Atomicity, Consistency, Isolation, Durability |
| Locking | Shared (S), Exclusive (X), Intent locks - zapobiega conflicts |
| Isolation Levels | READ UNCOMMITTED → READ COMMITTED → REPEATABLE READ → SERIALIZABLE |
| Optimistic Locking | Trust, check at end (ROWVERSION, DbUpdateConcurrencyException) |
| Pessimistic Locking | Lock early (WITH (XLOCK)), ryzyko deadlock |
| Database Types | SQL (ACID, relational), NoSQL (scalable, flexible), NewSQL (hybrid) |
| Normalization | 3NF = good OLTP, eliminate redundancy |
| Indexes | B-tree, fast reads, slow writes, need maintenance |
| Statistics | Query planner używa do wyboru SEEK vs SCAN |
| Execution Plan | Teoretyczne - Index Seek vs Scan |
| Query Profiler | Praktyczne - CPU time, I/O, memory |
| N+1 Problem | Include() to load related in 1 query |
| Change Tracking | EF knows what changed, generates UPDATE |
| Projections | Select() only needed fields, faster |
| Performance | Profile before optimizing, measure impact |

