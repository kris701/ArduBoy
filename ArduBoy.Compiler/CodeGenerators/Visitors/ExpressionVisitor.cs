using ArduBoy.Compiler.Models.Script.Expressions;

namespace ArduBoy.Compiler.CodeGenerators.Visitors
{
    public partial class GeneratorVisitors
    {
        public string Visit(ValueExpression node) => node.Value;

        public string Visit(ComparisonExp node)
        {
            byte typeCode;
            switch (node.Type)
            {
                case "==": typeCode = OperatorCodes.EQCode; break;
                case "<": typeCode = OperatorCodes.LTCode; break;
                case ">": typeCode = OperatorCodes.GTCode; break;
                case "!=": typeCode = OperatorCodes.NEQCode; break;
                default: throw new Exception("Invalid comparison!");
            }

            return $"{node.Left} {node.Type} {node.Right}";
        }

        public string Visit(CallExp node)
        {
            return $":call {node.Name}";
        }
    }
}
