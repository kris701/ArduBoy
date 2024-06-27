using ArduBoy.Compiler.Models.AST;
using ArduBoy.Compiler.Models.Script;
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
        public IExp VisitExp(ASTNode node)
        {
            IExp? returnNode;
            if ((returnNode = TryVisitComparisonExp(node)) != null) return returnNode;
            if ((returnNode = TryVisitValueExp(node)) != null) return returnNode;

            throw new Exception($"Could not parse content of node: '{node}'");
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
            {
                return new ValueExpression(node.Content);
            }
            return null;
        }
    }
}
