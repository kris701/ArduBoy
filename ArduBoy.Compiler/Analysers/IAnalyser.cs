using ArduBoy.Compiler.Compilers;
using ArduBoy.Compiler.Models.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Analysers
{
	public interface IAnalyser
	{
		public event LogEventHandler? DoLog;
		public ArduBoyScriptDefinition Analyse(ArduBoyScriptDefinition from);
	}
}
