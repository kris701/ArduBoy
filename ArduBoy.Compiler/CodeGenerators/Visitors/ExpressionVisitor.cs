using ArduBoy.Compiler.Models.Script.Expressions;
using ArduBoy.Compiler.Models.Script.Expressions.Arithmetic;
using ArduBoy.Compiler.Models.Script.Expressions.Drawing;
using System.Text;

namespace ArduBoy.Compiler.CodeGenerators.Visitors
{
    public partial class GeneratorVisitors
    {
        public string Visit(ValueExpression node) => node.Value;

        public string Visit(ComparisonExp node) => $"{Visit((dynamic)node.Left)} {OperatorCodes.GetByteCode(node.Operator)} {Visit((dynamic)node.Right)}";

        public string Visit(CallExp node)
        {
            return $"{OperatorCodes.GetByteCode(":call")} {node.Name}";
        }

        public string Visit(IfNode node)
        {
            var sb = new StringBuilder();
			sb.AppendLine($"{OperatorCodes.GetByteCode(":if")} {node.Content.Count} {Visit(node.Expression)}");
            foreach (var child in node.Content)
				sb.AppendLine(Visit((dynamic)child));

			return sb.ToString();
        }

		public string Visit(WaitExp node)
        {
            return $"{OperatorCodes.GetByteCode(":wait")} {node.WaitTime}";
        }

        public string Visit(SetExp node)
        {
            return $"{OperatorCodes.GetByteCode(":set")} {node.Name} {node.Value}";
        }

        public string Visit(AddExp node)
        {
            return $"{OperatorCodes.GetByteCode(":add")} {node.Name} {node.Value}";
        }

        public string Visit(SubExp node)
        {
            return $"{OperatorCodes.GetByteCode(":sub")} {node.Name} {node.Value}";
        }

        public string Visit(MultExp node)
        {
            return $"{OperatorCodes.GetByteCode(":mult")} {node.Name} {node.Value}";
        }

        public string Visit(DivExp node)
        {
            return $"{OperatorCodes.GetByteCode(":div")} {node.Name} {node.Value}";
        }

        public string Visit(AudioExp node)
        {
            return $"{OperatorCodes.GetByteCode(":audio")} {node.Value}";
        }

        public string Visit(VariableExp node)
        {
            if (node.IsStatic)
                return node.Name;
            return $"%{node.Name}";
        }

        public string Visit(DrawLineExp node)
        {
            return $"{OperatorCodes.GetByteCode(":draw-line")} {Visit((dynamic)node.X1)} {Visit((dynamic)node.Y1)} {Visit((dynamic)node.X2)} {Visit((dynamic)node.Y2)} {Visit((dynamic)node.Color)}";
        }

        public string Visit(DrawCircleExp node)
        {
            return $"{OperatorCodes.GetByteCode(":draw-circle")} {Visit((dynamic)node.X)} {Visit((dynamic)node.Y)} {Visit((dynamic)node.Radius)} {Visit((dynamic)node.Color)}";
        }

        public string Visit(DrawFillCircleExp node)
        {
            return $"{OperatorCodes.GetByteCode(":draw-fill-circle")} {Visit((dynamic)node.X)} {Visit((dynamic)node.Y)} {Visit((dynamic)node.Radius)} {Visit((dynamic)node.Color)}";
        }

        public string Visit(DrawTriangleExp node)
        {
            return $"{OperatorCodes.GetByteCode(":draw-triangle")} {Visit((dynamic)node.X1)} {Visit((dynamic)node.Y1)} {Visit((dynamic)node.X2)} {Visit((dynamic)node.Y2)} {Visit((dynamic)node.X3)} {Visit((dynamic)node.Y3)} {Visit((dynamic)node.Color)}";
        }

        public string Visit(DrawFillTriangleExp node)
        {
            return $"{OperatorCodes.GetByteCode(":draw-fill-triangle")} {Visit((dynamic)node.X1)} {Visit((dynamic)node.Y1)} {Visit((dynamic)node.X2)} {Visit((dynamic)node.Y2)} {Visit((dynamic)node.X3)} {Visit((dynamic)node.Y3)} {Visit((dynamic)node.Color)}";
        }

        public string Visit(DrawRectangleExp node)
        {
            return $"{OperatorCodes.GetByteCode(":draw-rect")} {Visit((dynamic)node.X)} {Visit((dynamic)node.Y)} {Visit((dynamic)node.Width)} {Visit((dynamic)node.Height)} {Visit((dynamic)node.Color)}";
        }

        public string Visit(DrawFillRectangleExp node)
        {
            return $"{OperatorCodes.GetByteCode(":draw-fill-rect")} {Visit((dynamic)node.X)} {Visit((dynamic)node.Y)} {Visit((dynamic)node.Width)} {Visit((dynamic)node.Height)} {Visit((dynamic)node.Color)}";
        }

        public string Visit(DrawTextExp node)
        {
            return $"{OperatorCodes.GetByteCode(":draw-text")} {Visit((dynamic)node.X)} {Visit((dynamic)node.Y)} {Visit((dynamic)node.Size)} {Visit((dynamic)node.Color)} {Visit((dynamic)node.Text)}";
        }

        public string Visit(DrawFillExp node)
        {
            return $"{OperatorCodes.GetByteCode(":draw-fill")} {Visit((dynamic)node.Color)}";
        }

        public string Visit(GotoExp node)
        {
            return $"{OperatorCodes.GetByteCode(":goto")} {node.Name}";
        }
    }
}
