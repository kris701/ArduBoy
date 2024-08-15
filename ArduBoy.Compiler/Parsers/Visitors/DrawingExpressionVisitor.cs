using ArduBoy.Compiler.Models.AST;
using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Expressions.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

	}
}
