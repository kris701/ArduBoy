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
			if ((returnNode = TryVisitAddDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitSubDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitMultDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitDivDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitAudioExpression(node)) != null) return returnNode;
			if ((returnNode = TryVisitDrawLineDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitDrawFillDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitDrawTriangleDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitDrawFillTriangleDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitDrawTextDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitDrawCircleDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitDrawFillCircleDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitDrawRectangleDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitDrawFillRectangleDeclaration(node)) != null) return returnNode;

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

		public DrawLineExp? TryVisitDrawLineDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":draw-line") &&
				DoesContentContainNLooseChildren(node, ":draw-line", 5))
				return VisitDrawLineDeclaration(node);
			return null;
		}

		public DrawLineExp VisitDrawLineDeclaration(ASTNode node)
		{
			var split = RemoveNodeTypeAndEscapeChars(node.Content, ":draw-line").Split(' ');
			var newNode = new DrawLineExp(
				VisitExp(new ASTNode(split[0])),
				VisitExp(new ASTNode(split[1])),
				VisitExp(new ASTNode(split[2])),
				VisitExp(new ASTNode(split[3])),
				VisitExp(new ASTNode(split[4])));
			return newNode;
		}

		public DrawFillExp? TryVisitDrawFillDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":draw-fill") &&
				DoesContentContainNLooseChildren(node, ":draw-fill", 1))
				return VisitDrawFillDeclaration(node);
			return null;
		}

		public DrawFillExp VisitDrawFillDeclaration(ASTNode node)
		{
			var newNode = new DrawFillExp(
				VisitExp(new ASTNode(RemoveNodeTypeAndEscapeChars(node.Content, ":draw-fill"))));
			return newNode;
		}

		public DrawTriangleExp? TryVisitDrawTriangleDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":draw-triangle") &&
				DoesContentContainNLooseChildren(node, ":draw-triangle", 7))
				return VisitDrawTriangleDeclaration(node);
			return null;
		}

		public DrawTriangleExp VisitDrawTriangleDeclaration(ASTNode node)
		{
			var split = RemoveNodeTypeAndEscapeChars(node.Content, ":draw-triangle").Split(' ');
			var newNode = new DrawTriangleExp(
				VisitExp(new ASTNode(split[0])),
				VisitExp(new ASTNode(split[1])),
				VisitExp(new ASTNode(split[2])),
				VisitExp(new ASTNode(split[3])),
				VisitExp(new ASTNode(split[4])),
				VisitExp(new ASTNode(split[5])),
				VisitExp(new ASTNode(split[6])));
			return newNode;
		}

		public DrawFillTriangleExp? TryVisitDrawFillTriangleDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":draw-fill-triangle") &&
				DoesContentContainNLooseChildren(node, ":draw-fill-triangle", 7))
				return VisitDrawFillTriangleDeclaration(node);
			return null;
		}

		public DrawFillTriangleExp VisitDrawFillTriangleDeclaration(ASTNode node)
		{
			var split = RemoveNodeTypeAndEscapeChars(node.Content, ":draw-fill-triangle").Split(' ');
			var newNode = new DrawFillTriangleExp(
				VisitExp(new ASTNode(split[0])),
				VisitExp(new ASTNode(split[1])),
				VisitExp(new ASTNode(split[2])),
				VisitExp(new ASTNode(split[3])),
				VisitExp(new ASTNode(split[4])),
				VisitExp(new ASTNode(split[5])),
				VisitExp(new ASTNode(split[6])));
			return newNode;
		}

		public DrawTextExp? TryVisitDrawTextDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":draw-text"))
				return VisitDrawTextDeclaration(node);
			return null;
		}

		public DrawTextExp VisitDrawTextDeclaration(ASTNode node)
		{
			var split = RemoveNodeTypeAndEscapeChars(node.Content, ":draw-text").Split(' ');
			var textNode = new ASTNode(string.Join(' ', split[4..]));
			IExp? returnNode;
			returnNode = TryVisitVariableExp(textNode);
			if (returnNode == null)
				returnNode = TryVisitValueExp(textNode);
			if (returnNode == null)
				throw new Exception("Could not parse the text nodes text!");
			var newNode = new DrawTextExp(
				VisitExp(new ASTNode(split[0])),
				VisitExp(new ASTNode(split[1])),
				VisitExp(new ASTNode(split[2])),
				returnNode,
				VisitExp(new ASTNode(split[3])));
			return newNode;
		}

		public DrawCircleExp? TryVisitDrawCircleDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":draw-circle") &&
				DoesContentContainNLooseChildren(node, ":draw-circle", 4))
				return VisitDrawCircleDeclaration(node);
			return null;
		}

		public DrawCircleExp VisitDrawCircleDeclaration(ASTNode node)
		{
			var split = RemoveNodeTypeAndEscapeChars(node.Content, ":draw-circle").Split(' ');
			var newNode = new DrawCircleExp(
				VisitExp(new ASTNode(split[0])),
				VisitExp(new ASTNode(split[1])),
				VisitExp(new ASTNode(split[2])),
				VisitExp(new ASTNode(split[3])));
			return newNode;
		}

		public DrawFillCircleExp? TryVisitDrawFillCircleDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":draw-fill-circle") &&
				DoesContentContainNLooseChildren(node, ":draw-fill-circle", 4))
				return VisitDrawFillCircleDeclaration(node);
			return null;
		}

		public DrawFillCircleExp VisitDrawFillCircleDeclaration(ASTNode node)
		{
			var split = RemoveNodeTypeAndEscapeChars(node.Content, ":draw-fill-circle").Split(' ');
			var newNode = new DrawFillCircleExp(
				VisitExp(new ASTNode(split[0])),
				VisitExp(new ASTNode(split[1])),
				VisitExp(new ASTNode(split[2])),
				VisitExp(new ASTNode(split[3])));
			return newNode;
		}

		public DrawRectangleExp? TryVisitDrawRectangleDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":draw-rect") &&
				DoesContentContainNLooseChildren(node, ":draw-rect", 5))
				return VisitDrawRectangleDeclaration(node);
			return null;
		}

		public DrawRectangleExp VisitDrawRectangleDeclaration(ASTNode node)
		{
			var split = RemoveNodeTypeAndEscapeChars(node.Content, ":draw-rect").Split(' ');
			var newNode = new DrawRectangleExp(
				VisitExp(new ASTNode(split[0])),
				VisitExp(new ASTNode(split[1])),
				VisitExp(new ASTNode(split[2])),
				VisitExp(new ASTNode(split[3])),
				VisitExp(new ASTNode(split[4])));
			return newNode;
		}

		public DrawFillRectangleExp? TryVisitDrawFillRectangleDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":draw-fill-rect") &&
				DoesContentContainNLooseChildren(node, ":draw-fill-rect", 5))
				return VisitDrawFillRectangleDeclaration(node);
			return null;
		}

		public DrawFillRectangleExp VisitDrawFillRectangleDeclaration(ASTNode node)
		{
			var split = RemoveNodeTypeAndEscapeChars(node.Content, ":draw-fill-rect").Split(' ');
			var newNode = new DrawFillRectangleExp(
				VisitExp(new ASTNode(split[0])),
				VisitExp(new ASTNode(split[1])),
				VisitExp(new ASTNode(split[2])),
				VisitExp(new ASTNode(split[3])),
				VisitExp(new ASTNode(split[4])));
			return newNode;
		}

		public IfNode? TryVisitIfDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":if") &&
				DoesNodeHaveSpecificChildCount(node, ":if", 2))
				return VisitIfDeclaration(node);
			return null;
		}

		public IfNode VisitIfDeclaration(ASTNode node)
		{
			var newNode = new IfNode(
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

		public VariableExp VisitVariableExp(ASTNode node) => new VariableExp(node.Content.Substring(1, node.Content.Length - 2), false);

		public CallExp? TryVisitCallExp(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":call"))
				VisitCallExp(node);
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
			var newNode = new SetExp(
				split[0],
				VisitExp(new ASTNode(split[1])));
			return newNode;
		}

		public AddExp? TryVisitAddDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":add") &&
				DoesContentContainNLooseChildren(node, ":add", 2))
				return VisitAddDeclaration(node);
			return null;
		}

		public AddExp VisitAddDeclaration(ASTNode node)
		{
			var split = RemoveNodeTypeAndEscapeChars(node.Content, ":add").Split(' ');
			var newNode = new AddExp(
				split[0],
				VisitExp(new ASTNode(split[1])));
			return newNode;
		}

		public SubExp? TryVisitSubDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":sub") &&
				DoesContentContainNLooseChildren(node, ":sub", 2))
				return VisitSubDeclaration(node);
			return null;
		}

		public SubExp VisitSubDeclaration(ASTNode node)
		{
			var split = RemoveNodeTypeAndEscapeChars(node.Content, ":sub").Split(' ');
			var newNode = new SubExp(
				split[0],
				VisitExp(new ASTNode(split[1])));
			return newNode;
		}

		public MultExp? TryVisitMultDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":mult") &&
				DoesContentContainNLooseChildren(node, ":mult", 2))
				return VisitMultDeclaration(node);
			return null;
		}

		public MultExp VisitMultDeclaration(ASTNode node)
		{
			var split = RemoveNodeTypeAndEscapeChars(node.Content, ":mult").Split(' ');
			var newNode = new MultExp(
				split[0],
				VisitExp(new ASTNode(split[1])));
			return newNode;
		}

		public DivExp? TryVisitDivDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":div") &&
				DoesContentContainNLooseChildren(node, ":div", 2))
				return VisitDivDeclaration(node);
			return null;
		}

		public DivExp VisitDivDeclaration(ASTNode node)
		{
			var split = RemoveNodeTypeAndEscapeChars(node.Content, ":div").Split(' ');
			var newNode = new DivExp(
				split[0],
				VisitExp(new ASTNode(split[1])));
			return newNode;
		}
	}
}
