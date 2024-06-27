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
        public string As { get; set; }

        public StaticDefineNode(INode parent, string name, string @as) : base(parent)
        {
            Name = name;
            As = @as;
        }
    }
}
