using Newtonsoft.Json.Linq;

public class RuleEngine
{

    public List<Workflow> Workflows { get; set; } = new();

    // Diccionario de operadores y sus implementaciones
    private static readonly Dictionary<string, Func<object, object, bool>> Operators = new()
    {
        { ">", (left, right) => Compare(left, right, (x, y) => x > y) },
        { "<", (left, right) => Compare(left, right, (x, y) => x < y) },
        { ">=", (left, right) => Compare(left, right, (x, y) => x >= y) },
        { "<=", (left, right) => Compare(left, right, (x, y) => x <= y) },
        { "==", (left, right) => Equals(left, right) },
        { "!=", (left, right) => !Equals(left, right) },
        { "IN", (left, right) => InOperator(left, right) },
        { "BETWEEN", (left, right) => BetweenOperator(left, right) }
    };

    private static bool Compare(object left, object right, Func<double, double, bool> comparison)
    {
        if (TryConvertToDouble(left, out double leftValue) && TryConvertToDouble(right, out double rightValue))
        {
            return comparison(leftValue, rightValue);
        }

        throw new InvalidOperationException($"No se pudo comparar los valores: {left}, {right}");
    }

    private static bool TryConvertToDouble(object value, out double result)
    {
        result = 0;
        if (value is JValue jValue) value = jValue.Value;
        return double.TryParse(value?.ToString(), out result);
    }

    private static object ExtractSimpleValue(object input, JObject data)
    {
        if (input is JObject jObject && jObject.ContainsKey("var"))
        {
            string pathStr = jObject["var"]?.ToString();
            if (!string.IsNullOrEmpty(pathStr))
            {
                var tokens = pathStr.Split('.');
                JToken current = data;

                foreach (var token in tokens)
                {
                    current = current[token];
                    if (current == null) return null;
                }

                return current is JValue jValue ? jValue.Value : current;
            }
        }

        return input is JValue jVal ? jVal.Value : input;
    }

    private static bool InOperator(object left, object right)
    {
        if (right is JArray array)
        {
            foreach (var item in array)
            {
                if (Equals(left, item.ToObject<object>()))
                {
                    return true;
                }
            }
            return false;
        }

        throw new InvalidOperationException($"Operador IN no soporta este tipo de dato para 'right': {right?.GetType()}");
    }


    private static bool BetweenOperator(object left, object right)
    {
        if (right is JArray range && range.Count == 2)
        {
            var min = Convert.ToDouble(range[0].ToObject<object>());
            var max = Convert.ToDouble(range[1].ToObject<object>());
            var value = Convert.ToDouble(left);
            return value >= min && value <= max;
        }

        throw new InvalidOperationException($"Operador BETWEEN no soporta este tipo de dato para 'right': {right?.GetType()}");
    }

    private void ExecuteActions(List<Action> actions, JObject data)
    {
        // Implementación del método para ejecutar acciones
    }

    public void ExecuteAll(object input)
    {
        foreach (var workflow in Workflows)
        {
            Console.WriteLine($"Evaluating workflow: {workflow.Name}");
            workflow.Execute(input);
        }
    }
}
