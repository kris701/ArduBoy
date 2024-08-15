using ArduBoy.Compiler.Compilers;
using ArduBoy.Compiler.Models.Script;

namespace ArduBoy.Compiler.Analysers
{
	public interface IAnalyser
	{
		public event LogEventHandler? DoLog;
		public ArduBoyScriptDefinition Analyse(ArduBoyScriptDefinition from);
	}
}
