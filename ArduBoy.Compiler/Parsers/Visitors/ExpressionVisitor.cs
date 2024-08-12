using ArduBoy.Compiler.Models.AST;
using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Expressions;
using ArduBoy.Compiler.Models.Script.Expressions.Arithmetic;
using ArduBoy.Compiler.Models.Script.Expressions.Drawing;

namespace ArduBoy.Compiler.Parsers.Visitors
{
    public partial class ParserVisitor
    {
        public IExp VisitExp(INode parent, ASTNode node)
        {
            IExp? returnNode;
            if ((returnNode = TryVisitIfDeclaration(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitForDeclaration(parent, node)) != null) return returnNode;
			if ((returnNode = TryVisitStaticsExp(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitIncludeExp(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitCallExp(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitWaitDeclaration(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitSetDeclaration(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitAddDeclaration(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitSubDeclaration(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitMultDeclaration(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitDivDeclaration(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitAudioExpression(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitDrawLineDeclaration(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitDrawFillDeclaration(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitDrawTriangleDeclaration(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitDrawFillTriangleDeclaration(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitDrawTextDeclaration(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitDrawCircleDeclaration(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitDrawFillCircleDeclaration(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitDrawRectangleDeclaration(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitDrawFillRectangleDeclaration(parent, node)) != null) return returnNode;

            if ((returnNode = TryVisitVariableExp(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitComparisonExp(parent, node)) != null) return returnNode;
            if ((returnNode = TryVisitValueExp(parent, node)) != null) return returnNode;

            throw new Exception($"Could not parse content of node: '{node}'");
        }

        public IExp? TryVisitAudioExpression(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":audio") &&
                DoesContentContainNLooseChildren(node, ":audio", 1))
            {
                var newLabel = new AudioExp(parent, null);
                newLabel.Value = VisitExp(newLabel, new ASTNode(RemoveNodeTypeAndEscapeChars(node.Content, ":audio")));
				return newLabel;
            }
            return null;
        }

        public IExp? TryVisitDrawLineDeclaration(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":draw-line") &&
                DoesContentContainNLooseChildren(node, ":draw-line", 5))
            {
                var newNode = new DrawLineExp(parent, null, null, null, null, null);
				var split = RemoveNodeTypeAndEscapeChars(node.Content, ":draw-line").Split(' ');
				newNode.X1 = VisitExp(newNode, new ASTNode(split[0]));
				newNode.Y1 = VisitExp(newNode, new ASTNode(split[1]));
				newNode.X2 = VisitExp(newNode, new ASTNode(split[2]));
				newNode.Y2 = VisitExp(newNode, new ASTNode(split[3]));
				newNode.Color = VisitExp(newNode, new ASTNode(split[4]));
                return newNode;
            }
            return null;
        }


        public IExp? TryVisitDrawFillDeclaration(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":draw-fill") &&
                DoesContentContainNLooseChildren(node, ":draw-fill", 1))
            {
                var newNode = new DrawFillExp(parent, null);
                newNode.Color = VisitExp(newNode, new ASTNode(RemoveNodeTypeAndEscapeChars(node.Content, ":draw-fill")));
				return newNode;
            }
            return null;
        }

        public IExp? TryVisitDrawTriangleDeclaration(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":draw-triangle") &&
                DoesContentContainNLooseChildren(node, ":draw-triangle", 7))
            {
                var newNode = new DrawTriangleExp(parent, null, null, null, null, null, null, null);
				var split = RemoveNodeTypeAndEscapeChars(node.Content, ":draw-triangle").Split(' ');
				newNode.X1 = VisitExp(newNode, new ASTNode(split[0]));
				newNode.Y1 = VisitExp(newNode, new ASTNode(split[1]));
				newNode.X2 = VisitExp(newNode, new ASTNode(split[2]));
				newNode.Y2 = VisitExp(newNode, new ASTNode(split[3]));
				newNode.X3 = VisitExp(newNode, new ASTNode(split[4]));
				newNode.Y3 = VisitExp(newNode, new ASTNode(split[5]));
				newNode.Color = VisitExp(newNode, new ASTNode(split[6]));
                return newNode;
            }
            return null;
        }

        public IExp? TryVisitDrawFillTriangleDeclaration(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":draw-fill-triangle") &&
                DoesContentContainNLooseChildren(node, ":draw-fill-triangle", 7))
            {
                var newNode = new DrawFillTriangleExp(parent, null, null, null, null, null, null, null);
				var split = RemoveNodeTypeAndEscapeChars(node.Content, ":draw-fill-triangle").Split(' ');
				newNode.X1 = VisitExp(newNode, new ASTNode(split[0]));
				newNode.Y1 = VisitExp(newNode, new ASTNode(split[1]));
				newNode.X2 = VisitExp(newNode, new ASTNode(split[2]));
				newNode.Y2 = VisitExp(newNode, new ASTNode(split[3]));
				newNode.X3 = VisitExp(newNode, new ASTNode(split[4]));
				newNode.Y3 = VisitExp(newNode, new ASTNode(split[5]));
				newNode.Color = VisitExp(newNode, new ASTNode(split[6]));
                return newNode;
            }
            return null;
        }

        public IExp? TryVisitDrawTextDeclaration(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":draw-text"))
            {
                var newNode = new DrawTextExp(parent, null, null, null, null, null);
				var split = RemoveNodeTypeAndEscapeChars(node.Content, ":draw-text").Split(' ');
				newNode.X = VisitExp(newNode, new ASTNode(split[0]));
				newNode.Y = VisitExp(newNode, new ASTNode(split[1]));
				newNode.Size = VisitExp(newNode, new ASTNode(split[2]));
				newNode.Color = VisitExp(newNode, new ASTNode(split[3]));
                var textNode = new ASTNode(string.Join(' ', split[4..]));
				IExp? returnNode;
                returnNode = TryVisitVariableExp(newNode, textNode);
                if (returnNode == null)
                    returnNode = TryVisitValueExp(newNode, textNode);
                if (returnNode == null)
                    throw new Exception("Could not parse the text nodes text!");
                newNode.Text = returnNode;
				return newNode;
            }
            return null;
        }

        public IExp? TryVisitDrawCircleDeclaration(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":draw-circle") &&
                DoesContentContainNLooseChildren(node, ":draw-circle", 4))
            {
                var newNode = new DrawCircleExp(parent, null, null, null, null);
				var split = RemoveNodeTypeAndEscapeChars(node.Content, ":draw-circle").Split(' ');
				newNode.X = VisitExp(newNode, new ASTNode(split[0]));
				newNode.Y = VisitExp(newNode, new ASTNode(split[1]));
				newNode.Radius = VisitExp(newNode, new ASTNode(split[2]));
				newNode.Color = VisitExp(newNode, new ASTNode(split[3]));
                return newNode;
            }
            return null;
        }

        public IExp? TryVisitDrawFillCircleDeclaration(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":draw-fill-circle") &&
                DoesContentContainNLooseChildren(node, ":draw-fill-circle", 4))
            {
                var newNode = new DrawFillCircleExp(parent, null, null, null, null);
				var split = RemoveNodeTypeAndEscapeChars(node.Content, ":draw-fill-circle").Split(' ');
				newNode.X = VisitExp(newNode, new ASTNode(split[0]));
				newNode.Y = VisitExp(newNode, new ASTNode(split[1]));
				newNode.Radius = VisitExp(newNode, new ASTNode(split[2]));
				newNode.Color = VisitExp(newNode, new ASTNode(split[3]));
                return newNode;
            }
            return null;
        }

        public IExp? TryVisitDrawRectangleDeclaration(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":draw-rect") &&
                DoesContentContainNLooseChildren(node, ":draw-rect", 5))
            {
                var newNode = new DrawRectangleExp(parent, null, null, null, null, null);
				var split = RemoveNodeTypeAndEscapeChars(node.Content, ":draw-rect").Split(' ');
				newNode.X = VisitExp(newNode, new ASTNode(split[0]));
				newNode.Y = VisitExp(newNode, new ASTNode(split[1]));
				newNode.Width = VisitExp(newNode, new ASTNode(split[2]));
				newNode.Height = VisitExp(newNode, new ASTNode(split[3]));
				newNode.Color = VisitExp(newNode, new ASTNode(split[4]));
                return newNode;
			}
            return null;
        }

        public IExp? TryVisitDrawFillRectangleDeclaration(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":draw-fill-rect") &&
                DoesContentContainNLooseChildren(node, ":draw-fill-rect", 5))
            {
                var newNode = new DrawFillRectangleExp(parent, null, null, null, null, null);
				var split = RemoveNodeTypeAndEscapeChars(node.Content, ":draw-fill-rect").Split(' ');
				newNode.X = VisitExp(newNode, new ASTNode(split[0]));
				newNode.Y = VisitExp(newNode, new ASTNode(split[1]));
				newNode.Width = VisitExp(newNode, new ASTNode(split[2]));
				newNode.Height = VisitExp(newNode, new ASTNode(split[3]));
				newNode.Color = VisitExp(newNode, new ASTNode(split[4]));
                return newNode;
            }
            return null;
        }

        public IExp? TryVisitIfDeclaration(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":if") &&
                DoesNodeHaveSpecificChildCount(node, ":if", 2))
            {
                var newNode = new IfNode(parent, null, new List<INode>());
                newNode.Expression = TryVisitComparisonExp(newNode, node.Children[0]) as ComparisonExp;
                foreach (var child in GetEmptyNode(node, 1))
					newNode.Content.Add(VisitExp(newNode, child));
                return newNode;
            }
            return null;
        }

		public IExp? TryVisitForDeclaration(INode parent, ASTNode node)
		{
			if (IsOfValidNodeType(node.Content, ":for") &&
				DoesNodeHaveSpecificChildCount(node, ":for", 4))
			{
                var newNode = new ForExp(parent, null, null, null, new List<INode>());
				newNode.Initialisation = TryVisitSetDeclaration(newNode, node.Children[0]) as SetExp;
				newNode.Condition = TryVisitComparisonExp(newNode, node.Children[1]) as ComparisonExp;
				newNode.Updation = VisitExp(newNode, node.Children[2]) as BaseArithmeticExp;
				foreach (var child in GetEmptyNode(node, 3))
					newNode.Content.Add(VisitExp(newNode, child));

				return newNode;
			}
			return null;
		}

		public IExp? TryVisitComparisonExp(INode parent, ASTNode node)
        {
            if (DoesContentContainNLooseChildren(node, "", 3))
            {
                var newNode = new ComparisonExp(parent, null, null, null);
				var split = node.Content.Split(' ');
				newNode.Left = VisitExp(newNode, new ASTNode(split[0]));
                var op = split[1];
				newNode.Right = VisitExp(newNode, new ASTNode(split[2]));

                switch (op)
                {
                    case "==":
                    case "<":
                    case ">":
                    case "!=":
                        break;
                    default: throw new Exception($"Invalid comparison method: '{op}'");
                }

                newNode.Operator = op;

				return newNode;
            }
            return null;
        }

        public IExp? TryVisitValueExp(INode parent, ASTNode node)
        {
            if (node.Content != "")
                return new ValueExpression(parent, node.Content);
            return null;
        }

        public IExp? TryVisitVariableExp(INode parent, ASTNode node)
        {
            if (node.Content.StartsWith('%') && node.Content.EndsWith('%'))
                return new VariableExp(parent, node.Content.Substring(1, node.Content.Length - 2), false);
            return null;
        }

        public IExp? TryVisitCallExp(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":call"))
                return new CallExp(parent, RemoveNodeTypeAndEscapeChars(node.Content, ":call"));
            return null;
        }

        public IExp? TryVisitStaticsExp(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":static"))
            {
                var split = RemoveNodeTypeAndEscapeChars(node.Content, ":static").Split(' ');
                var newNode = new StaticsExp(parent, split[0], null);
                newNode.Value = new ValueExpression(newNode, string.Join(' ', split[1..]));
				return newNode;
            }
            return null;
        }

        public IExp? TryVisitIncludeExp(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":include"))
                return new IncludeExp(parent, RemoveNodeTypeAndEscapeChars(node.Content, ":include"));
            return null;
        }

        public IExp? TryVisitWaitDeclaration(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":wait") &&
                DoesContentContainNLooseChildren(node, ":wait", 1))
            {
                var newNode = new WaitExp(parent, null);
                newNode.WaitTime = VisitExp(newNode, new ASTNode(RemoveNodeTypeAndEscapeChars(node.Content, ":wait")));
				return newNode;
            }
            return null;
        }

        public IExp? TryVisitSetDeclaration(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":set") &&
                DoesContentContainNLooseChildren(node, ":set", 2))
            {
                var split = RemoveNodeTypeAndEscapeChars(node.Content, ":set").Split(' ');
                var newNode = new SetExp(parent, split[0], null);
                newNode.Value = VisitExp(newNode, new ASTNode(split[1]));
				return newNode;
            }
            return null;
        }

        public IExp? TryVisitAddDeclaration(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":add") &&
                DoesContentContainNLooseChildren(node, ":add", 2))
            {
                var split = RemoveNodeTypeAndEscapeChars(node.Content, ":add").Split(' ');
                var newNode = new AddExp(parent, split[0], null);
                newNode.Value = VisitExp(newNode, new ASTNode(split[1]));
				return newNode;
            }
            return null;
        }

        public IExp? TryVisitSubDeclaration(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":sub") &&
                DoesContentContainNLooseChildren(node, ":sub", 2))
            {
                var split = RemoveNodeTypeAndEscapeChars(node.Content, ":sub").Split(' ');
                var newNode = new SubExp(parent, split[0], null);
                newNode.Value = VisitExp(newNode, new ASTNode(split[1]));
				return newNode;
            }
            return null;
        }

        public IExp? TryVisitMultDeclaration(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":mult") &&
                DoesContentContainNLooseChildren(node, ":mult", 2))
            {
                var split = RemoveNodeTypeAndEscapeChars(node.Content, ":mult").Split(' ');
                var newNode = new MultExp(parent, split[0], null);
                newNode.Value = VisitExp(newNode, new ASTNode(split[1]));
				return newNode;
            }
            return null;
        }

        public IExp? TryVisitDivDeclaration(INode parent, ASTNode node)
        {
            if (IsOfValidNodeType(node.Content, ":div") &&
                DoesContentContainNLooseChildren(node, ":div", 2))
            {
                var split = RemoveNodeTypeAndEscapeChars(node.Content, ":div").Split(' ');
                var newNode = new DivExp(parent, split[0], null);
                newNode.Value = VisitExp(newNode, new ASTNode(split[1]));
				return newNode;
            }
            return null;
        }
    }
}
