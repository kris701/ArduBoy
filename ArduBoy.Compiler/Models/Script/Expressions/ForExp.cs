using ArduBoy.Compiler.Models.Script.Expressions.Arithmetic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Expressions
{
	public class ForExp : BaseNode, IExp, IContentNode
	{
		public SetExp Initialisation { get; set; }
		public ComparisonExp Condition { get; set; }
		public BaseArithmeticExp Updation { get; set; }
		public List<INode> Content { get; set; }

		public ForExp(INode parent, SetExp initialisation, ComparisonExp condition, BaseArithmeticExp updation, List<INode> content) : base(parent)
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
