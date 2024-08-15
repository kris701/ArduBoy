using ArduBoy.Compiler.Models.AST;
using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Models.Script.Declarations;
using ArduBoy.Compiler.Parsers.Visitors;

namespace ArduBoy.Compiler.Parsers
{
	public class ArduBoyScriptParser : IParser
	{
		public ArduBoyScriptDefinition Parse(ASTNode node)
		{
			var newDef = new ArduBoyScriptDefinition();
			var visitor = new ParserVisitor();
			foreach (var child in node.Children)
			{
				var visited = visitor.VisitDecl(child);
				switch (visited)
				{
					case NameDecl d: newDef.Name = d; break;
					case StaticsDecl d: newDef.Statics = d; break;
					case IncludesDecl d: newDef.Includes = d; break;
					case ReservedsDecl d: newDef.Reserveds = d; break;
					case FuncDecl d: newDef.Funcs.Add(d); break;
				}
			}
			newDef.SetParents();
			return newDef;
		}
	}
}
