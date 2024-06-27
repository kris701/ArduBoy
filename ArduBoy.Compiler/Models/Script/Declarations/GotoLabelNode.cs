using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Declarations
{
    public class GotoLabelNode : BaseNode, IDecl
    {
        public string Label { get; set; }

        public GotoLabelNode(string label)
        {
            Label = label;
        }

        public override string ToString()
        {
            return $":{Label}";
        }
    }
}
