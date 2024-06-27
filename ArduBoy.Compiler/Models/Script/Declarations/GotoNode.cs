using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ArduBoy.Compiler.Models.Script.Declarations
{
    public class GotoNode : BaseNode, IDecl
    {
        public string To { get; set; }

        public GotoNode(string to)
        {
            To = to;
        }

        public override string ToString()
        {
            return $"GOTO {To}";
        }
    }
}
