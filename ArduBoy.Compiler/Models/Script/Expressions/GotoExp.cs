namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class GotoExp : BaseNode, IExp
    {
        public string Name { get; set; }

        public GotoExp(INode parent, string target) : base(parent)
        {
            Name = target;
        }

        public override string ToString() => $"_goto {Name}";
    }
}
