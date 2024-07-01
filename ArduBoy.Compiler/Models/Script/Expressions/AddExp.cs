namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class AddExp : BaseNode, IExp, INamedNode
    {
        public string Name { get; set; }
        public IExp Value { get; set; }

        public AddExp(string name, IExp value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"(:add {Name} {Value})";
        }
    }
}
