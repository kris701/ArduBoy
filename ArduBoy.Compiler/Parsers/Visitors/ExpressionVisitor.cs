using ArduBoy.Compiler.Models.AST;
using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Expressions;

namespace ArduBoy.Compiler.Parsers.Visitors
{
    public partial class ParserVisitor
    {
        public IExp VisitExp(ASTNode node)
        {
            IExp? returnNode;
            if ((returnNode = TryVisitIfDeclaration(node)) != null) return returnNode;
            if ((returnNode = TryVisitComparisonExp(node)) != null) return returnNode;
            if ((returnNode = TryVisitStaticsExp(node)) != null) return returnNode;
            if ((returnNode = TryVisitCallExp(node)) != null) return returnNode;
            if ((returnNode = TryVisitValueExp(node)) != null) return returnNode;

            throw new Exception($"Could not parse content of node: '{node}'");
        }

        public IExp? TryVisitIfDeclaration(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":if") &&
                DoesNodeHaveSpecificChildCount(node, ":if", 2))
            {
                var exp = TryVisitComparisonExp(node.Children[0]) as ComparisonExp;
                var branch = new List<INode>();
                foreach (var child in node.Children[1].Children)
                    branch.Add(VisitExp(child));

                return new IfNode(exp, branch);
            }
            return null;
        }

        public IExp? TryVisitComparisonExp(ASTNode node)
        {
            if (DoesContentContainNLooseChildren(node, "", 3))
            {
                var split = node.Content.Split(' ');
                var left = TryVisitValueExp(new ASTNode(split[0])) as ValueExpression;
                var comparere = split[1];
                var right = TryVisitValueExp(new ASTNode(split[2])) as ValueExpression;

                switch (comparere)
                {
                    case "==":
                    case "<":
                    case ">":
                    case "!=":
                        break;
                    default: throw new Exception($"Invalid comparison method: '{comparere}'");
                }

                return new ComparisonExp(left, right, comparere);
            }
            return null;
        }

        public IExp? TryVisitValueExp(ASTNode node)
        {
            if (node.Content != "")
                return new ValueExpression(node.Content);
            return null;
        }

        public IExp? TryVisitCallExp(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":call"))
                return new CallExp(RemoveNodeTypeAndEscapeChars(node.Content, ":call"));
            return null;
        }

        public IExp? TryVisitStaticsExp(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":static"))
            {
                var split = RemoveNodeTypeAndEscapeChars(node.Content, ":static").Split(' ');
                return new StaticsExp(split[0], new ValueExpression(split[1]));
            }
            return null;
        }
    }
}
