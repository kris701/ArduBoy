using ArduBoy.Compiler.Compilers;
using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Declarations;
using ArduBoy.Compiler.Models.Script.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Optimisers
{
	public class ArduBoyScriptOptimiser : IOptimiser
	{
		public event LogEventHandler? DoLog;

		public ArduBoyScriptDefinition Optimise(ArduBoyScriptDefinition from)
		{
			DoLog?.Invoke("Removing unreferenced code function blocks...");
			RemoveUnreferencedCodeBlocks(from);
			return from;
		}

		private void RemoveUnreferencedCodeBlocks(ArduBoyScriptDefinition from)
		{
			var calls = from.FindTypes<CallExp>();
			var toRemove = new List<FuncDecl>();
			foreach (var func in from.Funcs)
			{
				if (func.Name.ToUpper() == "SETUP" ||
					func.Name.ToUpper() == "LOOP")
					continue;
				if (!calls.Any(x => x.Name == func.Name))
					toRemove.Add(func);
			}
			foreach (var remove in toRemove)
				from.Funcs.Remove(remove);
		}
	}
}
