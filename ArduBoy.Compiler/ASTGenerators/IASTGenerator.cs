using ArduBoy.Compiler.Models.AST;

namespace ArduBoy.Compiler.ASTGenerators
{
    public interface IASTGenerator
    {
        public ASTNode Generate(string text);
    }
}
