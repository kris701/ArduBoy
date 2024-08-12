namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class SetExp : BaseNode, IExp, INamedNode
    {
        public string Name { get; set; }
        public IExp Value { get; set; }

        public SetExp(INode parent, string name, IExp value) : base(parent)
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
