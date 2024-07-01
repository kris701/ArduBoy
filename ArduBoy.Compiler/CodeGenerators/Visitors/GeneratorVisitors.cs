using ArduBoy.Compiler.Models.Script;
using System.Text;

namespace ArduBoy.Compiler.CodeGenerators.Visitors
{
    public partial class GeneratorVisitors
    {
        public string Visit(ArduBoyScriptDefinition node)
        {
            var sb = new StringBuilder();
            foreach (var func in node.Funcs)
                sb.AppendLine(Visit(func));
            return sb.ToString();
        }
    }
}
