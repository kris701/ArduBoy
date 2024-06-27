using ArduBoy.Compiler.Models.Script;

namespace ArduBoy.Compiler.CodeGenerators
{
    public interface ICodeGenerator
    {
        public string Generate(ArduBoyScriptDefinition script);
    }
}
