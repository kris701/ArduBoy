using ArduBoy.Compiler.CodeGenerators.Visitors;
using ArduBoy.Compiler.Models.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
