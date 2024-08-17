using ArduBoy.Compiler.Models.AST;
using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Expressions.Drawing;

namespace ArduBoy.Compiler.Parsers.Visitors
{
	public partial class ParserVisitor
	{
		public IExp? TryVisitDrawingExp(ASTNode node)
		{
			IExp? returnNode;
			if ((returnNode = TryVisitDrawLineDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitDrawFillDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitDrawTriangleDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitDrawFillTriangleDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitDrawTextDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitDrawCircleDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitDrawFillCircleDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitDrawRectangleDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitDrawFillRectangleDeclaration(node)) != null) return returnNode;

			return returnNode;
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
				VisitAsValueOrVariableExp(new ASTNode(split[0])),
				VisitAsValueOrVariableExp(new ASTNode(split[1])),
				VisitAsValueOrVariableExp(new ASTNode(split[2])),
				VisitAsValueOrVariableExp(new ASTNode(split[3])),
				VisitAsValueOrVariableExp(new ASTNode(split[4])));
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
				VisitAsValueOrVariableExp(new ASTNode(RemoveNodeTypeAndEscapeChars(node.Content, ":draw-fill"))));
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
				VisitAsValueOrVariableExp(new ASTNode(split[0])),
				VisitAsValueOrVariableExp(new ASTNode(split[1])),
				VisitAsValueOrVariableExp(new ASTNode(split[2])),
				VisitAsValueOrVariableExp(new ASTNode(split[3])),
				VisitAsValueOrVariableExp(new ASTNode(split[4])),
				VisitAsValueOrVariableExp(new ASTNode(split[5])),
				VisitAsValueOrVariableExp(new ASTNode(split[6])));
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
				VisitAsValueOrVariableExp(new ASTNode(split[0])),
				VisitAsValueOrVariableExp(new ASTNode(split[1])),
				VisitAsValueOrVariableExp(new ASTNode(split[2])),
				VisitAsValueOrVariableExp(new ASTNode(split[3])),
				VisitAsValueOrVariableExp(new ASTNode(split[4])),
				VisitAsValueOrVariableExp(new ASTNode(split[5])),
				VisitAsValueOrVariableExp(new ASTNode(split[6])));
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
			var newNode = new DrawTextExp(
				VisitAsValueOrVariableExp(new ASTNode(split[0])),
				VisitAsValueOrVariableExp(new ASTNode(split[1])),
				VisitAsValueOrVariableExp(new ASTNode(split[2])),
				VisitAsValueOrVariableExp(new ASTNode(string.Join(' ', split[4..]))),
				VisitAsValueOrVariableExp(new ASTNode(split[3])));
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
				VisitAsValueOrVariableExp(new ASTNode(split[0])),
				VisitAsValueOrVariableExp(new ASTNode(split[1])),
				VisitAsValueOrVariableExp(new ASTNode(split[2])),
				VisitAsValueOrVariableExp(new ASTNode(split[3])));
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
				VisitAsValueOrVariableExp(new ASTNode(split[0])),
				VisitAsValueOrVariableExp(new ASTNode(split[1])),
				VisitAsValueOrVariableExp(new ASTNode(split[2])),
				VisitAsValueOrVariableExp(new ASTNode(split[3])));
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
				VisitAsValueOrVariableExp(new ASTNode(split[0])),
				VisitAsValueOrVariableExp(new ASTNode(split[1])),
				VisitAsValueOrVariableExp(new ASTNode(split[2])),
				VisitAsValueOrVariableExp(new ASTNode(split[3])),
				VisitAsValueOrVariableExp(new ASTNode(split[4])));
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
				VisitAsValueOrVariableExp(new ASTNode(split[0])),
				VisitAsValueOrVariableExp(new ASTNode(split[1])),
				VisitAsValueOrVariableExp(new ASTNode(split[2])),
				VisitAsValueOrVariableExp(new ASTNode(split[3])),
				VisitAsValueOrVariableExp(new ASTNode(split[4])));
			return newNode;
		}

	}
}
