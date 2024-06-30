using ArduBoy.Compiler.Models.Script;

namespace ArduBoy.Compiler.Compilers
{
    public delegate void LogEventHandler(string text);
    public interface ICompiler
    {
        public event LogEventHandler? DoLog;
        public ArduBoyScriptDefinition Compile(ArduBoyScriptDefinition from);
    }
}
