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

        internal void AppendIfNotEmpty(StringBuilder sb, string value)
        {
            if (value == "")
                return;
            sb.AppendLine(value);
        }
    }
}
