using ArduBoy.Compiler.Models.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Exceptions
{
	public class CompilerException : BaseArduBoyException
	{
		public CompilerException(INode node, string? message) : base(node, "Compilation", message)
		{
		}
	}
}
