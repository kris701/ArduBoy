using ArduBoy.Compiler.Compilers;
using ArduBoy.Compiler.Models.Script;

namespace ArduBoy.Compiler.Optimisers
{
	public interface IOptimiser
	{
		public event LogEventHandler? DoLog;
		public ArduBoyScriptDefinition Optimise(ArduBoyScriptDefinition from);
	}
}
