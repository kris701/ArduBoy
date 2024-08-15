using ArduBoy.Compiler.Models.AST;
using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Expressions;
using ArduBoy.Compiler.Models.Script.Expressions.Arithmetic;
using ArduBoy.Compiler.Models.Script.Expressions.Drawing;

namespace ArduBoy.Compiler.Parsers.Visitors
{
	public partial class ParserVisitor
	{
		public IExp VisitExp(ASTNode node)
		{
			IExp? returnNode;
			if ((returnNode = TryVisitIfDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitForDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitWhileDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitStaticsExp(node)) != null) return returnNode;
			if ((returnNode = TryVisitIncludeExp(node)) != null) return returnNode;
			if ((returnNode = TryVisitCallExp(node)) != null) return returnNode;
			if ((returnNode = TryVisitWaitDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitSetDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitAudioExpression(node)) != null) return returnNode;
			if ((returnNode = TryVisitDrawingExp(node)) != null) return returnNode;
			if ((returnNode = TryVisitArithmeticExp(node)) != null) return returnNode;

			if ((returnNode = TryVisitVariableExp(node)) != null) return returnNode;
			if ((returnNode = TryVisitComparisonExp(node)) != null) return returnNode;
			if ((returnNode = TryVisitValueExp(node)) != null) return returnNode;

			throw new Exception($"Could not parse content of node: '{node}'");
		}

		public AudioExp? TryVisitAudioExpression(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":audio") &&
				DoesContentContainNLooseChildren(node, ":audio", 1))
				return VisitAudioExpression(node);
			return null;
		}

		public AudioExp VisitAudioExpression(ASTNode node)
		{
			var newLabel = new AudioExp(
				VisitExp(new ASTNode(RemoveNodeTypeAndEscapeChars(node.Content, ":audio"))));
			return newLabel;
		}

		public IfExp? TryVisitIfDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":if") &&
				DoesNodeHaveSpecificChildCount(node, ":if", 2))
				return VisitIfDeclaration(node);
			return null;
		}

		public IfExp VisitIfDeclaration(ASTNode node)
		{
			var newNode = new IfExp(
				VisitComparisonExp(node.Children[0]),
				new List<INode>());
			foreach (var child in GetEmptyNode(node, 1))
				newNode.Content.Add(VisitExp(child));
			return newNode;
		}

		public ForExp? TryVisitForDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":for") &&
				DoesNodeHaveSpecificChildCount(node, ":for", 4))
				return VisitForDeclaration(node);
			return null;
		}

		public ForExp VisitForDeclaration(ASTNode node)
		{
			var newNode = new ForExp(
				VisitSetDeclaration(node.Children[0]),
				VisitComparisonExp(node.Children[1]),
				VisitArithmeticDeclaration(node.Children[2]),
				new List<INode>());
			foreach (var child in GetEmptyNode(node, 3))
				newNode.Content.Add(VisitExp(child));
			return newNode;
		}

		public WhileExp? TryVisitWhileDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":while") &&
				DoesNodeHaveSpecificChildCount(node, ":while", 2))
				return VisitWhileDeclaration(node);
			return null;
		}

		public WhileExp VisitWhileDeclaration(ASTNode node)
		{
			var newNode = new WhileExp(
				VisitComparisonExp(node.Children[0]),
				new List<INode>());
			foreach (var child in GetEmptyNode(node, 1))
				newNode.Content.Add(VisitExp(child));
			return newNode;
		}

		public ComparisonExp? TryVisitComparisonExp(ASTNode node)
		{
			if (DoesContentContainNLooseChildren(node, "", 3))
				return VisitComparisonExp(node);
			return null;
		}

		public ComparisonExp VisitComparisonExp(ASTNode node)
		{
			var split = node.Content.Split(' ');
			var op = split[1];

			switch (op)
			{
				case "==":
				case "<":
				case ">":
				case "!=":
					break;
				default: throw new Exception($"Invalid comparison method: '{op}'");
			}

			var newNode = new ComparisonExp(
				VisitExp(new ASTNode(split[0])),
				VisitExp(new ASTNode(split[2])),
				op);

			return newNode;
		}

		public ValueExpression? TryVisitValueExp(ASTNode node)
		{
			if (node.Content != "")
				return VisitValueExp(node);
			return null;
		}

		public ValueExpression VisitValueExp(ASTNode node) => new ValueExpression(node.Content);

		public VariableExp? TryVisitVariableExp(ASTNode node)
		{
			if (node.Content.StartsWith('%') && node.Content.EndsWith('%'))
				return VisitVariableExp(node);
			return null;
		}

		public VariableExp VisitVariableExp(ASTNode node) => new VariableExp(node.Content.Substring(1, node.Content.Length - 2));

		public CallExp? TryVisitCallExp(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":call"))
				return VisitCallExp(node);
			return null;
		}

		public CallExp VisitCallExp(ASTNode node) => new CallExp(RemoveNodeTypeAndEscapeChars(node.Content, ":call"));

		public StaticsExp? TryVisitStaticsExp(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":static"))
				return VisitStaticsExp(node);
			return null;
		}

		public StaticsExp VisitStaticsExp(ASTNode node)
		{
			var split = RemoveNodeTypeAndEscapeChars(node.Content, ":static").Split(' ');
			var newNode = new StaticsExp(
				split[0],
				new ValueExpression(string.Join(' ', split[1..])));
			return newNode;
		}

		public IncludeExp? TryVisitIncludeExp(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":include"))
				return VisitIncludeExp(node);
			return null;
		}

		public IncludeExp VisitIncludeExp(ASTNode node) => new IncludeExp(RemoveNodeTypeAndEscapeChars(node.Content, ":include"));

		public WaitExp? TryVisitWaitDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":wait") &&
				DoesContentContainNLooseChildren(node, ":wait", 1))
				return VisitWaitDeclaration(node);
			return null;
		}

		public WaitExp VisitWaitDeclaration(ASTNode node)
		{
			var newNode = new WaitExp(
				VisitExp(new ASTNode(RemoveNodeTypeAndEscapeChars(node.Content, ":wait"))));
			return newNode;
		}

		public IArithmeticExp VisitArithmeticDeclaration(ASTNode node)
		{
			IArithmeticExp? returnNode;
			if ((returnNode = TryVisitAddDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitSubDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitMultDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitDivDeclaration(node)) != null) return returnNode;
			throw new Exception("Cannot parse as arithmetic node!");
		}

		public SetExp? TryVisitSetDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":set") &&
				DoesContentContainNLooseChildren(node, ":set", 2))
				return VisitSetDeclaration(node);
			return null;
		}

		public SetExp VisitSetDeclaration(ASTNode node)
		{
			var split = RemoveNodeTypeAndEscapeChars(node.Content, ":set").Split(' ');
			IExp? returnNode;
			returnNode = TryVisitVariableExp(new ASTNode(split[1]));
			if (returnNode == null)
				returnNode = TryVisitValueExp(new ASTNode(split[1]));
			if (returnNode == null)
				throw new Exception("Could not parse the set nodes value!");
			var newNode = new SetExp(
				split[0],
				returnNode);
			return newNode;
		}
	}
}
