using ArduBoy.Compiler.Models.Script.Declarations;
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
        public string Visit(GotoLabelNode node)
        {
            return $"{OperatorCodes.GotoLabelCode}{node.Label}";
        }

        public string Visit(GotoNode node)
        {
            return $"{OperatorCodes.GotoCode}{node.To}";
        }

        public string Visit(WaitNode node)
        {
            return $"{OperatorCodes.WaitCode}{node.WaitTime}";
        }

        public string Visit(StaticDefineNode node)
        {
            return "";
            //return $"DEFINE {node.Name} {node.As}";
        }

        public string Visit(IfNode node)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{OperatorCodes.IfCode} {node.Branch.Count} {Visit(node.Expression)}");
            foreach(var child in node.Branch)
                AppendIfNotEmpty(sb, Visit((dynamic)child));
            return sb.ToString();
        }
    }
}
