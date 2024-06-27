using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script
{
    public class ArduBoyScriptDefinition
    {
        public List<INode> Nodes { get; set; }
        public ArduBoyScriptDefinition() 
        {
            Nodes = new List<INode>();        
        }
    }
}
