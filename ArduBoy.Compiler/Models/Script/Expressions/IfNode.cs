namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class IfNode : BaseNode, IExp, IContentNode
	{
        public ComparisonExp Expression { get; set; }
        public List<INode> Content { get; set; }

        public IfNode(INode parent, ComparisonExp expression, List<INode> content) : base(parent)
        {
            Expression = expression;
            Content = content;
        }

        public override string ToString()
        {
            return $"(:if ({Expression}) ({Content.Count} contents))";
        }
    }
}
