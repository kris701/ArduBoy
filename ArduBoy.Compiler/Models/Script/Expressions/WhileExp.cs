using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Expressions
{
	public class WhileExp : BaseNode, IExp, IContentNode
	{
		public ComparisonExp Condition { get; set; }
		public List<INode> Content { get; set; }

		public WhileExp(INode parent, ComparisonExp condition, List<INode> content) : base(parent)
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
