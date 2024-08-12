namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class IncludeExp : BaseNode, IExp
    {
        public string Name { get; set; }

        public IncludeExp(INode parent, string name) : base(parent)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"(:include {Name})";
        }
    }
}
