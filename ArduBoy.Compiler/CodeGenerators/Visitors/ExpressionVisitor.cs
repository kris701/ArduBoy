using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.CodeGenerators.Visitors
{
    public partial class GeneratorVisitors
    {
        public string Visit(ValueExpression node)
        {
            if (node.Value != null)
                return node.Value;
            return Visit(node.Reference);
        }

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

            return $"{node.Left} {typeCode} {node.Right}";
        }
    }
}
