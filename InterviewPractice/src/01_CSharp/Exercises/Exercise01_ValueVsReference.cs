using System;

namespace CSharpExercises.Exercises;

public static class ValueVsReferenceExercise
{
    public static void Run()
    {
        Console.WriteLine(@"
╔════════════════════════════════════════════════════════════╗
║   EXERCISE 1: VALUE vs REFERENCE TYPES                    ║
╚════════════════════════════════════════════════════════════╝

CONCEPTS:
- Value types (int, bool, struct) → allocated on STACK
- Reference types (class, string) → allocated on HEAP
- Stack: automatic cleanup, fast
- Heap: garbage collected, slower

INTERVIEW QUESTION:
""Explain value vs reference types in C#. How does it affect memory?""

═══════════════════════════════════════════════════════════
");

        // ===== VALUE TYPES (STACK) =====
        Console.WriteLine("📌 VALUE TYPES (Stack Memory)\n");

        int x = 5;
        int y = x;  // Copy the VALUE
        y = 10;

        Console.WriteLine($"int x = 5;");
        Console.WriteLine($"int y = x;  // Copies value");
        Console.WriteLine($"y = 10;");
        Console.WriteLine($"x = {x}, y = {y}");
        Console.WriteLine("✅ x is still 5 (y's change doesn't affect x)\n");

        // ===== REFERENCE TYPES (HEAP) =====
        Console.WriteLine("📌 REFERENCE TYPES (Heap Memory)\n");

        var project1 = new Project { Id = 1, Name = "TaskManager" };
        var project2 = project1;  // Copy the REFERENCE (not value)
        project2.Name = "Updated";

        Console.WriteLine($"var project1 = new Project {{ Id = 1, Name = \"TaskManager\" }};");
        Console.WriteLine($"var project2 = project1;  // Copies reference");
        Console.WriteLine($"project2.Name = \"Updated\";");
        Console.WriteLine($"project1.Name = {project1.Name}");
        Console.WriteLine("✅ project1.Name changed! Both variables point to SAME object\n");

        // ===== NULLABLE TYPES =====
        Console.WriteLine("📌 NULLABLE TYPES (C# 8+)\n");

        int? nullableInt = null;
        string? nullableString = "hello";

        Console.WriteLine($"int? nullableInt = null;");
        Console.WriteLine($"string? nullableString = \"hello\";");
        Console.WriteLine($"nullableInt.HasValue = {nullableInt?.ToString() ?? "null"}");
        Console.WriteLine($"nullableString?.Length = {nullableString?.Length}");
        Console.WriteLine("✅ Nullable types prevent null reference exceptions\n");

        // ===== MEMORY VISUALIZATION =====
        Console.WriteLine("📌 MEMORY LAYOUT\n");
        Console.WriteLine(@"
STACK                           HEAP
─────────────────────────────────────────────
int x: [5]
int y: [10]
                                Project@12345
var project1: [ref@12345] ──→   { Id: 1
var project2: [ref@12345] ──→    Name: Updated }

Result:
- x, y are independent (value types)
- project1, project2 point to same object (reference type)
");

        // ===== INTERVIEW ANSWER =====
        Console.WriteLine(@"
📝 INTERVIEW ANSWER:

Q: ""Explain value vs reference types.""

A: ""Value types are stored on the stack (int, bool, struct).
   Reference types are stored on the heap (class, string).

   When you copy a value type, you get a new independent copy.
   When you copy a reference type, both variables point to the SAME object.

   This affects:
   - Performance: stack is faster than heap
   - Memory: each value type instance is independent
   - Behavior: changes to objects affect all references to them

   Example: int x = 5; int y = x; y = 10; x is still 5.
   But: var p1 = new Project(); var p2 = p1; p2.Name = ""X"";
        p1.Name is also ""X"" now.""
");
    }
}

// Helper class for reference type demonstration
public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public override string ToString() => $"Project(Id: {Id}, Name: {Name})";
}
