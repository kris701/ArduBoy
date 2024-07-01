namespace ArduBoy.Compiler.Models.Script.Declarations
{
    public class FuncDecl : BaseNode, IDecl, INamedNode
    {
        public string Name { get; set; }
        public List<INode> Content { get; set; }

        public FuncDecl(string name, List<INode> content)
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
