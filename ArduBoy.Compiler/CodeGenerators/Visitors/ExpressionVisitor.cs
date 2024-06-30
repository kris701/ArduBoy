using ArduBoy.Compiler.Models.Script.Expressions;
using System.Text;

namespace ArduBoy.Compiler.CodeGenerators.Visitors
{
    public partial class GeneratorVisitors
    {
        public string Visit(ValueExpression node) => node.Value;

        public string Visit(ComparisonExp node) => $"{node.Left} {OperatorCodes.GetByteCode(node.Type)} {node.Right}";

        public string Visit(CallExp node)
        {
            return $"{OperatorCodes.GetByteCode(":call")} {node.Name}";
        }

        public string Visit(IfNode node)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{OperatorCodes.GetByteCode(":if")} {node.Content.Count} {Visit(node.Expression)}");
            foreach (var child in node.Content)
                AppendIfNotEmpty(sb, Visit((dynamic)child));
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

        public string Visit(AudioExp node)
        {
            return $"{OperatorCodes.GetByteCode(":audio")} {node.Value}";
        }

        public string Visit(VariableExp node)
        {
            return $"%{node.Name}%";
        }

        public string Visit(DrawLineExp node)
        {
            return $"{OperatorCodes.GetByteCode(":draw-line")} {node.X1} {node.Y1} {node.X2} {node.Y2} {node.Color}";
        }
    }
}
