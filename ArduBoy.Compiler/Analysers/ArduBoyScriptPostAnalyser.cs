using ArduBoy.Compiler.Compilers;
using ArduBoy.Compiler.Models.Exceptions;
using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Expressions;
using ArduBoy.Compiler.Models.Script.Expressions.Arithmetic;
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
			foreach(var set in from.FindTypes<SetExp>())
				if (set.Value is ValueExpression val)
					if (!int.TryParse(val.Value, out int tmp))
						throw new PostAnalyserException(set, "Variable of set expression is not a integer!");
			foreach (var add in from.FindTypes<AddExp>())
				if (add.Value is ValueExpression val)
					if (!int.TryParse(val.Value, out int tmp))
						throw new PostAnalyserException(add, "Variable of add expression is not a integer!");
			foreach (var sub in from.FindTypes<SubExp>())
				if (sub.Value is ValueExpression val)
					if (!int.TryParse(val.Value, out int tmp))
						throw new PostAnalyserException(sub, "Variable of sub expression is not a integer!");
			foreach (var div in from.FindTypes<DivExp>())
				if (div.Value is ValueExpression val)
					if (!int.TryParse(val.Value, out int tmp))
						throw new PostAnalyserException(div, "Variable of div expression is not a integer!");
			foreach (var mult in from.FindTypes<MultExp>())
				if (mult.Value is ValueExpression val)
					if (!int.TryParse(val.Value, out int tmp))
						throw new PostAnalyserException(mult, "Variable of molt expression is not a integer!");
		}
	}
}
