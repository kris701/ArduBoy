using ArduBoy.Compiler.Models.AST;
using ArduBoy.Compiler.Models.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Parsers
{
    public interface IParser
    {
        public ArduBoyScriptDefinition Parse(ASTNode node);
    }
}
