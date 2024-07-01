namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class SetExp : BaseNode, IExp, INamedNode
    {
        public string Name { get; set; }
        public IExp Value { get; set; }

        public SetExp(string name, IExp value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"(:set {Name} {Value})";
        }
    }
}
