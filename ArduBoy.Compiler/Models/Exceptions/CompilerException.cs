using ArduBoy.Compiler.Models.Script;

namespace ArduBoy.Compiler.Models.Exceptions
{
	public class CompilerException : BaseArduBoyException
	{
		public CompilerException(INode node, string? message) : base(node, "Compilation", message)
		{
		}
	}
}
