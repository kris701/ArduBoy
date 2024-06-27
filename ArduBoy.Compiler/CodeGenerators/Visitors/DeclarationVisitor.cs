using ArduBoy.Compiler.Models.Script.Declarations;
using ArduBoy.Compiler.Models.Script.Expressions;
using System.Text;

namespace ArduBoy.Compiler.CodeGenerators.Visitors
{
    public partial class GeneratorVisitors
    {
        public string Visit(FuncDecl node)
        {
            var sb = new StringBuilder();

            sb.AppendLine($":{node.Name}");
            foreach (var child in node.Content)
                sb.AppendLine(Visit((dynamic)child));
            sb.AppendLine(" ");

            return sb.ToString();
        }

        public string Visit(WaitNode node)
        {
            return $":wait {node.WaitTime}";
        }

        public string Visit(IfNode node)
        {
            var sb = new StringBuilder();
            sb.AppendLine($":if {node.Content.Count} {Visit(node.Expression)}");
            foreach (var child in node.Content)
                AppendIfNotEmpty(sb, Visit((dynamic)child));
            return sb.ToString();
        }
    }
}
