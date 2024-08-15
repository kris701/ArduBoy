using ArduBoy.Compiler.Compilers;
using ArduBoy.Compiler.Models.Exceptions;
using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Analysers
{
	public class ArduBoyScriptPostAnalyser : IAnalyser
	{
		public event LogEventHandler? DoLog;

		public ArduBoyScriptDefinition Analyse(ArduBoyScriptDefinition from)
		{
			DoLog?.Invoke("Checking if types are valid...");
			CheckIfAssignmentsAreValid(from);

			return from;
		}

		private void CheckIfAssignmentsAreValid(ArduBoyScriptDefinition from)
		{
			var setExps = from.FindTypes<SetExp>();
			foreach(var set in setExps)
				if (set.Value is ValueExpression val)
					if (!int.TryParse(val.Value, out int tmp))
						throw new PostAnalyserException(set, "Variable of set expression is not a integer!");
		}
	}
}
