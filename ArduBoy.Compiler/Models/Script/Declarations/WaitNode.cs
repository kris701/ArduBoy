using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Declarations
{
    public class WaitNode : BaseNode, IDecl
    {
        public int WaitTime { get; set; }

        public WaitNode(int waitTime)
        {
            WaitTime = waitTime;
        }

        public override string ToString()
        {
            return $"WAIT {WaitTime}";
        }
    }
}
