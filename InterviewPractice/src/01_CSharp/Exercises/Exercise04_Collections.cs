namespace CSharpExercises.Exercises;

public static class CollectionsExercise
{
    public static void Run()
    {
        Console.WriteLine(@"
╔════════════════════════════════════════════════════════════╗
║   EXERCISE 4: COLLECTIONS & GENERICS                      ║
╚════════════════════════════════════════════════════════════╝

CONCEPTS:
- List<T> = ordered, resizable collection
- Dictionary<K,V> = key-value pairs
- HashSet<T> = unique items, no duplicates
- Queue<T> = FIFO (First In First Out)
- Stack<T> = LIFO (Last In First Out)

INTERVIEW QUESTION:
""When would you use List vs Dictionary vs HashSet?""

═══════════════════════════════════════════════════════════
");

        // ===== LIST<T> =====
        Console.WriteLine("📌 LIST<T> (Ordered, resizable)\n");

        var projects = new List<string> { "TaskManager", "BlogApp", "API Design" };
        Console.WriteLine("var projects = new List<string> { \"TaskManager\", \"BlogApp\", \"API Design\" };");
        Console.WriteLine($"Count: {projects.Count}");
        Console.WriteLine($"First: {projects[0]}");
        Console.WriteLine($"Items: {string.Join(", ", projects)}\n");

        projects.Add("Mobile App");
        Console.WriteLine("projects.Add(\"Mobile App\");");
        Console.WriteLine($"Items: {string.Join(", ", projects)}\n");

        projects.Remove("BlogApp");
        Console.WriteLine("projects.Remove(\"BlogApp\");");
        Console.WriteLine($"Items: {string.Join(", ", projects)}\n");

        // ===== DICTIONARY<K,V> =====
        Console.WriteLine("📌 DICTIONARY<K,V> (Key-Value pairs)\n");

        var projectDict = new Dictionary<int, string>
        {
            { 1, "TaskManager" },
            { 2, "BlogApp" },
            { 3, "API Design" }
        };

        Console.WriteLine("""
            var projectDict = new Dictionary<int, string>
            {
                { 1, "TaskManager" },
                { 2, "BlogApp" },
                { 3, "API Design" }
            };
            """);

        Console.WriteLine($"Value at key 1: {projectDict[1]}");
        Console.WriteLine($"ContainsKey(2): {projectDict.ContainsKey(2)}");
        Console.WriteLine($"ContainsKey(99): {projectDict.ContainsKey(99)}\n");

        Console.WriteLine("All entries:");
        foreach (var kvp in projectDict)
            Console.WriteLine($"  {kvp.Key}: {kvp.Value}");

        // ===== HASHSET<T> =====
        Console.WriteLine("\n📌 HASHSET<T> (Unique items)\n");

        var tags = new HashSet<string> { "important", "urgent", "important", "todo" };
        Console.WriteLine("""
            var tags = new HashSet<string>
            {
                "important", "urgent", "important", "todo"
            };
            """);
        Console.WriteLine($"Count (duplicates removed): {tags.Count}");
        Console.WriteLine($"Items: {string.Join(", ", tags)}\n");

        // ===== QUEUE<T> =====
        Console.WriteLine("📌 QUEUE<T> (FIFO - First In First Out)\n");

        var queue = new Queue<string>();
        queue.Enqueue("Task 1");
        queue.Enqueue("Task 2");
        queue.Enqueue("Task 3");

        Console.WriteLine("queue.Enqueue(\"Task 1\");");
        Console.WriteLine("queue.Enqueue(\"Task 2\");");
        Console.WriteLine("queue.Enqueue(\"Task 3\");");
        Console.WriteLine($"Queue: {string.Join(" → ", queue)}\n");

        var dequeued = queue.Dequeue();
        Console.WriteLine($"queue.Dequeue() = {dequeued}  (First item removed)");
        Console.WriteLine($"Remaining: {string.Join(" → ", queue)}\n");

        // ===== STACK<T> =====
        Console.WriteLine("📌 STACK<T> (LIFO - Last In First Out)\n");

        var stack = new Stack<string>();
        stack.Push("Action 1");
        stack.Push("Action 2");
        stack.Push("Action 3");

        Console.WriteLine("stack.Push(\"Action 1\");");
        Console.WriteLine("stack.Push(\"Action 2\");");
        Console.WriteLine("stack.Push(\"Action 3\");");
        Console.WriteLine($"Stack: {string.Join(" ← ", stack)}\n");

        var popped = stack.Pop();
        Console.WriteLine($"stack.Pop() = {popped}  (Last item removed)");
        Console.WriteLine($"Remaining: {string.Join(" ← ", stack)}\n");

        // ===== WHEN TO USE WHAT =====
        Console.WriteLine("📌 WHEN TO USE WHAT\n");
        Console.WriteLine("""
            Use List<T> when:
            ✅ Order matters
            ✅ You need to access by index (projects[0])
            ✅ You frequently add/remove items
            Example: projects, tasks, comments

            Use Dictionary<K,V> when:
            ✅ You need fast lookup by key
            ✅ Each item has unique identifier
            ✅ You need key-value relationships
            Example: userId → userName, projectId → project

            Use HashSet<T> when:
            ✅ You need unique items only
            ✅ You don't care about order
            ✅ Fast membership checking
            Example: visited pages, unique tags

            Use Queue<T> when:
            ✅ FIFO order matters
            ✅ Process items in order they arrived
            Example: job queue, message queue

            Use Stack<T> when:
            ✅ LIFO order matters
            ✅ Undo/redo functionality
            Example: browser history, undo stack
        """);

        // ===== INTERVIEW ANSWER =====
        Console.WriteLine(@"

📝 INTERVIEW ANSWER:

Q: ""When would you use List vs Dictionary vs HashSet?""

A: ""It depends on your use case:

   List<T>: When order matters and you need index access.
   var projects = new List<Project>();
   var first = projects[0];  // Fast index access

   Dictionary<K,V>: When you need fast lookup by key.
   var dict = new Dictionary<int, Project>();
   var project = dict[projectId];  // O(1) lookup time

   HashSet<T>: When you only care about unique items.
   var unique = new HashSet<string>();
   unique.Add(""tag1"");  // No duplicates allowed

   In web APIs:
   - List<T> for returning collections (GetAll)
   - Dictionary<K,V> for caching (userId -> user)
   - HashSet<T> for permissions (roles: admin, user)""
");
    }
}
