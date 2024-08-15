namespace ArduBoy.Compiler.Models.Script.Expressions
{
	public class IfExp : BaseNode, IExp, IContentNode
	{
		public ComparisonExp Expression { get; set; }
		public List<INode> Content { get; set; }

		public IfExp(ComparisonExp expression, List<INode> content)
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
