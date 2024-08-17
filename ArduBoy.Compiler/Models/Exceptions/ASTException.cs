using ArduBoy.Compiler.Models.AST;

namespace ArduBoy.Compiler.Models.Exceptions
{
	public class ASTException : BaseArduBoyException
	{
		public ASTException(ASTNode node, string? message) : base(node, "AST", message)
		{
		}
	}
}
