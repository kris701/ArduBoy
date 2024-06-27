using ArduBoy.Compiler.Models.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.CodeGenerators.Visitors
{
    public partial class GeneratorVisitors
    {
        public string Visit(ArduBoyScriptDefinition node)
        {
            var sb = new StringBuilder();
            foreach (var child in node.Nodes)
                sb.AppendLine(Visit((dynamic)child));
            return sb.ToString();
        }
    }
}
