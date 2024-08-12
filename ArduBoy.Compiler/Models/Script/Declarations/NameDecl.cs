namespace ArduBoy.Compiler.Models.Script.Declarations
{
    public class NameDecl : BaseNode, IDecl
    {
        public string Name { get; set; }

        public NameDecl(INode parent, string name) : base(parent)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"(:name {Name})";
        }
    }
}
