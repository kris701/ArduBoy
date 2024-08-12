using ArduBoy.Compiler.Models.AST;
using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Declarations;
using ArduBoy.Compiler.Models.Script.Expressions;

namespace ArduBoy.Compiler.Parsers.Visitors
{
    public partial class ParserVisitor
    {
        public IDecl VisitDecl(INode parent, ASTNode node)
        {
            IDecl? returnNode;
            if ((returnNode = TryVisitStaticsDeclaration(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitIncludesDeclaration(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitNameDeclaration(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitFuncDeclaration(parent, node)) != null) return returnNode;

            throw new Exception($"Could not parse content of node: '{node}'");
        }

        public IDecl? TryVisitStaticsDeclaration(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":statics") &&
                DoesNodeHaveSpecificChildCount(node, ":statics", 1))
            {
                var split = node.Content.Split(' ');
                var newNode = new StaticsDecl(parent, new List<INode>());
                foreach (var child in GetEmptyNode(node))
					newNode.Content.Add(TryVisitStaticsExp(newNode, child) as StaticsExp);
                return newNode;
            }
            return null;
        }

        public IDecl? TryVisitIncludesDeclaration(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":includes") &&
                DoesNodeHaveSpecificChildCount(node, ":includes", 1))
            {
                var split = node.Content.Split(' ');
                var newNode = new IncludesDecl(parent, new List<INode>());
                foreach (var child in GetEmptyNode(node))
					newNode.Content.Add(TryVisitIncludeExp(newNode, child) as IncludeExp);
                return newNode;
            }
            return null;
        }

        public IDecl? TryVisitNameDeclaration(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":name"))
                return new NameDecl(parent, RemoveNodeTypeAndEscapeChars(node.Content, ":name"));
            return null;
        }

        public IDecl? TryVisitFuncDeclaration(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":func") &&
                DoesContentContainNLooseChildren(node, ":func", 1))
            {
                var newNode = new FuncDecl(parent, RemoveNodeTypeAndEscapeChars(node.Content, ":func"), new List<INode>());
                foreach (var child in GetEmptyNode(node))
                    if (child.Content != "")
						newNode.Content.Add(VisitExp(newNode, child));
                return newNode;
            }
            return null;
        }
    }
}
