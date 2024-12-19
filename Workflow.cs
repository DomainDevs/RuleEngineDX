public class Workflow
{
    public string Name { get; set; } // Nombre del flujo
    public List<Rule> Rules { get; set; } = new(); // Lista de reglas asociadas al flujo

    public List<Rule> Evaluate(object input)
    {
        return Rules
            .Where(rule => rule.IsActive && rule.Evaluate(input))
            .ToList();
    }

    public void Execute(object input)
    {
        var matchedRules = Evaluate(input);
        foreach (var rule in matchedRules)
        {
            Console.WriteLine($"Regla '{rule.Name}'");
            rule.ExecuteActions();
        }
    }
}