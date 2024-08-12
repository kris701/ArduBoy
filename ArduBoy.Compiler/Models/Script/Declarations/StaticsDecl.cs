using ArduBoy.Compiler.Models.Script.Expressions;

namespace ArduBoy.Compiler.Models.Script.Declarations
{
    public class StaticsDecl : BaseNode, IDecl, IContentNode
	{
        public List<INode> Content { get; set; }

        public StaticsDecl(INode parent, List<INode> content) : base(parent)
        {
            Content = content;
        }

        public override string ToString()
        {
            return $"(:statics ({Content.Count} more statics))";
        }
    }
}
