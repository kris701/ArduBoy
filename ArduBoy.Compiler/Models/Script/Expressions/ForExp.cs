using ArduBoy.Compiler.Models.Script.Expressions.Arithmetic;

namespace ArduBoy.Compiler.Models.Script.Expressions
{
	public class ForExp : BaseNode, IExp, IContentNode
	{
		public SetExp Initialisation { get; set; }
		public ComparisonExp Condition { get; set; }
		public IArithmeticExp Updation { get; set; }
		public List<INode> Content { get; set; }

		public ForExp(SetExp initialisation, ComparisonExp condition, IArithmeticExp updation, List<INode> content)
		{
			Initialisation = initialisation;
			Updation = updation;
			Condition = condition;
			Content = content;
		}

		public override string ToString()
		{
			return $"(:for ({Initialisation}) ({Updation}) ({Condition}) ({Content.Count} contents))";
		}
	}
}
