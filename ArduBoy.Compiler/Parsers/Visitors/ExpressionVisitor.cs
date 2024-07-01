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

            if ((returnNode = TryVisitVariableExp(node)) != null) return returnNode;
            if ((returnNode = TryVisitComparisonExp(node)) != null) return returnNode;
            if ((returnNode = TryVisitValueExp(node)) != null) return returnNode;

            throw new Exception($"Could not parse content of node: '{node}'");
        }

        public IExp? TryVisitAudioExpression(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":audio") &&
                DoesContentContainNLooseChildren(node, ":audio", 1))
            {
                var newLabel = new AudioExp(int.Parse(RemoveNodeTypeAndEscapeChars(node.Content, ":audio")));
                return newLabel;
            }
            return null;
        }

        public IExp? TryVisitDrawLineDeclaration(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":draw-line") &&
                DoesContentContainNLooseChildren(node, ":draw-line", 5))
            {
                var split = RemoveNodeTypeAndEscapeChars(node.Content, ":draw-line").Split(' ');
                var x1 = VisitExp(new ASTNode(split[0]));
                var y1 = VisitExp(new ASTNode(split[1]));
                var x2 = VisitExp(new ASTNode(split[2]));
                var y2 = VisitExp(new ASTNode(split[3]));
                var color = VisitExp(new ASTNode(split[4]));
                return new DrawLineExp(x1, y1, x2, y2, color);
            }
            return null;
        }

        public IExp? TryVisitIfDeclaration(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":if") &&
                DoesNodeHaveSpecificChildCount(node, ":if", 2))
            {
                var exp = TryVisitComparisonExp(node.Children[0]) as ComparisonExp;
                var branch = new List<IExp>();
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
                var left = VisitExp(new ASTNode(split[0]));
                var comparere = split[1];
                var right = VisitExp(new ASTNode(split[2]));

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

        public IExp? TryVisitVariableExp(ASTNode node)
        {
            if (node.Content.StartsWith('%') && node.Content.EndsWith('%'))
                return new VariableExp(node.Content.Substring(1, node.Content.Length - 2));
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

        public IExp? TryVisitIncludeExp(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":include"))
            {
                return new IncludeExp(RemoveNodeTypeAndEscapeChars(node.Content, ":include"));
            }
            return null;
        }

        public IExp? TryVisitWaitDeclaration(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":wait") &&
                DoesContentContainNLooseChildren(node, ":wait", 1))
            {
                var newLabel = new WaitExp(int.Parse(node.Content.Split(' ')[1]));
                return newLabel;
            }
            return null;
        }

        public IExp? TryVisitSetDeclaration(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":set") &&
                DoesContentContainNLooseChildren(node, ":set", 2))
            {
                var split = RemoveNodeTypeAndEscapeChars(node.Content, ":set").Split(' ');
                var newLabel = new SetExp(
                    split[0],
                    VisitExp(new ASTNode(split[1])));
                return newLabel;
            }
            return null;
        }

        public IExp? TryVisitAddDeclaration(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":add") &&
                DoesContentContainNLooseChildren(node, ":add", 2))
            {
                var split = RemoveNodeTypeAndEscapeChars(node.Content, ":add").Split(' ');
                var newLabel = new AddExp(
                    split[0],
                    VisitExp(new ASTNode(split[1])));
                return newLabel;
            }
            return null;
        }

        public IExp? TryVisitSubDeclaration(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":sub") &&
                DoesContentContainNLooseChildren(node, ":sub", 2))
            {
                var split = RemoveNodeTypeAndEscapeChars(node.Content, ":sub").Split(' ');
                var newLabel = new SubExp(
                    split[0],
                    VisitExp(new ASTNode(split[1])));
                return newLabel;
            }
            return null;
        }

        public IExp? TryVisitMultDeclaration(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":mult") &&
                DoesContentContainNLooseChildren(node, ":mult", 2))
            {
                var split = RemoveNodeTypeAndEscapeChars(node.Content, ":mult").Split(' ');
                var newLabel = new MultExp(
                    split[0],
                    VisitExp(new ASTNode(split[1])));
                return newLabel;
            }
            return null;
        }

        public IExp? TryVisitDivDeclaration(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":div") &&
                DoesContentContainNLooseChildren(node, ":div", 2))
            {
                var split = RemoveNodeTypeAndEscapeChars(node.Content, ":div").Split(' ');
                var newLabel = new DivExp(
                    split[0],
                    VisitExp(new ASTNode(split[1])));
                return newLabel;
            }
            return null;
        }
    }
}
