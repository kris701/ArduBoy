using ArduBoy.Compiler.CodeGenerators.Visitors;
using ArduBoy.Compiler.Models.Script;

namespace ArduBoy.Compiler.CodeGenerators
{
    public class ArduBoyCodeGenerator : ICodeGenerator
    {
        public string Generate(ArduBoyScriptDefinition script)
        {
            var visitor = new GeneratorVisitors();
            var retStr = visitor.Visit((dynamic)script);
            while (retStr.Contains($"{Environment.NewLine}{Environment.NewLine}"))
                retStr = retStr.Replace($"{Environment.NewLine}{Environment.NewLine}", Environment.NewLine);
            return retStr;
        }
    }
}
