namespace ArduBoy.Compiler.Models.Script.Expressions
{
	public class WhileExp : BaseNode, IExp, IContentNode
	{
		public ComparisonExp Condition { get; set; }
		public List<INode> Content { get; set; }

		public WhileExp(ComparisonExp condition, List<INode> content)
		{
			Condition = condition;
			Content = content;
		}

		public override string ToString()
		{
			return $"(:while ({Condition}) ({Content.Count} contents))";
		}
	}
}
