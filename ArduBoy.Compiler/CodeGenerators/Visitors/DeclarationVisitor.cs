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

            sb.AppendLine($"{OperatorCodes.GetByteCode(":")} {node.Name}");
            foreach (var child in node.Content)
                sb.AppendLine(Visit((dynamic)child));
            sb.AppendLine(" ");

            return sb.ToString();
        }
    }
}
