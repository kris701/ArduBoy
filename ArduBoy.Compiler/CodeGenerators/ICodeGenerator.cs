using ArduBoy.Compiler.Models.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.CodeGenerators
{
    public interface ICodeGenerator
    {
        public string Generate(ArduBoyScriptDefinition script);
    }
}
