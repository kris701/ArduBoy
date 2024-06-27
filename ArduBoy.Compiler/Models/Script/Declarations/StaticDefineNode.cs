using ArduBoy.Compiler.Models.Script.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Declarations
{
    public class StaticDefineNode : BaseNode, IDecl
    {
        public string Name { get; set; }
        public ValueExpression As { get; set; }

        public StaticDefineNode(string name, ValueExpression @as)
        {
            Name = name;
            As = @as;
        }

        public override string ToString()
        {
            return $"DEFINE {Name} {As}";
        }
    }
}
