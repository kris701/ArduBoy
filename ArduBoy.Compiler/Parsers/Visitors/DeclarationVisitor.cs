using ArduBoy.Compiler.Models.AST;
using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Declarations;
using ArduBoy.Compiler.Models.Script.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Parsers.Visitors
{
    public partial class ParserVisitor
    {
        public IDecl VisitDecl(ASTNode node)
        {
            IDecl? returnNode;
            if ((returnNode = TryVisitIfDeclaration(node)) != null) return returnNode;
            if ((returnNode = TryVisitDefineDeclaration(node)) != null) return returnNode;
            if ((returnNode = TryVisitGotoLabelDeclaration(node)) != null) return returnNode;
            if ((returnNode = TryVisitGotoDeclaration(node)) != null) return returnNode;
            if ((returnNode = TryVisitWaitDeclaration(node)) != null) return returnNode;

            throw new Exception($"Could not parse content of node: '{node}'");
        }

        public IDecl? TryVisitIfDeclaration(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, "if"))
            {
                var inner = node.Content.Substring(node.Content.IndexOf(' ')).Trim();
                var exp = TryVisitComparisonExp(new ASTNode(inner)) as ComparisonExp;
                var branch = new List<INode>();
                foreach (var child in node.Children)
                    branch.Add(VisitDecl(child));

                return new IfNode(exp, branch);
            }
            return null;
        }

        public IDecl? TryVisitDefineDeclaration(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, "define") &&
                DoesContentContainNLooseChildren(node, "define", 2))
            {
                var split = node.Content.Split(' ');
                var newDefine = new StaticDefineNode(split[1], TryVisitValueExp(new ASTNode(split[2])) as ValueExpression);

                return newDefine;
            }
            return null;
        }

        public IDecl? TryVisitGotoLabelDeclaration(ASTNode node)
        {
            if (node.Content.StartsWith(':'))
            {
                var newLabel = new GotoLabelNode(node.Content.Substring(1));
                return newLabel;
            }
            return null;
        }

        public IDecl? TryVisitGotoDeclaration(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, "goto") &&
                DoesContentContainNLooseChildren(node, "goto", 1))
            {
                var newLabel = new GotoNode(node.Content.Split(' ')[1]);
                return newLabel;
            }
            return null;
        }

        public IDecl? TryVisitWaitDeclaration(ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, "wait") &&
                DoesContentContainNLooseChildren(node, "wait", 1))
            {
                var newLabel = new WaitNode(int.Parse(node.Content.Split(' ')[1]));
                return newLabel;
            }
            return null;
        }
    }
}
