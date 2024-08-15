using ArduBoy.Compiler.Models.Script.Declarations;

namespace ArduBoy.Compiler.Models.Script
{
	public class ArduBoyScriptDefinition : BaseNode
	{
		public NameDecl? Name { get; set; }
		public StaticsDecl? Statics { get; set; }
		public IncludesDecl? Includes { get; set; }
		public ReservedsDecl? Reserveds { get; set; }
		public List<FuncDecl> Funcs { get; set; }

		public ArduBoyScriptDefinition()
		{
			Funcs = new List<FuncDecl>();
		}
	}
}
