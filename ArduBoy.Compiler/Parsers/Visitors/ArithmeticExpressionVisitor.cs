using ArduBoy.Compiler.Models.AST;
using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Expressions.Arithmetic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Parsers.Visitors
{
	public partial class ParserVisitor
	{
		public IExp? TryVisitArithmeticExp(ASTNode node)
		{
			IExp? returnNode;
			if ((returnNode = TryVisitAddDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitSubDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitMultDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitDivDeclaration(node)) != null) return returnNode;

			return returnNode;
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
