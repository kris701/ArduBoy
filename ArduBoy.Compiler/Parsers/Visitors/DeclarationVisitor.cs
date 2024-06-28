using ArduBoy.Compiler.Models.AST;
using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Declarations;
using ArduBoy.Compiler.Models.Script.Expressions;

namespace ArduBoy.Compiler.Parsers.Visitors
{
    public partial class ParserVisitor
    {
        public IDecl VisitDecl(ASTNode node)
        {
            IDecl? returnNode;
            if ((returnNode = TryVisitStaticsDeclaration(node)) != null) return returnNode;
            if ((returnNode = TryVisitIncludesDeclaration(node)) != null) return returnNode;
            if ((returnNode = TryVisitNameDeclaration(node)) != null) return returnNode;
            if ((returnNode = TryVisitFuncDeclaration(node)) != null) return returnNode;
            
            throw new Exception($"Could not parse content of node: '{node}'");
        }

        public IDecl? TryVisitStaticsDeclaration(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":statics") &&
                DoesNodeHaveSpecificChildCount(node, ":statics", 1))
            {
                var split = node.Content.Split(' ');
                var statics = new List<StaticsExp>();
                foreach (var child in node.Children[0].Children)
                    statics.Add(TryVisitStaticsExp(child) as StaticsExp);
                var newDefine = new StaticsDecl(statics);

                return newDefine;
            }
            return null;
        }

        public IDecl? TryVisitIncludesDeclaration(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":includes") &&
                DoesNodeHaveSpecificChildCount(node, ":includes", 1))
            {
                var split = node.Content.Split(' ');
                var includes = new List<IncludeExp>();
                foreach (var child in node.Children[0].Children)
                    includes.Add(TryVisitIncludeExp(child) as IncludeExp);
                var newDefine = new IncludesDecl(includes);

                return newDefine;
            }
            return null;
        }

        public IDecl? TryVisitNameDeclaration(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":name"))
            {
                var newLabel = new NameDecl(RemoveNodeTypeAndEscapeChars(node.Content, ":name"));
                return newLabel;
            }
            return null;
        }

        public IDecl? TryVisitFuncDeclaration(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":func") &&
                DoesContentContainNLooseChildren(node, ":func", 1))
            {
                var exp = new List<INode>();
                foreach (var child in node.Children[0].Children)
                    if (child.Content != "")
                        exp.Add(VisitExp(child));
                var newLabel = new FuncDecl(
                    RemoveNodeTypeAndEscapeChars(node.Content, ":func"),
                    exp);
                return newLabel;
            }
            return null;
        }

    }
}
