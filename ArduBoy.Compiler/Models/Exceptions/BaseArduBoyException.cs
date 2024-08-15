using ArduBoy.Compiler.Models.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Exceptions
{
	public abstract class BaseArduBoyException : Exception
	{
		public INode Node { get; private set; }
		public string Stage { get; private set; }
		public BaseArduBoyException(INode node, string stage, string? message) : base(message)
		{
			Node = node;
			Stage = stage;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.AppendLine();
			sb.AppendLine($"Stage:   {Stage}");
			sb.AppendLine($"Message: {Message}");
			sb.AppendLine($"Node:    [{Node.GetType().Name}] {Node}");
			return sb.ToString();
		}
	}
}
