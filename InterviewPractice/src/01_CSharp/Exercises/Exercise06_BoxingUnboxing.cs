namespace CSharpExercises.Exercises;

public static class BoxingUnboxingExercise
{
    public static void Run()
    {
        Console.WriteLine(@"
╔════════════════════════════════════════════════════════════╗
║   EXERCISE 6: BOXING & UNBOXING                           ║
╚════════════════════════════════════════════════════════════╝

CONCEPTS:
- Boxing = value type → object (heap allocation)
- Unboxing = object → value type (copy back)
- Both have performance cost
- Avoid boxing with generic collections

INTERVIEW QUESTION:
""Explain boxing and unboxing. When should you avoid them?""

═══════════════════════════════════════════════════════════
");

        // ===== BOXING =====
        Console.WriteLine("📌 BOXING (Value type → Object/Heap)\n");

        int number = 42;
        Console.WriteLine($"int number = 42;  // Stack memory");
        Console.WriteLine($"  Address: [local stack]");
        Console.WriteLine($"  Value: 42\n");

        object boxed = number;  // Boxing: allocate on heap, copy value
        Console.WriteLine($"object boxed = number;  // BOXING");
        Console.WriteLine($"  Allocate object on heap");
        Console.WriteLine($"  Copy value (42) to heap");
        Console.WriteLine($"  boxed points to heap location\n");

        // ===== UNBOXING =====
        Console.WriteLine("📌 UNBOXING (Object → Value type)\n");

        int unboxed = (int)boxed;  // Unboxing: copy from heap back to value
        Console.WriteLine($"int unboxed = (int)boxed;  // UNBOXING");
        Console.WriteLine($"  Check type (must be int)");
        Console.WriteLine($"  Copy value (42) back to stack");
        Console.WriteLine($"  unboxed = 42\n");

        // ===== TYPE MISMATCH =====
        Console.WriteLine("📌 TYPE MISMATCH (throws exception)\n");

        object boxedInt = 42;
        Console.WriteLine($"object boxedInt = 42;");

        try
        {
            long wrongType = (long)boxedInt;
        }
        catch (InvalidCastException ex)
        {
            Console.WriteLine($"❌ (long)boxedInt throws InvalidCastException");
            Console.WriteLine($"   Reason: boxedInt is int, not long");
            Console.WriteLine($"   Boxing preserves original type\n");
        }

        // ===== PERFORMANCE PROBLEM =====
        Console.WriteLine("📌 PERFORMANCE PROBLEM (Iterations)\n");

        Console.WriteLine("❌ BAD: Boxing in loop");
        Console.WriteLine("""
            List<object> list = new List<object>();
            for (int i = 0; i < 1_000_000; i++)
            {
                list.Add(i);  // BOXING! 1 million times!
            }
        """);

        var badWatch = System.Diagnostics.Stopwatch.StartNew();
        var badList = new List<object>();
        for (int i = 0; i < 1_000_000; i++)
        {
            badList.Add(i);  // Boxing
        }
        badWatch.Stop();
        Console.WriteLine($"Time: {badWatch.ElapsedMilliseconds}ms");
        Console.WriteLine($"⚠️  Allocations: 1 million! GC pressure!\n");

        Console.WriteLine("✅ GOOD: Generic collection (no boxing)");
        Console.WriteLine("""
            List<int> list = new List<int>();
            for (int i = 0; i < 1_000_000; i++)
            {
                list.Add(i);  // No boxing!
            }
        """);

        var goodWatch = System.Diagnostics.Stopwatch.StartNew();
        var goodList = new List<int>();
        for (int i = 0; i < 1_000_000; i++)
        {
            goodList.Add(i);  // No boxing
        }
        goodWatch.Stop();
        Console.WriteLine($"Time: {goodWatch.ElapsedMilliseconds}ms");
        Console.WriteLine($"✅ No allocations! Much faster!\n");

        Console.WriteLine($"Performance improvement: {((double)badWatch.ElapsedMilliseconds / goodWatch.ElapsedMilliseconds):F1}x faster\n");

        // ===== WHERE BOXING HAPPENS =====
        Console.WriteLine("📌 WHERE BOXING HAPPENS\n");

        Console.WriteLine("❌ Boxing with object collection:");
        var objectList = new List<object>();
        objectList.Add(5);           // BOXING
        objectList.Add("text");      // No boxing (already reference type)
        objectList.Add(3.14);        // BOXING
        Console.WriteLine("  5 → boxed to object");
        Console.WriteLine("  3.14 → boxed to object\n");

        Console.WriteLine("❌ Boxing with non-generic interface:");
        IFormattable formatted = 42;  // BOXING
        Console.WriteLine("  42 → boxed (int doesn't implement IFormattable)\n");

        Console.WriteLine("✅ No boxing with generic collection:");
        var intList = new List<int>();
        intList.Add(5);              // No boxing (same type)
        Console.WriteLine("  5 → stays as int\n");

        // ===== INTERVIEW ANSWER =====
        Console.WriteLine(@"

📝 INTERVIEW ANSWER:

Q: ""Explain boxing and unboxing. When should you avoid them?""

A: ""Boxing wraps a value type in an object on the heap.
   Unboxing extracts the value back.

   Example:
   int x = 5;           // Stack
   object box = x;      // BOXING: allocate heap, copy value
   int y = (int)box;    // UNBOXING: copy from heap to stack

   Performance cost:
   - Heap allocation (slow)
   - Garbage collection (cleanup)
   - Memory overhead
   - Type checking during unbox

   Avoid boxing by:
   1. Use generic collections (List<int>, not List<object>)
   2. Don't pass value types as object parameters
   3. Use constraints if you need generics

   In real code:
   ❌ List<object> with integers = millions of boxing operations
   ✅ List<int> = no boxing, much faster

   Rule of thumb: If you're boxing, you're probably doing it wrong.""
");
    }
}
