// Action.cs
public class Action
{
    public string Name { get; set; }

    public void Execute()
    {
        Console.WriteLine($"Executing action: {Name}");
    }
}
