namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class CallExp : BaseNode, IExp, INamedNode
    {
        public string Name { get; set; }

        public CallExp(INode parent, string name) : base(parent)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"(:call {Name})";
        }
    }
}
