namespace CSharpExercises.Exercises;

public static class AsyncAwaitExercise
{
    public static void Run()
    {
        Console.WriteLine(@"
╔════════════════════════════════════════════════════════════╗
║   EXERCISE 3: ASYNC/AWAIT PATTERNS                        ║
╚════════════════════════════════════════════════════════════╝

CONCEPTS:
- async/await = concurrent operations WITHOUT threads
- await = ""release thread, continue when done""
- NOT multi-threading, but concurrency
- Better scalability for I/O operations

INTERVIEW QUESTION:
""Why is async/await important in web APIs?""

═══════════════════════════════════════════════════════════
");

        // Synchronous vs Asynchronous
        Console.WriteLine("📌 SYNCHRONOUS vs ASYNCHRONOUS\n");

        Console.WriteLine("SYNCHRONOUS (blocking):");
        Console.WriteLine("  Thread waits for operation to complete");
        Console.WriteLine("  var data = FetchDataFromDatabase(); // BLOCKED HERE");
        Console.WriteLine("  Console.WriteLine(data); // Runs after fetch completes\n");

        Console.WriteLine("ASYNCHRONOUS (non-blocking):");
        Console.WriteLine("  Thread released, continues other work");
        Console.WriteLine("  var data = await FetchDataFromDatabaseAsync(); // Thread freed!");
        Console.WriteLine("  Console.WriteLine(data); // Resumes when data arrives\n");

        // ===== DEMONSTRATE ASYNC =====
        Console.WriteLine("📌 RUNNING ASYNC DEMONSTRATION\n");
        RunAsyncDemo().GetAwaiter().GetResult();

        // ===== MULTIPLE CONCURRENT OPERATIONS =====
        Console.WriteLine("\n📌 MULTIPLE CONCURRENT OPERATIONS\n");
        RunConcurrentDemo().GetAwaiter().GetResult();

        // ===== INTERVIEW ANSWER =====
        Console.WriteLine(@"

📝 INTERVIEW ANSWER:

Q: ""Why is async/await important in web APIs?""

A: ""In web APIs, many requests come concurrently.
   If each request blocks a thread:
   - 10,000 requests = need 10,000 threads (wasteful, slow)

   With async/await:
   - 10,000 requests = 1 thread handles them all
   - Thread is released during I/O waits
   - When I/O completes, thread resumes

   Example:
   Request 1: await dbContext.GetProjectAsync()
   └─ Thread freed while waiting on DB

   Request 2-10000 can use that thread meanwhile

   When Request 1's DB response arrives:
   └─ Thread resumes Request 1

   Result: 1 thread pool handles thousands of requests.""
");
    }

    // ===== ASYNC DEMO =====
    static async Task RunAsyncDemo()
    {
        Console.WriteLine("Starting async operations...\n");
        var startTime = DateTime.Now;

        // Task 1: Simulate network request (500ms)
        Console.WriteLine($"[{GetElapsed(startTime)}] Fetching project from API...");
        var project = await SimulateNetworkRequestAsync("project", 500);
        Console.WriteLine($"[{GetElapsed(startTime)}] ✅ Got project: {project}\n");

        // Task 2: Simulate database query (300ms)
        Console.WriteLine($"[{GetElapsed(startTime)}] Querying database...");
        var tasks = await SimulateNetworkRequestAsync("tasks", 300);
        Console.WriteLine($"[{GetElapsed(startTime)}] ✅ Got tasks: {tasks}\n");

        // Task 3: Simulate file I/O (200ms)
        Console.WriteLine($"[{GetElapsed(startTime)}] Reading file...");
        var file = await SimulateNetworkRequestAsync("file", 200);
        Console.WriteLine($"[{GetElapsed(startTime)}] ✅ Got file: {file}\n");

        Console.WriteLine($"[{GetElapsed(startTime)}] ✅ All done!");
        Console.WriteLine($"⏱️  Total time: {(DateTime.Now - startTime).TotalMilliseconds:F0}ms");
        Console.WriteLine("💡 If done synchronously, would be 1000ms (500+300+200)");
        Console.WriteLine("💡 With async, operations run concurrently!");
    }

    // ===== CONCURRENT OPERATIONS =====
    static async Task RunConcurrentDemo()
    {
        Console.WriteLine("Running 5 operations concurrently...\n");
        var startTime = DateTime.Now;

        // Start all tasks at once (concurrent)
        var task1 = SimulateNetworkRequestAsync("Task 1", 200);
        var task2 = SimulateNetworkRequestAsync("Task 2", 150);
        var task3 = SimulateNetworkRequestAsync("Task 3", 100);
        var task4 = SimulateNetworkRequestAsync("Task 4", 250);
        var task5 = SimulateNetworkRequestAsync("Task 5", 180);

        Console.WriteLine($"[0ms] Started 5 tasks\n");

        // Wait for all to complete
        var results = await Task.WhenAll(task1, task2, task3, task4, task5);

        Console.WriteLine($"\n[{(DateTime.Now - startTime).TotalMilliseconds:F0}ms] ✅ All tasks completed!");
        Console.WriteLine($"Results: {string.Join(", ", results)}\n");
        Console.WriteLine("💡 All 5 tasks ran at the SAME TIME (concurrent)");
        Console.WriteLine("💡 Total time = slowest task (250ms), not sum (880ms)");
    }

    // Simulates I/O operation (network, database, etc)
    static async Task<string> SimulateNetworkRequestAsync(string name, int delayMs)
    {
        var startTime = DateTime.Now;
        Console.WriteLine($"[{GetElapsed(startTime)}] ⏳ {name} (waiting {delayMs}ms)...");

        await Task.Delay(delayMs);

        Console.WriteLine($"[{GetElapsed(startTime)}] ✅ {name} completed");
        return $"{name} data";
    }

    static string GetElapsed(DateTime startTime) =>
        $"{(DateTime.Now - startTime).TotalMilliseconds:F0}ms";
}
