namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class SubExp : BaseNode, IExp, INamedNode
    {
        public string Name { get; set; }
        public IExp Value { get; set; }

        public SubExp(string name, IExp value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"(:sub {Name} {Value})";
        }
    }
}
