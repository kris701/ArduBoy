using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Declarations
{
    public class GotoNode : BaseNode, IDecl
    {
        public string To { get; set; }

        public GotoNode(INode parent, string to) : base(parent)
        {
            To = to;
        }
    }
}
