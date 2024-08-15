using ArduBoy.Compiler.Compilers;
using ArduBoy.Compiler.Helpers;
using ArduBoy.Compiler.Models.Exceptions;
using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Expressions;

namespace ArduBoy.Compiler.Analysers
{
	public class ArduBoyScriptPreAnalyser : IAnalyser
	{
		public event LogEventHandler? DoLog;

		public ArduBoyScriptDefinition Analyse(ArduBoyScriptDefinition from)
		{
			DoLog?.Invoke("Checking if base script is valid...");
			CheckIfScriptAreValid(from);
			DoLog?.Invoke("Check if named nodes are valid...");
			CheckIfNamedNodesAreValid(from);

			return from;
		}

		private void CheckIfScriptAreValid(ArduBoyScriptDefinition from)
		{
			if (!from.Funcs.Any(x => x.Name.ToLower() == "setup"))
				throw new PreAnalyserException(from, "No setup function given!");
			if (!from.Funcs.Any(x => x.Name.ToLower() == "loop"))
				throw new PreAnalyserException(from, "No loop function given!");
			if (from.Includes != null)
				foreach (var node in from.Includes.Content)
					if (node is IncludeExp include && !ResourceHelpers.EmbeddedFileExists($"ArduBoy.Compiler.CoreIncludes.{include.Name}.abs"))
						throw new PreAnalyserException(include, $"Unknown include: {include.Name}");
		}

		private void CheckIfNamedNodesAreValid(ArduBoyScriptDefinition from)
		{
			var namedNodes = from.FindTypes<INamedNode>();
			foreach (var node in namedNodes)
			{
				if (node is not ValueExpression)
				{
					if (node.Name.Trim().StartsWith('%') || node.Name.Trim().EndsWith('%'))
						throw new PreAnalyserException(node, "Name of node seems to be defined as a variable, however it is not valid in this context.");
				}
			}
		}
	}
}
