namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class StaticsExp : BaseNode, IExp
    {
        public string Name { get; set; }
        public IExp Value { get; set; }

        public StaticsExp(INode parent, string name, IExp value) : base(parent)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"(:static {Name} {Value})";
        }
    }
}
