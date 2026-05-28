namespace CSharpExercises.Exercises;

public static class ExceptionHandlingExercise
{
    public static void Run()
    {
        Console.WriteLine(@"
╔════════════════════════════════════════════════════════════╗
║   EXERCISE 5: EXCEPTION HANDLING                          ║
╚════════════════════════════════════════════════════════════╝

CONCEPTS:
- try-catch = catch specific errors
- finally = cleanup (always runs)
- Custom exceptions = domain-specific errors
- Proper exception handling = critical for APIs

INTERVIEW QUESTION:
""How would you handle errors in an API endpoint?""

═══════════════════════════════════════════════════════════
");

        // ===== BASIC TRY-CATCH =====
        Console.WriteLine("📌 BASIC TRY-CATCH\n");

        Console.WriteLine("try-catch example:");
        try
        {
            int result = 10 / int.Parse("0");
            Console.WriteLine(result);
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine($"❌ Caught: {ex.GetType().Name} - {ex.Message}\n");
        }

        // ===== MULTIPLE CATCH BLOCKS =====
        Console.WriteLine("📌 MULTIPLE CATCH BLOCKS (Specific → General)\n");

        SimulateApiCall(null);
        SimulateApiCall("0");
        SimulateApiCall("invalid");
        SimulateApiCall("5");

        // ===== FINALLY =====
        Console.WriteLine("\n📌 FINALLY (Always runs)\n");

        try
        {
            Console.WriteLine("Try: Opening database connection");
            throw new Exception("Connection failed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Catch: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Finally: Closing database connection (always runs)\n");
        }

        // ===== USING STATEMENT (IDisposable) =====
        Console.WriteLine("📌 USING STATEMENT (Automatic cleanup)\n");

        using (var file = new FileSimulator("data.txt"))
        {
            Console.WriteLine("Using: Resource is open");
            Console.WriteLine("Using: Working with resource");
        }
        Console.WriteLine("After using: Resource is disposed automatically\n");

        // ===== CUSTOM EXCEPTIONS =====
        Console.WriteLine("📌 CUSTOM EXCEPTIONS\n");

        try
        {
            var project = CreateProject("", "description");
        }
        catch (InvalidProjectException ex)
        {
            Console.WriteLine($"❌ Custom exception: {ex.Message}\n");
        }

        // ===== API ERROR HANDLING PATTERN =====
        Console.WriteLine("📌 API ERROR HANDLING PATTERN\n");
        Console.WriteLine("""
            [HttpGet("{id}")]
            public async Task<IActionResult> GetProject(int id)
            {
                try
                {
                    var project = await _service.GetAsync(id);

                    if (project == null)
                        return NotFound();  // 404

                    return Ok(project);    // 200
                }
                catch (ArgumentException ex)
                {
                    _logger.LogWarning(ex, "Invalid input");
                    return BadRequest(ex.Message);  // 400
                }
                catch (UnauthorizedAccessException ex)
                {
                    _logger.LogWarning(ex, "Unauthorized");
                    return Forbid();  // 403
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error");
                    return StatusCode(500);  // 500
                }
            }
        """);

        // ===== INTERVIEW ANSWER =====
        Console.WriteLine(@"

📝 INTERVIEW ANSWER:

Q: ""How would you handle errors in an API endpoint?""

A: ""Use try-catch to handle specific exceptions:

   1. Catch specific exceptions first (ArgumentException, etc)
      → Return 400 Bad Request

   2. Catch authorization exceptions
      → Return 403 Forbidden

   3. Catch generic Exception last (catch-all)
      → Return 500 Internal Server Error
      → Log the error for debugging

   4. Log at appropriate levels:
      - LogWarning: expected errors (validation, auth)
      - LogError: unexpected errors (bugs)

   5. Return meaningful status codes:
      - 200 OK: success
      - 400 Bad Request: invalid input
      - 401 Unauthorized: missing auth
      - 403 Forbidden: auth but no permission
      - 404 Not Found: resource doesn't exist
      - 500 Internal Server Error: unexpected error

   Never expose internal stack traces to client.
   Always log full details server-side.""
");
    }

    // Demonstrates multiple catch blocks
    static void SimulateApiCall(string? input)
    {
        try
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            int value = int.Parse(input);

            if (value == 0)
                throw new DivideByZeroException("Cannot divide by zero");

            int result = 10 / value;
            Console.WriteLine($"✅ Success: 10 / {value} = {result}\n");
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"❌ Null input: {ex.Message}");
            Console.WriteLine("→ Return 400 Bad Request\n");
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"❌ Invalid format: {ex.Message}");
            Console.WriteLine("→ Return 400 Bad Request\n");
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine($"❌ Division error: {ex.Message}");
            Console.WriteLine("→ Return 400 Bad Request\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Unexpected error: {ex.GetType().Name}");
            Console.WriteLine("→ Return 500 Internal Server Error\n");
        }
    }

    // Simulates IDisposable resource
    class FileSimulator : IDisposable
    {
        private readonly string _filename;

        public FileSimulator(string filename)
        {
            _filename = filename;
            Console.WriteLine($"FileSimulator: Opened {filename}");
        }

        public void Dispose()
        {
            Console.WriteLine($"FileSimulator: Disposed (closed {_filename})");
        }
    }

    // Custom exception
    class InvalidProjectException : Exception
    {
        public InvalidProjectException(string message) : base(message) { }
    }

    // Throws custom exception
    static Project CreateProject(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidProjectException("Project name is required");

        return new Project { Name = name, Description = description };
    }

    class Project
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
