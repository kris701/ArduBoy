using ArduBoy.Compiler.Models.AST;
using ArduBoy.Compiler.Models.Script;

namespace ArduBoy.Compiler.Parsers
{
    public interface IParser
    {
        public ArduBoyScriptDefinition Parse(ASTNode node);
    }
}
