using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script
{
    public interface INode
    {
        public INode Parent { get; set; }
    }
}
