using DynamicExpresso;

public class Condition
{

    public object Left { get; set; }
    public string Operator { get; set; }
    public object Right { get; set; }
    public string LogicalOperator { get; set; }  // Operador lógico, como "AND", "OR"
    public List<Condition> SubConditions { get; set; }  // Subcondiciones para evaluaciones más complejas
    public string Expression { get; set; }

    public bool Evaluate(object input)
    {


        var interpreter = new Interpreter();
        try
        {
            return interpreter.Eval<bool>(Expression, new DynamicExpresso.Parameter("input", input.GetType(), input));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error evaluating condition: {ex.Message}");
            return false;
        }
    }
}