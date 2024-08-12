using ArduBoy.Compiler.Models.Script.Expressions;

namespace ArduBoy.Compiler.Models.Script.Declarations
{
    public class IncludesDecl : BaseNode, IDecl, IContentNode
	{
        public List<INode> Content { get; set; }

        public IncludesDecl(INode parent, List<INode> content) : base(parent)
        {
            Content = content;
        }

        public override string ToString()
        {
            return $"(:includes ({Content.Count} more includes))";
        }
    }
}
