using System.Text.Json;

class Program
{
    static void Main(string[] args)
    {
        // Cargar reglas desde el archivo JSON
        RuleEngine engine = new RuleEngine
        {
            Workflows = LoadWorkflowsFromJson("rules.json"),
        };

        // Ejemplo de datos de entrada para probar todas las reglas
        var inputs = new[]
        {
            new { Age = 17, IsMember = false, Country = "Canada", Role = "Guest", Salary = 45000 },
            new { Age = 25, IsMember = true, Country = "USA", Role = "Admin", Salary = 75000 },
            new { Age = 30, IsMember = false, Country = "Mexico", Role = "User", Salary = 60000 }
        };

        // Probar cada conjunto de datos de entrada
        foreach (var input in inputs)
        {
            Console.WriteLine("=================================");
            Console.WriteLine($"Testing input: {JsonSerializer.Serialize(input)}");
            engine.ExecuteAll(input);
        }
    }

    static List<Workflow> LoadWorkflowsFromJson(string filePath)
    {
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<Workflow>>(json);
    }
}