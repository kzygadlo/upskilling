# Interview Practice - Coding Exercises

Interactive C# console application for practicing .NET interview topics.

## 📂 Project Structure

```
InterviewPractice/
├── CodingExercises.sln         ← Solution file
├── src/
│   ├── 01_CSharp/              ← Day 1 exercises
│   │   ├── 01_CSharp.csproj
│   │   ├── Program.cs          ← Interactive menu
│   │   └── Exercises/
│   │       ├── Exercise01_ValueVsReference.cs
│   │       ├── Exercise02_LINQ.cs
│   │       ├── Exercise03_AsyncAwait.cs
│   │       ├── Exercise04_Collections.cs
│   │       ├── Exercise05_ExceptionHandling.cs
│   │       ├── Exercise06_BoxingUnboxing.cs
│   │       └── Exercise07_ModernCSharp.cs
│   │
│   ├── 02_API/                 ← (Coming: Day 2 exercises)
│   ├── 03_DataAccess/          ← (Coming: Day 3 exercises)
│   └── 04_Testing/             ← (Coming: Day 4 exercises)
│
└── README.md
```

## 🚀 Running

```bash
# Navigate to project
cd InterviewPractice

# Run solution
dotnet run --project src/01_CSharp

# Or open in IDE
# - Visual Studio: Open CodingExercises.sln
# - VS Code: Open folder, run C# exercise in terminal
```

## 📝 Exercises Overview

### 01_CSharp (Day 1: C#, .NET Platform & Runtime)

#### Exercise 1: Value vs Reference Types
- **Concept**: Stack vs Heap memory allocation
- **Interview Question**: "Explain value vs reference types in C#. How does it affect memory?"
- **Topics Covered**:
  - Value types (int, bool, struct)
  - Reference types (class, string)
  - Nullable types
  - Memory layout visualization

#### Exercise 2: LINQ Fundamentals
- **Concept**: Querying and transforming collections
- **Interview Question**: "What's the difference between LINQ to Objects and LINQ to Entities?"
- **Topics Covered**:
  - Where (filtering)
  - Select (transformation)
  - OrderBy (sorting)
  - Chaining multiple operations
  - First vs FirstOrDefault
  - GroupBy
  - Count & Any

#### Exercise 3: Async/Await Patterns
- **Concept**: Concurrent operations without multi-threading
- **Interview Question**: "Why is async/await important in web APIs?"
- **Topics Covered**:
  - Synchronous vs asynchronous
  - async/await demonstration
  - Concurrent operations
  - Performance benefits
  - Thread pool efficiency

#### Exercise 4: Collections & Generics
- **Concept**: When to use List, Dictionary, HashSet, Queue, Stack
- **Interview Question**: "When would you use List vs Dictionary vs HashSet?"
- **Topics Covered**:
  - List<T>
  - Dictionary<K,V>
  - HashSet<T>
  - Queue<T> (FIFO)
  - Stack<T> (LIFO)
  - Performance considerations

#### Exercise 5: Exception Handling
- **Concept**: Proper error handling in applications
- **Interview Question**: "How would you handle errors in an API endpoint?"
- **Topics Covered**:
  - try-catch blocks
  - Multiple catch blocks (specific → general)
  - finally (cleanup)
  - Custom exceptions
  - API error handling patterns
  - HTTP status codes

#### Exercise 6: Boxing & Unboxing
- **Concept**: Value type to object conversion (performance impact)
- **Interview Question**: "Explain boxing and unboxing. When should you avoid them?"
- **Topics Covered**:
  - Boxing (value → object/heap)
  - Unboxing (object → value)
  - Type safety
  - Performance costs
  - Real-world impact (1M iterations comparison)
  - Where boxing happens

#### Exercise 7: Modern C# Features
- **Concept**: C# 9+ features (Records, Init-only, Nullable, Pattern Matching)
- **Interview Question**: "What modern C# features should you know?"
- **Topics Covered**:
  - Records (immutable data)
  - Init-only properties
  - Nullable reference types
  - Pattern matching (switch expressions)
  - Top-level statements

## 📊 Interview Questions Per Exercise

Each exercise includes:
1. **Interview Question** (what you might be asked)
2. **Concept Explanation** (why it matters)
3. **Code Examples** (practical demonstration)
4. **Expected Answer** (how to respond in interview)

## 🎯 How to Use

### For Learning
1. Open an exercise
2. Read the concept explanation
3. Follow the code examples
4. Understand the interview question
5. Practice explaining the answer

### For Interview Prep
1. Run the exercise
2. Read the "Interview Answer" section
3. Practice explaining out loud (15-30 seconds)
4. Try explaining without looking at notes

### Quick Reference
- **Value vs Reference**: Stack is fast, independent copies
- **LINQ**: Filtering, transforming, sorting collections
- **Async/Await**: Non-blocking operations, better scalability
- **Collections**: List=order, Dict=lookup, HashSet=unique, Queue=FIFO, Stack=LIFO
- **Exceptions**: Catch specific, log, return appropriate HTTP status
- **Boxing**: Avoid with generic collections (performance)
- **Modern C#**: Records, init-only, nullable, pattern matching

## 📚 Related Resources

- [INTERVIEW_PREP_4DAYS.md](../INTERVIEW_PREP_4DAYS.md) - Full 4-day learning plan
- [CLAUDE.md](../CLAUDE.md) - Project instructions
- [TaskManager Backend](../Backend/) - Real project using these concepts

## 🔄 Next Steps

After 01_CSharp:
- **02_API** - ASP.NET Core REST API exercises
- **03_DataAccess** - Entity Framework & SQL exercises
- **04_Testing** - Unit testing & mocking exercises

## 💡 Tips

- Run exercises multiple times, explain out loud
- Try modifying code to understand behavior
- Answer interview questions before peeking at answers
- Use this as reference during actual interviews
- Time yourself (aim for 1-2 min per answer)

---

**Created**: May 27, 2026  
**For**: BCM dotnet Guild Internal Interview  
**Status**: Day 1 (01_CSharp) Complete
