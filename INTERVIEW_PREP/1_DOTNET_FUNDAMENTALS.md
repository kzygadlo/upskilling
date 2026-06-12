# 1️⃣ .NET C# FUNDAMENTALS - Q&A

---

## Q1.1: Czym są value types vs reference types? Jak to wpływa na pamięć?

**SHORT:**
Value types (int, bool, struct) → Stack (copied). Reference types (class, string, array) → Heap (referenced). Different memory allocation = different performance.

**FULL ANSWER:**

**CONCEPT**: Memory allocation strategy in .NET

### Value Types
```csharp
// VALUE TYPE (stack allocation)
int x = 5;
int y = x;      // Copy the VALUE
y = 10;
Console.WriteLine(x);  // Still 5 ✓ (unchanged)

struct Point { public int X; }
var p1 = new Point { X = 1 };
var p2 = p1;    // Copy entire struct
p2.X = 2;
Console.WriteLine(p1.X);  // Still 1 ✓
```

**Where:** Stack (fast, automatic cleanup)
**Behavior:** Copy by value (modifications don't affect original)
**Types:** int, bool, float, decimal, struct, enum
**Size:** Fixed, known at compile time
**Performance:** Fast (stack is cheap)
**Example:** `int x = 5; int y = x; // y = 5, x = 5`

### Reference Types
```csharp
// REFERENCE TYPE (heap allocation)
class Project { public int Id; }
var p1 = new Project { Id = 1 };
var p2 = p1;    // Copy the REFERENCE (not the object)
p2.Id = 2;
Console.WriteLine(p1.Id);  // Now 2! ✓ (both point to same object)

string s1 = "Hello";
string s2 = s1;  // s1 and s2 point to same string in memory
```

**Where:** Heap (with GC cleanup)
**Behavior:** Copy by reference (modifications affect all references)
**Types:** class, string, array, delegate, interface
**Size:** Variable, determined at runtime
**Performance:** Slower (GC overhead)
**Example:** `var obj1 = new Class(); var obj2 = obj1; // obj2.Prop = 10 → obj1.Prop also 10`

### Memory Model
```
STACK (Thread-local, auto-cleanup)
┌─────────────────┐
│ int x = 5       │
│ bool b = true   │
│ Point p = ...   │ (entire struct here)
│ ref to proj = ─┐│
└─────────────────┘
                 │
                 ↓ HEAP (Managed by GC)
                 ┌──────────────────┐
                 │ Project object   │
                 │   Id = 2         │
                 │   Name = "..."   │
                 └──────────────────┘
```

### Performance Implications
| Aspect | Value Type | Reference Type |
|--------|-----------|----------------|
| Allocation | Stack (fast) | Heap (GC) |
| Cleanup | Auto when scope exits | Garbage collected |
| Copying | Full copy (memory) | Pointer copy (fast) |
| Default | 0/false | null |
| Boxing | Possible (costly) | N/A |

### Common Pitfalls
```csharp
// ❌ Boxing (value → object wrapper) = SLOW
object boxed = 5;  // Allocates on heap
int unboxed = (int)boxed;  // Unboxes

// ✅ Avoid boxing in collections
List<int> numbers = new();  // Generic = no boxing
```

### Key Takeaway
> *Stack is fast but limited (size, lifetime). Heap is flexible but GC overhead. Value types for small, short-lived data. Reference types for complex, long-lived objects.*

---

## Q1.2: Generics - czym są, po co się ich używa?

**SHORT:**
**TECHNIQUE**: Type parameterization. Write code once, use with any type. `List<T>` instead of ArrayList (type-safe, no boxing).

**FULL ANSWER:**

### Without Generics (Old C#)
```csharp
// ArrayList = object-based (type-unsafe)
ArrayList list = new();
list.Add(5);
list.Add("string");  // ✓ Compiler allows it
var item = list[0];  // Returns object
int number = (int)item;  // Manual casting

// Problems:
// - Type-unsafe (can add wrong types)
// - Boxing overhead
// - Casting required
```

### With Generics (Modern C#)
```csharp
// List<T> = type-parameterized
List<int> numbers = new();
numbers.Add(5);
numbers.Add("string");  // ❌ Compiler error (safe!)
int number = numbers[0];  // No casting needed

// Benefits:
// - Type-safe (compile-time checking)
// - No boxing
// - No casting
// - IntelliSense works
```

### Generic Method Example
```csharp
// Generic method - works with ANY type
public T GetFirstOrDefault<T>(List<T> list)
{
    return list.Count > 0 ? list[0] : default(T);
}

// Usage:
var firstInt = GetFirstOrDefault(new List<int> { 1, 2, 3 });  // 1
var firstStr = GetFirstOrDefault(new List<string> { "a", "b" });  // "a"
```

### Generic Class Example
```csharp
// Repository pattern with generics
public class Repository<T> where T : class
{
    private DbContext _db;

    public T GetById(int id) 
    {
        return _db.Set<T>().FirstOrDefault(e => e.Id == id);
    }
    
    public void Add(T entity)
    {
        _db.Set<T>().Add(entity);
    }
}

// Usage:
var userRepo = new Repository<User>();
var user = userRepo.GetById(1);

var projectRepo = new Repository<Project>();
var project = projectRepo.GetById(1);
```

### Generic Constraints
```csharp
// Constrain T to specific types
public T Create<T>() where T : class, new()
{
    return new T();  // T must be class with parameterless constructor
}

// Constraints:
// - where T : class          (reference type)
// - where T : struct         (value type)
// - where T : IInterface     (implements interface)
// - where T : BaseClass      (inherits base)
// - where T : new()          (has parameterless constructor)
// - Combinable: where T : class, IRepository, new()
```

### Why Generics Matter
1. **Code reuse** - write once, use for many types
2. **Type safety** - compiler catches errors
3. **Performance** - no boxing/unboxing
4. **Readability** - `List<int>` clearer than ArrayList

### Key Takeaway
> *Generics are fundamental to modern .NET. Any collection or utility should be generic. No ArrayLists or casting.*

---

## Q1.3: Nullable types - czym są i dlaczego są przydatne?

**SHORT:**
**CONCEPT**: Allow value types to be null. `int?` means "int or null". **Nullable reference types** (C# 8+) warn when strings/classes might be null.

**FULL ANSWER:**

### Nullable Value Types
```csharp
// Regular int cannot be null
int age = 0;  // Valid, but is 0 age or no age?

// Nullable int CAN be null
int? age = null;  // ✓ Clear intent: age is not known
int? age = 25;

// Usage
if (age.HasValue)
{
    Console.WriteLine($"Age: {age.Value}");
}
else
{
    Console.WriteLine("Age unknown");
}

// Shorthand
int nonNullAge = age ?? 0;  // null-coalescing operator
```

### Nullable Reference Types (C# 8+)
```csharp
#nullable enable  // Enable nullable reference types

// Without NRT:
string name = null;  // ✓ Compiles (no warning)

// With NRT:
string name = null;  // ❌ CS8625 Warning: non-nullable reference type
string? name = null;  // ✓ OK: nullable reference type

public class User
{
    public string Name { get; set; }  // ❌ Warning if not initialized
    public string? Email { get; set; }  // ✓ OK: can be null
}

// Benefits:
// - Compile-time null checking
// - Prevents NullReferenceException
// - Clear API contract (is this nullable or not?)
```

### Real-World Example: Database Field
```csharp
public class Project
{
    public int Id { get; set; }           // Never null
    public string Name { get; set; }      // Never null (required)
    public string? Description { get; set; }  // Can be null (optional)
}

// API Design:
public ProjectDto GetProject(int id)
{
    // Consumer knows:
    // - id: will always have value
    // - Description: might be null (prepare for it)
}
```

### When NULL Means What
```csharp
// Database:
int? deleted_at = null;  // Row not deleted
int? deleted_at = 12345; // Deleted at timestamp

// API Response:
string? errorMessage = null;  // No error
string? errorMessage = "Invalid email";  // Error occurred

// Business logic:
int? orderId = null;  // Order not yet created
int? orderId = 1001;  // Order exists
```

### Key Takeaway
> *Nullable types make intent clear: "This value CAN be null" vs "This value is null because it's not set". Critical for APIs and domain models.*

---

## Q1.4: Czym są lambda expressions? Jak działają?

**SHORT:**
**LANGUAGE FEATURE**: Anonymous functions. `x => x.Age > 18` instead of full method. Used with LINQ, delegates, callbacks.

**FULL ANSWER:**

### Lambda Syntax
```csharp
// Traditional method
public bool IsAdult(int age)
{
    return age > 18;
}

// Lambda expression (equivalent)
x => x > 18

// With multiple statements
x => 
{
    var senior = x > 65;
    return senior;
};

// Syntax: (parameters) => expression or { statements }
```

### Common Uses
```csharp
// LINQ filtering
var adults = users.Where(u => u.Age > 18).ToList();
var names = users.Select(u => u.Name).ToList();
var sorted = users.OrderBy(u => u.LastName).ToList();

// Dictionary key selector
var usersByEmail = users.ToDictionary(u => u.Email);

// Event subscription
button.Click += (sender, e) => Console.WriteLine("Clicked!");

// Async callbacks
async Task FetchData()
{
    var data = await client.GetAsync(url);
}

// Predicate
Func<int, bool> isEven = x => x % 2 == 0;
bool result = isEven(4);  // true
```

### Lambda + Delegates
```csharp
// Delegate = type-safe function pointer
public delegate int Calculator(int a, int b);

// Traditional method
public int Add(int a, int b) => a + b;
Calculator calc = Add;

// Lambda way
Func<int, int, int> add = (a, b) => a + b;
Func<int, int, int> multiply = (a, b) => a * b;

// Usage
int result = add(5, 3);  // 8
```

### Closure Concept
```csharp
int multiplier = 2;

// Lambda "captures" multiplier variable
Func<int, int> double_it = x => x * multiplier;

var result = double_it(5);  // 10
multiplier = 3;
result = double_it(5);  // Now 15 (captured variable changed!)
```

### Expression vs Statement Lambdas
```csharp
// Expression lambda (returns value)
x => x * 2

// Statement lambda (contains logic)
x =>
{
    var doubled = x * 2;
    return doubled;
};
```

### Key Takeaway
> *Lambdas are concise function definitions. Essential for LINQ, callbacks, and functional programming patterns. Preferred over anonymous methods in modern C#.*

---

## Q1.5: IEnumerable vs IQueryable - czym się różnią?

**SHORT:**
**CONCEPT**: IEnumerable = in-memory LINQ (C#). IQueryable = remote query (SQL translation). Where you use them matters for performance.

**FULL ANSWER:**

### Side-by-side Comparison
```csharp
// IEnumerable (Local)
List<Project> projects = GetProjectsFromMemory();
var active = projects
    .Where(p => p.Active)
    .OrderBy(p => p.Name)
    .ToList();  // LINQ to Objects - executes in C#

// IQueryable (Remote)
using var db = new TaskManagerDbContext();
var active = db.Projects
    .Where(p => p.Active)
    .OrderBy(p => p.Name)
    .ToListAsync();  // LINQ to Entities - executes as SQL
```

### Execution Location
```csharp
IEnumerable:
  Code → C# objects in RAM → Filter/sort in C# → Result

IQueryable:
  Code → SQL Query → Database → Result set
```

### Performance Difference
```csharp
// ❌ SLOW: Load all, then filter
var allProjects = db.Projects.AsEnumerable();  // Loads 10,000 projects
var active = allProjects.Where(p => p.Active).ToList();  // Filters in C#
// Problem: 10k objects loaded, then filtered in memory

// ✅ FAST: Filter at source
var active = db.Projects
    .Where(p => p.Active)
    .ToListAsync();  // SQL WHERE clause executes at DB
// Problem: Only active ones loaded
```

### Method Translation
```csharp
IEnumerable<T> methods:
  - Where, Select, OrderBy, GroupBy
  - Works with any in-memory collection
  - Uses LINQ to Objects

IQueryable<T> methods:
  - Where, Select, OrderBy, GroupBy
  - TRANSLATES to SQL before execution
  - Uses LINQ provider (EF Core, EF6, etc.)
```

### Key Difference: Complex Predicates
```csharp
// IEnumerable: Custom logic possible
Func<Project, bool> isValid = p => 
{
    var hasManager = p.Manager != null;
    var isRecent = (DateTime.Now - p.CreatedAt).Days < 30;
    return hasManager && isRecent;
};
var results = projects.AsEnumerable().Where(isValid).ToList();  // Works

// IQueryable: Only translatable operations
var results = db.Projects
    .Where(p => p.Manager != null && 
                 (DateTime.Now - p.CreatedAt).Days < 30)
    .ToListAsync();  // Works (translates to SQL)

// IQueryable: Custom method won't translate
Func<Project, bool> isValid = p => IsValidProject(p);  // Custom C# method
var results = db.Projects.Where(isValid).ToListAsync();  // ❌ Error!
```

### When to Use What
| Scenario | Use |
|----------|-----|
| Filter 100 items in memory | IEnumerable |
| Filter 1M rows in database | IQueryable (filter at DB) |
| Complex C# logic filtering | IEnumerable |
| Simple SQL-translatable filter | IQueryable |
| Performance critical | IQueryable (let DB do work) |

### Key Takeaway
> *IQueryable for database queries (translate to SQL). IEnumerable for in-memory collections (execute in C#). Choose based on data source and filter complexity.*

---

## Q1.6: Stack vs Heap - czym są i co jest gdzie?

**SHORT:**
**CONCEPT**: Stack = local variables, fast, auto-cleanup. Heap = objects, GC manages. Stack issues = stack overflow. Heap issues = memory leak.

**FULL ANSWER:**

### Memory Layout
```
┌─────────────────────────────────────┐
│ STACK (Thread-local)                │
├─────────────────────────────────────┤
│ Local variables:                    │
│  int x = 5                          │
│  bool active = true                 │
│  string name;  → reference only    │
│  Point p = ... (full struct)        │
├─────────────────────────────────────┤
│ Return address                      │
│ Method parameters                   │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│ HEAP (Process-wide, GC managed)     │
├─────────────────────────────────────┤
│ Objects:                            │
│  string "John" → referenced         │
│  Project { Id=1, Name="..." }       │
│  List<int> [1, 2, 3]                │
│  Complex objects                    │
│                                     │
│ [GC periodically cleans up]         │
└─────────────────────────────────────┘
```

### Stack Characteristics
```csharp
public void StackExample()
{
    int x = 5;              // Stack
    bool active = true;     // Stack
    var point = new Point(); // struct: Stack
    
    // All cleaned up automatically when method returns
}
```

**Properties:**
- **LIFO** (Last In First Out)
- **Thread-local** (each thread has own stack)
- **Auto cleanup** (when variable scope exits)
- **Size limited** (typically 1MB per thread)
- **Fast** (just move pointer)

### Heap Characteristics
```csharp
public void HeapExample()
{
    var project = new Project();  // Reference on stack, object on heap
    var name = "John";            // Reference on stack, string on heap
    
    // Only REFERENCE is cleaned up from stack
    // Object stays on heap until GC collects it
}
```

**Properties:**
- **No order** (fragmented)
- **Process-wide** (shared)
- **GC managed** (automatic cleanup when unreachable)
- **Size flexible** (limited by RAM)
- **Slower** (GC overhead)

### Stack Overflow Example
```csharp
// ❌ Infinite recursion = Stack Overflow
public int Calculate(int n)
{
    return Calculate(n + 1);  // Each call adds to stack
}                              // Eventually: StackOverflowException
```

### Memory Leak Example
```csharp
// ❌ Leak: object referenced but never cleaned
List<WeakReference> cached = new();
while (true)
{
    var large = new byte[1_000_000];  // 1MB on heap
    cached.Add(new WeakReference(large));
}
// Objects accumulate on heap, GC can't clean them up

// ✅ Fix: Clear references
cached.Clear();  // Objects eligible for GC
```

### Key Differences
| Property | Stack | Heap |
|----------|-------|------|
| Access speed | O(1) - instant | O(1) - instant |
| Size | Limited (~1MB) | Limited by RAM |
| Cleanup | Automatic | GC |
| Order | LIFO | No order |
| Thread | Per-thread | Shared |

### Key Takeaway
> *Stack for small, short-lived locals. Heap for objects. Understand difference → prevent stack overflow and memory leaks.*

---

## Q1.7: Garbage Collection - czym jest i jak działa?

**SHORT:**
**MECHANISM**: Automatic memory management. GC marks unreachable objects, cleans them up. Three generations optimize for short-lived objects.

**FULL ANSWER:**

### What is GC?
GC = automatic memory cleanup. When no references point to object → GC can collect it.

```csharp
public void Example()
{
    var project = new Project();  // Created on heap
    // ... use project ...
    
    // When project variable goes out of scope
    // → No more references
    // → GC will eventually collect it
}
```

### Three Generations

**Gen 0: Young Objects**
```csharp
var temp = new byte[1024];  // Gen 0
var data = Process(temp);   // Likely dies soon
// Gen 0 collection is frequent, fast
```
- Most objects die young
- Collected frequently (every few allocations)
- Fast collection (small area)

**Gen 1: Survivors**
```csharp
var cache = new Dictionary<int, Data>();
// Lives longer than Gen 0
// Gets promoted to Gen 1
```
- Objects that survived Gen 0 collection
- Collected less frequently
- Medium cost

**Gen 2: Long-lived Objects**
```csharp
public static readonly Logger log = new();  // Lives entire app life
// Promoted to Gen 2
// Collected rarely (Full GC is expensive)
```
- Singletons, static fields, long-lived caches
- Full GC is expensive (Stop-The-World pause)
- Should minimize Gen 2 objects

### How GC Works
```
1. Mark Phase
   └─ Start from "roots" (local vars, static fields)
   └─ Follow references
   └─ Mark reachable objects

2. Sweep Phase
   └─ Objects not marked = unreachable
   └─ Reclaim their memory

3. Compact Phase (Gen 2 only)
   └─ Move live objects together
   └─ Eliminate fragmentation
```

### Gen 0 Collection Example
```csharp
var list = new List<int>();
for (int i = 0; i < 1_000_000; i++)
{
    var temp = new int[1000];  // Gen 0: allocated
    list.AddRange(temp);       // Used
    // End of loop: temp eligible for Gen 0 collection
}
// After loop: Gen 0 collection runs
// temp objects cleaned up (fast!)
```

### GC Pressure Anti-Patterns
```csharp
// ❌ BAD: Massive allocations
var lists = new List<List<int>>();
for (int i = 0; i < 1_000_000; i++)
{
    lists.Add(new List<int> { Random.Shared.Next() });
}
// 1M allocations = high GC pressure

// ✅ GOOD: Reuse allocations
var list = new List<int>(capacity: 1_000_000);
for (int i = 0; i < 1_000_000; i++)
{
    list.Add(Random.Shared.Next());
}
// Single allocation, reused
```

### Forcing GC (Don't Do This)
```csharp
// ❌ Avoid manual GC calls
GC.Collect();  // Expensive, pauses application
GC.WaitForPendingFinalizers();

// Let GC run naturally (adaptive tuning)
```

### LOH: Large Object Heap
```csharp
// Objects > 85KB go to LOH
var largeArray = new byte[100_000];  // LOH (Gen 2 level)
var largeArray = new byte[10];       // Regular heap

// LOH is not compacted by default
// Risk: fragmentation with many large objects
```

### Key Takeaway
> *GC manages memory automatically. Gen 0 collected often (fast). Gen 2 collected rarely (expensive). Design code to minimize Gen 2 pressure: reuse allocations, avoid large objects.*

---

## Q1.8: Jaka jest rola CLR w .NET?

**SHORT:**
**COMPONENT**: Common Language Runtime. Engine executing .NET code. Handles compilation (IL → machine code), GC, type safety, threading.

**FULL ANSWER:**

### CLR Architecture
```
C# Code
  ↓ (Compiler)
IL (Intermediate Language)
  ↓ (CLR JIT Compiler)
Machine Code
  ↓ (CLR)
Execution
```

### What CLR Does

1. **JIT Compilation**
```csharp
// Your C# code
public int Calculate(int x)
{
    return x * 2;
}

// Compiled to IL (Intermediate Language)
IL_0000: ldarg.1      // Load argument x
IL_0001: ldc.i4.2     // Load constant 2
IL_0002: mul           // Multiply
IL_0003: ret           // Return result

// CLR JIT compiles IL → native machine code
// First call: compile (slow)
// Subsequent calls: execute compiled version (fast)
```

2. **Memory Management**
   - GC (Garbage Collection)
   - Type safety
   - References vs values

3. **Type Safety**
```csharp
int x = 5;
x = "string";  // ❌ Compiler + CLR error
// CLR enforces type system at runtime
```

4. **Managed Execution**
   - Thread pooling
   - Synchronization
   - Exception handling
   - Security checks

### JIT Compilation Modes

**Tiered JIT (Modern)**
```
Startup:
  1. Interpreter or minimal JIT (fast startup)
  2. Profile code
  3. When hot: Recompile with optimizations

Benefit: Fast startup + optimized performance
```

**AOT: Ahead of Time Compilation**
```csharp
// Compile to machine code before deployment
// Benefits: instant start, small size, no JIT overhead
// Trade-off: less optimization (no runtime profiling)
```

### CLR vs JVM
| Feature | CLR | JVM |
|---------|-----|-----|
| Language | C#, VB.NET, F# | Java |
| Platform | Windows, Linux, macOS | Windows, Linux, macOS |
| JIT | Tiered | Standard |
| GC | Generational | Generational |
| AOT | Yes | Yes (GraalVM) |

### Key Takeaway
> *CLR is the engine. C# is language, CLR is runtime. Understanding JIT helps optimize performance. Tiered JIT = best of both (fast startup + optimized execution).*

---

## Q1.9: IDisposable - kiedy używać, jak używać?

**SHORT:**
**PATTERN**: For managed cleanup. Use when class holds unmanaged resources (files, connections, streams). Implement Dispose() for cleanup.

**FULL ANSWER:**

### When to Implement IDisposable
```csharp
// ✅ Needs IDisposable
public class DatabaseConnection : IDisposable
{
    private SqlConnection _connection;
    
    public void Dispose()
    {
        _connection?.Close();
        _connection?.Dispose();
    }
}

// ✅ Needs IDisposable
public class FileWriter : IDisposable
{
    private FileStream _stream;
    
    public void Dispose()
    {
        _stream?.Flush();
        _stream?.Close();
    }
}

// ❌ Doesn't need IDisposable
public class User
{
    public string Name { get; set; }  // No resources to cleanup
}
```

### Using Statements (Recommended)
```csharp
// ✅ GOOD: using statement (auto-disposes)
using var connection = new DatabaseConnection();
connection.Open();
// At end of block: Dispose() called automatically

// Legacy syntax (C# 7 and earlier)
using (var connection = new DatabaseConnection())
{
    connection.Open();
}  // Dispose() called here
```

### Manual Dispose Pattern
```csharp
public class Resource : IDisposable
{
    private bool disposed = false;
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);  // Tell GC: no finalizer needed
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                // Cleanup managed resources
                _managedResource?.Dispose();
            }
            
            // Cleanup unmanaged resources
            // _unmanagedHandle.Release();
            
            disposed = true;
        }
    }
    
    ~Resource()
    {
        Dispose(false);  // Finalizer as safety net
    }
}
```

### IAsyncDisposable (Modern)
```csharp
public class AsyncResource : IAsyncDisposable
{
    public async ValueTask DisposeAsync()
    {
        await _stream.FlushAsync();
        await _stream.DisposeAsync();
    }
}

// Usage
await using var resource = new AsyncResource();
// At end: DisposeAsync() called automatically
```

### Real Example: DbContext
```csharp
// EF Core DbContext implements IDisposable
public class MyDbContext : DbContext
{
    // Manages database connection
}

// Usage
using var db = new MyDbContext();
var projects = db.Projects.ToList();
// At end of using: database connection closed
```

### Antipattern: Not Disposing
```csharp
// ❌ BAD: Resource leak
public void ProcessFile()
{
    var stream = new FileStream("data.txt", FileMode.Open);
    var data = stream.ReadByte();
    // ❌ stream never disposed, file locked
}

// ✅ GOOD
public void ProcessFile()
{
    using var stream = new FileStream("data.txt", FileMode.Open);
    var data = stream.ReadByte();
    // ✅ File automatically closed
}
```

### Key Takeaway
> *Use IDisposable + using for resources (DB, files, streams). Dispose() cleans up. using statement = guaranteed cleanup (even on exception).*

---

## Q1.10: Dispose() vs finalizer - czym się różnią?

**SHORT:**
**DIFFERENCE**: Dispose() = explicit cleanup (fast). Finalizer (~Destructor) = safety net (slow, unpredictable timing). Use Dispose(), finalizer only as fallback.

**FULL ANSWER:**

### Side-by-Side
```csharp
public class Resource : IDisposable
{
    // DISPOSE: Explicit cleanup
    public void Dispose()
    {
        // Called immediately when using block ends
        // Can be called multiple times
        // Cleanup happens NOW
        _connection?.Close();
    }
    
    // FINALIZER: Safety net
    ~Resource()
    {
        // Called by GC when object collected
        // Unpredictable timing (might be minutes later!)
        // Only called if Dispose() wasn't called
        _connection?.Close();  // Late cleanup
    }
}
```

### Timing
```
Dispose():
  using var res = new Resource();
  // ... use ...
  } ← HERE: Dispose() called immediately

Finalizer:
  var res = new Resource();
  res = null;  // No longer referenced
  // ... could be seconds/minutes later ...
  // GC runs: ~Resource() called
```

### Performance
```csharp
// Dispose: Fast
public void Dispose()
{
    _stream.Close();  // Immediate
}

// Finalizer: Slow
~Resource()
{
    _stream.Close();  // When GC decides to run (might pause app)
}

// Impact:
// - Dispose: negligible overhead
// - Finalizer: GC has to track object, run finalizer, causes pauses
```

### Resource Leak Example
```csharp
// ❌ Relying on finalizer
public void ReadFile()
{
    var stream = new FileStream("data.txt", FileMode.Open);
    var data = stream.ReadByte();
    // Dispose not called
    // File stays locked until GC runs finalizer (might be minutes!)
}

// ✅ Using Dispose
public void ReadFile()
{
    using var stream = new FileStream("data.txt", FileMode.Open);
    var data = stream.ReadByte();
    // File closed immediately
}
```

### Pattern: Dispose + Finalizer
```csharp
public class Resource : IDisposable
{
    private bool disposed = false;
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);  // Tell GC: skip finalizer
    }
    
    protected void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
                _managedResource?.Dispose();  // Cleanup
            
            disposed = true;
        }
    }
    
    ~Resource()
    {
        Dispose(false);  // Fallback if Dispose() not called
    }
}
```

### Key Differences
| Aspect | Dispose | Finalizer |
|--------|---------|-----------|
| Timing | Immediate | Unpredictable (GC) |
| Cost | Low | High (GC tracking) |
| Control | You decide | GC decides |
| Reliability | Guaranteed (using) | Uncertain |
| Best for | Primary cleanup | Safety net only |

### Key Takeaway
> *Use Dispose() + using for reliable cleanup. Finalizer is safety net (slow, unpredictable). Good design: always call Dispose explicitly, finalizer as fallback.*

---

## Q1.11: async/await - czym to jest i jak działa?

**SHORT:**
**PATTERN**: Enable non-blocking code. Thread free'd during await, resumes later. Critical for scalability (1 thread → 1000+ concurrent operations).

**FULL ANSWER:**

### What async/await Does
```csharp
// Naive blocking (BAD)
public int FetchData()
{
    var response = _client.GetStringAsync(url).Result;  // BLOCKS thread
    return response.Length;
}
// Thread waits for network (wasteful)

// async/await (GOOD)
public async Task<int> FetchDataAsync()
{
    var response = await _client.GetStringAsync(url);  // DOESN'T block
    return response.Length;
}
// Thread freed during network call
```

### How It Works Internally
```
Compiler converts async method into STATE MACHINE:

public async Task<string> FetchAsync()
{
    var data = await http.GetAsync(url);     // ← Await point 1
    var processed = await Process(data);      // ← Await point 2
    return processed;
}

Becomes (simplified):

public Task<string> FetchAsync()
{
    var machine = new FetchAsyncStateMachine();
    machine.ExecuteAsync();
    return machine.Task;
}

class FetchAsyncStateMachine
{
    private int state = 0;
    
    public async Task ExecuteAsync()
    {
        switch(state)
        {
            case 0:  // Before first await
                var data = await http.GetAsync(url);
                state = 1;
                break;
            case 1:  // Before second await
                var processed = await Process(data);
                state = 2;
                break;
            case 2:  // After all awaits
                return processed;
        }
    }
}
```

### Key Concepts

**Await Releases Thread**
```csharp
public async Task ProcessAsync()
{
    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}");
    
    var data = await FetchFromNetworkAsync();  // ← Thread released here
    
    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}");
    // Might be different thread (from ThreadPool)
}
```

**Task Represents Future Result**
```csharp
// Task = "This operation will complete in the future"
public async Task<int> GetCountAsync()
{
    var result = await db.Projects.CountAsync();
    return result;
}

// Task<T> = "Future operation will return T"
Task<int> countTask = GetCountAsync();
// countTask is not a result yet, it's a promise
int count = await countTask;  // Now we have the result
```

### Real Example: Scalability
```csharp
// Blocking (OLD)
public void HandleRequest(HttpContext context)
{
    var user = db.Users.FirstOrDefault(u => u.Id == userId).Result;  // BLOCKS
    var projects = db.Projects.Where(p => p.UserId == userId).ToList();  // BLOCKS
    context.Response.WriteAsJsonAsync(new { user, projects });
}
// 1000 requests = 1000 threads = expensive

// Async (MODERN)
public async Task HandleRequestAsync(HttpContext context)
{
    var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);  // No block
    var projects = await db.Projects.Where(p => p.UserId == userId).ToListAsync();  // No block
    await context.Response.WriteAsJsonAsync(new { user, projects });
}
// 1000 requests = maybe 10 threads = cheap
```

### ConfigureAwait(false)
```csharp
// Library code (not UI)
public async Task<Data> FetchAsync()
{
    // Don't need UI thread context
    var response = await http.GetAsync(url).ConfigureAwait(false);
    var data = await response.Content.ReadAsAsync<Data>().ConfigureAwait(false);
    return data;
}

// UI code (WPF, WinForms)
public async void ButtonClick()
{
    // Need UI thread context (to update controls)
    var data = await FetchAsync();  // ConfigureAwait(true) implicit
    Label.Text = data.Name;  // Update UI (must be UI thread)
}
```

### Common Mistakes
```csharp
// ❌ async void (only for event handlers)
public async void ProcessAsync()  // DON'T
{
    await Task.Delay(1000);
}

// ✅ async Task
public async Task ProcessAsync()
{
    await Task.Delay(1000);
}

// ❌ Blocking async (defeats purpose)
var result = FetchAsync().Result;  // Blocks anyway!

// ✅ Awaiting
var result = await FetchAsync();
```

### Key Takeaway
> *async/await = non-blocking code. Thread freed during I/O. Scales to thousands of concurrent operations. Use for I/O-bound (network, database), not CPU-bound.*

---

## Q1.12: async Task vs async void - czym się różnią?

**SHORT:**
**PATTERN**: async Task = preferred (trackable, exceptions handled). async void = only for event handlers. async void = fire-and-forget, hard to track.

**FULL ANSWER:**

### async Task (GOOD)
```csharp
public async Task FetchDataAsync()
{
    var response = await _client.GetAsync(url);
    return response;
}

// Usage
Task task = FetchDataAsync();
await task;  // Can wait for completion
bool completed = task.IsCompleted;  // Can check status
Exception? ex = task.Exception;  // Can catch exceptions
```

**Benefits:**
- Trackable (caller can await)
- Exceptions propagated
- Can check completion status
- Composable (can chain tasks)

### async void (BAD)
```csharp
public async void FetchDataAsync()  // ❌ Avoid
{
    var response = await _client.GetAsync(url);
    return;
}

// Usage - PROBLEM: Can't wait
FetchDataAsync();  // Fire and forget
// Caller has no way to know if completed
// Exceptions are lost
```

**Problems:**
- Fire-and-forget (untrackable)
- Exceptions not propagated (app crash)
- Can't check status
- Hard to test
- Can't compose

### Exception Handling
```csharp
// async Task (GOOD)
public async Task SaveAsync()
{
    await db.SaveChangesAsync();  // If error: thrown to caller
}

// Caller can catch
try
{
    await SaveAsync();
}
catch (DbException ex)
{
    // Exception caught, app survives
}

// async void (BAD)
public async void SaveAsync()  // ❌
{
    await db.SaveChangesAsync();  // If error: ???
}

// Caller can't catch
try
{
    SaveAsync();  // Returns immediately
    // Exception happens later, in different context
}
catch (DbException ex)
{
    // Never caught! App crashes
}
```

### Event Handlers Exception
```csharp
// Event handler = async void OK (only place it's OK)
button.Click += async (sender, e) =>
{
    await ProcessAsync();  // Async void here is acceptable
};

// Why OK for events:
// - Events don't return values
// - Caller doesn't need to wait
// - Event system handles exceptions
```

### Comparison Table
| Aspect | async Task | async void |
|--------|-----------|-----------|
| Return type | Task | void |
| Awaitable | Yes | No |
| Exception handling | Propagated | Lost |
| Status check | Yes | No |
| Testable | Yes | No |
| Use case | Normal code | Event handlers only |

### Pattern: async Task with Result
```csharp
public async Task<int> CountProjectsAsync()
{
    return await db.Projects.CountAsync();
}

// Usage
int count = await CountProjectsAsync();

// Task<T> provides result
Task<int> task = CountProjectsAsync();
int result = await task;
```

### Key Takeaway
> *async Task = standard. async void = event handlers only. Never use async void for regular methods - exceptions crash app, impossible to track.*

---

## Q1.13: Threads vs Tasks vs ThreadPool - czym się różnią?

**SHORT:**
**CONCEPT**: Threads = OS threads (expensive). ThreadPool = thread cache (reuse). Tasks = abstraction (can use threads or async). Tasks = preferred (scalable).

**FULL ANSWER:**

### Threads (Low-level)
```csharp
// Create thread explicitly
var thread = new Thread(() =>
{
    Console.WriteLine("Working...");
    Thread.Sleep(1000);
});
thread.Start();
thread.Join();  // Wait for completion
// ❌ Expensive: OS creates actual thread (~1MB memory per thread)
```

**Characteristics:**
- OS-level threads (real threads)
- ~1MB memory per thread
- Slow to create (~1ms)
- No pooling (create new each time)
- Good for: CPU-bound dedicated tasks

### ThreadPool (Reusable Threads)
```csharp
// Queue work to thread pool
ThreadPool.QueueUserWorkItem(_ =>
{
    Console.WriteLine("Working...");
});
// ✓ Efficient: reuses threads from pool
```

**Characteristics:**
- Pool of reusable threads
- Adaptive sizing (creates as needed)
- Faster than new threads
- Good for: I/O-bound work

### Tasks (High-level Abstraction)
```csharp
// Tasks use thread pool under the hood
Task task = Task.Run(() =>
{
    Console.WriteLine("Working...");
});
await task;
// ✓ Highest abstraction, most flexible
```

**Characteristics:**
- Abstraction over threads
- Use ThreadPool by default
- Composable (chain, combine)
- Better for: async/await patterns

### Comparison
| Aspect | Thread | ThreadPool | Task |
|--------|--------|-----------|------|
| Cost | High | Medium | Low |
| Memory | ~1MB | Shared | Shared |
| Creation | Slow | Fast | Fast |
| Pooling | No | Yes | Yes |
| Abstraction | Low | Medium | High |
| Async-friendly | No | No | Yes |

### Real Example: Scalability
```csharp
// ❌ Threads (scalability problem)
for (int i = 0; i < 1000; i++)
{
    var thread = new Thread(() =>
    {
        var data = FetchFromNetwork();  // I/O wait
    });
    thread.Start();
}
// 1000 threads × 1MB = 1GB memory! Crash.

// ✓ ThreadPool (better)
for (int i = 0; i < 1000; i++)
{
    ThreadPool.QueueUserWorkItem(_ =>
    {
        var data = FetchFromNetwork();  // I/O wait
    });
}
// Reuses ~10 threads (default processor count)

// ✓✓ Tasks + async (best)
var tasks = new List<Task>();
for (int i = 0; i < 1000; i++)
{
    tasks.Add(FetchFromNetworkAsync());
}
await Task.WhenAll(tasks);
// No threads blocked, truly scalable
```

### Task Composition
```csharp
// Tasks = composable
var task1 = FetchDataAsync();
var task2 = ProcessDataAsync();
var task3 = SaveDataAsync();

// Wait for all
await Task.WhenAll(task1, task2, task3);

// Wait for first
var first = await Task.WhenAny(task1, task2, task3);

// Chain
var result = await task1
    .ContinueWith(t => task2)
    .ContinueWith(t => task3);
```

### Key Takeaway
> *Use Tasks (not Threads). Tasks = scalable, composable, async-friendly. ThreadPool is used internally by Tasks. Threads only for long-running CPU work.*

---

## Q1.14: Race conditions - czym są i jak ich unikać?

**SHORT:**
**PROBLEM**: Multiple threads access/modify shared data simultaneously → unpredictable results. **SOLUTION**: Synchronization (lock, Interlocked, ConcurrentCollections).

**FULL ANSWER:**

### Race Condition Example
```csharp
// ❌ Race condition
public class Counter
{
    private int count = 0;
    
    public void Increment()
    {
        count++;  // NOT atomic! Three operations:
                  // 1. Read count (say, 5)
                  // 2. Add 1 (6)
                  // 3. Write count (6)
    }
}

// Two threads race:
// Thread A: Read count=5
// Thread B: Read count=5
// Thread A: Write count=6
// Thread B: Write count=6
// Result: count=6, but should be 7 (two increments lost!)
```

### Synchronization Approaches

**1. Lock (Monitor)**
```csharp
public class Counter
{
    private int count = 0;
    private readonly object lockObj = new();
    
    public void Increment()
    {
        lock (lockObj)
        {
            count++;  // Only one thread at a time
        }
    }
}

// Threads wait for lock, no race condition
// Trade-off: slower (lock contention)
```

**2. Interlocked Operations**
```csharp
public class Counter
{
    private int count = 0;
    
    public void Increment()
    {
        Interlocked.Increment(ref count);  // Atomic operation
    }
}

// Atomic = executed as single operation
// Faster than lock (lock-free)
// Limited to numeric operations
```

**3. Thread-Safe Collections**
```csharp
// ❌ Not thread-safe
var list = new List<int>();

// ✓ Thread-safe
var dict = new ConcurrentDictionary<int, string>();
dict.TryAdd(1, "value");
dict.AddOrUpdate(1, "new", (k, old) => "new");

var queue = new ConcurrentQueue<int>();
queue.Enqueue(5);
queue.TryDequeue(out var item);
```

**4. Async Instead of Threading**
```csharp
// ❌ Threads + locks (complex)
public int Calculate()
{
    lock (lockObj)
    {
        var result = ExpensiveOperation();
        return result;
    }
}

// ✓ Async (simple, scalable)
public async Task<int> CalculateAsync()
{
    var result = await ExpensiveOperationAsync();
    return result;
}

// async avoids shared state, no locks needed
```

### Real Example: Bank Transfer
```csharp
// ❌ Race condition
public class Account
{
    public decimal Balance { get; set; }
    
    public void Transfer(Account target, decimal amount)
    {
        if (Balance >= amount)  // Thread A checks: $100 >= $90? Yes
        {
            Balance -= amount;  // Thread B checks: $100 >= $90? Yes
            target.Balance += amount;  // Both think they have enough!
        }
    }
}

// ✓ Synchronized
public class Account
{
    private decimal balance;
    private readonly object lockObj = new();
    
    public void Transfer(Account target, decimal amount)
    {
        lock (lockObj)
        {
            if (balance >= amount)
            {
                balance -= amount;
                lock (target.lockObj)
                {
                    target.balance += amount;
                }
            }
        }
    }
}
```

### Detecting Race Conditions
```csharp
// Stress test: run operation N times with M threads
public void TestRaceCondition()
{
    var counter = new Counter();
    var tasks = Enumerable.Range(0, 100)
        .Select(_ => Task.Run(() =>
        {
            for (int i = 0; i < 10000; i++)
                counter.Increment();
        }))
        .ToList();
    
    Task.WaitAll(tasks.ToArray());
    
    Assert.Equal(1_000_000, counter.Count);  // If race: might be < 1M
}
```

### Key Takeaway
> *Race conditions = shared data + concurrent access. Fix: synchronization (lock, Interlocked, ConcurrentCollections) or use async (avoids shared state).*

---

## Q1.15: Deadlocks - czym są i jak je rozwiązywać?

**SHORT:**
**PROBLEM**: Thread A locks X and waits for Y. Thread B locks Y and waits for X. Neither proceeds. **SOLUTION**: Lock ordering, timeouts, async.

**FULL ANSWER:**

### Classic Deadlock
```csharp
// ❌ Deadlock
public class Account
{
    private readonly object lockObj = new();
    public decimal Balance { get; set; }
    
    public void TransferFromA_to_B(Account accountB)
    {
        lock (this.lockObj)  // Lock A
        {
            // ... do something ...
            lock (accountB.lockObj)  // Wait for B
            {
                // Transfer logic
            }
        }
    }
}

// Thread 1: TransferFromA_to_B(B)
//   Locks A, waits for B lock

// Thread 2: TransferFromA_to_B(A)
//   Locks B, waits for A lock

// Neither thread proceeds = DEADLOCK
```

### Prevention: Lock Ordering
```csharp
// ✓ Always lock in same order
public void TransferFromAtoB(Account from, Account to)
{
    // Always lock smaller ID first
    var first = from.Id < to.Id ? from : to;
    var second = from.Id < to.Id ? to : from;
    
    lock (first.lockObj)
    {
        lock (second.lockObj)
        {
            // Transfer logic
            // No deadlock: consistent lock order
        }
    }
}
```

### Prevention: Timeout
```csharp
// ✓ Timeout prevents forever waiting
public bool TryTransfer(Account to, decimal amount)
{
    if (Monitor.TryEnter(this.lockObj, TimeSpan.FromSeconds(1)))
    {
        try
        {
            lock (to.lockObj)
            {
                // Transfer
                return true;
            }
        }
        finally
        {
            Monitor.Exit(this.lockObj);
        }
    }
    
    return false;  // Timeout, no deadlock
}
```

### Prevention: Async (Best)
```csharp
// ✓ Async avoids locks entirely
public async Task TransferAsync(Account to, decimal amount)
{
    using (await asyncLock.LockAsync())  // Async-friendly lock
    {
        // No deadlock: single async context
        await SaveAsync();
    }
}

// Or use SemaphoreSlim
private readonly SemaphoreSlim semaphore = new(1);

public async Task TransferAsync(Account to, decimal amount)
{
    await semaphore.WaitAsync();
    try
    {
        // Transfer
    }
    finally
    {
        semaphore.Release();
    }
}
```

### Detecting Deadlocks
```csharp
// Deadlocks are hard to detect programmatically
// Usually found through:
// - Stress testing
// - Production monitoring (thread dumps)
// - Profilers (show lock contention)

// Simple test:
public void TestDeadlock()
{
    var a = new Account { Id = 1 };
    var b = new Account { Id = 2 };
    
    var t1 = Task.Run(() => a.Transfer(b, 100));
    var t2 = Task.Run(() => b.Transfer(a, 100));
    
    bool completed = Task.WaitAll(new[] { t1, t2 }, 
        TimeSpan.FromSeconds(5));
    
    Assert.True(completed);  // If false: deadlock
}
```

### SemaphoreSlim (Modern Async)
```csharp
public class AsyncResource
{
    private readonly SemaphoreSlim semaphore = new(1, 1);
    
    public async Task AccessAsync()
    {
        await semaphore.WaitAsync();
        try
        {
            // Critical section
            await ExpensiveOperationAsync();
        }
        finally
        {
            semaphore.Release();
        }
    }
}
```

### Key Takeaway
> *Deadlock = circular lock dependencies. Prevent: consistent lock ordering, timeouts, or use async (avoids locks). Test with stress/timeout to detect.*

---

## 📌 SUMMARY - KEY POINTS

| Topic | Key Concept |
|-------|-------------|
| Value vs Ref | Stack vs Heap, copy vs reference |
| Generics | Type parameterization, reusable code |
| Nullable | `int?` for optional values, NRT for safety |
| Lambda | `x => x.Age > 18` shorthand |
| IEnum vs IQuery | In-memory vs SQL translation |
| Stack vs Heap | Local vars vs objects, auto vs GC cleanup |
| GC | Generations (0 = frequent, 2 = rare), adaptive |
| CLR | JIT compiles IL → machine code |
| IDisposable | Cleanup resources (files, DB), use `using` |
| Dispose vs ~| Explicit (fast) vs finalizer (slow, late) |
| async/await | Non-blocking, thread freed during I/O |
| Task vs void | Task = trackable, void = event-only |
| Thread vs Task | Threads = expensive, Tasks = scalable |
| Race condition | Sync shared data: lock, Interlocked, async |
| Deadlock | Circular locks: order them, timeout, async |

