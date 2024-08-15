using ArduBoy.Compiler.Compilers;
using ArduBoy.Compiler.Models.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Optimisers
{
	public interface IOptimiser
	{
		public event LogEventHandler? DoLog;
		public ArduBoyScriptDefinition Optimise(ArduBoyScriptDefinition from);
	}
}
