namespace CSharpExercises.Exercises;

public static class ModernCSharpExercise
{
    public static void Run()
    {
        Console.WriteLine(@"
╔════════════════════════════════════════════════════════════╗
║   EXERCISE 7: MODERN C# FEATURES (C# 9+)                  ║
╚════════════════════════════════════════════════════════════╝

CONCEPTS:
- Records (C# 9) = immutable data containers
- Init-only properties = set once, then read-only
- Nullable reference types = null safety
- Pattern matching = powerful conditionals

═══════════════════════════════════════════════════════════
");

        // ===== RECORDS =====
        Console.WriteLine("📌 RECORDS (C# 9+) - Immutable data\n");

        var project1 = new ProjectRecord(1, "TaskManager");
        var project2 = new ProjectRecord(1, "TaskManager");

        Console.WriteLine("record ProjectRecord(int Id, string Name);");
        Console.WriteLine($"\nvar project1 = new ProjectRecord(1, \"TaskManager\");");
        Console.WriteLine($"var project2 = new ProjectRecord(1, \"TaskManager\");\n");

        Console.WriteLine($"project1 == project2: {project1 == project2}");
        Console.WriteLine("✅ Records compare by VALUE, not reference\n");

        Console.WriteLine($"project1.ToString(): {project1}");
        Console.WriteLine("✅ Records have built-in ToString()\n");

        // ===== INIT-ONLY PROPERTIES =====
        Console.WriteLine("📌 INIT-ONLY PROPERTIES (C# 9+) - Immutability\n");

        var task = new TaskItem { Id = 1, Title = "Test", Status = "Active" };
        Console.WriteLine("""
            var task = new TaskItem
            {
                Id = 1,
                Title = "Test",
                Status = "Active"
            };
            """);

        Console.WriteLine($"task.Title = {task.Title}  (can read)\n");

        try
        {
            task.Title = "Updated";  // ❌ Can't set init-only property
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"❌ task.Title = \"Updated\"  // Error!");
            Console.WriteLine("   Cannot set init-only property after initialization\n");
        }

        // ===== NULLABLE REFERENCE TYPES =====
        Console.WriteLine("📌 NULLABLE REFERENCE TYPES (C# 8+)\n");

        string nonNull = "hello";      // Cannot be null
        string? nullable = null;       // Can be null
        string? alsoPossible = "world"; // Can be non-null too

        Console.WriteLine("string nonNull = \"hello\";     // Cannot be null");
        Console.WriteLine("string? nullable = null;        // Can be null");
        Console.WriteLine("string? alsoPossible = \"world\"; // Can be non-null\n");

        // Compiler helps prevent null errors
        int length = nonNull.Length;           // ✅ Safe
        Console.WriteLine($"nonNull.Length = {length}\n");

        // int nullLength = nullable.Length;   // ❌ Compiler warning
        int? safeNullLength = nullable?.Length;  // ✅ Safe (null-conditional)
        Console.WriteLine($"nullable?.Length = {safeNullLength}\n");

        // ===== PATTERN MATCHING =====
        Console.WriteLine("📌 PATTERN MATCHING (C# 7+)\n");

        object[] items = { "text", 42, 3.14, new TaskItem(), null! };

        foreach (var item in items)
        {
            var result = item switch
            {
                string s => $"String: {s}",
                int i => $"Integer: {i}",
                double d => $"Double: {d}",
                TaskItem t => $"Task: {t.Title}",
                null => "Null value",
                _ => "Unknown type"
            };

            Console.WriteLine(result);
        }

        Console.WriteLine();

        // Pattern matching with properties
        var activeProject = new ProjectData { Name = "Active", Status = "Active", TaskCount = 5 };
        var emptyProject = new ProjectData { Name = "Empty", Status = "Active", TaskCount = 0 };
        var closedProject = new ProjectData { Name = "Closed", Status = "Completed", TaskCount = 10 };

        Console.WriteLine("Pattern matching with properties:\n");
        CheckProject(activeProject);
        CheckProject(emptyProject);
        CheckProject(closedProject);

        // ===== TOP-LEVEL STATEMENTS =====
        Console.WriteLine("\n📌 TOP-LEVEL STATEMENTS (C# 9+)\n");

        Console.WriteLine("""
            ✅ OLD (C# 8):
            class Program
            {
                static void Main(string[] args)
                {
                    Console.WriteLine("Hello");
                }
            }

            ✅ NEW (C# 9+):
            Console.WriteLine("Hello");
            // That's it! No class, no Main needed
        """);

        // ===== INTERVIEW ANSWER =====
        Console.WriteLine(@"

📝 INTERVIEW ANSWER:

Q: ""What modern C# features should you know?""

A: ""Key modern features (C# 9+):

   1. Records: Immutable data objects
      public record Project(int Id, string Name);
      └─ Auto-implements Equals, ToString

   2. Init-only properties: Set once, then immutable
      public class Task { public string Title { get; init; } }
      └─ Prevents accidental modifications

   3. Nullable reference types: Null safety
      string? nullable = null;    // Can be null
      string nonNull = ""hello"";  // Never null
      └─ Compiler prevents null errors

   4. Pattern matching: Powerful conditionals
      var result = obj switch {
          string s => ...,
          int i => ...,
          _ => ...
      };
      └─ More readable than if-else chains

   5. Top-level statements: No boilerplate
      Console.WriteLine(""Hello"");  // Direct code
      └─ No need for Main() method

   Use these in modern .NET 9 projects!""
");
    }

    // Demonstrates pattern matching
    static void CheckProject(ProjectData project)
    {
        var status = project switch
        {
            { Status: "Active", TaskCount: 0 } =>
                $"🤔 {project.Name}: Active but no tasks",
            { Status: "Active", TaskCount: > 0 } =>
                $"✅ {project.Name}: Active with {project.TaskCount} tasks",
            { Status: "Completed" } =>
                $"✔️ {project.Name}: Completed",
            _ =>
                $"❓ {project.Name}: Unknown status"
        };

        Console.WriteLine(status);
    }

    // Record (C# 9+)
    public record ProjectRecord(int Id, string Name);

    // Class with init-only properties
    public class TaskItem
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;

        public override string ToString() => $"Task: {Title}";
    }

    // Data class for pattern matching demo
    public class ProjectData
    {
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int TaskCount { get; set; }
    }
}
