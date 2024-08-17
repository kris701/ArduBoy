using ArduBoy.Compiler.Models.AST;
using ArduBoy.Compiler.Models.Script;

namespace ArduBoy.Compiler.Models.Exceptions
{
	public class ParserException : BaseArduBoyException
	{
		public ParserException(ASTNode node, string? message) : base(node, "Parsing", message)
		{
		}
	}
}
