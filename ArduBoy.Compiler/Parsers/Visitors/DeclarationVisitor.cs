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
        public IDecl VisitDecl(ASTNode node, INode? parent)
        {
            IDecl? returnNode;
            if ((returnNode = TryVisitIfDeclaration(node, parent)) != null) return returnNode;
            if ((returnNode = TryVisitDefineDeclaration(node, parent)) != null) return returnNode;
            if ((returnNode = TryVisitGotoLabelDeclaration(node, parent)) != null) return returnNode;
            if ((returnNode = TryVisitGotoDeclaration(node, parent)) != null) return returnNode;

            throw new Exception($"Could not parse content of node: '{node}'");
        }

        public IDecl? TryVisitIfDeclaration(ASTNode node, INode? parent)
        {
            if (IsOfValidNodeType(node.Content, "if"))
            {
                var newIf = new IfNode(parent, null, new List<INode>());
                var inner = node.Content.Substring(node.Content.IndexOf(' ')).Trim();
                newIf.Expression = TryVisitComparisonExp(new ASTNode(inner), newIf) as ComparisonExp;
                foreach (var child in node.Children)
                    newIf.Branch.Add(VisitDecl(child, newIf));

                return newIf;
            }
            return null;
        }

        public IDecl? TryVisitDefineDeclaration(ASTNode node, INode? parent)
        {
            if (IsOfValidNodeType(node.Content, "define") &&
                DoesContentContainNLooseChildren(node, "define", 2))
            {
                var split = node.Content.Split(' ');
                var newDefine = new StaticDefineNode(parent, split[1], split[2]);

                return newDefine;
            }
            return null;
        }

        public IDecl? TryVisitGotoLabelDeclaration(ASTNode node, INode? parent)
        {
            if (node.Content.StartsWith(':'))
            {
                var newLabel = new GotoLabelNode(parent, node.Content.Substring(1));
                return newLabel;
            }
            return null;
        }

        public IDecl? TryVisitGotoDeclaration(ASTNode node, INode? parent)
        {
            if (IsOfValidNodeType(node.Content, "goto") &&
                DoesContentContainNLooseChildren(node, "goto", 1))
            {
                var newLabel = new GotoNode(parent, node.Content.Split(' ')[1]);
                return newLabel;
            }
            return null;
        }
    }
}
