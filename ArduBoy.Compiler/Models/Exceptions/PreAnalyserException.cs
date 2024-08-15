using ArduBoy.Compiler.Models.Script;

namespace ArduBoy.Compiler.Models.Exceptions
{
	public class PreAnalyserException : BaseArduBoyException
	{
		public PreAnalyserException(INode node, string? message) : base(node, "Pre Analyser", message)
		{
		}
	}
}
