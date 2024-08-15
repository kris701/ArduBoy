using ArduBoy.Compiler.Compilers;
using ArduBoy.Compiler.Helpers;
using ArduBoy.Compiler.Models.Exceptions;
using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Expressions;
using ArduBoy.Compiler.Models.Script.Expressions.Drawing;

namespace ArduBoy.Compiler.Analysers
{
	public class ArduBoyScriptPostAnalyser : IAnalyser
	{
		public event LogEventHandler? DoLog;

		public ArduBoyScriptDefinition Analyse(ArduBoyScriptDefinition from)
		{
			DoLog?.Invoke("Checking if types are valid...");
			CheckIfAssignmentsAreValid(from);
			DoLog?.Invoke("Check if variable references exists...");
			CheckIfAllVariableNamesExist(from);

			return from;
		}

		private void CheckIfAssignmentsAreValid(ArduBoyScriptDefinition from)
		{
			var valueExps = from.FindTypes<ValueExpression>();
			foreach (var valueExp in valueExps)
			{
				var evalType = valueExp.EvaluatedType();
				if (evalType == ValueExpression.ValueTypes.Unknown)
					throw new PostAnalyserException(valueExp, "Value expression has an unknown type!");
				if (valueExp.Parent == null)
					continue;

				if (evalType == ValueExpression.ValueTypes.String)
				{
					if (valueExp.Parent is StaticsExp)
						continue;
					if (valueExp.Parent is DrawTextExp drawExp && drawExp.Text == valueExp)
						continue;
				}

				if (evalType != ValueExpression.ValueTypes.Integer)
					throw new PostAnalyserException(valueExp, "Value expression must be a integer type!");
			}
		}

		private void CheckIfAllVariableNamesExist(ArduBoyScriptDefinition from)
		{
			var existing = new HashSet<string>();
			existing.AddRange(from.FindTypes<StaticsExp>().Select(x => x.Name).ToHashSet());
			existing.AddRange(from.FindTypes<ReservedExp>().Select(x => x.Name).ToHashSet());
			existing.AddRange(from.FindTypes<SetExp>().Select(x => x.Name).ToHashSet());

			var varExps = from.FindTypes<VariableExp>();
			foreach (var varExp in varExps)
			{
				if (!existing.Contains(varExp.Name))
					throw new PostAnalyserException(varExp, "Variable is never set!");
			}
		}
	}
}
