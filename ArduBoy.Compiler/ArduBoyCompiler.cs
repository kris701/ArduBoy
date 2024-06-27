using ArduBoy.Compiler.ASTGenerators;
using ArduBoy.Compiler.CodeGenerators;
using ArduBoy.Compiler.Contextualisers;
using ArduBoy.Compiler.Parsers;
using System.Text;

namespace ArduBoy.Compiler
{
    public class ArduBoyCompiler : ICompiler
    {
        public string Compile(string from)
        {
            var astGenerator = new ArduBoyScriptASTGenerator();
            var parser = new ArduBoyScriptParser();
            var contextualiser = new ArduBoyContextualiser();
            var codeGenerator = new ArduBoyCodeGenerator();

            var ast = astGenerator.Generate(from);
            var parsedDef = parser.Parse(ast);
            var contextualised = contextualiser.Contextualise(parsedDef);
            return codeGenerator.Generate(contextualised);
        }
    }
}
