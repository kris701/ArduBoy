using ArduBoy.Compiler.Models.Script;

namespace ArduBoy.Compiler.Models.Exceptions
{
	public class PostAnalyserException : BaseArduBoyException
	{
		public PostAnalyserException(INode node, string? message) : base(node, "Post Analyser", message)
		{
		}
	}
}
