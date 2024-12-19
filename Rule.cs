public class Rule
{
    public string Id { get; set; } // Identificador único de la regla
    public string Name { get; set; } // Nombre descriptivo de la regla
    public bool IsActive { get; set; } = true; // Indica si la regla está activa
    public List<Condition> Conditions { get; set; } = new(); // Lista de condiciones asociadas
    public List<Action> Actions { get; set; } = new(); // Lista de acciones asociadas

    public bool Evaluate(object input)
    {
        if (!IsActive)
        {
            return false; // No evaluar reglas inactivas
        }

        return Conditions.All(condition => condition.Evaluate(input));
    }

    public void ExecuteActions()
    {
        foreach (var action in Actions)
        {
            Console.WriteLine($"  - Tipo: {action.Name}");
            action.Execute();
        }
    }
}