namespace ArduBoy.Compiler.Models.Script.Declarations
{
    public class FuncDecl : BaseNode, IDecl, INamedNode, IContentNode
	{
        public string Name { get; set; }
        public List<INode> Content { get; set; }

        public FuncDecl(INode parent, string name, List<INode> content) : base(parent)
        {
            Name = name;
            Content = content;
        }

        public override string ToString()
        {
            return $"(:func {Name} ({Content.Count} contents))";
        }
    }
}
