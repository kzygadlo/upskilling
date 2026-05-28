using CSharpExercises.Exercises;

Console.WriteLine(@"
╔════════════════════════════════════════════════════════════╗
║   .NET INTERVIEW PREP - C# EXERCISES                       ║
║   BCM dotnet Guild Skills Framework Learning              ║
╚════════════════════════════════════════════════════════════╝
");

while (true)
{
    Console.WriteLine("""

        === DAY 1: C#, .NET Platform & Runtime ===

        1. Value vs Reference Types
        2. LINQ Fundamentals
        3. Async/Await Patterns
        4. Collections & Generics
        5. Exception Handling
        6. Boxing & Unboxing
        7. Modern C# Features (Records, Init-only, Nullable)

        0. Exit

        Choose exercise (0-7):
        """);

    var choice = Console.ReadLine()?.Trim();

    try
    {
        switch (choice)
        {
            case "1":
                ValueVsReferenceExercise.Run();
                break;
            case "2":
                LINQExercise.Run();
                break;
            case "3":
                AsyncAwaitExercise.Run();
                break;
            case "4":
                CollectionsExercise.Run();
                break;
            case "5":
                ExceptionHandlingExercise.Run();
                break;
            case "6":
                BoxingUnboxingExercise.Run();
                break;
            case "7":
                ModernCSharpExercise.Run();
                break;
            case "0":
                Console.WriteLine("Goodbye! 👋");
                return;
            default:
                Console.WriteLine("❌ Invalid choice. Try again.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error: {ex.Message}");
    }

    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
    Console.Clear();
}
