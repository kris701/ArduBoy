namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class DivExp : BaseNode, IExp, INamedNode
    {
        public string Name { get; set; }
        public IExp Value { get; set; }

        public DivExp(string name, IExp value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"(:div {Name} {Value})";
        }
    }
}
