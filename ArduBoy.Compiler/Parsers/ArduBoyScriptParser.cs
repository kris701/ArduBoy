using ArduBoy.Compiler.Models.AST;
using ArduBoy.Compiler.Models.Script;
using ArduBoy.Compiler.Parsers.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Parsers
{
    public class ArduBoyScriptParser : IParser
    {
        public ArduBoyScriptDefinition Parse(ASTNode node)
        {
            var newDef = new ArduBoyScriptDefinition();
            var visitor = new ParserVisitor();
            foreach (var child in node.Children)
                newDef.Nodes.Add(visitor.VisitDecl(child, null));
            return newDef;
        }
    }
}
