namespace CSharpExercises.Exercises;

public static class LINQExercise
{
    public static void Run()
    {
        Console.WriteLine(@"
╔════════════════════════════════════════════════════════════╗
║   EXERCISE 2: LINQ FUNDAMENTALS                           ║
╚════════════════════════════════════════════════════════════╝

CONCEPTS:
- LINQ = Language Integrated Query
- Works with IEnumerable (lists, arrays, etc)
- Where = filter data
- Select = transform data
- OrderBy = sort data
- FirstOrDefault = get first item or null

INTERVIEW QUESTION:
""What's the difference between LINQ to Objects and LINQ to Entities?""

═══════════════════════════════════════════════════════════
");

        // Create sample data
        var projects = new List<Project>
        {
            new() { Id = 1, Name = "TaskManager", Status = "Active", CreatedAt = new DateTime(2024, 1, 15) },
            new() { Id = 2, Name = "BlogApp", Status = "Completed", CreatedAt = new DateTime(2024, 2, 20) },
            new() { Id = 3, Name = "API Design", Status = "Active", CreatedAt = new DateTime(2024, 1, 10) },
            new() { Id = 4, Name = "Mobile App", Status = "Planning", CreatedAt = new DateTime(2024, 3, 5) },
            new() { Id = 5, Name = "Database Optimization", Status = "Active", CreatedAt = new DateTime(2024, 1, 25) }
        };

        Console.WriteLine("📌 ORIGINAL DATA:\n");
        foreach (var p in projects)
            Console.WriteLine($"  {p.Id}. {p.Name,-25} | {p.Status,-10} | {p.CreatedAt:yyyy-MM-dd}");

        // ===== WHERE (Filter) =====
        Console.WriteLine("\n📌 WHERE (Filter active projects)\n");
        var activeProjects = projects.Where(p => p.Status == "Active").ToList();
        Console.WriteLine("var activeProjects = projects.Where(p => p.Status == \"Active\").ToList();");
        Console.WriteLine($"Result: {activeProjects.Count} projects\n");
        foreach (var p in activeProjects)
            Console.WriteLine($"  - {p.Name}");

        // ===== SELECT (Transform) =====
        Console.WriteLine("\n📌 SELECT (Get only names)\n");
        var names = projects.Select(p => p.Name).ToList();
        Console.WriteLine("var names = projects.Select(p => p.Name).ToList();");
        Console.WriteLine($"Result: {string.Join(", ", names)}\n");

        // ===== ORDER BY =====
        Console.WriteLine("\n📌 ORDER BY (Sort by creation date)\n");
        var sorted = projects.OrderBy(p => p.CreatedAt).ToList();
        Console.WriteLine("var sorted = projects.OrderBy(p => p.CreatedAt).ToList();");
        Console.WriteLine("Sorted by date:\n");
        foreach (var p in sorted)
            Console.WriteLine($"  {p.CreatedAt:yyyy-MM-dd} - {p.Name}");

        // ===== CHAINING (Multiple operations) =====
        Console.WriteLine("\n📌 CHAINING (Active projects, sorted by name)\n");
        var result = projects
            .Where(p => p.Status == "Active")
            .OrderBy(p => p.Name)
            .Select(p => $"{p.Name} (created: {p.CreatedAt:yyyy-MM-dd})")
            .ToList();

        Console.WriteLine("""
            var result = projects
                .Where(p => p.Status == "Active")
                .OrderBy(p => p.Name)
                .Select(p => $"{p.Name} (created: {p.CreatedAt:yyyy-MM-dd})")
                .ToList();
            """);
        Console.WriteLine("Result:\n");
        foreach (var r in result)
            Console.WriteLine($"  - {r}");

        // ===== FIRST vs FIRSTORDEFAULT =====
        Console.WriteLine("\n📌 FIRST vs FIRSTORDEFAULT\n");

        var first = projects.First(p => p.Status == "Active");
        Console.WriteLine($"projects.First(p => p.Status == \"Active\") = {first.Name}");
        Console.WriteLine("⚠️  Throws exception if no match found\n");

        var firstOrDefault = projects.FirstOrDefault(p => p.Status == "NonExistent");
        Console.WriteLine($"projects.FirstOrDefault(p => p.Status == \"NonExistent\") = {firstOrDefault?.Name ?? "null"}");
        Console.WriteLine("✅ Returns null if no match found\n");

        // ===== COUNT & ANY =====
        Console.WriteLine("\n📌 COUNT & ANY\n");

        var count = projects.Count(p => p.Status == "Active");
        Console.WriteLine($"projects.Count(p => p.Status == \"Active\") = {count}");

        var hasActive = projects.Any(p => p.Status == "Active");
        Console.WriteLine($"projects.Any(p => p.Status == \"Active\") = {hasActive}");

        var hasCompleted = projects.Any(p => p.Status == "Completed");
        Console.WriteLine($"projects.Any(p => p.Status == \"Completed\") = {hasCompleted}\n");

        // ===== GROUPBY =====
        Console.WriteLine("\n📌 GROUPBY (Group by status)\n");

        var grouped = projects.GroupBy(p => p.Status).ToList();
        Console.WriteLine("var grouped = projects.GroupBy(p => p.Status).ToList();");
        Console.WriteLine("Grouped result:\n");
        foreach (var group in grouped)
        {
            Console.WriteLine($"  {group.Key} ({group.Count()} projects):");
            foreach (var p in group)
                Console.WriteLine($"    - {p.Name}");
        }

        // ===== INTERVIEW ANSWER =====
        Console.WriteLine(@"

📝 INTERVIEW ANSWER:

Q: ""What's the difference between LINQ to Objects and LINQ to Entities?""

A: ""LINQ to Objects executes in-memory (C#):
   var results = list.Where(x => x > 5).ToList();
   └─ Executes in C#, filters items in memory

   LINQ to Entities translates to SQL and executes in database:
   var results = dbContext.Items.Where(x => x.Value > 5).ToList();
   └─ Translated to: SELECT * FROM Items WHERE Value > 5
   └─ Executes on SQL Server, more efficient

   Key difference: Where the code executes (memory vs database).""
");
    }
}

// Helper class
public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
