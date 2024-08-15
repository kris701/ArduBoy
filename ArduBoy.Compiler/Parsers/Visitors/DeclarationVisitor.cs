using ArduBoy.Compiler.Models.AST;
using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Declarations;

namespace ArduBoy.Compiler.Parsers.Visitors
{
	public partial class ParserVisitor
	{
		public IDecl VisitDecl(ASTNode node)
		{
			IDecl? returnNode;
			if ((returnNode = TryVisitStaticsDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitReservedsDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitIncludesDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitNameDeclaration(node)) != null) return returnNode;
			if ((returnNode = TryVisitFuncDeclaration(node)) != null) return returnNode;

			throw new Exception($"Could not parse content of node: '{node}'");
		}

		public StaticsDecl? TryVisitStaticsDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":statics") &&
				DoesNodeHaveSpecificChildCount(node, ":statics", 1))
				return VisitStaticsDeclaration(node);
			return null;
		}

		public StaticsDecl VisitStaticsDeclaration(ASTNode node)
		{
			var split = node.Content.Split(' ');
			var newNode = new StaticsDecl(new List<INode>());
			foreach (var child in GetEmptyNode(node))
				newNode.Content.Add(VisitStaticsExp(child));
			return newNode;
		}

		public ReservedsDecl? TryVisitReservedsDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":reserveds") &&
				DoesNodeHaveSpecificChildCount(node, ":reserveds", 1))
				return VisitReservedsDeclaration(node);
			return null;
		}

		public ReservedsDecl VisitReservedsDeclaration(ASTNode node)
		{
			var split = node.Content.Split(' ');
			var newNode = new ReservedsDecl(new List<INode>());
			foreach (var child in GetEmptyNode(node))
				newNode.Content.Add(VisitReservedExp(child));
			return newNode;
		}

		public IncludesDecl? TryVisitIncludesDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":includes") &&
				DoesNodeHaveSpecificChildCount(node, ":includes", 1))
				return VisitIncludesDeclaration(node);
			return null;
		}

		public IncludesDecl VisitIncludesDeclaration(ASTNode node)
		{
			var split = node.Content.Split(' ');
			var newNode = new IncludesDecl(new List<INode>());
			foreach (var child in GetEmptyNode(node))
				newNode.Content.Add(VisitIncludeExp(child));
			return newNode;
		}

		public NameDecl? TryVisitNameDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":name"))
				return VisitNameDeclaration(node);
			return null;
		}

		public NameDecl VisitNameDeclaration(ASTNode node) => new NameDecl(RemoveNodeTypeAndEscapeChars(node.Content, ":name"));

		public FuncDecl? TryVisitFuncDeclaration(ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":func") &&
				DoesContentContainNLooseChildren(node, ":func", 1))
				return VisitFuncDeclaration(node);
			return null;
		}

		public FuncDecl VisitFuncDeclaration(ASTNode node)
		{
			var newNode = new FuncDecl(RemoveNodeTypeAndEscapeChars(node.Content, ":func"), new List<INode>());
			foreach (var child in GetEmptyNode(node))
				if (child.Content != "")
					newNode.Content.Add(VisitExp(child));
			return newNode;
		}
	}
}
