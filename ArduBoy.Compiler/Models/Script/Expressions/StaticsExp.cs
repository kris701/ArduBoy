namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class StaticsExp : BaseNode, IExp
    {
        public string Name { get; set; }
        public ValueExpression Value { get; set; }

        public StaticsExp(string name, ValueExpression value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"({Name} {Value})";
        }
    }
}
