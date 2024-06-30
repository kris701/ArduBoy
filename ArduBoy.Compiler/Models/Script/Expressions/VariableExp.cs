using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduBoy.Compiler.Models.Script.Expressions
{
    public class VariableExp : BaseNode, IExp
    {
        public string Name { get; set; }

        public VariableExp(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"%{Name}%";
        }
    }
}
